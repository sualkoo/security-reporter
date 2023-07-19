using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services
{
    public class ExecutiveSummaryExtractor
    {
        private ZipArchiveEntry execSumEntry;
        public ExecutiveSummaryExtractor(ZipArchiveEntry execSumEntry) { 
            this.execSumEntry = execSumEntry;
        }

        public string ExtractExecutiveSummary()
        {
            DocumentInformation newDocumentInfo = new DocumentInformation();
            newDocumentInfo.ReportDocumentHistory = new List<ReportVersionEntry>();
            string line;
            bool readingExecSum = false;
            string resultString = "";

            if (execSumEntry == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                using (StreamReader reader = new StreamReader(execSumEntry.Open()))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line == "%-<ExecSum>")
                            {
                                readingExecSum = false;
                            }

                            if (readingExecSum)
                            {
                                resultString += line;
                            }

                            if (line == "%-<ExecSum>->")
                            {
                                readingExecSum = true;
                            }

                        }

                    }
                }
                return resultString;
            }
        }
    }
}
