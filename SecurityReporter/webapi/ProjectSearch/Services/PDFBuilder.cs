using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services;

public class PdfBuilder : IPdfBuilder
{
    private readonly string imageName = "reportbuilder";
    private readonly ILogger<PdfBuilder> logger;

    public PdfBuilder(ILogger<PdfBuilder> logger)
    {
        this.logger = logger;
    }
    
    private void CheckRequiredPrograms()
    {
        // Check if pdflatex is installed
        var pdflatexCheckInfo = new ProcessStartInfo("pdflatex", "--version");
        pdflatexCheckInfo.RedirectStandardOutput = true;
        pdflatexCheckInfo.UseShellExecute = false;

        var pdflatexCheckProcess = Process.Start(pdflatexCheckInfo);
        pdflatexCheckProcess.WaitForExit();

        if (pdflatexCheckProcess.ExitCode != 0)
        {
            throw new CustomException(StatusCodes.Status500InternalServerError,
                "PDFLateX is not installed or could not be found on the system. Make sure you have 'texlive-full' installed on the server.");
        }
    }

    public async Task<FileContentResult> GeneratePdfFromZip(Stream zipFileStream, Guid projectReportId)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return await GeneratePdfWindows(zipFileStream, projectReportId);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            return await GeneratePdfUnix(zipFileStream, projectReportId);
        }
     
        throw new CustomException(StatusCodes.Status500InternalServerError, "This feature is not yet supported on this operating system on serverside.");
    }

    private async Task<FileContentResult> GeneratePdfWindows(Stream zipFileStream, Guid projectReportId)
    {
        throw new CustomException(StatusCodes.Status500InternalServerError, "This feature is not yet supported on this operating system on serverside (Windows is under development). ");
    }
    
    private async Task<FileContentResult> GeneratePdfUnix(Stream zipFileStream, Guid projectReportId)
    {
        CheckRequiredPrograms();
        logger.LogDebug("Extracting LateX source to temporary directory");
        var workingDirectory = Directory.GetCurrentDirectory();
        var latexSourceDir = Path.Combine(workingDirectory, "temp", projectReportId.ToString());
        Directory.CreateDirectory(latexSourceDir);
        var zipArchive = new ZipArchive(zipFileStream);
        zipArchive.ExtractToDirectory(latexSourceDir);

        Console.WriteLine(latexSourceDir + "/Main");

        var generatePdfInfo = new ProcessStartInfo("pdflatex",
            " -halt-on-error -interaction=batchmode Main");
        generatePdfInfo.WorkingDirectory = latexSourceDir;
        generatePdfInfo.UseShellExecute = false;
        generatePdfInfo.RedirectStandardOutput = true;
        generatePdfInfo.RedirectStandardError = true;
        var generatePdfProcess = Process.Start(generatePdfInfo);
        generatePdfProcess.WaitForExit();
        if (generatePdfProcess.ExitCode != 0)
        {
            throw new CustomException(StatusCodes.Status500InternalServerError,
                "Latex sources cannot be compiled to PDF due to errors in source code.");
        }

        var pdfBytes = await File.ReadAllBytesAsync(Path.Combine(latexSourceDir, "Main.pdf"));

        Directory.Delete(latexSourceDir, true);

        logger.LogDebug("Returning PDF");

        return new FileContentResult(pdfBytes, "application/pdf")
        {
            FileDownloadName = $"{projectReportId}.pdf"
        };
    }
    
    // Use this if you want to generate PDFs using report-builder docker image
    public async Task<FileContentResult> GeneratePdfFromZipDocker(Stream zipFileStream, Guid projectReportId)
    {
        var containerName = "reportbuilder-instance-" + Guid.NewGuid();

        logger.LogDebug("Creating temporary directory for source files");
        var workingDirectory = Directory.GetCurrentDirectory();
        var pdfDirectory = Path.Combine(workingDirectory, "temp");
        Directory.CreateDirectory(pdfDirectory);

        logger.LogDebug("Extracting zip source files to temporary directory");
        var tempDirectory = Path.Combine(pdfDirectory, Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDirectory);
        var zipArchive = new ZipArchive(zipFileStream);
        zipArchive.ExtractToDirectory(tempDirectory);

        try
        {
            logger.LogDebug($"Checking if '{imageName}' image exists...");
            var checkImageExistsInfo = new ProcessStartInfo("docker", $"images {imageName}");
            checkImageExistsInfo.RedirectStandardOutput = true;
            checkImageExistsInfo.UseShellExecute = false;
            using (var checkImageExistsProcess = Process.Start(checkImageExistsInfo))
            {
                var output = await checkImageExistsProcess.StandardOutput.ReadToEndAsync();
                if (!output.Contains(imageName))
                    throw new CustomException(StatusCodes.Status500InternalServerError,
                        $"Docker image '{imageName}' not found.");

                logger.LogDebug("Image found.");
            }

            logger.LogDebug($"Checking if '{containerName}' container exists...");
            var checkContainerExistsInfo =
                new ProcessStartInfo("docker", $"ps -a -f name={containerName}");
            checkContainerExistsInfo.RedirectStandardOutput = true;
            checkContainerExistsInfo.UseShellExecute = false;
            using (var checkContainerExistsProcess = Process.Start(checkContainerExistsInfo))
            {
                var output = await checkContainerExistsProcess.StandardOutput.ReadToEndAsync();
                if (output.Contains(imageName))
                {
                    logger.LogDebug("Starting docker container - " + containerName);
                    var startContainerInfo =
                        new ProcessStartInfo("docker", $"start {containerName}");
                    startContainerInfo.UseShellExecute = false;
                    var startContainerProcess = Process.Start(startContainerInfo);
                    startContainerProcess.WaitForExit();
                }
                else
                {
                    logger.LogDebug("Building docker container - " + containerName);
                    var buildContainerInfo =
                        new ProcessStartInfo("docker", $"run -itd --name {containerName} {imageName}");
                    buildContainerInfo.UseShellExecute = false;
                    var buildContainerProcess = Process.Start(buildContainerInfo);
                    buildContainerProcess.WaitForExit();
                }
            }

            logger.LogDebug("Copying source files to docker container");
            var copyFilesToContainerInfo =
                new ProcessStartInfo("docker", $"cp {tempDirectory}/. {containerName}:/data");
            copyFilesToContainerInfo.UseShellExecute = false;
            var copyFilesToContainerProcess = Process.Start(copyFilesToContainerInfo);
            copyFilesToContainerProcess.WaitForExit();

            logger.LogDebug("Compiling sources...");
            var compileSourcesInfo = new ProcessStartInfo("docker",
                $"exec -i {containerName} sh -c \"printf '\\n' | pdflatex /data/Main -interaction=batchmode\"");
            compileSourcesInfo.UseShellExecute = false;
            var compileSourcesProcess = Process.Start(compileSourcesInfo);
            compileSourcesProcess.WaitForExit();


            logger.LogDebug("Getting compiled PDF from docker container...");
            var extractPdfInfo =
                new ProcessStartInfo("docker", $"cp {containerName}:/data/Main.pdf {tempDirectory}");
            extractPdfInfo.UseShellExecute = false;
            var extractPdfProcess = Process.Start(extractPdfInfo);
            extractPdfProcess.WaitForExit();

            if (extractPdfProcess.ExitCode != 0)
                throw new CustomException(StatusCodes.Status500InternalServerError,
                    "Latex sources cannot be compiled due to errors.");


            // Step 8: Read the PDF content
            var pdfBytes = await File.ReadAllBytesAsync(Path.Combine(tempDirectory, "Main.pdf"));

            logger.LogDebug("Returning PDF");

            return new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = $"{projectReportId}.pdf"
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            throw new CustomException(StatusCodes.Status500InternalServerError,
                "An error occurred while generating a PDF file.");
        }
        finally
        {
            logger.LogDebug("Cleaning up");

            Directory.Delete(tempDirectory, true);

            logger.LogDebug($"Stopping container {containerName}");
            var stopContainerInfo = new ProcessStartInfo("docker", $"stop {containerName}");
            stopContainerInfo.UseShellExecute = false;
            var stopContainerProcess = Process.Start(stopContainerInfo);
            stopContainerProcess.WaitForExit();

            logger.LogDebug($"Removing container {containerName}");
            var removeContainerInfo = new ProcessStartInfo("docker", $"rm {containerName}");
            removeContainerInfo.UseShellExecute = false;
            var removeContainerProcess = Process.Start(removeContainerInfo);
            removeContainerProcess.WaitForExit();
        }
    }
}