using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Procedures;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Procedures;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services.Procedure
{
    public class ProceduresService : ServiceBase<PAGNET_TITULOS_PAGOS>, IProceduresService
    {
        private readonly IPagNet_Titulos_PagosRepository _rep;
        private readonly IPROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository _RepIndicadorEntradaSaida;
        private readonly IPROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository _RepIndicadorReceitaDia;
        private readonly IPROC_PAGNET_IND_PAG_ANORepository _IndPagRealizadoAno;
        private readonly IPROC_PAGNET_IND_PAG_REALIZADO_DIARepository _IndPagRealizadoDia;
        private readonly IPROC_PAGNET_CONS_TITULOS_VENCIDOSRepository _TitulosVencidos;
        private readonly IProc_PagNet_Cons_Tran_CreRepository _ProcConsFech;
        private readonly IPROC_PAGNET_CONSULTA_TITULOSRepository _procConsultaTitulos;
        private readonly IPROC_PAGNET_SOLICITACAOBOLETORepository _solicitacaoBoleto;
        private readonly IPROC_PAGNET_CONSULTABOLETORepository _ProcConsultaBoletos;
        private readonly IPROC_PAGNET_REG_CARGA_CLI_NETCARDRepository _ProcAtualizaCarga;
        private readonly IPROC_CONFIRMAPGTOCARGARepository _ConfirmaCarga;
        private readonly IPROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository _IncUsuarioNC;        
        private readonly IPROC_PAGNET_DETALHAMENTO_COBRANCARepository _detalhamentoCobranca;
        private readonly IPROC_PAGNET_BUSCA_USUARIO_NCRepository _BuscaUsuNC;
        private readonly IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository _IncluiClienteUsuNC;
        
        private readonly IPROC_PAGNET_MAIORES_RECEITASRepository _maioresReceitas;
        private readonly IPROC_PAGNET_MAIORES_DESPESASRepository _maioresDespesas;
        private readonly IPROC_PAGNET_INFO_CONTA_CORRENTERepository _infoContaCorrente;
        private readonly IPROC_PAGNET_EXTRATO_BANCARIORepository _extratoBancario;

        
        public ProceduresService(IPagNet_Titulos_PagosRepository rep,
                                IPROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository RepIndicadorEntradaSaida,
                                IPROC_PAGNET_IND_PAG_REALIZADO_DIARepository IndPagRealizadoDia,
                                IPROC_PAGNET_IND_PAG_ANORepository IndPagRealizadoAno,
                                IPROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository RepIndicadorReceitaDia,
                                IPROC_PAGNET_CONS_TITULOS_VENCIDOSRepository TitulosVencidos,
                                IProc_PagNet_Cons_Tran_CreRepository ProcConsFech,
                                IPROC_PAGNET_CONSULTABOLETORepository ProcConsultaBoletos,
                                IPROC_PAGNET_CONSULTA_TITULOSRepository procConsultaTitulos,
                                IPROC_PAGNET_SOLICITACAOBOLETORepository solicitacaoBoleto,
                                IPROC_PAGNET_REG_CARGA_CLI_NETCARDRepository ProcAtualizaCarga,
                                IPROC_CONFIRMAPGTOCARGARepository ConfirmaCarga,
                                IPROC_PAGNET_DETALHAMENTO_COBRANCARepository detalhamentoCobranca,
                                IPROC_PAGNET_MAIORES_RECEITASRepository maioresReceitas,
                                IPROC_PAGNET_MAIORES_DESPESASRepository maioresDespesas,
                                IPROC_PAGNET_INFO_CONTA_CORRENTERepository infoContaCorrente,
                                IPROC_PAGNET_EXTRATO_BANCARIORepository extratoBancario,
                                IPROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository IncUsuarioNC,
                                IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository IncluiClienteUsuNC,
                                IPROC_PAGNET_BUSCA_USUARIO_NCRepository BuscaUsuNC)
            : base(rep)
        {
            _rep = rep;
            _RepIndicadorEntradaSaida = RepIndicadorEntradaSaida;
            _RepIndicadorReceitaDia = RepIndicadorReceitaDia;
            _IndPagRealizadoDia = IndPagRealizadoDia;
            _IndPagRealizadoAno = IndPagRealizadoAno;
            _TitulosVencidos = TitulosVencidos;
            _ProcConsFech = ProcConsFech;
            _procConsultaTitulos = procConsultaTitulos;
            _solicitacaoBoleto = solicitacaoBoleto;
            _ProcConsultaBoletos = ProcConsultaBoletos;
            _ProcAtualizaCarga = ProcAtualizaCarga;
            _ConfirmaCarga = ConfirmaCarga;
            _detalhamentoCobranca = detalhamentoCobranca;
            _maioresReceitas = maioresReceitas;
            _maioresDespesas = maioresDespesas;
            _infoContaCorrente = infoContaCorrente;
            _extratoBancario = extratoBancario;
            _IncUsuarioNC = IncUsuarioNC;
            _BuscaUsuNC = BuscaUsuNC;
            _IncluiClienteUsuNC = IncluiClienteUsuNC;
        }


        public List<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO> ConsultaIndicadorEntradaSaidaAno(int codSubRede)
        {
            var dados = _RepIndicadorEntradaSaida.ConsultaIndicadorEntradaSaidaAno(codSubRede);

            return dados.ToList();
        }
        public List<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA> ConsultaIndicadorReceitaDia(DateTime dtInicio, DateTime dtFim, int codSubRede)
        {
            var dados = _RepIndicadorReceitaDia.ConsultaIndicadorReceitaDia(dtInicio, dtFim, codSubRede);

            return dados.ToList();
        }

        public List<PROC_PAGNET_IND_PAG_REALIZADO_DIA> ConsultaPagRealizadoDia(DateTime dtInicio, DateTime dtFim, int codSubRede, string codBanco, int codCre)
        {
            var dados = _IndPagRealizadoDia.ConsultaPagRealizadoDia(dtInicio, dtFim, codSubRede, codBanco, codCre);

            return dados.ToList();
        }
        public List<PROC_PAGNET_IND_PAG_ANO> ConsultaPagRealizadoAno(int codSubRede, string codBanco, int codCre)
        {
            var dados = _IndPagRealizadoAno.ConsultaPagRealizadoAno(codSubRede, codBanco, codCre);

            return dados.ToList();
        }

        public List<PROC_PAGNET_CONS_TITULOS_VENCIDOS> RetornaTitulosVencidos(int codEmpresa, DateTime dtInicio, DateTime dtFim)
        {
            try
            {
                var dados = _TitulosVencidos.ConsultaTitulosVencidos(codEmpresa, dtInicio, dtFim);

                var retorno = dados.ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PROC_PAGNET_CONS_TITULOS_BORDERO> BuscaListaTitulosEmAberto(DateTime dtInicio, DateTime dtFim, int codFavorito,int codEmpresa, int codContaCorrente, string codBanco)
        {
            try
            {
                var dados = _ProcConsFech.ConsultaTitulosEmAbertos(dtInicio, dtFim, codFavorito, codEmpresa, codContaCorrente, codBanco);

                var retorno = dados.ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PROC_PAGNET_CONSULTA_TITULOS>> ConsultaTitulos(DateTime dtInicio, DateTime dtFim, int codFavorecido, int codEmpresa, string status, int codigoTitulo)
        {
            var dados = await _procConsultaTitulos.ConsultaTitulos(dtInicio, dtFim, codFavorecido, codEmpresa, status, codigoTitulo);
            return dados.ToList();
        }

        public async Task<IEnumerable<PROC_PAGNET_SOLICITACAOBOLETO>> ConsultaEmissaoBoleto(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa)
        {
            string MSG_ERRO;
            int ERRO;

            var dados = _solicitacaoBoleto.ConsultaEmissaoBoleto(dtInicio, dtFim, codCliente, codEmpresa, out ERRO, out MSG_ERRO);

            if (ERRO > 0)
            {
                throw new Exception(MSG_ERRO);
            }

            return dados.ToList();
        }

        public async Task<IEnumerable<PROC_PAGNET_CONSULTABOLETO>> ConsultaBoletos(DateTime dtInicio, DateTime dtFim, int codCliente, int codEmpresa, string status)
        {
            string MSG_ERRO;
            int ERRO;

            var dados = _ProcConsultaBoletos.ConsultaBoletos(dtInicio, dtFim, codCliente, codEmpresa, status, out ERRO, out MSG_ERRO);

            if (ERRO > 0)
            {
                throw new Exception(MSG_ERRO);
            }

            return dados.ToList();

        
        }

        public void AtualizaSolicitacaoCarga(DateTime datRef)
        {
            string MSG_ERRO;
            int ERRO;
            _ProcAtualizaCarga.AtualizaSolicitacaoCarga(datRef, out ERRO, out MSG_ERRO);


            if (ERRO > 0)
            {
                throw new Exception(MSG_ERRO);
            }

        }

        public void RegistraPagamentoCargaNetCard(int codigoCliente, int NumeroCarga)
        {
            _ConfirmaCarga.RegistraPagamentoCargaNetCard(codigoCliente, NumeroCarga);
        }
        public void IncluiUsuarioNC(string CPF, int sistema, int codEmpresa)
        {
            _IncUsuarioNC.IncluiUsuarioNC(CPF, sistema, codEmpresa);
        }

        public async Task<IEnumerable<PROC_PAGNET_DETALHAMENTO_COBRANCA>> DetalhamentoCobranca(int codEmissaoFaturamento)
        {
            var dados =  _detalhamentoCobranca.DetalhamentoCobranca(codEmissaoFaturamento);

            return dados;
        }

        public async Task<IEnumerable<PROC_PAGNET_MAIORES_RECEITAS>> Listar_MAIORES_RECEITAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var dados = await _maioresReceitas.Listar_MAIORES_RECEITAS(codEmpresa, codContaCorrente, dataInicio, dataFim);

            return dados;
        }

        public async Task<IEnumerable<PROC_PAGNET_MAIORES_DESPESAS>> Listar_MAIORES_DESPESAS(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var dados = await _maioresDespesas.Listar_MAIORES_DESPESAS(codEmpresa, codContaCorrente, dataInicio, dataFim);

            return dados;
        }

        public async Task<PROC_PAGNET_INFO_CONTA_CORRENTE> Consultar_INFO_CONTA_CORRENTE(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var dados = await _infoContaCorrente.Consultar_INFO_CONTA_CORRENTE(codEmpresa, codContaCorrente, dataInicio, dataFim);

            return dados;
        }

        public async Task<IEnumerable<PROC_PAGNET_EXTRATO_BANCARIO>> Listar_EXTRATO_BANCARIO(int codEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var dados = await _extratoBancario.Listar_EXTRATO_BANCARIO(codEmpresa, codContaCorrente, dataInicio, dataFim);

            return dados;
        }

        public async Task<PROC_PAGNET_BUSCA_USUARIO_NC> BuscaDadosUsuarioNC(string Matricula, int Sistema, int codCliente)
        {
            var dados = await _BuscaUsuNC.BuscaDadosUsuarioNC(Matricula, Sistema, codCliente);

            return dados;
        }

        public async Task<PROC_PAGNET_INC_CLIENTE_USUARIO_NC> IncluiClienteUsuarioNC(string CPF, int Sistema, int codEmpresa, int codUsuario)
        {
            var dadosRet = await _IncluiClienteUsuNC.IncluiClienteUsuarioNC(CPF, Sistema, codEmpresa, codUsuario);

            return dadosRet;
        }
    }
}
