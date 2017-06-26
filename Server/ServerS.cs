using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;



namespace ServerSide
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client socket
        public Socket workSocket = null;
        // Size of the received buffer
        public const int BufferSize = 1024;
            // Received buffer
        public byte[] buffer = new byte[BufferSize];
        // Incoming content size
        public int contentSize;

        
        public int MAX_COUNT = 3;
        public int try_counter;

        public int receiving_stage;
        // Received dynamic buffer
        public byte[] _contentDynamicBuff;
        // How many bytes have been read
        public int _totBytesRead;
        // Username 
        public string iduser;
        // Image path retrieved from a MySql Database
        public string img;
        // Face matching coefficient
        public double coeff;
        // FaceMatching object
        public FaceRecognition f = new FaceRecognition();
        
    }

    public class ServerS
    {
        // Thread signal
        private ManualResetEvent allDone = new ManualResetEvent(false);
        // Database connection
        private DB db = new DB(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

        private int person_counter = 0;
        // Log file
        private string logFile = @"C:\Users\Luca\Documents\Visual Studio 2013\Projects\ServerSide\Current_Log_File.txt";
        FileStream fs;

        public void StartListening()
        {
            // Data buffer for the incoming data
            byte[] bytes = new Byte[1024];
            // Local endpoint for the socket
            // ServerIP = new IPAddress(new byte[] { 192, 168, 1, 5});
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 8080); 
            // TCP/IP socket
            Console.Write(localEndPoint.ToString()+"\n");
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections
            try
            {
                listener.Bind(localEndPoint);
                // Set pending queue to 100
                listener.Listen(100);
                while (true)
                {
                    // Set the event to nonsignaled state
                    allDone.Reset();
                    // Start an asynchronous socket to listen for connections
                    Console.WriteLine("Waiting for a connection \n");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    // Wait until a connection is established before continuing
                    
                    allDone.WaitOne();
                    
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Send a signal to the main thread in order to continue
            allDone.Set();
            // Get the socket that handles the client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            // Create the state object
            StateObject state = new StateObject();
            state.receiving_stage = 0;
            state.workSocket = handler;
            // Create the Log file
            if (!System.IO.File.Exists(logFile))
            {
                System.IO.File.Delete(logFile);
            }
            fs = System.IO.File.Create(logFile);
            fs.Close();
            Console.WriteLine("Connected with : {0}", state.workSocket.RemoteEndPoint.ToString());
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadHeaderCallback), state);
        }

        private void ReadHeaderCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket from the asynchronous state object
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            // Read the data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead < sizeof(int))
            {
                // Read again
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
              new AsyncCallback(ReadHeaderCallback), state);
            }
            else
            {
                state.contentSize = BitConverter.ToInt32(state.buffer, 0);
                state._totBytesRead = 0;
                state._contentDynamicBuff = new byte[state.contentSize];
                Console.WriteLine("ReadHeaderCallback size of data = {0}", state.contentSize);
                // Send an ack to the client
                Send(handler, "ACK");
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                   new AsyncCallback(ReadCallback), state);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket from the asynchronous state object
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            // Check whether the data provided by the client is correct
            bool control = false;
           
            // Read data from the client socket
            int bytesRead = handler.EndReceive(ar);
            // String to be written into the Log file
            string text;

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far
                Buffer.BlockCopy(state.buffer, 0, state._contentDynamicBuff, state._totBytesRead, bytesRead);
                state._totBytesRead += bytesRead;
                // Check whether we have read the entire content
                if (state._totBytesRead >= state.contentSize)
                {
                    switch (state.receiving_stage)
                    {
                        case 0:
                            // Expect iduser
                            Console.WriteLine("Received from {0} iduser equal to {1}", state.workSocket.RemoteEndPoint.ToString(), System.Text.Encoding.UTF8.GetString(state._contentDynamicBuff));
                            string token = System.Text.Encoding.UTF8.GetString(state._contentDynamicBuff);
                            state.iduser = token;
                            
                            text = "Attempt of login with these credentials -> IdUser: " + state.iduser ;
                            WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                            control = CheckID(System.Text.Encoding.UTF8.GetString(state._contentDynamicBuff));
                            if (control == true)
                            {
                                // Send the ACK and move to the next stage
                                text = "User recognition";

                                WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                                string after_id = db.FindCredetials(state.iduser);
                                Send(handler, "ACK@"+after_id);
                                state.receiving_stage++;
                            }
                            else
                            {
                                // Send the NACK and remain in the current stage
                                text = "User NOT in the system";
                                WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                                Send(handler, "NACK");
                                state.receiving_stage=0;
                            }
                            break;
                        case 1:
                            // Expect the image
                            state.img = RetrieveImgPath(state.iduser);
                            Console.WriteLine("Imagepath: {0}", state.img);
                            string ack = String.Format("Image correctly recived.{0}", Environment.NewLine);
                            state.coeff = state.f.waitCoeff(state.img,state._contentDynamicBuff);
                            //Counter to allow MAX_COUNT picture taken
                            state.try_counter++;
                            person_counter++; //Increment ID counter

                            // If coeff >= 0.5  => Same person
                            if (state.coeff >= 0.5)
                            {
                                // Send the ACK and go to stage 3
                                text = "Picture taken. Result: Match";
                                WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                                text = "Authorized access";
                                state.try_counter = 0;

                               

                                if (CheckIdFirst(state.iduser)==false || CheckEntryTime(state.iduser,FindLastID(state.iduser))==false)
                                {
                                    InsertFirstCredential(state.iduser,person_counter);
                                    WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                                    state.try_counter = 0;
                                    
                                    Send(handler, "EN@"+GetEntryOrExitTime(state.iduser,1,person_counter));

                                    Thread.Sleep(1000);
                                    Console.WriteLine("EN");
                                    state.receiving_stage = 0;
                                }


                                else if ( CheckExitTime(state.iduser, FindLastID(state.iduser)) == false)
                                { 
                                        InserExitCredential(state.iduser,person_counter);
                                        WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                                        Send(handler, "EX@"+GetEntryOrExitTime(state.iduser,0,person_counter));
                                        Console.WriteLine("EX");
                                        
                                        state.receiving_stage = 0;
                                    }
                            }
                             
                                
                            
                            else
                            {
                                // Send the NACK and increment the stage counter
                                text = "Picture taken. Result: Non match";
                                Console.WriteLine("Try Again");
                                WriteLogFile(text, state.workSocket.RemoteEndPoint.ToString(), state.iduser, logFile);
                                Send(handler, "NACK");
                                if (state.try_counter == state.MAX_COUNT)
                                {
                                    SendEmail();
                                    //sendMessage("Max number of tris exceded", "Luigi").Wait();
                                    state.receiving_stage=0;
                                }
                                else
                                {
                                    state.receiving_stage =1;
                                }
                            }
                            break;


                        case 2:
                            Console.WriteLine("Restart: state [0]");
                            state.receiving_stage = 0;
                            // Send the ACK and close the socket
                          //  Send(handler, "ACK");
                            //Console.WriteLine("Closing the connection");
                            //handler.Shutdown(SocketShutdown.Both);
                            //handler.Close();
                            //state.workSocket.Close();
                            break;

                        default:
                            Console.WriteLine("Switch case index does not exist");
                            break;
                    }
                    if (state.workSocket.Connected)
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadHeaderCallback), state);
                    }
                }
                else
                {
                    Console.WriteLine("Not all data received! Read more..");
                    // Not all data received: get more
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        public async Task sendMessage(string text, string name)
        {
            try
            {
                var bot = new Telegram.Bot.TelegramBotClient("377125814:AAHTAlSNFgyMq0tLM5yb1iasdQW0asdh_WM");
                var updates = await bot.GetUpdatesAsync();
                int last = updates.Length - 1;
                long dest = 0;
                foreach (var update in updates)
                {

                    if (update.Message.Chat.FirstName == name)
                    {
                        dest = update.Message.Chat.Id;
                        Console.WriteLine(update.Message.Text);
                    }
                }
                await bot.SendTextMessageAsync(dest, text);

            }
            catch (Exception e)
            {
                Console.WriteLine("err:" + e);
            }
        }

        private bool CheckID(string userid)
        {
            return db.CheckID(userid);
        }

        public bool CheckIdFirst(string IdUser)
        {
            return db.CheckIdFirst(IdUser);
        }

        private bool InsertFirstCredential(string userid, int id_seq)
        {
            return db.InserFirstCredential(userid,id_seq);
        }

        private string RetrieveImgPath(string userid)
        {
            return db.FindPath(userid);
        }

        private bool InsertEntryTime(string IdUser)
        {
            return db.InsertEntryTime(IdUser);
        }

        private string GetEntryOrExitTime(string iduse,int code,int id){

            return db.GetEntryOrExitTime(iduse, code, id);
    }

        private bool InsertExitTime(string IdUser, int id_seq)
        {
            return db.InsertExitTime(IdUser, id_seq);
        }

        private int FindID(string IdUser)
        {
            return db.FindID(IdUser);

        }

        private bool CheckEntryTime(string IdUser,int id)
        {
            return db.CheckEntryTime(IdUser,id);
        }

        private bool CheckExitTime(string IdUser, int id)
        {
            return db.CheckExitTime(IdUser, id);
        }

        private bool CheckPerson(string IdUser)
        {
            return db.CheckPerson(IdUser);
        }

      

        public bool InserExitCredential(string IdUser, int id_seq)
        {
            return db.InserExitCredential(IdUser, id_seq);
        }

        public int FindLastID(string IdUser){
            return db.FindLastID(IdUser);
        }

        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void WriteLogFile(string s, string IPAddr, string user, string path)
        {

            string str = "IP Address and port: " + IPAddr + ", User: " + user + ", Timestamp: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            using (StreamWriter sw = System.IO.File.AppendText(path))
            {
                sw.WriteLine(str);
                sw.WriteLine(s);
            }
        }
    
       

        public void SendEmail()
        {
            // Specify senders gmail address
            string SendersAddress = "projectandlab@gmail.com";
            // Specify the address you want to sent Email to (it can be any valid email address)
            string ReceiversAddress =;
            // Specify the password of the gmail account you are using to sent mail (pw of sender@gmail.com)
            const string SendersPassword =;
            // Write the subject of your mail
            const string subject = "Amind LogFile";
            // Write the content of your mail
            const string body = "Dear Admin, an employee access count excedeed.";
            // Create  the file attachment for this e-mail message
            Attachment data = new Attachment(logFile, MediaTypeNames.Application.Octet);

            try
            {
                // Use Smtp client which allows to send email using SMTP Protocol
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(SendersAddress, SendersPassword),
                    Timeout = 3000
                };
                // MailMessage represents a mail message: it is 4 parameters(From,TO,subject,body)
                MailMessage message = new MailMessage(SendersAddress, ReceiversAddress, subject, body);
                // Add the file attachment to this e-mail message
                message.Attachments.Add(data);
                smtp.Send(message);
                Console.WriteLine("Mail Sent Successfully");
                message.Dispose();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception in sending the email");
                Console.WriteLine(ex.Message);
                return;
            }
        }

     

    }
}