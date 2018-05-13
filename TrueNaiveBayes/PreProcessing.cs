using Nuve.Lang;
using Nuve.Morphologic.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Functions
{
    public static class PreProcessing
    {
        public static string editFile(string file_name, StreamReader sr, StreamReader sr2, Language tr, char[] seperators, string[] stopWords)
        {
            String file = sr.ReadToEnd();
            file = StripPunctuation(file);//noktalamalari sil
            file = file.ToLower();//küçük harfe çevir

            foreach (string stopWord in stopWords)
            {
                file = file.Replace(stopWord, "");//Stop word'leri sil
            }

            string[] words = file.Split(seperators, StringSplitOptions.RemoveEmptyEntries);//Whitspaceden arinidir//China's broke again

            for (int i = 0; i < words.GetLength(0); ++i)
            {
                IList<Word> solutions = tr.Analyze(words[i]);
                if (solutions.Any())//kelimenin gövdesi varsa
                {
                    words[i] = solutions[0].GetStem().GetSurface();//Kelimenin yerine gövde halini koy
                }
            }
            // file = String.Join(" ", words);


            file = String.Join("_", words);//kelimeleri _ ile birlestirerek bütün haline getir//And it's whole again

            // Console.WriteLine(file);
            return file;
        }


        public static string StripPunctuation(this string s)
        {
            string cleanString = Regex.Replace(s, "[^A-Za-z0-9güsöçIGÜSÖÇi ]", "");
            return cleanString;
        }

    }
}

