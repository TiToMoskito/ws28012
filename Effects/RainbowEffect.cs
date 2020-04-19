using System.Drawing;
using System.Threading;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;

namespace ws2812.Effects
{
    internal class RainbowEffect : IEffect
    {
        // Color Fade
        int r = 255;
        int g = 0;
        int b = 0;

        public void Play(Ws2812b _device, BitmapImage _image)
        {
            if (r > 0 && b == 0)
            {
                r--;
                g++;
            }
            if (g > 0 && r == 0)
            {
                g--;
                b++;
            }
            if (b > 0 && g == 0)
            {
                r++;
                b--;
            }

            _image.Clear(Color.FromArgb(r, g, b));
            _device.Update();
            Thread.Sleep(10);
        }

        public void OnDisconnect()
        {
        }
    }
}
