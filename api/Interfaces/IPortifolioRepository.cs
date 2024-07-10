using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortifolioRepository
    {
        Task<List<Stock>> GetUserPortifolio(AppUser user);
        Task<Portifolio> CreateAsync(Portifolio portfolio);
        Task<Portifolio> DeletePortifolio(AppUser appUser, string symbol);
    }
}