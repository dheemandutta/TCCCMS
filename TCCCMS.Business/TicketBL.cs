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

        public List<Ticket> GetAllTicketPageWise(int pageIndex, ref int recordCount, int length/*, int VesselID*/)
        {
            TicketDAL dAL = new TicketDAL();
            return dAL.GetAllTicketPageWise(pageIndex, ref recordCount, length/*, VesselID*/);
        }
    }
}
