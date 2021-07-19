#pragma warning disable 1591

using System;

namespace Telenet.Core.DependencyInjection
{
    internal class SingletonFactory<TService> : TransientFactory<TService>
    {
        public SingletonFactory(Func<TService> factory)
            : base(factory)
        { }

        private static TService _instance;
        private static object _sync = new object();

        public override object GetService()
        {
            if (_instance == null)
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = Factory();
                    }
                }
            }

            return _instance;
        }
    }
}

#pragma warning restore 1591
