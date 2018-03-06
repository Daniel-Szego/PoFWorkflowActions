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
    public class PoFPermissions : Activity
    {

        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(PoFPermissions));
        public static DependencyProperty ListeProperty = DependencyProperty.Register("Liste", typeof(string), typeof(PoFPermissions));
        public static DependencyProperty ElementIDProperty = DependencyProperty.Register("ElementID", typeof(int), typeof(PoFPermissions));

        #region Eigenschaften

        [Description("Url der Bibliothek")]
        [Category("PoF")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Url
        {
            get
            {
                return ((string)(base.GetValue(PoFPermissions.UrlProperty)));
            }
            set
            {
                base.SetValue(PoFPermissions.UrlProperty, value);
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
                return ((string)(base.GetValue(PoFPermissions.ListeProperty)));
            }
            set
            {
                base.SetValue(PoFPermissions.ListeProperty, value);
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
                return ((int)(base.GetValue(PoFPermissions.ElementIDProperty)));
            }
            set
            {
                base.SetValue(PoFPermissions.ElementIDProperty, value);
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
                        SPList list = web.Lists[Liste.ToString()];
                        SPListItem listitem = list.Items.GetItemById(ElementID);

                        if (listitem != null)
                        {
                            if (!listitem.HasUniqueRoleAssignments)
                                listitem.BreakRoleInheritance(true);

                            for (int i = listitem.RoleAssignments.Count - 1; i >= 0; i--)
                            {
                                listitem.RoleAssignments.Remove(i);
                            }

                            listitem.Update();
                        }
                    }
                }
            });
            return base.Execute(executionContext);
        }
    }
}
