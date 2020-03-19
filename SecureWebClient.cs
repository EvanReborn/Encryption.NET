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
    public class SecureWebClient
    {
        public static string DownloadString(string URL, string UserAgent = "Mozilla Firefox")
        {
            using (WebClient client = new WebClient { Proxy = null })
            {
                try
                {
                    client.Headers["User-Agent"] = UserAgent;
                    return client.DownloadString(URL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while completing the request!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                    return ex.Message;
                }
            }
        }

        public static byte[] DownloadData(string URL, string UserAgent = "Mozilla Firefox")
        {
            using (WebClient client = new WebClient { Proxy = null })
            {
                try
                {
                    client.Headers["User-Agent"] = UserAgent;
                    return client.DownloadData(URL);
                }
                catch
                {
                    MessageBox.Show("An error occured while completing the request!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        public static bool DownloadFile(string URL, string Path, string UserAgent = "Mozilla Firefox")
        {
            using (WebClient client = new WebClient { Proxy = null })
            {
                try
                {
                    client.Headers["User-Agent"] = UserAgent;
                    client.DownloadFile(URL, Path);
                    return true;
                }
                catch
                {
                    MessageBox.Show("An error occured while completing the request!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static string UploadValues(string URL, NameValueCollection Values, string UserAgent = "Mozilla Firefox")
        {
            using (WebClient client = new WebClient { Proxy = null })
            {
                try
                {
                    client.Headers["User-Agent"] = UserAgent;
                    return Encoding.Default.GetString(client.UploadValues(URL, Values));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while completing the request!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                    return ex.Message;
                }
            }
        }

        public static string SendEncrypteValues(string URL, NameValueCollection Values, string UserAgent)
        {
            if (!Encryption.SessionStarted)
            {
                MessageBox.Show($"No session has been started!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                return "No session started!";
            }

            using (var client = new WebClient { Proxy = null })
            {
                try
                {
                    client.Headers["User-Agent"] = UserAgent;

                    Values.Add(new NameValueCollection
                    {
                        ["session_id"] = Encryption.Key,
                        ["request_id"] = Encryption.IV
                    });

                    return Encryption.Decrypt(Encoding.Default.GetString(client.UploadValues(URL, Values)));
                }
                catch (Exception ex)
                {
                    if (ex.GetType().IsAssignableFrom(typeof(ArgumentNullException))) // If decryption method fails
                    {
                        MessageBox.Show($"Error while decrypting the response! Please try again later.", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                        return "Encryption Error!";
                    }

                    MessageBox.Show("An error occured while completing the request!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "Unknown Error!";
                }


            }
        }
    }
}
