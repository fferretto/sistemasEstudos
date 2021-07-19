#pragma warning disable 1591

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Telenet.Carga.Abstractions
{
    public class ObterLogCargaCartao
    {
        public ObterLogCargaCartao(string pasta, string nomeOperadora, string nomeArquivoCarga, IEnumerable<IResumoCarga> resumoCarga)
        {
            pasta = Path.Combine(Path.Combine(pasta, nomeOperadora).Replace(" ", "_"), "Logs");

            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            _filename = Path.Combine(pasta, string.Concat(Path.GetFileNameWithoutExtension(nomeArquivoCarga), ".log"));
            _resumoCarga = resumoCarga;
        }

        private string _filename;
        private IEnumerable<IResumoCarga> _resumoCarga;

        public string NomeArquivoLog { get { return _filename; } }

        public void SalvarLog()
        {
            var resumoCarga = _resumoCarga
                .Where(r => r.TipoRegistro == 'L')
                .ToList();

            if (!resumoCarga.Any())
            {
                return;
            }

            using (var conteudoLog = new MemoryStream())
            using (var arquivoLog = new StreamWriter(conteudoLog, Encoding.UTF8))
            {
                resumoCarga.ForEach(l => arquivoLog.WriteLine(l.RegistroLog));
                arquivoLog.Flush();
                conteudoLog.Position = 0;
                File.WriteAllBytes(_filename, conteudoLog.ToArray());
            }
        }
    }
}

#pragma warning restore 1591