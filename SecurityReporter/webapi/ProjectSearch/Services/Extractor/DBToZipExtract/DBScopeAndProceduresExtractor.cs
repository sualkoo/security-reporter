using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBScopeAndProceduresExtractor
    {
        private ScopeAndProcedures scopeAndProcedures = null;
        public DBScopeAndProceduresExtractor(ScopeAndProcedures scopeAndProcedures)
        {
            this.scopeAndProcedures = scopeAndProcedures;
        }
        public byte[] extractScopeAndProcedures()
        {
            return null;
        }
    }
}
