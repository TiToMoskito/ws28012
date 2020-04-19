using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Device.Spi;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ws2812
{
    public class Default : WebSocketBehavior
    {
        Effect m_effect;
        CancellationTokenSource m_source;
        CancellationToken m_token;

        protected override void OnMessage(MessageEventArgs e)
        {
            int[] args = Array.ConvertAll(e.Data.Split(' '), int.Parse);

            EEffect effect = (EEffect)args[0];

            if (effect.Equals(null))
            {
                Console.WriteLine("Effect not found!");
                return;
            }

            if (m_source == null)
            {
                m_source = new CancellationTokenSource();
                m_token = m_source.Token;
            }

            if (m_effect == null)
                m_effect = new Effect();

            m_effect.Initialize(effect, args);
            var task = Task.Run(() => m_effect.Play(m_token), m_token);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Client disconnected");

            if (m_effect != null)
            {
                m_source.Cancel();
                m_effect.Dispose();
            }               

            using (var spi = SpiDevice.Create(Program.settings))
            {
                var device = new Ws2812b(spi, Program.count);
                BitmapImage image = device.Image;
                image.Clear();
                device.Update();
            }

            base.OnClose(e);
        }

        protected override void OnOpen()
        {
            Console.WriteLine("Client connected");
            base.OnOpen();
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Console.WriteLine("Error "+e.Message);

            base.OnError(e);
        }
    }
}
