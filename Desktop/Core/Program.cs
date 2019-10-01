using System;
using ElectronCgi.DotNet;

namespace Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new ConnectionBuilder()
                .WithLogging()
                .Build();
            
            connection.On<dynamic, dynamic>("greeting", message => {
                return "Hello" + " " + message;
            });

            connection.Listen();
        }
    }
}
