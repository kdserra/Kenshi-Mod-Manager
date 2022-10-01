using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    public static class Utilities
    {
        public static async Task WaitUntil(Func<bool> predicate, int sleep = 50)
        {
            while (!predicate())
            {
                await Task.Delay(sleep);
            }
        }

        public async static Task<Image> GetImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(url))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        public async static Task<Image> GetSteamWorkshopThumbnail(string id)
        {
            try
            {
                string steamCommunityFileIDURL = "https://steamcommunity.com/sharedfiles/filedetails/?id=";
                string url = steamCommunityFileIDURL + id;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                WebBrowser wb = new WebBrowser();
                wb.DocumentStream = response.GetResponseStream();
                wb.ScriptErrorsSuppressed = true;
                HtmlDocument doc = wb.Document;
                await Task.Delay(2000);

                //await GetImage(srcImageURL);
            }
            catch { return null; }
            return null;
        }
    }
}
