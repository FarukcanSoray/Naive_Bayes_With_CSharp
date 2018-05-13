using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Nuve;
using Nuve.Lang;
using Nuve.Morphologic.Structure;
using Functions;
using System.Text;
namespace TrueNaiveBayes
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> sayi = new List<int>() { 5, 7, 25, 100 };
            Console.WriteLine(Likelihood.two_pass_variance(sayi));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //------------
            HashSet<string> classifierGrams = new HashSet<string>();
            List<Document> documents = new List<Document>();

            prepareDocuments(documents, classifierGrams);
            Tuple<List<Document>, List<Document>> datas = Functions.Likelihood.splitTrainingTestData(documents);

            //------------

            //      foreach (string cc in classifierGrams){ Console.WriteLine(cc); }
            //       Console.WriteLine(classifierGrams.Count);
            /*  foreach (Document ccc in documents)
              {
                   Console.WriteLine(ccc.className);

              }*/

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


            Parallel.ForEach(list, i =>
            {
                StreamReader sr = new StreamReader(i, Encoding.GetEncoding("ISO-8859-9"));
                string cleanData = Functions.PreProcessing.editFile(i, sr, sr2, tr, seperators, stopWords);
                Dictionary<string, int> grams = Functions.CreateGramsFrequencies.make2Gram(cleanData).Concat(Functions.CreateGramsFrequencies.make3gram(cleanData)).ToDictionary(e => e.Key, e => e.Value);//2gram ile 3 grami birlestir
                documents.Add(new Document(Path.GetFileNameWithoutExtension(i), System.IO.Directory.GetParent(i).Name, grams));
                foreach (KeyValuePair<string, int> ix in grams)
                    if (ix.Value >= 50)
                    {
                        if (!classifierGrams.Contains(ix.Key))
                            classifierGrams.Add(ix.Key);
                    }
                sr.Close();
            });
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

