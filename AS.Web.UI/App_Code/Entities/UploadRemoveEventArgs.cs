using System;

/// <summary>
/// Summary description for UploadRemoveEventArgs
/// </summary>
public class UploadRemoveEventArgs : EventArgs
{
    public int IdControl { get; set; }
}

public class UploadEventArgs : EventArgs
{
    public bool IsUpload { get; set; }
    public bool IsDelete { get; set; }
}