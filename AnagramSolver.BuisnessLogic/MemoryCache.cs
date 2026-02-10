using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    internal class MemoryCache<T>
    {
        private Dictionary<string, T> _cache = new();

        public bool TryGetCache(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void AddCache(string key, T value)
        {
            _cache[key] = value;
        }
    }
}
