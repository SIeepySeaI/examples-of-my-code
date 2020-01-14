using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleAppTest
{
    /// <summary>
    /// Логика обновления данных на контроллере в административном здании
    /// </summary>
    class AdminLogic
    {
        int middleTemp;
        MainLogic mainLogic = new MainLogic();
        Database database = new Database();
        const int LENGTH_SERVICE_TABLE = 6;
        const int Key = 1;
        public void UpdateServiceData(string[] TableStructure, byte[,] ControllersData, int controllerQuantity, Socket contr, IPEndPoint endPoint)
        {
            //Определяем среднюю температуру исходя из количества контроллеров
            for (int i = 0; i < controllerQuantity; i++)
            {
                middleTemp += ControllersData[i, 0];
            }
            middleTemp = middleTemp / 3;

            //Запрашиваем из БД данные по дополнительной логике
            mainLogic.TakeAllTableFromDB(TableStructure[3], LENGTH_SERVICE_TABLE, Database.NumOfline, out byte[,] AdminMiddleTable);
            mainLogic.TakeAllTableFromDB(TableStructure[4], LENGTH_SERVICE_TABLE, Database.NumOfline, out byte[,] AdminMiddleUpdateTable);
            mainLogic.TakeAllTableFromDB(TableStructure[5], LENGTH_SERVICE_TABLE, Database.NumOfline, out byte[,] AdminMiddleKeyTable);
            ClearAdminKeyTable(TableStructure[5]);
            
            AdminMiddleTable[0, 0] = Convert.ToByte(middleTemp);
            for (int i = 0; i < LENGTH_SERVICE_TABLE; i++)
            {
                //Проверяем таблицы с доп. настройками на обновление
                if ((AdminMiddleUpdateTable[0, i] != AdminMiddleTable[0, i]) & AdminMiddleKeyTable[0, i] == Key)
                {
                    if (i == 3 | i == 4)
                    {
                        //Если поддерживаемая температура изначально выставлена по 1-му контроллеру
                        if (AdminMiddleTable[0, i] == 0)
                        {
                            SaveStateMainController(0, 3, TableStructure[0], TableStructure[0]);
                            if (i == 3)
                            {
                                SetMaxParam(TableStructure[1], TableStructure[2], true);
                            }
                            else SetMaxParam(TableStructure[1], TableStructure[2], false);

                        }
                        //Если поддерживаемая температура будет устанавливаться по 1-му контроллеру
                        else if (AdminMiddleUpdateTable[0, i] == 0)
                        {
                            SaveStateMainController(3, 0, TableStructure[0], TableStructure[1]);
                            SaveStateMainController(3, 0, TableStructure[2], TableStructure[2]);
                        }
                    }
                    AdminMiddleTable[0, i] = AdminMiddleUpdateTable[0, i];
                }
            }
            UpdateMiddleTemp(TableStructure[3], AdminMiddleTable);
            //Проверяем вкл ли поддержание по средней температуре в активном режиме
            if (AdminMiddleTable[0, 3] == 3)
            {
                ActiveStateHandler(middleTemp, AdminMiddleTable[0, 1], ControllersData[0, 12], contr, endPoint);
            }
            else if (AdminMiddleTable[0, 3] != 0)
            {
                for (int i = 1; i <= 2; i++)
                {
                    ActiveStateHandler(ControllersData[i, 0], AdminMiddleTable[0, 1], ControllersData[0, 12], contr, endPoint);
                }
            }
            else if (AdminMiddleTable[0, 4] == 3)
            {
                PassiveStateHandler(middleTemp, AdminMiddleTable[0, 2], contr, endPoint);
            }
            else if (AdminMiddleTable[0, 4] != 0)
            {
                for (int i = 1; i <= 2; i++)
                {
                    if (AdminMiddleTable[0, 4] == i) PassiveStateHandler(ControllersData[i, 0], AdminMiddleTable[0, 2], contr, endPoint);
                }
            }
        }
        /// <summary>
        /// Перезаписывает данные из одной строки в другую
        /// </summary>
        /// <param name="saveLine">Порядковый номер сохраняемой строки</param>
        /// <param name="savePlaceLine">Порядковый номер строки куда сохраняем</param>
        /// <param name="tableName">Имя таблицы с сохраняемой строкой</param>
        /// <param name="savePlacetableName">Имя таблицы со строкой куда сохраняем</param>
        void SaveStateMainController(int saveLine, int savePlaceLine, string tableName, string savePlacetableName)
        {
            string[] ColumsName = new string[Database.lengthByte];
            string updateColums = "";
            // устанавливаем соединение с БД
            mainLogic.conn.Open();
            //команда для получения имен строк
            string sql = "SHOW FIELDS FROM " + tableName;
            // запрос
            MySqlCommand command = new MySqlCommand(sql, mainLogic.conn);
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int j = 0;
            while (reader.Read())
            {
                ColumsName[j] = reader[0].ToString();
                j++;
            }
            mainLogic.conn.Close();
            mainLogic.TakeControllerDataFromDB(tableName, Database.lengthByte, Database.NumOfline, out byte[,] AdminTable);
            mainLogic.conn.Open();
            for (int k = 3; k <= MainLogic.ContrDataLength + 3; k++)
            {
                updateColums += " " + ColumsName[k] + " = " + AdminTable[saveLine, k - 3].ToString() + ",";
            }
            updateColums = updateColums.Substring(0, updateColums.Length - 1);
            sql = "UPDATE " + savePlacetableName + " SET " + updateColums + " WHERE " + " id = " + savePlaceLine.ToString();
            command = new MySqlCommand(sql, mainLogic.conn);
            //Выполняем запрос
            command.ExecuteNonQuery();

            updateColums = "";
            mainLogic.conn.Close();
        }
        /// <summary>
        /// Выставляет максимальные значения поддерживаемых температур
        /// </summary>
        /// <param name="updateData">Имя таблицы с данными на обновление</param>
        /// <param name="keyData">Имя таблицы с ключами разрешающими обновление </param>
        /// <param name="tempState">Режим поддерживаемой температуры. True - Активное поддержание температуры. False - пассивное</param>
        void SetMaxParam(string updateData, string keyData, bool tempState)
        {
            //Баг где-то здесь
            string[] сolumsName = new string[Database.lengthByte];
            int[] columsIndexPassive = { 13, 14 };
            int[] lineDataPassive = { 44, 1 };
            int[] сolumsIndexActive = { 4, 9, 13, 14 };
            int[] lineDataActive = { 100, 100, 4, 0 };
            string sqlUpdateRequest = "";
            string sqlKeyRequest = "";
            // устанавливаем соединение с БД
            mainLogic.conn.Open();
            //команда для получения имен строк
            string sql = "SHOW FIELDS FROM " + updateData;
            // запрос
            MySqlCommand command = new MySqlCommand(sql, mainLogic.conn);
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int j = 0;
            while (reader.Read())
            {
                сolumsName[j] = reader[0].ToString();
                j++;
            }
            mainLogic.conn.Close();
            mainLogic.conn.Open();
            if (tempState == true)
            {
                //Собираем запрос который выставит максимальные значения поддерживаемых температур и откроет клапан
                for (int k = 0; k < сolumsIndexActive.Length; k++)
                {
                    sqlUpdateRequest += " " + сolumsName[сolumsIndexActive[k]] + " = " + lineDataActive[k] + ",";
                    sqlKeyRequest += " " + сolumsName[сolumsIndexActive[k]] + " = " + 1 + ",";
                }
            }
            else
            {
                //Собираем запрос который выставит максимальные значения поддерживаемых температур и откроет клапан
                for (int k = 0; k < columsIndexPassive.Length; k++)
                {
                    sqlUpdateRequest += " " + сolumsName[columsIndexPassive[k]] + " = " + lineDataPassive[k] + ",";
                    sqlKeyRequest += " " + сolumsName[columsIndexPassive[k]] + " = " + 1 + ",";
                }
            }

            sqlUpdateRequest = sqlUpdateRequest.Substring(0, sqlUpdateRequest.Length - 1);
            sqlKeyRequest = sqlKeyRequest.Substring(0, sqlKeyRequest.Length - 1);
            sql = "UPDATE " + updateData + " SET " + sqlUpdateRequest + " WHERE " + " id = " + 0;
            command = new MySqlCommand(sql, mainLogic.conn);
            //Выполняем запрос
            command.ExecuteNonQuery();
            sql = "UPDATE " + keyData + " SET " + sqlKeyRequest + " WHERE " + " id = " + 0;
            command = new MySqlCommand(sql, mainLogic.conn);
            //Выполняем запрос
            command.ExecuteNonQuery();
            mainLogic.conn.Close();
        }
        void UpdateMiddleTemp(string mainTable, byte[,] mainAdminData)
        {
            string[] сolumsName = new string[Database.lengthByte];
            string sqlRequest = "";
            // устанавливаем соединение с БД
            mainLogic.conn.Open();
            //команда для получения имен строк
            string sql = "SHOW FIELDS FROM " + mainTable;
            // запрос
            MySqlCommand command = new MySqlCommand(sql, mainLogic.conn);
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int j = 0;
            while (reader.Read())
            {
                сolumsName[j] = reader[0].ToString();
                j++;
            }
            mainLogic.conn.Close();
            mainLogic.conn.Open();
            for (int k = 1; k < LENGTH_SERVICE_TABLE; k++)
            {
                sqlRequest += " " + сolumsName[k] + " = " + mainAdminData[0,k-1] + ",";
            }
            sqlRequest = sqlRequest.Substring(0, sqlRequest.Length - 1);
            sql = "UPDATE " + mainTable + " SET " + sqlRequest + " WHERE " + " id = " + 1;
            command = new MySqlCommand(sql, mainLogic.conn);
            //Выполняем запрос
            command.ExecuteNonQuery();
            mainLogic.conn.Close();
        }
        void ClearAdminKeyTable(string keyTableName)
        {
            string[] сolumsName = new string[Database.lengthByte];
            string sqlKeyRequest = "";
            // устанавливаем соединение с БД
            mainLogic.conn.Open();
            //команда для получения имен строк
            string sql = "SHOW FIELDS FROM " + keyTableName;
            // запрос
            MySqlCommand command = new MySqlCommand(sql, mainLogic.conn);
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            int j = 0;
            while (reader.Read())
            {
                сolumsName[j] = reader[0].ToString();
                j++;
            }
            mainLogic.conn.Close();
            mainLogic.conn.Open();
            //Собираем запрос который выставит максимальные значения поддерживаемых температур и откроет клапан
            for (int k = 1; k < LENGTH_SERVICE_TABLE; k++)
            {
                sqlKeyRequest += " " + сolumsName[k] + " = " + 0 + ",";
            }
            sqlKeyRequest = sqlKeyRequest.Substring(0, sqlKeyRequest.Length - 1);
            sql = "UPDATE " + keyTableName + " SET " + sqlKeyRequest + " WHERE " + " id = " + 1;
            command = new MySqlCommand(sql, mainLogic.conn);
            //Выполняем запрос
            command.ExecuteNonQuery();
            mainLogic.conn.Close();
        }
        /// <summary>
        /// Записывает данные в регистр 4-го контроллера
        /// </summary>
        /// <param name="dataPlace">Номер регистра</param>
        /// <param name="data">Байт с данными</param>
        /// <param name="contr">Объект сокета</param>
        /// <param name="endPoint">Адрес конвектора</param>
        void WriteSingleRegister(byte dataPlace, byte data, Socket contr, IPEndPoint endPoint)
        {
            //Шаблон команды
            byte[] commandCom = { 0x04, 0x06, 0x00, 0x03, 0x00, 0x02, 0x00, 0x00 };
            //Адрес регистра с данными
            commandCom[3] = dataPlace;
            //Данные
            commandCom[5] = data;
            //Контрольная сумма для команды
            CRC.ModRTU_CRC(commandCom, 6, out byte CRCHigh, out byte CRCLow);
            commandCom[7] = CRCHigh;
            commandCom[6] = CRCLow;
            //Создаем подключение
            if (!contr.Connected)
            {
                contr = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                contr.Connect(endPoint);
            }
            contr.Send(commandCom);
            //Thread.Sleep(50);
            contr.Close();
        }
        /// <summary>
        /// Управляет активным состоянием в зависимости от выбранного режима работы и температуры
        /// </summary>
        /// <param name="tempNow">Действительная температура</param>
        /// <param name="setTemp">Температура которую должен поддерживать контроллер</param>
        /// <param name="sysStatus">Байт отвечающий за режим работы</param>
        /// <param name="contr">Объект сокета</param>
        /// <param name="endPoint">Адресс куда отправляем данные</param>
        void ActiveStateHandler(int tempNow, byte setTemp, byte sysStatus, Socket contr, IPEndPoint endPoint)
        {
            //Если средняя температура больше выставленной в активном режиме
            if (tempNow >= setTemp)
            {
                //Данные при которых контроллер работает в активном режиме
                byte[] currectActiveStat = { 2, 3, 6, 7, 10, 11, 14, 15 };
                //Если включен режим активного поддержания температуры
                for (int i = 0; i < currectActiveStat.Length; i++)
                {
                    if (sysStatus == currectActiveStat[i])
                    {
                        //Значения при которых включен режим работы по периодам
                        if (i == 3 | i == 5 | i == 7)
                        {
                            //Вырубаем активное поддержание температуры
                            WriteSingleRegister(0x0c, 0x05, contr, endPoint);
                            break;
                        }
                        //При всех остальных совпадениях
                        else
                        {
                            //Вырубаем активное поддержание температуры
                            WriteSingleRegister(0x0c, 0x04, contr, endPoint);
                            break;
                        }
                    }
                }

            }
            //Если средняя температура меньше заданной
            else if (tempNow < setTemp)
            {
                byte[] currectPeriodStat = { 1, 3, 5, 7, 9, 11, 13, 15 };

                int markerOffState = 0;
                for (int i = 0; i < currectPeriodStat.Length; i++)
                {
                    if (sysStatus == currectPeriodStat[i])
                    {
                        if (sysStatus == currectPeriodStat[i])
                        {
                            WriteSingleRegister(0x0c, 0x07, contr, endPoint);
                            break;
                        }
                        else
                        {
                            markerOffState++;
                        }
                    }
                }
                //Если реж. работы по периодам не включен
                if (markerOffState == currectPeriodStat.Length)
                {
                    WriteSingleRegister(0x0c, 0x06, contr, endPoint);
                }
            }
        }
        /// <summary>
        /// Управляет контроллером в пассивном состояниии в зависимости от выбранного режима работы и температуры
        /// </summary>
        /// <param name="tempNow">Действительная температура</param>
        /// <param name="setTemp">Температура которую должен поддерживать контроллер</param>
        /// <param name="contr">Объект сокета</param>
        /// <param name="endPoint">Адресс куда отправляем данные</param>
        void PassiveStateHandler(int tempNow, byte setTemp, Socket contr, IPEndPoint endPoint)
        {
            //Если средняя температура больше выставленной в пассивном режиме
            if (tempNow >= setTemp)
            {
                //Закрываем клапан на трубах
                WriteSingleRegister(0x0a, 0x00, contr, endPoint);
            }
            //Если меньше
            else if (tempNow < setTemp)
            {
                //Открываем клапан
                WriteSingleRegister(0x0a, 0x01, contr, endPoint);
            }
        }

    }

}
