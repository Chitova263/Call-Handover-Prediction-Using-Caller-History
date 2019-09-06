using VerticalHandoverPrediction.CallSession;

//Converted to a singleton
namespace VerticalHandoverPrediction.CallAdmissionControl
{
    public interface ICAC
    {
        bool AdmitCall(ICall call);
    }
}
