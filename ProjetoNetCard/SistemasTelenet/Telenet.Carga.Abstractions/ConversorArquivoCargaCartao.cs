#pragma warning disable 1591

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Telenet.Carga.Abstractions
{
    public sealed class ConversorArquivoCargaCartao
    {
        public ConversorArquivoCargaCartao(string pasta, string nomeOperadora, string nomeOriginal, Stream arquivo)
            : this(Path.Combine(pasta, nomeOperadora), nomeOriginal)
        {
            _bytes = ObterBytes(arquivo);
        }

        public ConversorArquivoCargaCartao(string pasta, string nomeOperadora, string nomeOriginal, byte[] arquivo)
            : this(Path.Combine(pasta, nomeOperadora), nomeOriginal)
        {
            _bytes = arquivo;
        }

        private ConversorArquivoCargaCartao(string pasta, string nomeOriginal)
        {
            NomeOriginal = nomeOriginal;
            _extensao = Path.GetExtension(nomeOriginal).ToLower();
            _pasta = pasta.Replace(" ", "_");

            if (!Directory.Exists(_pasta))
            {
                Directory.CreateDirectory(_pasta);
            }
        }

        private static string CriaArquivo(string nomeBase, string pasta, string extensao)
        {
            var i = 0;
            var naoExiste = true;
            Func<int, string> criaNome = (index) => Path.Combine(pasta, string.Concat(string.Format(nomeBase, index), extensao));
            var nomeArquivo = criaNome(++i);

            do
            {
                try
                {
                    using (var stream = File.Open(nomeArquivo, FileMode.CreateNew)) { };
                    naoExiste = false;
                }
                catch (IOException)
                {
                    nomeArquivo = criaNome(++i);
                }
            } while (naoExiste);

            return nomeArquivo;
        }

        private string CriaNomeBaseNetCard()
        {
            return string.Concat(
               Path.GetFileNameWithoutExtension(NomeOriginal.Trim()),
               "_",
               DateTime.Now.ToString("ddMMyyyyhhmmss"),
               "_",
               "carga",
               "_",
               "{0}");
        }

        private string CriaNomeBaseConsulta(int codigoCliente, string cnpj)
        {
            return string.Concat(
                codigoCliente.ToString().PadLeft(5, '0'),
                "_",
                cnpj.Trim(),
                "_",
                DateTime.Now.ToString("ddMMyyyyhhmmss"),
                "_",
                "carga",
                "_",
                "{0}");
        }

        private byte[] _bytes;
        private string _extensao;
        private string _pasta;
        private bool linhaTratada;

        private static byte[] ObterBytes(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            return buffer;
        }

        private static bool TentaRemoverAcentosMenosHifem(string str, out string novaStr)
        {
            // Não pode remover o ponto e a vírgula desta rotina, pois isto causa problemas na conversão de valores de cargas dos arquivos.
            // Importante lembrar que, com a falta do ponto e da vírgula nesta rotina o nome de usuários em cargas com inclusão de cartão
            // poderão ser incluídos com pontos e vírgulas caso contenham estes caracteres nos campos, pois eles não serão mais considerados nesta
            // rotina.
            novaStr = str;

            var acentos = new[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            var semAcento = new[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (var i = 0; i < acentos.Length; i++) { novaStr = novaStr.Replace(acentos[i], semAcento[i]); }
            string[] caracteresEspeciais = { "'", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", "#" };
            novaStr = caracteresEspeciais.Aggregate(novaStr, (current, t) => current.Replace(t, ""));
            novaStr = novaStr.Replace("^\\s+", "");
            novaStr = novaStr.Replace("\\s+$", "");
            novaStr = novaStr.Replace("\\s+", " ");

            return str != novaStr;
        }

        private bool LerArquivo(string nomeArquivo, Encoding encoding, out StringBuilder conteudo)
        {
            var encodingCorreto = false;
            Func<string, string> handler;
            conteudo = new StringBuilder();

            if (_extensao == ".csv")
            {
                handler = (l) =>
                {
                    if (TentaRemoverAcentosMenosHifem(l, out string nova))
                        encodingCorreto = true;

                    return nova.ToUpper() + Environment.NewLine;
                };
            }
            else
            {
                handler = (l) =>
                {
                    if (TentaRemoverAcentosMenosHifem(l, out string nova))
                        encodingCorreto = true;

                    return nova.ToUpper() + Environment.NewLine;
                };
            }

            using (var buffer = new MemoryStream(_bytes))
            using (var reader = new StreamReader(buffer, encoding))
            {
                string linha;
                var cont = 0;

                while ((linha = reader.ReadLine()) != null)
                {
                    cont++;
                    conteudo.Append(handler(linha));
                }
            }

            return encodingCorreto;
        }

        private string SalvaArquivo(string nomeBase)
        {
            var nomeArquivo = CriaArquivo(nomeBase, _pasta, _extensao);
            var encodings = new Encoding[] { Encoding.UTF8, Encoding.Default };
            var tratouEncoding = false;
            StringBuilder conteudo = null;

            foreach (var encoding in encodings)
            {
                // Tenta ler o arquivo com um encoding conhecido para tratamento de acentos e caracteres especiais.
                tratouEncoding = LerArquivo(nomeArquivo, encoding, out conteudo);
                if (tratouEncoding)
                    break;
            }

            // Salva o arquivo com o resultado final.
            File.WriteAllText(nomeArquivo, conteudo.ToString(), Encoding.ASCII);
            return nomeArquivo;
        }

        public string NomeOriginal { get; private set; }

        public string SalvaArquivoConsulta(int codigoCliente, string cnpj)
        {
            var nomeBase = CriaNomeBaseConsulta(codigoCliente, cnpj);
            return SalvaArquivo(nomeBase);
        }

        public string SalvaArquivoNetCard()
        {
            var nomeBase = CriaNomeBaseNetCard();
            return SalvaArquivo(nomeBase);
        }
    }
}

#pragma warning restore 1591
