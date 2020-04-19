using Iot.Device.Graphics;
using Iot.Device.Ws28xx;

namespace ws2812
{
    internal interface IEffect
    {
        void Play(Ws2812b _device, BitmapImage _image);
        void OnDisconnect();
    }
}
