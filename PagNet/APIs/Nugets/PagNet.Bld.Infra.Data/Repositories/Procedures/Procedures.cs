﻿using Microsoft.EntityFrameworkCore;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Procedures;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagNet.Bld.Infra.Data.Repositories.Procedures
{
    public class PROC_PAGNET_CONS_TRAN_CRERepository : RepositoryBase<PROC_PAGNET_CONS_TITULOS_BORDERO>, IPROC_PAGNET_CONS_TRAN_CRERepository
    {
        public PROC_PAGNET_CONS_TRAN_CRERepository(ContextPagNet context)
            : base(context)
        { }


        public IEnumerable<PROC_PAGNET_CONS_TITULOS_BORDERO> ConsultaTitulosEmAbertos(DateTime dtInicio, DateTime dtFim, int codFavorito, int codEmpresa, int codContaCorrente, string codBanco)
        {
            var dados = DbNetCard.PROC_PAGNET_CONS_TITULOS_BORDERO.FromSql($"PROC_PAGNET_CONS_TITULOS_BORDERO {dtInicio}, {dtFim}, {codFavorito}, {codEmpresa}, {codContaCorrente}, {codBanco}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_CONS_TITULOS_VENCIDOSRepository : RepositoryBase<PROC_PAGNET_CONS_TITULOS_VENCIDOS>, IPROC_PAGNET_CONS_TITULOS_VENCIDOSRepository
    {
        public PROC_PAGNET_CONS_TITULOS_VENCIDOSRepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_CONS_TITULOS_VENCIDOS> ConsultaTitulosVencidos(int codEmpresa, DateTime dtInicio, DateTime dtFim)
        {
            var dados = DbNetCard.PROC_PAGNET_CONS_TITULOS_VENCIDOS.FromSql($"PROC_PAGNET_CONS_TITULOS_VENCIDOS  {codEmpresa}, {dtInicio}, {dtFim}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_INDICADOR_PAGAMENTO_DIARepository : RepositoryBase<PROC_PAGNET_IND_PAG_PREVISTO_DIA>, IPROC_PAGNET_INDICADOR_PAGAMENTO_DIARepository
    {
        public PROC_PAGNET_INDICADOR_PAGAMENTO_DIARepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_IND_PAG_PREVISTO_DIA> ConsultaValorPagoDia(DateTime dtInicio, DateTime dtFim, int codSubRede)
        {
            var dados = DbNetCard.PROC_PAGNET_IND_PAG_PREVISTO_DIA.FromSql($"PROC_PAGNET_IND_PAG_PREVISTO_DIA {dtInicio}, {dtFim}, {codSubRede}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_IND_PAG_REALIZADO_DIARepository : RepositoryBase<PROC_PAGNET_IND_PAG_REALIZADO_DIA>, IPROC_PAGNET_IND_PAG_REALIZADO_DIARepository
    {
        public PROC_PAGNET_IND_PAG_REALIZADO_DIARepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_IND_PAG_REALIZADO_DIA> ConsultaPagRealizadoDia(DateTime dtInicio, DateTime dtFim, int codSubRede, string codBanco, int codCre)
        {
            var dados = DbNetCard.PROC_PAGNET_IND_PAG_REALIZADO_DIA.FromSql($"PROC_PAGNET_IND_PAG_REALIZADO_DIA {dtInicio}, {dtFim}, {codSubRede}, {codBanco}, {codCre}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_IND_PAG_ANORepository : RepositoryBase<PROC_PAGNET_IND_PAG_ANO>, IPROC_PAGNET_IND_PAG_ANORepository
    {
        public PROC_PAGNET_IND_PAG_ANORepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_IND_PAG_ANO> ConsultaPagRealizadoAno(int codSubRede, string codBanco, int codCre)
        {
            var dados = DbNetCard.PROC_PAGNET_IND_PAG_ANO.FromSql($"PROC_PAGNET_IND_PAG_ANO {codSubRede}, {codBanco}, {codCre}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository : RepositoryBase<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO>, IPROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository
    {
        public PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO> ConsultaIndicadorEntradaSaidaAno(int codSubRede)
        {
            var dados = DbNetCard.PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO.FromSql($"PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO {codSubRede}");

            return dados;
        }
    }

    public class PROC_PAGNET_CONSULTA_TITULOSRepository : RepositoryBase<PROC_PAGNET_CONSULTA_TITULOS>, IPROC_PAGNET_CONSULTA_TITULOSRepository
    {
        public PROC_PAGNET_CONSULTA_TITULOSRepository(ContextPagNet context)
            : base(context)
        { }

        public async Task<IEnumerable<PROC_PAGNET_CONSULTA_TITULOS>> ConsultaTitulos(DateTime dtInicio, DateTime dtFim, int codFavorecido, int codEmpresa, string status, int codigoTitulo)
        {
            var dados = DbNetCard.PROC_PAGNET_CONSULTA_TITULOS.FromSql($"PROC_PAGNET_CONSULTA_TITULOS {dtInicio}, {dtFim}, {codFavorecido}, {codEmpresa}, {status}, {codigoTitulo}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository : RepositoryBase<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA>, IPROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository
    {
        public PROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA> ConsultaIndicadorReceitaDia(DateTime dtInicio, DateTime dtFim, int codSubRede)
        {
            var dados = DbNetCard.PROC_PAGNET_INDI_RECEB_PREVISTA_DIA.FromSql($"PROC_PAGNET_INDI_RECEB_PREVISTA_DIA {dtInicio}, {dtFim}, {codSubRede}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_CONSCARGA_PGTORepository : RepositoryBase<PROC_PAGNET_CONSCARGA_PGTO>, IPROC_PAGNET_CONSCARGA_PGTORepository
    {
        public PROC_PAGNET_CONSCARGA_PGTORepository(ContextPagNet context)
            : base(context)
        { }

        public async Task<IEnumerable<PROC_PAGNET_CONSCARGA_PGTO>> ConsultaCargaPendenteBoleto(int codCli, int codSubRede, DateTime dtInicio, DateTime dtFim, int numCarga)
        {
            var dados = DbNetCard.PROC_PAGNET_CONSCARGA_PGTO.FromSql($"PROC_PAGNET_CONSCARGA_PGTO {codCli}, {codSubRede}, {dtInicio}, {dtFim}, {numCarga}").ToList();

            return dados;
        }
    }
    public class PROC_PAGNET_CONS_BORDERORepository : RepositoryBase<PROC_PAGNET_CONS_BORDERO>, IPROC_PAGNET_CONS_BORDERORepository
    {
        public PROC_PAGNET_CONS_BORDERORepository(ContextPagNet context)
            : base(context)
        { }

        public async Task<IEnumerable<PROC_PAGNET_CONS_BORDERO>> ConsultaBorderoPagamento(int codEmpresa, int codBordero, int codContaCorrente, string Status)
        {
            var dados = DbNetCard.PROC_PAGNET_CONS_BORDERO.FromSql($"PROC_PAGNET_CONS_BORDERO {codEmpresa}, {codBordero}, {codContaCorrente}, {Status}").ToList();

            return dados;
        }
    }
    public class MW_CONSCEPRepository : RepositoryBase<MW_CONSCEP>, IMW_CONSCEPRepository
    {
        public MW_CONSCEPRepository(ContextConcentrador context)
            : base(context)
        { }

        public async Task<MW_CONSCEP> ConsultaEndereco(string CEP)
        {
            var dados = await DbConcentrador.MW_CONSCEP.FromSql($"MW_CONSCEP {CEP}").FirstOrDefaultAsync();

            return dados;
        }
    }

    public class PROC_PAGNET_SOLICITACAOBOLETORepository : RepositoryBase<PROC_PAGNET_SOLICITACAOBOLETO>, IPROC_PAGNET_SOLICITACAOBOLETORepository
    {
        public PROC_PAGNET_SOLICITACAOBOLETORepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_SOLICITACAOBOLETO> ConsultaEmissaoBoleto(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, out int ERRO, out string MSG_ERRO)
        {


            var datIni = new SqlParameter
            {
                ParameterName = "dtInicio",
                DbType = System.Data.DbType.DateTime,
                Value = dtInicio,
                Direction = System.Data.ParameterDirection.Input
            };
            var datFin = new SqlParameter
            {
                ParameterName = "dtFim",
                DbType = System.Data.DbType.DateTime,
                Value = dtFim,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigocliente = new SqlParameter
            {
                ParameterName = "codCliente",
                DbType = System.Data.DbType.Int32,
                Value = codCliente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoempresa = new SqlParameter
            {
                ParameterName = "codEmpresa",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };
            var CODERRO = new SqlParameter
            {
                ParameterName = "ERRO",
                DbType = System.Data.DbType.Int32,
                Direction = System.Data.ParameterDirection.Output
            };
            var MSGERRO = new SqlParameter
            {
                ParameterName = "MSG_ERRO",
                DbType = System.Data.DbType.String,
                Size = 500,
                Direction = System.Data.ParameterDirection.Output
            };

            var query = "EXEC PROC_PAGNET_SOLICITACAOBOLETO @dtInicio, @dtFim, @codCliente, @codEmpresa, @ERRO OUT, @MSG_ERRO OUT";
            var result = DbNetCard.PROC_PAGNET_SOLICITACAOBOLETO.FromSql(query, datIni, datFin, codigocliente, codigoempresa, CODERRO, MSGERRO).ToList();


            ERRO = Convert.ToInt32(CODERRO.Value);
            MSG_ERRO = Convert.ToString(MSGERRO.Value);

            return result.ToList();

        }
    }

    public class PROC_PAGNET_CONSULTABOLETORepository : RepositoryBase<PROC_PAGNET_CONSULTABOLETO>, IPROC_PAGNET_CONSULTABOLETORepository
    {
        public PROC_PAGNET_CONSULTABOLETORepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_CONSULTABOLETO> ConsultaBoletos(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, string status, out int ERRO, out string MSG_ERRO)
        {
            var datIni = new SqlParameter
            {
                ParameterName = "dtInicio",
                DbType = System.Data.DbType.DateTime,
                Value = dtInicio,
                Direction = System.Data.ParameterDirection.Input
            };
            var datFin = new SqlParameter
            {
                ParameterName = "dtFim",
                DbType = System.Data.DbType.DateTime,
                Value = dtFim,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigocliente = new SqlParameter
            {
                ParameterName = "codCliente",
                DbType = System.Data.DbType.Int32,
                Value = codCliente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoempresa = new SqlParameter
            {
                ParameterName = "codEmpresa",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };
            var StatusBusca = new SqlParameter
            {
                ParameterName = "String",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Size = 100,
                Direction = System.Data.ParameterDirection.Input
            };
            var CODERRO = new SqlParameter
            {
                ParameterName = "ERRO",
                DbType = System.Data.DbType.Int32,
                Direction = System.Data.ParameterDirection.Output
            };
            var MSGERRO = new SqlParameter
            {
                ParameterName = "MSG_ERRO",
                DbType = System.Data.DbType.String,
                Size = 500,
                Direction = System.Data.ParameterDirection.Output
            };

            var query = "EXEC PROC_PAGNET_CONSULTABOLETO @dtInicio, @dtFim, @codCliente, @codEmpresa, @status, @ERRO OUT, @MSG_ERRO OUT";
            var result = DbNetCard.PROC_PAGNET_CONSULTABOLETO.FromSql(query, datIni, datFin, codigocliente, codigoempresa, StatusBusca, CODERRO, MSGERRO).ToList();


            ERRO = Convert.ToInt32(CODERRO.Value);
            MSG_ERRO = Convert.ToString(MSGERRO.Value);

            return result.ToList();

        }
    }

    public class PROC_PAGNET_REG_CARGA_CLI_NETCARDRepository : RepositoryBase<PROC_PAGNET_REG_CARGA_CLI_NETCARD>, IPROC_PAGNET_REG_CARGA_CLI_NETCARDRepository
    {
        public PROC_PAGNET_REG_CARGA_CLI_NETCARDRepository(ContextPagNet context)
            : base(context)
        { }

        public void AtualizaSolicitacaoCarga(DateTime datRef, out int ERRO, out string MSG_ERRO)
        {
            var dataReferencia = new SqlParameter
            {
                ParameterName = "datRef",
                DbType = System.Data.DbType.DateTime,
                Value = datRef,
                Direction = System.Data.ParameterDirection.Input
            };

            var CODERRO = new SqlParameter
            {
                ParameterName = "ERRO",
                DbType = System.Data.DbType.Int32,
                Direction = System.Data.ParameterDirection.Output
            };
            var MSGERRO = new SqlParameter
            {
                ParameterName = "MSG_ERRO",
                DbType = System.Data.DbType.String,
                Size = 500,
                Direction = System.Data.ParameterDirection.Output
            };

            var query = "EXEC PROC_PAGNET_REG_CARGA_CLI_NETCARD @DATREF, @ERRO OUT, @MSG_ERRO OUT";
            DbNetCard.Database.ExecuteSqlCommand(query, dataReferencia, CODERRO, MSGERRO);


            ERRO = Convert.ToInt32(CODERRO.Value);
            MSG_ERRO = Convert.ToString(MSGERRO.Value);

        }
    }
    public class PROC_CONFIRMAPGTOCARGARepository : RepositoryBase<PROC_CONFIRMAPGTOCARGA>, IPROC_CONFIRMAPGTOCARGARepository
    {
        public PROC_CONFIRMAPGTOCARGARepository(ContextPagNet context)
            : base(context)
        { }
        public void RegistraPagamentoCargaNetCard(int codigoCliente, int NumeroCarga)
        {
            var dados = DbNetCard.PROC_CONFIRMAPGTOCARGA.FromSql($"PROC_CONFIRMAPGTOCARGA {codigoCliente}, {NumeroCarga}, NULL, PagNet").FirstOrDefault();

        }
    }
    public class PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository : RepositoryBase<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC>, IPROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository
    {
        public PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository(ContextPagNet context)
            : base(context)
        { }

        public void IncluiUsuarioNC(string CPF, int sistema, int codEmpresa)
        {
            var dados = DbNetCard.PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC.FromSql($"PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC {CPF}, {sistema}, {codEmpresa}").FirstOrDefault();
        }
    }

    public class PROC_PAGNET_DETALHAMENTO_COBRANCARepository : RepositoryBase<PROC_PAGNET_DETALHAMENTO_COBRANCA>, IPROC_PAGNET_DETALHAMENTO_COBRANCARepository
    {
        public PROC_PAGNET_DETALHAMENTO_COBRANCARepository(ContextPagNet context)
            : base(context)
        { }

        public IEnumerable<PROC_PAGNET_DETALHAMENTO_COBRANCA> DetalhamentoCobranca(int codEmissaoFaturamento)
        {
            var codigo = new SqlParameter
            {
                ParameterName = "codEmissaoFaturamento",
                DbType = System.Data.DbType.Int32,
                Value = codEmissaoFaturamento,
                Direction = System.Data.ParameterDirection.Input
            };


            var query = "EXEC PROC_PAGNET_DETALHAMENTO_COBRANCA @CodEmissaoFaturamento";
            var result = DbNetCard.PROC_PAGNET_DETALHAMENTO_COBRANCA.FromSql(query, codigo).ToList();


            return result.ToList();

        }
    }
    public class PROC_PAGNET_MAIORES_RECEITASRepository : RepositoryBase<PROC_PAGNET_MAIORES_RECEITAS>, IPROC_PAGNET_MAIORES_RECEITASRepository
    {
        public PROC_PAGNET_MAIORES_RECEITASRepository(ContextPagNet context)
            : base(context)
        { }


        public async Task<IEnumerable<PROC_PAGNET_MAIORES_RECEITAS>> Listar_MAIORES_RECEITAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var datIni = new SqlParameter
            {
                ParameterName = "DTINICIO",
                DbType = System.Data.DbType.DateTime,
                Value = dataInicio,
                Direction = System.Data.ParameterDirection.Input
            };
            var datFin = new SqlParameter
            {
                ParameterName = "DTFIM",
                DbType = System.Data.DbType.DateTime,
                Value = dataFim,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoContaCorrente = new SqlParameter
            {
                ParameterName = "CODCONTACORRENTE",
                DbType = System.Data.DbType.Int32,
                Value = codContaCorrente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoEmpresa = new SqlParameter
            {
                ParameterName = "CODEMPRESA",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_MAIORES_RECEITAS @DTINICIO, @DTFIM, @CODCONTACORRENTE, @CODEMPRESA";
            var result = DbNetCard.PROC_PAGNET_MAIORES_RECEITAS.FromSql(query, datIni, datFin, codigoContaCorrente, codigoEmpresa).ToList();


            return result.ToList();
        }
    }
    public class PROC_PAGNET_MAIORES_DESPESASRepository : RepositoryBase<PROC_PAGNET_MAIORES_DESPESAS>, IPROC_PAGNET_MAIORES_DESPESASRepository
    {
        public PROC_PAGNET_MAIORES_DESPESASRepository(ContextPagNet context)
            : base(context)
        { }

        public async Task<IEnumerable<PROC_PAGNET_MAIORES_DESPESAS>> Listar_MAIORES_DESPESAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var datIni = new SqlParameter
            {
                ParameterName = "DTINICIO",
                DbType = System.Data.DbType.DateTime,
                Value = dataInicio,
                Direction = System.Data.ParameterDirection.Input
            };
            var datFin = new SqlParameter
            {
                ParameterName = "DTFIM",
                DbType = System.Data.DbType.DateTime,
                Value = dataFim,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoContaCorrente = new SqlParameter
            {
                ParameterName = "CODCONTACORRENTE",
                DbType = System.Data.DbType.Int32,
                Value = codContaCorrente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoEmpresa = new SqlParameter
            {
                ParameterName = "CODEMPRESA",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_MAIORES_DESPESAS @DTINICIO, @DTFIM, @CODCONTACORRENTE, @CODEMPRESA";
            var result = DbNetCard.PROC_PAGNET_MAIORES_DESPESAS.FromSql(query, datIni, datFin, codigoContaCorrente, codigoEmpresa).ToList();


            return result.ToList();
        }
    }
    public class PROC_PAGNET_INFO_CONTA_CORRENTERepository : RepositoryBase<PROC_PAGNET_INFO_CONTA_CORRENTE>, IPROC_PAGNET_INFO_CONTA_CORRENTERepository
    {
        public PROC_PAGNET_INFO_CONTA_CORRENTERepository(ContextPagNet context)
            : base(context)
        { }


        public async Task<PROC_PAGNET_INFO_CONTA_CORRENTE> Consultar_INFO_CONTA_CORRENTE(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var datIni = new SqlParameter
            {
                ParameterName = "DTINICIO",
                DbType = System.Data.DbType.DateTime,
                Value = dataInicio,
                Direction = System.Data.ParameterDirection.Input
            };
            var datFin = new SqlParameter
            {
                ParameterName = "DTFIM",
                DbType = System.Data.DbType.DateTime,
                Value = dataFim,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoContaCorrente = new SqlParameter
            {
                ParameterName = "CODCONTACORRENTE",
                DbType = System.Data.DbType.Int32,
                Value = codContaCorrente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoEmpresa = new SqlParameter
            {
                ParameterName = "CODEMPRESA",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_INFO_CONTA_CORRENTE @DTINICIO, @DTFIM, @CODCONTACORRENTE, @CODEMPRESA";
            var result = DbNetCard.PROC_PAGNET_INFO_CONTA_CORRENTE.FromSql(query, datIni, datFin, codigoContaCorrente, codigoEmpresa).FirstOrDefault();


            return result;
        }
    }
    public class PROC_PAGNET_EXTRATO_BANCARIORepository : RepositoryBase<PROC_PAGNET_EXTRATO_BANCARIO>, IPROC_PAGNET_EXTRATO_BANCARIORepository
    {
        public PROC_PAGNET_EXTRATO_BANCARIORepository(ContextPagNet context)
            : base(context)
        { }


        public async Task<IEnumerable<PROC_PAGNET_EXTRATO_BANCARIO>> Listar_EXTRATO_BANCARIO(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var datIni = new SqlParameter
            {
                ParameterName = "DTINICIO",
                DbType = System.Data.DbType.DateTime,
                Value = dataInicio,
                Direction = System.Data.ParameterDirection.Input
            };
            var datFin = new SqlParameter
            {
                ParameterName = "DTFIM",
                DbType = System.Data.DbType.DateTime,
                Value = dataFim,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoContaCorrente = new SqlParameter
            {
                ParameterName = "CODCONTACORRENTE",
                DbType = System.Data.DbType.Int32,
                Value = codContaCorrente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoEmpresa = new SqlParameter
            {
                ParameterName = "CODEMPRESA",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_EXTRATO_BANCARIO @DTINICIO, @DTFIM, @CODCONTACORRENTE, @CODEMPRESA";
            var result = DbNetCard.PROC_PAGNET_EXTRATO_BANCARIO.FromSql(query, datIni, datFin, codigoContaCorrente, codigoEmpresa).ToList();


            return result.ToList();
        }
    }
    public class PROC_PAGNET_BUSCA_USUARIO_NCRepository : RepositoryBase<PROC_PAGNET_BUSCA_USUARIO_NC>, IPROC_PAGNET_BUSCA_USUARIO_NCRepository
    {
        public PROC_PAGNET_BUSCA_USUARIO_NCRepository(ContextPagNet context)
            : base(context)
        { }


        public async Task<PROC_PAGNET_BUSCA_USUARIO_NC> BuscaDadosUsuarioNC(string Matricula, int Sistema, int codCliente)
        {
            var Param_Matricula = new SqlParameter
            {
                ParameterName = "Matricula",
                DbType = System.Data.DbType.String,
                Value = Matricula,
                Direction = System.Data.ParameterDirection.Input
            };
            var Param_Sistema = new SqlParameter
            {
                ParameterName = "Sistema",
                DbType = System.Data.DbType.Int32,
                Value = Sistema,
                Direction = System.Data.ParameterDirection.Input
            };
            var Param_codCliente = new SqlParameter
            {
                ParameterName = "codCliente",
                DbType = System.Data.DbType.Int32,
                Value = codCliente,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_BUSCA_USUARIO_NC @MATRICULA, @SISTEMA, @CODCLIENTE";
            var result = DbNetCard.PROC_PAGNET_BUSCA_USUARIO_NC.FromSql(query, Param_Matricula, Param_Sistema, Param_codCliente).FirstOrDefault();


            return result;
        }
    }
    public class PROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository : RepositoryBase<PROC_PAGNET_INC_CLIENTE_USUARIO_NC>, IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository
    {
        public PROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository(ContextPagNet context)
            : base(context)
        { }


        public async Task<PROC_PAGNET_INC_CLIENTE_USUARIO_NC> IncluiClienteUsuarioNC(string CPF, int Sistema, int codEmpresa, int codUsuario)
        {
            var Param_CPF = new SqlParameter
            {
                ParameterName = "CPF",
                DbType = System.Data.DbType.String,
                Value = CPF,
                Direction = System.Data.ParameterDirection.Input
            };
            var Param_Sistema = new SqlParameter
            {
                ParameterName = "Sistema",
                DbType = System.Data.DbType.Int32,
                Value = Sistema,
                Direction = System.Data.ParameterDirection.Input
            };
            var Param_codEmpresa = new SqlParameter
            {
                ParameterName = "CODEMPRESA",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };
            var Param_codUsuario = new SqlParameter
            {
                ParameterName = "CODIGOUSUARIO_PN",
                DbType = System.Data.DbType.Int32,
                Value = codUsuario,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_INC_CLIENTE_USUARIO_NC @CPF, @SISTEMA, @CODEMPRESA, @CODIGOUSUARIO_PN";
            var result = DbNetCard.PROC_PAGNET_INC_CLIENTE_USUARIO_NC.FromSql(query, Param_CPF, Param_Sistema, Param_codEmpresa, Param_codUsuario).FirstOrDefault();


            return result;
        }
    }
    public class PROC_BAIXA_LOTE_CPFRepository : RepositoryBase<PROC_BAIXA_LOTE_CPF>, IPROC_BAIXA_LOTE_CPFRepository
    {
        public PROC_BAIXA_LOTE_CPFRepository(ContextPagNet context)
            : base(context)
        { }

        public PROC_BAIXA_LOTE_CPF ExecutarProcedure(int codigoCliente, int Lote, string cpf, string ResultSet,
            out string RetOut, out string MensOut, out int CodRetOut)
        {

            var CODCLI = new SqlParameter
            {
                ParameterName = "CODCLI",
                DbType = System.Data.DbType.Int32,
                Value = codigoCliente,
                Direction = System.Data.ParameterDirection.Input
            };
            var NUMFECCLI = new SqlParameter
            {
                ParameterName = "NUMFECCLI",
                DbType = System.Data.DbType.Int32,
                Value = Lote,
                Direction = System.Data.ParameterDirection.Input
            };
            var CPF = new SqlParameter
            {
                ParameterName = "CPF",
                DbType = System.Data.DbType.String,
                Value = cpf,
                Direction = System.Data.ParameterDirection.Input
            };
            var RESULTSET = new SqlParameter
            {
                ParameterName = "RESULTSET",
                DbType = System.Data.DbType.String,
                Value = ResultSet,
                Direction = System.Data.ParameterDirection.Input
            };
            var RETOUT = new SqlParameter
            {
                ParameterName = "RETOUT",
                DbType = System.Data.DbType.String,
                Direction = System.Data.ParameterDirection.Output
            };
            var MENSOUT = new SqlParameter
            {
                ParameterName = "MENSOUT",
                DbType = System.Data.DbType.String,
                Size = 500,
                Direction = System.Data.ParameterDirection.Output
            };
            var CODRETOUT = new SqlParameter
            {
                ParameterName = "CODRETOUT",
                DbType = System.Data.DbType.String,
                Size = 500,
                Direction = System.Data.ParameterDirection.Output
            };

            var query = "EXEC PROC_BAIXA_LOTE_CPF @CODCLI, @NUMFECCLI, @CPF, @RESULTSET, @RETOUT OUT, @MENSOUT OUT, @CODRETOUT OUT";
            var result = DbNetCard.PROC_BAIXA_LOTE_CPF.FromSql(query, CODCLI, NUMFECCLI, CPF, RESULTSET, RETOUT, MENSOUT, CODRETOUT).FirstOrDefault();


            RetOut = Convert.ToString(RETOUT.Value);
            MensOut = Convert.ToString(MENSOUT.Value);
            CodRetOut = Convert.ToInt32(CODRETOUT.Value);

            return result;

        }
    }
    public class PROC_PAGNET_LIBERA_DESCONTOFOLHARepository : RepositoryBase<PROC_PAGNET_LIBERA_DESCONTOFOLHA>, IPROC_PAGNET_LIBERA_DESCONTOFOLHARepository
    {
        public PROC_PAGNET_LIBERA_DESCONTOFOLHARepository(ContextPagNet context)
            : base(context)
        { }

        public PROC_PAGNET_LIBERA_DESCONTOFOLHA ExecutarDescontoFolha(int codCliente, int codFatura, int numeroLote, string listaCPF, int codUsuarioPN)
        {
            var codigoCliente = new SqlParameter
            {
                ParameterName = "codigoCliente",
                DbType = System.Data.DbType.Int32,
                Value = codCliente,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoFatura = new SqlParameter
            {
                ParameterName = "codigoFatura",
                DbType = System.Data.DbType.Int32,
                Value = codFatura,
                Direction = System.Data.ParameterDirection.Input
            };
            var nroLote = new SqlParameter
            {
                ParameterName = "nroLote",
                DbType = System.Data.DbType.Int32,
                Value = numeroLote,
                Direction = System.Data.ParameterDirection.Input
            };
            var CPFs = new SqlParameter
            {
                ParameterName = "CPFs",
                DbType = System.Data.DbType.String,
                Value = listaCPF,
                Direction = System.Data.ParameterDirection.Input
            };
            var codigoUsuario = new SqlParameter
            {
                ParameterName = "codigoUsuario",
                DbType = System.Data.DbType.Int32,
                Value = codUsuarioPN,
                Direction = System.Data.ParameterDirection.Input
            };

            var query = "EXEC PROC_PAGNET_LIBERA_DESCONTOFOLHA @codigoCliente, @codigoFatura, @nroLote, @CPFs, @codigoUsuario";
            var result = DbNetCard.PROC_PAGNET_LIBERA_DESCONTOFOLHA.FromSql(query, codigoCliente, codigoFatura, nroLote, CPFs, codigoUsuario).FirstOrDefault();

            return result;

        }
        
    }

    public class PROC_PAGNET_CONS_FATURAS_BORDERORepository : RepositoryBase<PROC_PAGNET_CONS_FATURAS_BORDERO>, IPROC_PAGNET_CONS_FATURAS_BORDERORepository
    {
        public PROC_PAGNET_CONS_FATURAS_BORDERORepository(ContextPagNet context)
            : base(context)
        { }

        public List<PROC_PAGNET_CONS_FATURAS_BORDERO> ExecutarConsFaturaBordero(DateTime dtInicio, DateTime dtFim, int codigoCliente, int codEmpresa, int codContaCorrente)
        {

            var CODCLI = new SqlParameter
            {
                ParameterName = "codCliente",
                DbType = System.Data.DbType.Int32,
                Value = codigoCliente,
                Direction = System.Data.ParameterDirection.Input
            };
            var DTINICIAL = new SqlParameter
            {
                ParameterName = "dtInicio",
                DbType = System.Data.DbType.String,
                Value = dtInicio.ToString("yyyyMMdd"),
                Direction = System.Data.ParameterDirection.Input
            };
            var DTFINAL = new SqlParameter
            {
                ParameterName = "dtFim",
                DbType = System.Data.DbType.String,
                Value = dtFim.ToString("yyyyMMdd"),
                Direction = System.Data.ParameterDirection.Input
            };
            var CODIGOEMPRESA = new SqlParameter
            {
                ParameterName = "codEmpresa",
                DbType = System.Data.DbType.Int32,
                Value = codEmpresa,
                Direction = System.Data.ParameterDirection.Input
            };
            var CODIGOCONTACORRENTE = new SqlParameter
            {
                ParameterName = "codContaCorrente",
                DbType = System.Data.DbType.Int32,
                Value = codContaCorrente,
                Direction = System.Data.ParameterDirection.Input
            };
            

            var query = "EXEC PROC_PAGNET_CONS_FATURAS_BORDERO @dtInicio, @dtFim, @codCliente, @codEmpresa, @codContaCorrente";
            var result = DbNetCard.PROC_PAGNET_CONS_FATURAS_BORDERO.FromSql(query, DTINICIAL, DTFINAL, CODCLI, CODIGOEMPRESA, CODIGOCONTACORRENTE).ToList();

            
            return result;

        }
    }
}
