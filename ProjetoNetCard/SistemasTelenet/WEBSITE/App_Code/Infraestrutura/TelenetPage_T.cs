using System;
using Telenet.Core.DependencyInjection;

/// <summary>
/// Implementa a página ancestral para páginas com configurações pré definidas.
/// </summary>
public class TelenetPage<TPageConfig> : TelenetPage
{
    /// <summary>
    /// Cria uma nova instância da página.
    /// </summary>
	public TelenetPage()
        : base()
	{ }

    /// <summary>
    /// Obtém o objeto com as configurações pré definidas da página.
    /// </summary>
    protected TPageConfig Config  { get; private set; }

    /// <summary>
    /// Executa as operações de pré inicialização da página atual.
    /// </summary>
    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        Config = ServiceConfiguration.ServiceProvider.GetService<TPageConfig>();
    }
}