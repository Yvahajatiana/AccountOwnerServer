using Contracts;
using Entities;
using Entities.ExtendedModels;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
        {
            var owners = await FindAllAsync();

            return owners.OrderBy(o => o.Name);
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid Id)
        {
            var owner = await FindByConditionAsync(o => o.Id.Equals(Id));

            return owner.DefaultIfEmpty(new Owner())
                        .FirstOrDefault();
        }

        public async Task<OwnerExtended> GetOwnerWithDetailsAsync(Guid Id)
        {
            return new OwnerExtended( await GetOwnerByIdAsync(Id))
            {
                Accounts = await RepositoryContext.Accounts
                .Where(a => a.OwnerId.Equals(Id)).ToListAsync()
            };
        }

        public async Task CreateOwnerAsync(Owner owner)
        {
            owner.Id = Guid.NewGuid();
            Create(owner);
            await SaveAsync();
        }

        public async Task UpdateOwnerAsync(Owner ownerDb, Owner owner)
        {
            ownerDb.Map(owner);
            Update(ownerDb);
            await SaveAsync();
        }

        public async Task DeleteOwnerAsync(Owner owner)
        {
            Delete(owner);
            await SaveAsync();
        }
    }
}
