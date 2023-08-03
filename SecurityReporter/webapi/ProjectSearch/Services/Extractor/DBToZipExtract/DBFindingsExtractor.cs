using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBFindingsExtractor
    {
        private Finding finding = null;
        public DBFindingsExtractor(Finding finding)
        {
            this.finding = finding;
        }
        public Tuple<MemoryStream, List<(string FileName, Image image)>> extractFinding()
        {
            string findingContent =
@"%
%
%		Finding: Template
%		Author: the DR
%
%
\renewcommand{\FindingAuthor}{" + this.finding.FindingAuthor + @"}
% DO NOT USE \par, \newline or any other line breaking command in FindingName => report will not build
\renewcommand{\FindingName}{" + this.finding.FindingName + @"}
\renewcommand{\Location}{" + string.Join(", ", this.finding.Location) + @"}
\renewcommand{\Component}{" + this.finding.Component + @"}
\renewcommand{\FoundWith}{" + this.finding.FoundWith + @"}
\renewcommand{\TestMethod}{" + this.finding.TestMethod + @"}
\renewcommand{\CVSS}{" + this.finding.CVSS + @"}
\renewcommand{\CVSSvector}{" + this.finding.CVSSVector + @"}
\renewcommand{\CWE}{" + this.finding.CWE.ToString() + @"}
% Poor-man's combo boxes:
% High, Medium, Low, Info, TBR (To Be Rated)
\renewcommand{\Criticality}{" + this.finding.Criticality.ToString() + @"}
% Easy, Average, Hard, TBR (To Be Rated)
\renewcommand{\Exploitability}{" + this.finding.Exploitability.ToString() + @"}
% Access control, Application Design, Information Disclosure, Outdated Software, Security Configuration
\renewcommand{\Category}{" + this.finding.Category.ToString() + @"}
% Easy, Average, Difficult, TBR (To Be Rated)
\renewcommand{\Detectability}{" + this.finding.Detectability.ToString() + @"}


\ReportFindingHeader{\FindingName}


%-------------------------------------------
%	Details                                |
%-------------------------------------------

\subsection*{Details}
" + this.finding.SubsectionDetails + @"
\subsection*{Impact}
" + this.finding.SubsectionImpact + @"
\subsection*{Repeatability}
" + this.finding.SubsectionRepeatability + @"
\subsection*{Countermeasures}
" + this.finding.SubsectionCountermeasures + @"
\subsection*{References}
" + this.finding.SubsectionReferences + @"
";
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes(findingContent);
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var imageList= CreateFindingImages();

            Tuple<MemoryStream, List<(string FileName, Image image)>> result = 
                new Tuple<MemoryStream, List<(string FileName, Image image)>>(memoryStream, imageList);

            return result;
        }

        private List<(string FileName, Image Image)> CreateFindingImages()
        {
            List<(string FileName, Image Image)> result = new List<(string FileName, Image Image)>();

            foreach (var image in finding.imagesList)
            {
                if (image != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(image.Content))
                    {
                        result.Add((image.FileName, Image.FromStream(memoryStream)));
                    }
                }
            }

            return result;
        }
    }
}
