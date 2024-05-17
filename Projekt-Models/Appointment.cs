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

        public string AppointDiscription { get; set; }

        public DateTime PlacedApp { get; set; }

        public int CustomerId { get; set; }
        public int CompanyId { get; set; }

        // one to many
        public Customer Customer { get; set; }
        public Company Company { get; set; }

        public List<BookingHistory> BookingHistories { get; set; }


    }
}
