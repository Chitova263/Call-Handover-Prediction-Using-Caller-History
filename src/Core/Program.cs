using Serilog;
using System;
using ElectronCgi.DotNet;

namespace VerticalHandoverPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var connection = new ConnectionBuilder()
                .WithLogging()
                .Build();

            var network = new NetworkBuilder()
                .RegisterRat(options =>
                {
                    options.Capacity = 10;
                    options.Name = "RAT 1";
                    options.SupportedServices = Service.Voice;
                })
                .RegisterRat(options =>
                {
                    options.Capacity = 10;
                    options.Name = "RAT 2";
                    options.SupportedServices = Service.Data | Service.Video;
                })
                .RegisterRat(options =>
                {
                    options.Capacity = 10;
                    options.Name = "RAT 3";
                    options.SupportedServices = Service.Data | Service.Video;
                })
                .RegisterRat(options =>
                {
                    options.Capacity = 10;
                    options.Name = "RAT 4";
                    options.SupportedServices = Service.Voice | Service.Data | Service.Video;
                })
                .RegisterMobileTerminals()
                .Build();

            var simulator = new SimulatorBuilder(network, options =>
                {
                    options.NumberOfCalls = 20000;
                    options.VoiceCallLifeTime = TimeSpan.FromMinutes(60);
                    options.DataCallLifeSpan = TimeSpan.FromMinutes(80);
                    options.VideoCallLifeSpan = TimeSpan.FromMinutes(70);
                    options.VoiceBasicBandwidthUnits = 10;
                    options.DataBasicBandwidthUnits = 30;
                    options.VideoBasicBandwidthUnits = 50;
                    options.PreserveCallLogs = true;
                    options.UseInMemoryCallLogs = true;
                })
                .Build();

            //var result1 = simulator.Run<PredictiveAlgorithm>();
            var result2 = simulator.Run<NonPredictiveAlgorithm>();
        }
    }
}
