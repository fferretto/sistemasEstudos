using PagNet.Application.Helpers;
using PagNet.Application.Models;
using System;

namespace PagNet.Application.Cnab.BancoCEF
{
    public class CnabCEF240
    {
        private readonly string ValorVazio = string.Empty;

        public string HeaderArquivo(mdCedente Cedente)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("104", 3); //Código do Banco na Compensação 
                str += Geral.FormataInteiro("0", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("0", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN / CNAB -Branco
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Geral.FormataInteiro(Cedente.CodConvenioPag, 6); //Código do Convenio no Banco
                str += Geral.FormataInteiro(Cedente.ParametroTransmissao, 2); // Parâmetro de transmissão - preencher com o código informado pelo Banco. 
                str += Geral.FormataTexto("P", 1); // Ambiente Cliente   “T” – teste “P” – produção              
                str += Geral.FormataTexto(ValorVazio, 1); // Parâmetro de transmissão - preencher com o código informado pelo Banco. 
                str += Geral.FormataTexto(ValorVazio, 3); //Origem Aplicativo 
                str += Geral.FormataInteiro(ValorVazio, 4); //Número da Versão
                str += Geral.FormataTexto(ValorVazio, 3);    //Uso Exclusivo FEBRABAN / CNAB -Branco
                str += Geral.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Geral.FormataInteiro(Cedente.DigitoAgencia, 1);   //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Geral.FormataTexto(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência / Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Cedente.nmOperadora), 30); //Nome da Empresa
                str += Geral.FormataTexto("CAIXA", 30); //Nome do Banco
                str += Geral.FormataTexto(ValorVazio, 10); //Brancos 
                str += Geral.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Geral.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Geral.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Geral.FormataInteiro(Cedente.NumSeq.ToString(), 6); //Número Sequencial do Arquivo
                str += Geral.FormataInteiro("080", 3); //Número da Versão do Layout
                str += Geral.FormataInteiro("01600", 5); //Densidade de Gravação Arquivo
                str += Geral.FormataTexto(ValorVazio, 20); //Reservado do Banco  
                str += Geral.FormataTexto(ValorVazio, 20); //Uso Reservado da Empresa
                str += Geral.FormataTexto(ValorVazio, 11); //Uso Exclusivo FEBRABAN / CNAB
                str += Geral.FormataTexto(ValorVazio, 3); //Ident. Cobrança 
                str += Geral.FormataInteiro(ValorVazio, 3); //Uso exclusivo das VAN 
                str += Geral.FormataTexto(ValorVazio, 2); //Tipo de serviço 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrência Cob. Sem papel 

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

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Código do Registro 
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("041", 3); //Versão do leiaute do lote
                str += Geral.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CodConvenioPag.ToString(), 6); //Código do Convenio no Banco
                str += Geral.FormataInteiro("01", 2); //Tipo de Compromisso
                str += Geral.FormataInteiro("error", 4); //Código do compromisso 
                str += Geral.FormataTexto("error", 2); //Parâmetro de transmissão 
                str += Geral.FormataTexto(ValorVazio, 6); //Filler 
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
                str += Geral.FormataTexto(ValorVazio, 3); //Complemento do CEP
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
        public string DetalheLoteSegmento_A(mdCedente Cedente, TransacaoTransferencia Transferencia, int codCamaraCentralizadora, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("A", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro(Cedente.codMovimento.ToString(), 1); //Tipo de Movimento
                str += Geral.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Geral.FormataInteiro(codCamaraCentralizadora.ToString(), 3); //Código Câmara Compensação (018 = TED CIP / 700 = DOC)
                str += Geral.FormataInteiro(Transferencia.codBancoFavorecido.ToString(), 3); //Código do Banco Favorecido
                str += Geral.FormataInteiro(Transferencia.AgenciaFavorecido, 5); //Código da Agência Favorecido
                str += Geral.FormataTexto(Transferencia.DigitoAgenciaFavorecido, 1); //Dígito Verificador da Agência
                str += Geral.FormataInteiro(Transferencia.ContaCorrenteFavorecido, 12); //Conta Corrente do Favorecido
                str += Geral.FormataTexto(Transferencia.DigitoContaFavorecido, 1); //Dígito Verificador da Conta
                str += Geral.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Geral.FormataTexto(Geral.RemoveCaracterEspecial(Transferencia.nmFavorecido), 30); //Nome do Favorecido
                str += Geral.FormataInteiro(Transferencia.SeuNumero, 6); //Nro. do Documento 
                str += Geral.FormataTexto(ValorVazio, 13); //Filler 
                str += Geral.FormataTexto("1", 1); //Tipo de conta – Finalidade TED 
                str += Geral.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataTexto("BRL", 3); //Tipo da Moeda
                str += Geral.FormataInteiro("0", 15); //Quantidade de Moeda
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(Transferencia.Valor), 15); //Valor do Pagamento
                str += Geral.FormataInteiro(ValorVazio, 9); //Nro. do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 3); //Filler 
                str += Geral.FormataInteiro("01", 2); //Quantidade de Parcelas 
                str += Geral.FormataTexto(ValorVazio, 1); //Indicador de bloqueio 
                str += Geral.FormataInteiro(ValorVazio, 1); //Indicador forma parcelamento
                str += Geral.FormataTexto(ValorVazio, 2); //Período ou dia vencimento 
                str += Geral.FormataInteiro(ValorVazio, 2); //Número Parcela 
                str += Geral.FormataInteiro(ValorVazio, 8); //Data da Efetivação 
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor Real Efetivado 
                str += Geral.FormataTexto(Transferencia.Mensagem, 40); //Informação 2 - Mensagem
                str += Geral.FormataInteiro(Transferencia.FinalidadeDoc, 2); //Finalidade do DOC
                str += Geral.FormataTexto(ValorVazio, 10); //Uso FEBRABAN 
                str += Geral.FormataInteiro(Transferencia.EmissaoAvisoFavorecido, 1); //Emissão de Aviso ao Favorecido
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

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("B", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataTexto(ValorVazio, 3); //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataInteiro(Transferencia.tipInscricaoFavorecido.ToString(), 1); //Tipo de Inscrição do Favorecido
                str += Geral.FormataInteiro(Transferencia.CpfCnpjFavorecido, 14); //CNPJ/CPF do Favorecido
                str += Geral.FormataTexto(ValorVazio, 30); //Logradouro do Favorecido
                str += Geral.FormataInteiro(ValorVazio, 5); //Número do Local do Favorecido
                str += Geral.FormataTexto(ValorVazio, 15); //Complemento do Local Favorecido
                str += Geral.FormataTexto(ValorVazio, 15); //Bairro do Favorecido
                str += Geral.FormataTexto(ValorVazio, 20); //Cidade do Favorecido
                str += Geral.FormataInteiro(ValorVazio, 5); //CEP do Favorecido
                str += Geral.FormataTexto(ValorVazio, 3); //Complemento CEP do Favorecido
                str += Geral.FormataTexto(ValorVazio, 2); //Estado do Favorecido
                str += Geral.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data de Vencimento
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor do Documento
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor do Abatimento
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor do Desconto
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor da Mora
                str += Geral.FormataInteiro(ValorVazio, 15); //Valor da Multa
                str += Geral.FormataTexto(ValorVazio, 15); //Código/Documento do Favorecido
                str += Geral.FormataTexto(ValorVazio, 15); //Uso Exclusivo para o SIAPE

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Detalhe do lote de remessa do CNAB240 Segmento B.", ex);
            }
        }
        public string TrailerLote(int NumLot, decimal TotalValorPagar, int TotRegistros)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("5", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Geral.FormataInteiro(TotalValorPagar.ToString(), 18); //Somatória dos Valores
                str += Geral.FormataInteiro("0", 18); //Somatória Quantidade Moeda 
                str += Geral.FormataInteiro("0", 6);     //Número Aviso de Débito
                str += Geral.FormataTexto(ValorVazio, 165);   //Uso Exclusivo FEBRABAN/CNAB 
                str += Geral.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do lote de remessa do CNAB240.", ex);
            }
        }
        public string TrailerArquivo(int TotaLote, int TotalReg, int qtContas)
        {
            try
            {
                string str = ValorVazio;
                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro("9999", 4); //Lote de Serviço 
                str += Geral.FormataInteiro("9", 1); //Tipo de Registro 
                str += Geral.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB
                str += Geral.FormataInteiro(TotaLote.ToString(), 6); //Quantidade de lotes do arquivo
                str += Geral.FormataInteiro(TotalReg.ToString(), 6); //Quantidade de registros no arquivo
                str += Geral.FormataInteiro(ValorVazio, 6); //Qtde de Contas p/ Conc. (Lotes) 
                str += Geral.FormataTexto(ValorVazio, 205); //Brancos 


                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar Trailer do arquivo de remessa do CNAB240.", ex);
            }
        }
        public string DetalheLoteSegmento_J(mdCedente Cedente, TransacoesPagamento Trans, int NumLot, int NumSeq)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataInteiro("0", 1); //Tipo de Movimento
                str += Geral.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Geral.FormataTexto(Geral.FormataCodigoBarraPagamento(Trans.codigoBarras), 44);     //Código de Barras 
                str += Geral.FormataTexto(Cedente.nmOperadora, 30);   //Nome do Cedente
                str += Geral.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Geral.FormataInteiro(Geral.TrataValorMonetario(Trans.Valor), 15); //Valor Nominal do Título
                str += Geral.FormataInteiro(Trans.vlDesconto.ToString(), 15); //Valor Desconto + Abatimento
                str += Geral.FormataInteiro(Trans.vlJurosMulta.ToString(), 15); //Valor Multa + Juros
                str += Geral.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Geral.FormataInteiro(Trans.vlTotalPagar.ToString(), 15); //Valor do Pagamento
                str += Geral.FormataInteiro(ValorVazio, 15); // Quantidade de Moeda
                str += Geral.FormataTexto(Trans.SeuNumero, 6); //Número do Documento Cliente
                str += Geral.FormataTexto(ValorVazio, 14); //Branco
                str += Geral.FormataTexto(Trans.NossoNumero, 9); //Número do Documento Banco
                str += Geral.FormataTexto(ValorVazio, 11); //Branco
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

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Geral.FormataInteiro("3", 1); //Tipo de Registro 
                str += Geral.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Geral.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Geral.FormataTexto(ValorVazio, 1); //Brancos 
                str += Geral.FormataTexto("", 2); //Código de Movimento Remessa
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

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
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
        public string HeaderLotePagBoleto(mdCedente Cedente, TransacoesPagamento Transacoes, int NumLot)
        {
            try
            {
                string str = ValorVazio;

                str += Geral.FormataInteiro("104", 3); //Código do Banco 
                str += Geral.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Geral.FormataInteiro("1", 1); //Código do Registro 
                str += Geral.FormataTexto("C", 1); //Tipo da Operação
                str += Geral.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Geral.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Geral.FormataInteiro("041", 3); //Versão do leiaute do lote
                str += Geral.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Geral.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Geral.FormataInteiro(Cedente.CodConvenioPag.ToString(), 6); //Código do Convenio no Banco
                str += Geral.FormataInteiro("01", 2); //Tipo de Compromisso
                str += Geral.FormataInteiro("error", 4); //Código do compromisso 
                str += Geral.FormataTexto("error", 2); //Parâmetro de transmissão 
                str += Geral.FormataTexto(ValorVazio, 6); //Filler 
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
                str += Geral.FormataTexto(ValorVazio, 3); //Complemento do CEP
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

    }
}
