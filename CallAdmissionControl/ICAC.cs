using VerticalHandoverPrediction.CallSession;

namespace VerticalHandoverPrediction.CallAdmissionControl
{
    public interface ICAC
    {
        void AdmitCall(ICall call);
    }
}