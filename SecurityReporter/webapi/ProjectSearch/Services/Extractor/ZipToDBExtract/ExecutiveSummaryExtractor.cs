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
            if (execSumEntry == null)
            {
                throw new ArgumentNullException();
            }

            using (StreamReader reader = new StreamReader(execSumEntry.Open()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
