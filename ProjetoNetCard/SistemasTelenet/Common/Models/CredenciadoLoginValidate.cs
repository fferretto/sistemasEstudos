using NetCard.Common.Util;
using System;
using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public sealed class CredenciadoLoginValidate : LoginValidateBase
    {
        public CredenciadoLoginValidate(ObjConn objConexao)
            : base(objConexao)
        { }

        private DadosAcesso CreateDadosAcesso(System.Data.IDataReader reader, Login login)
        {
            return new DadosAcesso
            {
                Nome = Convert.ToString(reader["RAZAO"]),
                Cnpj = Convert.ToString(reader["CNPJ"]),
                CodCen = Convert.ToInt32(reader["CODCEN"]),
                UltAcesso = Convert.ToString(reader["ULTACESSO"]),
                Login = login.LogIn,
                Codigo = Convert.ToInt32(login.LogIn),
                Acesso = login.Acesso,
                Sistema = new Sistema()
            };
        }

        protected override LoginValidation OnValidate(System.Data.IDataReader reader, Login login)
        {
            var listLogins = new List<DadosAcesso>();
            var retorno = string.Empty;
            var acao = string.Empty;

            if (reader.Read())
            {
                retorno = Convert.ToString(reader["RETORNO"]);
                if (retorno != Constantes.ok)
                {
                    return new LoginValidation(false, retorno, null, null);
                }

                acao = reader["ACAO"].ToString();
                var flagPj = (Convert.ToString(reader["FLAG_PJ"])) == "S" ? 1 : 0;
                var flagVa = (Convert.ToString(reader["FLAG_VA"])) == "S" ? 1 : 0;
                var flagPont = (Convert.ToString(reader["FLAG_PT"])) == "S" ? 1 : 0;
                var contFlag = flagPj + flagVa + flagPont;

               


                if (contFlag > 0)
                {
                    if (flagPj == 1)
                    {
                        var valido = CreateDadosAcesso(reader, login);
                        valido.Sistema.IdAba = 0;
                        valido.Sistema.ControllerPrincipal = "Home";
                        valido.Sistema.ActionPrincipal = "Index";
                        valido.Sistema.cartaoPJVA = 0;
                        valido.Sistema.NomeAba = "CARTÃO PÓS PAGO\n (CRÉDITO)";
                        listLogins.Add(valido);
                    }
                    if (flagVa == 1)
                    {
                        var valido = CreateDadosAcesso(reader, login);
                        valido.Sistema.IdAba = 1;
                        valido.Sistema.ControllerPrincipal = "Home";
                        valido.Sistema.ActionPrincipal = "Index";
                        valido.Sistema.cartaoPJVA = 1;
                        valido.Sistema.NomeAba = "CARTÃO PRÉ PAGO\n (DÉBITO)";
                        listLogins.Add(valido);
                    }
                    if (flagPont == 1)
                    {
                        var valido = CreateDadosAcesso(reader, login);
                        valido.Endereco = Convert.ToString(reader["ENDCRE"]);
                        valido.Bairro = Convert.ToString(reader["NOMBAI"]);
                        valido.DtAdesaoFidelidade = Convert.ToDateTime(reader["DTADESAO"]);
                        valido.Sistema.IdAba = 2;
                        valido.Sistema.ControllerPrincipal = "CreFidelidade";
                        valido.Sistema.ActionPrincipal = "ConsultaPontuacao";
                        valido.Sistema.cartaoPJVA = 2;
                        valido.Sistema.NomeAba = "FIDELIDADE" + "\n" + "              ";
                        listLogins.Add(valido);
                    }

                }
            }

            return new LoginValidation(true, retorno, acao, listLogins);
        }
    }
}
