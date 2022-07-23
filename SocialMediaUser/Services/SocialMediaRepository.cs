using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaUser.Services;

public class SocialMediaRepository<T> : IRepository<T> where T: class
{
    private readonly SocialMediaDbContext _dbContext;

    private readonly DbSet<T> _dbSet;


    public SocialMediaRepository(SocialMediaDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
        _dbSet = _dbContext.Set<T>();
    }
    
    
    public void Add(T item)
    {
        _dbSet.Add(item);
        _dbContext.SaveChanges();
    }

    public void Remove(T item)
    {
        _dbSet.Remove(item);
        _dbContext.SaveChanges();
    }

    public void Update(T item)
    {
        _dbSet.Update(item);
        _dbContext.SaveChanges();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public T? this[int index] => _dbSet.Find(index);

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }
}