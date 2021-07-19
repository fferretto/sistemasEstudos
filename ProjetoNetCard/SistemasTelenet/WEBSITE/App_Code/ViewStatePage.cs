using System;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for ViewStatePage
/// </summary>
public abstract class ViewStatePage : TelenetPage
{
    protected override void SavePageStateToPersistenceMedium(object viewstate)
    {
        //base.SavePageStateToPersistenceMedium(state);

        string VSKEY = "VIEWSTATE_" + Session.SessionID + "_" + Request.RawUrl + "_" + DateTime.Now.Ticks.ToString();
        // verifica se o parâmetro do appsettings foi ativado
        if (ConfigurationManager.AppSettings["ServerSideViewState"].ToUpper() == "TRUE")
        {
            // verifica se o tipo de armazenamento e por CACHE ou Session
            if (ConfigurationManager.AppSettings["ViewStateStore"].ToUpper() == "CACHE")
            {
                //armazena o viewstate no CACHE
                Cache.Add(VSKEY, viewstate, null, System.DateTime.Now.AddMinutes(Session.Timeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                // armazena o viewstate na sessao
                DataTable VsDataTable;
                DataRow DbRow;

                // verifica se ja existe um datatable das viewstates na sessao
                if ((Session["__VSDataTable"] == null))
                {
                    // nao existe, cria...
                    DataColumn[] PkColumn = new DataColumn[1];
                    DataColumn DbColumn;
                    VsDataTable = new DataTable("VState");

                    //Coluna 1 - Nome: VSKey - PrimaryKey
                    DbColumn = new DataColumn("VSKey", typeof(string));
                    VsDataTable.Columns.Add(DbColumn);
                    PkColumn[0] = DbColumn;
                    VsDataTable.PrimaryKey = PkColumn;

                    //Coluna 2 - Nome: ViewStateData
                    DbColumn = new DataColumn("VSData", typeof(object));
                    VsDataTable.Columns.Add(DbColumn);

                    //Coluna 3 - Nome: DateTime
                    DbColumn = new DataColumn("DateTime", typeof(System.DateTime));
                    VsDataTable.Columns.Add(DbColumn);
                }
                else
                {
                    // o datatable das viewstates ja existe, pega ele da sessao
                    VsDataTable = (DataTable)Session["__VSDataTable"];
                }

                // verifica se ja temos uma viewstate com esta mesma chave
                // se sim, atualiza as informacoes na linha
                // do contrario, adiciona ela no datatable
                DbRow = VsDataTable.Rows.Find(VSKEY);

                if ((DbRow != null))
                {
                    // linha encontrada! atualiza as informacoes sem criar uma nova linha
                    DbRow["VsData"] = viewstate;
                }
                else
                {
                    // cria uma nova linha
                    DbRow = VsDataTable.NewRow();
                    DbRow["VSKey"] = VSKEY;
                    DbRow["VsData"] = viewstate;
                    DbRow["DateTime"] = System.DateTime.Now;
                    VsDataTable.Rows.Add(DbRow);
                }

                // isto evita que o datatable tenha um grande tamanho. 
                // apaga a primeira linha
                if (Convert.ToInt32(ConfigurationManager.AppSettings["ViewStateTableSize"]) < VsDataTable.Rows.Count)
                {
                    VsDataTable.Rows[0].Delete();
                }

                // grava o datatable na sessao
                Session["__VSDataTable"] = VsDataTable;
            }

            // registra um campo hidden da pagina
            // que ira conter somente a key que foi gerada.
            // com ela, sera possivel encontrar este viewstate armazenado
            // no nosso datatable que esta na session e, com isto, recuperar a sessao. 
            ClientScript.RegisterHiddenField("__VIEWSTATE_KEY", VSKEY);
        }
        else
        {
            // chama o processo normal, pois nao foi ativo no appsettings no web.config
            base.SavePageStateToPersistenceMedium(viewstate);
        }
    }

    protected override object LoadPageStateFromPersistenceMedium()
    {
        //Verifica se o ServerSideViewState esta ativado
        if (ConfigurationManager.AppSettings["ServerSideViewState"].ToUpper() == "TRUE")
        {

            // pega a key que esta no campo hidden criado
            string VSKey = Request.Form["__VIEWSTATE_KEY"];

            // e valida
            if (!VSKey.StartsWith("VIEWSTATE_"))
            {
                throw new Exception("Invalid VIEWSTATE Key: " + VSKey);
            }

            // verifica se utiliza CACHE
            if (ConfigurationManager.AppSettings["ViewStateStore"].ToUpper() == "CACHE")
            {
                return Cache[VSKey];
            }
            else
            {
                // ou session
                DataTable VsDataTable = (DataTable)Session["__VSDataTable"];

                if (VsDataTable != null)
                {
                    DataRow DbRow = VsDataTable.Rows.Find(VSKey);

                    if ((DbRow == null))
                    {
                        //throw new Exception("Nao foi possivel encontrar o 'VIEWStateKey'. Tente limpar o cache de seu navegador ou recarregar esta pagina usando as teclas CTRL + F5.");
                        return null;
                    }
                    return DbRow["VsData"];
                }
                return null;
            }
        }
        else
        {
            // retorna o viewstate no modo normal
            return base.LoadPageStateFromPersistenceMedium();
        }
    }

}
