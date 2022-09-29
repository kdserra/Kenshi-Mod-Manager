using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Kenshi_Mod_Manager
{
    public static class Utilities
    {
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
    }
}
