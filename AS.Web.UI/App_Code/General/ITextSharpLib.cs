using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
/// <summary>
/// Summary description for ITextSharpFunc
/// </summary>
public class ITextSharpLib
{
    public ITextSharpLib()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static Cell GetTDCellCenterNormal(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_MIDDLE;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static void AddTDCellCenterNormal(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellCenterNormal(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCellLeftNormal(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellLeftItalic(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.ITALIC);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static void AddTDCellLeftItalic(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellLeftItalic(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellLeftNormal(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellLeftNormal(data,TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCellLeftNormalWithColSpan(object data, BaseFont TimesFont, int spannumber)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.Colspan = spannumber;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellLeftBoldItalicWithColSpan(object data, BaseFont TimesFont, int spannumber)
    {
        Font CurrentFont = new Font(TimesFont, 10f, Font.BOLDITALIC);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.Colspan = spannumber;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellRightNormalWithColSpan(object data, BaseFont TimesFont, int spannumber)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.Colspan = spannumber;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static void AddTDCellLeftNormalWithColSpan(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont,int spannumber)
    {
        Cell CurrentCell = GetTDCellLeftNormalWithColSpan(data, TimesFont,spannumber);
        currentTable.AddCell(CurrentCell);
    }

    public static void AddTDCellLeftBoldItalicWithColSpan(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int spannumber)
    {
        Cell CurrentCell = GetTDCellLeftBoldItalicWithColSpan(data, TimesFont, spannumber);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellRightNormalWithColSpan(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int spannumber)
    {
        Cell CurrentCell = GetTDCellRightNormalWithColSpan(data, TimesFont, spannumber);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCellLeftStrong(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.BOLD);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellLeftStrong(object data, BaseFont TimesFont,Color BGcolor)
    {
        Font CurrentFont = new Font(TimesFont, 10f, Font.BOLD);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        CurrentCell.BackgroundColor = BGcolor;
        return CurrentCell;
    }
    public static Cell GetTDCellLeftStrongBorderBottom(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.BorderColorBottom = Color.BLACK;
        CurrentCell.BorderWidthBottom = 1;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static void AddTDCellLeftStrong(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellLeftStrong(data,TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellLeftStrong(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont,Color BGcolor)
    {
        Cell CurrentCell = GetTDCellLeftStrong(data, TimesFont,BGcolor);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellLeftStrongBorderBottom(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellLeftStrongBorderBottom(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCellRightNormal(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont,9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }

    public static void AddTDCell(iTextSharp.text.Table currentTable,object data, BaseFont TimesFont, int FontStyle, int align, int valign)
    {
        Cell CurrentCell = GetTDCell( data,  TimesFont,  FontStyle,  align,  valign);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCell(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int FontStyle, int align, int valign,Color BGcolor)
    {
        Cell CurrentCell = GetTDCell(data, TimesFont, FontStyle, align, valign,BGcolor);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCell(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int FontStyle,System.Single FontSize, int align, int valign)
    {
        Cell CurrentCell = GetTDCell(data, TimesFont, FontStyle, FontSize, align, valign);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCell(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int FontStyle, int align, int valign, int colspan)
    {
        Cell CurrentCell = GetTDCell(data, TimesFont, FontStyle, align, valign,colspan);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellWhiteColor(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int FontStyle, int align, int valign, int colspan,Color BGcolor)
    {
        Cell CurrentCell = GetTDCellWhiteColor(data, TimesFont, FontStyle, align, valign, colspan,BGcolor);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCell(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont, int FontStyle, int align, int valign, int colspan, Color BGcolor)
    {
        Cell CurrentCell = GetTDCell(data, TimesFont, FontStyle, align, valign, colspan, BGcolor);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCell(object data, BaseFont TimesFont, int FontStyle, int align, int valign)
    {
        Font CurrentFont = new Font(TimesFont, 9f,FontStyle);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = align;
        CurrentCell.VerticalAlignment = valign;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCell(object data, BaseFont TimesFont, int FontStyle, int align, int valign, Color BGcolor)
    {
        Font CurrentFont = new Font(TimesFont, 9f, FontStyle);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;

        
        CurrentCell.HorizontalAlignment = align;
        CurrentCell.VerticalAlignment = valign;
        CurrentCell.Add(CurrentChunk);
        CurrentCell.BackgroundColor = BGcolor;
        CurrentCell.SetVerticalAlignment("middle");
        //CurrentCell.Border = 1;
        //CurrentCell.BorderColor = Color.BLUE;
        //CurrentCell.BorderColorBottom = Color.BLUE;
        //CurrentCell.BorderColorLeft = Color.BLUE;
        //CurrentCell.BorderColorRight = Color.BLUE;
        return CurrentCell;
    }

    public static Cell GetTDCell(object data, BaseFont TimesFont, int FontStyle,System.Single fontsize, int align, int valign)
    {
        Font CurrentFont = new Font(TimesFont, fontsize, FontStyle);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.HorizontalAlignment = align;
        CurrentCell.VerticalAlignment = valign;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCell(object data, BaseFont TimesFont, int FontStyle, int align, int valign,int colspan)
    {
        Font CurrentFont = new Font(TimesFont, 9f, FontStyle);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = align;
        CurrentCell.VerticalAlignment = valign;
        CurrentCell.Colspan = colspan;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCell(object data, BaseFont TimesFont, int FontStyle, int align, int valign, int colspan,Color BGcolor)
    {
        Font CurrentFont = new Font(TimesFont,9f, FontStyle);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = align;
        CurrentCell.VerticalAlignment = valign;
        CurrentCell.Colspan = colspan;
        CurrentCell.Add(CurrentChunk);
        CurrentCell.BackgroundColor = BGcolor;
        return CurrentCell;
    }
    public static Cell GetTDCellWhiteColor(object data, BaseFont TimesFont, int FontStyle, int align, int valign, int colspan, Color BGcolor)
    {
        Font CurrentFont = new Font(TimesFont, 10f, FontStyle);
        CurrentFont.Color = Color.WHITE;
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = align;
        CurrentCell.VerticalAlignment = valign;
        CurrentCell.Colspan = colspan;
        CurrentCell.Add(CurrentChunk);
        CurrentCell.BackgroundColor = BGcolor;
        return CurrentCell;
    }
    public static Cell GetTDCellRightNormalTopSolid(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.BorderColorTop = Color.BLACK;
        CurrentCell.BorderWidthTop = 1;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellLeftItalicTopSolid(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.ITALIC);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.BorderColorTop = Color.BLACK;
        CurrentCell.BorderWidthTop = 1;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellLeftItalicLeftSolid(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.ITALIC);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.BorderColorLeft = Color.BLACK;
        CurrentCell.BorderWidthLeft = 1;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static void AddTDCellRightNormalTopSolid(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellRightNormalTopSolid(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellLeftItalicLeftSolid(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        currentTable.AddCell(GetTDCellLeftItalicLeftSolid(data,TimesFont));
    }
    public static void AddTDCellLeftItalicTopSolid(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellLeftItalicTopSolid(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellRightNormal(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellRightNormal(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
   
    public static Cell GetTDCellRightStrong(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.BOLD);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellRightStrong(object data, BaseFont TimesFont,Color BGcolor)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.BOLD);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);
        CurrentCell.BackgroundColor = BGcolor;
        return CurrentCell;
    }
    public static void AddTDCellRightStrong(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellRightStrong(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellRightStrong(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont,Color BGcolor)
    {
        Cell CurrentCell = GetTDCellRightStrong(data, TimesFont,BGcolor);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCellRightStrongBorderBottom(object data, BaseFont TimesFont)
    {
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        string url = string.Format("{0}", data);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.BorderWidthBottom = 1;
        CurrentCell.BorderColorBottom = Color.BLACK;
        CurrentChunk.SetTextRise(5f);
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static void AddTDCellRightStrongBorderBottom(iTextSharp.text.Table currentTable, object data, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellRightStrongBorderBottom(data, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static Cell GetTDCellRightWithCombine(object data1, object data2, bool IsBoldFirst, BaseFont TimesFont)
    {
        string url = string.Format("{0}", data1);
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        Font StrongFont = new Font(TimesFont, 9f, Font.BOLD);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        if (IsBoldFirst) CurrentChunk = new Chunk(url, StrongFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.VerticalAlignment = PdfCell.ALIGN_MIDDLE;
        CurrentCell.Add(CurrentChunk);

        url = string.Format("{0}", data2);
        CurrentChunk = new Chunk(url, CurrentFont);
        if (!IsBoldFirst) CurrentChunk = new Chunk(url, StrongFont);
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellLeftWithCombine(object data1, object data2, bool IsBoldFirst, BaseFont TimesFont)
    {
        string url = string.Format("{0}", data1);
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        Font StrongFont = new Font(TimesFont, 9f, Font.BOLD);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        if (IsBoldFirst) CurrentChunk = new Chunk(url, StrongFont);
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
        CurrentCell.VerticalAlignment = PdfCell.ALIGN_MIDDLE;
        CurrentCell.Add(CurrentChunk);

        url = string.Format("{0}", data2);
        CurrentChunk = new Chunk(url, CurrentFont);
        if (!IsBoldFirst) CurrentChunk = new Chunk(url, StrongFont);
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellRightWithCombine(object data1, object data2, bool IsFirst, BaseFont TimesFont,bool isUnderLine,bool isBold)
    {
        string url = string.Format("{0}", data1);
        Font CurrentFont = new Font(TimesFont,9f, Font.NORMAL);
        Font StrongFont = new Font(TimesFont, 9f, Font.BOLD);
        Font UnderLineFont = new Font(TimesFont, 9f, Font.UNDERLINE);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        if (IsFirst)
        {
           if(isBold) CurrentChunk = new Chunk(url, StrongFont);
           else if (isUnderLine) CurrentChunk = new Chunk(url, UnderLineFont);
        }
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);

        url = string.Format("{0}", data2);
        CurrentChunk = new Chunk(url, CurrentFont);
        if (!IsFirst)
        {
            if (isBold) CurrentChunk = new Chunk(url, StrongFont);
            else if (isUnderLine) CurrentChunk = new Chunk(url, UnderLineFont);
        }
        CurrentCell.Add(CurrentChunk);
        return CurrentCell;
    }
    public static Cell GetTDCellRightWithCombine(object data1, object data2, bool IsFirst, BaseFont TimesFont, bool isUnderLine, bool isBold, Color BGcolor)
    {
        string url = string.Format("{0}", data1);
        Font CurrentFont = new Font(TimesFont, 9f, Font.NORMAL);
        Font StrongFont = new Font(TimesFont, 9f, Font.BOLD);
        Font UnderLineFont = new Font(TimesFont, 9f, Font.UNDERLINE);
        Chunk CurrentChunk = new Chunk(url, CurrentFont);
        if (IsFirst)
        {
            if (isBold) CurrentChunk = new Chunk(url, StrongFont);
            else if (isUnderLine) CurrentChunk = new Chunk(url, UnderLineFont);
        }
        Cell CurrentCell = new Cell();
        CurrentCell.Border = 0;
        CurrentCell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
        CurrentCell.Add(CurrentChunk);

        url = string.Format("{0}", data2);
        CurrentChunk = new Chunk(url, CurrentFont);
        if (!IsFirst)
        {
            if (isBold) CurrentChunk = new Chunk(url, StrongFont);
            else if (isUnderLine) CurrentChunk = new Chunk(url, UnderLineFont);
        }
        CurrentCell.Add(CurrentChunk);
        CurrentCell.BackgroundColor = BGcolor;
        return CurrentCell;
    }
    public static void AddTDCellRightWithCombine(iTextSharp.text.Table currentTable, object data1, object data2, bool IsFirst, BaseFont TimesFont, bool isUnderLine, bool isBold)
    {
        Cell CurrentCell = GetTDCellRightWithCombine( data1,  data2,  IsFirst,  TimesFont, isUnderLine, isBold);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellLeftWithCombine(iTextSharp.text.Table currentTable, object data1, object data2, bool IsBoldFirst, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellLeftWithCombine(data1, data2, IsBoldFirst, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellRightWithCombine(iTextSharp.text.Table currentTable, object data1, object data2, bool IsFirst, BaseFont TimesFont, bool isUnderLine, bool isBold,Color BGcolor)
    {
        Cell CurrentCell = GetTDCellRightWithCombine(data1, data2, IsFirst, TimesFont, isUnderLine, isBold,BGcolor);
        currentTable.AddCell(CurrentCell);
    }
    public static void AddTDCellRightWithCombine(iTextSharp.text.Table currentTable, object data1, object data2, bool IsBoldFirst, BaseFont TimesFont)
    {
        Cell CurrentCell = GetTDCellRightWithCombine(data1, data2, IsBoldFirst, TimesFont);
        currentTable.AddCell(CurrentCell);
    }
    public static iTextSharp.text.Table SetDefaultTable(int colum)
    {
        iTextSharp.text.Table CurrentTable = new iTextSharp.text.Table(colum);
        int[] widths = new int[colum];
        for (int i = 0; i < colum; i++)
            widths[i] = 2;
        CurrentTable.SetWidths(widths);
        CurrentTable.Width = 100;
        CurrentTable.Border = 0;
        CurrentTable.DefaultCellBackgroundColor = Color.WHITE;
        CurrentTable.DefaultCellBorderWidth = 0.657f;
        CurrentTable.DefaultCell.VerticalAlignment = PdfCell.ALIGN_UNDEFINED;
        CurrentTable.DefaultCell.UseBorderPadding = true;
        CurrentTable.Padding = 1.2f;
        CurrentTable.CellsFitPage = true;
        CurrentTable.AutoFillEmptyCells = true;
        CurrentTable.BorderColor = Color.WHITE;
        return CurrentTable;
    }
    public static Table BuildTableHeaders(string[] columnHeaders, int exportColumnNumber, BaseFont TimesFont)
    {
        iTextSharp.text.Table currentTable = SetDefaultTable(exportColumnNumber);
        foreach (string column in columnHeaders)
        {
            AddTDCellCenterNormal(currentTable, column, TimesFont);
        }
        return currentTable;
    }
}

