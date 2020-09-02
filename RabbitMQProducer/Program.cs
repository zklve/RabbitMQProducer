using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "127.0.0.1";
            factory.UserName = "guest";//默认用户名,用户可以在服务端自定义创建，有相关命令行
            factory.Password = "guest";//默认密码

            //连接服务器，即正在创建终结点。
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    //创建一个名称为kibaqueue的消息队列
                    channel.QueueDeclare("kibaQueue", false, false, false, null);
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 1;

                    for(int i = 0; i <20; i++)
                    {
                        Thread.Sleep(1000);
                        string message = "I am Kiba518" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ; //传递的消息内容
                        channel.BasicPublish("", "kibaQueue", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                        Console.WriteLine($"Send:{message}");
                    }
                }
            }
        }
    }
}
