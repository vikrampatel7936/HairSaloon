using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HairSaloon
{
    [Serializable]
    [XmlInclude(typeof(Men)), XmlInclude(typeof(Women)), XmlInclude(typeof(Kids))]
    public class Customer:ICustomer
    {
        private string name;
        private int age;
        private string phoneNumber;
        private string email;
        private string creditCard;
        private string vaccinated;

        [XmlElement("Name")]
        public string Name { get => name; set => name = value; }
        [XmlElement("Age")]
        public int Age { get => age; set => age = value; }
        [XmlElement("Phone Number")]
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Email { get => email; set => email = value; }
        [XmlElement("CreditCard")]
        public string CreditCard 
        {
            get => creditCard;
            set
            {
                //length is 16 when user enters card number 
                if (value.Length == 16)
                {
                    creditCard = $"{value.Substring(0, 4)} XXXX XXXX {value.Substring(12, 4)}";
                }
                //length is 19 while fetching from xml file as it includes 3 empty spaces
                else if (value.Length == 19)
                {
                    creditCard = $"{value.Substring(0, 4)} XXXX XXXX {value.Substring(15, 4)}";
                }
            }
        }
        [XmlElement("Vaccinated")]
        public string Vaccinated { get => vaccinated; set => vaccinated = value; }

        public Customer()
        {

        }
        public Customer(string name, int age, string phoneNumber, string email, string creditCard, string vaccinated)
        {
            Name = name;
            Age = age;
            PhoneNumber = phoneNumber;
            Email = email;
            CreditCard = creditCard;
            Vaccinated = vaccinated;
        }
    }
}
