using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using System;
using System.Device.Spi;
using System.IO;
using WebSocketSharp.Server;

namespace ws2812
{
    class Program
    {
        public const int count = 40;
        public static SpiConnectionSettings settings;
        public static EStrip strip;

        public static Config config;

        static void Main(string[] args)
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
            config = new Config(configFilePath);

            strip = (EStrip)Convert.ToInt32(config.Read("Strip", "Type"));

            settings = new SpiConnectionSettings(Convert.ToInt32(config.Read("SPI", "Bus")), Convert.ToInt32(config.Read("SPI", "Chip")))
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            var wssv = new WebSocketServer(Convert.ToInt32(config.Read("WebSocket", "Port")));
            wssv.AddWebSocketService<Default>("/");
            wssv.Start();

            if (wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);
                foreach (var path in wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            wssv.Stop();
            config.Dispose();
            OnProcessExit();
        }       

        static void OnProcessExit()
        {
            Console.WriteLine("Closing");

            using (var spi = SpiDevice.Create(settings))
            {
                var device = new Ws2812b(spi, count);
                BitmapImage image = device.Image;
                image.Clear();
                device.Update();
            }
            Environment.Exit(0);
        }

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
