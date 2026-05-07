using System.ComponentModel;

/// <summary>
/// The event category list
/// </summary>
public enum EventCategory
{
    [Description("Unknown case")] 
    Unknown = 0,

    [Description("Statement")]
    Statement = 1,
}