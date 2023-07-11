using System;
using System.Collections.Generic;
using System.Text;

namespace XParser
{
    public class Paragraph
    {
        // A large chunk of text separable by new line (\n)
        //Paragraphs should implement the subSectionTitle or figure or listOfItems or table next to it in a document
        //Any of these class attributes can be set to null if absent in a paragraph content. 
        //A paragraph is the large chunk of text and any of subSectionTitle
        //above it or any of figures, list of items, tables, that may be next to iy
        public string subSectionTitle { get; set; }
        public string textChunk { get; set; }
        public string figure { get; set; } //maybe just figure name for now
        public string listOfItems { get; set; }
        public string table { get; set; } //maybe just figure name for now
    }
}
