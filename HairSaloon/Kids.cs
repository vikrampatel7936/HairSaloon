using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HairSaloon
{
    [Serializable]
    public class Kids : Customer
    {
        private Dictionary<string, float> styles = new Dictionary<string, float>();

        [XmlIgnore]
        public Dictionary<string, float> Styles
        {
            get { return styles; }
            private set { styles = value; }
        }

        public Kids()
        {
            //adding services to dictionary and their prices
            Styles.Add("Trim Hair", 15);
            Styles.Add("Hair Color", 20);
            Styles.Add("Hair Wash", 10);
        }

        //parameterized constructor also calling base class parameterized constructor
        public Kids(string name, int age, string phoneNumber, string email, string creditCard, string vaccinated) : base(name, age, phoneNumber, email, creditCard, vaccinated)
        {
            //adding services to dictionary and their prices
            Styles.Add("Trim Hair", 15);
            Styles.Add("Hair Color", 20);
            Styles.Add("Hair Wash", 10);
        }
    }
}
