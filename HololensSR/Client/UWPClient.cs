using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UWPClient {

	static void Main(string[] args)
    {
        try
        {
            int port = 8001;
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(args[0]), port);
            TcpClient client = new TcpClient();
            Console.WriteLine("Connecting...\n");
            client.Connect(ip);
            Console.WriteLine("Connected!\n");
            using (NetworkStream ns = client.GetStream())
            {
                using (StreamReader sr = new StreamReader(ns))
                {
                    StreamWriter sw = new StreamWriter(ns);
                    Console.WriteLine("Enter a message: ");
                    string msg = Console.ReadLine();
                    while(msg != "") {
                    	sw.WriteLine(msg);
                    	sw.Flush();
                    	Console.WriteLine("Enter a message: ");
                        msg = Console.ReadLine();
                    }
                                        
                }
            }
        }
        catch (Exception e)
        {
        	Console.WriteLine(e.ToString());
        }
    }

}