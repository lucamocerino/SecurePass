using System;
using System.Collections;
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
        bool firstExecution = true;
        void ProgramStarted()
        {
            attempt = 0;
          

            Debug.Print("Program Started");

            if (firstExecution)
            {
                Debug.Print("First Execution");
                setUpCamera();
                while (!camera.CameraReady) ;
                setUpEthernet();
                firstExecution = false;
            }


            startDisplay();
            
           
           
        }

        void RestartProgram()
        {

          

            ProgramStarted();
        }
    }
}
