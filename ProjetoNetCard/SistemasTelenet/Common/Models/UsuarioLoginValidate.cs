using NetCard.Common.Util;
using System;
using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public sealed class UsuarioLoginValidate : LoginValidateBase
    {
        public UsuarioLoginValidate(ObjConn objConexao)
            : base(objConexao)
        { }

        protected override LoginValidation OnValidate(System.Data.IDataReader reader, Login login)
        {
            long resultado;
            if (!long.TryParse(login.LogIn, out resultado))
            {
                return new LoginValidation(false, "Cartão informado deve conter apenas números.", null, null);
            }

            var idAba = 0;
            var listLogins = new List<DadosAcesso>();
            var retorno = string.Empty;
            var acao = string.Empty;

            while (reader.Read())
            {
                retorno = Convert.ToString(reader["RETORNO"]);

                if (retorno != Constantes.ok)
                {
                    return new LoginValidation(false, retorno, null, null);
                }

                acao = Convert.ToString(reader["ACAO"]);
                var urlInicial = Convert.ToString(reader["MW_TELA_INICIAL"]).Split('/');

                var valido = new DadosAcesso
                {
                    Acesso = login.Acesso,
                    CodCrt = Convert.ToString(reader["CODCRT"]),
                    Cpf = Convert.ToString(reader["CPF"]),
                    IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                    NumTit = Convert.ToInt32(reader["NUMTIT"]),
                    Nome = Convert.ToString(reader["NOMUSU"]),
                    NumDep = Convert.ToInt16(reader["NUMDEP"]),
                    Sistema = new Sistema
                    {
                        IdAba = idAba,
                        ControllerPrincipal = Convert.ToInt16(reader["SISTEMA"]) > 1 ? "UsuTermoAdesao" : urlInicial.Length >= 2 ? urlInicial[0] : "Home",
                        ActionPrincipal = Convert.ToInt16(reader["SISTEMA"]) > 1 ? "Pontuacao" : urlInicial.Length >= 2 ? urlInicial[1] : "Index",
                        cartaoPJVA = Convert.ToInt32(reader["SISTEMA"]),
                    },
                    TemSenha = Convert.ToString(reader["TEMSENHA"]) == Constantes.sim
                };

                if (Convert.ToInt16(reader["SISTEMA"]) < 2)
                    valido.TipProd = Convert.ToInt16(reader["TIPOPROD"]);
                
                var rotulo = reader["ROTULO"].ToString();

                if (rotulo.Length > 15)
                {
                    rotulo = rotulo.Substring(0, 15);
                }

                if (rotulo.Length <= 15)
                {
                    var complementoRotulo = (15 - rotulo.Length) / 2;
                    rotulo = rotulo.PadLeft(rotulo.Length + complementoRotulo, ' ');
                    rotulo = rotulo.PadRight(rotulo.Length + complementoRotulo, ' ');
                    rotulo = rotulo.PadRight(15, ' ');
                }

                valido.Sistema.NomeAba = Convert.ToInt16(reader["SISTEMA"]) > 1 ? rotulo + "\n\n" : rotulo + "\n" + valido.CodCrtMask;
                valido.SistemaAcessado = valido.Sistema.IdAba;
                valido.Status = reader["STA"].ToString();

                if (Convert.ToInt16(reader["SISTEMA"]) < 2)
                {
                    valido.Codigo = Convert.ToInt32(reader["CODCLI"]);
                }

                valido.UltAcesso = reader["ULTACESSO"].ToString();

                if (reader["HABMOBILE"] != DBNull.Value && reader["HABMOBILE"].ToString().Trim() != string.Empty)
                    valido.HabMobile = reader["HABMOBILE"].ToString();

                if (reader["HABTOKENCOMCAD"] != DBNull.Value && reader["HABTOKENCOMCAD"].ToString().Trim() != string.Empty)
                    valido.HabTokencomCad = reader["HABTOKENCOMCAD"].ToString() == Constantes.sim;

                if (reader["HABFIDELIDADE"] != DBNull.Value && reader["HABFIDELIDADE"].ToString().Trim() != string.Empty)
                {
                    valido.HabFidelidade = reader["HABFIDELIDADE"].ToString() == Constantes.sim;
                    valido.AdesaoPontuacao = reader["ADERIU"].ToString() == Constantes.sim;
                }

                listLogins.Add(valido);
                idAba++;
            }

            return new LoginValidation(true, retorno, acao, listLogins);
        }
    }
}
