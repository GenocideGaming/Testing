using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace Server.Scripts.Custom.WebService
{
    //By feeding this class a xml string (xmlDoc.OuterXml) it will upload to the specified uri a http post request
    public class XmlUploader
    {
        string mXmlString;
        string mUriString;
        Thread mUploadThread;

        public Thread UploadThread { get { return mUploadThread; } }

        public XmlUploader(string xmlString, string uri)
        {
            mXmlString = xmlString;
            mUriString = uri;
            StartUploadThread();
        }
        public void StartUploadThread()
        {
            mUploadThread = new Thread(new ThreadStart(UploadData));
            mUploadThread.IsBackground = true;
            mUploadThread.Start();
        }
        public void UploadData()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(mUriString);
            request.Method = "POST";
            request.ContentType = "text/xml";

            byte[] postBytes = Encoding.UTF8.GetBytes(mXmlString);
            request.Headers["Authorization"] = Convert.ToBase64String(Encoding.UTF8.GetBytes("relpor_uo_server:sahufxqq1"));
            request.KeepAlive = false;
            request.AllowAutoRedirect = false;
            request.PreAuthenticate = true;
            request.ContentLength = postBytes.Length;

            try
            {
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(postBytes, 0, postBytes.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
