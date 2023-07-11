﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XParser
{
    public class ChapterEight
    //cconcl-> sectionTitle, (paragraph|subSectionTitle|figure|list|table){1 or multiple time }, pageRange
    //Paragraphs should implement the subSectionTitle or figure or list or table next to it in a document
    //Hence I'm moving it to the paragraph class

    //cconcl -> sectionTitle, (paragraph){1 or  multiple time}, pageRange
    {
        public string sectionTitle { get; set; }
        public List<Paragraph> paragraph { get; set; }
        public string pageRange { get; set; }

    }
}
