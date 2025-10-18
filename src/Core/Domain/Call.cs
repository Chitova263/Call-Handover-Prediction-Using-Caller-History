using System;

namespace VerticalHandoverPrediction.Domain;

public class Call
{
    public Guid CallId { get; set; }
    public CallType CallType { get; set; }

    private Call(CallType callType)
    {
        CallId = Guid.NewGuid();    
    }

    public static Call VoiceCall()
    {
        return new Call(CallType.Voice);
    }
    
    public static Call DataCall()
    {
        return new Call(CallType.Data);
    }
    
    public static Call VideoCall()
    {
        return new Call(CallType.Video);
    }
}