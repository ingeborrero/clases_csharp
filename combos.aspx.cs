using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Global_combos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string combo = Request.Params["combo"];
        string html = "";
        DataTable dt = new DataTable();
        DataManager manager = new DataManager();
        try
        {

            switch (combo)
            {
                case "1":
                    //dt = genericas.fu_getClientesUsuario(Request.Params["codi_dep"]);

                    html = "<select  id=\"ddl_municipio\" >";
                    if ((dt != null))
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            html = html + " <option value = \"" + row["codi_mun"] + "\">" + row["NOMB_MUN"] + "</option>";
                        }
                    }
                    html = html + "</select>";
                    break;

                case "2":                    
                    dt = manager.getMunicipios(Request.Params["codi_dep"].ToString());

                    html = "<select  id=\"ddl_municipio\" >";
                    if ((dt != null))
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            html = html + " <option value = \"" + row["codi_mun"] + "\">" + row["NOMB_MUN"] + "</option>";
                        }
                    }
                    html = html + "</select>";

                    break;

                case "3":
                    //lo_bdObj = new dll_nIUVA.Entidades(System.Configuration.ConfigurationManager.AppSettings("connStrBancos"));
                    //dt = lo_bdObj.fu_getMunicipiosPagos(Request.Params("id_cliente"), Request.Params("fech_ini"), Request.Params("fech_fin"));


                    //html = "<select  id=\"cmbDeptos\" onchange=\"limpiaCampos();\"> ";
                    //html = html + "<option value=\"-1\" selected=\"selected\" onchange=\"limpiaCampos()\">Seleccione un Municipio</option> ";
                    //if ((dt != null))
                    //{
                    //    DataRow dr = default(DataRow);
                    //    foreach (dr in dt.Rows)
                    //    {
                    //        html = html + " <option value = \"" + dr.Item(0) + ":" + dr.Item(1) + "\">" + dr.Item("NOMB_DEP") + " - " + dr.Item("NOMB_MUN") + " Cantidad de Pagos: " + dr.Item("CANTIDAD").ToString + "</option>";
                    //    }
                    //}
                    //html = html + "</select>";
                    break;
                case "4":
                    DataManager dataManager = new DataManager();
                    dt = dataManager.getClientesUsuario(Session[clsValoresGlobales.prefijoSession + ".docUsu"].ToString());

                    html = "<select  id=\"ddl_Proyecto\"  onchange=\"cargarTipoFiltros()\">";
                    if ((dt != null))
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            html = html + " <option value = \"" + row["id_cliente"] + "\">" + row["nomb_cliente"] + "</option>";
                        }
                    }
                    html = html + "</select>";
                    
                    break;

            }

            Response.Write(html);
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}