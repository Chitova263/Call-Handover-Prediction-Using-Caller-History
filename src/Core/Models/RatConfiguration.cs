namespace VerticalHandoverPrediction
{
    public sealed class RatConfiguration
    {
        public int Capacity { get; set; }
        public string Name { get; set; }
        public Service SupportedServices { get; set; }
        public int Priority { get; set; }

        public RatConfiguration()
        {

        }
        
       
    }
}
