using AS.Common.DataProtection;
using AS.Common.WebUI;

public class CryptorServices : ICryptor
{
    #region ICryptor Members

    public string DecryptText(string str, string key)
    {
        return Cryptophy.DecryptText(str, key);
    }

    public string DecryptText(string str)
    {
        return Cryptophy.DecryptText(str);
    }

    public string EncryptText(string str, string key)
    {
        return Cryptophy.EncryptText(str, key);
    }

    public string EncryptText(string str)
    {
        return Cryptophy.EncryptText(str);
    }

    #endregion
    static CryptorServices _current = new CryptorServices();
    private CryptorServices() { }
    public static CryptorServices Current
    {
        get
        {
            return _current;
        }
    }
}