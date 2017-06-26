using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Drawing;

using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Newtonsoft.Json;

namespace ServerSide
{



    public class FaceRecognition
    {
        private VerifyResult f;
        private static double coeff = 2;
        private Semaphore _pool;
        private readonly IFaceServiceClient faceServiceClient =
               new FaceServiceClient("e68191f348764966b5e622cb66b45dd5", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");



        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        public double waitCoeff(string path, byte[] firstImage)
        {
            _pool = new Semaphore(0, 1);
            try
            {
                MakeRequest(path, firstImage);
                _pool.WaitOne();
            }
            catch (Exception e)
            {
                //Console.WriteLine("Get coefficient error: " + e);
                return 0;
            }
            return coeff;
        }


        async void MakeRequest(string path, byte[] firstImage)
        {
            Face[] faceArray;

            Guid id1 = new Guid();
            Guid id2 = new Guid();

            try
            {

                using (Stream imageFileStream = new MemoryStream(firstImage))
                {
                    var faces = await faceServiceClient.DetectAsync(imageFileStream, true, true);

                    foreach (var face in faces)
                    {
                        if (faces.Length == 0 || faces.Length > 1)
                        {
                            Console.WriteLine("More faces. Retry");
                            _pool.Release();
                            coeff = 0;
                            break;
                        }
                        else
                        {
                            id1 = face.FaceId;
                        }
                        


                    }

                    faceArray = faces.ToArray();
                }
                using (Stream s1 = File.OpenRead(path))
                {
                    var faces1 = await faceServiceClient.DetectAsync(s1, true, true);

                    foreach (var face in faces1)
                    {
                        id2 = face.FaceId;


                    }


                }

                Console.Write(id1.ToString() + "\n");
                Console.Write(id2.ToString() + "\n");
                Task.Run(async () =>
                {
                    f = await faceServiceClient.VerifyAsync(id1, id2);
                    coeff = f.Confidence;
                    Console.Write(coeff);
                }).Wait();
                _pool.Release();

            }
            catch (Exception er)
            {
               // Console.WriteLine("Make request error: " + er);
                _pool.Release();
                coeff = 0;
            }
        }



    }
}

