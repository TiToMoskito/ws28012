using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Device.Spi;
using System.IO;
using WebSocketSharp.Server;

namespace ws2812
{
    class Program
    {
        public static int count = 40;
        public static SpiConnectionSettings settings;
        public static EStrip strip;

        public static Config config;

        private static WebSocketServer server;

        static void Main(string[] args)
        {
            
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
            config = new Config(configFilePath);

            strip = (EStrip)Convert.ToInt32(config.Read("Strip", "Type"));
            count = Convert.ToInt32(config.Read("Strip", "Count"));

            settings = new SpiConnectionSettings(Convert.ToInt32(config.Read("SPI", "Bus")), Convert.ToInt32(config.Read("SPI", "Chip")))
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            server = new WebSocketServer(Convert.ToInt32(config.Read("WebSocket", "Port")));
            server.AddWebSocketService<Default>("/");
            server.Start();

            if (server.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", server.Port);
            }

            CreateHostBuilder(args).Build().Run();            
        }

        public static void OnProcessExit()
        {
            Console.WriteLine("OnProcessExit");

            server.Stop();
            config.Dispose();

            using (var spi = SpiDevice.Create(settings))
            {
                var device = new Ws2812b(spi, count);
                BitmapImage image = device.Image;
                image.Clear();
                device.Update();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    

        public enum EStrip
        {
            RGB,
            RBG,
            GRB,
            GBR,
            BRG
        }
    }
}
