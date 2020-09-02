using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitConsumer
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
                    //channel.QueueDeclare("kibaQueue", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);//消费者
                    channel.BasicConsume("kibaQueue", true, consumer);//消费消息
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);

                        Thread.Sleep(1000);
                        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                 
                }

                Console.WriteLine("消费完成");
                Console.ReadLine();
            }
        }
    }
}
