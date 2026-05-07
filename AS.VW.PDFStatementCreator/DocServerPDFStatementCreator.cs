using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Repository;

namespace AS.VW.PDFStatementCreator
{
    public class DocServerPdfStatementCreator : PdfStatementCreatorBase
    {
        private readonly IDocumentStorageRepository _documentStorageRespository = null;

        public DocServerPdfStatementCreator()
        { 
            _documentStorageRespository = RepositoryFactory.Create<IDocumentStorageRepository>();
        }

        public override PdfStatementCreatorResult Execute(object parameterContent, int? asClientId = null, int? siteId = null, string userId = "", string userMode = "", DateTime? reportDate = null)
        {
            byte[] result = _documentStorageRespository.DownloadDocument(long.Parse(parameterContent.ToString()));
            return new PdfStatementCreatorResult(PdfStatementResultType.Byte, ForceReturnType, result);
        }
    }
}
