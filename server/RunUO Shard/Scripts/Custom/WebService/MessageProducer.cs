using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Threading;

namespace Server.Scripts.Custom.WebService
{
    class MessageProducer
    {
        private static IModel Model;
        private static IConnection Connection;
        private static string QueueName = "runuodb";
        private static Queue<string> MessageQueue;
        private static Thread MessageThread;

        public static void Initialize()
        {
            if (FeatureList.Database.Active)
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = "localhost";
                Connection = connectionFactory.CreateConnection();
                Model = Connection.CreateModel();
                Model.ExchangeDeclare("relpor_exchange", "fanout", true);
                MessageQueue = new Queue<string>();
                MessageThread = new Thread(new ThreadStart(ProcessQueue));
                MessageThread.IsBackground = true;
                MessageThread.Start();

                Console.WriteLine("Message queue started, ready to produce messages.");
            }
        }
        public static void ProcessQueue()
        {
            while (true)
            {
                if (MessageQueue.Count > 0)
                {
                    string message = MessageQueue.Dequeue();

                    if (!(message is string) || message == string.Empty)
                        continue;

                    try
                    {
                        IBasicProperties basicProperties = Model.CreateBasicProperties();

                        byte[] payload = Encoding.UTF8.GetBytes(message);
                        Model.BasicPublish("relpor_exchange", string.Empty, basicProperties, payload);

                    }
                    catch (NullReferenceException)
                    {
                        Model = Connection.CreateModel();
                        Model.ExchangeDeclare("relpor_exchange", "fanout", true);

                        //try sending the message again
                        IBasicProperties basicProperties = Model.CreateBasicProperties();

                        byte[] payload = Encoding.UTF8.GetBytes(message);
                        Model.BasicPublish("relpor_exchange", string.Empty, basicProperties, payload);
                    }
                }

                Thread.Sleep(50);
            }
        }
        
        public static void EnqueueMessage(string message)
        {
            MessageQueue.Enqueue(message);
        }

        public void Dispose()
        {
	        if (Connection != null)
	            Connection.Close();
	        if (Model != null)
                Model.Abort();
        }
    }
}
