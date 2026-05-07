using System;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace AS.Security.WS.Entities
{
    //User Entity
    [Serializable]
    public class User
    {
        #region Constructors
        public User() { }
        #endregion

        #region Properties
        public Guid RecId { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int SiteID { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int ASClient { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserID { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string OriginalUserID { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public bool IsChangeUserID { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserNameFirst { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserNameLast { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserNameFull { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserPassword { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int UserPasswordType { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime UserPasswordDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserPasswordTmp { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int UserPasswordTmpType { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime UserPasswordTmpDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginAttempts { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime LastLoginDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime PrevLoginDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string TandCStat { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime TandCStatDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserSecRole { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string SuperUser { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string PCISuperUser { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ASCSUser { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserAccsType { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string UserAccsLevel { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ActvStat { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime CreatedDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime UpdatedDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// For receiving SMS Notification
        /// </summary>
        public string PhoneForSMS { get; set; } = string.Empty;
        /// <summary>
        /// For receiving Email Notification
        /// </summary>
        public string ContactEmail { get; set; } = string.Empty;
        /// 	
        /// </summary>
        /// <value>This type is tinyint</value>
        public byte Fto_status { get; set; } = 0;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime Fto_CreateDate { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginQuestionIndex { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string LoginQuestionAnswer { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM01 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM02 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM03 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM04 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM05 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM06 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM07 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM08 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM09 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM10 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM11 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsM12 { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsYTD { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int LoginsPriorYear { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string ClientIPAddr { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string HostIPAddr { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is varchar</value>
        public string BrowserType { get; set; } = string.Empty;
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is datetime</value>
        public DateTime ActvStatDTS { get; set; } = new DateTime(1900, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public int Theme_id { get; set; }
        /// <summary>
        /// 	
        /// </summary>
        /// <value>This type is int</value>
        public string HierarchyName { get; set; } = string.Empty;

        #endregion
        public string Status {
            get { return ActvStat; }
            set { this.ActvStat = value; }
        }
        public char StatusChar
        {
            get { return ActvStat == "1" ? 'Y' : 'N'; }
        }

        public char OptInOut { get; set; } = 'N';

        public char RstMerPw { get; set; } = 'N';

        public string EntityID { get; set; } = "";

        public int EntityType { get; set; } = -1;

        public string InitialID { get; set; } = "";

        public int UserType { get; set; }

        public Guid CreatedBy { get; set; }

        public int ReportType { get; set; }

        public Guid UpdatedBy { get; set; }

        /// <summary>
        /// TK 36296 - Climate Control
        /// </summary>
        public string SalesRepCode { get; set; } = string.Empty;

        public string Organizations { get; set; } = string.Empty;
    }


    //User Entity Collections
    [Serializable]
    public class UserCollection : CollectionBase
    {

        public User this[int index]
        {
            get { return ((User)List[index]); }
            set { List[index] = value; }
        }

        public int Add(User value)
        {
            return (List.Add(value));
        }

        public int IndexOf(User value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, User value)
        {
            List.Insert(index, value);
        }

        public void Remove(User value)
        {
            List.Remove(value);
        }

        public bool Contains(User value)
        {
            // If value is not of type User, this will return false.
            return (List.Contains(value));
        }

    }
}
