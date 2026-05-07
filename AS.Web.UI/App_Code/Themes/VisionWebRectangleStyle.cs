using System.Drawing;

public class VisionWebRectangleStyle
{
    public VisionWebRectangleStyle()
    {
        this.HasBottomLeft = this.HasBottomRight = this.HasTopLeft = this.HasTopRight = true;
        this.HasBorder = false;
        this.Alpha = 255;
    }
    public int Width;
    public int Height;
    public bool HasBorder;
    public int Alpha;
    public Color MainColor;
    public Color SubColor;
    public Color BorderColor;
    public int Radius;
    public float BorderWidth;

    /// <summary>
    /// True: Draw top left rounded corner
    /// </summary>
    public bool HasTopLeft;
    /// <summary>
    /// True: Draw top right rounded corner
    /// </summary>
    public bool HasTopRight;
    /// <summary>
    /// /// <summary>
    /// True: Draw bottom left rounded corner
    /// </summary>
    /// </summary>
    public bool HasBottomLeft;
    /// <summary>
    /// True: Draw bottom right rounded corner
    /// </summary>
    public bool HasBottomRight;
}
