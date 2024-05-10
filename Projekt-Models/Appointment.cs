using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Models
{
    public class Appointment
    {
        [Key]
        public int id { get; set; }

        public string AppointDiss { get; set; }

        public DateTime placedApp { get; set; }

        public int customerId { get; set; }
        // one to many
        public Customer customer { get; set; }
    }
}
