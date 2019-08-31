namespace VerticalHandoverPrediction
{
    public interface IJCAC
    {
        IHetNet HetNet { get; }
            
        JCAC AdmitCall(ICall call, IMobileTerminal mobileTerminal);
    }
}