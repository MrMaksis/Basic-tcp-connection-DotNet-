//TCP Server

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);                                    //Инициализация Tcp соединения

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);    //Инициализация сокета (сокет это шлюж конечных точек tcp)
            tcpSocket.Bind(tcpEndPoint);                                                                    //Метод прослушивания сокета - от клиента
            tcpSocket.Listen(5);                                                                            //Включение сокета на прослушивание

            while (true)                                                                                    //Цикл бесконечного прослушивания
            {
                var listener = tcpSocket.Accept();                                                          //Под сокет обработки клиента (временный/для каждого подключенного клиента)
                var buffer = new byte[256];                                                                 //Массив хранилище входящих запросов от клиента
                var size = 0;                                                                               //Переменная - актуальный размер (байт) входящего запроса от клиента
                var data = new StringBuilder();                                                             //Форматирования запроса от клиента в строку

                do                                                                                          //Цикл получения массива байт от клиента (перебор всех полученных байт в массив)
                {
                    size = listener.Receive(buffer);                                                        //Получение размера (байт) запроса от клиента
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));                                  //Преоброзование байт запроса от клиента в строку UTF8

                }
                while (listener.Available > 0);

                Console.WriteLine(data);                                                                    //Вывести раскадированный запрос клиента

                listener.Send(Encoding.UTF8.GetBytes("Успех!"));                                            //Отправить ответ клиенту, строку преобразованную в байты

                listener.Shutdown(SocketShutdown.Both);                                                     //Выключить временное соеденение с клиентом
                listener.Close();                                                                           //Закрыть временное соединение с клиентом
            }

        }
    }
}
