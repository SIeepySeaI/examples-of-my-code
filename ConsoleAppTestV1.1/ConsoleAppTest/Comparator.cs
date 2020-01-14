using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ConsoleAppTest
{
    class Comparator
    {
        //int v = 0;
        const int KeyDB = 1;
        public void CompareData(byte[,] ControllersData, byte[,] SiteData, byte[,] KeyData, int lengthTable, int CountLine, Socket contr, EndPoint endPoint)
        {
            //Создаем подключение
            if (!contr.Connected) if (!contr.Connected)
                {
                    contr = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    contr.Connect(endPoint);
                }
            //Цикл по кол-ву контроллеров
            for (int i = 0; i < CountLine; i++)
            {
                //Цикл по длинне данных с контроллеров
                for (int r = 0; r <= lengthTable; r++)
                {
                    if (ControllersData[i, r] != SiteData[i, r] & KeyData[i, r] == KeyDB)
                    {
                        //Шаблон команды
                        byte[] command_com = { 0x00, 0x06, 0x00, 0x03, 0x00, 0x02, 0x00, 0x00 };
                        //Задаем SlaveId того уст-ва на которое передаем данные 
                        command_com[0] = SiteData[i, lengthTable + 1];
                        //Адрес регистра с данными
                        command_com[3] = Convert.ToByte(r);
                        //Данные
                        command_com[5] = SiteData[i, r];
                        //Контрольная сумма для команды
                        CRC.ModRTU_CRC(command_com, 6, out byte CRCHigh, out byte CRCLow);
                        command_com[7] = CRCHigh;
                        command_com[6] = CRCLow;
                        //Отладка по текстовым файлам
                        //string[] byteToStr = new string[command_com.Length];
                        //string FilePath = Directory.GetCurrentDirectory().ToString() + v + ".txt";
                        //v++;
                        //for (int j = 0; j < command_com.Length; j++)
                        //{
                        //    byteToStr[j] = command_com[j].ToString();
                        //}
                        //File.WriteAllLines(FilePath, byteToStr);

                        contr.Send(command_com);
                        //Ждем(иначе цикл перезаписывает отправляемые данные)
                        Thread.Sleep(500);
                    }
                }
            }
        }
    }
}
