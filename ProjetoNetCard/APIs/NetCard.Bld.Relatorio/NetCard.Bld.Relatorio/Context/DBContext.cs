using Microsoft.Extensions.DependencyInjection;
using NetCard.Bld.Relatorio.Abstraction.Interface;
using NetCard.Bld.Relatorio.Data;
using NetCard.Bld.Relatorio.Entity;
using System;
using Telenet.Data;

namespace NetCard.Bld.Relatorio.Context
{
    public class DbNetCardContext : IDbClientContext
    {
        public DbNetCardContext(IServiceProvider user)
        {
            _user = user;
        }
        protected readonly IServiceProvider _user;

        public string ConnectionString
        {
            get
            {
                var service = _user.CreateScope().ServiceProvider.GetService(typeof(IParametrosApp)) as IParametrosApp;
                return service.GetConnectionString();
            }
        }
        public string Name => "NetCard";
    }
    public class DbConcentradorContext : IDbClientContext
    {
        public DbConcentradorContext(IServiceProvider user)
        {
            _user = user;
        }
        protected readonly IServiceProvider _user;
        public string ConnectionString
        {
            get
            {
                var service = _user.CreateScope().ServiceProvider.GetService(typeof(IParametrosApp)) as IParametrosApp;
                return service.GetConnectionStringConcentrador();
            }
        }
        public string Name => "NetCard";
    }

}
