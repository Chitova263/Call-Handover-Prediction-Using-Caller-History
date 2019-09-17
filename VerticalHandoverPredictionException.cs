using System;
using System.Runtime.Serialization;

namespace VerticalHandoverPrediction
{
    [Serializable]
    internal class VerticalHandoverPredictionException : Exception
    {
        public VerticalHandoverPredictionException()
        {
        }

        public VerticalHandoverPredictionException(string message) : base(message)
        {
        }

        public VerticalHandoverPredictionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VerticalHandoverPredictionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}