

# WS2812

### Installing
Please open a SSH shell. You do not need to use a root shell to install ws2812. If a normal unprivilidged user exists you should start the installation with this user. 

    sudo apt-get update
    sudo apt-get install gcc make build-essential python-dev git scons swig
    sudo echo "blacklist snd_bcm2835" > /etc/modprobe.d/snd-blacklist.conf
    git clone https://github.com/jgarff/rpi_ws281x
    cd rpi_ws281x/
    sudo scons
    cd python
    sudo python3 setup.py build
    sudo python3 setup.py install
    sudo pip3 install adafruit-circuitpython-neopixel
    sudo reboot
  
Or use the automatic installer

    curl -sL http://titomoskito.com/ws2812/install.sh | sudo bash -
    
### GPIO
Set the Data Line to GPIO18
![enter image description here](http://titomoskito.com/ws2812/gpio.png)

That's it :)

> Tested with Raspberry Pi Zero WH
