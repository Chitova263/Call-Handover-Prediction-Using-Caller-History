using CsvHelper.Configuration;

namespace VerticalHandoverPrediction.Simulator
{
    public class EffectOfCallsOnVHO
    {
        public int CallsGenerated { get; set; }
        public int PredictiveSchemeNumOfVHO { get; set; }
        public int NonPredictiveSchemeNumOfVHO { get; set; }
        public int VerticalHandoversAvoided => NonPredictiveSchemeNumOfVHO - PredictiveSchemeNumOfVHO;
    }

    public class EffectOfCallsOnVHOMap : ClassMap<EffectOfCallsOnVHO>
    {
        public EffectOfCallsOnVHOMap()
        {
            AutoMap();
        }
    }
}