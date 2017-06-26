using System;
using System.Collections;
using System.Threading;
using System.Text;
using System.Resources;
using System.Net;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
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
        private string firstName, lastName, role;

        private void startDisplay()
        {
            // Initialize components
            GlideTouch.Initialize();
            // Starting window: idCheck
            idCheckWindow();
        }

        private void idButton_TapEvent(Object sender)
        {
             
            string response = sendString(employeeId.Text);
            //string response = "ACK@Edoardo@Mazzella@Software Engineer";
            Debug.Print(response);
            string[] substrings = response.Split('@');
            if (substrings[0] == "ACK")
            {
                firstName = substrings[1];
                lastName = substrings[2];
                role = substrings[3];
                //Debug.Print(role);
                confirmCredentialsWindow();
            }
            else
                wrongIdWindow();
        }

        private void backButton_TapEvent(Object sender)
        {
            idCheckWindow();
        }

        private void confirmButton_TapEvent(Object sender)
        {
            pictureWindow();
        }

        private void shoot_TapEvent(Gadgeteer.Modules.GHIElectronics.Button sender, Gadgeteer.Modules.GHIElectronics.Button.ButtonState state)
        {
            camera.StopStreaming();
            //takepic_flag = true;
            camera.TakePicture();
            Thread.Sleep(500);
        }

        private void okButton_TapEvent(Object sender)
        {
           RestartProgram();
        }
    }
}
