using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaiveBayes;



namespace Functions
{
    class Test
    {
        public static Dictionary<string, Tuple<double, double, double>> test(List<Document> testDatas, Dictionary<Tuple<string, string>, Tuple<double, double>> meanAndVariances, HashSet<string> classifierGrams)
        {
            List<string> classes = new List<string>() { "ekonomi", "magazin", "saglik", "siyasi", "spor" };
            int[,] confusionMatrix = new int[5, 5];
            foreach (Document data in testDatas)
            {
                Dictionary<string, double> probabilities = new Dictionary<string, double>();
                foreach (string className in classes)
                {
                    List<double> bayesResults = new List<double>();
                    foreach (string gram in classifierGrams)
                    {
                        if (data.gramsFrequencies.ContainsKey(gram))
                            bayesResults.Add(bayesianFormula(meanAndVariances[Tuple.Create(gram, className)], data.gramsFrequencies[gram]));//add to bayesResults
                        else
                            bayesResults.Add(bayesianFormula(meanAndVariances[Tuple.Create(gram, className)], 0));//add to bayesResults

                    }
                    double probability = 0;
                    //  Double.
                    foreach (double result in bayesResults)
                    {

                        if (result != 0 && !double.IsNaN(result))
                        {
                            probability += Math.Log(result);
                        }

                    }
                    probabilities.Add(className, probability);
                }
                string max = probabilities.OrderByDescending(x => x.Value).First().Key;
                confusionMatrix[classes.IndexOf(data.className), classes.IndexOf(max)]++;

                //Console.WriteLine(max + " " + data.className);
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(string.Format("{0} ", confusionMatrix[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            return calculateFMeasure_Recall_Precision(confusionMatrix);

        }

        public static double bayesianFormula(Tuple<double, double> meanAndVariance, int testFrequency)
        {
            return (1 / Math.Sqrt(2 * Math.PI * meanAndVariance.Item2)) * (Math.Pow(Math.E, -(Math.Pow(testFrequency - meanAndVariance.Item1, 2) / (2 * meanAndVariance.Item2))));
        }

        public static Dictionary<string, Tuple<double, double, double>> calculateFMeasure_Recall_Precision(int[,] confusionMatrix)
        {
            List<int> totalPredicts = new List<int>(5);
            List<int> totalActuals = new List<int>(5);
            List<int> truePositives = new List<int>(5);
            List<string> classes = new List<string>() { "ekonomi", "magazin", "saglik", "siyasi", "spor" };
            Dictionary<string, Tuple<double, double, double>> valuesForEachClass = new Dictionary<string, Tuple<double, double, double>>();
            for (int i = 0; i < 5; ++i)
            {
                int totalPredicted = 0;
                for (int j = 0; j < 5; ++j)
                {
                    totalPredicted += confusionMatrix[i, j];
                }
                totalPredicts.Add(totalPredicted);
            }

            for (int i = 0; i < 5; ++i)
            {
                int totalActual = 0;
                for (int j = 0; j < 5; ++j)
                {
                    totalActual += confusionMatrix[j, i];
                }
                totalActuals.Add(totalActual);
            }

            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (i == j)
                        truePositives.Add(confusionMatrix[i, j]);
                }
            }

            for (int i = 0; i < 5; ++i)
            {
                double precision = (double)truePositives.ElementAt(i) / totalPredicts.ElementAt(i);
                double recall = (double)truePositives.ElementAt(i) / totalActuals.ElementAt(i);
                double fmeasure = 2 * precision * recall / (precision + recall);
                valuesForEachClass.Add(classes.ElementAt(i), Tuple.Create(fmeasure, recall, precision));
            }

            foreach (var i in valuesForEachClass)
            {
                Console.WriteLine(i.Key + " " + i.Value);
            }

            return valuesForEachClass;
        }

    }
}
