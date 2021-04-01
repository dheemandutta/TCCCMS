using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TCCCMS.Models;
using TCCCMS.Data;

namespace TCCCMS.Business
{
    public class TicketBL
    {
        public int SaveTicket(Ticket ticket)
        {
            TicketDAL ticketDal = new TicketDAL();

            return ticketDal.SaveTicket(ticket);
        
        }
        
    }
}
