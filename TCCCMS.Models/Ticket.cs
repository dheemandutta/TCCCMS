using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
        public string Error { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string Email { get; set; }
        public int IsSolved { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
