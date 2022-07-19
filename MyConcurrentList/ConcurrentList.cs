using System.Collections;

namespace MyConcurrentList;

public class ConcurrentList<T> : IList<T>
{
    private readonly object _objectLock = new();

    private readonly List<T> _list = new();
    
    
    public IEnumerator<T> GetEnumerator()
    {
        lock (_objectLock)
        {
            return _list.GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        lock (_objectLock)
        {
            _list.Add(item);
        }
    }

    public void Clear()
    {
        lock (_objectLock)
        {
            _list.Clear();
        }
    }

    public bool Contains(T item)
    {
        lock (_objectLock)
        {
            return _list.Contains(item);
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (_objectLock)
        {
            _list.CopyTo(array, arrayIndex);
        }
    }

    public bool Remove(T item)
    {
        lock (_objectLock)
        {
            return _list.Remove(item);
        }
    }

    public int Count
    {
        get
        {
            lock (_objectLock)
            {
                return _list.Count;
            }
        }
    }
    public bool IsReadOnly => false;
    
    public int IndexOf(T item)
    {
        lock (_objectLock)
        {
            return _list.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        lock (_objectLock)
        {
            _list.Insert(index, item);
        }
    }

    public void RemoveAt(int index)
    {
        lock (_objectLock)
        {
            _list.RemoveAt(index);
        }
    }

    public T this[int index]
    {
        get
        {
            lock (_objectLock)
            {
                return _list[index];
            }
        }
        set
        {
            lock (_objectLock)
            {
                _list[index] = value;
            }
        }
    }
}