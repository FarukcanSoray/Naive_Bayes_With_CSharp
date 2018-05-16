using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NaiveBayes;

namespace Functions
{
    class Likelihood
    {
        public static Tuple<List<Document>, List<Document>> splitTrainingTestData(List<Document> documents)
        {
            List<Document> testDatas = new List<Document>();
            Console.WriteLine(documents.Count);
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4];
                for (int i = 1; i <= 288; ++i)
                {
                    rng.GetBytes(data);

                    UInt32 value = BitConverter.ToUInt32(data, 0);
                    value %= (UInt32)documents.Count;
                    testDatas.Add(documents.ElementAt((int)value));
                    documents.RemoveAt((int)value);
                }
                Console.WriteLine(documents.Count);
                Console.WriteLine(testDatas.Count);

            }
            return Tuple.Create(documents, testDatas);
        }

        public static Tuple<double, double> two_pass_variance(List<int> sayilar)
        {
            int n = 0, sum1 = 0;
            double sum2 = 0.0;

            foreach (int i in sayilar)
            {
                n++;
                sum1 += i;
            }

            double mean = (double)sum1 / n;
            foreach (int i in sayilar)
                sum2 += (i - mean) * (i - mean);

            double variance = sum2 / (n - 1);

            return Tuple.Create(mean, variance);
        }

        public static Dictionary<Tuple<string, string>, Tuple<double, double>> training(List<Document> trainingData, HashSet<string> classifierGrams)
        {
            Dictionary<Tuple<string, string>, Tuple<double, double>> classifierXClass_MeanAndVariances = new Dictionary<Tuple<string, string>, Tuple<double, double>>();//<classifiergram,class><mean, variance>
            foreach (string gram in classifierGrams)
            {
                Dictionary<Tuple<string, string>, List<int>> gramCountInClass = new Dictionary<Tuple<string, string>, List<int>>();//tuple<gram, class>
                foreach (Document data in trainingData)
                {
                    if (!gramCountInClass.ContainsKey(new Tuple<string, string>(gram, data.className)))
                        gramCountInClass.Add(new Tuple<string, string>(gram, data.className), new List<int>());

                    if (data.gramsFrequencies.ContainsKey(gram))
                        gramCountInClass[new Tuple<string, string>(gram, data.className)].Add(data.gramsFrequencies[gram]);
                    else
                        gramCountInClass[new Tuple<string, string>(gram, data.className)].Add(0);
                }
                foreach (KeyValuePair<Tuple<string, string>, List<int>> i in gramCountInClass)
                {
                    classifierXClass_MeanAndVariances.Add(i.Key, two_pass_variance(i.Value));
                    //  Console.WriteLine(two_pass_variance(i.Value));
                }

            }
            //Console.WriteLine(classifierXClass_MeanAndVariances.Count);
            return classifierXClass_MeanAndVariances;
        }

    }
}
