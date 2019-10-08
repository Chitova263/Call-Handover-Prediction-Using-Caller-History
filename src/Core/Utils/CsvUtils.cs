namespace VerticalHandoverPrediction.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;
    using System.Linq;
    using System;
    using System.Text;
    using CsvHelper.Configuration;
    public sealed class CsvUtils
    {
        private static CsvUtils instance = null;
        private static readonly object _locker = new object();

        private CsvUtils()
        {
            
        }
        public static CsvUtils _Instance
        {
            get
            {
                lock (_locker)
                {
                    if (instance == null)
                    {
                        instance = new CsvUtils();
                    }
                    return instance;
                }
            }
        }

        public void Write<TMap,TRecord>(TRecord record, string filename) 
            where TMap : ClassMap
            where TRecord : class
        {
            try
            {
                using (var writer = new StreamWriter(filename, true ))
                using (var reader = new StreamReader(filename))
                using (var csvWriter = new CsvWriter(writer))
                using (var csvReader = new CsvReader(reader))
                {
                    csvReader.Read();
                    csvReader.ReadHeader();
                    csvWriter.Configuration.Delimiter =",";
                    csvWriter.Configuration.RegisterClassMap<TMap>();
                    csvWriter.WriteRecord<TRecord>(record);
                    csvWriter.NextRecord();
                }
            }
            catch (CsvHelper.ReaderException)
            {
                using (var writer = new StreamWriter(filename, true))
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.Configuration.Delimiter =",";
                    csvWriter.Configuration.RegisterClassMap<TMap>();
                    csvWriter.WriteHeader<TRecord>();
                    csvWriter.NextRecord();
                    csvWriter.WriteRecord(record);
                    csvWriter.NextRecord();
                }
            }
        }

        public IEnumerable<Tout> Read<TMap,Tout>(string filename) 
            where TMap : ClassMap
            where Tout : class
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