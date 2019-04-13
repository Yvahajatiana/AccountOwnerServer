using Entities.ExtendedModels;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();

        Task<Owner> GetOwnerByIdAsync(Guid Id);

        Task<OwnerExtended> GetOwnerWithDetailsAsync(Guid Id);

        Task CreateOwnerAsync(Owner owner);

        Task UpdateOwnerAsync(Owner ownerDb, Owner owner);

        Task DeleteOwnerAsync(Owner owner);
    }
}
