using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APBD.Utils
{
    public class WriteToFIle
    {

        public static void Write(ArrayList lines)
        {
       
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter writter = File.AppendText(Path.Combine(docPath, "requestsLog.txt")))
            {
                foreach (string line in lines)
                    writter.WriteLine(line);
            }
        }
    }
}
