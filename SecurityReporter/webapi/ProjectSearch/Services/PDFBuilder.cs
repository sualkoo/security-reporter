using System.Diagnostics;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public class PdfBuilder : IPdfBuilder
    {
        private ILogger<PdfBuilder> Logger;
        private readonly string imageName = "reportbuilder";

        public PdfBuilder(ILogger<PdfBuilder> logger)
        {
            Logger = logger;
        }

        public async Task<FileContentResult> GeneratePdfFromZip(Stream zipFileStream, Guid projectReportId)
        {
            string containerName = "reportbuilder-instance-" + Guid.NewGuid();
            
            Logger.LogDebug("Creating temporary directory for source files");
            string workingDirectory = Directory.GetCurrentDirectory();
            string pdfDirectory = Path.Combine(workingDirectory, "temp");
            Directory.CreateDirectory(pdfDirectory);

            Logger.LogDebug("Extracting zip source files to temporary directory");
            string tempDirectory = Path.Combine(pdfDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
            ZipArchive zipArchive = new ZipArchive(zipFileStream);
            zipArchive.ExtractToDirectory(tempDirectory);

            try
            {
                Logger.LogDebug($"Checking if '{imageName}' image exists...");
                ProcessStartInfo checkImageExistsInfo = new ProcessStartInfo("docker", $"images {imageName}");
                checkImageExistsInfo.RedirectStandardOutput = true;
                checkImageExistsInfo.UseShellExecute = false;
                using (Process checkImageExistsProcess = Process.Start(checkImageExistsInfo))
                {
                    string output = await checkImageExistsProcess.StandardOutput.ReadToEndAsync();
                    if (!output.Contains(imageName))
                    {
                        throw new CustomException(StatusCodes.Status500InternalServerError,
                            $"Docker image '{imageName}' not found.");
                    }

                    Logger.LogDebug("Image found.");
                }

                Logger.LogDebug($"Checking if '{containerName}' container exists...");
                ProcessStartInfo checkContainerExistsInfo =
                    new ProcessStartInfo("docker", $"ps -a -f name={containerName}");
                checkContainerExistsInfo.RedirectStandardOutput = true;
                checkContainerExistsInfo.UseShellExecute = false;
                using (Process checkContainerExistsProcess = Process.Start(checkContainerExistsInfo))
                {
                    string output = await checkContainerExistsProcess.StandardOutput.ReadToEndAsync();
                    if (output.Contains(imageName))
                    {
                        Logger.LogDebug("Starting docker container - " + containerName);
                        ProcessStartInfo startContainerInfo =
                            new ProcessStartInfo("docker", $"start {containerName}");
                        startContainerInfo.UseShellExecute = false;
                        Process startContainerProcess = Process.Start(startContainerInfo);
                        startContainerProcess.WaitForExit();
                    }
                    else
                    {
                        Logger.LogDebug("Building docker container - " + containerName);
                        ProcessStartInfo buildContainerInfo =
                            new ProcessStartInfo("docker", $"run -itd --name {containerName} {imageName}");
                        buildContainerInfo.UseShellExecute = false;
                        Process buildContainerProcess = Process.Start(buildContainerInfo);
                        buildContainerProcess.WaitForExit();
                    }
                }

                Logger.LogDebug("Copying source files to docker container");
                ProcessStartInfo copyFilesToContainerInfo =
                    new ProcessStartInfo("docker", $"cp {tempDirectory}/. {containerName}:/data");
                copyFilesToContainerInfo.UseShellExecute = false;
                Process copyFilesToContainerProcess = Process.Start(copyFilesToContainerInfo);
                copyFilesToContainerProcess.WaitForExit();

                Logger.LogDebug("Compiling sources...");
                ProcessStartInfo compileSourcesInfo = new ProcessStartInfo("docker",
                    $"exec -i {containerName} sh -c \"printf '\\n' | pdflatex /data/Main -interaction=batchmode\"");
                compileSourcesInfo.UseShellExecute = false;
                Process compileSourcesProcess = Process.Start(compileSourcesInfo);
                compileSourcesProcess.WaitForExit();


                Logger.LogDebug("Getting compiled PDF from docker container...");
                ProcessStartInfo extractPdfInfo =
                    new ProcessStartInfo("docker", $"cp {containerName}:/data/Main.pdf {tempDirectory}");
                extractPdfInfo.UseShellExecute = false;
                Process extractPdfProcess = Process.Start(extractPdfInfo);
                extractPdfProcess.WaitForExit();
                
                if (extractPdfProcess.ExitCode != 0)
                {
                    throw new CustomException(StatusCodes.Status500InternalServerError, "Latex sources cannot be compiled due to errors.");
                }
                

                // Step 8: Read the PDF content
                byte[] pdfBytes = await File.ReadAllBytesAsync(Path.Combine(tempDirectory, "Main.pdf"));

                Logger.LogDebug("Returning PDF");

                return new FileContentResult(pdfBytes, "application/pdf")
                {
                    FileDownloadName = $"{projectReportId}.pdf"
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                throw new CustomException(StatusCodes.Status500InternalServerError, "An error occurred while generating a PDF file.");
            }
            finally
            {
                Logger.LogDebug("Cleaning up");

                Directory.Delete(tempDirectory, true);

                Logger.LogDebug($"Stopping container {containerName}");
                ProcessStartInfo stopContainerInfo = new ProcessStartInfo("docker", $"stop {containerName}");
                stopContainerInfo.UseShellExecute = false;
                Process stopContainerProcess = Process.Start(stopContainerInfo);
                stopContainerProcess.WaitForExit();
                
                Logger.LogDebug($"Removing container {containerName}");
                ProcessStartInfo removeContainerInfo = new ProcessStartInfo("docker", $"rm {containerName}");
                removeContainerInfo.UseShellExecute = false;
                Process removeContainerProcess = Process.Start(removeContainerInfo);
                removeContainerProcess.WaitForExit();
            }
        }
    }
}