namespace VerticalHandoverPrediction.Utils
{
    using System;
    using Newtonsoft.Json;
    public static class JsonDumper
    {
        public static void Dump(this object obj)
        {
            System.Console.WriteLine(obj.GetType());
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
