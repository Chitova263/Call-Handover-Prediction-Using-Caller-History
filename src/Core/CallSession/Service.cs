using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace VerticalHandoverPrediction.CallSession
{
    [Flags]
    public enum Service
    {
        Voice,
        Data,
        Video,
    }
}