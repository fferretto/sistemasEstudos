#pragma warning disable 1591

using System.Data;
using Telenet.Carga.Abstractions;
using Telenet.Core.Data;

namespace Telenet.Carga
{
    internal class CartaoUsuario : ICartaoUsuario, ILoadableObject
    {
        public string Cpf { get; private set; }

        public string CpfOrigem { get; private set; }

        public short Filial { get; private set; }

        public string Matricula { get; private set; }

        public string Numero { get; private set; }

        public string Nome { get; private set; }

        public string Setor { get; private set; }

        public void LoadFrom(IDataReader reader)
        {
            Cpf = reader.GetValue<string>("CPF");
            Filial = reader.GetValue<short>("CODFIL");
            Matricula = reader.GetValue<string>("MAT");
            Nome = reader.GetValue<string>("NOMUSU");
            Numero = reader.GetValue<string>("CODCRT");
            Setor = reader.GetValue<string>("CODSET");
        }
    }
}

#pragma warning restore 1591
