using PagNet.Application.Interface;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PagNet.Application.Application
{
    public class DiversosApp : IDiversosApp
    {
        private readonly ISubRedeService _subRede;
        private readonly IOperadoraService _ope;
        private readonly IPagNet_CadEmpresaService _empresa;
        private readonly IPagNet_InstrucaoCobrancaService _instrucaoCobranca;
        private readonly IPagNet_CodigoOcorrenciaService _tipoOcorrenciaboleto;
        private readonly IPagNet_Formas_FaturamentoService _formaLiquidacao;
        private readonly IPagNet_CadPlanoContasService _PlanoContas;

        public DiversosApp(IOperadoraService ope,
                           IPagNet_CadEmpresaService empresa,
                           IPagNet_InstrucaoCobrancaService instrucaoCobranca,
                           IPagNet_CodigoOcorrenciaService tipoOcorrenciaboleto,
                           IPagNet_Formas_FaturamentoService formaLiquidacao,
                           IPagNet_CadPlanoContasService PlanoContas,
                           ISubRedeService subRede)
        {
            _instrucaoCobranca = instrucaoCobranca;
            _subRede = subRede;
            _ope = ope;
            _empresa = empresa;
            _tipoOcorrenciaboleto = tipoOcorrenciaboleto;
            _formaLiquidacao = formaLiquidacao;
            _PlanoContas = PlanoContas;
        }

        public object[][] GetOperadoras()
        {
            var lista = _ope.GetHashOperadora().ToList();
            lista.Insert(0, new object[] { "0", " " });

            return lista.ToArray();
        }
        public object[][] GetTiposOcorrenciasBoleto()
        {
            var lista = _tipoOcorrenciaboleto.GetTiposOcorrencias().ToList();

            return lista.ToArray();
        }
        public object[][] GetSubRede()
        {
            var lista = _subRede.GetSubRede().ToList();
            lista.Insert(0, new object[] { "0", " " });

            return lista.ToArray();
        }
        public object[][] BuscaSubRedeByID(int id)
        {
            var lista = _subRede.BuscaSubRedeByID(id).ToList();
            lista.Insert(0, new object[] { "0", " " });

            return lista.ToArray();
        }

        public object[][] GetInstrucaoCobranca()
        {
            var lista = _instrucaoCobranca.GetHashInstrucaoCobranca().ToList();
            //lista.Insert(0, new object[] { "0", " " });

            return lista.ToArray();
        }
        public string GetCaminhoArquivoPadrao(int codOpe, int codEmpresa)
        {
            var caminhoPadrao = _ope.GetOperadoraById(codOpe).Result;

            return Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, codEmpresa.ToString());
        }
        public string GetCaminhoArquivoPadrao(int codOpe)
        {
            var caminhoPadrao = _ope.GetOperadoraById(codOpe).Result;

            return Path.Combine(caminhoPadrao.CAMINHOARQUIVO);
        }
        public object[][] StatusTransacao()
        {
            var lista = new object[][]
             {
                new object[] { "TODOS", "TODOS"},
                new object[] { "EM_ABERTO", "EM ABERTO"},
                new object[] { "EM_BORDERO", "TÍTULOS EM BORDERÔ"},
                new object[] { "EM_BORDERO", "EM BORDERO" },
                new object[] { "BAIXADO", "BAIXADO"},
                new object[] { "BAIXADO_MANUALMENTE", "BAIXADO MANUALMENTE" }
             };

            return lista.ToArray();
        }

        public string GetnmSubRedeByID(int codSubRede)
        {
            var dados = _subRede.GetSubRedeByID(codSubRede).Result;

            return dados.NOMSUBREDE;
        }

        public object[][] GetEmpresa(int codEmpresa)
        {
            if (codEmpresa == 0)
            {
                var lista = _empresa.GetEmpresa().ToList();

                if (lista.Count > 1)
                {
                    lista.Insert(0, new object[] { "0", " " });
                }
                return lista.ToArray();
            }
            else
            {
                var dadosEmpresa = _empresa.ConsultaEmpresaById(codEmpresa).Result;
                var lista = new object[][]
                {
                    new object[] { dadosEmpresa.CODEMPRESA, dadosEmpresa.NMFANTASIA}
                };
                return lista.ToArray();
            }

        }

        public string GetnmEmpresaByID(int codEmpresa)
        {
            var empresa = _empresa.ConsultaEmpresaById(codEmpresa).Result;

            return empresa.NMFANTASIA;
        }

        public object[][] GetFormasLiquidacao()
        {
            var lista = _formaLiquidacao.GetHashFormasFaturamento().ToList();

            return lista.ToArray();
        }
        public object[][] DDLPlanoContasPagamento(int CodEmpresa)
        {
            var listaPlanoContas = _PlanoContas.BuscaPlanosContasPagamento(CodEmpresa).Result;
            //caso não exista nenhum plano de conta cadastrado, o sistema retorna o plano de contas default.
            if (listaPlanoContas == null || listaPlanoContas.Count == 0)
            {
                var listaDefault = _PlanoContas.BuscaPlanosContasDefaultPagamento().Result;
                listaPlanoContas.AddRange(listaDefault);
            }
            var ListaRaiz = listaPlanoContas.Where(x => x.CODPLANOCONTAS_PAI == null).ToList();

            var listaPlano = MontaDDLPlanoContas(ListaRaiz, listaPlanoContas, 0).ToList();
            var listaDDL = listaPlano.Select(x => new object[] { x.CODPLANOCONTAS, x.CLASSIFICACAO + " " + x.DESCRICAO }).ToList();
            listaDDL.Insert(0, new object[] { "0", " " });

            return listaDDL.ToArray();
        }
        public object[][] DDLPlanoContasRecebimento(int CodEmpresa)
        {
            var listaPlanoContas = _PlanoContas.BuscaPlanosContasRecebimento(CodEmpresa).Result;
            //caso não exista nenhum plano de conta cadastrado, o sistema retorna o plano de contas default.
            if (listaPlanoContas == null || listaPlanoContas.Count == 0)
            {
                var listaDefault = _PlanoContas.BuscaPlanosContasDefaultRecebimento().Result;
                listaPlanoContas.AddRange(listaDefault);
            }
            var ListaRaiz = listaPlanoContas.Where(x => x.CODPLANOCONTAS_PAI == null).ToList();

            var listaPlano = MontaDDLPlanoContas(ListaRaiz, listaPlanoContas, 0).ToList();
            var listaDDL = listaPlano.Select(x => new object[] { x.CODPLANOCONTAS, x.CLASSIFICACAO + " " + x.DESCRICAO }).ToList();
            listaDDL.Insert(0, new object[] { "0", " " });

            return listaDDL.ToArray();
        }

        public object[][] DDLPlanoContas(int CodEmpresa)
        {
            var listaPlanoContas = _PlanoContas.BuscaTodosPlanosContas(CodEmpresa).Result;
            //caso não exista nenhum plano de conta cadastrado, o sistema retorna o plano de contas default.
            if (listaPlanoContas == null || listaPlanoContas.Count ==0)
            {
                var listaDefault = _PlanoContas.BuscaPlanosContasDefault().Result;
                listaPlanoContas.AddRange(listaDefault);
            }
            var ListaRaiz = listaPlanoContas.Where(x => x.CODPLANOCONTAS_PAI == null).ToList();

            var listaPlano = MontaDDLPlanoContas(ListaRaiz, listaPlanoContas, 0).ToList();
            var listaDDL = listaPlano.Select(x => new object[] { x.CODPLANOCONTAS, x.CLASSIFICACAO + " " + x.DESCRICAO }).ToList();
            listaDDL.Insert(0, new object[] { "0", " " });

            return listaDDL.ToArray();

        }
        public List<PAGNET_CADPLANOCONTAS> MontaDDLPlanoContas(List<PAGNET_CADPLANOCONTAS> ListaRaiz, List<PAGNET_CADPLANOCONTAS> lista, int nivel)
        {
            string espaco = "";
            for (int i = 0; i < nivel; i++)
            {
                espaco += "    ";
            }
            try
            {
                List<PAGNET_CADPLANOCONTAS> listaRetorno = new List<PAGNET_CADPLANOCONTAS>();
                PAGNET_CADPLANOCONTAS itemLista;

                foreach (var item in ListaRaiz)
                {
                    itemLista = new PAGNET_CADPLANOCONTAS();
                    itemLista.CODPLANOCONTAS = item.CODPLANOCONTAS;
                    itemLista.CLASSIFICACAO = item.CLASSIFICACAO;
                    itemLista.DESCRICAO = item.DESCRICAO;
                    listaRetorno.Add(itemLista);

                    if (lista.Where(x => x.CODPLANOCONTAS_PAI == item.CODPLANOCONTAS).Count() > 0)
                    {
                        listaRetorno.AddRange(MontaDDLPlanoContas(lista.Where(x => x.CODPLANOCONTAS_PAI == item.CODPLANOCONTAS).ToList(), lista, nivel + 1));
                    }
                }
                return listaRetorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
