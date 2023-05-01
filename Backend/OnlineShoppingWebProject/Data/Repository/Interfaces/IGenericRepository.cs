using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repository
{
	public interface IGenericRepository<T> where T : class
	{
		T GetById(int id);

		IEnumerable<T> GetAll();
		
		IEnumerable<T> FindAll(Expression<Func<T, bool>> expression);
		
		T FindFirst(Expression<Func<T, bool>> expression);

		void Add(T entity);
		
		void AddRange(IEnumerable<T> entities);
		
		void Remove(T entity);
		
		void RemoveRange(IEnumerable<T> entities);
	}
}
