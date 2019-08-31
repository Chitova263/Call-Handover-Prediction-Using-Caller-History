using System;

namespace VerticalHandoverPrediction
{

    public class JCAC : IJCAC
    {
        public IHetNet HetNet { get; private set; }

        private JCAC(IHetNet hetNet)
        {
            this.HetNet = hetNet ?? throw new System.ArgumentNullException(nameof(HetNet));
        }

        public static JCAC Initialize(IHetNet hetNet)
        {
            var network = new JCAC(hetNet);
            return network;
        }

        public bool AdmitCall(ICall call, IMobileTerminal mobileTerminal)
        {
            throw new NotImplementedException();
        }

        //Implement JCAC algorithm
        /*
            takes in a call => if call is on 
         */


    }
}