using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StrictlyStatsDataLayer
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);

        List<T> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        T Get(Expression<Func<T, bool>> predicate);
        TableQuery<T> AsQueryable();

        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Delete(int id);
    }

}