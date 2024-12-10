using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataConnectors
{
    internal interface IDatabaseClient : IDisposable
    {
        
        public Task<IEnumerable<TEntity>> GetListOfTypeAsync<TEntity>() where TEntity : ModelBase;
        public void Insert<TEntity>(TEntity model) where TEntity : ModelBase;
        public void Delete<TEntity>(TEntity model) where TEntity : ModelBase;
        public void Replace<TEntity>(TEntity model) where TEntity : ModelBase;
        public Task<T?> GetByIdAsync<T>(int id) where T : ModelBase;
        public Task SaveChangesAsync();

    }
}
