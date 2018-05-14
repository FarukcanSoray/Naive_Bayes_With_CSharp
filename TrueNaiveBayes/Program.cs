using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuve;
using Nuve.Lang;
using Functions;

namespace TrueNaiveBayes
{
    class Program
    {
        static void Main(string[] args)
        {
          
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //------------
            HashSet<string> classifierGrams = new HashSet<string>();
            List<Document> documents = new List<Document>();

            prepareDocuments(documents, classifierGrams);
            Tuple<List<Document>, List<Document>> datas = Likelihood.splitTrainingTestData(documents);
            Console.WriteLine(classifierGrams.Count);
            Likelihood.training(datas.Item1, classifierGrams);
            //------------

            stopwatch.Stop();
            var elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine(elapsed_time);
        }

        static void prepareDocuments(List<Document> documents, HashSet<string> classifierGrams)
        {
            var filenames = from fullFilename
               in Directory.EnumerateFiles("raw_texts/", "*.txt", SearchOption.AllDirectories)
                            select Path.GetFullPath(fullFilename);
            List<string> list = filenames.ToList();
            StreamReader sr2 = new StreamReader("stop_words.txt", Encoding.GetEncoding("ISO-8859-9"));
            Language tr = LanguageFactory.Create(LanguageType.Turkish);
            char[] seperators = { ' ', '\n', '\t', '\r', '\0' };
            string[] stopWords = sr2.ReadToEnd().Split('\n');
            Dictionary<string, int> allGrams = new Dictionary<string, int>();

            Parallel.ForEach(list, i =>
            {
                StreamReader sr = new StreamReader(i, Encoding.GetEncoding("ISO-8859-9"));
                string cleanData = PreProcessing.editFile(i, sr, sr2, tr, seperators, stopWords);
                Dictionary<string, int> grams = CreateGramsFrequencies.make2Gram(cleanData).Concat(CreateGramsFrequencies.make3gram(cleanData)).ToDictionary(e => e.Key, e => e.Value);//2gram ile 3 grami birlestir
                documents.Add(new Document(Path.GetFileNameWithoutExtension(i), Directory.GetParent(i).Name, grams));
                foreach (KeyValuePair<string, int> ix in grams)
                {
                    if (!allGrams.ContainsKey(ix.Key))
                        allGrams.Add(ix.Key, ix.Value);
                    else
                        allGrams[ix.Key] += ix.Value;
                }
                sr.Close();
            });

            foreach (KeyValuePair<string, int> gram in allGrams)
            {
                if (gram.Value >= 50)
                    if (!classifierGrams.Contains(gram.Key))
                    {
                        classifierGrams.Add(gram.Key);
                    }
            }
        }
    }
}


//   {
// string parent = System.IO.Directory.GetParent(i).Name;
//  string parent = Path.GetFileNameWithoutExtension(i);
//   Console.WriteLine(parent);
//editFile(i);
//        Console.WriteLine("------------------------------------------");
//    }

