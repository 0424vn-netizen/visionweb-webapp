using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AS.Security.WS.Entities;
using AS.Common.DBManager;

namespace AS.Security.WS.Data
{
    public class JumpSiteDao : BaseSecDao
	{
		///Description for BindJumpSite
		///Author:
		///Date:  

        public JumpSiteDao(string connString) : base(connString) { }

        public JumpSite BindJumpSite(IDataReader reader)
		{
			JumpSite item = new JumpSite();
			if (!reader.IsDBNull(reader.GetOrdinal("RedId"))) item.RedId = (long)reader["RedId"];		
			if (!reader.IsDBNull(reader.GetOrdinal("UserId"))) item.UserId = (Guid)reader["UserId"];		
			if (!reader.IsDBNull(reader.GetOrdinal("RemoteIP"))) item.RemoteIP = (string)reader["RemoteIP"];		
			if (!reader.IsDBNull(reader.GetOrdinal("DesSysId"))) item.DesSysId = (int)reader["DesSysId"];		
			if (!reader.IsDBNull(reader.GetOrdinal("TempPassword"))) item.TempPassword = (string)reader["TempPassword"];		
			if (!reader.IsDBNull(reader.GetOrdinal("CreatedTimeDTS"))) item.CreatedTimeDTS = (DateTime)reader["CreatedTimeDTS"];		
			return item;
		}
		
		
		///Description for GetJumpSite
		///Author:
		///Date: 
		public  JumpSite GetJumpSite(long redId )
		{
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_GetJumpSite");
			SecurityDatabase.AddInParameter(command,"@RedId", DbType.Int64, redId);
			JumpSite jumpSite =  null;
			using (IDataReader reader = base.ExecuteReader(command))
			{
				if (reader.Read())
				{
					jumpSite = BindJumpSite(reader);
					reader.Close();
				}
			}
			return jumpSite;
			
		}

        public bool CheckJumpSite(JumpSiteModel jumsiteModel)
        {
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_CheckJumpSite");
            SecurityDatabase.AddInParameter(command, "@UserId", DbType.Guid, jumsiteModel.UserId);
            SecurityDatabase.AddInParameter(command, "@RemoteIP", DbType.AnsiString, jumsiteModel.RemoteIp);
            SecurityDatabase.AddInParameter(command, "@DesSysId", DbType.Int32, jumsiteModel.DesSysId);
            SecurityDatabase.AddInParameter(command, "@TempPassword", DbType.AnsiString, jumsiteModel.TempPassword);
            SecurityDatabase.AddInParameter(command, "@ASPLog_ClientIPAddr", DbType.AnsiString, jumsiteModel.RemoteIp);
            SecurityDatabase.AddInParameter(command, "@ASPLog_HostIPAddr", DbType.AnsiString, jumsiteModel.HostIp);
            SecurityDatabase.AddInParameter(command, "@ASPLog_BrowserType", DbType.AnsiString, jumsiteModel.Browser);
            SecurityDatabase.AddInParameter(command, "@Jumper", DbType.Guid, jumsiteModel.Jumper);
            SecurityDatabase.AddInParameter(command, "@ASClient", DbType.Int32, jumsiteModel.ClientId);

            return (int)base.ExecuteScalar(command) == 1;

        }
		
		
		///Description for InsertJumpSite
		///Author:
		///Date: 
		public  void InsertJumpSite(Guid userId, string remoteIP, int desSysId, string tempPassword, out long redId,Guid jumper)
		{
			
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_InsJumpSite");
			SecurityDatabase.AddInParameter(command,"@UserId", DbType.Guid, userId);
			SecurityDatabase.AddInParameter(command,"@RemoteIP", DbType.AnsiString, remoteIP);
			SecurityDatabase.AddInParameter(command,"@DesSysId", DbType.Int32, desSysId);
			SecurityDatabase.AddInParameter(command,"@TempPassword", DbType.AnsiString, tempPassword);
			SecurityDatabase.AddOutParameter(command,"@RedId", DbType.Int64, 0);
            SecurityDatabase.AddInParameter(command, "@Jumper", DbType.Guid, jumper);
			base.ExecuteNonQuery(command);
			redId = (long)SecurityDatabase.GetParameterValue(command,"@RedId");
			
			
		}
		
		///Description for UpdateJumpSite
		///Author:
		///Date: 
		public  void UpdateJumpSite(long redId, Guid userId, string remoteIP, int desSysId, string tempPassword, DateTime createdTimeDTS )
		{
			
			
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_UpdJumpSite");
			SecurityDatabase.AddInParameter(command,"@RedId", DbType.Int64, redId);
			SecurityDatabase.AddInParameter(command,"@UserId", DbType.Guid, userId);
			SecurityDatabase.AddInParameter(command,"@RemoteIP", DbType.AnsiString, remoteIP);
			SecurityDatabase.AddInParameter(command,"@DesSysId", DbType.Int32, desSysId);
			SecurityDatabase.AddInParameter(command,"@TempPassword", DbType.AnsiString, tempPassword);
			SecurityDatabase.AddInParameter(command,"@CreatedTimeDTS", DbType.DateTime, createdTimeDTS);
			base.ExecuteNonQuery(command);
			
			
		}
		
		///Description for DeleteJumpSite
		///Author:
		///Date: 
		public  void DeleteJumpSite(long redId )
		{
			
			
            DbCommand command = SecurityDatabase.GetStoredProcCommand("spa_SEC_DelJumpSite");
			SecurityDatabase.AddInParameter(command,"@RedId", DbType.Int64, redId);
			base.ExecuteNonQuery(command);
			
			
		}
	}
}