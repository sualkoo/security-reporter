using System.Text.RegularExpressions;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

public class StringNormalizer
{
    public static string Normalize(string content)
    {
        // Remove new lines, white spaces, and tabs
        string normalizedContent = Regex.Replace(content, @"[\r\n\t\s]+", " ");
        return normalizedContent;
    }
}