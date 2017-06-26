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
        //you can fail the match at most three times
        int attempt;
        string en_ex_nack;
        string hour;
       

        private void setUpCamera()
        {
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution320x240;
            camera.PictureCaptured += new Camera.PictureCapturedEventHandler(camera_PictureCaptured);
            camera.BitmapStreamed += camera_BitmapStreamed;
            Debug.Print("setup camera");
         
        }

        
        private void camera_BitmapStreamed(Camera sender, Bitmap e)
        {
           this.displayTE35.SimpleGraphics.DisplayImage(e, 0, 0);
        }
        
        private void camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            // Send the image to the remote host
            Debug.Print("Picture captured. Attempt #" + attempt);
            byte[] output = e.PictureData;

            string matchRetVal = SendImage(output);
            // string matchRetVal = "EX@21:30";
            if (matchRetVal.IndexOf("EN") == 0 || matchRetVal.IndexOf("EX") == 0)
            {
                string[] substrings = matchRetVal.Split('@');
                en_ex_nack = substrings[0];
                hour = substrings[1];
            }
            else en_ex_nack = matchRetVal;

            //recognized on entrance
            if (en_ex_nack == "EN")
            {
                inWindow();
            }
            //recognized on exit
            else if (en_ex_nack == "EX")
            {
                outWindow();
            }
            //not recognized
            else
            {
                attempt++;
                if (attempt < 3)
                    retryWindow();
                else
                {
                    matchFailedWindow();
                    Thread.Sleep(5000);
                    RestartProgram();
                }
            }
        }

    }
}
