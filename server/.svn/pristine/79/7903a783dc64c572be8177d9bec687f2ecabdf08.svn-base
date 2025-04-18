using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using Server.Misc;
using RunUODB;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace Server.Scripts.Custom.WebService
{
    public class RequestListener
    {
        private static RequestListener mInstance;
        public static RequestListener Instance { get { return mInstance; } }

        public static byte[] TodaysKey;

        private static X509Certificate serverCertificate = null;

        private TcpListener mTcpListener;
        private Thread mListenThread;


        public static void Initialize()
        {
            if(mInstance == null)
                mInstance = new RequestListener();
            
        }

        public RequestListener()
        {
            mTcpListener = new TcpListener(IPAddress.Any, 2595);
            mListenThread = new Thread(new ThreadStart(ListenForClients));
            mListenThread.IsBackground = true;
            mListenThread.Start();

            Console.WriteLine("Listening for TCP web requests on port 2595");
           
            serverCertificate = X509Certificate.CreateFromCertFile("anotherCert.cer");
        }

        private void ListenForClients()
        {
            try
            {
                mTcpListener.Start();

                while (true)
                {
                    TcpClient client = mTcpListener.AcceptTcpClient();

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
                    clientThread.Start(client);
                    Thread.Sleep(50);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("TCP listener socket exception, restart the server. " + ex.Message);
            }
        }

        private void HandleClientCommunication(object client)
        {
            TcpClient tcpClient = client as TcpClient;
            SslStream clientStream = new SslStream(tcpClient.GetStream(), false);

            try
            {
                clientStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);

                byte[] buffer = new byte[256];
                while (true)
                {
                    int bytesRead = clientStream.Read(buffer, 0, buffer.Length);
                    string message = UTF8Encoding.UTF8.GetString(buffer);
                    if (bytesRead == 0)
                        break;
                    Console.WriteLine("Got a message from webserver.");

                    DoCommand(message, clientStream);
                }

                clientStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            tcpClient.Close();
       
        }
        private static void DoCommand(string message, SslStream clientStream)
        {
            string[] splits = message.Split('#');

            switch (splits[0])
            {
                case "ap": //account packet
                    string username = splits[1];
                    string password = splits[2];
                    string ip = splits[3];
                    string email = splits[4];
                    string userID = splits[5];

                    bool success = AccountHandler.CreateAccount(username, password, ip);
                    if (success)
                    {
                        //GameAccount gameAccount = DatabaseController.AddGameAccount(username, string.Empty, email, userID);
                        //SendAccountSuccessPacket(clientStream, gameAccount);
                        SendAccountSuccessPacket(clientStream, null);
                    }
                    else
                    {
                        SendAccountFailurePacket(clientStream);
                    }
                    break;
                    
            }
        }
        private static void SendAccountSuccessPacket(SslStream clientStream, GameAccount gameAccount)
        {
            byte[] buffer = UTF8Encoding.UTF8.GetBytes("sp#"+gameAccount.Username + "#" + gameAccount.Id);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            Console.WriteLine("Sent success message.");
        }
        private static void SendAccountFailurePacket(SslStream clientStream)
        {
            byte[] buffer = UTF8Encoding.UTF8.GetBytes("fp#");
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            Console.WriteLine("Sent failure message.");
        }
        /*
        public static string DecryptBytes(byte[] encryptedBuffer)
        {

            SymmetricAlgorithm symm = RijndaelManaged.Create();
            symm.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = symm.CreateDecryptor(Key, Vector);
            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();


            CryptoStream cs = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cs.Write(encryptedBuffer, 0, encryptedBuffer.Length);
            cs.FlushFinalBlock();

            StreamReader reader = new StreamReader(cs);
            string message = reader.ReadToEnd();

            reader.Close();
            cs.Close();
            memoryStream.Close();

            return message;
        }
        public static byte[] EncryptString(string TextValue)
        {
            SymmetricAlgorithm symm = RijndaelManaged.Create();
            symm.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = symm.CreateEncryptor(Key, Vector);

            //Translates our text value into a byte array.
            Byte[] bytes = UTF8Encoding.UTF8.GetBytes(TextValue);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cs = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();


            byte[] encrypted = memoryStream.ToArray();


            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        */
    }
}
