using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SciencetechDeviceController.Model
{
    public class DeviceSettings
    {
        [XmlElement]
        public int BaudRate;
        [XmlElement]
        public int Parity;
        [XmlElement]
        public int Handshaking;
        [XmlElement]
        public int Stopbits;
        [XmlElement]
        public int Databits;
        [XmlElement]
        public string TerminationCharacter;
        [XmlElement]
        public string PortName;
    }
}
