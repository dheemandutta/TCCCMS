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
        public string SaveTicket(Ticket ticket,int userType)
        {
            TicketDAL ticketDal = new TicketDAL();

            return ticketDal.SaveTicket(ticket, userType);
        
        }
        
    }
}
