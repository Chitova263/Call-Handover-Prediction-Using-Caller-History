namespace VerticalHandoverPrediction
{
    public interface IJCAC
    {
        IHetNet HetNet { get; }
        
        bool AdmitCall(ICall call, IMobileTerminal mobileTerminal);
    }
}