using System;

namespace AS.Web.Business.General
{
	public static partial class GeneralFuncsLib
	{
		public static string DecryptText(string encryptedText, Guid? requestId)
		{
			if (string.IsNullOrEmpty(encryptedText))
				return null;
			try
			{
				var decryptedText = Common.DataProtection.Cryptophy.DecryptText(encryptedText);
				return decryptedText;
			}
			catch (Exception ex)
			{
				Common.Logger.LoggerManager.Error(string.Format("Cannot decrypt text: EncryptedText={0} - RequestId={1}. \n", encryptedText, requestId), ex);
				return null;
			}
		}
	}
}
