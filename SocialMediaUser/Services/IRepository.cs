using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SocialMediaUser.Services;

public interface IRepository<T> where T: class
{
    public void Add(T item);

    public void Remove(T item);

    public void Update(T item);

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    
    public T? this[int index] { get; }

    public IEnumerable<T> GetAll();

    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
}