using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;

        private IOwnerRepository _ownerRepository;

        private IAccountRepository _accountRepository;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IOwnerRepository OwnerRepository
        {
            get
            {
                if(_ownerRepository == null)
                {
                    _ownerRepository = new OwnerRepository(_repositoryContext);
                }

                return _ownerRepository;
            }
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if(_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_repositoryContext);
                }

                return _accountRepository;
            }
        }
    }
}
