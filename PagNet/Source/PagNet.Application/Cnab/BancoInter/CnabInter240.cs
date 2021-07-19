using PagNet.Application.Helpers;
using System;
using PagNet.Application.Models;


namespace PagNet.Application.Cnab.BancoInter
{
    public class CnabInter240
    {
        private readonly string ValorVazio = string.Empty;
        public string HeaderArquivo(mdCedente Cedente)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("077", 3); //Código do Banco na Compensação 
                str += Geral.FormataInteiro("0", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("0", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN / CNAB -Branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Geral.FormataTexto(Cedente.CodConvenioPag, 20); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Geral.FormataInteiro(Cedente.DigitoAgencia, 1);   //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Geral.FormataTexto(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência / Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora), 30); //Nome da Empresa
                str += Geral.FormataTexto("INTER", 30); //Nome do Banco
                str += Geral.FormataTexto(ValorVazio, 10); //Brancos 
                str += Geral.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Geral.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Geral.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Geral.FormataInteiro(Cedente.NumSeq.ToString(), 6); //Número Sequencial do Arquivo
                str += Geral.FormataInteiro("103", 3); //Número da Versão do Layout
                str += Geral.FormataInteiro("1600", 5); //Densidade de Gravação Arquivo
                str += Geral.FormataTexto(ValorVazio, 20); //Brancos 
                str += Geral.FormataTexto(ValorVazio, 20); //Uso Reservado da Empresa
                str += Geral.FormataTexto(ValorVazio, 29); //Uso Exclusivo FEBRABAN / CNAB


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }
        public string TrailerArquivo(int TotaLote, int TotalReg, int qtContas)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("077", 3); //Código do Banco 
                str += Geral.FormataInteiro("9999", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("9", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB
                str += Geral.FormataInteiro(TotaLote.ToString(), 6); //Quantidade de lotes do arquivo
                str += Geral.FormataInteiro(TotalReg.ToString(), 6); //Quantidade de registros no arquivo
                str += Geral.FormataInteiro(qtContas.ToString(), 6); //Qtde de Contas p/ Conc. (Lotes) 
                str += Geral.FormataTexto(ValorVazio, 205); //Brancos 


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do arquivo de remessa do CNAB240.", ex);
            }
        }
        public string HeaderLotePagBoleto(mdCedente Cedente, TransacaoTransferencia Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("077", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("046", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataTexto(Cedente.CodConvenioPag.ToString(), 20); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(Cedente.DigitoAgencia.ToString(), 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 40); //Informação 1 - Mensagem
                str += Geral.FormataTexto(ValorVazio, 30); //Endereço 
                str += Geral.FormataInteiro(ValorVazio, 5); //Número 
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade 
                str += Geral.FormataInteiro(ValorVazio, 5); //CEP 
                str += Geral.FormataInteiro(ValorVazio, 3); //Complemento do CEP
                str += Geral.FormataTexto(ValorVazio, 2); //UF 
                str += Geral.FormataInteiro("01", 2); // Indicativo de Forma de Pagamento do Serviço 
                str += Geral.FormataTexto(ValorVazio, 6); //Uso Exclusivo FEBRABAN/CNAB  
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }
        public string DetalheLoteSegmento_O(mdCedente Cedente, TransacoesPagamento Trans, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("077", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("O", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 1); //Tipo de Movimento
                str += Geral.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Geral.FormataTexto(Geral.FormataCodigoBarraPagConcessionario(Trans.codigoBarras), 44);     //Código de Barras 
                str += Geral.FormataTexto(Trans.nmFavorecido, 30);   //Nome do Cedente
                str += Geral.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Geral.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataInteiro(Trans.vlTotalPagar.ToString(), 15); //Valor Total do Pagamento
                str += Geral.FormataTexto(Trans.SeuNumero, 20); //Número do Documento Cliente
                str += Geral.FormataTexto(Trans.NossoNumero, 20); //Número do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 68); //Brancos 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento O.", ex);
            }
        }
        public string TrailerLote(int NumLot, decimal TotalValorPagar, int TotRegistros)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("077", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("5", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Geral.FormataInteiro(TotalValorPagar.ToString(), 18); //Somatória dos Valores
                str += Geral.FormataTexto(ValorVazio, 189);   //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do lote de remessa do CNAB240.", ex);
            }
        }
        public string HeaderLote(mdCedente Cedente, TransacaoTransferencia Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("077", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("060", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 15); //Número de Inscrição da Empresa
                str += Geral.FormataTexto(Cedente.CodConvenioPag.ToString(), 20); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(Cedente.DigitoAgencia.ToString(), 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 40); //MENSAGEM 1 
                str += Geral.FormataTexto(ValorVazio, 40); //MENSAGEM 2 
                str += Geral.FormataInteiro(ValorVazio, 8); //Número 
                str += Geral.FormataInteiro(ValorVazio, 8); //Complemento do Endereço
                str += Geral.FormataInteiro(ValorVazio, 8); //Cidade 
                str += Geral.FormataInteiro(ValorVazio, 33); //Uso Exclusivo FEBRABAN/CNAB  

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }






    }
}
