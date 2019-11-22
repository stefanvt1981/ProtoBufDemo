using Google.Protobuf;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ProtoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var persoon = new Persoon
            {
                Voornaam = "Stefan",
                Achternaam = "Achternaam",
                Leeftijd = 38
            };

            // XML
            var outputStream = new StringWriter();
            XmlSerializer ser = new XmlSerializer(typeof(Persoon));

            ser.Serialize(outputStream, persoon);

            string xml = outputStream.ToString();
            outputStream.Dispose();

            // JSON
            var json = JsonConvert.SerializeObject(persoon);

            // Protocol Buffers
            var persoonPB = new PersoonPB
            {
                Voornaam = "Stefan",
                Achternaam = "van Tilborg",
                Leeftijd = 38
            };

            using (var output = File.Create("persoon.dat"))
            {
                persoonPB.WriteTo(output);
            }

            using (var input = File.OpenRead("persoon.dat"))
            {
                using var sr = new StreamReader(input, Encoding.UTF8);

                string content = sr.ReadToEnd();

            }
        }
    }
}
