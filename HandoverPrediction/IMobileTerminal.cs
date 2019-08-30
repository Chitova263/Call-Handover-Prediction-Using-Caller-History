namespace HandoverPrediction
{
    public interface IMobileTerminal
    {
        MobileTerminalState SetMobileTerminalState(MobileTerminalState state);
        MobileTerminalState ComputeMobileTerminalCurrentState(Service service);
    }
}