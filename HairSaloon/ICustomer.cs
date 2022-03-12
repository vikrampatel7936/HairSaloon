using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSaloon
{
    public interface ICustomer
    {
        string Name { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        int Age { get; set; }
        string CreditCard { get; set; }
        string Vaccinated { get; set; }
    }
}
