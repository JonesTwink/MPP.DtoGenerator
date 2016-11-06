using System;
using System.Configuration;

namespace DtoClassGeneratorLibrary
{
    internal class GeneratorSettings
    {
        private int _maxThreadsAmount = 0;
        private string _generatedClassNamespace = ConfigurationManager.AppSettings["generatedClassNamespace"];

        public int MaxThreadsAmount
        {
            get
            {
                if (_maxThreadsAmount == 0)
                {
                    return 1;
                }
                else
                {
                    return _maxThreadsAmount;
                }
            }
            set
            {
            }
        }

        public string GeneratedClassNamespace
        {
            get
            {
                if (_generatedClassNamespace == String.Empty)
                {
                    return "DefaultNamespace";
                }
                else
                {
                    return _generatedClassNamespace;
                }                       
            }

            set
            {
            }
        }
        internal GeneratorSettings()
        {
            Int32.TryParse(ConfigurationManager.AppSettings["maxThreadAmount"], out _maxThreadsAmount);
        }

    }
}
