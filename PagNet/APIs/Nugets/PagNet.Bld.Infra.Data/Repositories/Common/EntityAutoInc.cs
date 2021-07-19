using System.Collections.Generic;

namespace PagNet.Bld.Infra.Data.Repositories.Common
{
    public static class EntityAutoInc<TEntity>
        where TEntity : class
    {
        private static Dictionary<string, int> _currentId = new Dictionary<string, int>();
        private static object _sync = new object();

        public static void InitialSeed(string baseName, int initialSeed)
        {

            lock (_sync)
            {
                if (!_currentId.ContainsKey(baseName))
                    _currentId.Add(baseName, initialSeed);
                else
                    _currentId[baseName] = initialSeed;
            }
        }

        public static int GetNextId(string baseName)
        {
            lock (_sync)
            {
                _currentId[baseName] += 1;

                return _currentId[baseName];
            }
        }
    }
}
