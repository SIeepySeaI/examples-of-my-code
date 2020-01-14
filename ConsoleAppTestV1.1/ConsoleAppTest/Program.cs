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
    class Program
    {
        string[] TableStructure = { "controllers_data" , "controllers_data_update", "controllers_key_update" };
        string IPaddress = "0.0.0.0";
        int connectionPort =3;
        int quantityDevice = 8;
        static void Main(string[] args)
        {
            Program pr = new Program();
            MainLogic workshop = new MainLogic();
            MainLogic adminBild = new MainLogic();
            workshop.UpdateControllerData(pr.IPaddress, pr.connectionPort, pr.quantityDevice, pr.TableStructure);
            pr.TableStructure = new string[] 
            { 
                "administration_controllers_data",
                "administration_controllers_update",
                "administration_controllers_key_update",
                "administration_term_control_service",
                "administration_term_control_service_update",
                "administration_term_control_service_key" 
            };
            pr.IPaddress = "0.0.0.0";
            pr.quantityDevice = 3;
            adminBild.UpdateControllerData(pr.IPaddress, pr.connectionPort, pr.quantityDevice, pr.TableStructure);
        }
    }
}
