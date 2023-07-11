using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class ChapterFive
    //cplan-> sectionTitle, (paragraph|subSectionTitle|figure|list|table){1 or multiple time }, pageRange
    //Paragraphs should implement the subSectionTitle or figure or list or table next to it in a document
    //Hence I'm moving it to the paragraph class

    //cplan -> sectionTitle, (paragraph){1 or  multiple time}, pageRange
    {
        public string sectionTitle { get; set; }
        public List<Paragraph> paragraph { get; set; }
        public string pageRange { get; set; }

    }
}
