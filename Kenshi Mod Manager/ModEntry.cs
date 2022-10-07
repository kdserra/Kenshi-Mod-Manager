using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Kenshi_Mod_Manager
{
    public class ModEntry
    {
        public readonly string Id;
        public readonly string DisplayName;
        public readonly string FileName;
        public readonly string ModInfoName;
        public readonly Image Image;
        public readonly string[] Categories;
        public readonly string KenshiFormatModFileName;

        public ModEntry(
            string id,
            string displayName,
            string fileName,
            string modInfoName,
            string kenshiFormatModFileName,
            Image image,
            string[] categories)
        {
            Id = id;
            DisplayName = displayName;
            FileName = fileName;
            ModInfoName = modInfoName;
            KenshiFormatModFileName = kenshiFormatModFileName;
            Image = image;
            Categories = categories;
        }

        public Tag[] GetModTags()
        {
            Tag[] tags = new Tag[Categories.Length];
            for (int i = 0; i < Categories.Length; i++)
            {
                string category = Categories[i];
                Tag? tag = Utilities.GetTagFromString(category);
                if (tag == null) { continue; }
                tags[i] = (Tag)tag;
            }
            return tags;
        }

        public string GetCategoriesString()
        {
            string output = "";
            if (Categories != null)
            {
                for (int i = 0; i < Categories.Length; i++)
                {
                    string category = Categories[i];
                    if (i > 0) { output += ", "; }
                    output += category;
                }
            }
            return output;
        }
    }
}
