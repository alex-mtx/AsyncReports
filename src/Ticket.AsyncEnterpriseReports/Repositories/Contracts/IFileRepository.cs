using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.AsyncEnterpriseReports.Repositories.Contracts
{
    public interface IFileRepository
    {
        void Write(string fullFileName);
        void Write(byte[] content, string filename);
        void Delete(string fullFileName);
        void DeleteAllUpUntil(DateTime finalDate);
    }
}
