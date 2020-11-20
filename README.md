# WS2812

### Installing
Please open a SSH shell. You do not need to use a root shell to install ws2812. If a normal unprivilidged user exists you should start the installation with this user. 

`sudo apt-get update`

`sudo apt-get install gcc make build-essential python-dev git scons swig`

`sudo nano /etc/modprobe.d/snd-blacklist.conf`

`blacklist snd_bcm2835`

`sudo nano /boot/config.txt`

#Enable audio (loads snd_bcm2835)
`dtparam=audio=on`

`sudo reboot`

`git clone https://github.com/jgarff/rpi_ws281x`

`cd rpi_ws281x/`

`sudo scons`

`cd python`

`sudo python3 setup.py build`

`sudo python3 setup.py install`

`sudo pip3 install adafruit-circuitpython-neopixel`

That's it :)


