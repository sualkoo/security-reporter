using System.Text;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

public static class DbFindingsExtractor
{
    public static Tuple<MemoryStream, List<(string FileName, byte[] image)>> ExtractFinding(Finding finding)
    {
        var findingContent =
            @"%
%
%		Finding: Template
%		Author: the DR
%
%
\renewcommand{\FindingAuthor}{" + finding.FindingAuthor + @"}
% DO NOT USE \par, \newline or any other line breaking command in FindingName => report will not build
\renewcommand{\FindingName}{" + finding.FindingName + @"}
\renewcommand{\Location}{" + string.Join(", ", finding.Location) + @"}
\renewcommand{\Component}{" + finding.Component + @"}
\renewcommand{\FoundWith}{" + finding.FoundWith + @"}
\renewcommand{\TestMethod}{" + finding.TestMethod + @"}
\renewcommand{\CVSS}{" + finding.CVSS + @"}
\renewcommand{\CVSSvector}{" + finding.CVSSVector + @"}
\renewcommand{\CWE}{" + ((finding.CWE != null) ? finding.CWE : "") + @"}
% Poor-man's combo boxes:
% High, Medium, Low, Info, TBR (To Be Rated)
\renewcommand{\Criticality}{" + finding.Criticality + @"}
% Easy, Average, Hard, TBR (To Be Rated)
\renewcommand{\Exploitability}{" + finding.Exploitability + @"}
% Access control, Application Design, Information Disclosure, Outdated Software, Security Configuration
\renewcommand{\Category}{" + finding.Category + @"}
% Easy, Average, Difficult, TBR (To Be Rated)
\renewcommand{\Detectability}{" + finding.Detectability + @"}


\ReportFindingHeader{\FindingName}


%-------------------------------------------
%	Details                                |
%-------------------------------------------

\subsection*{Details}
" + finding.SubsectionDetails + @"
\subsection*{Impact}
" + finding.SubsectionImpact + @"
\subsection*{Repeatability}
" + finding.SubsectionRepeatability + @"
\subsection*{Countermeasures}
" + finding.SubsectionCountermeasures + @"
\subsection*{References}
" + finding.SubsectionReferences + @"
";
        var memoryStream = new MemoryStream();
        var bytes = Encoding.UTF8.GetBytes(findingContent);
        memoryStream.Write(bytes, 0, bytes.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var imageList = CreateFindingImages(finding);

        var result =
            new Tuple<MemoryStream, List<(string FileName, byte[] image)>>(memoryStream, imageList);

        return result;
    }

    private static List<(string FileName, byte[] Image)> CreateFindingImages(Finding finding)
    {
        var result = new List<(string FileName, byte[] Image)>();

        foreach (var image in finding.GetImages())
            if (image != null)
                using (var memoryStream = new MemoryStream(image.Content))
                {
                    var imageBytes = memoryStream.ToArray();
                    result.Add((image.FileName, imageBytes));
                }

        return result;
    }
}