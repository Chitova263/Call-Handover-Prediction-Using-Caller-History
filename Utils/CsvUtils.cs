using System.Collections.Generic;
using System.IO;
using CsvHelper;
using VerticalHandoverPrediction.Mobile;
using MoreLinq.Extensions;
using System.Linq;
using System;
using System.Text;
using CsvHelper.Configuration;

namespace VerticalHandoverPrediction.Utils
{
    public sealed class CsvUtils
    {
        private static CsvUtils instance = null;
        private static readonly object padlock = new object();
        private List<string> Header { get; set; } = new List<string>();

        private CsvUtils()
        {
            
        }
        public static CsvUtils _Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CsvUtils();
                    }
                    return instance;
                }
            }
        }

        public void Write<TMap,TSource>(TSource log, string filename) where TMap: ClassMap
        {
            using (var writer = new StreamWriter(filename, true, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.Configuration.Delimiter =",";
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.RegisterClassMap<TMap>();

                if(!Header.Contains($"{log.GetType()}"))
                {
                    Header.Add($"{log.GetType()}");
                    csvWriter.WriteHeader<TSource>();
                    csvWriter.NextRecord();
                }

                csvWriter.WriteRecord(log);
                csvWriter.NextRecord();
            }
        }

        public IEnumerable<Tout> Read<TMap,Tout>(string filename) where TMap : ClassMap
        {
            try
            {
                using (var reader = new StreamReader(filename))
                using (var csvReader = new CsvReader(reader))
                {
                    csvReader.Configuration.HasHeaderRecord = true;
                    csvReader.Configuration.RegisterClassMap<TMap>();
                    return csvReader.GetRecords<Tout>().ToList();
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                return new List<Tout>();
            }   
        }

        public void Clear(string filename)
        {
            using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.WriteField(String.Empty);
            }
        }

    }
}