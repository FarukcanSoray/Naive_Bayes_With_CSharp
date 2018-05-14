using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueNaiveBayes
{
    class Document
    {
        private readonly string _documentName;
        private readonly string _class;
        private readonly Dictionary<string, int> _gramsFrequencies;

        public string documentName { get { return _documentName; } }
        public string className { get { return _class; } }

        public Dictionary<string, int> gramsFrequencies { get { return _gramsFrequencies; } }

        public Document(string name, string className, Dictionary<string, int> gramFreq)
        {
            _documentName = name;
            _class = className;
            _gramsFrequencies = gramFreq;
        }


    }
}
