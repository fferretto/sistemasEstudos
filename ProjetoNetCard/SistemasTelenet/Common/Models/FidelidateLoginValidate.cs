using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace NetCard.Common.Models
{
    public sealed class FidelidateLoginValidate : LoginValidateBase
    {
        public FidelidateLoginValidate(ObjConn objConexao)
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
                    Acesso = login.Acesso,
                    CodCrt = Convert.ToString(reader["CODCRT"]),
                    Cpf = Convert.ToString(reader["CPF"]),
                    Nome = Convert.ToString(reader["NOMUSU"]),
                    NumDep = Convert.ToInt16(reader["NUMDEP"]),
                    TemSenha = Convert.ToString(reader["TEMSENHA"]) == Constantes.sim,
                    Sistema = new Sistema
                    {
                        IdAba = idAba,
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
                    rotulo = rotulo.PadRight(15, ' ');
                }

                valido.Sistema.NomeAba = rotulo + "\n\n";
                valido.SistemaAcessado = valido.Sistema.IdAba;
                valido.Status = Convert.ToString(reader["STA"]);
                valido.UltAcesso = Convert.ToString(reader["ULTACESSO"]);

                if (reader["HABFIDELIDADE"] != DBNull.Value && reader["HABFIDELIDADE"].ToString().Trim() != string.Empty)
                {
                    valido.HabFidelidade = Convert.ToString(reader["HABFIDELIDADE"]) == Constantes.sim;
                    valido.AdesaoPontuacao = Convert.ToString(reader["ADERIU"]) == Constantes.sim;
                    valido.Sistema.ControllerPrincipal = "UsuTermoAdesao";
                    valido.Sistema.ActionPrincipal = valido.AdesaoPontuacao ? "Pontuacao" : "TermoAdesao";
                }

                listLogins.Add(valido);
                idAba++;
            }

            return new LoginValidation(true, retorno, acao, listLogins);
        }
    }
}
