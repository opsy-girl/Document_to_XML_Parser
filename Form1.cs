using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlicerLibrary;
using System.Configuration;
using System.Collections.Specialized;

namespace XParser
{
    public partial class Form1 : Form
    {

        int scount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SectionCount"]);
        string generateFilesHere;
        Dictionary<int, string> sectionList = new Dictionary<int, string>();
        string[] files;
        string HasChapters = "No";
        string numwords = "";

        public Form1()
        {
            InitializeComponent();

            for (int a = 1; a <= scount; a++)
            {
                string appValue = System.Configuration.ConfigurationManager.AppSettings[a.ToString()];
                sectionList.Add(a, appValue);
                if (appValue.Contains("hapter"))
                {
                    HasChapters = "True";
                }
            }

            if (HasChapters == "True")
            {
                int sectionCount = sectionList.Count() + 1;
                for (int m = 0; m < 10; m++) //maximum of ten chapters allowed
                {
                    switch (m + 1)
                    {
                        case 1: numwords = "one"; break;
                        case 2: numwords = "two"; break;
                        case 3: numwords = "three"; break;
                        case 4: numwords = "four"; break;
                        case 5: numwords = "five"; break;
                        case 6: numwords = "six"; break;
                        case 7: numwords = "seven"; break;
                        case 8: numwords = "eight"; break;
                        case 9: numwords = "nine"; break;
                        case 10: numwords = "ten"; break;
                    }
                    int n = m + 1;
                    sectionList.Add(sectionCount, "Chapter" + n); sectionCount = sectionCount + 1;
                    sectionList.Add(sectionCount, "Chapter" + numwords); sectionCount = sectionCount + 1;
                }


            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();
                    generateFilesHere = fbd.SelectedPath;

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        files = Directory.GetFiles(fbd.SelectedPath);

                        System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                    }
                }
            }
            catch (Exception ex)
            {
                string f = ex.Message.ToString();
            }
            NewSlicer SS = new NewSlicer();
            Dictionary<string, int> chapterPages = new Dictionary<string, int>();
            int totalPages = 0;


            for (int k = 0; k <= files.Length - 1; k++)
            {
                //Make sure all the slices have section names else it will not compile correctly
                List<TextLine> LineItemsList = SS.SliceDocument(files[k], sectionList, out chapterPages, out totalPages);

                List<EyeChunk> DocTokenList = SS.SeeDocumentTokens(sectionList, LineItemsList);

                int FinalResult = SS.CreateClassObjectOfProposal_And_XML_File(DocTokenList, files[k], generateFilesHere);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
