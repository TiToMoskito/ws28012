using System;
using System.Threading;
using System.Threading.Tasks;
using ws2812.Effects;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using System.Device.Spi;

namespace ws2812
{
    internal class Effect : IDisposable
    {
        IEffect m_effect;

        internal void Initialize(EEffect _effect, int[] _args)
        {
            switch (_effect)
            {
                case EEffect.ColorWipe:
                    m_effect = new ColorWipeEffect(_args);
                    break;
                case EEffect.Rainbow:
                    m_effect = new RainbowEffect();
                    break;
            }
        }

        public void Play(CancellationToken token)
        {
            using (var spi = SpiDevice.Create(Program.settings))
            {
                var device = new Ws2812b(spi, Program.count);

                BitmapImage image = device.Image;
                image.Clear();
                device.Update();
                Thread.Sleep(10);

                while (!token.IsCancellationRequested)
                {
                    m_effect.Play(device, image);
                }

                image.Clear();
                device.Update();                
            }            
        }

        public void Dispose()
        {
            m_effect.OnDisconnect();
            m_effect = null;
        }
    }

    public enum EEffect
    {
        ColorWipe,
        Rainbow
    }
}
