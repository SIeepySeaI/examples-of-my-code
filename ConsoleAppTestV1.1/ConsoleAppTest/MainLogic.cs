using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

using MySql.Data.MySqlClient;

namespace ConsoleAppTest
{
    /// <summary>
    /// Предоставляет порт для взаимодействия с контроллером
    /// </summary>
    class ControllerPort
    {
        /// <summary>
        /// Инициализирует и возвращает сокет по данному ip
        /// </summary>
        /// <param name="IPaddress">ip-адрес</param>
        /// <returns>Cокет для обмена данными с контроллером</returns>

        public Socket SocketInitialization(string IPaddress)
        {

            IPAddress IP = IPAddress.Parse(IPaddress);

            Socket ServSender = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            return ServSender;
        }
        /// <summary>
        /// Инициализирует и возвращает точку подключения контроллера
        /// </summary>
        /// <param name="IPaddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public IPEndPoint EndPointInitialization(string IPaddress, int port)
        {
            IPAddress Ip = IPAddress.Parse(IPaddress);
            IPEndPoint endpoint = new IPEndPoint(Ip, port);
            return endpoint;
        }
    }
    class Database
    {
        //ВАЖНО
        //Константы определяющие размеры массивов с табличными данными
        public const int NumOfline = 8;
        public const int lengthByte = 40;
        public byte[,] byteFromDB = new byte[NumOfline, lengthByte];
        public byte[,] keyFromDB = new byte[NumOfline, lengthByte];
    }
    class MainLogic
    {
        //длина строки таблицы
        const int lengthMessDB = 32;
        //Неактивное состояние ключа
        const int DefaultUpdateKey = 0;
        //Количество контроллеров
        const int ControllersItem = 8;
        //перменная количества байт
        const int lengthMessContr = 60;
        const int quantityTableDefault = 3;
        //Количество таблиц при котором задействуется логика  
        const int quantityTableAdministration = 6;
        public const int ContrDataLength = 27;

        public byte[] bytes = new byte[1024];

        public const int BUFFER_SIZE = 200;
        static public byte[] buffer = new byte[BUFFER_SIZE];

        //int ContrDataLength = 27;

        byte[,] AllControllers = new byte[ControllersItem, lengthMessContr];
        int CounterControllers = 0;
        //string testingOutData;
        public Mutex mutexDoneAsync = new Mutex();
        public static ControllerPort port = new ControllerPort();
        public static Comparator comp = new Comparator();
        public static Database OurDB = new Database();
        public static IAsyncResult Reseived;
        // строка подключения к БД
        const string connStr = "server=0.0.0.0;user=****;database=****;password=****;";
        // создаём объект для подключения к БД
        public readonly MySqlConnection conn = new MySqlConnection(connStr);
        //Переменная с названиями столбцов
        string[] ColumsName = new string[200];
        static int waitCounter = 0;
        string updateColums;
        public Socket controllerAddress;
        public IPEndPoint endPoint;


        /// <summary>
        /// Обновление данных на контроллерах и таблицах
        /// </summary>
        /// <param name="ip">Адресс устройства </param>
        /// <param name="socketPort">Объект сокета через ктоторый передаем данные на контроллеры</param>
        /// <param name="controllerQuantity">Количество контроллеров</param>
        /// <param name="tableSructure">Массив с названиями таблиц вида:актуальные данные, данные на обновление, ключи разрешающие обновление</param>
        public void UpdateControllerData(string ip, int socketPort, int controllerQuantity, string[] tableSructure)
        {
            //Console.WriteLine("Узнать SlaveID устройств?");
            //Console.ReadLine();
            TakeAllTableFromDB(tableSructure[0],lengthMessDB, controllerQuantity, out byte[,] SlaveID);
            //Console.WriteLine("Запросить данные c контроллеров?");
            //Console.ReadLine();
            controllerAddress = port.SocketInitialization(ip);
            endPoint = port.EndPointInitialization(ip, socketPort);
            RequestAllController(SlaveID, controllerQuantity);
            //Вывод массива для отладки
            //testingOutData = "";
            for (int i = 0; i < controllerQuantity; i++)
            {
                for (int k = 0; k <= ContrDataLength; k++)
                {
                    //testingOutData += AllControllers[i, k] + " ";
                }
                //Console.WriteLine(testingOutData + "\n");
                //testingOutData = "";
            }
            //Console.WriteLine("Записать в базу данных?");
            //Console.ReadLine();
            UpdateDB(tableSructure[0], controllerQuantity);
            if (tableSructure.Length == quantityTableAdministration)
            {
                AdminLogic admin = new AdminLogic();
                admin.UpdateServiceData(tableSructure, AllControllers, controllerQuantity, controllerAddress, endPoint);
            }

            //Console.WriteLine("Запросить данные на обновление?");
            //Console.ReadLine();
            DataBaseRequest(tableSructure[1], tableSructure[2], controllerQuantity);

            //Console.WriteLine("Сравнить и отправить данные на контроллеры?");
            //Console.ReadLine();
            comp.CompareData(AllControllers, OurDB.byteFromDB, OurDB.keyFromDB, ContrDataLength, controllerQuantity, controllerAddress, endPoint);
        }
        /// <summary>
        /// Опрашивает все контроллеры
        /// </summary>
        /// <param name="SlaveID">Таблица содержащяя индефикаторы контроллеров</param>
        /// <param name="controllerQuantity">Количество контроллеров</param>
        void RequestAllController(byte[,] SlaveID, int controllerQuantity)
        {

            CounterControllers = 0;
            while (CounterControllers < controllerQuantity)
            {
                ControllerRequest(SlaveID[CounterControllers,lengthMessDB-2]);
                mutexDoneAsync.WaitOne();
                while (!Reseived.IsCompleted)
                {
                    if (waitCounter == 10)
                    {
                        controllerAddress.EndReceive(Reseived);
                        break;
                    }
                    Thread.Sleep(500);

                    waitCounter++;
                }
                if (waitCounter == 10)
                {
                    waitCounter = 0;
                }
                else
                {
                    CounterControllers++;
                }
                mutexDoneAsync.ReleaseMutex();
            }
        }
        /// <summary>
        /// Обновляет таблицу с актуальными данными с контроллеров
        /// </summary>
        /// <param name="DBTableNameData">Имя таблицы</param>
        private void UpdateDB(string DBTableNameData, int controllerQuantity)
        
        {
            // устанавливаем соединение с БД
            conn.Open();
            //команда для получения имен строк
            string sql = "SHOW FIELDS FROM " + DBTableNameData;
            // запрос
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int j = 0;
            while (reader.Read())
            {
                ColumsName[j] = reader[0].ToString();
                //testingOutData += ColumsName[j] + " ";
                j++;
            }
            //Console.WriteLine(testingOutData);
            //testingOutData = "";
            conn.Close();
            for (int i = 0; i < controllerQuantity; i++)
            {
                conn.Open();
                for (int k = 3; k <= ContrDataLength + 3; k++)
                {
                    updateColums += " " + ColumsName[k] + " = " + AllControllers[i, k - 3].ToString() + ",";
                }
                updateColums = updateColums.Substring(0, updateColums.Length - 1);
                sql = "UPDATE " + DBTableNameData + " SET " + updateColums + " WHERE " + " id = " + i.ToString();
                command = new MySqlCommand(sql, conn);
                //Выполняем запрос
                command.ExecuteNonQuery();

                updateColums = "";
                //TempNow,WorkStatus,  16 => "TimeMinutes", "TimeHours"
                conn.Close();
            }
        }
        /// <summary>
        /// Возвращает массив со всеми данными с полученными с контроллеров таблицы
        /// </summary>
        /// <param name="TableName">Имя таблицы</param>
        /// <param name="ArrayLength">Длина строки</param>
        /// <param name="NumOfLine">Количество строк</param>
        /// <param name="DataFromTable">Возвращаемый массив с данными</param>
        public void TakeControllerDataFromDB(string TableName, int ArrayLength, int NumOfLine, out byte[,] DataFromTable)
        {
            DataFromTable = new byte[NumOfLine, ArrayLength];
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = "SELECT * FROM " + TableName;
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int k = 0;
            while (reader.Read())
            {
                //testingOutData = "";
                for (int i = 3; i < lengthMessDB; i++)
                {
                    // -3 первых значений не нужных для сравнения
                    DataFromTable[k, i - 3] = Convert.ToByte(reader.GetInt16(i));
                    //testingOutData += DataFromTable[k, i - 3].ToString() + " ";
                }
                //Console.WriteLine(testingOutData);
                //testingOutData = "";
                k++;
            }
            k = 0;
            reader.Close();
            conn.Close();
        }
        public void TakeAllTableFromDB(string TableName, int ArrayLength, int NumOfLine, out byte[,] DataFromTable)
        {
            DataFromTable = new byte[NumOfLine+1, ArrayLength];
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = "SELECT * FROM " + TableName;
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int k = 0;
            while (reader.Read())
            {
                //testingOutData = "";
                for (int i = 1; i < ArrayLength; i++)
                {
                    try
                    {
                        DataFromTable[k, i - 1] = Convert.ToByte(reader.GetInt16(i));
                    }
                    catch (Exception ex)
                    {
                        DataFromTable[k, i - 1] = 1;
                    }

                    //testingOutData += DataFromTable[k, i - 1].ToString() + " ";
                }
                //Console.WriteLine(testingOutData);
                //testingOutData = "";
                k++;
            }
            k = 0;
            reader.Close();
            conn.Close();
        }
        /// <summary>
        /// Запрашивает данные на обновление и ключи, а затем отчищает таблицу с ключами
        /// </summary>
        /// <param name="DBTableName">Имя таблицы с данными на обновление</param>
        /// <param name="DBTableNameKey">Имя таблицы с ключами на обновление</param>
        void DataBaseRequest(string DBTableName, string DBTableNameKey, int controllerQuantity)
        {
            TakeControllerDataFromDB(DBTableName, lengthMessDB, ControllersItem, out OurDB.byteFromDB);

            TakeControllerDataFromDB(DBTableNameKey, lengthMessDB, ControllersItem, out OurDB.keyFromDB);
            string sql;
            // объект для выполнения SQL-запроса
            MySqlCommand command;
            // объект для чтения ответа сервера
            MySqlDataReader reader;
            //проверяем таблицу с ключами на изменение
            conn.Open();
            sql = "SHOW FIELDS FROM " + DBTableNameKey;
            // запрос
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            // читаем результат
            int j = 0;
            while (reader.Read())
            {
                ColumsName[j] = reader[0].ToString();
                //testingOutData += ColumsName[j] + " ";
                j++;
            }
            reader.Close();
            for (int l = 3; l <= ContrDataLength + 3; l++)
            {
                updateColums += " " + ColumsName[l] + " = " + DefaultUpdateKey + ",";
            }
            updateColums = updateColums.Substring(0, updateColums.Length - 1);
            sql = "UPDATE " + DBTableNameKey + " SET " + updateColums + " WHERE ";
            updateColums = "";
            for (int i = 0; i < controllerQuantity; i++)
            {
                sql += " id = " + i.ToString() + " or";
            }
            sql = sql.Substring(0, sql.Length - 2);

            command = new MySqlCommand(sql, conn);
            command.ExecuteNonQuery();
            // закрываем соединение с БД
            conn.Close();

        }
        /// <summary>
        /// Отправляет запрос контроллеру и определяет асинхронный метод для обработки ответа
        /// </summary>
        /// <param name="SlaveID">Индефикатор устройства</param>
        public void ControllerRequest(byte SlaveID)
        {
            //buffer = new byte[BUFFER_SIZE];
            //Открываем соединение
            if (!controllerAddress.Connected) controllerAddress.Connect(endPoint);
            byte[] command_com = { 0x00, 0x03, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00 };
            command_com[0] = SlaveID;
            //Вычисляем контрольную сумму для последних  2-х байт
            CRC.ModRTU_CRC(command_com, 6, out byte CRCHigh, out byte CRCLow);
            command_com[7] = CRCHigh;
            command_com[6] = CRCLow;
            //Отладка по текстовым файлам
            //string[] byteToStr = new string[command_com.Length];
            //string FilePath = Directory.GetCurrentDirectory().ToString() + v + ".txt";
            //for (int i = 0; i < command_com.Length; i++)
            //{
            //    byteToStr[i] = command_com[i].ToString();
            //}
            //File.WriteAllLines(FilePath, byteToStr);
            //Запрашиваем массив с информацией с контроллера
            controllerAddress.Send(command_com);
            //Записываем массив в buffer
            Reseived = controllerAddress.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, OnPacketReceived, controllerAddress);
        }
        public void OnPacketReceived(IAsyncResult ar)
        {
            int evenOrOdd = 0;
            controllerAddress.EndReceive(ar);
            CRC.ModRTU_CRC(buffer, lengthMessContr - 1, out byte CRCHigh, out byte CRCLow);
            if (CRCHigh != buffer[lengthMessContr] | CRCLow != buffer[lengthMessContr - 1])
            {
                //Console.WriteLine("Ошибка контрольной суммы!");
                waitCounter = 10;
                return;
            }
            for (int i = 3; i < lengthMessContr - 1; i++)
            {
                if (i % 2 == 0)
                {
                    AllControllers[CounterControllers, i - evenOrOdd - 3] = buffer[i];
                    //testingOutData += AllControllers[CounterControllers - 3, i - evenOrOdd] + " ";
                }
                else
                {
                    evenOrOdd++;
                }

            }

            //Console.WriteLine(testingOutData + "\n");
            //testingOutData = "";
        }
    }
}
