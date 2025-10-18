using System;

namespace VerticalHandoverPrediction.Models
{
    public sealed class MobileTerminal
    {
        public Guid MobileTerminalId { get; }
        public MobileTerminalState State { get; }
        public bool Activated { get; }
        public Session Session { get; }

        private MobileTerminal()
        {
            MobileTerminalId = Guid.NewGuid();
            State = MobileTerminalState.Idle;
            Activated = false;
        }

        public static MobileTerminal[] GenerateMobileTerminals(int count)
        {
            var terminals = new MobileTerminal[count];

            for (int i = 0; i < count; i++)
                terminals[i] = new MobileTerminal();
            
            return terminals;
        }
    }
}