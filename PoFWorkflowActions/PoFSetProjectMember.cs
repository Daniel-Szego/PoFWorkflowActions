using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using Microsoft.SharePoint.Publishing.Fields;
using System;


namespace PoFWorkflowActions
{
    public class PoFSetProjectMember : Activity
    {
        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty AccountNameProperty = DependencyProperty.Register("AccountName", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty DescriptProperty = DependencyProperty.Register("Descript", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty ContentTypeProperty = DependencyProperty.Register("ContentType", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty LinkProperty = DependencyProperty.Register("Link", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty TargetDateProperty = DependencyProperty.Register("TargetDate", typeof(DateTime), typeof(PoFSetProjectMember));
        public static DependencyProperty QuelleProperty = DependencyProperty.Register("Quelle", typeof(string), typeof(PoFSetProjectMember));
        public static DependencyProperty WorkflowIDProperty = DependencyProperty.Register("WorkflowID", typeof(int), typeof(PoFSetProjectMember));
        public static DependencyProperty TaskIDProperty = DependencyProperty.Register("TaskID", typeof(int), typeof(PoFSetProjectMember));

        #region Eigenschaften

        [Description("Url der Aufgabenliste")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Url
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.UrlProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.UrlProperty, value);
            }
        }

        [Description("Account Name des zugewiesenen Benutzers")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string AccountName
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.AccountNameProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.AccountNameProperty, value);
            }
        }

        [Description("Titel der Aufgabe")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.TitleProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.TitleProperty, value);
            }
        }

        [Description("Beschreibung der Aufgabe")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Descript
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.DescriptProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.DescriptProperty, value);
            }
        }

        [Description("Inhaltstyp der Aufgabe")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ContentType
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.ContentTypeProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.ContentTypeProperty, value);
            }
        }

        [Description("Link zum Dokument")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Link
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.LinkProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.LinkProperty, value);
            }
        }

        [Description("Zieltermin")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime TargetDate
        {
            get
            {
                return ((DateTime)(base.GetValue(PoFSetProjectMember.TargetDateProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.TargetDateProperty, value);
            }
        }

        [Description("Quelle")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Quelle
        {
            get
            {
                return ((string)(base.GetValue(PoFSetProjectMember.QuelleProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.QuelleProperty, value);
            }
        }

        [Description("WorkflowID")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int WorkflowID
        {
            get
            {
                return ((int)(base.GetValue(PoFSetProjectMember.WorkflowIDProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.WorkflowIDProperty, value);
            }
        }

        [Description("ID der Aufgabe")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int TaskID
        {
            get
            {
                return ((int)(base.GetValue(PoFSetProjectMember.TaskIDProperty)));
            }
            set
            {
                base.SetValue(PoFSetProjectMember.TaskIDProperty, value);
            }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite sitecollection = new SPSite(Url))
                {
                    using (SPWeb web = sitecollection.OpenWeb())
                    {
                        SPList list = web.Lists["Aufgaben"];
                        SPListItem listitem = list.Items.Add();
                        SPContentType ConType = list.ContentTypes[ContentType.ToString()];

                        if (ConType != null)
                            listitem["ContentTypeId"] = ConType.Id;

                        if (!String.IsNullOrEmpty(Title) & listitem.Fields.ContainsField("Title"))
                        {
                            listitem["Title"] = Title.ToString();
                        }

                        if (!String.IsNullOrEmpty(Descript) & (listitem.Fields.ContainsField("Beschreibung") | listitem.Fields.ContainsField("Body")))
                        {
                            listitem["Beschreibung"] = Descript.ToString();
                        }

                        if (!String.IsNullOrEmpty(Link) & listitem.Fields.ContainsField("Link"))
                        {
                            LinkFieldValue lfValue = new LinkFieldValue();
                            lfValue.NavigateUrl = Link.ToString();
                            lfValue.Text = "Link zu Dokument";
                            lfValue.UseDefaultIcon = false;

                            listitem["Link"] = lfValue;
                        }

                        if (!String.IsNullOrEmpty(AccountName) & listitem.Fields.ContainsField("AssignedTo"))
                        {
                            SPUserCollection users = web.Users;
                            SPUser user = UserExists(users, AccountName.ToString());

                            if (user != null)
                                listitem["AssignedTo"] = user;
                            else
                            {
                                SPGroupCollection groups = web.Groups;

                                if (GroupExists(groups, AccountName.ToString()))
                                    listitem["AssignedTo"] = web.Groups[AccountName.ToString()];
                            }
                        }

                        if (!String.IsNullOrEmpty(Quelle) & listitem.Fields.ContainsField("Aufgabenquelle"))
                            listitem["Aufgabenquelle"] = Quelle.ToString();

                        if ((TargetDate.ToString() != string.Empty) & (listitem.Fields.ContainsField("DueDate")))
                            listitem["DueDate"] = TargetDate;

                        if (listitem.Fields.ContainsField("WorkflowID"))
                            listitem["WorkflowID"] = WorkflowID;

                        listitem.Update();
                        TaskID = listitem.ID;
                    }
                }
            });
            return base.Execute(executionContext);
        }

        public static bool GroupExists(SPGroupCollection groups, string name)
        {
            if (string.IsNullOrEmpty(name) ||
                (name.Length > 255) ||
                (groups == null) ||
                (groups.Count == 0))
                return false;
            else
            {
                foreach (SPGroup group in groups)
                {
                    if (group.Name == name)
                        return true;
                }
            }
            return false;
        }

        public static SPUser UserExists(SPUserCollection users, string name)
        {
            SPUser result = null;

            /*if (string.IsNullOrEmpty(name) ||
                (name.Length > 255) ||
                (users == null) ||
                (users.Count == 0))
                return false;
            else
            {*/
            foreach (SPUser user in users)
            {
                if ((user.Name == name) | (user.LoginName == name))
                    result = user;
            }

            //  }
            return result;
        }
    }
}
