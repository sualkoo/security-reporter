using System.IO.Compression;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract
{
    public class ExecutiveSummaryExtractor
    {
        private readonly ZipArchiveEntry execSumEntry;
        public ExecutiveSummaryExtractor(ZipArchiveEntry execSumEntry)
        {
            this.execSumEntry = execSumEntry;
        }

        public string ExtractExecutiveSummary()
        {
            string line;
            bool readingExecSum = false;
            string resultString = "";

            if (execSumEntry == null)
            {
                throw new ArgumentNullException();
            }

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
