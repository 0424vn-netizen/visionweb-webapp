using System;
using System.Collections.Generic;
using System.Text;

using AS.Security.WS.Entities;
using AS.Security.WS.Data;

namespace AS.Security.WS.Business
{
	public class JumpSiteServices
	{
        readonly JumpSiteDao _JumpSiteDAO;
        public JumpSiteServices(string connString)
        {
            _JumpSiteDAO = new JumpSiteDao(connString);
        }
		///Description for GetJumpSite
		///Author:
		///Date: 
		public  JumpSite GetJumpSite(long redId )
		{
			
			return _JumpSiteDAO.GetJumpSite(redId);
		}

        public bool CheckJumpSite(JumpSiteModel jumsiteModel)
        {
            return _JumpSiteDAO.CheckJumpSite(jumsiteModel);
        }
		
		///Description for InsertJumpSite
		///Author:
		///Date: 
		public  void InsertJumpSite(Guid userId, string remoteIP, int desSysId, string tempPassword, out long redId, Guid jumber )
		{
			
            _JumpSiteDAO.InsertJumpSite(userId, remoteIP, desSysId, tempPassword, out redId, jumber);
		}
		
		///Description for UpdateJumpSite
		///Author:
		///Date: 
		public  void UpdateJumpSite(long redId, Guid userId, string remoteIP, int desSysId, string tempPassword, DateTime createdTimeDTS )
		{
			
			_JumpSiteDAO.UpdateJumpSite(redId, userId, remoteIP, desSysId, tempPassword, createdTimeDTS);
		}
		
		///Description for DeleteJumpSite
		///Author:
		///Date: 
		public  void DeleteJumpSite(long redId )
		{
			
			_JumpSiteDAO.DeleteJumpSite(redId);
		}
	}
}