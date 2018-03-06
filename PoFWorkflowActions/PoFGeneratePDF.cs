using System;
using Microsoft.Office.Word.Server.Conversions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.ComponentModel;
using System.IO;

namespace PoFWorkflowActions
{
    public class PoFGeneratePDF : Activity
    {

        #region Test

        public void TestPDF(string _Url, string _Liste, int _ElementID, string _WASName)
        {
                WASName = _WASName;
                using (SPSite sitecollection = new SPSite(_Url))
                {
                    using (SPWeb web = sitecollection.OpenWeb())
                    {
                        var list = web.Lists.TryGetList(_Liste);
                        if ((list != null) && (_ElementID > 0))
                        {
                            WordDocsToConvertToPdf(list, _ElementID);
                        }
                    }
                }
        }

        #endregion

        #region Eigenschaften

        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(PoFGeneratePDF));
        public static DependencyProperty ListeProperty = DependencyProperty.Register("Liste", typeof(string), typeof(PoFGeneratePDF));
        public static DependencyProperty ElementIDProperty = DependencyProperty.Register("ElementID", typeof(int), typeof(PoFGeneratePDF));
        public static DependencyProperty WASNameProperty = DependencyProperty.Register("WASName", typeof(string), typeof(PoFGeneratePDF));


        [Description("Url der Bibliothek")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Url
        {
            get
            {
                return ((string)(base.GetValue(PoFGeneratePDF.UrlProperty)));
            }
            set
            {
                base.SetValue(PoFGeneratePDF.UrlProperty, value);
            }
        }

        [Description("Liste")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Liste
        {
            get
            {
                return ((string)(base.GetValue(PoFGeneratePDF.ListeProperty)));
            }
            set
            {
                base.SetValue(PoFGeneratePDF.ListeProperty, value);
            }
        }

        [Description("ElementID")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ElementID
        {
            get
            {
                return ((int)(base.GetValue(PoFGeneratePDF.ElementIDProperty)));
            }
            set
            {
                base.SetValue(PoFGeneratePDF.ElementIDProperty, value);
            }
        }

        [Description("WAS Name")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string WASName
        {
            get
            {
                return ((string)(base.GetValue(PoFGeneratePDF.WASNameProperty)));
            }
            set
            {
                base.SetValue(PoFGeneratePDF.WASNameProperty, value);
            }
        }

        #endregion


        //Will require this using statement
  
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite sitecollection = new SPSite(Url))
                {
                    using (SPWeb web = sitecollection.OpenWeb())
                    {
                        var list = web.Lists.TryGetList(Liste);
                        if ((list != null) && (ElementID > 0))
                        {
                            WordDocsToConvertToPdf(list, ElementID);
                        }
                    }
                }
            });
            return base.Execute(executionContext);
        }

        private void WordDocsToConvertToPdf(SPList library, int ElementID)
        {
            //Perform a SPQuery that returns only Word Documents.
            SPQuery query = new SPQuery();
            query.Folder = library.RootFolder;
            //Include all subfolders so include Recursive Scope.
            query.ViewXml = @"<View Scope='Recursive'>
                                <Query>
                                   <Where>
                                        <Or>
                                            <Contains>
                                                <FieldRef Name='File_x0020_Type'/>
                                                <Value Type='Text'>doc</Value>
                                            </Contains>
                                            <Contains>
                                                <FieldRef Name='File_x0020_Type'/>
                                                <Value Type='Text'>docx</Value>
                                            </Contains>
                                        </Or>
                                    </Where>
                                </Query>
                            </View>";

            //Get Documents
//          SPListItemCollection listItems = library.GetItems(query);

            SPListItem listitem = library.Items.GetItemById(ElementID);

            //Check that there are any documents to convert.
//            if (listItems.Count > 0)
//            {
//                foreach (SPListItem li in listItems)
//                {
                    //Perform the conversion in memory first, therefore we require a MemoryStream.
                    using (MemoryStream destinationStream = new MemoryStream())
                    {
                        //Call the syncConverter class, passing in the name of the Word Automation Service for your Farm.
                        SyncConverter sc = new SyncConverter(WASName);
                        //Pass in your User Token or credentials under which this conversion job is executed.
                        
                        if (SPContext.Current != null)
                            sc.UserToken = SPContext.Current.Site.UserToken;
                        
                        sc.Settings.UpdateFields = true;

                        //Save format
                        sc.Settings.OutputFormat = SaveFormat.PDF;

                        //Convert to PDF by opening the file stream, and then converting to the destination memory stream.
                        ConversionItemInfo info = sc.Convert(listitem.File.OpenBinaryStream(), destinationStream);

                        var filename = Path.GetFileNameWithoutExtension(listitem.File.Name) + ".pdf";
                        if (info.Succeeded)
                        {
                            //File conversion successful, then add the memory stream to the SharePoint list.
                            SPFile newfile = library.RootFolder.Files.Add(filename, destinationStream, true);
                        }
                        else if (info.Failed)
                        {
                            throw new Exception(info.ErrorMessage);
                        }
                    }
//                }
//            }
        }
    }
}
