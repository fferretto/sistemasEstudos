using System;
using Telenet.Core.DependencyInjection;

/// <summary>
/// Implementa a página ancestral para páginas que hospedam serviços.
/// </summary>
public class TelenetServicePage<TService, TPageConfig> : TelenetPage<TPageConfig>
{
    /// <summary>
    /// Cria uma nova instância da página.
    /// </summary>
    public TelenetServicePage()
        : base()
	{ }

    /// <summary>
    /// Obtém o objeto com o serviço hospedado pela página.
    /// </summary>
    protected TService Service { get; private set; }

    /// <summary>
    /// Executa as operações de pré inicialização da página atual.
    /// </summary>
    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        Service = ServiceConfiguration.ServiceProvider.GetService<TService>();
    }
}