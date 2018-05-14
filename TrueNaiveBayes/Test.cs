using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueNaiveBayes;


namespace Functions
{
    class Test
    {
        public static void test(List<Document> testDatas, Dictionary<Tuple<string, string>, Tuple<double, double>> meanAndVariances, HashSet<string> classifierGrams)
        {
            HashSet<string> classes = new HashSet<string>() { "ekonomi", "magazin", "saglik", "siyasi", "spor" };
            //List<double> 
            foreach(Document data in testDatas)
            {
                Dictionary<string, double> probabilities = new Dictionary<string, double>();
                foreach(string className in classes)
                {
                    List<double> bayesResults = new List<double>();
                    foreach (string gram in classifierGrams)
                    {
                        bayesResults.Add(bayesianFormula(meanAndVariances[new Tuple<string, string>(gram, className)], data.gramsFrequencies[gram]));//add to bayesResults
                    }
                    double probability = 1;
                    foreach (double result in bayesResults)
                        probability *= result;
                    probabilities.Add(className, probability);
                }
                var max = from x in probabilities where x.Value == probabilities.Max(v => v.Value) select x.Key;
                Console.WriteLine(max);
            }
        }

        public static double bayesianFormula(Tuple<double, double> meanAndVariance, int testFrequency)
        {
            return (1/Math.Sqrt(2*Math.PI*meanAndVariance.Item2))*Math.Pow(Math.E, -(Math.Pow(testFrequency - meanAndVariance.Item1, 2)/(2*meanAndVariance.Item2)));
        }
    }
}
