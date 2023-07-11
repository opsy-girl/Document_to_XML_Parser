using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;



namespace XParser
{
    public class NewSlicer
    {
        // string FirstChapter = "";

        private class TextWithFontExtractionStategy : iTextSharp.text.pdf.parser.ITextExtractionStrategy
        {
            //HTML buffer
            private StringBuilder result = new StringBuilder();

            //Store last used properties
            private Vector lastBaseLine;
            private string lastFont;
            private float lastFontSize;
            private string lastFontLocation;
            // private string lastRenderMode;

            //http://api.itextpdf.com/itext/com/itextpdf/text/pdf/parser/TextRenderInfo.html
            private enum TextRenderMode
            {
                FillText = 0,
                StrokeText = 1,
                FillThenStrokeText = 2,
                Invisible = 3,
                FillTextAndAddToPathForClipping = 4,
                StrokeTextAndAddToPathForClipping = 5,
                FillThenStrokeTextAndAddToPathForClipping = 6,
                AddTextToPaddForClipping = 7
            }



            public void RenderText(iTextSharp.text.pdf.parser.TextRenderInfo renderInfo)
            {
                //string curFont = renderInfo.GetFont().PostscriptFontName;
                string curFont = renderInfo.GetFont().PostscriptFontName;
                var k = renderInfo.GetFont();
                string a = Convert.ToString(renderInfo.GetTextRenderMode());
                int b = Convert.ToInt32(TextRenderMode.FillThenStrokeText);


                if ((renderInfo.GetTextRenderMode() == (int)TextRenderMode.FillThenStrokeText))
                {
                    curFont += "-Bold";
                }

                curFont = curFont + "- " + a;

                //This code assumes that if the baseline changes then we're on a newline
                Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
                Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(curBaseline[Vector.I1], curBaseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
                Single curFontSize = rect.Height;
                String curFontLocation = rect.Height + ", W:" + rect.Width + ", T:" + rect.Top + ", B:" + rect.Bottom + ", L:" + rect.Left + ", R:"
                    + rect.Right + ", VB:" + rect.UseVariableBorders + ", R:" + rect.Rotation;


                //See if something has changed, either the baseline, the font or the font size
                if ((this.lastBaseLine == null) || (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]) || (curFontSize != lastFontSize) || (curFont != lastFont))
                {
                    //if we've put down at least one span tag close it
                    if ((this.lastBaseLine != null))
                    {
                        this.result.AppendLine("</span>");
                        //this.result.AppendLine("\\");
                    }
                    //If the baseline has changed then insert a line break
                    if ((this.lastBaseLine != null) && curBaseline[Vector.I2] != lastBaseLine[Vector.I2])
                    {
                        this.result.AppendLine("<br />");
                        //this.result.AppendLine("\\");
                    }
                    //Create an HTML tag with appropriate styles
                    this.result.AppendFormat("<span style=\"font-family:{0};font-size:{1};font-location:{2}\">", curFont, curFontSize, curFontLocation);
                    //this.result.AppendFormat("font:{0};size:{1}", curFont, curFontSize);
                }

                //Append the current text. 
                //When I tried to separate each word by space here, it scattered the whole thing.
                this.result.Append(renderInfo.GetText());

                //Set currently used properties
                this.lastBaseLine = curBaseline;
                this.lastFontSize = curFontSize;
                this.lastFont = curFont;
                this.lastFontLocation = curFontLocation;
            }

            private List<SimpleTextWithRectangle> textWithRectangleList = new List<SimpleTextWithRectangle>();

            public void RenderTextForIndenting(iTextSharp.text.pdf.parser.TextRenderInfo renderInfo)
            {
                if (renderInfo.GetText().Trim().Length == 0)
                    return;
                LineSegment ascent = renderInfo.GetAscentLine();
                LineSegment descent = renderInfo.GetDescentLine();

                float initX = descent.GetStartPoint().Length;
                float initY = descent.GetStartPoint().Length;
                float endX = ascent.GetEndPoint().Length;
                float endY = ascent.GetEndPoint().Length;

                Rectangle rectangle = new Rectangle(initX, initY, endX - initX, endY - initY);

                SimpleTextWithRectangle textWithRectangle = new SimpleTextWithRectangle(rectangle, renderInfo.GetText());
                textWithRectangleList.Add(textWithRectangle);


                //string curFont = renderInfo.GetFont().PostscriptFontName;
                string curFont = renderInfo.GetFont().PostscriptFontName;
                var k = renderInfo.GetFont();
                int a = renderInfo.GetTextRenderMode();
                int b = Convert.ToInt32(TextRenderMode.FillThenStrokeText);

                //Check if faux bold is used
                if ((renderInfo.GetTextRenderMode() == (int)TextRenderMode.FillThenStrokeText))
                {
                    curFont += "-Bold";
                }

                //This code assumes that if the baseline changes then we're on a newline
                Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
                Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(curBaseline[Vector.I1], curBaseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
                Single curFontSize = rect.Height;


                //See if something has changed, either the baseline, the font or the font size
                if ((this.lastBaseLine == null) || (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]) || (curFontSize != lastFontSize) || (curFont != lastFont))
                {
                    //if we've put down at least one span tag close it
                    if ((this.lastBaseLine != null))
                    {
                        this.result.AppendLine("</span>");
                        //this.result.AppendLine("\\");
                    }
                    //If the baseline has changed then insert a line break
                    if ((this.lastBaseLine != null) && curBaseline[Vector.I2] != lastBaseLine[Vector.I2])
                    {
                        this.result.AppendLine("<br />");
                        //this.result.AppendLine("\\");
                    }
                    //Create an HTML tag with appropriate styles
                    this.result.AppendFormat("<span style=\"font-family:{0};font-size:{1};font-location:{2}\">", curFont, curFontSize);
                    //this.result.AppendFormat("font:{0};size:{1}", curFont, curFontSize);
                }

                //Append the current text. 
                //When I tried to separate each word by space here, it scattered the whole thing.
                this.result.Append(renderInfo.GetText());

                //Set currently used properties
                this.lastBaseLine = curBaseline;
                this.lastFontSize = curFontSize;
                this.lastFont = curFont;
            }



            public string GetResultantText()
            {
                //If we wrote anything then we'll always have a missing closing tag so close it here
                if (result.Length > 0)
                {
                    result.Append("</span>");
                }
                return result.ToString();
            }

            //Not needed but must be there for itextextractionstrategy to work
            public void BeginTextBlock() { }
            public void EndTextBlock() { }
            public void RenderImage(ImageRenderInfo renderInfo) { }
        }

        private static string getMidString(string htmlString, string startTag, string endTag)
        {
            if (htmlString.Contains(startTag) && htmlString.Contains(endTag))
            {
                int Start, End;
                Start = htmlString.IndexOf(startTag, 0) + startTag.Length;
                if (endTag == "") { End = htmlString.Length; }
                else { End = htmlString.IndexOf(endTag, Start) - 1; } // I added -1 because of the special charater " that could not be contained in the split string
                return htmlString.Substring(Start, End - Start);
            }

            return "";
        }

        public List<TextLine> SliceDocument(string sourcePdf, Dictionary<int, string> DocSections, out Dictionary<string, int> ChapterPages,
             out int TotalPages)
        {

            ChapterPages = new Dictionary<string, int>();
            //  AllFontSizes = new List<int>();
            TotalPages = 0;
            iTextSharp.text.pdf.RandomAccessFileOrArray raf = null;
            iTextSharp.text.pdf.PdfReader reader = null;
            var sliceItemList = new List<TextLine>();


            List<int> LeftLineSpacing = new List<int>();
            List<int> RightLineSpacing = new List<int>();

            try
            {
                raf = new iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdf);
                reader = new iTextSharp.text.pdf.PdfReader(raf, null);
                TotalPages = reader.NumberOfPages;
                string CurrentChapter = "";

                try
                {
                    //LOOP THROUGH ALL THE PAGES OF THE DOCUMENT TO DETERMINE THE CHAPTERS AND PAGES, AND ALSO SEGMENT CONTENT
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        TextWithFontExtractionStategy S = new TextWithFontExtractionStategy();
                        string F = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, S);
                        string Ftext = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i);

                        if (F != "") //Handles blank pages
                        {
                            Ftext = Ftext.Replace("\0\0\0\0\n", "");
                            Ftext = Ftext.Replace("\0", ""); Ftext = Ftext.Replace(";", "");
                            Ftext = Ftext.Replace("!", ""); //Ftext = Ftext.Replace(":", "");
                            string[] FSplit = F.Split("<br />");
                            string[] FtextSplit = Ftext.Split("\n");

                            // Slice document into Chapters by chapter headers
                            if (i == 1)
                            {
                                ChapterPages.Add("TitleSection", i);
                                CurrentChapter = "TitleSection";
                            }
                            else
                            {

                                for (int m = 0; m <= DocSections.Count - 1; m++)
                                {
                                    string SectionName = DocSections.ElementAt(m).Value;
                                    String contentSectionCheck = "false";

                                    for (int t = 0; t <= FtextSplit.Count() - 1; t++)
                                    {
                                        string currentLine = FtextSplit[t].Trim().Replace(" ", "");
                                        string[] currentLineWords = FtextSplit[t].Trim().Split(" ");
                                        if (t <= 1) //takes care of first or second line to check for chapter heading
                                        {
                                            if (currentLine.ToUpper().Contains(SectionName.ToUpper()) == true &
                                                contentSectionCheck == "false")
                                            // & currentLineWords.Count() < 5 )
                                            //   & !Regex.Match(currentLineWords[currentLineWords.Count()-1], "^[0-9]+$").Success))
                                            {
                                                //modify chapter 1 and one to reflect the same
                                                ChapterPages.Add(SectionName, i);
                                                CurrentChapter = SectionName;
                                                if (currentLine.Replace(" ", "").ToUpper().Contains("CONTENT"))
                                                // that is the whole line contains just "content"
                                                {
                                                    contentSectionCheck = "true";
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            List<float> allFontSizes = new List<float>();
                            //float[] allFontSizes = new float[0];
                            int SliceItem_Page, SliceItem_Line = 0;
                            string SliceItem_Text, SliceItem_LineAlignType = "";
                            float SliceItem_FontSize, SliceItem_FontLeft, SliceItem_FontRight, SliceItem_FontTop, SliceItem_NextfontTop,
                            SliceItem_LineTopSpacing, SliceItem_LineBottomSpacing, SliceItem_FontBottom, PreviousFontBottom = 0;

                            int indexCheck = 0;
                            for (int k = 0; k <= FtextSplit.Count() - 1; k++)
                                if (FtextSplit[k].Trim() != "")
                                {
                                    {
                                        string fontType = getMidString(FSplit[k], "font-family:", ";font-size:");
                                        string fontSizeStr = getMidString(FSplit[k], "font-size:", ";font-location:");
                                        string fontTop = getMidString(FSplit[k], ", T:", ", B:");
                                        string NextfontTop = "0.0";

                                        //Get Next line where line text is not an empty string
                                        int nextNonEmptyLineIndex = 0;
                                        for (int m = k + 1; m < FtextSplit.Count() - k - 1; m++)
                                        {
                                            if (nextNonEmptyLineIndex == 0)
                                            {
                                                if (FtextSplit[m].Trim() != "")
                                                {
                                                    nextNonEmptyLineIndex = m;
                                                }
                                            }
                                        }



                                        if (k < FtextSplit.Count() - 1)
                                        {
                                            NextfontTop = getMidString(FSplit[nextNonEmptyLineIndex], ", T:", ", B:");
                                        }
                                        else if (k == FtextSplit.Count() - 1)
                                        {
                                            NextfontTop = getMidString(FSplit[nextNonEmptyLineIndex], ", B:", ", L:"); //On the very last line
                                        }
                                        string fontBottom = getMidString(FSplit[k], ", B:", ", L:");
                                        string fontLeftStr = getMidString(FSplit[k], ", L:", ", R:");
                                        string fontRightStr = getMidString(FSplit[k], ", R:", ", VB:");

                                        SliceItem_Page = i; //PageNo
                                                            // SliceItem_Line = k + 1; //lineNo
                                                            //SliceItem_Text = FSplit[k]; //lineText
                                        SliceItem_Text = FtextSplit[k]; // Same as linetext but with spacing in the words

                                        if (fontSizeStr == "") { SliceItem_FontSize = 0; } else { SliceItem_FontSize = float.Parse(fontSizeStr); } //Line FontSizes
                                        if (fontLeftStr == "") { SliceItem_FontLeft = 0; } else { SliceItem_FontLeft = float.Parse(fontLeftStr); } //Line FontLeft
                                        if (fontRightStr == "") { SliceItem_FontRight = 0; } else { SliceItem_FontRight = float.Parse(fontRightStr); } //Line FontRight
                                        if (fontTop == "") { SliceItem_FontTop = 0; } else { SliceItem_FontTop = float.Parse(fontTop); } //Line FontTop
                                        if (fontBottom == "") { SliceItem_FontBottom = 0; } else { SliceItem_FontBottom = float.Parse(fontBottom); } //Line FontBottom
                                        if (NextfontTop == "")
                                        { SliceItem_NextfontTop = 0; }
                                        else
                                        {
                                            SliceItem_NextfontTop = float.Parse(NextfontTop);
                                        } //NextLine FontTop

                                        //Create a process to determine the top and bottom line spacing. Unforturnately, I cant determine the bottom 
                                        //spacing in the same loop. I have to use a different loop to read all the lines

                                        if (k == 0 || indexCheck == 0)
                                        {
                                            PreviousFontBottom = SliceItem_FontTop;
                                            SliceItem_LineTopSpacing = 0;
                                            indexCheck = 1;
                                        }
                                        else
                                        {
                                            SliceItem_LineTopSpacing = PreviousFontBottom - SliceItem_FontTop;
                                        } //Line FontBottom

                                        SliceItem_LineBottomSpacing = SliceItem_FontBottom - SliceItem_NextfontTop; //NextLineSpacing - Got bottom spacing in another loop process

                                        PreviousFontBottom = SliceItem_FontBottom;

                                        //Create a process to determine the align type as left, centre or right using the left and right location space on a line
                                        bool leftSpace = false;
                                        bool rightSpace = false;



                                        if (SliceItem_FontLeft >= 130.0) { leftSpace = true; }
                                        if (SliceItem_FontRight >= 130.0) { rightSpace = true; }

                                        if (leftSpace && rightSpace) { SliceItem_LineAlignType = "center"; }
                                        //if (leftSpace && !rightSpace) { SliceItem_LineAlignType = "paragraph"; }
                                        if (!leftSpace && rightSpace) { SliceItem_LineAlignType = "short"; }
                                        if (!leftSpace && !rightSpace) { SliceItem_LineAlignType = "long"; }
                                        if (SliceItem_LineTopSpacing > 10 && SliceItem_LineBottomSpacing <= 10) { SliceItem_LineAlignType = "para-begin"; }
                                        if (SliceItem_LineTopSpacing < 7 && SliceItem_LineBottomSpacing >= 10) { SliceItem_LineAlignType = "para-end"; }


                                        TextLine SI = new TextLine(string.Format("ch{0}-Pg{1}-{2}", "0", SliceItem_Page, SliceItem_Line),
                                            CurrentChapter, SliceItem_Page, SliceItem_Line, SliceItem_Text, fontType,
                                            SliceItem_FontSize, SliceItem_FontLeft, SliceItem_FontRight,
                                            SliceItem_FontTop, SliceItem_FontBottom, SliceItem_LineTopSpacing, SliceItem_LineBottomSpacing,
                                             SliceItem_LineAlignType);

                                        sliceItemList.Add(SI);

                                        LeftLineSpacing.Add(Convert.ToInt32(Math.Round(SliceItem_FontLeft / 10)) * 10);
                                        RightLineSpacing.Add(Convert.ToInt32(Math.Round(SliceItem_FontRight / 10)) * 10);

                                    }

                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string m = ex.Message.ToString();
                }


                #region Manipulate left and right indent spacing to get a right value
                List<int> distinctLeftSpacing = LeftLineSpacing.Distinct().ToList();
                Dictionary<int, int> LeftSpacingList = new Dictionary<int, int>();
                foreach (int LineSpacing in distinctLeftSpacing)
                {
                    int count = LeftLineSpacing.Count(x => x == LineSpacing);
                    LeftSpacingList.Add(LineSpacing, count);
                }

                List<int> distinctRightSpacing = RightLineSpacing.Distinct().ToList();
                Dictionary<int, int> RightSpacingList = new Dictionary<int, int>();
                foreach (int LineSpacing in distinctRightSpacing)
                {
                    int count = RightLineSpacing.Count(x => x == LineSpacing);
                    RightSpacingList.Add(LineSpacing, count);
                }

                // Use OrderBy method and a loop to get the top 4 left and right spacing.
                List<int> LeftList = new List<int>();
                List<int> RightList = new List<int>();
                var aa = LeftSpacingList.OrderByDescending(i => i.Key);
                var bb = RightSpacingList.OrderByDescending(i => i.Key);

                for (int m = 0; m < 3; m++) { LeftList.Add(aa.ElementAt(m).Key); }
                for (int m = 0; m < 3; m++) { RightList.Add(bb.ElementAt(m).Key); }
                #endregion
            }
            catch (Exception ex)
            {
                string Message = ex.ToString();
            }

            return sliceItemList;
        }




        public List<EyeChunk> SeeDocumentTokens(Dictionary<int, string> DocSections, List<TextLine> DocTextLines)
        {
            //Get all the font sizes extracted from the original document
            List<int> AllFontSizes = new List<int>();
            for (int m = 0; m <= DocTextLines.Count - 1; m++)
            {
                AllFontSizes.Add(Convert.ToInt32(DocTextLines[m].FontSize));
            }
            List<int> FS = (List<int>)AllFontSizes.Distinct().ToList();

            List<EyeChunk> DocumentTokens = new List<EyeChunk>();


            //Algorithm to see
            for (int m = 0; m < DocSections.Count; m++) //e.g. title, abstract, chapter1
            {
                List<TextLine> SectionSlices = new List<TextLine>();
                for (int s = 0; s < DocTextLines.Count - 1; s++) //LineItemsList[s].Page == 1 (for title section, no longer necessary)
                {
                    if (DocSections.ElementAt(m).Value.Contains("Title") == true & DocTextLines[s].Page == 1) //i.e. Title section
                    {
                        SectionSlices.Add(DocTextLines[s]);
                    } //i.e. Only page 1 of title page
                    else if (DocTextLines[s].Section == DocSections.ElementAt(m).Value) // extract only lines of the given section
                    {
                        SectionSlices.Add(DocTextLines[s]);
                    }
                }
                if (SectionSlices.Count() != 0)
                {
                    EyeChunk ck = ChunkLineItems(SectionSlices, FS, DocSections.ElementAt(m).Value);
                    DocumentTokens.Add(ck);
                }
            }
            return DocumentTokens;
        }

        private EyeChunk ChunkLineItemsOLD(List<TextLine> SI, List<int> fontSizes, string ChapterName)
        {
            List<string> ParagraphArr = new List<string>(); List<string> LineArr = new List<string>();
            List<string> SubSectionArr = new List<string>(); List<string> PageNoArr = new List<string>(); string TitleName = "";
            string Author = ""; string Supervisor = ""; string Date = ""; string TitlePara1 = ""; string TitlePara2 = "";
            string paraTag = ""; string paraPrefix = ""; string paragraphContent = "";
            string subSectionHeader = ""; string Para1Next = ""; string DateText = "";


            string[] fulfilment = { "0", "0", "0", "0", "0" }; //declare an array of 5 elements
            string[] school = { "0", "0", "0", "0", "0" }; //declare an array of 5 elements
            int currentFontSize = 0; int PageNo = 0;

            string LineContd = ""; string LinePrev = "";
            int indexNo = 0;

            Author = "Next";
            Supervisor = "Next";
            string Submitted = "Next";
            TitlePara1 = "Next";
            TitlePara2 = "Next";
            int LineTopSpacing = 0;
            string LineAlignType = "";
            int LineBottomSpacing = 0;
            int PreviousBottomSpacing = 0;

            if (ChapterName.Contains("Title")) //First chapter name is expected to be title or any page that occurs first
            {
                for (int k = 0; k <= SI.Count - 1; k++)
                {
                    string Prefix = "(" + SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        "," + SI[k].FontLeft + "," + SI[k].FontRight + ", ) || ";

                    PageNo = SI[k].Page; // Page number will be same for all slices
                    LineTopSpacing = Convert.ToInt32(SI[k].LineTopSpacing);
                    LineAlignType = SI[k].LineAlignType;
                    LineBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);

                    //Split line text by space to determine if the first word is a section heading word
                    string[] LineContents = SI[k].LineText.Split(" ");

                    if (k == 0)
                    {
                        SubSectionArr.Add(Prefix + "Section Title: Title");
                        //PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                    }


                    // Identify title details based on the bottom line spacing

                    if (k == 0 & SI[k].LineText.ToUpper().Contains("UNIVERSITY"))
                    {
                        LinePrev = "University"; //Some proposals started with name of University as the first line
                                                 // PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                    }

                    if ((k == 0 & !SI[k].LineText.ToUpper().Contains("UNIVERSITY")) & indexNo == 0 ||
                        (k > 0 & LinePrev == "University" & SI[k].LineText.Trim() != ""
                        & !SI[k].LineText.ToUpper().Contains("UNIVERSITY") & !SI[k].LineText.ToUpper().Contains("COLLEGE") & indexNo == 0)
                        )
                    {
                        TitleName = Prefix + SI[k].LineText;
                        LineContd = "Next";
                        currentFontSize = Convert.ToInt32(SI[k].FontSize);
                        LinePrev = "";
                        PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                    }
                    // else if (LineBottomSpacing <= 10 & LineContd == "Next" & indexNo == 0) LineTopSpacing

                    else if (LineTopSpacing == PreviousBottomSpacing & LineContd == "Next" & indexNo == 0)
                    {
                        TitleName = TitleName + " \n " + Prefix + SI[k].LineText;
                        // LineContd = "Next";
                        PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                        // indexNo = k;

                    }
                    // else if (LineBottomSpacing > 10 & LineContd == "Next" & indexNo == 0)
                    else if (LineTopSpacing == PreviousBottomSpacing & LineBottomSpacing > LineTopSpacing
                        & indexNo == 0)
                    {
                        TitleName = TitleName + " \n " + Prefix + SI[k].LineText; LineContd = "Next";
                        LineContd = "Next";
                        indexNo = k;

                    }

                    else if (LineContd == "Next" & indexNo > k)
                    {
                        // TitleName = TitleName + " \n " + Prefix + SI[k].LineText;
                        LineContd = "";
                        indexNo = k;
                    }

                    //Identify other elements in the title page
                    if (k > 0 & Author == "Next" & k > indexNo) //
                    {
                        //Regex AuthorRegex = new Regex(@"((By:|By){0,1}\s \n){0,1}\w*$");
                        Regex AuthorRegex = new Regex(@"(By:|By){1}(\s{0,1}\w)+$");
                        //bool test = AuthorRegex.IsMatch("By Agnes Chitsidzo Chikonzo "); //This is so cool

                        if (SI[k].LineText.Trim() != "" & AuthorRegex.IsMatch(SI[k].LineText.Trim()))
                        {
                            Author = Prefix + SI[k].LineText;
                            LineContd = "Next";
                            indexNo = k;
                            PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                        }

                        else if (SI[k].LineText.Trim() == "By:" || SI[k].LineText.Trim() == "By")
                        {
                            Author = Prefix + SI[k + 1].LineText;
                            LineContd = "Next";
                            indexNo = k;
                            PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                        }

                        else if (SI[k].LineText.Trim() != "By:" & SI[k].LineText.Trim() != "By"
                            & !AuthorRegex.IsMatch(SI[k].LineText.Trim()) & SI[k].LineText.ToUpper().Contains("BY"))
                        {
                            Author = Prefix + SI[k + 1].LineText;
                            LineContd = "Next";
                            indexNo = k;
                            PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                        }

                        else if (LineTopSpacing > PreviousBottomSpacing)
                        {
                            Author = Prefix + SI[k].LineText;
                            PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                            indexNo = k;
                        }

                        else if (new Regex(@"(By:|By){1}(\s{0,1}\w)+$").IsMatch(SI[k].LineText.Trim()))
                        {
                            Author = Prefix + SI[k].LineText;
                            PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                            indexNo = k;
                        }
                    }


                    //if (Author == "Next" & LineContd == "Next" & LineTopSpacing > PreviousBottomSpacing & k > indexNo)
                    //{
                    //    Author = Author + " \n " + Prefix + SI[k].LineText;
                    //    LineContd = "";
                    //    indexNo = k;
                    //}

                    //Get SUPERVISOR element
                    if (Supervisor == "Next" & k > indexNo)
                    {
                        Regex SupervisorRegex = new Regex(@"(Supervisor:|Supervisors:|Supervised by:){1}(\s(?:\w+-)+\w+)*");
                        //bool test = SupervisorRegex.IsMatch("Supervisor: Ope-Oluwa Iwash"); //This is so cool
                        //bool test = SupervisorRegex.IsMatch("Supervised by"); (Supervisor:|Supervisors:|Supervised by:)\n{0,1}\s{0,1}((?:\w+-)+\w+)*$
                        //((By:|By){0,1}\s \n){0,1}  \w[\w']*(?:-\w+)

                        if (SupervisorRegex.IsMatch(SI[k].LineText))
                        {
                            Supervisor = Prefix + SI[k].LineText.Trim();
                            //PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                            LineContd = "Next SupLine";
                            LineContd = "";
                            indexNo = k;
                        }
                    }

                    if (Supervisor != "Next" & LineContd == "Next SupLine" & k > indexNo & LineTopSpacing > PreviousBottomSpacing)
                    {
                        // Supervisor = Supervisor + " \n " + Prefix + SI[k].LineText.Trim();
                        PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                        indexNo = k;
                        LineContd = "";
                    }
                    else if (Supervisor != "Next" & k > indexNo & LineTopSpacing > PreviousBottomSpacing)
                    {
                        indexNo = k;
                        PreviousBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);
                        LineContd = "";
                    }




                    if ((SI[k].LineText.Contains("submitted ") & Submitted == "Next") || (SI[k].LineText.Contains("fulfilment ") & Submitted == "Next"))
                    {
                        TitlePara1 = Prefix + SI[k].LineText.Trim() + " \n ";
                        Para1Next = "Next";
                        TitlePara1 = TitlePara1.Replace("\0", "fi");
                        Submitted = "Continue";
                    }

                    if (SI[k].LineText.Contains("requirements") && Para1Next == "Next")
                    {
                        TitlePara1 = TitlePara1 + " " + Prefix + SI[k].LineText.Trim() + " \n "; Para1Next = "Next";
                        TitlePara1 = TitlePara1.Replace("\0", "fi");
                    }

                    if (!SI[k].LineText.Contains("requirements ") && SI[k].LineText.Contains("degree ") && Para1Next == "Next")
                    {
                        TitlePara1 = TitlePara1 + " " + Prefix + SI[k].LineText.Trim() + " \n "; Para1Next = "Next";
                        TitlePara1 = TitlePara1.Replace("\0", "fi");
                    }

                    if (SI[k].LineText.Contains(" in ") && Para1Next == "Next")
                    {
                        TitlePara1 = TitlePara1 + " " + Prefix + SI[k].LineText.Trim() + " \n "; Para1Next = "Next";
                        TitlePara1 = TitlePara1.Replace("\0", "fi");
                    }

                    if (SI[k].LineText.Contains("Department ")) { TitlePara2 = Prefix + SI[k].LineText.Trim() + " \n "; }
                    if (SI[k].LineText.Contains("School ")) { TitlePara2 = TitlePara2 + Prefix + SI[k].LineText.Trim() + " \n "; }
                    if (SI[k].LineText.Contains("College ") || SI[k].LineText.Contains("Faculty "))
                    { TitlePara2 = TitlePara2 + Prefix + SI[k].LineText.Trim() + " \n "; }
                    if (SI[k].LineText.Contains("University ")) { TitlePara2 = TitlePara2 + Prefix + SI[k].LineText.Trim() + " \n "; }

                    if (k == SI.Count() - 1)
                    {
                        DateText = SI[k].LineText;
                        Date = Prefix + SI[k].LineText;
                    } //Date should always be the last text on the title page
                }


            }
            else
            {
                //Group line slices into document token elements of paragraphs
                for (int k = 0; k < SI.Count - 1; k++)
                {
                    string Prefix = SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        "," + SI[k].FontLeft + "," + SI[k].FontRight + ", || ";

                    //PageNo = SI[k].Page; // Get Page number from line item

                    LineTopSpacing = Convert.ToInt32(SI[k].LineTopSpacing);
                    LineAlignType = SI[k].LineAlignType;
                    LineBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);


                    //Split line text by space to determine if the first word is a section heading word
                    string[] LineContents = SI[k].LineText.Split(" ");
                    Regex headingRegex = new Regex("^([1-9][0-9]{0,1}){1}(.[1-9][0-9]{0,1}){1}(.[1-9][0-9]{0,1})*$");

                    if (headingRegex.IsMatch(LineContents[0]))  //Paragraph heading line
                    {
                        subSectionHeader = LineContents[0];

                        SubSectionArr.Add(Prefix + LineContents[0] + " || " + SI[k].LineText);
                    }

                    if (new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k].LineText))
                    {
                        PageNoArr.Add(Prefix + SI[k].LineText);
                    }


                    if (k == 0 & SI[k].LineText.Contains("Chapter")) //the first line in all sections and chapters (except title page) will be the SectionTitle
                    {
                        SubSectionArr.Add(Prefix + "Section Title: " + SI[k + 1].LineText);
                        paraTag = "begin";
                    }
                    else if (k == 0 & !SI[k].LineText.Contains("Chapter"))
                    {
                        SubSectionArr.Add(Prefix + "Section Title: " + SI[k].LineText);
                        paraTag = "begin";
                    }

                    //Paragraph start line
                    if (paraTag == "begin" && LineTopSpacing > 10 && LineBottomSpacing <= 10) //The first line of a paragraph is observed by the top spacing and or indenting on the line
                    {
                        paraPrefix = SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        "," + SI[k].FontLeft + "," + SI[k].FontRight + ", || ";

                        paragraphContent = "";
                        paragraphContent = SI[k].LineText + "\n";
                        paraTag = "continue";
                    }

                    //Paragraph continuation line
                    if ((paraTag == "continue" && LineTopSpacing <= 10 && LineBottomSpacing <= 10) ||
                        paraTag == "continue" && LineTopSpacing == 0 && LineBottomSpacing <= 10)
                    {
                        paragraphContent = paragraphContent + SI[k].LineText + "\n";
                        paraTag = "continue";
                    }

                    //Paragraph last line
                    if (paraTag == "continue" && LineBottomSpacing >= 15 && LineTopSpacing <= 10
                        && !(new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k + 1].LineText)))
                    //This is the end of a paragraph since its not at the next page yet
                    {
                        paraPrefix = paraPrefix + SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                       + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                       "," + SI[k].FontLeft + "," + SI[k].FontRight + ", || " + subSectionHeader + " || ";

                        //  paraPrefix = "(" + SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        // + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        // "," + SI[k].FontLeft + "," + SI[k].FontRight + " || " + ")" ;

                        List<string> paragraphContentArr = paragraphContent.Split("\n").Distinct().ToList();
                        string NewPargrapgh = "";
                        for (int m = 0; m < paragraphContentArr.Count; m++)
                        { NewPargrapgh = NewPargrapgh + paragraphContentArr[m] + " "; }

                        paragraphContent = paraPrefix + NewPargrapgh + SI[k].LineText;
                        //This should indicate the last line of a paragraph
                        //Add the location parameters first before adding it to the array

                        ParagraphArr.Add(paragraphContent);
                        paraTag = "begin"; //default to start a new paragraph.
                    }

                    if ((LineTopSpacing >= 10 && LineBottomSpacing > 10 && !(new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k + 1].LineText)))
                        || (LineTopSpacing >= 10 && LineBottomSpacing <= 10 && !(new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k + 1].LineText))))
                    {
                        LineArr.Add(Prefix + SI[k].LineText);
                    }



                }
            }


            //PageNoArr[0] = PageNo.ToString();
            EyeChunk CE = new EyeChunk(ChapterName, ParagraphArr, LineArr, SubSectionArr, PageNoArr, TitleName, Author,
                Supervisor, Date, TitlePara1, TitlePara2);


            return CE;
        }

        private EyeChunk ChunkLineItems(List<TextLine> SI, List<int> fontSizes, string ChapterName)
        {
            List<string> ParagraphArr = new List<string>(); List<string> LineArr = new List<string>();
            List<string> SubSectionArr = new List<string>(); List<string> PageNoArr = new List<string>();
            string paraTag = ""; string paraPrefix = ""; string paragraphContent = "";
            string subSectionHeader = "";

            string TitleName = ""; string Author = ""; string Supervisor = "";
            string Date = ""; string TitlePara1 = ""; string TitlePara2 = "";
            string TitleNameCheck = ""; string AuthorCheck = ""; string SupervisorCheck = "";
            string DateCheck = ""; string TitlePara1Check = ""; string TitlePara2Check = "";
            int LineTopSpacing = 0; string LineAlignType = ""; int LineBottomSpacing = 0;
            int PreviousBottomSpacing = 0; int PreviousTopSpacing = 0;

            int PageNo = 0;



            if (ChapterName.Contains("Title")) //First chapter name is expected to be title or any page that occurs first
            {
                TextLine[] NewSI = SI.Where(s => s.Page == 1).ToArray();


                // String[] TitleArray = ElementString.Split('\n').Where<string>(s => !string.Equals(s.Trim(), "")).ToArray();

                for (int k = 0; k <= NewSI.Length - 1; k++)
                {
                    //Check line text page number too before arranging title page,
                    //just to ensure we are reading the values from the first page only

                    string Prefix = "(" + NewSI[k].LineTopSpacing + "," + Convert.ToInt32(NewSI[k].FontSize) + ","
                        + NewSI[k].LineAlignType + "," + NewSI[k].Page + "," + NewSI[k].FontTop + "," + NewSI[k].FontBottom +
                        "," + NewSI[k].FontLeft + "," + NewSI[k].FontRight + ", ) || ";

                    PageNo = NewSI[k].Page; // Page number will be same for all slices
                    LineTopSpacing = Convert.ToInt32(NewSI[k].LineTopSpacing);
                    LineAlignType = NewSI[k].LineAlignType;
                    LineBottomSpacing = Convert.ToInt32(NewSI[k].LineBottomSpacing);
                    string lineContents = NewSI[k].LineText;
                    //Split line text by space to determine if the first word is a section heading word
                    string[] LineContentsSplit = NewSI[k].LineText.Split(" ");

                    //Identify date
                    if (NewSI[NewSI.Length - 1].LineText.Trim() == "1" || NewSI[NewSI.Length - 1].LineText.Trim() == "i")
                    {
                        Date = Prefix + NewSI[NewSI.Length - 2].LineText;  //Date should always be the last text on the title page
                        DateCheck = "End";
                    }
                    else if (NewSI[NewSI.Length - 2].LineText.Trim() == "1" || NewSI[NewSI.Length - 2].LineText.Trim() == "i")
                    {
                        Date = Prefix + NewSI[NewSI.Length - 3].LineText;  //Date should always be the last text on the title page
                        DateCheck = "End";
                    }
                    else if (k == NewSI.Length - 1)
                    {
                        Date = Prefix + lineContents; //Date should always be the last text on the title page
                        DateCheck = "End";
                    }

                    //Identify the title
                    if (k < 3 & TitleNameCheck == "" &
                        !lineContents.ToUpper().Contains("UNIVERSITY") &
                        !lineContents.ToUpper().Contains("COLLEGE") &
                        !lineContents.ToUpper().Contains("WWW"))
                    {
                        // SubSectionArr.Add(Prefix + "Section Title: Title");
                        PreviousBottomSpacing = LineBottomSpacing;
                        PreviousTopSpacing = LineTopSpacing;
                        TitleName = Prefix + NewSI[k].LineText;
                        TitleNameCheck = "Next";
                    }

                    if (k > 0 & TitleNameCheck == "Next" & LineTopSpacing == PreviousBottomSpacing
                        & LineBottomSpacing > PreviousBottomSpacing)
                    {
                        TitleName = TitleName + " \n " + Prefix + lineContents;
                        PreviousBottomSpacing = LineBottomSpacing;
                        PreviousTopSpacing = LineTopSpacing;
                        TitleNameCheck = "End";
                    }
                    else if (k > 0 & TitleNameCheck == "Next" & LineTopSpacing == PreviousBottomSpacing)
                    // & LineBottomSpacing == PreviousBottomSpacing)
                    {
                        TitleName = TitleName + " \n " + Prefix + lineContents;
                        PreviousBottomSpacing = LineBottomSpacing;
                        PreviousTopSpacing = LineTopSpacing;
                    }



                    //Identify AUTHOR
                    if (k > 0 & TitleNameCheck == "End" & AuthorCheck == "" & lineContents.Trim() != "")
                    {
                        Regex AuthorRegex1 = new Regex(@"(By:|By){1}(\s{0,1}\w)+$", RegexOptions.IgnoreCase);
                        // Regex AuthorRegex = new Regex(@"(By:|By){1}(\s*\w)+$");
                        Regex AuthorRegex2 = new Regex(@"(By:|By){1}(\s(?:\w + -) +\w +)*", RegexOptions.IgnoreCase);

                        if (AuthorRegex1.IsMatch(NewSI[k].LineText.Trim()) || AuthorRegex2.IsMatch(NewSI[k].LineText.Trim()))
                        {
                            Author = Prefix + lineContents;
                            PreviousBottomSpacing = LineBottomSpacing;
                            PreviousTopSpacing = LineTopSpacing;
                            AuthorCheck = "Next";
                        }
                        else if (lineContents.Trim().ToUpper() == "BY:" || lineContents.Trim().ToUpper() == "BY")
                        {
                            Author = Prefix + lineContents.Trim() ;
                            AuthorCheck = "Next";
                            PreviousBottomSpacing = LineBottomSpacing;
                            PreviousTopSpacing = LineTopSpacing;
                        }
                        else if (LineTopSpacing > PreviousTopSpacing )
                           // & LineBottomSpacing > PreviousBottomSpacing) // Only Author name appear on the page
                        {
                            Author = Prefix + lineContents;
                            AuthorCheck = "End";
                            PreviousBottomSpacing = LineBottomSpacing;
                        }

                    }


                    if (AuthorCheck == "Next" & LineTopSpacing == PreviousBottomSpacing
                        & LineBottomSpacing > PreviousBottomSpacing)
                    {
                        Author = Author + " \n " + Prefix + lineContents;
                        PreviousBottomSpacing = LineBottomSpacing;
                        AuthorCheck = "End";
                    }
                    else if (AuthorCheck == "Next" & LineTopSpacing == PreviousBottomSpacing)
                    // & LineBottomSpacing == PreviousBottomSpacing)
                    {
                        Author = Author + " \n " + Prefix + lineContents;
                        PreviousBottomSpacing = LineBottomSpacing;
                    }
                    

                    //Get SUPERVISOR 
                    if (k > 0) //this line may be found on any line in the title page
                    {
                        Regex SupervisorRegex = new Regex(@"(Supervisor:|Supervisors:|Supervised by:){1}(\s(?:\w+-)+\w+)*", RegexOptions.IgnoreCase);
                        //Regex SupervisorRegex2 = new Regex(@"(Supervisor:|Supervisors:|Supervised by:){1}(\s+\w)*");
                       //Regex SupervisorRegex = new Regex(@"(Supervisor:|Supervisors:|Supervised by:){1}(\s(?:\w+-)+\w+)*");
                        //bool test = SupervisorRegex.IsMatch("supervisor: professor ina fourie"); //This is so cool
                        //bool test = SupervisorRegex.IsMatch("Supervised by"); (Supervisor:|Supervisors:|Supervised by:)
                        //\n{0,1}\s{0,1}((?:\w+-)+\w+)*$
                        //((By:|By){0,1}\s \n){0,1}  \w[\w']*(?:-\w+)
                        //Regex AuthorRegex = new Regex(@"(By:|By){1}(\s*\w)+$"); 
                        //bool checktest = test;

                        if (SupervisorRegex.IsMatch(lineContents.Trim()))
                        {
                            Supervisor = Prefix + lineContents.Trim();
                            PreviousBottomSpacing = LineBottomSpacing;
                            PreviousTopSpacing = LineTopSpacing;
                            SupervisorCheck = "Next";
                        }
                        else if (lineContents.Trim().ToUpper() == "SUPERVISOR:" ||
                            lineContents.Trim().ToUpper() == "SUPERVISORS:" ||
                            lineContents.Trim().ToUpper() == "SUPERVISED BY:")
                        {
                            Supervisor = Prefix + lineContents.Trim() + " \n " + Prefix + NewSI[k+1].LineText ;
                            PreviousBottomSpacing = LineBottomSpacing;
                            PreviousTopSpacing = LineTopSpacing;
                            SupervisorCheck = "Next";
                        }

                        if (SupervisorCheck == "Next" & LineTopSpacing == PreviousBottomSpacing
                           & LineBottomSpacing > PreviousBottomSpacing)
                        {
                            Supervisor = Supervisor + " \n " + Prefix + lineContents;
                            PreviousBottomSpacing = LineBottomSpacing;
                            SupervisorCheck = "End";
                        }
                        else if (SupervisorCheck == "Next" & LineTopSpacing == PreviousBottomSpacing)
                        //& LineBottomSpacing == PreviousBottomSpacing)
                        {
                            Supervisor = Supervisor + " \n " + Prefix + lineContents;
                            PreviousBottomSpacing = LineBottomSpacing;
                        }
                        //else if (SupervisorCheck == "Next" & LineTopSpacing > PreviousTopSpacing)
                        //{
                        //    Supervisor = Supervisor + " \n " + Prefix + lineContents;
                        //    PreviousBottomSpacing = LineBottomSpacing;
                        //}

                    }

                    if (k > 0 & TitlePara1Check == "")
                    {
                        if (lineContents.ToUpper().Contains("SUBMITTED ") || lineContents.ToUpper().Contains("FULFILMENT "))
                        {
                            TitlePara1 = Prefix + lineContents.Trim() + " \n ";
                            TitlePara1Check = "Next";
                            TitlePara1 = TitlePara1.Replace("\0", "fi");
                        }
                    }

                    if (k > 0 & TitlePara2Check == "")
                    {
                        if (lineContents.ToUpper().Contains("DEPARTMENT ") || lineContents.ToUpper().Contains("DEGREE ")
                         || lineContents.ToUpper().Contains("SCHOOL ") || lineContents.ToUpper().Contains("COLLEGE ")
                        || lineContents.ToUpper().Contains("FACULTY ") || lineContents.ToUpper().Contains("UNIVERNewSITY "))
                        {
                        }
                    }

                }
            }


            else
            {
                //Group line slices into document token elements of paragraphs
                for (int k = 0; k < SI.Count - 1; k++)
                {
                    string Prefix = SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        "," + SI[k].FontLeft + "," + SI[k].FontRight + ", || ";

                    //PageNo = SI[k].Page; // Get Page number from line item

                    LineTopSpacing = Convert.ToInt32(SI[k].LineTopSpacing);
                    LineAlignType = SI[k].LineAlignType;
                    LineBottomSpacing = Convert.ToInt32(SI[k].LineBottomSpacing);


                    //Split line text by space to determine if the first word is a section heading word
                    string[] LineContents = SI[k].LineText.Split(" ");
                    Regex headingRegex = new Regex("^([1-9][0-9]{0,1}){1}(.[1-9][0-9]{0,1}){1}(.[1-9][0-9]{0,1})*$");

                    if (headingRegex.IsMatch(LineContents[0]))  //Paragraph heading line
                    {
                        subSectionHeader = LineContents[0];

                        SubSectionArr.Add(Prefix + LineContents[0] + " || " + SI[k].LineText);
                    }

                    if (new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k].LineText))
                    {
                        PageNoArr.Add(Prefix + SI[k].LineText);
                    }


                    if (k == 0 & SI[k].LineText.Contains("Chapter")) //the first line in all sections and chapters (except title page) will be the SectionTitle
                    {
                        SubSectionArr.Add(Prefix + "Section Title: " + SI[k + 1].LineText);
                        paraTag = "begin";
                    }
                    else if (k == 0 & !SI[k].LineText.Contains("Chapter"))
                    {
                        SubSectionArr.Add(Prefix + "Section Title: " + SI[k].LineText);
                        paraTag = "begin";
                    }

                    //Paragraph start line
                    if (paraTag == "begin" && LineTopSpacing > 10 && LineBottomSpacing <= 10) //The first line of a paragraph is observed by the top spacing and or indenting on the line
                    {
                        paraPrefix = SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        "," + SI[k].FontLeft + "," + SI[k].FontRight + ", || ";

                        paragraphContent = "";
                        paragraphContent = SI[k].LineText + "\n";
                        paraTag = "continue";
                    }

                    //Paragraph continuation line
                    if ((paraTag == "continue" && LineTopSpacing <= 10 && LineBottomSpacing <= 10) ||
                        paraTag == "continue" && LineTopSpacing == 0 && LineBottomSpacing <= 10)
                    {
                        paragraphContent = paragraphContent + SI[k].LineText + "\n";
                        paraTag = "continue";
                    }

                    //Paragraph last line
                    if (paraTag == "continue" && LineBottomSpacing >= 15 && LineTopSpacing <= 10
                        && !(new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k + 1].LineText)))
                    //This is the end of a paragraph since its not at the next page yet
                    {
                        paraPrefix = paraPrefix + SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                       + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                       "," + SI[k].FontLeft + "," + SI[k].FontRight + ", || " + subSectionHeader + " || ";

                        //  paraPrefix = "(" + SI[k].LineTopSpacing + "," + Convert.ToInt32(SI[k].FontSize) + ","
                        // + SI[k].LineAlignType + "," + SI[k].Page + "," + SI[k].FontTop + "," + SI[k].FontBottom +
                        // "," + SI[k].FontLeft + "," + SI[k].FontRight + " || " + ")" ;

                        List<string> paragraphContentArr = paragraphContent.Split("\n").Distinct().ToList();
                        string NewPargrapgh = "";
                        for (int m = 0; m < paragraphContentArr.Count; m++)
                        { NewPargrapgh = NewPargrapgh + paragraphContentArr[m] + " "; }

                        paragraphContent = paraPrefix + NewPargrapgh + SI[k].LineText;
                        //This should indicate the last line of a paragraph
                        //Add the location parameters first before adding it to the array

                        ParagraphArr.Add(paragraphContent);
                        paraTag = "begin"; //default to start a new paragraph.
                    }

                    if ((LineTopSpacing >= 10 && LineBottomSpacing > 10 && !(new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k + 1].LineText)))
                        || (LineTopSpacing >= 10 && LineBottomSpacing <= 10 && !(new Regex("^[1-9][0-9]{0,1}$").IsMatch(SI[k + 1].LineText))))
                    {
                        LineArr.Add(Prefix + SI[k].LineText);
                    }



                }
            }


            //PageNoArr[0] = PageNo.ToString();
            EyeChunk CE = new EyeChunk(ChapterName, ParagraphArr, LineArr, SubSectionArr, PageNoArr, TitleName, Author,
                Supervisor, Date, TitlePara1, TitlePara2);


            return CE;
        }

        public int CreateClassObjectOfProposal_And_XML_File(List<EyeChunk> chunkElement, string filename, string filePath) //sample of proposal has 14 elements
        {
            int checkResult = 0;
            //Read chunk of proposal elements into a proposal object
            // var chunkElementCount = chunkElement.ToList();
            ProposalDocument P = new ProposalDocument();
            PreliminarySection preliminary = new PreliminarySection();
            ContentSection DocContent = new ContentSection();
            ReferenceSection DocReferences = new ReferenceSection();
            AppendixSection DocAppendix = new AppendixSection();
            P.preliminarySection = preliminary;
            P.contentSection = DocContent;
            P.referenceSection = DocReferences;
            P.appendixSection = DocAppendix;

            for (int a = 0; a <= chunkElement.Count() - 1; a++)
            {
                // if chunkElement[a].DocumentSection=""
                //Use a select case in a private class based on the document section of each chunck element
                //selec-case will receive element and proposal and return proposal untill all element cycle is completed
                //that way it isnt hard coded

                string currentSection = chunkElement[a].DocumentSection;

                if (currentSection == "TitleSection")
                {
                    P = TitleSectionXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Declaration"))
                {
                    P = DeclarationXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Acknowledgement"))
                {
                    P = AcknowledgementXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Abstract"))
                {
                    P = AbstractXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Table"))
                {
                    P = ListofTableXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Figure"))
                {
                    P = ListofFigureXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Abstract"))
                {
                    P = AbstractXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Content"))
                {
                    P = ListofContentXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter1") || currentSection.ToUpper().Contains("CHAPTERONE"))
                {
                    P = ChapterOneXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter2") || currentSection.ToUpper().Contains("CHAPTERTWO"))
                {
                    P = ChapterTwoXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter3") || currentSection.ToUpper().Contains("CHAPTERTHREE"))
                {
                    P = ChapterThreeXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter4") || currentSection.ToUpper().Contains("CHAPTERFOUR"))
                {
                    P = ChapterFourXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter5") || currentSection.ToUpper().Contains("CHAPTERFIVE"))
                {
                    P = ChapterFiveXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter6") || currentSection.ToUpper().Contains("CHAPTERSIX"))
                {
                    P = ChapterSixXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter7") || currentSection.ToUpper().Contains("CHAPTERSEVEN"))
                {
                    P = ChapterSevenXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter8") || currentSection.ToUpper().Contains("CHAPTEREIGHT"))
                {
                    P = ChapterEightXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter9") || currentSection.ToUpper().Contains("CHAPTERNINE"))
                {
                    P = ChapterNineXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Chapter10") || currentSection.ToUpper().Contains("CHAPTERTEN"))
                {
                    P = ChapterTenXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Reference") || currentSection.Contains("Bibliography"))
                {
                    P = ReferenceXML(chunkElement[a], P);
                }
                else if (currentSection.Contains("Appendix") || currentSection.Contains("Appendice"))
                {
                    P = AppendixXML(chunkElement[a], P);
                }
            }

            WriteComplexObjectXML(P, filename, filePath);

            return checkResult;

        }

        private ProposalDocument TitleSectionXML(EyeChunk titleChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            TitlePage tp = new TitlePage();
            // PNew = new ProposalDocument();

            tp.title = WriteElementContentIntoString(titleChunk.TitleName);
            tp.author = WriteElementContentIntoString(titleChunk.Author);
            List<string> k = new List<string>();
            k.Add(WriteElementContentIntoString(titleChunk.Supervisor));
            tp.supervisors = k;
            tp.titleParagragh1 = WriteElementContentIntoString(titleChunk.TitleParagraph1);
            tp.titleParagraph2 = WriteElementContentIntoString(titleChunk.TitleParagraph2);
            tp.submissionDate = WriteElementContentIntoString(titleChunk.Date);

            try
            {
                P.preliminarySection.titlePage = tp; //
                                                     //P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }


            return P;
        }

        private ProposalDocument DeclarationXML(EyeChunk DeclarationChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            DeclarationPage dp = new DeclarationPage();
            List<Paragraph> lp = new List<Paragraph>();
            string parachunk = "";
            Paragraph decp = new Paragraph();
            for (int m = 0; m < DeclarationChunk.LineArr.Count - 1; m++)
            {
                parachunk = parachunk + WriteElementContentIntoString(DeclarationChunk.LineArr[m]);
                if (DeclarationChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    decp.textChunk = parachunk;
                    lp.Add(decp); parachunk = "";
                }
            }
            decp.textChunk = parachunk;
            lp.Add(decp);
            dp.paragraph = lp;

            try
            {
                P.preliminarySection.declarationPage = dp; //
                                                           // P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }


            return P;
        }

        private ProposalDocument AcknowledgementXML(EyeChunk AcknowledgementChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            AcknowledgementPage ap = new AcknowledgementPage();
            List<Paragraph> lp2 = new List<Paragraph>();
            string parachunk2 = "";
            for (int m = 0; m < AcknowledgementChunk.LineArr.Count - 1; m++)
            {
                Paragraph p = new Paragraph();
                parachunk2 = parachunk2 + WriteElementContentIntoString(AcknowledgementChunk.LineArr[m]);
                if (AcknowledgementChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { p.textChunk = parachunk2; lp2.Add(p); parachunk2 = ""; }
            }
            ap.paragraph = lp2;

            try
            {
                P.preliminarySection.acknowledgementPage = ap; //
                                                               //  P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }


            return P;
        }

        private ProposalDocument ListofContentXML(EyeChunk ListOfContentChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            ListOfContents abp = new ListOfContents();
            List<String> lp3 = new List<String>();
            string chunk = "";
            for (int m = 0; m < ListOfContentChunk.LineArr.Count - 1; m++)
            {
                chunk = chunk + WriteElementContentIntoString(ListOfContentChunk.LineArr[m]) + "\n";
                if (ListOfContentChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { lp3.Add(chunk); chunk = ""; }
            }
            abp.table = lp3;

            try
            {
                P.preliminarySection.listOfContentsPage = abp; //
                                                               //  P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }


            return P;
        }

        private ProposalDocument ListofFigureXML(EyeChunk ListOfContentChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            ListOfFigures abp = new ListOfFigures();
            List<String> lp3 = new List<String>();
            string chunk = "";
            for (int m = 0; m < ListOfContentChunk.LineArr.Count - 1; m++)
            {
                chunk = chunk + WriteElementContentIntoString(ListOfContentChunk.LineArr[m]) + "\n";
                if (ListOfContentChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { lp3.Add(chunk); chunk = ""; }
            }
            abp.figures = lp3;

            try
            {
                P.preliminarySection.listOfFiguresPage = abp; //
                                                              // P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }


            return P;
        }

        private ProposalDocument ListofTableXML(EyeChunk ListOfContentChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            ListOfTables abp = new ListOfTables();
            List<String> lp3 = new List<String>();
            string chunk = "";
            for (int m = 0; m < ListOfContentChunk.LineArr.Count - 1; m++)
            {
                chunk = chunk + WriteElementContentIntoString(ListOfContentChunk.LineArr[m]) + "\n";
                if (ListOfContentChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { lp3.Add(chunk); chunk = ""; }
            }
            abp.table = lp3;
            //PreliminarySection newpreliminary = P.preliminarySection; //


            try
            {
                P.preliminarySection.listOfTablesPage = abp; //
                                                             //P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }


            return P;
        }

        private ProposalDocument AbstractXML(EyeChunk AbstractChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            AbstractPage abp = new AbstractPage();
            List<Paragraph> lp3 = new List<Paragraph>();
            string parachunk3 = "";
            for (int m = 0; m < AbstractChunk.LineArr.Count - 1; m++)
            {
                Paragraph p = new Paragraph();
                parachunk3 = parachunk3 + WriteElementContentIntoString(AbstractChunk.LineArr[m]);
                if (AbstractChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { p.textChunk = parachunk3; lp3.Add(p); parachunk3 = ""; }
            }
            abp.paragraph = lp3;
            //PreliminarySection newpreliminary = P.preliminarySection; //


            try
            {
                P.preliminarySection.abstractPage = abp; //
                                                         // P.preliminarySection = preliminary; //
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }


        private ProposalDocument ChapterOneXML(EyeChunk IntroductionChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            List<Paragraph> lpChp1 = new List<Paragraph>();
            string Chap1Parachunk = "";
            Paragraph p1 = new Paragraph();
            for (int m = 0; m < IntroductionChunk.LineArr.Count - 1; m++)
            {
                if (IntroductionChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p1.textChunk = Chap1Parachunk;
                    lpChp1.Add(p1);
                    Chap1Parachunk = "";
                    Chap1Parachunk = Chap1Parachunk + WriteElementContentIntoString(IntroductionChunk.LineArr[m]) + "\n";
                    p1 = new Paragraph();
                }
                else { Chap1Parachunk = Chap1Parachunk + WriteElementContentIntoString(IntroductionChunk.LineArr[m]) + "\n"; }
            }
            lpChp1.Add(p1);
            ChapterOne chapter1 = new ChapterOne();
            chapter1.paragraph = lpChp1;

            try
            {
                P.contentSection.chapterOne = chapter1;
                // P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument ChapterTwoXML(EyeChunk LiteratureChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            List<Paragraph> lpChp2 = new List<Paragraph>();
            string Chap2Parachunk = "";
            Paragraph p2 = new Paragraph();
            for (int m = 0; m < LiteratureChunk.LineArr.Count - 2; m++)
            {
                if (LiteratureChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p2.textChunk = Chap2Parachunk;
                    lpChp2.Add(p2);
                    Chap2Parachunk = "";
                    Chap2Parachunk = Chap2Parachunk + WriteElementContentIntoString(LiteratureChunk.LineArr[m]) + "\n";
                    p2 = new Paragraph();
                }
                else { Chap2Parachunk = Chap2Parachunk + WriteElementContentIntoString(LiteratureChunk.LineArr[m]) + "\n"; }
            }
            lpChp2.Add(p2);
            ChapterTwo chapter2 = new ChapterTwo();
            chapter2.paragraph = lpChp2;

            try
            {
                P.contentSection.chapterTwo = chapter2;
                // P.contentSection = DocContent;
            }
            catch (Exception ex) { string errMsg = ex.Message; }


            return P;
        }

        private ProposalDocument ChapterThreeXML(EyeChunk MethodologyChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp3 = new List<Paragraph>();
            string Chap3Parachunk = "";
            Paragraph p3 = new Paragraph();
            for (int m = 0; m < MethodologyChunk.LineArr.Count - 3; m++)
            {
                if (MethodologyChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p3.textChunk = Chap3Parachunk;
                    lpChp3.Add(p3);
                    Chap3Parachunk = "";
                    Chap3Parachunk = Chap3Parachunk + WriteElementContentIntoString(MethodologyChunk.LineArr[m]) + "\n";
                    p3 = new Paragraph();
                }
                else { Chap3Parachunk = Chap3Parachunk + WriteElementContentIntoString(MethodologyChunk.LineArr[m]) + "\n"; }
            }
            lpChp3.Add(p3);
            ChapterThree chapter3 = new ChapterThree();
            chapter3.paragraph = lpChp3;

            try
            {
                P.contentSection.chapterThree = chapter3;
                //P.contentSection = DocContent;
            }
            catch (Exception ex) { string errMsg = ex.Message; }

            return P;
        }

        private ProposalDocument ChapterFourXML(EyeChunk ChapterFourChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp4 = new List<Paragraph>();
            string Chap4Parachunk = "";
            Paragraph p4 = new Paragraph();
            for (int m = 0; m < ChapterFourChunk.LineArr.Count - 3; m++)
            {
                if (ChapterFourChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p4.textChunk = Chap4Parachunk;
                    lpChp4.Add(p4);
                    Chap4Parachunk = "";
                    Chap4Parachunk = Chap4Parachunk + WriteElementContentIntoString(ChapterFourChunk.LineArr[m]) + "\n";
                    p4 = new Paragraph();
                }
                else { Chap4Parachunk = Chap4Parachunk + WriteElementContentIntoString(ChapterFourChunk.LineArr[m]) + "\n"; }
            }
            lpChp4.Add(p4);
            ChapterFour chapter4 = new ChapterFour();
            chapter4.paragraph = lpChp4;

            try
            {
                P.contentSection.chapterFour = chapter4;
                //P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument ChapterFiveXML(EyeChunk ChapterFiveChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp5 = new List<Paragraph>();
            string Chap5Parachunk = "";
            Paragraph p5 = new Paragraph();
            for (int m = 0; m < ChapterFiveChunk.LineArr.Count - 3; m++)
            {
                if (ChapterFiveChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p5.textChunk = Chap5Parachunk;
                    lpChp5.Add(p5);
                    Chap5Parachunk = "";
                    Chap5Parachunk = Chap5Parachunk + WriteElementContentIntoString(ChapterFiveChunk.LineArr[m]) + "\n";
                    p5 = new Paragraph();
                }
                else { Chap5Parachunk = Chap5Parachunk + WriteElementContentIntoString(ChapterFiveChunk.LineArr[m]) + "\n"; }
            }
            lpChp5.Add(p5);
            ChapterFive chapter5 = new ChapterFive();
            chapter5.paragraph = lpChp5;

            try
            {
                P.contentSection.chapterFive = chapter5;
                //P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }
            return P;
        }

        private ProposalDocument ChapterSixXML(EyeChunk ChapterSixChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp6 = new List<Paragraph>();
            string Chap6Parachunk = "";
            Paragraph p6 = new Paragraph();
            for (int m = 0; m < ChapterSixChunk.LineArr.Count - 3; m++)
            {
                if (ChapterSixChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p6.textChunk = Chap6Parachunk;
                    lpChp6.Add(p6);
                    Chap6Parachunk = "";
                    Chap6Parachunk = Chap6Parachunk + WriteElementContentIntoString(ChapterSixChunk.LineArr[m]) + "\n";
                    p6 = new Paragraph();
                }
                else { Chap6Parachunk = Chap6Parachunk + WriteElementContentIntoString(ChapterSixChunk.LineArr[m]) + "\n"; }
            }
            lpChp6.Add(p6);
            ChapterSix chapter6 = new ChapterSix();
            chapter6.paragraph = lpChp6;

            try
            {
                P.contentSection.chapterSix = chapter6;
                // P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument ChapterSevenXML(EyeChunk ChapterSevenChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp7 = new List<Paragraph>();
            string Chap7Parachunk = "";
            Paragraph p7 = new Paragraph();
            for (int m = 0; m < ChapterSevenChunk.LineArr.Count - 3; m++)
            {
                if (ChapterSevenChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p7.textChunk = Chap7Parachunk;
                    lpChp7.Add(p7);
                    Chap7Parachunk = "";
                    Chap7Parachunk = Chap7Parachunk + WriteElementContentIntoString(ChapterSevenChunk.LineArr[m]) + "\n";
                    p7 = new Paragraph();
                }
                else { Chap7Parachunk = Chap7Parachunk + WriteElementContentIntoString(ChapterSevenChunk.LineArr[m]) + "\n"; }
            }
            lpChp7.Add(p7);
            ChapterSeven chapter7 = new ChapterSeven();
            chapter7.paragraph = lpChp7;

            try
            {
                P.contentSection.chapterSeven = chapter7;
                //P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument ChapterEightXML(EyeChunk ChapterEightChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp8 = new List<Paragraph>();
            string Chap8Parachunk = "";
            Paragraph p8 = new Paragraph();
            for (int m = 0; m < ChapterEightChunk.LineArr.Count - 3; m++)
            {
                if (ChapterEightChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p8.textChunk = Chap8Parachunk;
                    lpChp8.Add(p8);
                    Chap8Parachunk = "";
                    Chap8Parachunk = Chap8Parachunk + WriteElementContentIntoString(ChapterEightChunk.LineArr[m]) + "\n";
                    p8 = new Paragraph();
                }
                else { Chap8Parachunk = Chap8Parachunk + WriteElementContentIntoString(ChapterEightChunk.LineArr[m]) + "\n"; }
            }
            lpChp8.Add(p8);
            ChapterEight chapter8 = new ChapterEight();
            chapter8.paragraph = lpChp8;

            try
            {
                P.contentSection.chapterEight = chapter8;
                // P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument ChapterNineXML(EyeChunk ChapterNineChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp9 = new List<Paragraph>();
            string Chap9Parachunk = "";
            Paragraph p9 = new Paragraph();
            for (int m = 0; m < ChapterNineChunk.LineArr.Count - 3; m++)
            {
                if (ChapterNineChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p9.textChunk = Chap9Parachunk;
                    lpChp9.Add(p9);
                    Chap9Parachunk = "";
                    Chap9Parachunk = Chap9Parachunk + WriteElementContentIntoString(ChapterNineChunk.LineArr[m]) + "\n";
                    p9 = new Paragraph();
                }
                else { Chap9Parachunk = Chap9Parachunk + WriteElementContentIntoString(ChapterNineChunk.LineArr[m]) + "\n"; }
            }
            lpChp9.Add(p9);
            ChapterNine chapter9 = new ChapterNine();
            chapter9.paragraph = lpChp9;

            try
            {
                P.contentSection.chapterNine = chapter9;
                // P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument ChapterTenXML(EyeChunk ChapterTenChunk, ProposalDocument P)
        {
            List<Paragraph> lpChp10 = new List<Paragraph>();
            string Chap10Parachunk = "";
            Paragraph p10 = new Paragraph();
            for (int m = 0; m < ChapterTenChunk.LineArr.Count - 3; m++)
            {
                if (ChapterTenChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                {
                    p10.textChunk = Chap10Parachunk;
                    lpChp10.Add(p10);
                    Chap10Parachunk = "";
                    Chap10Parachunk = Chap10Parachunk + WriteElementContentIntoString(ChapterTenChunk.LineArr[m]) + "\n";
                    p10 = new Paragraph();
                }
                else { Chap10Parachunk = Chap10Parachunk + WriteElementContentIntoString(ChapterTenChunk.LineArr[m]) + "\n"; }
            }
            lpChp10.Add(p10);
            ChapterTen chapter10 = new ChapterTen();
            chapter10.paragraph = lpChp10;

            try
            {
                P.contentSection.chapterTen = chapter10;
                //P.contentSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }


        private ProposalDocument ReferenceXML(EyeChunk ReferenceChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            References abp = new References();
            List<String> lp3 = new List<String>();
            string chunk = "";
            for (int m = 0; m < ReferenceChunk.LineArr.Count - 1; m++)
            {
                chunk = chunk + WriteElementContentIntoString(ReferenceChunk.LineArr[m]) + "\n";
                if (ReferenceChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { lp3.Add(chunk); chunk = ""; }
            }
            abp.listOfReferences = lp3;



            try
            {
                P.referenceSection.referencePage = abp;
                //P.referenceSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private ProposalDocument AppendixXML(EyeChunk ReferenceChunk, ProposalDocument P)
        //  out ProposalDocument PNew)
        {
            Appendices abp = new Appendices();
            List<String> lp3 = new List<String>();
            string chunk = "";
            for (int m = 0; m < ReferenceChunk.LineArr.Count - 1; m++)
            {
                chunk = chunk + WriteElementContentIntoString(ReferenceChunk.LineArr[m]) + "\n";
                if (ReferenceChunk.LineArr[m].Split("||")[0].Split(",")[2] == "para-begin")
                { lp3.Add(chunk); chunk = ""; }
            }
            abp.listOfFigures = lp3;



            try
            {
                P.appendixSection.Appendices = abp;
                // P.appendixSection = DocContent;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return P;
        }

        private string WriteElementContentIntoString(string ElementString)
        {
            if (ElementString.Trim() != "")
            {
                String[] TitleArray = ElementString.Split('\n').Where<string>(s => !string.Equals(s.Trim(), "")).ToArray();
                String Result = "";
                TitleArray[TitleArray.ToList().Count - 1] = TitleArray[TitleArray.Length - 1] + "||";
                try
                {
                    var SplitStringValues = TitleArray.Select(x => x.Split("||")[1].Trim());
                    Result = string.Join(" ", SplitStringValues);
                }
                catch (Exception ex)
                {
                    string m = ex.Message.ToString();
                }

                return Result;
            }
            else { return ""; }

            //var SplitStringValues = TitleArray.Select(x => x.Split("||")[x.Split("||").Length-1].Trim());


        }

        public static void WriteComplexObjectXML(ProposalDocument bb, string filename, string filePath)
        {

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(ProposalDocument));

            // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\"
            //   + filename.Split("\\")[filename.Split("\\").Length - 1].Split(".")[0] + ".xml";

            var path = filePath + "\\"
                + filename.Split("\\")[filename.Split("\\").Length - 1].Split(".")[0] + ".xml";

            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, bb);
            file.Close();

        }

    }
}
