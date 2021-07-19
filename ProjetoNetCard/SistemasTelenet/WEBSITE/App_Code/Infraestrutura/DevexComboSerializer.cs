using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using SIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Descrição resumida de DevexComboSerializer
/// </summary>
public class DevexComboSerializer : ICustomSerializer
{
    private class DadosCombo
    {
        public string ID { get; set; }
        public string Width { get; set; }
        public bool Native { get; set; }
        public string CssClass { get; set; }
        public string ClientInstanceName { get; set; }
        public string ClientSideEvents { get; set; }
    }

    public object Deserialize(Type type, string text)
    {
        var dadosCombo = JsonConvert.DeserializeObject<DadosCombo>(text);
        var combo = new ASPxComboBox();

        if (dadosCombo == null)
        {
            return combo;
        }

        combo.ID = dadosCombo.ID;
        combo.Width = JsonConvert.DeserializeObject<Unit>(dadosCombo.Width);
        combo.Native = dadosCombo.Native;
        combo.CssClass = dadosCombo.CssClass;
        combo.ClientInstanceName = dadosCombo.ID;
        combo.ClientSideEvents.SelectedIndexChanged = dadosCombo.ClientSideEvents;

        return combo;
    }

    public string Serialize(Type type, object obj)
    {
        var aspCombo = obj as ASPxComboBox;

        if (aspCombo == null)
        {
            return string.Empty;
        }

        var combo = new DadosCombo
        {
            ID = aspCombo.ID,
            Width = JsonConvert.SerializeObject(aspCombo.Width),
            Native = aspCombo.Native,
            CssClass = aspCombo.CssClass,
            ClientInstanceName = aspCombo.ID,
            ClientSideEvents = aspCombo.ClientSideEvents.SelectedIndexChanged
        };

        return JsonConvert.SerializeObject(combo);
    }
}