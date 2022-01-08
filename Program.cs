using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yeelight_OPENRGB_Fix
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
            


            int yeeport = 55444;
            string telnetmusicon = "{\"id\":1,\"method\":\"set_music\",\"params\":[1,\"" + GetLocalIPAddress() + "\"," + yeeport.ToString() + "]}";
            string telnetmusicoff = "{\"id\":1,\"method\":\"set_music\",\"params\":[0,\"" + GetLocalIPAddress() + "\"," + yeeport.ToString() + "]}";
            Console.WriteLine("Enter Amount of yeelight devices 1 - 8:");
            string usrinput = Console.ReadLine();
            Console.WriteLine("Do u want to enable music mode or disable it? enable / disable");
            string enabl1 = Console.ReadLine();
            int usrint = Int32.Parse(usrinput);
            if (usrint > 9)
            {
                Console.WriteLine("The maxium devices are 9");
                await Task.Delay(3000);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Remember u need to enter the ip as the same order they are in openrgb");
                for (int i = 0; i <= usrint; i++)
                {
                    
                    
                    if (i == 1)
                    {
                        Console.WriteLine("Enter the ip of ur 1 yeelight");
                        string y1 = Console.ReadLine();
                        yeeport = 55444;

                        if (enabl1 == "enable")
                        {
                            telnetconnection(y1, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y1, telnetmusicoff);
                        }
                    }
                    else if (i == 2)
                    {
                        Console.WriteLine("Enter the ip of ur 2 yeelight");
                        string y2 = Console.ReadLine();
                        yeeport = 55445;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y2, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y2, telnetmusicoff);
                        }
                    }
                    else if (i == 3)
                    {
                        Console.WriteLine("Enter the ip of ur 3 yeelight");
                        string y3 = Console.ReadLine();
                        yeeport = 55446;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y3, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y3, telnetmusicoff);
                        }
                    }
                    else if (i == 4)
                    {
                        Console.WriteLine("Enter the ip of ur 4 yeelight");
                        string y4 = Console.ReadLine();
                        yeeport = 55447;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y4, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y4, telnetmusicoff);
                        }
                    }
                    else if (i == 5)
                    {
                        Console.WriteLine("Enter the ip of ur 5 yeelight");
                        string y5 = Console.ReadLine();
                        yeeport = 55448;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y5, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y5, telnetmusicoff);
                        }
                    }
                    else if (i == 6)
                    {
                        Console.WriteLine("Enter the ip of ur 6 yeelight");
                        string y6 = Console.ReadLine(); 
                        yeeport = 55449;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y6, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y6, telnetmusicoff);
                        }
                    }
                    else if (i == 7)
                    {
                        Console.WriteLine("Enter the ip of ur 7 yeelight");
                        string y7 = Console.ReadLine();
                        yeeport = 55450;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y7, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y7, telnetmusicoff);
                        }
                    }
                    else if (i == 8)
                    {
                        Console.WriteLine("Enter the ip of ur 8 yeelight");
                        string y8 = Console.ReadLine();
                        yeeport = 55451;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y8, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y8, telnetmusicoff);
                        }
                    }
                    else if (i == 9)
                    {
                        Console.WriteLine("Enter the ip of ur 9 yeelight");
                        string y9 = Console.ReadLine();
                        yeeport = 55452;
                        if (enabl1 == "enable")
                        {
                            telnetconnection(y9, telnetmusicon);
                        }
                        else
                        {
                            telnetconnection(y9, telnetmusicoff);
                        }
                    }
                }
            }
            await Task.Delay(5000);
            Console.WriteLine("Done with yeelights press any key to exit (when exiting the yeelights may stop working in music mode)");
            Console.ReadKey();
        }
        static async Task telnetconnection(string ip, string command2)
        {
            TcpClient socket = null;
            try
            {
                socket = new TcpClient(ip, 55443);
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
            //t.Start();

            string onstr = "{\"id\":1,\"method\":\"set_power\",\"params\":[\"on\",\"sudden\",1,0]}";
            string offstr = "{\"id\":1,\"method\":\"set_power\",\"params\":[\"off\",\"sudden\",0,0]}";
            output.Write(offstr + "\r\n");
            output.Flush();
            await Task.Delay(600);
            output.Write(onstr + "\r\n");
            output.Flush();
            await Task.Delay(600);
            output.Write(offstr + "\r\n");
            output.Flush();
            await Task.Delay(600);
            output.Write(onstr + "\r\n");
            output.Flush();
            await Task.Delay(600);
            //Console.WriteLine("Running requested command:" + command2);
            output.Write(command2 + "\r\n");
            output.Flush();
            // to prevent console and telnet connection closing
            while (true)
            {
               //output.Write("" + "\r\n");
               //output.Flush();
            }




        }



    
        static async void keepalive(StreamWriter output)
        {
            await Task.Delay(4000);
            output.Write("keepconnection");
            keepalive(output);

            Console.WriteLine("Sent keep alive command");
            

        }
    }
}
