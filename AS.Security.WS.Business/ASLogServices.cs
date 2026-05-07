using System;
using System.Collections.Generic;
using System.Text;


using AS.Security.WS.Entities;
using AS.Security.WS.Data;
using System.Data;
using AS.Common.DBManager;

namespace AS.Security.WS.Business
{
    public class ASLogServices
    {
        readonly AsLogDao _Log;

        public ASLogServices(string connString)
        {
            _Log = new AsLogDao(connString);
        }
        
        public int InsertASPXTrackingLog(AspxTracking item)
        {
            return _Log.Insert(item);
        }
    }
}
