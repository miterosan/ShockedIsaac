using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShockedIsaac.API
{
    public class ModBridge
    {
        private readonly int port = 11000;

        public required Func<int, Task> OnDamage;
        public required Func<int, Task> OnIntentionalDamage;
        public required Func<Task> OnDeath;

        public async Task StartServer() {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new(ipAddress, port);

            Socket listener = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);

            listener.Listen(10);

            Console.WriteLine("Waiting for isaac events...");

            while(true) {
                Socket handler = await listener.AcceptAsync();

                var bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                var data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                var command = data.Trim();

                if (command != "") {
                    try {
                        await handleEvent(command);
                    }
                    catch(Exception e) {
                        Console.WriteLine(e.Message);
                    }
                }

                handler.Close();
             }
        }

        private async Task handleEvent(string command) {
            var split = command.Split(",");
            
            switch (split[0])
            {   
                case "onDamage": {
                    await OnDamage?.Invoke((int)float.Parse(split[1]));
                    break;
                }
                case "onIntentionalDamage": {
                    await OnIntentionalDamage?.Invoke((int)float.Parse(split[1]));
                    break;
                }
                case "onDeath": {
                    await OnDeath?.Invoke();
                    break;
                }
                
                default: throw new NotImplementedException(split[0]);
            }
        }
    }
}