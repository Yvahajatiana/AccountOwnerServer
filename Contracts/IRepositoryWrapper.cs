using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IOwnerRepository OwnerRepository { get; }

        IAccountRepository AccountRepository { get; }
    }
}
