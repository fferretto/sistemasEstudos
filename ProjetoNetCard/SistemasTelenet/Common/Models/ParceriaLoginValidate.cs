using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetCard.Common.Models
{
    public sealed class ParceriaLoginValidate : LoginValidateBase
    {
        public ParceriaLoginValidate(ObjConn objConexao)
            : base(objConexao)
        { }

        protected override LoginValidation OnValidate(IDataReader reader, Login login)
        {
            var listLogins = new List<DadosAcesso>();
            var idAba = 0;
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

                var valido = new DadosAcesso
                {
                    Login = login.LogIn,
                    Codigo = Convert.ToInt32(reader["CODIGO"]),
                    Sistema = new Sistema
                    {
                        IdAba = idAba,
                        ControllerPrincipal = "Home",
                        ActionPrincipal = "Index",
                        cartaoPJVA = Convert.ToInt32(reader["SISTEMA"])
                    }
                };

                var rotulo = Convert.ToString(reader["ROTULO"]);

                if (rotulo.Length > 15)
                    rotulo = rotulo.Substring(0, 15);

                if (rotulo.Length <= 15)
                {
                    var complementoRotulo = (15 - rotulo.Length) / 2;
                    rotulo = rotulo.PadLeft(rotulo.Length + complementoRotulo, ' ');
                    rotulo = rotulo.PadRight(rotulo.Length + complementoRotulo, ' ');
                }

                valido.Sistema.NomeAba = rotulo + "\n" + valido.Codigo.ToString().PadLeft(5, '0');
                valido.Acesso = login.Acesso;
                valido.SistemaAcessado = valido.Sistema.IdAba;
                valido.Nome = Convert.ToString(reader["NOMCLI"]);
                valido.Cnpj = Convert.ToString(reader["CNPJCLI"]);
                valido.Id = Convert.ToInt32(reader["ID_OPERADOR"]);
                valido.CnpjOpe = Convert.ToString(reader["CNPJOPER"]);
                valido.RazaoOpe = Convert.ToString(reader["RAZAOOPER"]);
                valido.Saldo = Convert.ToDecimal(reader["SALDOCONTA"]);
                valido.Conpmo = Convert.ToString(reader["CONPMO"]);
                valido.TipProd = Convert.ToInt16(reader["TIPOPROD"]);
                valido.CodParceria = Convert.ToInt32(reader["CODPARCERIA"]);
                valido.TipoAcesso = Convert.ToString(reader["TIPOACESSO"]); // tipo acesso pode ser C = Cliente ou P = Parceria
                valido.Status = Convert.ToString(reader["STACLI"]);
                valido.UltAcesso = Convert.ToString(reader["ULTACESSO"]);
                valido.Endereco = Convert.ToString(reader["ENDERECO"]);
                valido.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                valido.Bairro = Convert.ToString(reader["BAIRRO"]);
                valido.Uf = Convert.ToString(reader["UF"]);
                valido.Cidade = Convert.ToString(reader["CIDADE"]);
                valido.Cep = Convert.ToString(reader["CEP"]);

                if (reader["HABMAXPARC"] != DBNull.Value && reader["HABMAXPARC"].ToString().Trim() != string.Empty)
                    valido.HabMaxParc = reader["HABMAXPARC"].ToString();

                if (reader["HABPREMIO"] != DBNull.Value && reader["HABPREMIO"].ToString().Trim() != string.Empty)
                    valido.HabPremio = reader["HABPREMIO"].ToString();

                if (reader["HABLIMDEP"] != DBNull.Value && reader["HABLIMDEP"].ToString().Trim() != string.Empty)
                    valido.HabLimDep = reader["HABLIMDEP"].ToString();

                if (reader["HABINCDEP"] != DBNull.Value && reader["HABINCDEP"].ToString().Trim() != string.Empty)
                    valido.HabIncDep = reader["HABINCDEP"].ToString();

                if (reader["HABEXCDEP"] != DBNull.Value && reader["HABEXCDEP"].ToString().Trim() != string.Empty)
                    valido.HabExcDep = reader["HABEXCDEP"].ToString();

                if (reader["HABCANCMASS"] != DBNull.Value && reader["HABCANCMASS"].ToString().Trim() != string.Empty)
                    valido.HabCancMass = reader["HABCANCMASS"].ToString();

                if (reader["HABTRANSFSAL"] != DBNull.Value && reader["HABTRANSFSAL"].ToString().Trim() != string.Empty)
                    valido.HabTransfSal = reader["HABTRANSFSAL"].ToString();

                if (reader["HABBENEFICIOS"] != DBNull.Value && reader["HABBENEFICIOS"].ToString().Trim() != string.Empty)
                {
                    if (reader["HABBENEFICIOS"].ToString().Equals("S"))
                    {
                        var existeBeneficio = new Cliente().VerificaExistenciaBeneficio(ObjConexao, valido.Codigo.ToString());

                        if (existeBeneficio)
                            valido.HabBeneficios = "S";
                        else
                            valido.HabBeneficios = "N";
                    }
                    else
                    {
                        valido.HabBeneficios = "N";
                    }
                }
                else
                    valido.HabBeneficios = "N";

                if (reader["HOR_INI_ALT_LIM"] != DBNull.Value && reader["HOR_INI_ALT_LIM"].ToString().Trim() != string.Empty)
                    valido.HorIniAltLim = reader["HOR_INI_ALT_LIM"].ToString();

                if (reader["HOR_FIM_ALT_LIM"] != DBNull.Value && reader["HOR_FIM_ALT_LIM"].ToString().Trim() != string.Empty)
                    valido.HorFimAltLim = reader["HOR_FIM_ALT_LIM"].ToString();

                if (reader["QT_MAX_ALT_LIM"] != DBNull.Value && reader["QT_MAX_ALT_LIM"].ToString().Trim() != string.Empty)
                    valido.QtMaxAltLim = reader["QT_MAX_ALT_LIM"].ToString();

                if (reader["PC_MAX_ALT_LIM"] != DBNull.Value && reader["PC_MAX_ALT_LIM"].ToString().Trim() != string.Empty)
                    valido.PcMaxAltLim = reader["PC_MAX_ALT_LIM"].ToString();

                listLogins.Add(valido);
                idAba++;
            }

            return new LoginValidation(true, retorno, acao, listLogins);
        }
    }
}
