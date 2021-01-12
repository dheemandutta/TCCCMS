using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Business
{
    public class ShipBL
    {
        public List<Ship> GetAllShipsPageWise(int pageIndex, ref int totalCount, int pageSize)
        {
            List<Ship> shipsList = new List<Ship>();
            ShipDAL shipDal = new ShipDAL();
            shipsList = shipDal.GetAllShipsPageWise(pageIndex, ref totalCount, pageSize);

            return shipsList;
        }

        public int SaveShipDetails(Ship ship)
        {

            ShipDAL shipBl = new ShipDAL();

            return shipBl.SaveShipDetails(ship);
        }
        public Ship GetShipDetailsById(int shipId)
        {
            ShipDAL shipDal = new ShipDAL();
            //Ship ship = new Ship();

            return shipDal.GetShipDetailsById(shipId);        }

        #region DropDown---
        public List<VesselType> GetVesselTypeListForDopDown()
        {
            List<VesselType> vesselTypeList = new List<VesselType>();
            ShipDAL shipDal = new ShipDAL();
            return shipDal.GetVesselTypeListForDopDown();
        }
        public List<VesselSubType> GetVesselSubTypeListByTypeForDopDown(int vesselTypeId)
        {
            List<VesselSubType> vesselTypeList = new List<VesselSubType>();
            ShipDAL shipDal = new ShipDAL();
            return shipDal.GetVesselSubTypeListByTypeForDopDown(vesselTypeId);
        }
        public List<VesselSubSubType> GetVesselSubSubTypeListBySubTypeForDopDown(int vesselSubTypeId)
        {
            List<VesselSubSubType> vesselTypeList = new List<VesselSubSubType>();
            ShipDAL shipDal = new ShipDAL();
            return shipDal.GetVesselSubSubTypeListBySubTypeForDopDown(vesselSubTypeId);
        }
        #endregion

    }
}
