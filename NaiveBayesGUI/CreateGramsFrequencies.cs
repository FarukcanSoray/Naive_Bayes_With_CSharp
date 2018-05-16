using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    class CreateGramsFrequencies
    {
        public static Dictionary<string, int> make2Gram(string file)
        {
            Dictionary<string, int> grams = new Dictionary<string, int>();
            for (int i = 0; i < file.Length - 2; ++i)
            {
                if (!grams.ContainsKey(file.Substring(i, 2)))
                    grams.Add(file.Substring(i, 2), 1);
                else
                    grams[file.Substring(i, 2)] += 1;
            }

            return grams;
        }

        public static Dictionary<string, int> make3gram(string file)
        {
            Dictionary<string, int> grams = new Dictionary<string, int>();
            for (int i = 0; i < file.Length - 3; ++i)
            {
                if (!grams.ContainsKey(file.Substring(i, 3)))
                    grams.Add(file.Substring(i, 3), 1);
                else
                    grams[file.Substring(i, 3)] += 1;
            }

            return grams;
        }
    }
}
