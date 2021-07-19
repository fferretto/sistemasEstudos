#pragma warning disable 1591

using System.Collections.Generic;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    public class ConsultasCarga : IConsultasCarga
    {
        public ConsultasCarga(IContextoConsultaCarga contexto, IAcessoDados acessoDados)
        {
            _contexto = contexto;
            _acessoDados = acessoDados;
        }

        private IAcessoDados _acessoDados;
        private IContextoConsultaCarga _contexto;

        private bool ConfiguraFiltro(FiltroCartao filtro, out string campo, out string valor)
        {
            campo = string.Empty;
            valor = string.Empty;

            if (!string.IsNullOrEmpty(filtro.Cpf))
            {
                campo = "CPFUSUARIO";
                valor = filtro.Cpf;
            }
            else if (!string.IsNullOrEmpty(filtro.NomeUsuario))
            {
                campo = "NOMECLIENTE";
                valor = filtro.NomeUsuario;
            }
            else if (!string.IsNullOrEmpty(filtro.Numero))
            {
                campo = "NUMEROCARTAO";
                valor = filtro.Numero;
            }
            else
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ICargaSolicitada> CargasSolicitadas()
        {
            return _acessoDados.ObterCargasSolicitadas(_contexto.CodigoCliente);
        }

        public bool CpfValidoParaTroca(string cpf, out int erro, out string mensagem)
        {
            return _acessoDados.CpfValidoParaTrocar(_contexto.CodigoCliente, cpf, out erro, out mensagem);
        }

        public bool CpfTemporarioValidoParaTroca(string cpf, out int erro, out string mensagem)
        {
            return _acessoDados.CpfTemporarioValidoParaTrocar(_contexto.CodigoCliente, cpf, out erro, out mensagem);
        }

        public ICartaoUsuario ObterCartao(FiltroCartao filtro)
        {
            var campo = string.Empty;
            var valor = string.Empty;

            if (ConfiguraFiltro(filtro, out campo, out valor))
            {
                return _acessoDados.ObterCartaoUsuario(_contexto.CodigoCliente, campo, valor);
            }

            return null;
        }

        public IEnumerable<string> LookupPara(FiltroCartao filtro)
        {
            var campo = string.Empty;
            var valor = string.Empty;

            if (ConfiguraFiltro(filtro, out campo, out valor))
            {
                return _acessoDados.LookupPara(_contexto.CodigoCliente, campo, valor);
            }

            return null;
        }

        public bool ClientePodeTrocarCpf()
        {
            return _acessoDados.ClientePodeTrocarCpf(_contexto.CodigoCliente);
        }
    }
}

#pragma warning restore 1591
