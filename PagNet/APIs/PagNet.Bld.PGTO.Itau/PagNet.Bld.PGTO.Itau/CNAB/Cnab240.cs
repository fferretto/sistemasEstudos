using PagNet.Bld.PGTO.Itau.Abstraction.Model;
using PagNet.Bld.PGTO.Itau.ModelAuxiliar;
using PagNet.Bld.PGTO.Itau.Util;
using System;

namespace PagNet.Bld.PGTO.Itau.CNAB
{
    public class Cnab240
    {
        private readonly string ValorVazio = string.Empty;
        public string HeaderArquivo(mdCedente Cedente)
        {
            try
            {
                string str = ValorVazio;
                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro("0", 4); //Lote de Serviço 
                str += Helper.FormataInteiro("0", 1); //Tipo de Registro 
                str += Helper.FormataTexto(ValorVazio, 6);    //Branco
                str += Helper.FormataInteiro("081", 3);    //LAYOUT DE ARQUIVO
                str += Helper.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Helper.FormataTexto(ValorVazio, 20); //BRANCOS 
                str += Helper.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Helper.FormataTexto(ValorVazio, 1);   //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Helper.FormataInteiro(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Agência / Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Cedente.nmOperadora), 30); //Nome da Empresa
                str += Helper.FormataTexto("Banco Itau", 30); //Nome do Banco
                str += Helper.FormataTexto(ValorVazio, 10); //Brancos 
                str += Helper.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Helper.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Helper.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Helper.FormataInteiro("0", 9); //ZEROS 
                str += Helper.FormataInteiro("0", 5); //Densidade de Gravação Arquivo
                str += Helper.FormataTexto(ValorVazio, 69); // BRANCOS


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

                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Helper.FormataInteiro("1", 1); //Tipo de Registro
                str += Helper.FormataTexto("C", 1); //Tipo da Operação
                str += Helper.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //TIPO DE PAGTO
                str += Helper.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Helper.FormataInteiro("040", 3); //Número da Versão do Lote
                str += Helper.FormataTexto(ValorVazio, 1); //branco
                str += Helper.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Helper.FormataTexto(ValorVazio, 4); //IDENTIFICAÇÃO DO LANÇAMENTO NO EXTRATO DO FAVORECIDO
                str += Helper.FormataTexto(ValorVazio, 16); //BRANCOS 
                str += Helper.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Helper.FormataInteiro(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Agência/Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Helper.FormataTexto(ValorVazio, 30); //FINALIDADE DO LOTE 
                str += Helper.FormataTexto(ValorVazio, 10); //HISTÓRICO DE C/C DEBITADA 
                str += Helper.FormataTexto(ValorVazio, 30); //Endereço 
                str += Helper.FormataInteiro(ValorVazio, 5); //Número 
                str += Helper.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Helper.FormataTexto(ValorVazio, 20); //Cidade 
                str += Helper.FormataInteiro(ValorVazio, 8); //CEP 
                str += Helper.FormataTexto(ValorVazio, 2); //UF 
                str += Helper.FormataTexto(ValorVazio, 8); //Branco 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

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

                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Helper.FormataInteiro("1", 1); //Tipo de Registro
                str += Helper.FormataTexto("C", 1); //Tipo da Operação
                str += Helper.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //TIPO DE PAGTO
                str += Helper.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Helper.FormataInteiro("030", 3); //Número da Versão do Lote
                str += Helper.FormataTexto(ValorVazio, 1); //branco
                str += Helper.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Helper.FormataTexto(ValorVazio, 4); //IDENTIFICAÇÃO DO LANÇAMENTO NO EXTRATO DO FAVORECIDO
                str += Helper.FormataTexto(ValorVazio, 16); //BRANCOS 
                str += Helper.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Conta
                str += Helper.FormataInteiro(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Agência/Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Helper.FormataTexto(ValorVazio, 30); //FINALIDADE DO LOTE 
                str += Helper.FormataTexto(ValorVazio, 10); //HISTÓRICO DE C/C DEBITADA 
                str += Helper.FormataTexto(ValorVazio, 30); //Endereço 
                str += Helper.FormataInteiro(ValorVazio, 5); //Número 
                str += Helper.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Helper.FormataTexto(ValorVazio, 20); //Cidade 
                str += Helper.FormataInteiro(ValorVazio, 8); //CEP 
                str += Helper.FormataTexto(ValorVazio, 2); //UF 
                str += Helper.FormataTexto(ValorVazio, 8); //Branco 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

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

                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("A", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataInteiro("0", 3); //Tipo de Movimento
                str += Helper.FormataInteiro(codCamaraCentralizadora.ToString(), 3); //Código Câmara Compensação (018 = TED CIP / 700 = DOC)
                str += Helper.FormataInteiro(Transferencia.codBancoFavorecido.ToString(), 3); //Código do Banco Favorecido
                str += Helper.FormataTexto(Transferencia.ContaCorrenteFavorecido, 20); //AGÊNCIA e CONTA FAVORECIDO
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Transferencia.nmFavorecido), 30); //Nome do Favorecido
                str += Helper.FormataTexto(Transferencia.SeuNumero, 20); //Nro. do Documento Cliente
                str += Helper.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Helper.FormataTexto("BRL", 3); //Tipo da Moeda
                str += Helper.FormataInteiro(ValorVazio, 8); //CÓDIGO ISPB
                str += Helper.FormataInteiro(ValorVazio, 7); //ZEROS 
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(Transferencia.Valor), 15); //Valor do Pagamento
                str += Helper.FormataTexto(Transferencia.NossoNumero, 15); //Nro. do Documento Banco
                str += Helper.FormataTexto(ValorVazio, 5); //Branco 
                str += Helper.FormataInteiro(ValorVazio, 8); //DATA REAL EFETIVAÇÃO DO PAGTO 
                str += Helper.FormataInteiro(ValorVazio, 15); //VALOR REAL EFETIVAÇÃO DO PAGTO 
                str += Helper.FormataTexto(ValorVazio, 18); //INFORMAÇÃO COMPLEMENTAR P/ HIST. DE C/C
                str += Helper.FormataTexto(ValorVazio, 2); //BRANCOS 
                str += Helper.FormataInteiro(ValorVazio, 6); //Nº DO DOC/TED/ OP/ CHEQUE NO RETORNO 
                str += Helper.FormataInteiro(Transferencia.CpfCnpjFavorecido, 14); //N DE INSCRIÇÃO DO FAVORECIDO (CPF/CNPJ) 
                str += Helper.FormataTexto(Transferencia.FinalidadeDoc, 2); //Finalidade do DOC
                str += Helper.FormataTexto(Transferencia.FinalidadeTed, 5); //Finalidade de TED
                str += Helper.FormataTexto(ValorVazio, 5); //BRANCOS 
                str += Helper.FormataTexto(Transferencia.EmissaoAvisoFavorecido, 1); //Emissão de Aviso ao Favorecido
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno


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

                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("B", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataTexto(ValorVazio, 3); //Brancos 
                str += Helper.FormataInteiro(Transferencia.tipInscricaoFavorecido.ToString(), 1); //Tipo de Inscrição do Favorecido
                str += Helper.FormataInteiro(Transferencia.CpfCnpjFavorecido, 14); //CNPJ/CPF do Favorecido
                str += Helper.FormataTexto(ValorVazio, 30); //Logradouro do Favorecido
                str += Helper.FormataInteiro(ValorVazio, 5); //Número do Local do Favorecido
                str += Helper.FormataTexto(ValorVazio, 15); //Complemento do Local Favorecido
                str += Helper.FormataTexto(ValorVazio, 15); //Bairro do Favorecido
                str += Helper.FormataTexto(ValorVazio, 20); //Cidade do Favorecido
                str += Helper.FormataInteiro(ValorVazio, 8); //CEP do Favorecido
                str += Helper.FormataTexto(ValorVazio, 2); //Estado do Favorecido
                str += Helper.FormataTexto(ValorVazio, 100); //ENDEREÇO DE E-MAIL
                str += Helper.FormataTexto(ValorVazio, 3); //BRANCOS 
                str += Helper.FormataTexto(ValorVazio, 10); //CÓDIGO DE OCORRÊNCIAS NO RETORNO


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

                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataInteiro("0", 3); //Tipo de Movimento
                str += Helper.FormataTexto(Helper.FormataCodigoBarraPagamento(Trans.codigoBarras), 44);     //Código de Barras 
                str += Helper.FormataTexto(Cedente.nmOperadora, 30);   //Nome do Cedente
                str += Helper.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(Trans.Valor), 15); //Valor Nominal do Título
                str += Helper.FormataInteiro(Trans.vlDesconto.ToString(), 15); //Valor Desconto + Abatimento
                str += Helper.FormataInteiro(Trans.vlJurosMulta.ToString(), 15); //Valor Multa + Juros
                str += Helper.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Helper.FormataInteiro(Trans.vlTotalPagar.ToString(), 15); //Valor do Pagamento
                str += Helper.FormataInteiro(ValorVazio, 15); // Quantidade de Moeda
                str += Helper.FormataTexto(Trans.SeuNumero, 20); //Número do Documento Cliente
                str += Helper.FormataTexto(ValorVazio, 13); //Número do Documento Banco
                str += Helper.FormataTexto(ValorVazio, 15); //Brancos 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

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

                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("5", 1); //Tipo de Registro 
                str += Helper.FormataTexto(ValorVazio, 9);    //Brancos 
                str += Helper.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Helper.FormataInteiro(TotalValorPagar.ToString(), 18); //Somatória dos Valores
                str += Helper.FormataInteiro(ValorVazio, 18); //ZEROS 
                str += Helper.FormataTexto(ValorVazio, 171); //BRANCOS 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

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
                str += Helper.FormataInteiro("341", 3); //Código do Banco 
                str += Helper.FormataInteiro("9999", 4); //Lote de Serviço 
                str += Helper.FormataInteiro("9", 1); //Tipo de Registro 
                str += Helper.FormataTexto(ValorVazio, 9);    //Branco
                str += Helper.FormataInteiro(TotaLote.ToString(), 6); //Quantidade de lotes do arquivo
                str += Helper.FormataInteiro(TotalReg.ToString(), 6); //Quantidade de registros no arquivo
                str += Helper.FormataTexto(ValorVazio, 211); //Brancos 


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do arquivo de remessa do CNAB240.", ex);
            }
        }

    }
}
