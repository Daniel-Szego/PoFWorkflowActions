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
    public class PoFReplace : Activity
    {
        public static DependencyProperty InStringProperty = DependencyProperty.Register("InString", typeof(string), typeof(PoFReplace));

        [Description("InString")]
        [Category("Eingabezeichenfolge")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string InString
        {
            get
            {
                return ((string)(base.GetValue(PoFReplace.InStringProperty)));
            }
            set
            {
                base.SetValue(PoFReplace.InStringProperty, value);
            }
        }

        public static DependencyProperty SearchStringProperty = DependencyProperty.Register("SearchString", typeof(string), typeof(PoFReplace));

        [Description("ReplaceString")]
        [Category("Zeichenfolge die ersetzt werden soll")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SearchString
        {
            get
            {
                return ((string)(base.GetValue(PoFReplace.SearchStringProperty)));
            }
            set
            {
                base.SetValue(PoFReplace.SearchStringProperty, value);
            }
        }

        public static DependencyProperty ReplaceStringProperty = DependencyProperty.Register("ReplaceString", typeof(string), typeof(PoFReplace));

        [Description("Replace")]
        [Category("Eingebezeichenfolge durch die ersetzt werden soll")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ReplaceString
        {
            get
            {
                return ((string)(base.GetValue(PoFReplace.ReplaceStringProperty)));
            }
            set
            {
                base.SetValue(PoFReplace.ReplaceStringProperty, value);
            }
        }

        public static DependencyProperty OutStringProperty = DependencyProperty.Register("OutString", typeof(string), typeof(PoFReplace));

        [Description("OutString")]
        [Category("Ausgabewert")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string OutString
        {
            get
            {
                return ((string)(base.GetValue(PoFReplace.OutStringProperty)));
            }
            set
            {
                base.SetValue(PoFReplace.OutStringProperty, value);
            }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (!String.IsNullOrEmpty(InString) & !String.IsNullOrEmpty(SearchString))
            {
                OutString = InString.Replace(SearchString, ReplaceString);
            }
            else
                OutString = InString;

            return base.Execute(executionContext);
        }
    }
}
