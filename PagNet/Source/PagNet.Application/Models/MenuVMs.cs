using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Application.Models
{
    public class DadosUserVm
    {
        public string nmUsuario { get; set; }
        public string Login { get; set; }
        public string Perfil { get; set; }
    }
    public class MenuVMs
    {
        public MenuVMs()
        {
            Listamenus = new List<MenuVMs>();
            ListamenusNetos = new List<MenuVMs>();
        }

        public IList<MenuVMs> Listamenus { get; set; }
        public IList<MenuVMs> ListamenusNetos { get; set; }

        public string CaminhoImgLogo { get; set; }

        public string nmUsuario { get; set; }
        public string Login { get; set; }
        public string Perfil { get; set; }

        public int codMenu { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? codMenuPai { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Nivel { get; set; }
        public int Ordem { get; set; }
        public string Ativo { get; set; }
        public string favIcon { get; set; }
        public int codParametro { get; set; }

        internal static MenuVMs ToViewMenus(PAGNET_MENU item, List<PAGNET_MENU> TodosMenus)
        {
            return new MenuVMs
            {
                codMenu = item.codMenu,
                Nome = item.Nome,
                Descricao = item.Descricao,
                codMenuPai = item.codMenuPai,
                Area = item.Area,
                Controller = item.Controller,
                Action = item.Action,
                Nivel = item.Nivel,
                Ordem = item.Ordem,
                Ativo = item.Ativo,
                favIcon = item.favIcon,
                codParametro = 0,
                Listamenus = MenuVMs.ToViewFilho(TodosMenus.Where(x => x.Nivel == 2 && x.codMenuPai == item.codMenu).OrderBy(x => x.Ordem).ToList(), TodosMenus)
            };
        }
        internal static MenuVMs ToViewMenusRelatorio(PAGNET_MENU item, List<PAGNET_RELATORIO> filho)
        {
            return new MenuVMs
            {
                codMenu = item.codMenu,
                Nome = item.Nome,
                Descricao = item.Descricao,
                codMenuPai = item.codMenuPai,
                Area = item.Area,
                Controller = item.Controller,
                Action = item.Action,
                Nivel = item.Nivel,
                Ordem = item.Ordem,
                Ativo = item.Ativo,
                favIcon = item.favIcon,
                codParametro = 0,
                Listamenus = MenuVMs.ToViewRelatorio(filho)
            };
        }

        internal static IList<MenuVMs> ToViewFilho<T>(ICollection<T> collection, List<PAGNET_MENU> TodosMenus) where T : PAGNET_MENU
        {
            return collection.Select(x => ToViewFilho(x, TodosMenus)).ToList();
        }

        internal static MenuVMs ToViewFilho(PAGNET_MENU filho, List<PAGNET_MENU> TodosMenus)
        {
            return new MenuVMs
            {
                codMenu = filho.codMenu,
                Nome = filho.Nome,
                Descricao = filho.Descricao,
                codMenuPai = filho.codMenuPai,
                Area = filho.Area,
                Controller = filho.Controller,
                Action = filho.Action,
                Nivel = filho.Nivel,
                Ordem = filho.Ordem,
                codParametro = 0,
                Ativo = filho.Ativo,
                ListamenusNetos = MenuVMs.ToViewNeto(TodosMenus.Where(x => x.Nivel == 3 && x.codMenuPai == filho.codMenu).OrderBy(x => x.Ordem).ToList())
            };
        }

        internal static IList<MenuVMs> ToViewNeto<T>(ICollection<T> collection) where T : PAGNET_MENU
        {
            return collection.Select(x => ToViewNeto(x)).ToList();
        }

        internal static MenuVMs ToViewNeto(PAGNET_MENU x)
        {
            return new MenuVMs
            {
                codMenu = x.codMenu,
                Nome = x.Nome,
                Descricao = x.Descricao,
                codMenuPai = x.codMenuPai,
                Area = x.Area,
                Controller = x.Controller,
                Action = x.Action,
                Nivel = x.Nivel,
                Ordem = x.Ordem,
                codParametro = 0,
                Ativo = x.Ativo
            };
        }

        internal static IList<MenuVMs> ToViewRelatorio<T>(ICollection<T> collection) where T : PAGNET_RELATORIO
        {
            return collection.Select(x => ToViewRelatorio(x)).ToList();
        }

        internal static MenuVMs ToViewRelatorio(PAGNET_RELATORIO x)
        {
            return new MenuVMs
            {
                codMenu = x.ID_REL,
                Nome = "Rel_" + x.ID_REL,
                Descricao = x.NOMREL,
                Area = "Relatorios",
                Controller = "Relatorios",
                Action = "Index",
                Nivel = 2,
                Ordem = 1,
                codParametro = x.ID_REL,
                Ativo = "S"
            };
        }
    }
}
