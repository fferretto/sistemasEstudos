using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace SIL.DAL
{
    public class daSegmentosEGrupos
    {
        public daSegmentosEGrupos(OPERADORA operadora)
        {
            _operadora = operadora;
            _strConBdTelenet = string.Format(ConstantesSIL.BDTELENET, _operadora.SERVIDORNC, _operadora.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        private OPERADORA _operadora;
        private string _strConBdTelenet;

        private DbCommand CriaComando(DbConnection connection, DbTransaction transaction, int sistema, int codigoCliente, ItemAutorizacao itemAutorizacao, bool estaAutorizado)
        {
            //'I' = Inclusão
            //'E' = Exclusão
            //'A' = Alteração

            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PROC_MANU_SEG_GRUPO_AUTORIZ";

            command.Parameters.Add(new SqlParameter("SISTEMA", sistema));
            command.Parameters.Add(new SqlParameter("OPERACAO", estaAutorizado ? 'I' : 'E'));
            command.Parameters.Add(new SqlParameter("CODCLI", codigoCliente));

            command.Parameters.Add(new SqlParameter("CODSEG", DBNull.Value));
            command.Parameters.Add(new SqlParameter("CODATI", DBNull.Value));
            command.Parameters.Add(new SqlParameter("CODGRUPO", DBNull.Value));

            command.Parameters.Add(new SqlParameter("PERLIMEXC", itemAutorizacao.PercentualLimiteExclusivo ? "S" : "N"));
            command.Parameters.Add(new SqlParameter("PERLIM", itemAutorizacao.PercentualLimite));
            command.Parameters.Add(new SqlParameter("LIMRISCO", itemAutorizacao.LimiteRisco));
            command.Parameters.Add(new SqlParameter("MAXPARC", itemAutorizacao.MaximoParcelas));
            command.Parameters.Add(new SqlParameter("PERSUB", itemAutorizacao.PercentualSubsidio));
            command.Parameters.Add(new SqlParameter("PERSUBDEP", itemAutorizacao.PercentualSubsidioDependente));

            var retorno = new SqlParameter("RETORNO", SqlDbType.VarChar, 50);
            retorno.Direction = ParameterDirection.Output;
            command.Parameters.Add(retorno);

            return command;
        }

        private void ExecutarComando(DbCommand command)
        {
            command.ExecuteNonQuery();
            var mensagem = Convert.ToString(command.Parameters["RETORNO"].Value);

            if (!mensagem.Equals("OK", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApplicationException(mensagem);
            }
        }

        private static GrupoCredenciado LerGrupo(DataRow[] rows)
        {
            var grupo = new GrupoCredenciado
            {
                Codigo = Convert.ToInt32(rows[0]["CODGRUPO"]),
                LimiteRisco = Convert.ToInt16(rows[0]["LIMRISCO"]),
                Nome = Convert.ToString(rows[0]["NOMGRUPO"]),
                MaximoParcelas = Convert.ToInt16(rows[0]["MAXPARC"]),
                PercentualLimite = Convert.ToInt16(rows[0]["PERLIM"]),
                EstaAutorizado = Convert.ToChar(rows[0]["CHECKGRUPO"]) == 'S',
                PercentualLimiteExclusivo = Convert.ToChar(rows[0]["PERLIMEXC"]) == 'S',
                PercentualSubsidio = Convert.ToInt16(rows[0]["PERSUB"]),
                PercentualSubsidioDependente = Convert.ToInt16(rows[0]["PERSUBDEP"])
            };

            foreach (DataRow row in rows)
            {
                grupo.Credenciados.Add(new Credenciado { Nome = Convert.ToString(row["NOMFAN"]) });
            }

            return grupo;
        }

        private static Segmento LerSegmento(DataRow[] rows)
        {
            var segmento = new Segmento
            {
                Codigo = Convert.ToInt32(rows[0]["CODSEG"]),
                LimiteRisco = Convert.ToInt16(rows[0]["LIMRISCO"]),
                Nome = Convert.ToString(rows[0]["NOMSEG"]),
                MaximoParcelas = Convert.ToInt16(rows[0]["MAXPARC"]),
                PercentualLimite = Convert.ToInt16(rows[0]["PERLIM"]),
                PercentualSubsidio = Convert.ToInt16(rows[0]["PERSUB"]),
                PercentualSubsidioDependente = Convert.ToInt16(rows[0]["PERSUBDEP"]),
                PercentualLimiteExclusivo = Convert.ToString(rows[0]["PERLIMEXC"]) == "S" ? true : false,
            };

            var checkeds = 0;
            var ramosAtividade = new List<RamoAtividade>();

            foreach (DataRow row in rows)
            {
                if (row["CODATI"] == DBNull.Value)
                {
                    if (Convert.ToChar(row["SEGAUT"]) == 'S')
                    {
                        checkeds++;
                    }
                    continue;
                }

                var checkAti = Convert.ToChar(row["CHECKATI"]) == 'S';

                if (checkAti)
                {
                    checkeds++;
                }

                //segmento.RamosAtividade.Add(new RamoAtividade
                ramosAtividade.Add(new RamoAtividade
                {
                    Codigo = Convert.ToInt32(row["CODATI"]),
                    Nome = Convert.ToString(row["NOMATI"]),
                    EstaAutorizado = checkAti
                });
            }

            segmento.EstaAutorizado = checkeds > 0;
            segmento.RamosAtividade = ramosAtividade;

            //segmento.EstaAutorizado = checkeds == 0
            //    ? AutorizacaoSegmento.Nao
            //    : checkeds == rows.Length
            //        ? AutorizacaoSegmento.Sim
            //        : AutorizacaoSegmento.Parcialmente;

            return segmento;
        }

        private void Salvar(string mensagemSemAlteracoes, string mensagemAlterado, int totalItens, Action<SqlConnection, SqlTransaction> procedimento, out string mensagem)
        {
            if (totalItens == 0)
            {
                mensagem = mensagemSemAlteracoes;
                return;
            }

            using (var connection = new SqlConnection(_strConBdTelenet))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        procedimento(connection, transaction);
                        transaction.Commit();
                        mensagem = mensagemAlterado;
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        mensagem = exception.Message;
                    }
                }
            }
        }

        public void AlterarGrupos(int codigoCliente, int sistema, IEnumerable<GrupoCredenciado> gruposCredenciados, out string mensagem)
        {
            Salvar(
                "Nao existem grupos de credenciados para serem atualizados.",
                "Grupos de credenciados alterados com sucesso.",
                gruposCredenciados.Count(),
                (connection, transaction) => SalvarGrupos(connection, transaction, sistema, codigoCliente, gruposCredenciados),
                out mensagem);
        }

        public void AlterarSegmentos(int codigoCliente, int sistema, IEnumerable<Segmento> segmentos, out string mensagem)
        {
            Salvar(
                "Nao existem segmentos para serem atualizados.",
                "Segmentos alterados com sucesso.",
                segmentos.Count(),
                (connection, transaction) => SalvarSegmentos(connection, transaction, sistema, codigoCliente, segmentos),
                out mensagem);
        }

        public IEnumerable<GrupoCredenciado> ObterGrupos(int codigoSistema, int codigoCliente)
        {
            var grupos = new List<GrupoCredenciado>();

            using (var conexao = new SqlConnection(_strConBdTelenet))
            using (var comando = new SqlCommand("PROC_LISTA_GRUPO_CRED_CLIENTE", conexao))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@SISTEMA", codigoSistema));
                comando.Parameters.Add(new SqlParameter("@CODCLI", codigoCliente));

                conexao.Open();

                using (var reader = comando.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    var codigosGrupos = dataTable
                        .Rows
                        .OfType<DataRow>()
                        .Where(r => r["CODGRUPO"] != DBNull.Value)
                        .Select(r => Convert.ToString(r["CODGRUPO"])).Distinct();

                    foreach (var codigoGrupo in codigosGrupos)
                    {
                        grupos.Add(LerGrupo(dataTable.Select("CODGRUPO = " + codigoGrupo)));
                    }
                }
            }

            return grupos;
        }
    
        public IEnumerable<Segmento> ObterSegmentos(int codigoSistema, int codigoCliente)
        {
            var segmentos = new List<Segmento>();

            using (var conexao = new SqlConnection(_strConBdTelenet))
            using (var comando = new SqlCommand("PROC_LISTA_SEG_ATI_CLIENTE", conexao))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@SISTEMA", codigoSistema));
                comando.Parameters.Add(new SqlParameter("@CODCLI", codigoCliente));

                conexao.Open();

                using (var reader = comando.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    var codigosSegmentos = dataTable
                        .Rows
                        .OfType<DataRow>()
                        .Where(r => r["CODSEG"] != DBNull.Value)
                        .Select(r => Convert.ToString(r["CODSEG"])).Distinct();

                    foreach (var codigoSegmento in codigosSegmentos)
                    {
                        segmentos.Add(LerSegmento(dataTable.Select("CODSEG = " + codigoSegmento)));
                    }
                }
            }

            return segmentos;
        }

        public void SalvarGrupos(DbConnection connection, DbTransaction transaction, int sistema, int codigoCliente, IEnumerable<GrupoCredenciado> gruposCredenciados)
        {
            foreach (var grupoCredenciado in gruposCredenciados)
            {
                var command = CriaComando(connection, transaction, sistema, codigoCliente, grupoCredenciado, grupoCredenciado.EstaAutorizado);
                command.Parameters["CODGRUPO"].Value = grupoCredenciado.Codigo;
                ExecutarComando(command);
            }
        }

        public void SalvarSegmentos(DbConnection connection, DbTransaction transaction, int sistema, int codigoCliente, IEnumerable<Segmento> segmentos)
        {
            foreach (var segmento in segmentos)
            {
                var command = CriaComando(connection, transaction, sistema, codigoCliente, segmento, segmento.EstaAutorizado);
                command.Parameters["CODSEG"].Value = segmento.Codigo;

                if (segmento.RamosAtividade.Count == 0)
                {
                    ExecutarComando(command);
                    continue;
                }

                foreach (var ramoAtividade in segmento.RamosAtividade)
                {
                    command.Parameters["CODATI"].Value = ramoAtividade.Codigo;
                    command.Parameters["OPERACAO"].Value = ramoAtividade.EstaAutorizado ? 'I' : 'E';
                    ExecutarComando(command);
                }
            }
        }
    }
}
