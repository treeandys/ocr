using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.IO;

namespace OcrTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Keywords
            string[] keywords = new string[] { "Конвертируйте файлы TIF в текст", "попробуйте прямо сейчас", "Как работает сервис оптического распознавания?" };
            //  Keywords number
            int keywordsNumber = keywords.Length;
            //  Host name
            string host = "172.16.3.227";
            //  Port number
            int port = 2525;

            //  Request url
            string url = string.Format("http://{0}:{1}", host, port);

            //  Http web request class instance
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                //throw new Exception("Test throw exception");
                //throw new WebException("Test throw web exception");
                //this is the test
                //  Try to get response
                using (WebResponse response = request.GetResponse())
                {
                    //  Stream reader class instance for response stream
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        //  Get response raw data
                        string raw = reader.ReadToEnd();

                        //  Number of found keywords
                        int found = 0;
                        //  Search keywords
                        for (int i = 0; i < keywordsNumber; i++)
                        {
                            int index = raw.IndexOf(keywords[i]);
                            if (index > 0)
                                found++;
                        }
                        if (found == keywordsNumber)
                            WriteLog(string.Format("Found {0} of {1} keywords", found, keywords.Length), Status.Success);
                        else if (found > 0 && found != keywordsNumber)
                            WriteLog(string.Format("Found {0} of {1} keywords", found, keywords.Length), Status.Warning);
                        else
                            WriteLog("Keywords not found", Status.Error);
                    }
                }
            }
            catch(WebException wex)
            {
                WriteLog(wex.Message, Status.Error);
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message, Status.Error);
            }
        }

        //  Write log message
        static void WriteLog(string message, Status status)
        {
            //  Log file full path
            string logPath = "log.txt";
            //  Stream writer class instance
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine("{0} [{1}] {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), status, message);
            }
        }

        //  Status enumeration
        enum Status
        {
            Error = 1,
            Success = 2,
            Warning = 3
        }
    }
}
