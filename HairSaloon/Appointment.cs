using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HairSaloon
{
    public class Appointment
    {
        [XmlElement("timeSlot")]
        public string timeSlot { get; set; }
        [XmlElement("Gender")]
        public string Gender { get; set; }
        [XmlElement("Customer")]
        public Customer Customer { get; set; }
        [XmlElement("Services")]
        public string Services { get; set; }
        [XmlElement("Price")]
        public float TotalPayment { get; set; }
    }
}
