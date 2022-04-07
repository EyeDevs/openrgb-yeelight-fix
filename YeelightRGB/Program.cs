using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace YeelightRGB
{
    internal class Program
    {


        StreamReader input = null;

        public Program(StreamReader input)
        {
            this.input = input;
        }
        public void Run()
        {
            String line;
            while ((line = input.ReadLine()) != null)
            {
                Console.Write(line + "\r\n");
            }
        }



        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        public static async Task Main(string[] args)
        {
            Console.WriteLine("Checking for openrgb");

            Process[] pname = Process.GetProcessesByName("OpenRGB");
            if (pname.Length > 0)
            {
                Console.WriteLine("OpenRGB is running killing process");
                foreach (var process in pname)
                {
                    process.Kill();
                }
                

            }
            Process.Start("Bin\\OpenRGB\\OpenRGB.exe");
            Console.WriteLine("Waiting for openrgb to load");
            Console.WriteLine();
            await Task.Delay(10000);

            string configfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/OpenRGB/OpenRGB.json";
            Console.WriteLine();
            Console.WriteLine("Searching for config...");
            if (!File.Exists(configfile))
            {
                Console.WriteLine("Config was not found in path:");
                Console.WriteLine(configfile);
                await Task.Delay(3000);
                Environment.Exit(0);

            }
            // Prevent to continue if config was not found
            await Task.Delay(1000);

            Console.WriteLine("Config was found Parsing Config...");
            // parse json

            string config = System.IO.File.ReadAllText(configfile);
            dynamic configjson = JObject.Parse(config);
            // Check if autostart is on

            if (configjson.AutoStart.enabled == "true")
            {

                Console.WriteLine("Warning: OpenRGB Autostart is enabled please disable it and run this program at start instead");
                await Task.Delay(3000);

            }

            Console.WriteLine();

            // Checks all yeelight devices and sends their ip them to telnet class
            int port = 55444;

            foreach (var device in configjson.YeelightDevices.devices)
            {
                if (device.music_mode == "false")
                {
                    Console.WriteLine("Warning: Music mode is disabled on Yeelight " + device.ip + " Change before continue press any key to continue");
                    Console.ReadKey();
                }
                
                string devip = device.ip;
                Thread t = new Thread(() => telnetrun(devip,port,GetLocalIPAddress()));
                t.Start();
                await Task.Delay(1500);

                port += 1;
            }


            while (true)
            {
                await Task.Delay(10000);
                
            }
        }

        static async void telnetrun(string ip, int port, string localip)
        {
            Console.WriteLine("Thread Running for Yeelight " + ip);
            TcpClient socket = null;
            try
            {
                socket = new TcpClient(ip, 55443);


                // keepalive for the socket
                int size = Marshal.SizeOf((uint)0);
                byte[] keepAlive = new byte[size * 3];

     
                Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, size);
                // Set amount of time without activity before sending a keepalive to 5 seconds
                Buffer.BlockCopy(BitConverter.GetBytes((uint)5000), 0, keepAlive, size, size);
                // Set keepalive interval to 5 seconds
                Buffer.BlockCopy(BitConverter.GetBytes((uint)5000), 0, keepAlive, size * 2, size);
                socket.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);
            }
            catch (SocketException)
            {
                Console.WriteLine("Unknown host - " + ip + ". Quitting");
                Console.ReadKey();
                Environment.Exit(0);
            }
            NetworkStream stream = socket.GetStream();
            StreamWriter output = new StreamWriter(stream);
            StreamReader input = new StreamReader(stream);

            Program cliobj = new Program(input);
            Thread t = new Thread(new ThreadStart(cliobj.Run));
            // Used for debugging
            //t.Start();

            //string onstr = "{\"id\":1,\"method\":\"set_power\",\"params\":[\"on\",\"sudden\",1,0]}";
            //string offstr = "{\"id\":1,\"method\":\"set_power\",\"params\":[\"off\",\"sudden\",0,0]}";


            // sets light to green to verify connection
            output.Write("{\"id\":1,\"method\":\"start_cf\",\"params\":[1,1,\"50,1,65280,100\"]}" + "\r\n");
            output.Flush();

            await Task.Delay(300);
            output.Write("{\"id\":1,\"method\":\"set_music\",\"params\":[1,\"" + localip + "\"," + port.ToString() + "]}" + "\r\n");
            output.Flush();
            await Task.Delay(1000);
            while (true)
            {
                
                
            }
        }

    }
}
