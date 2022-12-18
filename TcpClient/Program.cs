//TCP Client

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);                                    //Инициализация Tcp соединения

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);    //Инициализация сокета (сокет это шлюж конечных точек tcp)

            Console.Write("Введите сообщение: ");
            var message = Console.ReadLine();                                                               //Сообщение клиента

            var data = Encoding.UTF8.GetBytes(message);                                                     //Кодирование сообщения клиента в байты

            tcpSocket.Connect(tcpEndPoint);                                                                 //Подключение к серверу через tcp протокол
            tcpSocket.Send(data);                                                                           //Отправка закодированного (байт) сообщения на сервере через tcp протокол


            //Данные для получения ответа с сервера
            var buffer = new byte[256];                                                                     //Массив хранилище входящих запросов от клиента
            var size = 0;                                                                                   //Переменная - актуальный размер (байт) входящего запроса от клиента
            var answer = new StringBuilder();                                                               //Форматирования запроса от клиента в строку

            do                                                                                              //Цикл получения массива байт от сервера (ответ от сервера)
            {
                size = tcpSocket.Receive(buffer);                                                           //Получение размера (байт) запроса от клиента
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));                                    //Преобразование байт запроса от клиента в строку UTF8

            }
            while (tcpSocket.Available > 0);

            Console.WriteLine(answer.ToString());                                                          //Вывести полученный запрос от сервера

            tcpSocket.Shutdown(SocketShutdown.Both);                                                       //Выключить соединение с сервером
            tcpSocket.Close();                                                                             //Закрыть соединение с сервером

            Console.ReadLine();

        }
    }
}
