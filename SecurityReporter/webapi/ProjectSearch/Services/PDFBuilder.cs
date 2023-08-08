using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public class PDFBuilder : IPDFBuilder
    {
        private ILogger<PDFBuilder> Logger;
        
        public PDFBuilder(ILogger<PDFBuilder> logger)
        {
            Logger = logger;
        }
        
        public async Task<FileContentResult> GeneratePDFFromZip(FileContentResult zipFile, string outputPDFname)
        {
            Logger.LogDebug("Creating temporary directory for source files");
            string workingDirectory = Directory.GetCurrentDirectory();
            string pdfDirectory = Path.Combine(workingDirectory, "temp");
            Directory.CreateDirectory(pdfDirectory);

            Logger.LogDebug("Extracting zip source files to temporary directory");
            string tempDirectory = Path.Combine(pdfDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
            ZipArchive zipArchive = new ZipArchive(new MemoryStream(zipFile.FileContents));
            zipArchive.ExtractToDirectory(tempDirectory);

            var containerName = "reportbuilder-" + Guid.NewGuid();

            try
            {
                Logger.LogDebug("Checking if reportbuilder image exists...");
                ProcessStartInfo checkImageExistsInfo = new ProcessStartInfo("docker", "images reportbuilder");
                checkImageExistsInfo.RedirectStandardOutput = true;
                checkImageExistsInfo.UseShellExecute = false;
                using (Process checkImageExistsProcess = Process.Start(checkImageExistsInfo))
                {
                    string output = await checkImageExistsProcess.StandardOutput.ReadToEndAsync();
                    if (!output.Contains("reportbuilder"))
                    {
                        throw new CustomException(StatusCodes.Status500InternalServerError, "Docker image 'reportbuilder' not found.");
                    }
                    Logger.LogDebug("Image found.");
                }

                Logger.LogDebug("Building docker container - " + containerName);
                ProcessStartInfo buildContainerInfo =
                    new ProcessStartInfo("docker", $"run -itd --name {containerName} reportbuilder");
                buildContainerInfo.UseShellExecute = false;
                Process buildContainerProcess = Process.Start(buildContainerInfo);
                buildContainerProcess.WaitForExit();

                Logger.LogDebug("Copying source files to docker container");
                ProcessStartInfo copyFilesToContainerInfo =
                    new ProcessStartInfo("docker", $"cp {tempDirectory}/. {containerName}:/data");
                copyFilesToContainerInfo.UseShellExecute = false;
                Process copyFilesToContainerProcess = Process.Start(copyFilesToContainerInfo);
                copyFilesToContainerProcess.WaitForExit();

                Logger.LogDebug("Compiling sources...");
                string shellCommand = "pdflatex /data/Main";
                ProcessStartInfo compileSourcesInfo =
                    new ProcessStartInfo("docker", $"exec -it {containerName} sh -c \"{shellCommand}\"");
                compileSourcesInfo.UseShellExecute = false;
                Process compileSourcesProcess = Process.Start(compileSourcesInfo);
                compileSourcesProcess.WaitForExit();

                Logger.LogDebug("Getting compiled PDF from docker container...");
                ProcessStartInfo extractPdfInfo = new ProcessStartInfo("docker", $"cp {containerName}:/data/Main.pdf {tempDirectory}");
                extractPdfInfo.UseShellExecute = false;
                Process extractPdfProcess = Process.Start(extractPdfInfo);
                extractPdfProcess.WaitForExit();

                // Step 8: Read the PDF content
                byte[] pdfBytes = File.ReadAllBytes(Path.Combine(tempDirectory, "Main.pdf"));

                Logger.LogDebug("Returning PDF");
                return new FileContentResult(pdfBytes, "application/pdf")
                {
                    FileDownloadName = $"{outputPDFname}.pdf"
                };
            }
            finally
            {
                Logger.LogDebug("Cleaning up");
                
                Directory.Delete(tempDirectory, true);
                
                ProcessStartInfo stopContainerInfo = new ProcessStartInfo("docker", $"stop {containerName}");
                stopContainerInfo.UseShellExecute = false;
                Process stopContainerProcess = Process.Start(stopContainerInfo);
                stopContainerProcess.WaitForExit();
                
                ProcessStartInfo deleteContainerInfo = new ProcessStartInfo("docker", $"rm {containerName}");
                deleteContainerInfo.UseShellExecute = false;
                Process deleteContainerProcess = Process.Start(deleteContainerInfo);
                deleteContainerProcess.WaitForExit();
                
                
            }
        }
    }
}