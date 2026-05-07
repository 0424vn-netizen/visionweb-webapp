using System;

/// <summary>
/// Summary description for FileUploadModel
/// </summary>
[Serializable]
public class FileUploadModel
{
    public string FileName { get; set; }
    public string Note { get; set; }
    public long ServerDocumentID { get; set; }
    public string UploadedTime { get; set; }
    public long FileSize { get; set; }
    public string Extension { get; set; }
}