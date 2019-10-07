namespace VerticalHandoverPrediction
{
    using System;

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