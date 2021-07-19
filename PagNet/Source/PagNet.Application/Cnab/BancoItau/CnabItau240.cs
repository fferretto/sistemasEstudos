using PagNet.Application.Helpers;
using PagNet.Application.Models;
using System;

namespace PagNet.Application.Cnab.BancoItau
{
    public class CnabItau240
    {
        private readonly string ValorVazio = string.Empty;
        public string HeaderArquivo(mdCedente Cedente)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro("0", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("0", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 6);    //Branco
                str += Geral.FormataInteiro("081", 3);    //LAYOUT DE ARQUIVO
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Geral.FormataTexto(ValorVazio, 20); //BRANCOS 
                str += Geral.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Geral.FormataTexto(ValorVazio, 1);   //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Geral.FormataInteiro(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Agência / Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora), 30); //Nome da Empresa
                str += Geral.FormataTexto("Banco Itau", 30); //Nome do Banco
                str += Geral.FormataTexto(ValorVazio, 10); //Brancos 
                str += Geral.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Geral.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Geral.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Geral.FormataInteiro("0", 9); //ZEROS 
                str += Geral.FormataInteiro("0", 5); //Densidade de Gravação Arquivo
                str += Geral.FormataTexto(ValorVazio, 69); // BRANCOS


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }
        public string HeaderLote(mdCedente Cedente, TransacaoTransferencia Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //TIPO DE PAGTO
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("040", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataTexto(ValorVazio, 4); //IDENTIFICAÇÃO DO LANÇAMENTO NO EXTRATO DO FAVORECIDO
                str += Geral.FormataTexto(ValorVazio, 16); //BRANCOS 
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Geral.FormataInteiro(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 30); //FINALIDADE DO LOTE 
                str += Geral.FormataTexto(ValorVazio, 10); //HISTÓRICO DE C/C DEBITADA 
                str += Geral.FormataTexto(ValorVazio, 30); //Endereço 
                str += Geral.FormataInteiro(ValorVazio, 5); //Número 
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade 
                str += Geral.FormataInteiro(ValorVazio, 8); //CEP 
                str += Geral.FormataTexto(ValorVazio, 2); //UF 
                str += Geral.FormataTexto(ValorVazio, 8); //Branco 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240.", ex);
            }
        }
        public string HeaderLotePagBoleto(mdCedente Cedente, TransacoesPagamento Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //TIPO DE PAGTO
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("030", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataTexto(ValorVazio, 4); //IDENTIFICAÇÃO DO LANÇAMENTO NO EXTRATO DO FAVORECIDO
                str += Geral.FormataTexto(ValorVazio, 16); //BRANCOS 
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Geral.FormataInteiro(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 30); //FINALIDADE DO LOTE 
                str += Geral.FormataTexto(ValorVazio, 10); //HISTÓRICO DE C/C DEBITADA 
                str += Geral.FormataTexto(ValorVazio, 30); //Endereço 
                str += Geral.FormataInteiro(ValorVazio, 5); //Número 
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade 
                str += Geral.FormataInteiro(ValorVazio, 8); //CEP 
                str += Geral.FormataTexto(ValorVazio, 2); //UF 
                str += Geral.FormataTexto(ValorVazio, 8); //Branco 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240.", ex);
            }
        }
        public string DetalheLoteSegmento_A(mdCedente Cedente, TransacaoTransferencia Transferencia, int codCamaraCentralizadora, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("A", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 3); //Tipo de Movimento
                str += Geral.FormataInteiro(codCamaraCentralizadora.ToString(), 3); //Código Câmara Compensação (018 = TED CIP / 700 = DOC)
                str += Geral.FormataInteiro(Transferencia.codBancoFavorecido.ToString(), 3); //Código do Banco Favorecido
                str += Geral.FormataTexto(Transferencia.ContaCorrenteFavorecido, 20); //AGÊNCIA e CONTA FAVORECIDO
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Transferencia.nmFavorecido), 30); //Nome do Favorecido
                str += Geral.FormataTexto(Transferencia.SeuNumero, 20); //Nro. do Documento Cliente
                str += Geral.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataTexto("BRL", 3); //Tipo da Moeda
                str += Geral.FormataInteiro(ValorVazio, 8); //CÓDIGO ISPB
                str += Geral.FormataInteiro(ValorVazio, 7); //ZEROS 
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(Transferencia.Valor), 15); //Valor do Pagamento
                str += Geral.FormataTexto(Transferencia.NossoNumero, 15); //Nro. do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 5); //Branco 
                str += Geral.FormataInteiro(ValorVazio, 8); //DATA REAL EFETIVAÇÃO DO PAGTO 
                str += Geral.FormataInteiro(ValorVazio, 15); //VALOR REAL EFETIVAÇÃO DO PAGTO 
                str += Geral.FormataTexto(ValorVazio, 18); //INFORMAÇÃO COMPLEMENTAR P/ HIST. DE C/C
                str += Geral.FormataTexto(ValorVazio, 2); //BRANCOS 
                str += Geral.FormataInteiro(ValorVazio, 6); //Nº DO DOC/TED/ OP/ CHEQUE NO RETORNO 
                str += Geral.FormataInteiro(Transferencia.CpfCnpjFavorecido, 14); //N DE INSCRIÇÃO DO FAVORECIDO (CPF/CNPJ) 
                str += Geral.FormataTexto(Transferencia.FinalidadeDoc, 2); //Finalidade do DOC
                str += Geral.FormataTexto(Transferencia.FinalidadeTed, 5); //Finalidade de TED
                str += Geral.FormataTexto(ValorVazio, 5); //BRANCOS 
                str += Geral.FormataTexto(Transferencia.EmissaoAvisoFavorecido, 1); //Emissão de Aviso ao Favorecido
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento A.", ex);
            }
        }
        public string DetalheLoteSegmento_B(mdCedente Cedente, TransacaoTransferencia Transferencia, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("B", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataTexto(ValorVazio, 3); //Brancos 
                str += Geral.FormataInteiro(Transferencia.tipInscricaoFavorecido.ToString(), 1); //Tipo de Inscrição do Favorecido
                str += Geral.FormataInteiro(Transferencia.CpfCnpjFavorecido, 14); //CNPJ/CPF do Favorecido
                str += Geral.FormataTexto(ValorVazio, 30); //Logradouro do Favorecido
                str += Geral.FormataInteiro(ValorVazio, 5); //Número do Local do Favorecido
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Local Favorecido
                str += Geral.FormataTexto(ValorVazio, 15); //Bairro do Favorecido
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade do Favorecido
                str += Geral.FormataInteiro(ValorVazio, 8); //CEP do Favorecido
                str += Geral.FormataTexto(ValorVazio, 2); //Estado do Favorecido
                str += Geral.FormataTexto(ValorVazio, 100); //ENDEREÇO DE E-MAIL
                str += Geral.FormataTexto(ValorVazio, 3); //BRANCOS 
                str += Geral.FormataTexto(ValorVazio, 10); //CÓDIGO DE OCORRÊNCIAS NO RETORNO


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento B.", ex);
            }
        }
        public string DetalheLoteSegmento_J(mdCedente Cedente, TransacoesPagamento Trans, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 3); //Tipo de Movimento
                str += Geral.FormataTexto(Geral.FormataCodigoBarraPagamento(Trans.codigoBarras), 44);     //Código de Barras 
                str += Geral.FormataTexto(Cedente.nmOperadora, 30);   //Nome do Cedente
                str += Geral.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(Trans.Valor), 15); //Valor Nominal do Título
                str += Geral.FormataInteiro(Trans.vlDesconto.ToString(), 15); //Valor Desconto + Abatimento
                str += Geral.FormataInteiro(Trans.vlJurosMulta.ToString(), 15); //Valor Multa + Juros
                str += Geral.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataInteiro(Trans.vlTotalPagar.ToString(), 15); //Valor do Pagamento
                str += Geral.FormataInteiro(ValorVazio, 15); // Quantidade de Moeda
                str += Geral.FormataTexto(Trans.SeuNumero, 20); //Número do Documento Cliente
                str += Geral.FormataTexto(ValorVazio, 13); //Número do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 15); //Brancos 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }
        public string TrailerLote(int NumLot, decimal TotalValorPagar, int TotRegistros)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("5", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Brancos 
                str += Geral.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Geral.FormataInteiro(TotalValorPagar.ToString(), 18); //Somatória dos Valores
                str += Geral.FormataInteiro(ValorVazio, 18); //ZEROS 
                str += Geral.FormataTexto(ValorVazio, 171); //BRANCOS 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do lote de remessa do CNAB240.", ex);
            }
        }
        public string TrailerArquivo(int TotaLote, int TotalReg)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("341", 3); //Código do Banco 
                str += Geral.FormataInteiro("9999", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("9", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Branco
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

    }
}
