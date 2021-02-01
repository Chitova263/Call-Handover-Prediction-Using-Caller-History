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
                    options.Capacity = 60;
                    options.Name = "RAT 1";
                    options.SupportedServices = Service.Data | Service.Video;
                })
                .RegisterRat(options =>
                {
                    options.Capacity = 60;
                    options.Name = "RAT 1";
                    options.SupportedServices = Service.Data | Service.Video;
                })
                .RegisterRat(options =>
                {
                    options.Capacity = 60;
                    options.Name = "RAT 1";
                    options.SupportedServices = Service.Data | Service.Video;
                })
                .RegisterRat(options =>
                {
                    options.Capacity = 60;
                    options.Name = "RAT 1";
                    options.SupportedServices = Service.Data | Service.Video;
                })
                .RegisterMobileTerminals()
                .Build();

            var simulator = new SimulatorBuilder(network, options =>
                {
                    options.NumberOfCalls = 2000;
                    options.VoiceCallLifeTime = TimeSpan.FromMinutes(60);
                    options.DataCallLifeSpan = TimeSpan.FromMinutes(80);
                    options.VideoCallLifeSpan = TimeSpan.FromMinutes(70);
                    options.VoiceBasicBandwidthUnits = 1;
                    options.DataBasicBandwidthUnits = 2;
                    options.VideoBasicBandwidthUnits = 3;
                    options.PreserveCallLogs = true;
                    options.UseInMemoryCallLogs = true;
                })
                .Build();

            var result1 = simulator.Run<PredictiveAlgorithm>();
            var result2 = simulator.Run<NonPredictiveAlgorithm>();
        }
    }
}
