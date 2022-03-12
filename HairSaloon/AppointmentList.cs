using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HairSaloon
{
    public class AppointmentList : IDisposable
    {
        private List<Appointment> appointmentsList = null;

        [XmlArray("AppointmentsList")]
        [XmlArrayItem("Appointment", typeof(Appointment))]
        public List<Appointment> AppointmentsList { get => appointmentsList; set => appointmentsList = value; }

        public AppointmentList()    
        {
            AppointmentsList = new List<Appointment>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Appointment this[int i]
        {
            get { return AppointmentsList[i]; }
        }
    }
}
