using Google.Protobuf;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SpeedDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            AddressPB address = new AddressPB
            {
                Address1 = "123 Market St",
                Address2 = "",
                City = "Houston",
                State = "Texas",
                ZipCode = "77001"
            };

            CustomerPB customer = new CustomerPB
            {
                FirstName = "Fred",
                LastName = "Thompson",
                Address = address 
            };            
                       

            int iterations = 1000;
            int i;
            int size = 0;

            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (i = 0; i < iterations; i++) 
            {
                size = XmlTest(customer);
            }
            sw.Stop();

            Console.WriteLine($"XML marshall / unmarshall: { sw.ElapsedMilliseconds } ms");
            Console.WriteLine($"XML Size: {size}");

            sw.Reset();

            sw.Start();
            for (i = 0; i < iterations; i++) 
            {
                size = JsonTest(customer);
            }
            sw.Stop();

            Console.WriteLine($"JSON marshall / unmarshall: { sw.ElapsedMilliseconds } ms");
            Console.WriteLine($"JSON Size: {size}");

            sw.Reset();

            sw.Start();
            for (i = 0; i < iterations; i++) 
            {
                size = PbTest(customer);
            }
            sw.Stop();

            Console.WriteLine($"ProtoBuf marshall / unmarshall: { sw.ElapsedMilliseconds } ms");
            Console.WriteLine($"ProtoBuf Size: {size}");
        }

        private static int XmlTest(CustomerPB customer)
        {
            try
            {
                var outputStream = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(typeof(CustomerPB));

                ser.Serialize(outputStream, customer);

                byte[] data = outputStream.ToArray();

                var inputStream = new MemoryStream(data);
                var cust = (CustomerPB) ser.Deserialize(inputStream);

                return data.Length;

            } 
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        private static int JsonTest(CustomerPB customer)
        {
            try
            {
                var json = JsonConvert.SerializeObject(customer);            

                byte[] data = Encoding.UTF8.GetBytes(json);

                var cust = JsonConvert.DeserializeObject(json);

                return data.Length;
            } 
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        private static int PbTest(CustomerPB customer)
        {
            try
            {
                var outputStream = new MemoryStream();
                CodedOutputStream cos = new CodedOutputStream(outputStream);

                customer.WriteTo(cos);
                cos.Flush();                

                byte[] data = outputStream.ToArray();

                var inputStream = new MemoryStream(data);

                var cust = CustomerPB.Parser.ParseFrom(inputStream);

                return data.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
    }
}
