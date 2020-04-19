using System;
using System.Drawing;
using System.Threading;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;

namespace ws2812.Effects
{
    internal class ColorWipeEffect : IEffect
    {
        byte red, green, blue;

        public ColorWipeEffect(int[] _args)
        {
            if (_args.Length == 4)
            {
                switch (Program.strip)
                {
                    case Program.EStrip.RGB:
                        red = Convert.ToByte(_args[1]);
                        green = Convert.ToByte(_args[2]);
                        blue = Convert.ToByte(_args[3]);
                        break;
                    case Program.EStrip.RBG:
                        red = Convert.ToByte(_args[1]);
                        green = Convert.ToByte(_args[3]);
                        blue = Convert.ToByte(_args[2]);
                        break;
                    case Program.EStrip.GRB:
                        red = Convert.ToByte(_args[2]);
                        green = Convert.ToByte(_args[1]);
                        blue = Convert.ToByte(_args[3]);
                        break;
                    case Program.EStrip.GBR:
                        red = Convert.ToByte(_args[2]);
                        green = Convert.ToByte(_args[3]);
                        blue = Convert.ToByte(_args[1]);
                        break;
                    case Program.EStrip.BRG:
                        red = Convert.ToByte(_args[3]);
                        green = Convert.ToByte(_args[1]);
                        blue = Convert.ToByte(_args[2]);
                        break;
                    default:
                        red = Convert.ToByte(_args[1]);
                        green = Convert.ToByte(_args[2]);
                        blue = Convert.ToByte(_args[3]);
                        break;
                }
            }
        }

        public void Play(Ws2812b _device, BitmapImage _image)
        {
            _image.Clear(Color.FromArgb(red, green, blue));
            _device.Update();
            Thread.Sleep(10);
        }

        public void OnDisconnect()
        {
        }
    }
}
