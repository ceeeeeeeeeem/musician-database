using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service.Extensions
{
    public static class StringExtensions
    {
        public static string StylizeBandName(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Split the input into words
            string[] words = input.Split(' ');

            // Stylize each word
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    // Convert the first character to uppercase and the rest to lowercase
                    words[i] = words[i][0].ToString().ToUpper() + words[i].Substring(1).ToLower();
                }
            }

            // Join the stylized words back into a single string
            return string.Join(" ", words);
        }
    }
}
