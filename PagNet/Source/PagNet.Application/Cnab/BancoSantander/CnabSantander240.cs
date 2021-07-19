using PagNet.Application.Helpers;
using PagNet.Application.Models;
//using PagNet.Domain.Entities;
using System;

namespace PagNet.Application.Cnab.BancoSantander
{
    public class CnabSantander240
    {
        private readonly string ValorVazio = string.Empty;

        public string HeaderArquivo(mdCedente Cedente)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro("0", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("0", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Geral.FormataTexto(Cedente.CodConvenioPag, 20); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Geral.FormataInteiro(Cedente.DigitoAgencia, 1);   //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Geral.FormataInteiro(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência / Conta
                str += Geral.FormataTexto(Cedente.nmOperadora, 30); //Nome da Empresa
                str += Geral.FormataTexto("Banco Santander", 30); //Nome do Banco
                str += Geral.FormataTexto(ValorVazio, 10); //Brancos 
                str += Geral.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Geral.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Geral.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Geral.FormataInteiro(Cedente.NumSeq.ToString(), 6); //Número Sequencial do Arquivo
                str += Geral.FormataInteiro("060", 3); //Número da Versão do Layout
                str += Geral.FormataInteiro("0", 5); //Densidade de Gravação Arquivo
                str += Geral.FormataTexto(ValorVazio, 20); //Brancos 
                str += Geral.FormataTexto(ValorVazio, 20); //Uso Reservado da Empresa
                str += Geral.FormataTexto(ValorVazio, 19); //Brancos
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }
        public string TrailerArquivo(int TotaLote, int TotalReg)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("033", 3); //Código do Banco 
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
        public string HeaderLote(mdCedente Cedente, TransacaoTransferencia Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("031", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataTexto(Cedente.CodConvenioPag.ToString(), 20); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(Cedente.DigitoAgencia.ToString(), 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Cedente.nmOperadora.ToString(), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 40); //Informação 1 - Mensagem
                str += Geral.FormataTexto(ValorVazio, 30); //Endereço 
                str += Geral.FormataInteiro(ValorVazio, 5); //Número 
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade 
                str += Geral.FormataInteiro(ValorVazio, 5); //CEP 
                str += Geral.FormataInteiro(ValorVazio, 3); //Complemento do CEP
                str += Geral.FormataTexto(ValorVazio, 2); //UF 
                str += Geral.FormataTexto(ValorVazio, 8); //Branco 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno




                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }
        public string HeaderLotePagBoleto(mdCedente Cedente, TransacoesPagamento Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Tipo de Registro
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("030", 3); //Número da Versão do Lote
                str += Geral.FormataTexto(ValorVazio, 1); //branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataTexto(Cedente.CodConvenioPag.ToString(), 20); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Geral.FormataTexto(Cedente.DigitoAgencia.ToString(), 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Geral.FormataTexto(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Cedente.nmOperadora.ToString(), 30); //Nome da Empresa
                str += Geral.FormataTexto(ValorVazio, 40); //Informação 1 - Mensagem
                str += Geral.FormataTexto(ValorVazio, 30); //Endereço 
                str += Geral.FormataInteiro(ValorVazio, 5); //Número 
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade 
                str += Geral.FormataInteiro(ValorVazio, 5); //CEP 
                str += Geral.FormataInteiro(ValorVazio, 3); //Complemento do CEP
                str += Geral.FormataTexto(ValorVazio, 2); //UF 
                str += Geral.FormataTexto(ValorVazio, 8); //Branco 
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

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("5", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Brancos 
                str += Geral.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Geral.FormataInteiro(TotalValorPagar.ToString(), 18); //Somatória dos Valores
                str += Geral.FormataInteiro("0", 18); //Somatória Quantidade Moeda 
                str += Geral.FormataInteiro("0", 6);     //Número Aviso de Débito
                str += Geral.FormataTexto(ValorVazio, 165);   //Brancos 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do lote de remessa do CNAB240.", ex);
            }
        }
        public string DetalheLoteSegmento_A(mdCedente Cedente, TransacaoTransferencia Transferencia, int codCamaraCentralizadora, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("A", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 1); //Tipo de Movimento
                str += Geral.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Geral.FormataInteiro(codCamaraCentralizadora.ToString(), 3); //Código Câmara Compensação (018 = TED CIP / 700 = DOC)
                str += Geral.FormataInteiro(Transferencia.codBancoFavorecido.ToString(), 3); //Código do Banco Favorecido
                str += Geral.FormataInteiro(Transferencia.AgenciaFavorecido, 5); //Código da Agência Favorecido
                str += Geral.FormataTexto(Transferencia.DigitoAgenciaFavorecido, 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Transferencia.ContaCorrenteFavorecido, 12); //Conta Corrente do Favorecido
                str += Geral.FormataTexto(Transferencia.DigitoContaFavorecido, 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto("", 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Transferencia.nmFavorecido, 30); //Nome do Favorecido
                str += Geral.FormataTexto(Transferencia.SeuNumero, 20); //Nro. do Documento Cliente
                str += Geral.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataTexto("BRL", 3); //Tipo da Moeda
                str += Geral.FormataInteiro("0", 15); //Quantidade de Moeda
                str += Geral.FormataInteiro(Transferencia.Valor.ToString(), 15); //Valor do Pagamento
                str += Geral.FormataTexto(Transferencia.NossoNumero, 20); //Nro. do Documento Banco
                str += Geral.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data Real do Pagamento (Retorno)
                str += Geral.FormataInteiro(Transferencia.Valor.ToString(), 15); //Valor Real do Pagamento
                str += Geral.FormataTexto(Transferencia.Mensagem, 40); //Informação 2 - Mensagem
                str += Geral.FormataTexto(Transferencia.FinalidadeDoc, 2); //Finalidade do DOC
                str += Geral.FormataTexto(Transferencia.FinalidadeTed, 5); //Finalidade de TED
                str += Geral.FormataTexto(Transferencia.CodFinalidadeComplementar, 2); //Código Finalidade Complementar
                str += Geral.FormataTexto(ValorVazio, 3); //Branco 
                str += Geral.FormataTexto(Transferencia.EmissaoAvisoFavorecido, 1); //Emissão de Aviso ao Favorecido
                str += Geral.FormataTexto(Transferencia.OcorrenciaRetorno, 10); //Ocorrências para o Retorno



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

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
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
                str += Geral.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data de Vencimento
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor do Documento
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor do Abatimento
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor do Desconto
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor da Mora
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor da Multa
                str += Geral.FormataInteiro(ValorVazio, 4); //Horário de Envio de TED
                str += Geral.FormataTexto(ValorVazio, 11); //Brancos 
                str += Geral.FormataInteiro("0183", 4); //Código Histórico para Crédito
                str += Geral.FormataInteiro(Transferencia.EmissaoAvisoFavorecido, 1); //Emissão de Aviso ao Favorecido
                str += Geral.FormataTexto(ValorVazio, 1); //Filler  Nota G007 
                str += Geral.FormataTexto("N", 1); //TED para Instituição Financeira
                str += Geral.FormataTexto(ValorVazio, 8); //Identificação da IF no SPB



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

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 1); //Tipo de Movimento
                str += Geral.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Geral.FormataTexto(Geral.FormataCodigoBarraPagamento(Trans.codigoBarras), 44);     //Código de Barras 
                str += Geral.FormataTexto(Cedente.nmOperadora, 30);   //Nome do Cedente
                str += Geral.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Geral.FormataInteiro(Trans.Valor.ToString(), 15); //Valor Nominal do Título
                str += Geral.FormataInteiro(Trans.vlDesconto.ToString(), 15); //Valor Desconto + Abatimento
                str += Geral.FormataInteiro(Trans.vlJurosMulta.ToString(), 15); //Valor Multa + Juros
                str += Geral.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataInteiro(Trans.vlTotalPagar.ToString(), 15); //Valor do Pagamento
                str += Geral.FormataInteiro(ValorVazio, 15); // Quantidade de Moeda
                str += Geral.FormataTexto(Trans.SeuNumero, 20); //Número do Documento Cliente
                str += Geral.FormataTexto(Trans.NossoNumero, 20); //Número do Documento Banco
                str += Geral.FormataInteiro("0", 2); //Código da Moeda
                str += Geral.FormataTexto(ValorVazio, 6); //Brancos 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno



                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }
        public string DetalheLoteSegmento_J52(mdCedente Cedente, TransacaoTransferencia Trans, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataTexto(ValorVazio, 1); //Brancos 
                str += Geral.FormataInteiro("0", 2); //Código de Movimento Remessa
                str += Geral.FormataInteiro("52", 2); //Identificação Registro Opcional 
                //Dados Pagador
                str += Geral.FormataInteiro("2", 1);
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 15);
                str += Geral.FormataTexto(Cedente.nmOperadora, 40);
                //dados Beneficiario
                str += Geral.FormataInteiro("2", 1);
                str += Geral.FormataInteiro(Trans.CpfCnpj, 15);
                str += Geral.FormataTexto(Trans.nmFavorecido, 40);
                //Sacador
                str += Geral.FormataInteiro("2", 1);
                str += Geral.FormataInteiro(Trans.CpfCnpj, 15);
                str += Geral.FormataTexto(Trans.nmFavorecido, 40);

                str += Geral.FormataInteiro(ValorVazio, 53);

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento J52.", ex);
            }
        }
        public string DetalheLoteSegmento_O(mdCedente Cedente, TransacoesPagamento Trans, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("033", 3); //Código do Banco 
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


    }
}
