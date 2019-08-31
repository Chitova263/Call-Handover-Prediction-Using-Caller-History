using System;
using System.Runtime.Serialization;

namespace VerticalHandoverPrediction
{
    class HandoverPredictionException : Exception
    {
        public HandoverPredictionException()
        {
        }

        public HandoverPredictionException(string message) : base(message)
        {
        }

        public HandoverPredictionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HandoverPredictionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}