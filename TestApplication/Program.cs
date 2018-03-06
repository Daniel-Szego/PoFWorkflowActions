using PoFWorkflowActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Start --");
            try
            {
                string _Url = args[0];
                string _Liste = args[1];
                int _ElementID = int.Parse(args[2]);
                string WASName = args[3];
                PoFGeneratePDF activityToTest = new PoFGeneratePDF();
                activityToTest.TestPDF(_Url, _Liste, _ElementID, WASName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + e.Source + " " + e.StackTrace.ToString() + " ");
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message + " " + e.InnerException.StackTrace.ToString());
                }
            }

            Console.WriteLine("-- End --");
            Console.ReadLine();
        }
    }
}
