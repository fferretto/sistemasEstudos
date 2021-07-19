using System.Collections.Generic;
using TELENET.SIL.DA;
using TELENET.SIL.PO;

namespace TELENET.SIL.BL
{
    public class blTabelas
    {
        readonly OPERADORA FOperador;

        public blTabelas(OPERADORA Operador)
        { 
            FOperador = Operador; 
        }

        #region Status

        public List<STATUS> ColecaoStatus()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaStatus();
        }

        public List<STATUS> ColecaoStatusCred()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaStatusCred();
        }

        public List<STATUS> ColecaoStatusOper()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaStatusOper();
        }

        public List<STATUS> ColecaoStatusUsu()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaStatusUsu();
        }

        // uma lista mais configurável,
        // aqui podemos especificar qualquer campo da tabela
        public List<STATUS> ColecaoStatusCadUsu(string which)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaStatusCadUsu(which);
        }

        public List<STATUS> ColecaoStatusCli()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaStatusCli();
        }

        public STATUS GetStatus(string codSta)
        {
            return new daTabelas(FOperador).GetStatus(codSta);
        }

        public STATUS GetStatusCli(string codSta)
        {
            return new daTabelas(FOperador).GetStatusCli(codSta);
        }

        public STATUS GetStatusByName(string codSta)
        {
            return new daTabelas(FOperador).GetStatusByName(codSta);
        }

        #endregion

        #region Parentesco

        public List<PARENTESCO> ColecaoParentesco()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaParentesco();
        }

        #endregion

        #region Taxas

        public List<TAXAPJ> ColecaoTaxasCli(int sistema)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaTaxasCli(sistema);
        }

        public List<TAXAPJ> ListaTaxasCliComTaxaDefault(int sistema)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaTaxasCliComTaxaDefault(sistema);
        }

        public List<TAXAVA> ColecaoTaxasCre(int codCre)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaTaxasCre(codCre);
        }

        public TAXAVA GetTaxa(int codTaxa)
        {
            return new daTabelas(FOperador).GetTaxa(codTaxa);
        }

        public TAXAVA GetTaxaByName(string nomTaxa)
        {
            return new daTabelas(FOperador).GetTaxaByName(nomTaxa);
        }

        #endregion

        #region Agrupamento

        public List<AGRUPAMENTO> ColecaoAgrupamento()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaAgrupamento();
        }

        public AGRUPAMENTO GetAgrupamento(int codAg)
        {
            return new daTabelas(FOperador).GetAgrupamento(codAg);
        }

        public AGRUPAMENTO GetAgrupamentoByName(string nomAg)
        {
            return new daTabelas(FOperador).GetAgrupamentoByName(nomAg);
        }

        #endregion

        #region FORMA DE PAGAMENTO

        public List<REEMBOLSO> ColecaoForPag()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaForPag();
        }

        public REEMBOLSO GetForPag(short forPag)
        {
            return new daTabelas(FOperador).GetForPag(forPag);
        }

        public REEMBOLSO GetForPagByName(string desForPag)
        {
            return new daTabelas(FOperador).GetForPagByName(desForPag);
        }

        #endregion

        #region UF

        public List<UF> ColecaoUFLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaUF();
        }

        public UF GetUF(string sigUF)
        {
            return new daTabelas(FOperador).GetUF(sigUF);
        }

        public UF GetUFByName(string sigUF)
        {
            return new daTabelas(FOperador).GetUFByName(sigUF);
        }

        #endregion

        #region REGIAO

        public List<REGIAO> ColecaoRegiaoLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarRegiao();
        }

        public int ProximoCodigoRegiaoLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoRegiaoLivre();
        }

        public REGIAO GetRegiao(int codReg)
        {
            return new daTabelas(FOperador).GetRegiao(codReg);
        }

        public REGIAO GetRegiaoByName(string nomReg)
        {
            return new daTabelas(FOperador).GetRegiaoByName(nomReg);
        }

        public void Incluir(REGIAO Regiao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Regiao.DESREG = UtilSIL.RemoverAcentosMenosHifem(Regiao.DESREG);
            TabelasDAL.InserirRegiao(Regiao);
        }

        public void Alterar(REGIAO Regiao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Regiao.DESREG = UtilSIL.RemoverAcentosMenosHifem(Regiao.DESREG);
            TabelasDAL.AlterarRegiao(Regiao);
        }

        public void Excluir(REGIAO Regiao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirRegiao(Regiao);
        }


        #endregion

        #region REGIONAL

        public List<REGIONAL> ColecaoRegionalLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarRegional();
        }

        public int ProximoCodigoRegionalLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoRegionalLivre();
        }

        public REGIONAL GetRegional(int codReo)
        {
            return new daTabelas(FOperador).GetRegional(codReo);
        }

        public REGIONAL GetRegionalByName(string nomReo)
        {
            return new daTabelas(FOperador).GetRegionalByName(nomReo);
        }

        public void Incluir(REGIONAL Regional)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Regional.DESREO = UtilSIL.RemoverAcentos(Regional.DESREO);
            TabelasDAL.InserirRegional(Regional);
        }

        public void Alterar(REGIONAL Regional)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Regional.DESREO = UtilSIL.RemoverAcentos(Regional.DESREO);
            TabelasDAL.AlterarRegional(Regional);
        }

        public void Excluir(REGIONAL Regional)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirRegional(Regional);
        }

        #endregion

        #region CREDENCIADOR

        public List<EPS> ColecaoCredenciadorLK(string tipEps, int codReo)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarCredenciador(tipEps, codReo);
        }

        public int ProximoCodigoCredenciadorLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoCredenciadorLivre();
        }

        public EPS GetCredenciador(int codReo, int codEps)
        {
            return new daTabelas(FOperador).GetCredenciador(codReo, codEps);
        }

        public EPS GetCredenciadorComTipeps(int codEps)
        {
            return new daTabelas(FOperador).GetCredenciadorComTipeps(codEps);
        }

        public EPS GetVendedorByNameComTipEps(string nomEps)
        {
            return new daTabelas(FOperador).GetVendedorByName(nomEps);
        }

        public EPS GetCredenciadorByName(string nomEps, int codReo)
        {
            return new daTabelas(FOperador).GetCredenciadorByName(nomEps, codReo);
        }

        public void Incluir(string tipEps, int codReo, string NOMEPS)
        {
            //Persistir
            var Eps = new EPS();
            var TabelasDAL = new daTabelas(FOperador);
            NOMEPS = UtilSIL.RemoverAcentos(NOMEPS);
            TabelasDAL.InserirCredenciador(tipEps, codReo, NOMEPS);
        }

        public void Alterar(int CODEPS, string NOMEPS)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            NOMEPS = UtilSIL.RemoverAcentos(NOMEPS);
            TabelasDAL.AlterarCredenciador(CODEPS, NOMEPS);
        }

        public void Excluir(int CODEPS)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirCredenciador(CODEPS);
        }

        #endregion

        #region ATIVIDADE

        public List<ATIVIDADE> ColecaoAtividadeLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarAtividade();
        }

        public int ProximoCodigoAtividadeLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoAtividadeLivre();
        }

        public ATIVIDADE GetAtividade(int codAti)
        {
            return new daTabelas(FOperador).GetAtividade(codAti);
        }

        public ATIVIDADE GetAtividadeByName(string nomAti)
        {
            return new daTabelas(FOperador).GetAtividadeByName(nomAti);
        }

        public void Incluir(ATIVIDADE Atividade)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Atividade.NOMATI = UtilSIL.RemoverAcentos(Atividade.NOMATI);
            TabelasDAL.InserirAtividade(Atividade);
        }

        public void Alterar(ATIVIDADE Atividade)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Atividade.NOMATI = UtilSIL.RemoverAcentos(Atividade.NOMATI);
            TabelasDAL.AlterarAtividade(Atividade);
        }

        public void Excluir(ATIVIDADE Atividade)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirAtividade(Atividade);
        }

        #endregion

        #region SEGMENTO

        public SEGMENTO GetSegmentoByName(string nomSeg)
        {
            return new daTabelas(FOperador).GetSegmentoByName(nomSeg);
        }

        public List<SEGMENTO> ColecaoSegmentoLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarSegmento();
        }

        public List<SEGMENTO> ColecaoSegmentoLKPri(string cnpj)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarSegmento(cnpj);
        }

        public int ProximoCodigoSegmentoLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoSegmentoLivre();
        }

        public SEGMENTO GetSegmento(int codSeg)
        {
            return new daTabelas(FOperador).GetSegmento(codSeg);
        }

        public void Incluir(SEGMENTO Segmento)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Segmento.NOMSEG = UtilSIL.RemoverAcentos(Segmento.NOMSEG);            
            TabelasDAL.InserirSegmento(Segmento);
        }

        public void Alterar(SEGMENTO Segmento)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Segmento.NOMSEG = UtilSIL.RemoverAcentos(Segmento.NOMSEG);
            TabelasDAL.AlterarSegmento(Segmento);
        }

        public void Excluir(SEGMENTO Segmento)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirSegmento(Segmento);
        }

        public List<SEG> ColecaoSegLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarSeg();
        }

        public void Incluir(SEG Segmento)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Segmento.NOMSEG = UtilSIL.RemoverAcentos(Segmento.NOMSEG);
            TabelasDAL.InserirSeg(Segmento);
        }

        public void Alterar(SEG Segmento)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            Segmento.NOMSEG = UtilSIL.RemoverAcentos(Segmento.NOMSEG);
            TabelasDAL.AlterarSeg(Segmento);
        }

        public void Excluir(SEG Segmento)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirSeg(Segmento);
        }


        #endregion

        #region BAIRRO

        public List<BAIRRO> ColecaoBairroLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarBairro();
        }

        public int ProximoCodigoBairroLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoBairroLivre();
        }

        public BAIRRO GetBairro(int codBai)
        {
            return new daTabelas(FOperador).GetBairro(codBai);
        }

        public BAIRRO GetBairroByName(string nomBair)
        {
            return new daTabelas(FOperador).GetBairroByName(nomBair);
        }

        public void Incluir(BAIRRO Bairro)
        {
            Bairro.NOMBAI = UtilSIL.RemoverAcentos(Bairro.NOMBAI);

            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirBairro(Bairro);
        }

        public void Alterar(BAIRRO Bairro)
        {
            Bairro.NOMBAI = UtilSIL.RemoverAcentos(Bairro.NOMBAI);

            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarBairro(Bairro);

        }

        public void Excluir(BAIRRO Bairro)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirBairro(Bairro);
        }

        #endregion

        #region SUBREDE

        public List<SUBREDE> ColecaoSubRedeLK() // Refazer
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarSubrede();
        }

        public int ProximoCodigoSubRedeLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoSubRedeLivre();
        }

        public SUBREDE GetSubrede(int codSubrede)
        {
            return new daTabelas(FOperador).GetSubrede(codSubrede);
        }

        public SUBREDE GetSubRedeByName(string nomSubR)
        {
            return new daTabelas(FOperador).GetSubRedeByName(nomSubR);
        }

        public void Incluir(SUBREDE SubRede)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirSubRede(SubRede);
        }

        public void Alterar(SUBREDE SubRede)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarSubRede(SubRede);

        }

        public void Excluir(SUBREDE SubRede)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirSubRede(SubRede);
        }

        #endregion

        #region GRUPOSOCIETARIO

        public List<GRUPOSOCIETARIO> ColecaoGrupoSocietarioLK() // Refazer
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarGrupoSocietario();
        }

        public int ProximoCodigoGrupoSocietarioaLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoGrupoSocietarioLivre();
        }

        public GRUPOSOCIETARIO GetGrupoSocietario(int codGrupoSocietario)
        {
            return new daTabelas(FOperador).GetGrupoSocietario(codGrupoSocietario);
        }

        public GRUPOSOCIETARIO GetGrupoSocietarioByName(string nomGrupoSocietario)
        {
            return new daTabelas(FOperador).GetGrupoSocietarioByName(nomGrupoSocietario);
        }

        public void Incluir(GRUPOSOCIETARIO GrupoSocietario)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirGrupoSocietario(GrupoSocietario);
        }

        public void Alterar(GRUPOSOCIETARIO GrupoSocietario)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarGrupoSocietario(GrupoSocietario);

        }

        public void Excluir(GRUPOSOCIETARIO GrupoSocietario)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirGrupoSocietario(GrupoSocietario);
        }

        #endregion

        #region PARCERIA

        public List<PARCERIA> ColecaoParceriaLK() // Refazer
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarParceria();
        }

        public int ProximoCodigoParceriaLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoParceriaLivre();
        }

        public PARCERIA GetParceria(int codParceria)
        {
            return new daTabelas(FOperador).GetParceria(codParceria);
        }

        public PARCERIA GetParceriaByName(string nomParceria)
        {
            return new daTabelas(FOperador).GetParceriaByName(nomParceria);
        }

        public void Incluir(PARCERIA Parceria)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirParceria(Parceria);
        }

        public void Alterar(PARCERIA Parceria)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarParceria(Parceria);

        }

        public void Excluir(PARCERIA Parceria)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirParceria(Parceria);
        }

        #endregion

        #region UNIDADE

        public List<UNIDADE> ColecaoUnidadeLK() // Refazer
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarUnidade();
        }

        public int ProximoCodigoUnidadeLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoUnidadeLivre();
        }

        public UNIDADE GetUnidade(int codUnidade)
        {
            return new daTabelas(FOperador).GetUnidade(codUnidade);
        }

        public UNIDADE GetUnidadeByName(string nomUnidade)
        {
            return new daTabelas(FOperador).GetUnidadeByName(nomUnidade);
        }

        public void Incluir(UNIDADE Unidade)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirUnidade(Unidade);
        }

        public void Alterar(UNIDADE Unidade)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarUnidade(Unidade);

        }

        public void Excluir(UNIDADE Unidade)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirUnidade(Unidade);
        }

        #endregion

        #region SETORIND

        public List<SETORIND> ColecaoSetorLK() // Refazer
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarSetor();
        }

        public int ProximoCodigoSetorLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoSetorLivre();
        }

        public SETORIND GetSetor(int codSetor)
        {
            return new daTabelas(FOperador).GetSetor(codSetor);
        }

        public SETORIND GetSetorByName(string nomSetor)
        {
            return new daTabelas(FOperador).GetSetorByName(nomSetor);
        }

        public void Incluir(SETORIND Setor)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirSetor(Setor);
        }

        public void Alterar(SETORIND Setor)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarSetor(Setor);

        }

        public void Excluir(SETORIND Setor)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirSetor(Setor);
        }

        #endregion

        #region PORTE

        public List<PORTE> ColecaoPorteLK() // Refazer
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarPorte();
        }

        public int ProximoCodigoPorteLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoPorteLivre();
        }

        public PORTE GetPorte(int codPorte)
        {
            return new daTabelas(FOperador).GetPorte(codPorte);
        }

        public PORTE GetPorteByName(string nomPorte)
        {
            return new daTabelas(FOperador).GetPorteByName(nomPorte);
        }

        public void Incluir(PORTE Porte)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirPorte(Porte);
        }

        public void Alterar(PORTE Porte)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarPorte(Porte);

        }

        public void Excluir(PORTE Porte)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirPorte(Porte);
        }

        #endregion

        #region Produto

        public List<PRODUTO> ColecaoProduto() 
        {
            if (FOperador == null) return new List<PRODUTO>();
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarProduto();
        }

        public List<PRODUTONOVO> ColecaoProdutoNovo()
        {
            if (FOperador == null) return new List<PRODUTONOVO>();
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarProdutoNovo();
        }

        public List<TIPOPRODUTO> ColecaoTipoProd()
        {
            if (FOperador == null) return new List<TIPOPRODUTO>();
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarTipoProd();            
        }

        public List<USO> ColecaoTipoUso()
        {
            var listTipoUso = new List<USO>();
            var e = new USO();
            e.TIPOUSO = 'E';
            e.DESCRICAO = "Pessoa Jurídica";
            listTipoUso.Add(e);

            var p = new USO();
            p.TIPOUSO = 'P';
            p.DESCRICAO = "Pessoa Física";
            listTipoUso.Add(p);

            return listTipoUso;
        }

        public int ProximoCodigoProdutoLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoProdutolivre();
        }

        public PRODUTO GetProduto(int codProd)
        {
            return new daTabelas(FOperador).GetProduto(codProd);
        }

        public PRODUTO GetProdutoByName(string nomProduto)
        {
            return new daTabelas(FOperador).GetProdutoByName(nomProduto);
        }

        public void Incluir(PRODUTO Produto)
        {            
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirProduto(Produto);
        }

        public void Alterar(PRODUTO Produto)
        {           
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarProduto(Produto);
        }

        public void Excluir(PRODUTO Produto)
        {           
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirProduto(Produto);
        }

        #endregion

        #region VENDEDOR

        public List<VENDEDOR> ColecaoVendedorLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarVendedor();
        }

        public int ProximoCodigoVendedorLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoVendedorLivre();
        }

        public EPS GetVendedor(int codReo, int codEps)
        {
            return new daTabelas(FOperador).GetVendedor(codReo, codEps);
        }

        public EPS GetVendedorByName(string nomSubR)
        {
            return new daTabelas(FOperador).GetVendedorByName(nomSubR);
        }

        public EPS GetVendedorByName(int codReo, string nomSubR)
        {
            return new daTabelas(FOperador).GetVendedorByName(codReo, nomSubR);
        }

        public void Incluir(VENDEDOR Vendedor)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirVendedor(Vendedor);
        }

        public void Alterar(VENDEDOR Vendedor)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarVendedor(Vendedor);

        }

        public void Excluir(VENDEDOR Vendedor)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirVendedor(Vendedor);
        }

        #endregion

        #region LOCALIDADE

        public List<LOCALIDADE> ColecaoLocalidadeLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaLocalidade();
        }

        public int ProximoCodigoLocalidadeLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoLocalidadeLivre();
        }

        public int GetCodLoc(IFilter filter)
        {
            var filtro = string.Empty;
            if (filter != null)
                filtro = filter.FilterString;

            var daTabelas = new daTabelas(FOperador);
            return daTabelas.GetCodLoc(filtro);
        }

        public LOCALIDADE GetLocalidade(int codLoc)
        {
            return new daTabelas(FOperador).GetLocalidade(codLoc);
        }

        public LOCALIDADE GetLocalidadeByName(string nomLoc)
        {
            return new daTabelas(FOperador).GetLocalidadeByName(nomLoc);
        }

        public void Incluir(LOCALIDADE Localidade)
        {
            Localidade.NOMLOC = UtilSIL.RemoverAcentos(Localidade.NOMLOC);

            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirLocalidade(Localidade);
        }

        public void Alterar(LOCALIDADE Localidade)
        {
            Localidade.NOMLOC = UtilSIL.RemoverAcentos(Localidade.NOMLOC);

            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarLocalidade(Localidade);
        }

        public void Excluir(LOCALIDADE Localidade)
        {
           var TabelasDAL = new daTabelas(FOperador);
           TabelasDAL.ExcluirLocalidade(Localidade);           
        }


        #endregion

        #region TIPO REEMBOLSO

        public List<REEMBOLSO> ColecaoReembolsoLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaReembolso();
        }

        public int ProximoCodigoReembolsoLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoReembolsoLivre();
        }

        public void Incluir(REEMBOLSO Reembolso)
        {
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirReembolso(Reembolso);
        }

        public void Alterar(REEMBOLSO Reembolso)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarReembolso(Reembolso);
        }

        public void Excluir(REEMBOLSO Reembolso)
        {
            // Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirReembolso(Reembolso);
        }
        
        #endregion

        #region FILIAL

        public List<FILNUTRI> ColecaoFilialLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListaFilial();
        }

        public int ProximoCodigoFilialLivre()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ProximoCodigoFilialLivre();
        }

        public void Incluir(FILNUTRI Filial)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirFilial(Filial);
        }

        public void Alterar(FILNUTRI Filial)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirFilial(Filial);
        }

        public void Excluir(FILNUTRI Filial)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirFilial(Filial);
        }
        
        #endregion

        #region LOGOCARTAO

        public List<LOGOCARTAO> ColecaoLogoCartaoLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarLogoCartao();
        }

        public LOGOCARTAO GetLogoCartao(string codLogo)
        {
            return new daTabelas(FOperador).GetLogoCartao(codLogo);
        }

        public void Incluir(LOGOCARTAO LogoCartao)
        {
            // Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirLogoCartao(LogoCartao);
        }

        public void Alterar(LOGOCARTAO LogoCartao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarLogoCartao(LogoCartao);
        }

        public void Excluir(LOGOCARTAO LogoCartao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirLogoCartao(LogoCartao);
        }

        #endregion

        #region MODELOCARTAO

        public List<MODELOCARTAO> ColecaoModeloCartaoLK()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarModeloCartao();
        }

        public MODELOCARTAO GetModeloCartao(string codModelo)
        {
            return new daTabelas(FOperador).GetModeloCartao(codModelo);
        }

        public void Incluir(MODELOCARTAO ModeloCartao)
        {
            // Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.InserirModeloCartao(ModeloCartao);
        }

        public void Alterar(MODELOCARTAO ModeloCartao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.AlterarModeloCartao(ModeloCartao);
        }

        public void Excluir(MODELOCARTAO ModeloCartao)
        {
            //Persistir
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.ExcluirModeloCartao(ModeloCartao);
        }

        #endregion

        #region BENEFICIO

        public List<BENEFICIO> ColecaoBeneficio()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarBeneficio();
        }

        public List<BENEFICIO> ColecaoBeneficioNaoAssociados(int codCli)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarBeneficioNaoAssosiado(codCli);
        }

        public List<BENEFICIO> ColecaoBeneficioAssociados(int codCli)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ListarBeneficioAssosiado(codCli);
        }

        public List<BENEFICIO_CLIENTE> ColecaoBeneficioAssociadosdDoCliente(int codCli)
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ColecaoBeneficioAssociadoDoCliente(codCli);
        }



        #endregion

        #region ALERTAS

        public bool ExibeAlertas()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.ExibeAlertas();
        }

        public IEnumerable<ALERTA> ObterAlertas()
        {
            var daTabelas = new daTabelas(FOperador);
            return daTabelas.ObterAlertas();
        }

        public int CountAlertas()
        {
            var TabelasDAL = new daTabelas(FOperador);
            return TabelasDAL.CountAlertas();
        }

        public void MarcarAlertaLido(int idAlerta)
        {
            var TabelasDAL = new daTabelas(FOperador);
            TabelasDAL.MarcarAlertaLido(idAlerta);
        }
        

        #endregion
    }
}