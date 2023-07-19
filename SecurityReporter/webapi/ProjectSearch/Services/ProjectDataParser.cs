using System;
using System.ComponentModel;
using System.Globalization;
using System.IO.Compression;
using System.IO.Pipes;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Services
{
    public class ProjectDataParser : IProjectDataParser
    {
        public ProjectReportData Extract(Stream zipStream)
        {
            ProjectReportData newProjectReportData  = new ProjectReportData();
            
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read)) {
                if(zipStream != null) {
                    ZipArchiveEntry currentEntry = archive.GetEntry("Static/PentestTeam.tex");
                    PentestTeamExtractor pte = new PentestTeamExtractor(currentEntry);
                    Dictionary<string, ProjectInformationParticipant> pentestTeamDict = pte.extractPentestTeam();
                    currentEntry = archive.GetEntry("Config/Document_Information.tex");
                    DocumentInformationExtractor die = new DocumentInformationExtractor(currentEntry);
                    newProjectReportData.DocumentInfo = die.ExtractDocumentInformation();
                    currentEntry = archive.GetEntry("Config/Executive_Summary.tex");
                    ExecutiveSummaryExtractor ese = new ExecutiveSummaryExtractor(currentEntry);
                    newProjectReportData.ExecutiveSummary = ese.ExtractExecutiveSummary();
                    currentEntry = archive.GetEntry("Config/Project_Information.tex");
                    ProjectInformationExtractor pie = new ProjectInformationExtractor(currentEntry, pentestTeamDict);
                    newProjectReportData.ProjectInfo = pie.ExtractProjectInformation();
                    currentEntry = archive.GetEntry("Config/Testing_Methodology.tex");
                    TestingMethodologyExtractor tme = new TestingMethodologyExtractor(currentEntry);
                    newProjectReportData.TestingMethodology = tme.ExtractTestingMethodology();
                    //currentEntry = archive.GetEntry("Config/Findings_Database/");
                    newProjectReportData.Findings = extractFindings(archive);

                }
                else if(zipStream == null)
                {
                    throw new ArgumentNullException(nameof(zipStream));
                } else if(archive == null)
                {
                    throw new ArgumentNullException(nameof(archive));
                }
            }
            return newProjectReportData;
        }

        private List<Finding> extractFindings(ZipArchive archive)
        {
            FindingsExtractor fe = new FindingsExtractor();
            List<Finding> findingsList = new List<Finding> ();
            if (archive == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                foreach (var fileEntry in archive.Entries)
                {
                    if (fileEntry.Name.EndsWith("main.tex", StringComparison.OrdinalIgnoreCase)
                        && !fileEntry.FullName.Equals("Config/Findings_Database/DR_Template/main.tex"))
                    {
                        findingsList.Add(fe.extractFinding(fileEntry));
                    }
                }
            }

            return findingsList;
        }
    }
}
