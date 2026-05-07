using AS.Core.Common.Log;
using AS.VW.Scheduler.Cybersource.Auth.Client;
using log4net;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AS.VW.Scheduler.Cybersource.Auth.Business
{
    public class KeyProtection
    {
        private readonly byte[] _Key = Encoding.ASCII.GetBytes("NGERNINHXEOUJMSUHGJIIXOOHYTTXBMP");
        private readonly byte[] _IV = Encoding.ASCII.GetBytes("afpgvmwledfptycr");
        private readonly ILog LogManager = Logger.GetLogger(typeof(CybersourceClient));

        public string DecryptText(string decryptStr)
        {
            if (decryptStr == null || decryptStr == string.Empty)
            {
                return string.Empty;
            }

            RijndaelManaged rijndaelManaged = null;
            string result;
            try
            {
                rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.Key = this._Key;
                rijndaelManaged.IV = this._IV;

                byte[] encryptedBytes = Convert.FromBase64String(decryptStr);
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                byte[] decryptedBytes = cryptoTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                decryptStr = Encoding.ASCII.GetString(decryptedBytes);
                result = decryptStr;
            }
            catch (Exception ex)
            {
                LogManager.Error($"KeyProtection.DecryptText error: {ex.Message}");
                result = string.Empty;
            }
            finally
            {
                if (rijndaelManaged != null)
                {
                    rijndaelManaged.Clear();
                }
            }
            return result;
        }

        public string EncryptText(string plainText)
        {
            if (plainText == null || plainText == string.Empty)
            {
                return string.Empty;
            }

            RijndaelManaged rijndaelManaged = null;
            string result;
            try
            {
                rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.Key = this._Key;
                rijndaelManaged.IV = this._IV;

                byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                byte[] encryptedBytes = cryptoTransform.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                result = Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                LogManager.Error($"KeyProtection.EncryptText error: {ex.Message}");
                result = string.Empty;
            }
            finally
            {
                if (rijndaelManaged != null)
                {
                    rijndaelManaged.Clear();
                }
            }
            return result;
        }
    }
}
