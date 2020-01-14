using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
// для COM-порта
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using InteractiveDataDisplay.WPF;
// для связи через TCP-IP
using System.Net;
using System.Net.Sockets;

namespace COM_Port_Ed
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] messByte;
        int i = 0;
        int x;
        int byteRecieved;
        static string IPadress = "192.168.0.7";
        static int Port = 80;
        private SerialPort comPort;
        // переменные для TCP-IP
        //private IPAddress IP = new IPAddress.Parse(IPadress);
        //private EndPoint endpoint;//point  
        //private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//socket  
        static TcpClient ConvClient = new TcpClient(IPadress, Port);
        NetworkStream stream = ConvClient.GetStream();

        public MainWindow()
        {
            //this.comPort = new SerialPort();
            //this.comPort.Parity = Parity.Odd;
            //this.comPort.PortName = "COM3";
            //this.comPort.DataReceived += new SerialDataReceivedEventHandler(this.comPort_DataReceived);
            InitializeComponent();
            timer_set();
            setting_set();

        }


        // Расчет 16-ти битной контрольной суммы для ModBus-протокола
        public static void ModRTU_CRC(byte[] command_com, int length, out byte CRCHigh, out byte CRCLow)
        {
            ushort CRCFull = 0xFFFF;

                for (int pos = 0; pos< length; pos++)
                {
                    CRCFull ^= (ushort) command_com[pos];

                    for (int i = 8; i != 0; i--)
                    {
                        if ((CRCFull & 0x0001) != 0)
                        {
                            CRCFull >>= 1;
                            CRCFull ^= 0xA001;
                        }
                        else
                            CRCFull >>= 1;
                    }
                }
            CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRCLow = (byte)(CRCFull & 0xFF);
        }
        private void timer_set()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }
        // Запрос данных при включеннии
        private void setting_set()
        {

            byte[] command_com = { 0x0a, 0x03, 0x00, 0x00, 0x00, 0x1c, 0x00, 0x00 };
            ModRTU_CRC(command_com, 6, out byte CRCHigh, out byte CRCLow);
            command_com[7] = CRCHigh;
            command_com[6] = CRCLow;
            messByte = new int[200];

            //if (!comPort.IsOpen) comPort.Open();
            //comPort.Write(command_com, 0, 8);

            Thread.Sleep(450);
            //comPort.Read(messByte, 0, byteRecieved);


            

            //Данные с температурного датчика
            int tem_out = messByte[4] / 2;
            data_Temp.Text = (tem_out + "°C");
            // установленная температура
            tem_out = messByte[6] / 2;
            Temp_set.Text = (tem_out + "°C");
            //режим работы
            if (messByte[8] == 1)
            {
                work_Mod_1.IsChecked = true;
                work_Mod_2.IsChecked = false;
            }
            else
            {
                work_Mod_1.IsChecked = false;
                work_Mod_2.IsChecked = true;
            }
            //режим работы вентилятора
            if (messByte[10] == 0)
            {
                fan_Mod_0.IsChecked = true;

            }
                
            else if (messByte[10] == 1)
            {

                fan_Mod_1.IsChecked = true;

            }
            else if (messByte[10] == 2)
            {

                fan_Mod_2.IsChecked = true;

            }
            else if (messByte[10] == 2)
            {
                fan_Mod_3.IsChecked = true;
            }
            //Блокировка режима работы
            if (messByte[12] == 0)
                Sys_Mod_0.IsChecked = true;
            else if (messByte[12] == 1)
                Sys_Mod_1.IsChecked = true;
            else if (messByte[12] == 2)
                Sys_Mod_2.IsChecked = true;
            i = 0;
            x = 0;
        }
        private void comPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            byteRecieved = comPort.BytesToRead;

            x = byteRecieved + x;
            //x = byteRecieved;
            for (; i <= x; i++)
            {

                messByte[i] = comPort.ReadByte();


            }
            i = x + 1;
            
            //indata = serialPort1.ReadExisting();
        }

        // Сверение данных с датчиком
        private void timerTick(object sender, EventArgs e)
        {
            setting_set();
        }

        private void fan_mod_Checked(object sender, RoutedEventArgs e)
        {
            byte[] command_com = { 0x0a, 0x06, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00 };
            if(fan_Mod_0.IsChecked == true)
                command_com[5] = 0x00;
            else if (fan_Mod_1.IsChecked == true)
                command_com[5] = 0x01;
            else if (fan_Mod_2.IsChecked == true)
                command_com[5] = 0x02;
            else if (fan_Mod_3.IsChecked == true)
                command_com[5] = 0x03;
            set_command(command_com);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void work_mod_Checked(object sender, RoutedEventArgs e)
        {
            byte[] command_com = { 0x0a, 0x06, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00 };
            if (work_Mod_1.IsChecked == true)
                command_com[5] = 0x01;
            else
                command_com[5] = 0x02;
            set_command(command_com);
        }
        private void set_command(byte[] command_com)
        {
            ModRTU_CRC(command_com, 6, out byte CRCHigh, out byte CRCLow);
            command_com[7] = CRCHigh;
            command_com[6] = CRCLow;
            //if (!comPort.IsOpen) comPort.Open();
            //comPort.Write(command_com, 0, 8);
            //stream.Write(command_com, 0, 8);
        }

        private void Sys_mod_Checked(object sender, RoutedEventArgs e)
        {
            byte[] command_com = { 0x0a, 0x06, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00 };
            if (Sys_Mod_0.IsChecked == true)
                command_com[5] = 0x00;
            else if (Sys_Mod_1.IsChecked == true)
                command_com[5] = 0x01;
            else if (Sys_Mod_2.IsChecked == true)
                command_com[5] = 0x02;
            set_command(command_com);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bit_c.Text = null;
            for (int y = 0; y < 200; y++)
            {
                bit_c.Text += messByte[y] + " ";
            }

        }


    }
}
