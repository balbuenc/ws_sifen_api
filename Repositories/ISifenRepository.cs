using GoldenGateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Repositories
{
    public interface ISifenRepository
    {
        Task<IEnumerable<Dte>> GetAllDtes();
        Task<IEnumerable<Item>> GetDteByID(int Id);
        Task<bool> InsertDTE(Dte dte);

    }
}
