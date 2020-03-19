using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vigilante
{
    public static class Variables
    {
        public static string GrabVariable(string URL, string VariableName, string VariablePassword, string UserAgent = "Mozilla Firefox")
        {
            if (!Encryption.SessionStarted)
            {
                MessageBox.Show($"No session has been started!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                return "[No Session]";
            }

            using (var client = new WebClient { Proxy = null })
            {
                try
                {
                    client.Headers["User-Agent"] = UserAgent;

                    return Encryption.Decrypt(Encoding.Default.GetString(client.UploadValues(URL, new NameValueCollection
                    {
                        ["session_id"] = Encryption.Key,
                        ["request_id"] = Encryption.IV,
                        ["varname"] = VariableName,
                        ["varpass"] = VariablePassword,
                    })));
                }
                catch (Exception ex)
                {
                    if (ex.GetType().IsAssignableFrom(typeof(ArgumentNullException))) // If decryption method fails
                    {
                        MessageBox.Show($"Error while decrypting the response! Please try again later.", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                        return "[Decryption Error]";
                    }

                    MessageBox.Show("An error occured while completing the request!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "[Unknown Error]";
                }


            }
        }
    }
}
