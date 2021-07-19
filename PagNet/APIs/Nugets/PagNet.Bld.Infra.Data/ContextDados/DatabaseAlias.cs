namespace PagNet.Bld.Infra.Data.ContextDados
{
    /// <summary>
    /// Classe para recuperar as configurações de de->para de aliases para bancos de dados no arquivo appsettings.json 
    /// /// </summary> 
    public class DatabaseAlias
    {
        public string Name { get; set; }
        public string Ip { get; set; }
    }
}
