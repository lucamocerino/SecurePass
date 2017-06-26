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

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace Client
{
    public partial class Program
    {
        private string SendImage(byte[] byteData)
        {
            // Protocol for sending an image:
            // First, send the length of the content
            // Then, send the content
            Debug.Print("Image sent");
            server.Send(BitConverter.GetBytes(byteData.Length));
            receiveString();
            server.Send(byteData);
            return receiveString();
        }
        
        private string sendString(string data)
        {
            
            // Protocol for sending a string:
            // First, send the length of the content
            // Then, send the content
            try
            {
                Debug.Print("String sent");

                byte[] byteData = Encoding.UTF8.GetBytes(data);
                
                server.SendTo(BitConverter.GetBytes(byteData.Length), remoteEP);
                receiveString();
                server.Send(byteData);
               
            }
            catch (SocketException se)
            {
                Debug.Print(se.ToString());
            }
            return receiveString();
        }

        private string receiveString()
        {
            byte[] bytes = new byte[200];
            try
            {
                // Read the response from the remote server
                int received;
                received = server.Receive(bytes);
                Debug.Print(new string(Encoding.UTF8.GetChars(bytes)));
            }
            catch (Exception e)
            {
                Debug.Print("SendReceive");
                return new string(Encoding.UTF8.GetChars(bytes));
            }
            return new string(Encoding.UTF8.GetChars(bytes));
        }

    }
}
