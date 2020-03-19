using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vigilante;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // End-To-End encryption example
            Encryption.StartSession();

            SecureWebClient.SendEncrypteValues("https://yourdomain.ltd", new NameValueCollection 
            {
                ["example"]     = Encryption.Encrypt("data"),
                ["example2"]    = Encryption.Encrypt("data")
            }, "CUSTOM_USERAGENT");

            Encryption.EndSession();

            // Grab server-sided variables
            string variable = Variables.GrabVariable("https://yourdomain.ltd", "ExampleName", "YourPassword", "CUSTOM_USERAGENT");

            // Some of the improved WebClient methods
            SecureWebClient.DownloadString("https://thirdpartydomain.ltd"/*, "useragent"*/);
            SecureWebClient.DownloadData("https://thirdpartydomain.ltd"/*, "useragent"*/);
            SecureWebClient.UploadValues("https://thirdpartydomain.ltd", new NameValueCollection { ["example"] = "data" }/*, "useragent"*/);
        }
    }
}
