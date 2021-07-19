using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class MenuItemCarga : MenuItem
    {
        public MenuItemCarga(string menu, string tipoCarga)
            : base(menu, "FCARGA", "CliListCargasSolicitadas", "Index", null)
        {
            _tipoCarga = tipoCarga;
        }

        private readonly string _tipoCarga;

        public override object GetRouteValues()
        {
            return new { Id = IdPermissao, TipoCarga = _tipoCarga };
        }
    }
}
