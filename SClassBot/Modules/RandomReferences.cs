using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SClassBot.Modules
{
    public static class RandomReferences
    {
        private static readonly Random random = new Random();

        //Assortment of colors the SClass way
        private static readonly uint[] colors = {0xE629B2, 0xB0A8F5, 0xCEFFB6, 0xE2EB5F, 0xEBBC5F, 0xDF6949,
            0xE92222, 0x76C2BD, 0xA24EBE, 0xE20687};
        
        private static Dictionary<string, string[]> fileLinks;

        /*
         Will initialize during boot and also during s$gif call
         Searches a defined directory for text files containing links to different gifs on a website.
         Sorts them into a Dictionary with a key being the file name minus path and extension, and the values
         being a string array of links.
         
         This is used when a social command requires a random gif that relates to the command.         
         */
        public static void InitializeFiles()
        {
            fileLinks = new Dictionary<string, string[]>();
            const string directory = "Resources\\images";

            foreach (var f in Directory.EnumerateFiles(directory))
            {
                fileLinks.Add(f.Replace(".txt", "").Replace("Resources\\images\\", ""), File.ReadAllLines(f));
            }
        }

        public static string NewGif(string type)
        {
            var list = fileLinks[type.ToLower()].ToList();
            
            return list[random.Next(list.Count)];
        }

        public static uint NewColor()
        {
            return colors[random.Next(colors.Length)];
        }
    }
}