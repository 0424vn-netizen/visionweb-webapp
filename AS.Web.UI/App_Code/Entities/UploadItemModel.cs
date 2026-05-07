using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

/// <summary>
/// Summary description for UploadItemModel
/// </summary>
[Serializable]
public class UploadItemModel
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string Note { get; set; }

    public long ContentLength { get; set; }
    public string Extension { get; set; }

    public byte[] DataBytes { get; set; }
    public string IdFileClient { get; set; }
}