using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentControl.Model
{
    public class Equipment
    {
        public string Name { get; set; }
        public string Number { get; set; } = "-нет номера-";
        public DateTime ? DateOfLastVerification { get; set; }
        public DateTime ? DateOfNextVerification { get; set; }
        public string Adres { get; set; }
        public string NameCompany { get; set; }

        public Equipment() { }

        public Equipment (string name, string number, DateTime dateOfLastVerification,
            DateTime dateOfNextVerification, string adres, string nameCompany)
        {
            Name = name;
            Number = number;
            DateOfLastVerification = dateOfLastVerification;
            DateOfNextVerification = dateOfNextVerification;
            Adres = adres;
            NameCompany = nameCompany;
        }

       
    }
}
