using IniParser;
using IniParser.Model;
using System;
using System.IO;

namespace ws2812
{
    public class Config : IDisposable
    {
        string filePath;
        IniDataParser parser;
        IniData data;

        public Config(string _filePath)
        {
            filePath = _filePath;

            parser = new IniDataParser();
            data = new IniData();

            parser.Scheme.CommentString = "#";

            if (!File.Exists(filePath))
                WriteDefaultConfig();
            else
            {
                using (TextReader reader = File.OpenText(filePath))
                {
                    string text = reader.ReadToEnd();
                    data = parser.Parse(text);
                }                
            }                
        }

        void WriteDefaultConfig()
        {
            //Add a new section and some keys
            data.Sections.Add("SPI");
            data.Sections.Add("WebSocket");
            data.Sections.Add("Strip");

            Property key = new Property("Bus");
            key.Comments.Add("Select the used bus ex. spidev1.0 = 1");
            key.Value = "0";
            data["SPI"].Add(key);

            key = new Property("Chip");
            key.Comments.Add("Select the used chip ex. spidev1.0 = 0");
            key.Value = "1";
            data["SPI"].Add(key);

            key = new Property("Port");
            key.Comments.Add("Port which listens for connections");
            key.Value = "4649";
            data["WebSocket"].Add(key);

            key = new Property("Type");
            key.Comments.Add("RGB Type 0=RGB 1=RBG 2=GRB 3=GBR 4=BRG");
            key.Value = "0";
            data["Strip"].Add(key);
        }

        public void Write(string section, string key, string value)
        {
            data[section][key] = value;
        }

        public string Read(string section, string key)
        {
            return data[section][key];
        }

        ~Config()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
