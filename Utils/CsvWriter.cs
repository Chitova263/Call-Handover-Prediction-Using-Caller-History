using System;
using System.IO;
using CsvHelper;
using VerticalHandoverPrediction.Mobile;

namespace VerticalHandoverPrediction.Utils
{
    public sealed class Writer
    {
        public CsvWriter CsvWriter { get; }
        private Writer()
        {
            var str = @"/Users/DjMadd/Documents/Thesis/VerticalHandoverPrediction";        
            var writer = new StreamWriter($"{str}/jkby.csv");
            CsvWriter = new CsvWriter(writer);
            CsvWriter.Configuration.Delimiter = ";";
            CsvWriter.Configuration.HasHeaderRecord = true;
            CsvWriter.Configuration.AutoMap<CallLog>();
            CsvWriter.WriteHeader<CallLog>();
        }
        private static Writer instance = null;
        private static readonly object padlock = new object();
        public static Writer _Writer
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Writer();
                    }
                    return instance;
                }
            }
        }
    }
}