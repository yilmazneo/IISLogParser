using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IISLogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"C:\Program Files (x86)\Log Parser 2.2\LogParser.exe";

            //string arguments = @"""select cs-username,date,time,cs-method,cs-uri-stem,cs-uri-query,sc-status,c-ip from C:\temp\logs\{0} " +
                                    //"where cs-username='jayciwagner'  \" ";
            string arguments = @"""select cs-username,cs-uri-query from C:\temp\logs\ex\{0} ";
                                    //"where   \" ";
            // AND (cs-uri-stem ='/listapplications.aspx' OR cs-uri-stem='/ApplicationChecklist.aspx')
            string dir = @"C:\temp\logs\ex";
            StringBuilder sb = new StringBuilder();
            DirectoryInfo di = new DirectoryInfo(dir);
            foreach (var file in di.GetFiles())
            {
                p.StartInfo.Arguments = string.Format(arguments,file.Name);
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (output.Contains("cs-username"))
                {
                    sb.AppendLine(output);
                }
                Console.WriteLine(file.Name + " Is done");
            }
            Console.WriteLine("Done");
            File.AppendAllText(@"C:\temp\LOGOUTPUT.txt",sb.ToString());
            Console.ReadLine();
            
        }
    }
}
