namespace VerticalHandoverPrediction.Simulator.Extensions
{
    using System;
    using System.Linq;

    public static class EnumExtensions
    {
        public static Enum GetRandomEnumValue(this Type t)
        {
            return Enum.GetValues(t)          
                .OfType<Enum>()              
                .OrderBy(e => Guid.NewGuid()) 
                .FirstOrDefault();            
        }
    }
}