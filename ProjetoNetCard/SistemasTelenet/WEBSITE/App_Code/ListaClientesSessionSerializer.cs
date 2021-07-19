using Newtonsoft.Json;
using SIL;
using System.Collections.Generic;
using System.Linq;
using TELENET.SIL.PO;

/// <summary>
/// Descrição resumida de ListaClientesSessionSerializer
/// </summary>
public class ListaClientesSessionSerializer : SessionAbstractDataItemSerializer
{
    public ListaClientesSessionSerializer()
        : base()
    { }

    public override object Deserializer(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        var lista = JsonConvert.DeserializeObject<List<CLIENTEVA_PREPAGO>>(json);
        return new List<IDadosBasicosCliente>(lista.OfType<IDadosBasicosCliente>());
    }

    public override string Serializer()
    {
        if (Instance == null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(Instance);
    }
}