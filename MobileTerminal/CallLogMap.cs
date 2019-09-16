using CsvHelper.Configuration;

namespace VerticalHandoverPrediction.Mobile
{
    public class CallLogMap: ClassMap<CallLog>
    {
        public CallLogMap()
        {
            Map(x => x.UserId);
            Map(x => x.SessionId);
            Map(x => x.Duration);
            Map(x => x.SessionSequence);
        }
    }   
}