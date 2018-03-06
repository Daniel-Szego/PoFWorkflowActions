using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.ComponentModel;
using System;


namespace PoFWorkflowActions
{
    public class PoFActualizeProjectTaskExt : Activity
    {
        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(PoFActualizeProjectTaskExt));
        public static DependencyProperty TaskIDProperty = DependencyProperty.Register("TaskID", typeof(int), typeof(PoFActualizeProjectTaskExt));
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(PoFActualizeProjectTaskExt));
        public static DependencyProperty DueDateProperty = DependencyProperty.Register("DueDate", typeof(DateTime), typeof(PoFActualizeProjectTaskExt));
        public static DependencyProperty QuelleProperty = DependencyProperty.Register("Quelle", typeof(string), typeof(PoFActualizeProjectTaskExt));
        public static DependencyProperty WorkflowIDProperty = DependencyProperty.Register("WorkflowID", typeof(int), typeof(PoFActualizeProjectTaskExt));

        #region Eigenschaften

        [Description("Url der Aufgabenliste")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Url
        {
            get
            {
                return ((string)(base.GetValue(PoFActualizeProjectTaskExt.UrlProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTaskExt.UrlProperty, value);
            }
        }

        [Description("TaskID")]
        [Category("TaskID Category")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int TaskID
        {
            get
            {
                return ((int)(base.GetValue(PoFActualizeProjectTaskExt.TaskIDProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTaskExt.TaskIDProperty, value);
            }
        }

        [Description("Status")]
        [Category("Status Category")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(PoFActualizeProjectTaskExt.StatusProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTaskExt.StatusProperty, value);
            }
        }

        [Description("Zieltermin")]
        [Category("Zieltermin Category")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime DueDate
        {
            get
            {
                return ((DateTime)(base.GetValue(PoFActualizeProjectTaskExt.DueDateProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTaskExt.DueDateProperty, value);
            }
        }

        [Description("Quelle")]
        [Category("Quelle Category")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Quelle
        {
            get
            {
                return ((string)(base.GetValue(PoFActualizeProjectTaskExt.QuelleProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTaskExt.QuelleProperty, value);
            }
        }

        [Description("WorkflowID")]
        [Category("WorkflowID Category")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int WorkflowID
        {
            get
            {
                return ((int)(base.GetValue(PoFActualizeProjectTaskExt.WorkflowIDProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTaskExt.WorkflowIDProperty, value);
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
                        SPListItem listitem = list.Items.GetItemById(TaskID);
                        if (listitem != null)
                        {
                            if (Status.ToString() != string.Empty)
                                listitem["Status"] = Status.ToString();
                            if (Quelle.ToString() != string.Empty)
                                listitem["Aufgabenquelle"] = Quelle.ToString();
                            if (DueDate.ToString() != string.Empty)
                                listitem["DueDate"] = DueDate;
                            listitem["WorkflowID"] = WorkflowID;

                            listitem.Update();
                        }
                    }
                }
            });
            return base.Execute(executionContext);
        }
    }
}
