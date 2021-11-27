using System;
using System.IO;
using System.Net;
namespace Wiki_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            string link = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";
            Console.WriteLine(HTMLReader.Code(link));
        }
    }
    public class Web
    {

        public static string Code(string Url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }
    }
}
