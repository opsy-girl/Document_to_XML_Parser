using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XParser
{
    public class EyeChunk
    {

        public string DocumentSection { get; private set; }
        public List<string> ParagraphArr { get; private set; }
        public List<string> LineArr { get; private set; }
        public List<string> SubSectionArr { get; private set; }
        public List<string> PageNoArr { get; private set; }
        public string TitleName { get; private set; }
        public string Author { get; private set; }
        public string Supervisor { get; private set; }
        public string Date { get; private set; }
        public string TitleParagraph1 { get; private set; }
        public string TitleParagraph2 { get; private set; }

        public EyeChunk(string documentSection, List<string> paragraphArr, List<string> lineArr, List<string> subSectionArr,
            List<string> pageNoArr, string titleName, string author, string supervisor, string date,
            string titleParagraph1, string titleParagraph2)
        {
            DocumentSection = documentSection;
            ParagraphArr = paragraphArr;
            LineArr = lineArr;
            SubSectionArr = subSectionArr;
            PageNoArr = pageNoArr;
            TitleName = titleName;
            Author = author;
            Supervisor = supervisor;
            Date = date;
            TitleParagraph1 = titleParagraph1;
            TitleParagraph2 = titleParagraph2;
        }
    }
}
