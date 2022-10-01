﻿using System.Drawing;

namespace Kenshi_Mod_Manager
{
    public class ModEntry
    {
        public readonly string Name;
        public readonly string FileName;
        public readonly Image Image;
        public readonly string[] Categories;
        public readonly ModSource Source;

        public ModEntry(
            string name,
            string fileName,
            Image image,
            string[] categories,
            ModSource source)
        {
            Name = name;
            FileName = fileName;
            Image = image;
            Categories = categories;
            Source = source;
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
