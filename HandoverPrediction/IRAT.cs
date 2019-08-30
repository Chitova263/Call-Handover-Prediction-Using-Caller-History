namespace HandoverPrediction
{
    public interface IRAT
    {
        //Admits new call
        bool AdmitNewCall(Call call);
        //Gets the available network capacity
        int AvailableCapacity();          
    }
}