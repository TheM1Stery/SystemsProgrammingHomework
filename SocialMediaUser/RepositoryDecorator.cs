using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SimpleInjector.Lifestyles;

namespace SocialMediaUser;

public partial class App
{
    private partial class RepositoryDecorator<T>
    {
        public void Add(T item)
        {
            using var scope = AsyncScopedLifestyle.BeginScope(_container);
            var repository = _decorateeFactory.Invoke();
            repository.Add(item);
        }

        public void Remove(T item)
        {
            using var scope = AsyncScopedLifestyle.BeginScope(_container);
            var repository = _decorateeFactory.Invoke();
            repository.Remove(item);
        }

        public void Update(T item)
        {
            using var scope = AsyncScopedLifestyle.BeginScope(_container);
            var repository = _decorateeFactory.Invoke();
            repository.Update(item);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            using var scope = AsyncScopedLifestyle.BeginScope(_container);
            var repository = _decorateeFactory.Invoke();
            return repository.Find(expression);
        }

        public T? this[int index]
        {
            get
            {
                using var scope = AsyncScopedLifestyle.BeginScope(_container);
                var repository = _decorateeFactory.Invoke();
                return repository[index];
            }
        }

        public IEnumerable<T> GetAll()
        {
            using var scope = AsyncScopedLifestyle.BeginScope(_container);
            var repository = _decorateeFactory.Invoke();
            return repository.GetAll();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            using var scope = AsyncScopedLifestyle.BeginScope(_container);
            var repository = _decorateeFactory.Invoke();
            return repository.GetAll(includes);
        }
    }
}