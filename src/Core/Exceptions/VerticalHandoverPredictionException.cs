using System;

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
    }
}