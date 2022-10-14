﻿using GoldenGateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Repositories
{
    public interface ISifenRepository
    {
        Task<IEnumerable<Dte>> GetAllDtes();
        Task<IEnumerable<Item>> GetDteItemsByID(Int32 Id);

        Task<IEnumerable<Dte>> GetDteByID(Int32 Id);
        Task<bool> InsertDTE(Dte dte);
        Task<bool> InsertItem(Item item);
        Task<IEnumerable<Response>> SendDTE(Command command);

        Task<IEnumerable<Operation>> GetOperationsByDteID(Int32 Id);




    }
}
