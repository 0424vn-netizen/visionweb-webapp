using AS.Common.Logger;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using WebSupergoo.ABCpdf10;

namespace AS.VW.Scheduler.G2ML
{
    public static class Program
    {        
        private static ILog Logger = LoggerManager.GetLogger(typeof(Program));
        private static string FILE_NAME_G2_MONEY_LAUNDERING = System.AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["FILE_NAME_G2_MONEY_LAUNDERING"];
        private static string ExportTempFolder = ConfigurationManager.AppSettings["ExportTempFolder"];
        private static string Client_Id_Default = ConfigurationManager.AppSettings["Client_Id_Default"];
        private static DateTime ReportDate = DateTime.Now;
        private static DBExecute dbe = new DBExecute();
        private static string SPAName_UpdateExtractReportLog = ConfigurationManager.AppSettings["SPAName_UpdateExtractReportLog"];
        private static string SPAName_GetDailyMatchingReportForEmail = ConfigurationManager.AppSettings["SPAName_GetDailyMatchingReportForEmail"];
        static void Main(string[] args)
        {            
            DateTime start = DateTime.Now;
            Console.WriteLine("\n\n----- Program Starting... ----- \n");
            Logger.Info("\n\n----- Main Application Starting... ----- " + start.ToString());
            InitArgs(args);

            if (!File.Exists(FILE_NAME_G2_MONEY_LAUNDERING))
            {
                Console.WriteLine("\n\n----- File template does not exist... ----- \n");
                Logger.Info("\n\n----- File template does not exist.. ----- tpl_G2MoneyLaundering.en-US.html");
                return;
            }
            ExportAndSaveFile();
            Logger.Info("\n\n----- Main Application End... ----- " + DateTime.Now.ToString());
            Console.WriteLine("\n--- Program Run Completed ---\n\n");
        }

        private static void InitArgs(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    ReportDate = Convert.ToDateTime(args[0].Trim());
                }
                else if (args.Length >= 2)
                {
                    ReportDate = Convert.ToDateTime(args[0].Trim());
                    ExportTempFolder = args[1].Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n----- Error: " + ex + " ----- \n");
                Logger.Info("\n\n----- Error: " + ex + " ----- \n");
                throw new ArgumentException("Exception when InitArgs", ex);
            }
        }

        public static void ExportAndSaveFile()
        {
            FileInfo fileInfo = new FileInfo(ExportTempFolder);
            Console.WriteLine("\n\n----- Writing Status On Database... ----- \n");
            Logger.Info("\n\n----- Writing Status On Database... ----- ");
            var extractReportLogModel = new ExtractReportLogModel()
            {
                SpaName = SPAName_UpdateExtractReportLog,
                ASClient = Client_Id_Default,
                ReportDate = ReportDate,
                FileName = fileInfo.Name,
                FileType = "PDF",
                FilePath = ExportTempFolder,
                Status = "1",
                NumberOfMatches = 0
            };
            dbe.UpdateExtractReportLog(extractReportLogModel);
            Console.WriteLine("\n\n----- End Writing Status On Database... ----- \n");
            Logger.Info("\n\n----- End Writing Status On Database... ----- ");
            StringBuilder strContent = new StringBuilder();

            var dtTable = dbe.GetDataG2(SPAName_GetDailyMatchingReportForEmail, 1, ReportDate);

            try
            {
                var lstG2ClientNo = dtTable.Select("IsG2Client = 0").AsEnumerable().GroupBy(r => new { AsClientID = r["AsClientID"] }).ToList();

                var lstG2ClientYes = dtTable.Select("IsG2Client = 1").AsEnumerable().GroupBy(r => new { AsClientID = r["AsClientID"] }).ToList();


                string strTemplate = File.ReadAllText(FILE_NAME_G2_MONEY_LAUNDERING);

                int beginIndexClientYN = strTemplate.IndexOf("G2MONEYLAUNDERING_BOOLCLIENT_BEGIN");
                int endIndexClientYN = strTemplate.IndexOf("G2MONEYLAUNDERING_BOOLCLIENT_END");

                strContent.Append(strTemplate.Substring(0, beginIndexClientYN));

                string strG2ClientYesNo = strTemplate.Substring(beginIndexClientYN, endIndexClientYN - beginIndexClientYN);


                int beginIndexClientName = strTemplate.IndexOf("G2MONEYLAUNDERING_CLIENTNAME_BEGIN");
                int endIndexClientName = strTemplate.IndexOf("G2MONEYLAUNDERING_CLIENTNAME_END");

                string strClientName = strTemplate.Substring(beginIndexClientName, endIndexClientName - beginIndexClientName);

                int beginIndexClientRow = strTemplate.IndexOf("G2MONEYLAUNDERING_ROW_BEGIN");
                int endIndexClientRow = strTemplate.IndexOf("G2MONEYLAUNDERING_ROW_END");

                string strClientRow = strTemplate.Substring(beginIndexClientRow, endIndexClientRow - beginIndexClientRow);

                int beginIndexClientTotal = strTemplate.IndexOf("G2MONEYLAUNDERING_SUBTOTAL_BEGIN");
                int endIndexClientTotal = strTemplate.IndexOf("G2MONEYLAUNDERING_SUBTOTAL_END");

                string strTotal = strTemplate.Substring(beginIndexClientTotal, endIndexClientTotal - beginIndexClientTotal);

                double totalYes = 0;
                int countYes = 0;
                if (lstG2ClientYes.Count > 0)
                {
                    strContent.Append(strG2ClientYesNo.Replace("Client_YesNo", "G2 Client = Yes"));

                    foreach (var item in lstG2ClientYes)
                    {
                        var lstItem = item.ToList();
                        strContent.Append(strClientName.Replace("Client_Name", lstItem[0]["ClientName"].ToString()));
                        double subtotal = 0;
                        for (int i = 0; i < lstItem.Count; i++)
                        {
                            strContent.Append(strClientRow.Replace("Client_Name", lstItem[i]["ClientName"].ToString())
                                        .Replace("Merchant_Number", lstItem[i]["MerchantNumber"].ToString())
                                        .Replace("Merchant_Name", lstItem[i]["MerchantName"].ToString())
                                        .Replace("Card_Number", lstItem[i]["CardNumber"].ToString())
                                        .Replace("G2_Expiration", lstItem[i]["G2_Expiration"].ToString())
                                        .Replace("G2_Auth_Date", DateFormat(lstItem[i]["G2_AuthorizationDate"].ToString()))
                                        .Replace("Aperia_Auth_Date", DateFormat(lstItem[i]["AuthorizationDate"].ToString()))
                                        .Replace("Aperia_Auth_Time", lstItem[i]["AuthorizationTime"].ToString())
                                        .Replace("Aperia_Auth_Amount", NumberFormat(lstItem[i]["AuthorizationAmount"].ToString()))
                                        .Replace("Aperia_Report_Date", DateFormat(lstItem[i]["ReportDate"].ToString())));
                            subtotal += Convert.ToDouble(lstItem[i]["AuthorizationAmount"].ToString());
                        }
                        totalYes += subtotal;
                        countYes += lstItem.Count;
                        strContent.Append(strTotal.Replace("SUB_TOTAL", "Sub-Total")
                                        .Replace("G2_Auth_Date", lstItem.Count.ToString())
                                        .Replace("Aperia_Auth_Amount", NumberFormat(subtotal.ToString())));
                    }
                    strContent.Append(strTotal.Replace("SUB_TOTAL", "Total rows: " + lstG2ClientYes.Count.ToString())
                                        .Replace("G2_Auth_Date", countYes.ToString())
                                        .Replace("Aperia_Auth_Amount", NumberFormat(totalYes.ToString())));
                }
                double totalNo = 0;
                int countNo = 0;
                if (lstG2ClientNo.Count > 0)
                {
                    strContent.Append(strG2ClientYesNo.Replace("Client_YesNo", "G2 Client = No"));

                    foreach (var item in lstG2ClientNo)
                    {
                        var lstItem = item.ToList();
                        strContent.Append(strClientName.Replace("Client_Name", lstItem[0]["ClientName"].ToString()));
                        double subtotal = 0;
                        for (int i = 0; i < lstItem.Count; i++)
                        {
                            strContent.Append(strClientRow.Replace("Client_Name", lstItem[i]["ClientName"].ToString())
                                        .Replace("Merchant_Number", lstItem[i]["MerchantNumber"].ToString())
                                        .Replace("Merchant_Name", lstItem[i]["MerchantName"].ToString())
                                        .Replace("Card_Number", lstItem[i]["CardNumber"].ToString())
                                        .Replace("G2_Expiration", lstItem[i]["G2_Expiration"].ToString())
                                        .Replace("G2_Auth_Date", DateFormat(lstItem[i]["G2_AuthorizationDate"].ToString()))
                                        .Replace("Aperia_Auth_Date", DateFormat(lstItem[i]["AuthorizationDate"].ToString()))
                                        .Replace("Aperia_Auth_Time", lstItem[i]["AuthorizationTime"].ToString())
                                        .Replace("Aperia_Auth_Amount", NumberFormat(lstItem[i]["AuthorizationAmount"].ToString()))
                                        .Replace("Aperia_Report_Date", DateFormat(lstItem[i]["ReportDate"].ToString())));
                            subtotal += Convert.ToDouble(lstItem[i]["AuthorizationAmount"].ToString());
                        }
                        totalNo += subtotal;
                        countNo += lstItem.Count;
                        strContent.Append(strTotal.Replace("SUB_TOTAL", "Sub-Total")
                                        .Replace("G2_Auth_Date", lstItem.Count.ToString())
                                        .Replace("Aperia_Auth_Amount", NumberFormat(subtotal.ToString())));
                    }
                    strContent.Append(strTotal.Replace("SUB_TOTAL", "Total rows: " + lstG2ClientNo.Count.ToString())
                                        .Replace("G2_Auth_Date", countNo.ToString())
                                        .Replace("Aperia_Auth_Amount", NumberFormat(totalNo.ToString())));
                }
                strContent.Append("<br /><br /><br />");

                strContent.Append(strTemplate.Substring(endIndexClientTotal).Replace("SUB_TOTAL", "Grand Total: " + (lstG2ClientYes.Count + lstG2ClientNo.Count).ToString())
                                    .Replace("G2_Auth_Date", (countNo + countYes).ToString())
                                    .Replace("Aperia_Auth_Amount", NumberFormat((totalNo + totalYes).ToString())));

                ////////////////////////////////////////////////////////////////////////////////////
                Console.WriteLine("\n\n----- Start Save File... ----- \n");
                Logger.Info("\n\n----- Start Save File... ----- ");


                Doc pdfDoc = new Doc();
                pdfDoc.Units = UnitType.Points;
                pdfDoc.HtmlOptions.UseNoCache = true;

                pdfDoc.Rect.Inset(20, 30);
                pdfDoc.SaveOptions.Linearize = false;
                pdfDoc.HtmlOptions.Timeout = 600000;
                int theID;
                theID = pdfDoc.AddImageHtml(ReplaceStr(strContent.ToString()));
                while (true)
                {
                    if (!pdfDoc.Chainable(theID))
                        break;
                    pdfDoc.Page = pdfDoc.AddPage();
                    theID = pdfDoc.AddImageToChain(theID);

                }
                pdfDoc.Save(ExportTempFolder);
                pdfDoc.Clear();

                Console.WriteLine("\n\n----- End Start Save File... ----- \n");
                Logger.Info("\n\n----- End Start Save File... ----- ");
                Console.WriteLine("\n\n----- Update Status On Database... ----- \n");
                Logger.Info("\n\n----- Update Status On Database... ----- ");
                extractReportLogModel = new ExtractReportLogModel()
                {
                    SpaName = SPAName_UpdateExtractReportLog,
                    ASClient = Client_Id_Default,
                    ReportDate = ReportDate,
                    FileName = fileInfo.Name,
                    FileType = "PDF",
                    FilePath = ExportTempFolder,
                    Status = "2",
                    NumberOfMatches = countNo + countYes
                };
                dbe.UpdateExtractReportLog(extractReportLogModel);
                Console.WriteLine("\n\n----- Update Status On Database... ----- \n");
                Logger.Info("\n\n----- Update Status On Database... ----- ");
                Console.WriteLine("\n\n----- End Program... ----- \n");
                Logger.Info("\n\n----- End Program... ----- ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n----- Error: " + ex + " ----- \n");
                Logger.Info("\n\n----- Error: " + ex + " ----- \n");
                throw new ArgumentException("Exception when ExportAndSaveFile", ex);
            }
        }


        private static string ReplaceStr(string text)
        {
            text = text.Replace("G2MONEYLAUNDERING_BOOLCLIENT_BEGIN", "")
                 .Replace("G2MONEYLAUNDERING_BOOLCLIENT_END", "")
                 .Replace("G2MONEYLAUNDERING_CLIENTNAME_BEGIN", "")
                 .Replace("G2MONEYLAUNDERING_CLIENTNAME_END", "")
                 .Replace("G2MONEYLAUNDERING_ROW_BEGIN", "")
                 .Replace("G2MONEYLAUNDERING_ROW_END", "")
                 .Replace("G2MONEYLAUNDERING_SUBTOTAL_BEGIN", "")
                 .Replace("G2MONEYLAUNDERING_SUBTOTAL_END", "");
            return text;
        }

        private static string DateFormat(string text)
        {
            DateTime dt = Convert.ToDateTime(text);
            return dt.ToString("MM/dd/yy");
        }
        //This fix have been not Released
        private static string NumberFormat(string text)
        {
            string cssFormat = "<span style='color:{0}'>{1}</span>";
            if (string.IsNullOrEmpty(text) || Convert.ToDouble(text) == 0)
                return "$0.00";

            if (Convert.ToDouble(text) < 0)
                return string.Format(cssFormat, "Red", "($" + Convert.ToDouble(text).ToString("#,##0.00").Replace("-", "") + ")");

            return string.Format(cssFormat, "Black", "$" + Convert.ToDouble(text).ToString("#,##0.00"));
        }
    }
}
