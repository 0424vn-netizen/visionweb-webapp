using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Repository
{
    public interface IDocumentStorageRepository
    {
        byte[] DownloadDocument(long docId);
    }
}
