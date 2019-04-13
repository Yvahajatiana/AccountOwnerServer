﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAllAsync();

        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveAsync();

    }
}
