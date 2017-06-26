using System;
using System.Collections;
using System.Threading;
using System.Text;
using System.Resources;
using System.Globalization;
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
        private TextBox employeeId;

        private void idCheckWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.checkId));
            Glide.MainWindow = window;

            employeeId = (TextBox)window.GetChildByName("textBox");
            employeeId.Text = "";
            employeeId.TapEvent += Glide.OpenKeyboard;

            GHI.Glide.UI.Button idButton = (GHI.Glide.UI.Button)window.GetChildByName("button");
            try
            {
                idButton.TapEvent += idButton_TapEvent;
            }
            catch (Exception e)
            {
                Debug.Print("button"+e);
            }
        }

        private void confirmCredentialsWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.confirmCredentials));

            TextBlock employeeFirstName = (TextBlock)window.GetChildByName("firstName");
            employeeFirstName.Text = firstName;
            TextBlock employeeLastName = (TextBlock)window.GetChildByName("lastName");
            employeeLastName.Text = lastName;
            TextBlock employeeRole = (TextBlock)window.GetChildByName("role");
            employeeRole.Text = role;

            Glide.MainWindow = window;

            GHI.Glide.UI.Button confirmButton = (GHI.Glide.UI.Button)window.GetChildByName("confirmBtn");
            confirmButton.TapEvent += confirmButton_TapEvent;
        }

        private void wrongIdWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.wrongId));
            Glide.MainWindow = window;

            GHI.Glide.UI.Button backToId = (GHI.Glide.UI.Button)window.GetChildByName("backBtn");
            backToId.TapEvent += backButton_TapEvent;
        }

        private void pictureWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.picture));
            Glide.MainWindow = window;
            Thread.Sleep(3000);
            button.ButtonReleased += shoot_TapEvent;
        try { 
            //camera.StopStreaming();
            //Thread.Sleep(1000);
            camera.StartStreaming();
        }
        catch (Exception se)
            {
                Debug.Print(se.ToString());
            }
           
                  
          
        }

        private void inWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.inWin));

            TextBlock employeeFirstName = (TextBlock)window.GetChildByName("firstName");
            employeeFirstName.Text = firstName;
            TextBlock employeeLastName = (TextBlock)window.GetChildByName("lastName");
            employeeLastName.Text = lastName;
            TextBlock employeeEntrance = (TextBlock)window.GetChildByName("entrance");
            employeeEntrance.Text = hour;
            GHI.Glide.UI.Button okButton = (GHI.Glide.UI.Button)window.GetChildByName("okBtn");
            okButton.TapEvent += okButton_TapEvent;

            Glide.MainWindow = window;
        }

        private void outWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.outWin));

            TextBlock employeeFirstName = (TextBlock)window.GetChildByName("firstName");
            employeeFirstName.Text = firstName;
            TextBlock employeeLastName = (TextBlock)window.GetChildByName("lastName");
            employeeLastName.Text = lastName;
            TextBlock employeeExit = (TextBlock)window.GetChildByName("exit");
            employeeExit.Text = hour;
            GHI.Glide.UI.Button okButton = (GHI.Glide.UI.Button)window.GetChildByName("okBtn");
            okButton.TapEvent += okButton_TapEvent;

            Glide.MainWindow = window;
        }

        private void retryWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.retry));
            Glide.MainWindow = window;

            GHI.Glide.UI.Button retryButton = (GHI.Glide.UI.Button)window.GetChildByName("retryBtn");
            retryButton.TapEvent += confirmButton_TapEvent;
        }

        private void matchFailedWindow()
        {
            GHI.Glide.Display.Window window = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.matchFailed));
            Glide.MainWindow = window;
        }
    }
}
