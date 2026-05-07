using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Repository
{
    public class DocServerRepository : IDocumentStorageRepository
    {
        public byte[] DownloadDocument(long docId)
        {
            return WebServices.DocServices.DownloadDoc((int)docId);
        }
    }
}
