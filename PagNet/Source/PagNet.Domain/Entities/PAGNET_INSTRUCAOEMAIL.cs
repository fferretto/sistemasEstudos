using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public class PAGNET_INSTRUCAOEMAIL
    {
        public int CODINSTRUCAOEMAIL { get; set; }
        public int CODEMPRESA { get; set; }
        public string ASSUNTO { get; set; }
        public string MENSAGEM { get; set; }
    }
}
