using System.Collections.Generic;

namespace Handoverprediction.VerticalHandover
{
    public class User
    {
        public int UserId { get; set; }
        public List<Call> CallLogs { get; set; }
    }
}
