using Telenet.Carga.Abstractions;

/// <summary>
/// Summary description for ResumoErro
/// </summary>
public class ResumoErro : IResumoCarga
{
    public int CodigoCliente { get; set; }

    public string IdCarga { get; set; }

    public int NumeroCarga { get; set; }

    public string RegistroLog { get; set; }

    public char TipoRegistro { get; set; }
}