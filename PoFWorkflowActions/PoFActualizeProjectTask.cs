using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace PoFWorkflowActions
{
    public class PoFActualizeProjectTask : Activity
    {

        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(PoFActualizeProjectTask));
        public static DependencyProperty TaskIDProperty = DependencyProperty.Register("TaskID", typeof(int), typeof(PoFActualizeProjectTask));
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(PoFActualizeProjectTask));

        #region Eigenschaften

        [Description("Url der Aufgabenliste")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Url
        {
            get
            {
                return ((string)(base.GetValue(PoFActualizeProjectTask.UrlProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTask.UrlProperty, value);
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
                return ((int)(base.GetValue(PoFActualizeProjectTask.TaskIDProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTask.TaskIDProperty, value);
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
                return ((string)(base.GetValue(PoFActualizeProjectTask.StatusProperty)));
            }
            set
            {
                base.SetValue(PoFActualizeProjectTask.StatusProperty, value);
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
                            listitem["Status"] = Status.ToString();
                            listitem.Update();
                        }
                    }
                }
            });
            return base.Execute(executionContext);
        }
    }
}
