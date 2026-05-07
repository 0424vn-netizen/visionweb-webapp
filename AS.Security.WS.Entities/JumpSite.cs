using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
	//JumpSite Entity
	[Serializable]
	public class JumpSite
	{
		#region Constructors
		public JumpSite() {}
		
		public JumpSite (
			long redId,
			Guid userId,
			string remoteIP,
			int desSysId,
			string tempSecurityAuth,
			DateTime createdTimeDTS)
		
		{
			this.RedId = redId;
			this.UserId = userId;
			this.RemoteIP = remoteIP;
			this.DesSysId = desSysId;
			this.TempPassword = tempSecurityAuth;
			this.CreatedTimeDTS = createdTimeDTS;
		}
		#endregion
		
		#region Properties	
		/// <summary>
		/// 	
		/// </summary>
		/// <value>This type is bigint</value>
		public long RedId { get; set; }
		/// <summary>
		/// 	
		/// </summary>
		/// <value>This type is uniqueidentifier</value>
		public Guid UserId { get; set; }
		/// <summary>
		/// 	
		/// </summary>
		/// <value>This type is varchar</value>
		public string RemoteIP { get; set; }
		/// <summary>
		/// 	
		/// </summary>
		/// <value>This type is int</value>
		public int DesSysId { get; set; }
		/// <summary>
		/// 	
		/// </summary>
		/// <value>This type is varchar</value>
		public string TempPassword { get; set; }
		/// <summary>
		/// 	
		/// </summary>
		/// <value>This type is datetime</value>
		public DateTime CreatedTimeDTS { get; set; }
		#endregion
	}//End Class
	
	public enum JumpSiteColumns
	{
		RedId,
		UserId,
		RemoteIP,
		DesSysId,
		TempPassword,
		CreatedTimeDTS
	}//End enum
	
	//JumpSite Entity Collections
	[Serializable]
    public class JumpSiteCollection : CollectionBase 
	{

        public JumpSite this[ int index ]
		{
            get  { return( (JumpSite) List[index] ); }
            set  { List[index] = value;  }
        }

        public int Add( JumpSite value ) 
		{
            return( List.Add( value ) );
        }

        public int IndexOf( JumpSite value )
		{
            return( List.IndexOf( value ) );
        }

        public void Insert( int index, JumpSite value ) 
		{
            List.Insert( index, value );
        }

        public void Remove( JumpSite value ) 
		{
            List.Remove( value );
        }

        public bool Contains( JumpSite value ) 
		{
            // If value is not of type JumpSite, this will return false.
            return( List.Contains( value ) );
        }
		
  	}
}
