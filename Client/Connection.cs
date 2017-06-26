using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Net.NetworkInformation;
using GHI.Glide;
using GHI.Glide.Display;
using GHI.Glide.UI;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace Client
{
    public partial class Program
    {
        // The port number for the listening socket of the server
        private const int port = 8080;
        // A TCP/IP Socket
        private Socket server;
        static IPEndPoint remoteEP;

        void setUpEthernet()
        {
           //thernetJ11D.UseDHCP();
            Microsoft.SPOT.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            ethernetJ11D.NetworkSettings.EnableStaticIP("192.168.137.100", "255.255.255.0", "192.168.137.1");
           // ethernetJ11D.NetworkUp += ethernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += ethernetJ11D_NetworkDown;
            ethernetJ11D.UseThisNetworkInterface();
            Debug.Print("sueth");
        }
        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {

                Debug.Print("Network is now up");
                startConnection();
            }
            else
            {
                Debug.Print("Ethernet non piu' disponibile");
                camera.StopStreaming();
                GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.disconnected));
                Glide.MainWindow = window;
                Thread.Sleep(10000);
                RestartProgram();

            }
        }

        private void ethernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            //connectionThread.Join();
            Debug.Print("Network is down!");
        }
        /*
        private void ethernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is up:  " + ethernetJ11D.IsNetworkUp);
            Debug.Print("My IP is: " + ethernetJ11D.NetworkSettings.IPAddress);
            Debug.Print("DHCP Enabled: " + ethernetJ11D.NetworkSettings.IsDhcpEnabled); //true if DHCP is enabled, or false if not.
            Debug.Print("Subnet Mask:  " + ethernetJ11D.NetworkSettings.SubnetMask);
            Debug.Print("Gateway:      " + ethernetJ11D.NetworkSettings.GatewayAddress);
            Debug.Print("Connected: " + ethernetJ11D.IsNetworkConnected);
            Debug.Print("------------------------------------------------");
           // startConnection();
        }*/

       

        private void startConnection()
        {
            try
            {
                //// Establish the remote endpoint for the socket
                IPAddress ServerIP = new IPAddress(new byte[] { 192, 168, 137, 1});
                remoteEP = new IPEndPoint(ServerIP, port);
              
                // Create a TCP/IP socket
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.SetSocketOption(SocketOptionLevel.Tcp
                      , SocketOptionName.NoDelay
                      , true);
                // Set the timeout for synchronous receive methods to 120 second (120000 milliseconds)
                server.ReceiveTimeout = 120000;
                // Connect to the remote endpoint
                server.Connect(remoteEP);
               
               

                Debug.Print("Connection to : " + remoteEP.ToString());
                   
                
                
            }
            catch (SocketException se)
            {
                Debug.Print(se.ToString());
            }
        }

        private void closeConnection()
        {
            string content = "Close the connection";
            string response = sendString(content);
            Debug.Print("Response = " + response);
            server.Close();
        }

    }
}
