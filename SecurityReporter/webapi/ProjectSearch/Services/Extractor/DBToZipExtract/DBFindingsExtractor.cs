using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBFindingsExtractor
    {
        public Tuple<MemoryStream, List<(string FileName, byte[] image)>> extractFinding(Finding finding)
        {
            string findingContent =
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
\renewcommand{\CWE}{" + finding.CWE.ToString() + @"}
% Poor-man's combo boxes:
% High, Medium, Low, Info, TBR (To Be Rated)
\renewcommand{\Criticality}{" + finding.Criticality.ToString() + @"}
% Easy, Average, Hard, TBR (To Be Rated)
\renewcommand{\Exploitability}{" + finding.Exploitability.ToString() + @"}
% Access control, Application Design, Information Disclosure, Outdated Software, Security Configuration
\renewcommand{\Category}{" + finding.Category.ToString() + @"}
% Easy, Average, Difficult, TBR (To Be Rated)
\renewcommand{\Detectability}{" + finding.Detectability.ToString() + @"}


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
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes(findingContent);
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var imageList= CreateFindingImages(finding);

            Tuple<MemoryStream, List<(string FileName, byte[] image)>> result = 
                new Tuple<MemoryStream, List<(string FileName, byte[] image)>>(memoryStream, imageList);

            return result;
        }

        private List<(string FileName, byte[] Image)> CreateFindingImages(Finding finding)
        {
            List<(string FileName, byte[] Image)> result = new List<(string FileName, byte[] Image)>();

            foreach (var image in finding.imagesList)
            {
                if (image != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(image.Content))
                    {
                        byte[] imageBytes = memoryStream.ToArray();
                        result.Add((image.FileName, imageBytes));
                    }
                }
            }

            return result;
        }
    }
}
