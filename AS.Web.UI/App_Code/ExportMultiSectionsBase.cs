using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using AS.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for ExportMultiSections
/// </summary>
/// 

public abstract class ExportMultiSections : GlobalUserControl
{
    #region Constant
    private const string MULTI_EXPORT_TEMPLATE = "~/App_Data/tpl_ExportMultiSection.htm";
    private const string SHEET_CONTENT_TEMPLATE = "~/App_Data/tpl_ExportMultiSectionSheetContent.htm";
    public const string SHEET_TITLE_TEMPLATE = "<x:ExcelWorksheet><x:Name>{0}</x:Name><x:WorksheetSource HRef=3D\"Book2_files/sheet{1}.htm\"/></x:ExcelWorksheet>";
    #endregion

    #region Properties
    #region Export Multi Section

    //Collection of section export name
    public Dictionary<int, string> ExportSectionNames { get; set; }

    #endregion

    //Collection of filenames
    public List<string> SelectedFileNames;

    private Button _btnExportMultiSections;
    private HiddenField _hiddenField;

    public string FileName
    {
        get
        {
            if (!SessionManager.CurrentReportFilter.Value.IsNullOrEmpty())
            {
                return string.Format("{0}_{1}", SessionManager.CurrentReportFilter.Value, DateTime.Now.ToString("yyyyMMdd"));
            }
            return DateTime.Now.ToString("yyyyMMdd");
        }
    }
    #endregion

    public ExportMultiSections()
    {
        SelectedFileNames = new List<string>();
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //GetListFunctions();
        _btnExportMultiSections = Page.Master.FindControl("ContentPage").FindControl("btnExportMultiSections") as Button;
        _hiddenField = Page.Master.FindControl("ContentPage").FindControl("hdfExportMultiSections") as HiddenField;

        if (_btnExportMultiSections != null)
        {
            _btnExportMultiSections.CssClass = "hidden";
            _btnExportMultiSections.Click += (s, a) =>
            {
                DoExport();
            };
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        writer.BeginRender();

        string htmlPanelStart = "<div id='pnlMultiSections' class='hidden'>";
        string htmlPanelEnd = "</div>";

        string templateCheckbox = @"
                                    <div><label class='default-font-family font-size-default'>
                                        <input type='checkbox' onclick='doSetSelectedExportOptions(this.value, this.checked);' value='{0}' />
                                        {1}
                                    </label></div>
                                   ";
        if (!ExportSectionNames.IsNullData())
        {
            // register all checkbox
            foreach (var i in ExportSectionNames)
            {
                htmlPanelStart += string.Format(templateCheckbox, i.Key, i.Value);
            }

            htmlPanelStart += htmlPanelEnd;

            //Register javascript
            string script = @"<script type='text/javascript'>
                                       var hdfExportMultiSectionsId = '" + _hiddenField.ClientID + @"';
                                       var btnExportMultiSections = '" + _btnExportMultiSections.ClientID + @"';

                                        function getPanelListSections()
                                        {
                                            return $('#pnlMultiSections').html();
                                        }

                                        function doExportMultisections(lst)
                                        {
                                            var result= lst.join();
                                            $('#'+hdfExportMultiSectionsId).val(result);

                                            $('#'+btnExportMultiSections).click();
                                        }
                         </script>";

            htmlPanelStart += script;

            //render html
            HtmlString str = new HtmlString(htmlPanelStart);
            writer.Write(str);
        }
        else
        {
            AS.Common.Logger.LoggerManager.Error(string.Format("ExportSectionNames is null. Client={0}; UserId={1}; FilterValue={2}"
                , SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, SessionManager.CurrentReportFilter.Value));
        }

        writer.EndRender();

        base.Render(writer);
    }

    protected virtual Dictionary<int, Func<string>> GetListFunctions() { return new Dictionary<int, Func<string>>(); }

    private void DoExport()
    {
        //write files
        if (this.Visible)
        {
            string[] selectedFuncs = _hiddenField.Value.Split(',');

            var exportFunctions = GetListFunctions();
            //var a =  GetListFunctions();
            if (selectedFuncs != null && selectedFuncs.Count() > 0 && exportFunctions != null && exportFunctions.Count > 0)
            {
                foreach (var i in selectedFuncs)
                {
                    Func<string> function = exportFunctions[int.Parse(i)];
                    string fileName = function();
                    SelectedFileNames.Add(fileName);
                }
                MergeSelectedFile();
            }
        }
    }

    //Base on all files is selected to export, Read all this files and merge to one file with multi sheet
    //A file is corresponding with a sheet.
    private void MergeSelectedFile()
    {
        string fileContent = File.ReadAllText(Server.MapPath(MULTI_EXPORT_TEMPLATE));
        string sheetContentTemplate = File.ReadAllText(Server.MapPath(SHEET_CONTENT_TEMPLATE));
        int sheetNum = 1;
        string sheetTilte = string.Empty;
        string sheetContent = string.Empty;
        foreach (var fileName in SelectedFileNames)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string sheetName = Path.GetFileNameWithoutExtension(fileName).Split('-')[0].Replace(" ", string.Empty);
                sheetTilte += string.Format(SHEET_TITLE_TEMPLATE, sheetName, sheetNum);
                sheetContent += string.Format(sheetContentTemplate, sheetNum).Replace("[SHEET_CONTENT]", File.ReadAllText(fileName).ToString());
                sheetNum++;
            }
            else
            {
                AS.Common.Logger.LoggerManager.Error(string.Format("Function export return filename that is null! Client={0}; UserId={1}; FilterValue={2}",
                    SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, SessionManager.CurrentReportFilter.Value));
            }
        }
        fileContent = fileContent.Replace("[SHEET_TITLE]", sheetTilte).Replace("[FILE_CONTENT]", sheetContent);
        fileContent = ReformatExcelContent(fileContent);
        DeleteFile();
        DownloadFile(FileName, "xls", fileContent);
    }

    private string ReformatExcelContent(string content)
    {
        content = content.Replace("<head>", "<head><meta charset=\"UTF-8\">");
        content = Regex.Replace(content, @"<\/?([a-z][a-z0-9]*)\b[^>]*>", m => m.ToString().Replace("='", "=3D'").Replace("=\"", "=3D'").Replace("\"", "'"), RegexOptions.IgnoreCase);
        content = content.Replace("’", "'");
        return content;
    }

    //Delete all files when the export done
    private void DeleteFile()
    {
        foreach (var file in SelectedFileNames)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }

    private void DownloadFile(string fileName, string fileType, string content)
    {
        Response.BufferOutput = true;
        switch (fileType)
        {
            case "xls":
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + VeraCodeSolution.RemoveCRLF(fileName) + ".xls\"");
                break;
        }
        Response.Write(content);
        Response.Flush();
        Response.End();
    }

    public static string WriteContentToFile(string path, string fileName, string fileType, string fileContent)
    {
        string pathTemp = string.Format("{0}{1}-{2}.{3}", path, fileName, DateTime.Now.Ticks.ToString(), "xls");
        var filePath = HttpContext.Current.Server.MapPath(pathTemp);
        FileStream fs = new FileStream(filePath, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(fileContent);
        sw.Flush();
        sw.Close();
        fs.Close();
        return filePath;
    }
}