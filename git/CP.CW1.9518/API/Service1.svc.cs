using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace API
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class Service1 : IService1
    {
        public string SaveToDB(string flightNumber, string destination, string departureDate, string status, string gate)
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(directoryPath, "output.txt");

            // Save to output.txt file
            File.AppendAllText(filePath, String.Format("{0},{1},{2},{3},{4}\n", flightNumber, destination, departureDate, status, gate));
            return string.Format("Data save for flight: {0}", flightNumber);
        }
    }
}
