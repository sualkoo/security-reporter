using System;
using System.ComponentModel;
using System.Globalization;
using System.IO.Compression;
using System.IO.Pipes;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services.Extractor;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;


namespace webapi.ProjectSearch.Services
{
    public class ProjectDataParser : IProjectDataParser
    {
        private readonly ILogger Logger;

        public ProjectDataParser()
        {
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataParser>();
        }

        public ProjectReportData Extract(Stream zipStream)
        {
            ProjectReportData newProjectReportData  = new ProjectReportData();
            
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read)) {
                if(zipStream != null) {
                    ZipArchiveEntry currentEntry = archive.GetEntry("Static/PentestTeam.tex");
                    PentestTeamExtractor pte = new PentestTeamExtractor(currentEntry);
                    Dictionary<string, ProjectInformationParticipant> pentestTeamDict = pte.ExtractPentestTeam();
                    currentEntry = archive.GetEntry("Config/Document_Information.tex");
                    DocumentInformationExtractor die = new DocumentInformationExtractor(currentEntry);
                    newProjectReportData.DocumentInfo = die.ExtractDocumentInformation();
                    currentEntry = archive.GetEntry("Config/Executive_Summary.tex");
                    ExecutiveSummaryExtractor ese = new ExecutiveSummaryExtractor(currentEntry);
                    newProjectReportData.ExecutiveSummary = ese.ExtractExecutiveSummary();
                    currentEntry = archive.GetEntry("Config/Project_Information.tex");
                    ProjectInformationExtractor pie = new ProjectInformationExtractor(currentEntry, pentestTeamDict);
                    newProjectReportData.ProjectInfo = pie.ExtractProjectInformation();
                    currentEntry = archive.GetEntry("Config/Scope_and_Procedures.tex");
                    ScopeAndProceduresExtractor sape = new ScopeAndProceduresExtractor(currentEntry);
                    newProjectReportData.ScopeAndProcedures = sape.ExtractScopeAndProcedures();
                    currentEntry = archive.GetEntry("Config/Testing_Methodology.tex");
                    TestingMethodologyExtractor tme = new TestingMethodologyExtractor(currentEntry);
                    newProjectReportData.TestingMethodology = tme.ExtractTestingMethodology();
                    newProjectReportData.Findings = ExtractFindings(archive);
                    currentEntry = archive.GetEntry("Config/Findings_Database/Findings_Database.tex");
                    if(currentEntry != null)
                    {
                        using (StreamReader reader = new StreamReader(currentEntry.Open()))
                        {
                            newProjectReportData.FindingsDatabase = reader.ReadToEnd();
                        }
                    }
                    newProjectReportData.uploadDate = DateTime.Now;

                }
                else if(zipStream == null)
                {
                    throw new ArgumentNullException(nameof(zipStream));
                } else if(archive == null)
                {
                    throw new ArgumentNullException(nameof(archive));
                }
            }
            Logger.LogInformation("Successfully extracted data from zip file");
            return newProjectReportData;
        }

        private List<Finding> ExtractFindings(ZipArchive archive)
        {
            FindingsExtractor fe = new FindingsExtractor();
            Dictionary<string, List<ZipArchiveEntry>> findingDictionary = new Dictionary<string, List<ZipArchiveEntry>>(); 
            List<Finding> findingsList = new List<Finding> ();
            if (archive == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                foreach (var fileEntry in archive.Entries)
                {
                    if (fileEntry.FullName.StartsWith("Config/Findings_Database")
                        && !fileEntry.FullName.StartsWith("Config/Findings_Database/DR_Template"))
                    {
                        string[] splitString = fileEntry.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
                        string folderPath = "";
                        if(splitString.Length > 3)
                        {
                            folderPath = splitString[0] + "/" + splitString[1] + "/" + splitString[2];
                        }
                        if(!String.IsNullOrEmpty(folderPath))
                        {
                            if (!findingDictionary.ContainsKey(folderPath))
                            {
                                findingDictionary.Add(folderPath, new List<ZipArchiveEntry> { fileEntry });
                            }
                            else
                            {
                                findingDictionary[folderPath].Add(fileEntry);
                            }
                        }
                    }
                }

                foreach(var list in findingDictionary.Values)
                {
                    Finding newFinding = null;
                    List<ZipArchiveEntry> processedMembers = new List<ZipArchiveEntry> ();
                    
                    for(int count = 0; count < list.Count;)
                    {
                        foreach(var entry in list)
                        {
                            if(count == 0)
                            {
                                if(entry.FullName.EndsWith("main.tex"))
                                {
                                    newFinding = fe.extractFinding(entry);
                                    newFinding.imagesList = new List<FileData>();
                                    processedMembers.Add(entry);
                                    string[] splitString = entry.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
                                    newFinding.FolderName = splitString[2];
                                    count++;
                                }
                            } else
                            {
                                if(!processedMembers.Contains(entry))
                                {
                                    newFinding.imagesList.Add(ProcessImages(entry));
                                    processedMembers.Add(entry);
                                    count++;
                                }
                            }
                        }
                    }
                    findingsList.Add(newFinding);
                    
                }
            }

            return findingsList;
        }
        private FileData ProcessImages(ZipArchiveEntry image)
        {
            string[] splitString = image.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
            string fileName = "";
            if (splitString.Length > 3) fileName = splitString[3];

            byte[] contents;
            using(Stream entryStream = image.Open())
            using(MemoryStream memoryStream = new MemoryStream())
            {
                entryStream.CopyTo(memoryStream);
                contents = memoryStream.ToArray();
            }

            FileData newData = new FileData();
            newData.FileName = fileName;
            newData.Content = contents;

            return newData;
        }
    }
}
