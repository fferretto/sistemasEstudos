using PagNet.Application.Helpers;
using System;
using PagNet.Application.Models;

namespace PagNet.Application.Cnab.BancoABCBrasil
{
    public class CNABBrasilABC240
    {
        private readonly string ValorVazio = string.Empty;
        public string HeaderArquivo(mdCedente Cedente, string TipoTratamento)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("246", 3); //Código do Banco na Compensação 
                str += Geral.FormataInteiro(ValorVazio, 4); //Lote de Serviço 
                str += Geral.FormataInteiro(ValorVazio, 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 6);    //Uso Exclusivo FEBRABAN / CNAB -Branco
                str += Geral.FormataInteiro("080", 3);    //Nº da Versão do Layout do Arquivo
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Geral.FormataTexto(TipoTratamento, 1); //Tipo de Tratamento para Arquivo 
                str += Geral.FormataTexto(ValorVazio, 19); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Geral.FormataTexto(ValorVazio, 1);   //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Geral.FormataInteiro(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Agência / Conta
                str += Geral.FormataTexto((Geral.RemoveCaracterEspecial(Cedente.nmOperadora)).ToUpper(), 30); //Nome da Empresa
                str += Geral.FormataTexto("BRASIL ABC", 30); //Nome do Banco
                str += Geral.FormataTexto(ValorVazio, 10); //Brancos 
                str += Geral.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Geral.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Geral.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Geral.FormataInteiro(ValorVazio, 9); //Número Sequencial do Arquivo
                str += Geral.FormataInteiro(ValorVazio, 5); //Densidade de Gravação Arquivo
                str += Geral.FormataTexto(ValorVazio, 69); //Brancos 


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
                str += Geral.FormataInteiro("246", 3); //Código do Banco 
                str += Geral.FormataInteiro("9999", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("9", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB
                str += Geral.FormataInteiro(TotaLote.ToString(), 6); //Quantidade de lotes do arquivo
                str += Geral.FormataInteiro(TotalReg.ToString(), 6); //Quantidade de registros no arquivo
                str += Geral.FormataTexto(ValorVazio, 211); //Brancos 


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do arquivo de remessa do CNAB240.", ex);
            }
        }
        public string HeaderLote(mdCedente Cedente, TransacaoTransferencia Transacoes, int NumLot, DateTime DataPGTO)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("246", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("030", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataInteiro(DataPGTO.ToString("ddMMyyyy"), 8); //Data de Pagamento
                str += Geral.FormataTexto(ValorVazio, 12); //Complemento de Registro
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Geral.FormataInteiro(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()).ToUpper(), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 30); //Finalidade os Pagamentos do Lote 
                str += Geral.FormataTexto(ValorVazio, 10); //Complemento Histórico C/C Debitada 
                str += Geral.FormataTexto(ValorVazio, 30); //Endereço 
                str += Geral.FormataInteiro(ValorVazio, 5); //Número 
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade 
                str += Geral.FormataInteiro(Geral.RemoveCaracteres(Cedente.CEP), 8); //CEP 
                str += Geral.FormataTexto(ValorVazio, 2); //UF 
                str += Geral.FormataTexto(ValorVazio, 8); //Uso Exclusivo FEBRABAN/CNAB  
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }
        public string TrailerLote(int NumLot, decimal TotalValorPagar, int TotRegistros)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("246", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("5", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(TotalValorPagar), 18); //Somatória dos Valores
                str += Geral.FormataInteiro("0", 18); //Somatória Quantidade Moeda 
                str += Geral.FormataTexto(ValorVazio, 171);   //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do lote de remessa do CNAB240.", ex);
            }
        }
        public string DetalheLoteSegmento_A(mdCedente Cedente, TransacaoTransferencia trans, int codCamaraCentralizadora, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("246", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("A", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro(ValorVazio, 3); //Tipo de Movimento
                str += Geral.FormataInteiro(ValorVazio, 3); //Complemento de Registro 
                str += Geral.FormataInteiro(trans.codBancoFavorecido.ToString(), 3); //Código do Banco Favorecido
                str += Geral.FormataTexto(trans.ContaCorrenteFavorecido, 20); //Agência Conta Favorecido 
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(trans.nmFavorecido), 30); //Nome do Favorecido
                str += Geral.FormataTexto(trans.SeuNumero, 20); //Nro. do Documento Cliente
                str += Geral.FormataInteiro(trans.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataTexto("009", 3); //Tipo da Moeda
                str += Geral.FormataInteiro("0", 15); //Número Agência Debitada 
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(trans.Valor), 15); //Valor do Pagamento
                str += Geral.FormataTexto(ValorVazio, 15); //Nro. do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 5); //Complemento de Registro 
                str += Geral.FormataInteiro(trans.dtRealPagamento.ToString("ddMMyyyy"), 8); //Valor Real do Pagamento (Retorno)
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(trans.Valor), 15); //Valor Real Efetivação do Pagto            
                str += Geral.FormataTexto(ValorVazio, 20); //Informação 2 - Mensagem
                str += Geral.FormataInteiro(ValorVazio, 6); //Nº da TED 
                str += Geral.FormataInteiro(Geral.RemoveCaracteres(trans.CpfCnpjFavorecido), 14); //Nº de Inscrição do Favorecido (CPF/CNPJ) 
                str += Geral.FormataInteiro(trans.tipInscricaoFavorecido.ToString(), 1); //Nº de Inscrição do Favorecido (CPF/CNPJ) 
                str += Geral.FormataInteiro(ValorVazio, 1); //Complemento de Registro 
                str += Geral.FormataTexto(trans.FinalidadeTed, 5); //Finalidade de TED
                str += Geral.FormataTexto(ValorVazio, 5); //Complemento de Registro 
                str += Geral.FormataTexto(ValorVazio, 1); //Aviso ao Favorecido  
                str += Geral.FormataTexto(trans.OcorrenciaRetorno, 10); //Ocorrências para o Retorno



                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento A.", ex);
            }
        }
        public string DetalheLoteSegmento_J(mdCedente Cedente, TransacoesPagamento Trans, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("246", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 3); //Tipo de Movimento
                str += Geral.FormataInteiro(Geral.FormataCodigoBarraPagamento(Trans.codigoBarras), 44);     //Código de Barras 
                str += Geral.FormataTexto(Cedente.nmOperadora, 30);   //Nome do Cedente
                str += Geral.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(Trans.Valor), 15); //Valor Nominal do Título
                str += Geral.FormataInteiro(Trans.vlDesconto.ToString(), 15); //Valor Desconto + Abatimento
                str += Geral.FormataInteiro(Trans.vlJurosMulta.ToString(), 15); //Valor Multa + Juros
                str += Geral.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(Trans.vlTotalPagar).ToString(), 15); //Valor do Pagamento
                str += Geral.FormataInteiro(Trans.CpfCnpj, 14); // Número de Inscrição do Cedente 
                str += Geral.FormataInteiro(ValorVazio, 1); // Complemento de Registro
                str += Geral.FormataTexto(Trans.SeuNumero, 20); //Número do Documento Cliente 
                str += Geral.FormataInteiro(Trans.tipInscricaoFavorecido.ToString(), 1); // Quantidade de Moeda
                str += Geral.FormataTexto(ValorVazio, 12); //Número do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 15); //Brancos 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }

    }
}
