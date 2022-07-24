using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaUser.Services;

public class SocialMediaRepository<T> : IRepository<T> where T: class
{
    public SocialMediaRepository()
    {
        using var dbContext = new SocialMediaDbContext();
        dbContext.Database.EnsureCreated();
    }
    
    
    public void Add(T item)
    {
        using var dbContext = new SocialMediaDbContext();
        dbContext.Database.EnsureCreated();
        var dbSet = dbContext.Set<T>();
        dbSet.Add(item);
        dbContext.SaveChanges();
    }

    public void Remove(T item)
    {
        using var dbContext = new SocialMediaDbContext();
        var dbSet = dbContext.Set<T>();
        dbSet.Remove(item);
        dbContext.SaveChanges();
    }

    public void Update(T item)
    {
        using var dbContext = new SocialMediaDbContext();
        var dbSet = dbContext.Set<T>();
        dbSet.Update(item);
        dbContext.SaveChanges();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        using var dbContext = new SocialMediaDbContext();
        var dbSet = dbContext.Set<T>();
        return dbSet.Where(expression).ToList();
    }

    public T? this[int index]
    {
        get
        {
            using var dbContext = new SocialMediaDbContext();
            var dbSet = dbContext.Set<T>();
            return dbSet.Find(index);
        }
    }

    public IEnumerable<T> GetAll()
    {
        using var dbContext = new SocialMediaDbContext();
        var dbSet = dbContext.Set<T>();
        return dbSet.ToList();
    }
}