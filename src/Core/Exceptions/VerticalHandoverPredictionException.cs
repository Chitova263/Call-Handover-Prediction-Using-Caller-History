namespace VerticalHandoverPrediction.Exceptions
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