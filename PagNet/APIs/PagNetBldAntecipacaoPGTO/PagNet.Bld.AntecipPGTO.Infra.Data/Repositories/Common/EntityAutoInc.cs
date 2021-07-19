
namespace PagNet.Bld.AntecipPGTO.Infra.Data.Repositories.Common
{
    public static class EntityAutoInc<TEntity>
        where TEntity : class
    {
        private static int _currentId = -1;
        private static object _sync = new object();

        public static void InitialSeed(int initialSeed)
        {
            if (_currentId >= 0) return;

            lock (_sync)
            {
                _currentId = initialSeed;
            }
        }

        public static int GetNextId()
        {
            lock (_sync)
            {
                return ++_currentId;
            }
        }
    }
}
