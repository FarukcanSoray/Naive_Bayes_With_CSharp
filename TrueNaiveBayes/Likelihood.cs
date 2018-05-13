using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrueNaiveBayes;

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

        public static double two_pass_variance(List<int> sayilar)
        {
            int n = 0, sum1 = 0;
            double sum2 = 0.0;

            foreach (int i in sayilar)
            {
                n++;
                sum1 += i;
            }

            double mean = (double)sum1 / n;
            Console.WriteLine(mean);
            foreach (int i in sayilar)
                sum2 += (i - mean) * (i - mean);

            double variance = sum2 / (n - 1);

            return variance;
        }

        public static void training(List<Document> trainingData, HashSet<string> classifierGrams)
        {
            foreach (Document data in trainingData)
            {

            }
        }

    }
}
