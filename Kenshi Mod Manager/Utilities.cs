using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    public static class Utilities
    {
        public static string[] GetFiles(string path, Regex regex, SearchOption searchOption)
        {
            string[] allFilePaths = Directory.GetFiles(path, ".", searchOption);
            List<string> results = new List<string>() { };
            foreach (string file in allFilePaths)
            {
                if (!regex.IsMatch(file)) { continue; }
                results.Add(file);
            }
            return results.ToArray();
        }

        public static async Task WaitUntil(Func<bool> predicate, int sleep = 50)
        {
            while (!predicate())
            {
                await Task.Delay(sleep);
            }
        }

        public static Image GetImage(string url)
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
                await Utilities.WaitUntil(() => doc.ActiveElement != null);
                Regex workshopItemPreviewImageMainFilter = new Regex(@"(?<=workshopItemPreviewImageMain src="")https:\/\/steamuserimages-a.akamaihd.net\/ugc\/[0-9]*\/\w+\/");
                Regex previewImageMainFilter = new Regex(@"(?<=workshopItemPreviewImageEnlargeable src="")https:\/\/steamuserimages-a.akamaihd.net/ugc/[0-9]+/\w+/");
                string imageURL = workshopItemPreviewImageMainFilter.Match(doc.ActiveElement.OuterHtml).ToString();
                if (imageURL == "") { imageURL = previewImageMainFilter.Match(doc.ActiveElement.OuterHtml).ToString(); }
                if (imageURL == "") { imageURL = "https://community.cloudflare.steamstatic.com/public/images/sharedfiles/steam_workshop_default_image.png"; }
                Image image = Utilities.GetImage(imageURL);
                return image;
            }
            catch { return null; }
        }
    }
}
