using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DataProtection;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void uxDecrypt_Click(object sender, EventArgs e)
    {
        //DataProtection dp = new DataProtection();
        uxResult.Text = Cryptophy.DecryptText(uxEncryptedString.Text.Trim());
    }
    protected void uxEncrypt_Click(object sender, EventArgs e)
    {
        //DataProtection dp = new DataProtection();
        uxResult.Text = Cryptophy.EncryptText(uxClearText.Text.Trim());
    }
}
