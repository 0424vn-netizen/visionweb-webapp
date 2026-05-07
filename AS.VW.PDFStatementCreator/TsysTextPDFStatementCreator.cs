using AS.VW.Entities;
using AS.VW.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupergoo.ABCpdf9;

namespace AS.VW.PDFStatementCreator
{
    public class TsysTextPdfStatementCreator: PdfStatementCreatorBase
    {
        private readonly IStatementRepository _statementRepository = null;

        public TsysTextPdfStatementCreator()
        {
            _statementRepository = RepositoryFactory.Create<IStatementRepository>();
        }

        public override PdfStatementCreatorResult Execute(object parameterContent, int? asClientId = null, int? siteId = null, string userId = "", string userMode = "", DateTime? reportDate = null)
        {
            var table = _statementRepository.GetStatementDetail("spa_GetStatements_FIS", asClientId.Value , siteId.Value, userId, userMode, reportDate.Value, (string)parameterContent);

            StringBuilder buider = new StringBuilder();
            buider.Append(@"<pre class='PrintMode'>");
            for (int k = 0; k < table.Rows.Count; k++)
            {
                buider.Append("&nbsp;" + table.Rows[k]["LineData"].ToString());
                buider.Append(@"<br/>");
            }
            buider.Append(@"</pre>");
            buider = buider.Replace("\t", @"&#09;");

            Doc theDoc = new Doc();
            theDoc.HtmlOptions.Engine = EngineType.Gecko;

            int docH = 770;
            int docW = 630;

            theDoc.MediaBox.Width = docW;
            theDoc.MediaBox.Height = docH;
            string headerText = @"       
               <style>
                            .reporttitle {{
                            color:Black;
                            font-family:Arial;
                            font-size:19px;
                            font-weight:bold;
                            text-align:left;
                            }}
                            .gridtitle {{
                            color:Black;
                            font-family:Arial;
                            font-size:15px;
                            font-weight:bold;
                            }}
                </style>&nbsp;";


            const int margin = 25;
            const int marginTB = 12;
            const int headerHeight = 0;
            theDoc.Rect.SetRect(margin - 8, docH - marginTB - headerHeight, docW - margin * 2, headerHeight);
            theDoc.AddImageHtml(headerText);

            theDoc.Rect.SetRect(margin, marginTB, docW - margin * 2, docH - marginTB * 2 - headerHeight);
            int theID = theDoc.AddImageHtml(buider.ToString());
            theDoc.FrameRect();
            theDoc.Rect.SetRect(margin, marginTB, docW - margin * 2, docH - marginTB * 2);

            while (theDoc.Chainable(theID))
            {
                theDoc.Page = theDoc.AddPage();

                theID = theDoc.AddImageToChain(theID);
                theDoc.FrameRect();
            }

            theDoc.PageNumber = 1;

            return new PdfStatementCreatorResult(PdfStatementResultType.Byte, ForceReturnType, theDoc.GetData());
        }
    }
}
