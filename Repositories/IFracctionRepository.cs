using GoldenGateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Repositories
{
    public interface IFracctionRepository
    {
        Task<IEnumerable<Fraccion>> GetAllFractions();

        Task<Fraccion> GetFractionDetails(int id);

        Task<bool> InsertFraction(Fraccion fraccion);

        Task<bool> UpdateFraction(Fraccion fraccion);

        Task<bool> DeleteFraction(int id);
    }
}
