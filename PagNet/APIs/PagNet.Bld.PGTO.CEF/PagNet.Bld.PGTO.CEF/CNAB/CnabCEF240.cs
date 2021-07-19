using PagNet.Bld.PGTO.CEF.Abstraction.Model;
using PagNet.Bld.PGTO.CEF.ModelAuxiliar;
using PagNet.Bld.PGTO.CEF.Util;
using System;

namespace PagNet.Bld.PGTO.CEF.CNAB
{
    public class CnabCEF240
    {
        private readonly string ValorVazio = string.Empty;

        public string HeaderArquivo(mdCedente Cedente)
        {
            try
            {
                string str = ValorVazio;
                str += Helper.FormataInteiro("104", 3); //Código do Banco na Compensação 
                str += Helper.FormataInteiro("0", 4); //Lote de Serviço 
                str += Helper.FormataInteiro("0", 1); //Tipo de Registro 
                str += Helper.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN / CNAB -Branco
                str += Helper.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa 
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 14); //CNPJ/CPF
                str += Helper.FormataInteiro(Cedente.CodConvenioPag, 6); //Código do Convenio no Banco
                str += Helper.FormataInteiro(Cedente.ParametroTransmissao, 2); // Parâmetro de transmissão - preencher com o código informado pelo Banco. 
                str += Helper.FormataTexto("P", 1); // Ambiente Cliente   “T” – teste “P” – produção              
                str += Helper.FormataTexto(ValorVazio, 1); // Parâmetro de transmissão - preencher com o código informado pelo Banco. 
                str += Helper.FormataTexto(ValorVazio, 3); //Origem Aplicativo 
                str += Helper.FormataInteiro(ValorVazio, 4); //Número da Versão
                str += Helper.FormataTexto(ValorVazio, 3);    //Uso Exclusivo FEBRABAN / CNAB -Branco
                str += Helper.FormataInteiro(Cedente.Agencia, 5);     //Agência Mantenedora da Conta
                str += Helper.FormataInteiro(Cedente.DigitoAgencia, 1);   //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Cedente.ContaCorrente, 12); //Número da Conta Corrente
                str += Helper.FormataTexto(Cedente.DigitoContaCorrente, 1); //Dígito Verificador da Conta
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência / Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Cedente.nmOperadora), 30); //Nome da Empresa
                str += Helper.FormataTexto("CAIXA", 30); //Nome do Banco
                str += Helper.FormataTexto(ValorVazio, 10); //Brancos 
                str += Helper.FormataInteiro("1", 1); //Código Remessa / Retorno 1=Remessa / 2=Retorno
                str += Helper.FormataInteiro(DateTime.Now.ToString("ddMMyyyy"), 8); //Data da Geração do Arquivo
                str += Helper.FormataInteiro(DateTime.Now.ToString("hhmmss"), 6); //Hora da Geração do Arquivo
                str += Helper.FormataInteiro(Cedente.NumSeq.ToString(), 6); //Número Sequencial do Arquivo
                str += Helper.FormataInteiro("080", 3); //Número da Versão do Layout
                str += Helper.FormataInteiro("01600", 5); //Densidade de Gravação Arquivo
                str += Helper.FormataTexto(ValorVazio, 20); //Reservado do Banco  
                str += Helper.FormataTexto(ValorVazio, 20); //Uso Reservado da Empresa
                str += Helper.FormataTexto(ValorVazio, 11); //Uso Exclusivo FEBRABAN / CNAB
                str += Helper.FormataTexto(ValorVazio, 3); //Ident. Cobrança 
                str += Helper.FormataInteiro(ValorVazio, 3); //Uso exclusivo das VAN 
                str += Helper.FormataTexto(ValorVazio, 2); //Tipo de serviço 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrência Cob. Sem papel 

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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Helper.FormataInteiro("1", 1); //Código do Registro 
                str += Helper.FormataTexto("C", 1); //Tipo da Operação
                str += Helper.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Helper.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Helper.FormataInteiro("041", 3); //Versão do leiaute do lote
                str += Helper.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Helper.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Helper.FormataInteiro(Cedente.CodConvenioPag.ToString(), 6); //Código do Convenio no Banco
                str += Helper.FormataInteiro("01", 2); //Tipo de Compromisso
                str += Helper.FormataInteiro("error", 4); //Código do compromisso 
                str += Helper.FormataTexto("error", 2); //Parâmetro de transmissão 
                str += Helper.FormataTexto(ValorVazio, 6); //Filler 
                str += Helper.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Helper.FormataTexto(Cedente.DigitoAgencia.ToString(), 1); //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Helper.FormataTexto(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Conta
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Helper.FormataTexto(ValorVazio, 40); //Informação 1 - Mensagem
                str += Helper.FormataTexto(ValorVazio, 30); //Endereço 
                str += Helper.FormataInteiro(ValorVazio, 5); //Número 
                str += Helper.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Helper.FormataTexto(ValorVazio, 20); //Cidade 
                str += Helper.FormataInteiro(ValorVazio, 5); //CEP 
                str += Helper.FormataTexto(ValorVazio, 3); //Complemento do CEP
                str += Helper.FormataTexto(ValorVazio, 2); //UF 
                str += Helper.FormataTexto(ValorVazio, 8); //Uso Exclusivo FEBRABAN/CNAB  
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("A", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataInteiro(Cedente.codMovimento.ToString(), 1); //Tipo de Movimento
                str += Helper.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Helper.FormataInteiro(codCamaraCentralizadora.ToString(), 3); //Código Câmara Compensação (018 = TED CIP / 700 = DOC)
                str += Helper.FormataInteiro(Transferencia.codBancoFavorecido.ToString(), 3); //Código do Banco Favorecido
                str += Helper.FormataInteiro(Transferencia.AgenciaFavorecido, 5); //Código da Agência Favorecido
                str += Helper.FormataTexto(Transferencia.DigitoAgenciaFavorecido, 1); //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Transferencia.ContaCorrenteFavorecido, 12); //Conta Corrente do Favorecido
                str += Helper.FormataTexto(Transferencia.DigitoContaFavorecido, 1); //Dígito Verificador da Conta
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Transferencia.nmFavorecido), 30); //Nome do Favorecido
                str += Helper.FormataInteiro(Transferencia.SeuNumero, 6); //Nro. do Documento 
                str += Helper.FormataTexto(ValorVazio, 13); //Filler 
                str += Helper.FormataTexto("1", 1); //Tipo de conta – Finalidade TED 
                str += Helper.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Helper.FormataTexto("BRL", 3); //Tipo da Moeda
                str += Helper.FormataInteiro("0", 15); //Quantidade de Moeda
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(Transferencia.Valor), 15); //Valor do Pagamento
                str += Helper.FormataInteiro(ValorVazio, 9); //Nro. do Documento Banco
                str += Helper.FormataTexto(ValorVazio, 3); //Filler 
                str += Helper.FormataInteiro("01", 2); //Quantidade de Parcelas 
                str += Helper.FormataTexto(ValorVazio, 1); //Indicador de bloqueio 
                str += Helper.FormataInteiro(ValorVazio, 1); //Indicador forma parcelamento
                str += Helper.FormataTexto(ValorVazio, 2); //Período ou dia vencimento 
                str += Helper.FormataInteiro(ValorVazio, 2); //Número Parcela 
                str += Helper.FormataInteiro(ValorVazio, 8); //Data da Efetivação 
                str += Helper.FormataInteiro(ValorVazio, 15); //Valor Real Efetivado 
                str += Helper.FormataTexto(Transferencia.Mensagem, 40); //Informação 2 - Mensagem
                str += Helper.FormataInteiro(Transferencia.FinalidadeDoc, 2); //Finalidade do DOC
                str += Helper.FormataTexto(ValorVazio, 10); //Uso FEBRABAN 
                str += Helper.FormataInteiro(Transferencia.EmissaoAvisoFavorecido, 1); //Emissão de Aviso ao Favorecido
                str += Helper.FormataTexto(Transferencia.OcorrenciaRetorno, 10); //Ocorrências para o Retorno
                
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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("B", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataTexto(ValorVazio, 3); //Uso Exclusivo FEBRABAN/CNAB 
                str += Helper.FormataInteiro(Transferencia.tipInscricaoFavorecido.ToString(), 1); //Tipo de Inscrição do Favorecido
                str += Helper.FormataInteiro(Transferencia.CpfCnpjFavorecido, 14); //CNPJ/CPF do Favorecido
                str += Helper.FormataTexto(ValorVazio, 30); //Logradouro do Favorecido
                str += Helper.FormataInteiro(ValorVazio, 5); //Número do Local do Favorecido
                str += Helper.FormataTexto(ValorVazio, 15); //Complemento do Local Favorecido
                str += Helper.FormataTexto(ValorVazio, 15); //Bairro do Favorecido
                str += Helper.FormataTexto(ValorVazio, 20); //Cidade do Favorecido
                str += Helper.FormataInteiro(ValorVazio, 5); //CEP do Favorecido
                str += Helper.FormataTexto(ValorVazio, 3); //Complemento CEP do Favorecido
                str += Helper.FormataTexto(ValorVazio, 2); //Estado do Favorecido
                str += Helper.FormataInteiro(Transferencia.dtRealPagamento.ToString("ddMMyyyy"), 8); //Data de Vencimento
                str += Helper.FormataInteiro(ValorVazio, 15); //Valor do Documento
                str += Helper.FormataInteiro(ValorVazio, 15); //Valor do Abatimento
                str += Helper.FormataInteiro(ValorVazio, 15); //Valor do Desconto
                str += Helper.FormataInteiro(ValorVazio, 15); //Valor da Mora
                str += Helper.FormataInteiro(ValorVazio, 15); //Valor da Multa
                str += Helper.FormataTexto(ValorVazio, 15); //Código/Documento do Favorecido
                str += Helper.FormataTexto(ValorVazio, 15); //Uso Exclusivo para o SIAPE

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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("5", 1); //Tipo de Registro 
                str += Helper.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB 
                str += Helper.FormataInteiro(TotRegistros.ToString(), 6); //Quantidade de Registros do Lote
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(TotalValorPagar), 18); //Somatória dos Valores
                str += Helper.FormataInteiro("0", 18); //Somatória Quantidade Moeda 
                str += Helper.FormataInteiro("0", 6);     //Número Aviso de Débito
                str += Helper.FormataTexto(ValorVazio, 165);   //Uso Exclusivo FEBRABAN/CNAB 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno 

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
                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro("9999", 4); //Lote de Serviço 
                str += Helper.FormataInteiro("9", 1); //Tipo de Registro 
                str += Helper.FormataTexto(ValorVazio, 9);    //Uso Exclusivo FEBRABAN/CNAB
                str += Helper.FormataInteiro(TotaLote.ToString(), 6); //Quantidade de lotes do arquivo
                str += Helper.FormataInteiro(TotalReg.ToString(), 6); //Quantidade de registros no arquivo
                str += Helper.FormataInteiro(ValorVazio, 6); //Qtde de Contas p/ Conc. (Lotes) 
                str += Helper.FormataTexto(ValorVazio, 205); //Brancos 


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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataInteiro("0", 1); //Tipo de Movimento
                str += Helper.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Helper.FormataTexto(Helper.FormataCodigoBarraPagamento(Trans.codigoBarras), 44);     //Código de Barras 
                str += Helper.FormataTexto(Cedente.nmOperadora, 30);   //Nome do Cedente
                str += Helper.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(Trans.Valor), 15); //Valor Nominal do Título
                str += Helper.FormataInteiro(Trans.vlDesconto.ToString(), 15); //Valor Desconto + Abatimento
                str += Helper.FormataInteiro(Trans.vlJurosMulta.ToString(), 15); //Valor Multa + Juros
                str += Helper.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(Trans.vlTotalPagar), 15); //Valor do Pagamento
                str += Helper.FormataInteiro(ValorVazio, 15); // Quantidade de Moeda
                str += Helper.FormataTexto(Trans.SeuNumero, 6); //Número do Documento Cliente
                str += Helper.FormataTexto(ValorVazio, 14); //Branco
                str += Helper.FormataTexto(Trans.NossoNumero, 9); //Número do Documento Banco
                str += Helper.FormataTexto(ValorVazio, 11); //Branco
                str += Helper.FormataInteiro("0", 2); //Código da Moeda
                str += Helper.FormataTexto(ValorVazio, 6); //Brancos 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("J", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataTexto(ValorVazio, 1); //Brancos 
                str += Helper.FormataTexto("", 2); //Código de Movimento Remessa
                str += Helper.FormataInteiro("52", 2); //Identificação Registro Opcional 
                //Dados Pagador
                str += Helper.FormataInteiro("2", 1);
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 15);
                str += Helper.FormataTexto(Cedente.nmOperadora, 40);
                //dados Beneficiario
                str += Helper.FormataInteiro("2", 1);
                str += Helper.FormataInteiro(Trans.CpfCnpj, 15);
                str += Helper.FormataTexto(Trans.nmFavorecido, 40);
                //Sacador
                str += Helper.FormataInteiro("2", 1);
                str += Helper.FormataInteiro(Trans.CpfCnpj, 15);
                str += Helper.FormataTexto(Trans.nmFavorecido, 40);

                str += Helper.FormataInteiro(ValorVazio, 53);

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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço 
                str += Helper.FormataInteiro("3", 1); //Tipo de Registro 
                str += Helper.FormataInteiro(NumSeq.ToString(), 5);    //Número Sequencial do Registro no Lote
                str += Helper.FormataTexto("O", 1); //Código Segmento do Registro Detalhe 
                str += Helper.FormataInteiro("0", 1); //Tipo de Movimento
                str += Helper.FormataInteiro("00", 2); //Código de Instrução para Movimento
                str += Helper.FormataTexto(Helper.FormataCodigoBarraPagConcessionario(Trans.codigoBarras), 44);     //Código de Barras 
                str += Helper.FormataTexto(Trans.nmFavorecido, 30);   //Nome do Cedente
                str += Helper.FormataInteiro(Trans.dtVencimento.ToString("ddMMyyyy"), 8); //Data do Vencimento
                str += Helper.FormataInteiro(Trans.dtPagamento.ToString("ddMMyyyy"), 8); //Data do Pagamento
                str += Helper.FormataInteiro(Helper.TrataValorMonetario(Trans.vlTotalPagar), 15); //Valor Total do Pagamento
                str += Helper.FormataTexto(Trans.SeuNumero, 20); //Número do Documento Cliente
                str += Helper.FormataTexto(Trans.NossoNumero, 20); //Número do Documento Banco
                str += Helper.FormataTexto(ValorVazio, 68); //Brancos 
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno


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

                str += Helper.FormataInteiro("104", 3); //Código do Banco 
                str += Helper.FormataInteiro(NumLot.ToString(), 4); //Lote de Serviço
                str += Helper.FormataInteiro("1", 1); //Código do Registro 
                str += Helper.FormataTexto("C", 1); //Tipo da Operação
                str += Helper.FormataInteiro(Transacoes.TipoServico.ToString(), 2); //Tipo de Serviço
                str += Helper.FormataInteiro(Transacoes.codFormaLancamento.ToString(), 2); //Forma de Lançamento
                str += Helper.FormataInteiro("041", 3); //Versão do leiaute do lote
                str += Helper.FormataTexto(ValorVazio, 1); //Uso Exclusivo da FEBRABAN/CNAB
                str += Helper.FormataInteiro("2", 1); //Tipo de Inscrição da Empresa
                str += Helper.FormataInteiro(Cedente.CpfCNPJ, 14); //Número de Inscrição da Empresa
                str += Helper.FormataInteiro(Cedente.CodConvenioPag.ToString(), 6); //Código do Convenio no Banco
                str += Helper.FormataInteiro("01", 2); //Tipo de Compromisso
                str += Helper.FormataInteiro("error", 4); //Código do compromisso 
                str += Helper.FormataTexto("error", 2); //Parâmetro de transmissão 
                str += Helper.FormataTexto(ValorVazio, 6); //Filler 
                str += Helper.FormataInteiro(Cedente.Agencia.ToString(), 5); //Agência Mantenedora da Conta
                str += Helper.FormataTexto(Cedente.DigitoAgencia.ToString(), 1); //Dígito Verificador da Agência
                str += Helper.FormataInteiro(Cedente.ContaCorrente.ToString(), 12); //Número da Conta Corrente
                str += Helper.FormataTexto(Cedente.DigitoContaCorrente.ToString(), 1); //Dígito Verificador da Conta
                str += Helper.FormataTexto(ValorVazio, 1); //Dígito Verificador da Agência/Conta
                str += Helper.FormataTexto(Helper.RemoveCaracterEspecial(Cedente.nmOperadora.ToString()), 30); //Nome da Empresa
                str += Helper.FormataTexto(ValorVazio, 40); //Informação 1 - Mensagem
                str += Helper.FormataTexto(ValorVazio, 30); //Endereço 
                str += Helper.FormataInteiro(ValorVazio, 5); //Número 
                str += Helper.FormataTexto(ValorVazio, 15); //Complemento do Endereço
                str += Helper.FormataTexto(ValorVazio, 20); //Cidade 
                str += Helper.FormataInteiro(ValorVazio, 5); //CEP 
                str += Helper.FormataTexto(ValorVazio, 3); //Complemento do CEP
                str += Helper.FormataTexto(ValorVazio, 2); //UF 
                str += Helper.FormataTexto(ValorVazio, 8); //Uso Exclusivo FEBRABAN/CNAB  
                str += Helper.FormataTexto(ValorVazio, 10); //Ocorrências para o Retorno

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar header do lote de remessa do CNAB240 Segmento J.", ex);
            }
        }

    }
}
