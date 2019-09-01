using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Desarrollo: Jorge Borrero
/// Fecha: Marzo de 2018
/// Clase encargada de el acceso a los datos 
/// </summary>
public class DataManager
{

    private void registraError(string mensajeError, Int32 numeroError, Int32 numeroLinea, string metodo, string nombreAplicacion, string datoClave)
    {
        DataManager datos = new DataManager();
        datos.logError("web", mensajeError, numeroError, numeroLinea, metodo, nombreAplicacion, null, datoClave);
    }

    public void logError(string codi_usu, string msg_err, Int32 num_err, Int32 line_err, string nomb_proc,
                         string app_nomb, string xml_info, string dato_clave)
    {
        try
        {
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@codi_usu", SqlDbType.VarChar, System.Data.ParameterDirection.Input, codi_usu, 50);
                db.agregarParametro("@msg_err", SqlDbType.VarChar, System.Data.ParameterDirection.Input, msg_err, -1);
                db.agregarParametro("@num_err", SqlDbType.NVarChar, System.Data.ParameterDirection.Input, num_err, 9);
                db.agregarParametro("@line_err", SqlDbType.NVarChar, System.Data.ParameterDirection.Input, line_err, 9);
                db.agregarParametro("@nomb_proc", SqlDbType.VarChar, System.Data.ParameterDirection.Input, nomb_proc, 150);
                db.agregarParametro("@app_nomb", SqlDbType.VarChar, System.Data.ParameterDirection.Input, app_nomb, 150);
                db.agregarParametro("@xml_info", SqlDbType.Xml, System.Data.ParameterDirection.Input, xml_info, -1);
                db.agregarParametro("@dato_clave", SqlDbType.VarChar, System.Data.ParameterDirection.Input ,dato_clave, 30);
                db.agregarParametro("@v_err_Id", SqlDbType.NVarChar, System.Data.ParameterDirection.Output, 0, 18 );
                db.ejecutar("eco.PA_ECO_INSERTA_ERR_APP", System.Data.CommandType.StoredProcedure, 1000);
                db.cerrarConexion();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }


    public DataTable getClientesUsuario(string usuario)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_usuario", SqlDbType.VarChar, ParameterDirection.Input, usuario, 20);
                db.ejecutar("dbo.pa_Get_ClientesUsuario", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getClientesUsuario", ex.Source, "usuario: " + usuario);
            return null;
        }
    }


    public DataTable consultarExpediente(string idCliente, string filtro, string valor, Int16 tipoConsulta, bool Abierto, bool Cerrado, bool Fiscal, bool Coactivo)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.VarChar, ParameterDirection.Input, idCliente, 30);
                db.agregarParametro("@in_filtro", SqlDbType.VarChar, ParameterDirection.Input, filtro, 20);
                db.agregarParametro("@in_valor", SqlDbType.VarChar, ParameterDirection.Input, valor, 200);
                db.agregarParametro("@in_tipo", SqlDbType.NVarChar, ParameterDirection.Input, tipoConsulta, 2);
                db.agregarParametro("@in_abiertos", SqlDbType.Bit, ParameterDirection.Input, Abierto, 6);
                db.agregarParametro("@in_cerrados", SqlDbType.Bit, ParameterDirection.Input, Cerrado, 6);
                db.agregarParametro("@in_fiscal", SqlDbType.Bit, ParameterDirection.Input, Fiscal, 6);
                db.agregarParametro("@in_coactivo", SqlDbType.Bit, ParameterDirection.Input, Coactivo, 6);
                db.ejecutar("dbo.pa_Cons_Expediente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarExpediente", ex.Source, "in_cliente: " + idCliente + " in_valor: " + valor);
            return null;
        }
    }



    public DataTable consultarItemsValoresExpediente(Int64 idExpediente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_id_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);
                db.ejecutar("dbo.pa_Cons_ItemsValores", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarItemsValoresExpediente", ex.Source, "idExpediente: " + idExpediente.ToString());
            return null;
        }
    }


    public DataTable consultarCamposValoresExpediente(Int64 idDeta, Int64 idExpediente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_id_deta", SqlDbType.NVarChar, ParameterDirection.Input, idDeta, 18);
                db.agregarParametro("@in_id_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);
                db.ejecutar("dbo.pa_Cons_CamposValores", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarCamposValoresExpediente", ex.Source, "idDeta: " + idDeta.ToString()  +  " idExpediente: " + idExpediente.ToString());
            return null;
        }
    }


    public DataTable consultarDetaPuente_XX(Int64 idExpediente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);                
                db.agregarParametro("@in_id_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);
                db.ejecutar("dbo.pa_Cons_DetaPuenteExpediente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarDetaPuente_XX", ex.Source, "idExpediente: " + idExpediente.ToString());
            return null;
        }
    }


    public DataTable consultarxIDExpediente(Int32 idCliente, Int64 idExpediente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);
                db.ejecutar("dbo.pa_Cons_ExpedienteId", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarxIDExpediente", ex.Source, "idExpediente: " + idExpediente);
            return null;
        }
    }


    public DataTable consultarInfoProcesoxIDProceso(Int64 idProceso)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_id_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.ejecutar("dbo.pa_Cons_ProcesoId", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarInfoProcesoxIDProceso", ex.Source, "idProceso: " + idProceso);
            return null;
        }
    }



    public DataTable consultarHistoriaExpediente(Int32 idCliente, Int64 idExpediente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);
                db.ejecutar("dbo.pa_Cons_HistoriaExpediente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarHistoriaExpediente", ex.Source, "in_cliente: " +  idCliente + "idExpediente: " + idExpediente);
            return null;
        }
    }

    public DataTable consultarDocumentosExpediente(Int32 idCliente, Int64 idExpediente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);
                db.ejecutar("dbo.pa_Cons_DocumentosExpediente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarDocumentosExpediente", ex.Source, "in_cliente: " + idCliente + "idExpediente: " + idExpediente);
            return null;
        }
    }


    public DataTable consultarCatalogoGestor(Int32 idCliente, Int64 idDocumento, Int64 idGestorDoc)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_documento", SqlDbType.NVarChar, ParameterDirection.Input, idDocumento, 18);
                db.agregarParametro("@in_id_gestor_doc", SqlDbType.NVarChar, ParameterDirection.Input, idGestorDoc, 18);
                db.ejecutar("dbo.pa_Cons_CatalogoDocumentos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarCatalogoGestor", ex.Source, "in_cliente: " + idCliente + "idDocumento: " + idDocumento);
            return null;
        }
    }


    public DataTable consultarProcesosNiveles(Int32 idCliente, Int64 idProceso, Int16 nivel, string grupo, string acto, string notificacion)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_nivel", SqlDbType.NVarChar, ParameterDirection.Input, nivel, 5);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_acto", SqlDbType.VarChar, ParameterDirection.Input, acto, 10);
                db.agregarParametro("@in_notificacion", SqlDbType.VarChar, ParameterDirection.Input, notificacion, 10);
                db.ejecutar("dbo.pa_Cons_Procesos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {            
            registraError(ex.ToString(), 1, 1, "consultarProcesosNiveles", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  nivel: " + nivel);
            return null;
        }
    }

    public DataTable consultarProcesoConFiltroNivel3(Int32 idCliente, Int64 idProceso, string grupo, string seccion, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.ejecutar("dbo.pa_Cons_ProcesoNivel3", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesoConFiltroNivel3", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  filvig: " + filvig + "  vig_desde: " + vig_desde);
            return null;
        }
    }


    public DataTable consultarProcesoConFiltroNivel4(Int32 idCliente, Int64 idProceso, string grupo, string seccion, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.ejecutar("dbo.pa_Cons_ProcesoNivel4", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "pa_Cons_ProcesoNivel4", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  filvig: " + filvig + "  vig_desde: " + vig_desde);
            return null;
        }
    }


    public DataTable consultarActosConFiltro(Int32 idCliente, Int64 idProceso, string grupo, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);                
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.ejecutar("dbo.pa_Cons_Actos_Filtro", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarActosConFiltro", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  filvig: " + filvig + "  vig_desde: " + vig_desde);
            return null;
        }
    }

    public DataTable consultarProcesosConFiltro(Int32 idCliente, Int64 idProceso, bool esCoactivo, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_es_coactivo", SqlDbType.Bit, ParameterDirection.Input, esCoactivo, 6);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);

                db.ejecutar("dbo.pa_Cons_Procesos_Filtros", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesosConFiltro", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso);
            return null;
        }
    }



    public DataTable consultarProcesosFiltros(Int32 idCliente, Int64 idProceso, string grupo, string id_estado, string codi_item, string seccion, 
                                              Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, bool esItemDinamico = false)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);                
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_id_estado", SqlDbType.NVarChar, ParameterDirection.Input, id_estado, 18);
                db.agregarParametro("@in_codi_item", SqlDbType.VarChar, ParameterDirection.Input, codi_item, 50);
                db.agregarParametro("@in_es_item", SqlDbType.Bit, ParameterDirection.Input, esItemDinamico, 6);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);

                db.ejecutar("dbo.pa_Cons_Procesos_Filtros_Estados", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesosFiltros", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  codi_item: " + codi_item);
            return null;
        }
    }



    public DataTable consultarActosNotificarPersonal(Int32 idCliente, string Documento, string Item1)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 6);
                db.agregarParametro("@in_documento", SqlDbType.VarChar, ParameterDirection.Input, Documento, 15);
                db.agregarParametro("@in_item1", SqlDbType.VarChar, ParameterDirection.Input, Item1, 50);
                db.ejecutar("dbo.pa_Cons_ActosNotificaPersonal", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarActosNotificarPersonal", ex.Source, "in_cliente: " + idCliente + "Documento: " + Documento + "Item1: " + Item1);
            return null;
        }
    }


    public DataTable getDepartamentos()
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.ejecutar("dbo.pa_Get_Departamentos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDepartamentos", ex.Source, "");
            return null;
        }
    }


    public DataTable getMunicipios(string codiDepto)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);                
                db.agregarParametro("@in_codi_dep", SqlDbType.VarChar, ParameterDirection.Input, codiDepto, 2);
                db.ejecutar("dbo.pa_Get_Municipios", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getMunicipios", ex.Source, "");
            return null;
        }
    }


    public DataTable getSubEntidades(Int64 idEntidad)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_id_entidad", SqlDbType.NVarChar, ParameterDirection.Input, idEntidad, 18);
                db.ejecutar("dbo.pa_Get_SubEntidades", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getSubEntidades", ex.Source, "");
            return null;
        }
    }


    public DataTable getSubEntidad(Int64 idSubEntidad)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_id_subentidad", SqlDbType.NVarChar, ParameterDirection.Input, idSubEntidad, 18);
                db.ejecutar("dbo.pa_Get_SubEntidad", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getSubEntidad", ex.Source, "");
            return null;
        }
    }


    /// <summary>
    /// Funcion general: Obtener cantidades de procesos por cliente
    /// Usado en: LauncherCDP
    /// </summary>
    public DataTable consultarCantProcesosClientes(Int32 idCliente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.ejecutar("dbo.pa_Cons_TieneProcesosCliente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarTieneProcesosCliente", ex.Source, "in_cliente: " + idCliente);
            return null;
        }
    }



    /// <summary>
    /// Funcion general: Obtener cantidades de procesos por cliente
    /// Usado en: Impresiones-LauncherCDP
    /// </summary>
    public DataTable consultarProcesosCliente(Int32 idCliente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.ejecutar("dbo.pa_Cons_ProcesosCliente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesosCliente", ex.Source, "in_cliente: " + idCliente );
            return null;
        }
    }

    /// <summary>
    /// Funcion general: Obtener cantidades y acciones pendientes por autorizar por Producto o cliente
    /// Usado en: Impresiones-LauncherCDP
    /// </summary>
    public DataTable consultarPendienteAutorizarCliente(Int32 idCliente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.ejecutar("dbo.pa_Cons_PendAutorizacionCliente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarPendienteAutorizarCliente", ex.Source, "in_cliente: " + idCliente);
            return null;
        }
    }


    public DataTable consultarParaOrdenImpresionNormal(Int32 idCliente, Int32 idAccion)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_accion", SqlDbType.NVarChar, ParameterDirection.Input, idAccion, 5);
                db.ejecutar("dbo.pa_Cons_ParaOrdenImpresionNormal", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarParaOrdenImpresionNormal", ex.Source, "in_cliente: " + idCliente  + " in_id_accion: " + idAccion);
            return null;
        }
    }


    public DataTable getItemsCamposProceso(Int64 idProceso)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.ejecutar("dbo.pa_Get_Items_Campos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getItemsCamposProceso", ex.Source, "idProceso: " + idProceso);
            return null;
        }
    }


    public DataTable getItemsVerFiltros(Int64 idProceso)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.ejecutar("dbo.pa_Get_Items_Filtros", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getItemsVerFiltros", ex.Source, "idProceso: " + idProceso);
            return null;
        }
    }


    /// <summary>
    /// Obtener informacion del proyecto (Iuva Caqueta Fiscalizacion, Iuva Santander Coactivo, ...)
    /// </summary>
    /// <param name="idProyecto"></param>
    /// <returns></returns>
    public DataTable getInfProyecto(Int64  idProyecto)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_proyecto", SqlDbType.NVarChar, ParameterDirection.Input, idProyecto, 18);
                db.ejecutar("dbo.pa_Get_Inf_Proyecto", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getInfProyecto", ex.Source, "@in_proyecto: " + idProyecto.ToString());
            return null;
        }
    }



    /// <summary>
    /// Obtener informacion del Proceso (Omiso, Imexacto, Verif bienes) Agrupado, Coactivo, Fiscalizacion, Paralelo,...
    /// </summary>
    /// <param name="idProceso"></param>
    /// <returns></returns>
    public DataTable getInfProcesoProyecto(Int64 idProceso)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.ejecutar("dbo.pa_Get_Inf_Proceso", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getInfProcesoProyecto", ex.Source, "idProceso: " + idProceso.ToString());
            return null;
        }
    }



    public DataTable getInfProcesosProyecto(Int64 idProyecto)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_proyecto", SqlDbType.NVarChar, ParameterDirection.Input, idProyecto, 18);
                db.ejecutar("dbo.pa_Get_Inf_Procesos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getInfProcesosProyecto", ex.Source, "idProyecto: " + idProyecto.ToString());
            return null;
        }
    }



    public DataTable getInfEstado(Int64 idEstado)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_id_estado", SqlDbType.NVarChar, ParameterDirection.Input, idEstado, 18);
                db.ejecutar("dbo.pa_Get_Inf_Estado", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getInfProcesoProyecto", ex.Source, "idEstado: " + idEstado.ToString());
            return null;
        }
    }

    public DataTable getInfCliente(Int32 idCliente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.ejecutar("dbo.pa_Get_Inf_Cliente", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getInfProcesoProyecto", ex.Source, "idCliente: " + idCliente.ToString());
            return null;
        }
    }


    public DataTable getInfProyectosCliente(Int32 idProducto, Int32 idCliente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_producto", SqlDbType.NVarChar, ParameterDirection.Input, idProducto, 18);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.ejecutar("dbo.pa_Get_Inf_Proyectos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getInfProyectosCliente", ex.Source, "idCliente: " + idCliente.ToString());
            return null;
        }
    }


    public DataTable getEstadosSiguientesDisponibles(Int32 idCliente,  Int64 idEstado)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_estado", SqlDbType.NVarChar, ParameterDirection.Input, idEstado, 18);
                db.ejecutar("dbo.pa_Cons_Estados_Siguen", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getEstadosSiguientesDisponibles", ex.Source, "idEstado: " + idEstado.ToString());
            return null;
        }
    }

    public DataTable getEstadosAnterioresDisponibles(Int32 idCliente, Int64 idEstado)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_estado", SqlDbType.NVarChar, ParameterDirection.Input, idEstado, 18);
                db.ejecutar("dbo.pa_Cons_Estados_Ant", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getEstadosAnterioresDisponibles", ex.Source, "idEstado: " + idEstado.ToString());
            return null;
        }
    }



    public DataTable getDataResumenProcesosExport(Int32 idCliente)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);

                db.ejecutar("dbo.pa_Cons_Resumen_Proc", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDataResumenProcesosExport", ex.Source, "in_cliente: " + idCliente);
            return null;
        }
    }


    public DataTable getDataDetalleProcesosExport(Int32 idCliente, Int64 idProyecto, bool esEmbargos = false, bool esConsolidado = false)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proyecto", SqlDbType.NVarChar, ParameterDirection.Input, idProyecto, 18);
                db.agregarParametro("@in_embargos", SqlDbType.Bit, ParameterDirection.Input, esEmbargos, 6);
                db.agregarParametro("@in_consolidado", SqlDbType.Bit, ParameterDirection.Input, esConsolidado, 6);
                

                db.ejecutar("dbo.pa_Cons_Procesos_Deta", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDataDetalleProcesosExport", ex.Source, "in_cliente: " + idCliente + "  idProyecto: " + idProyecto);
            return null;
        }
    }


    public DataTable getDataDetalleProcesosExportConFiltros(Int32 idCliente, Int64 idProyecto, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, bool esCoactivo=false, bool esConsolidado = false)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proyecto", SqlDbType.NVarChar, ParameterDirection.Input, idProyecto, 18);
                db.agregarParametro("@in_es_coactivo", SqlDbType.Bit, ParameterDirection.Input, esCoactivo, 6);
                db.agregarParametro("@in_consolidado", SqlDbType.Bit, ParameterDirection.Input, esConsolidado, 6);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);        

                db.ejecutar("dbo.pa_Cons_Procesos_Deta_Filtro", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDataDetalleProcesosExportConFiltros", ex.Source, "in_cliente: " + idCliente + "  idProyecto: " + idProyecto);
            return null;
        }
    }



    public DataTable getDataDetalleProcesosActosExport(Int32 idCliente, Int64 idProceso, string grupo, string seccion, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);

                db.ejecutar("dbo.pa_Cons_ProcesoNivel3_Deta", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDataDetalleProcesosActosExport", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso);
            return null;
        }
    }


    public DataTable getDataDetalleExportNivel(Int32 idCliente, Int64 idProceso, string grupo, string id_estado, string codi_item, string seccion,
                                               Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, bool esItemDinamico = false)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_id_estado", SqlDbType.NVarChar, ParameterDirection.Input, id_estado, 18);
                db.agregarParametro("@in_codi_item", SqlDbType.VarChar, ParameterDirection.Input, codi_item, 50);
                db.agregarParametro("@in_es_item", SqlDbType.Bit, ParameterDirection.Input, esItemDinamico, 6);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);

                db.ejecutar("dbo.pa_Cons_Procesos_Filtros_Estados", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDataDetalleExportNivel", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  codi_item: " + codi_item);
            return null;
        }
    }



    public DataTable consultarProcesosNivelesEMBARGOS(Int32 idCliente, Int64 idProceso, Int16 nivel, string grupo, string acto, string notificacion)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_nivel", SqlDbType.NVarChar, ParameterDirection.Input, nivel, 5);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_acto", SqlDbType.VarChar, ParameterDirection.Input, acto, 10);
                db.agregarParametro("@in_notificacion", SqlDbType.VarChar, ParameterDirection.Input, notificacion, 10);
                db.ejecutar("dbo.pa_Cons_Procesos_Emb", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesosNivelesEMBARGOS", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  nivel: " + nivel);
            return null;
        }
    }


    public DataTable consultarActosConFiltroEMBARGOS(Int32 idCliente, Int64 idProceso, string grupo, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, Int32 id_subentidad)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.agregarParametro("@in_id_subentidad", SqlDbType.NVarChar, ParameterDirection.Input, id_subentidad, 18);
                db.ejecutar("dbo.pa_Cons_Actos_Filtro_Emb", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarActosConFiltroEMBARGOS", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  filvig: " + filvig + "  vig_desde: " + vig_desde);
            return null;
        }
    }

    public DataTable consultarProcesoConFiltroNivel3EMBARGOS(Int32 idCliente, Int64 idProceso, string grupo, string seccion, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, Int32 id_subentidad)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.agregarParametro("@in_id_subentidad", SqlDbType.NVarChar, ParameterDirection.Input, id_subentidad, 18);
                db.ejecutar("dbo.pa_Cons_ProcesoNivel3_Emb", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesoConFiltroNivel3EMBARGOS", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  filvig: " + filvig + "  vig_desde: " + vig_desde);
            return null;
        }
    }

    public DataTable consultarProcesoConFiltroNivel4EMBARGOS(Int32 idCliente, Int64 idProceso, string grupo, string seccion, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, Int32 id_subentidad)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.agregarParametro("@in_id_subentidad", SqlDbType.NVarChar, ParameterDirection.Input, id_subentidad, 18);
                db.ejecutar("dbo.pa_Cons_ProcesoNivel4_Emb", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarProcesoConFiltroNivel4EMBARGOS", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso + "  filvig: " + filvig + "  vig_desde: " + vig_desde);
            return null;
        }
    }


    public DataTable getDataDetalleProcesosActosExportEMBARGOS(Int32 idCliente, Int64 idProceso, string grupo, string seccion, Int32 filvig, Int32 vig_desde, Int32 vig_hasta, Int32 filfecha, DateTime fec_desde, DateTime fec_hasta, Int32 id_subentidad)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_proceso", SqlDbType.NVarChar, ParameterDirection.Input, idProceso, 18);
                db.agregarParametro("@in_grupo", SqlDbType.VarChar, ParameterDirection.Input, grupo, 10);
                db.agregarParametro("@in_seccion", SqlDbType.VarChar, ParameterDirection.Input, seccion, 20);
                db.agregarParametro("@in_filvig", SqlDbType.NVarChar, ParameterDirection.Input, filvig, 5);
                db.agregarParametro("@in_vig_desde", SqlDbType.NVarChar, ParameterDirection.Input, vig_desde, 5);
                db.agregarParametro("@in_vig_hasta", SqlDbType.NVarChar, ParameterDirection.Input, vig_hasta, 5);
                db.agregarParametro("@in_filfecha", SqlDbType.NVarChar, ParameterDirection.Input, filfecha, 5);
                db.agregarParametro("@in_fec_desde", SqlDbType.Date, ParameterDirection.Input, fec_desde, 12);
                db.agregarParametro("@in_fec_hasta", SqlDbType.Date, ParameterDirection.Input, fec_hasta, 12);
                db.agregarParametro("@in_id_subentidad", SqlDbType.NVarChar, ParameterDirection.Input, id_subentidad, 18);

                db.ejecutar("dbo.pa_Cons_ProcesoNivel3_Deta_Emb", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "getDataDetalleProcesosActosExportEMBARGOS", ex.Source, "in_cliente: " + idCliente + "  idProceso: " + idProceso);
            return null;
        }
    }


    public DataTable consultarExpedienteEmbargos(Int32 idCliente, Int64 idExpediente, Int16 tipoConsulta)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 6);
                db.agregarParametro("@in_expediente", SqlDbType.NVarChar, ParameterDirection.Input, idExpediente, 18);                
                db.agregarParametro("@in_tipo", SqlDbType.NVarChar, ParameterDirection.Input, tipoConsulta, 2);
                db.ejecutar("dbo.pa_Cons_Embargos", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarExpedienteEmbargos", ex.Source, "in_cliente: " + idCliente + " idExpediente: " + idExpediente);
            return null;
        }
    }


    public DataTable consultarxIDExpedienteEmbargo(Int32 idCliente, Int64 idExpedienteEmb)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_expediente_emb", SqlDbType.NVarChar, ParameterDirection.Input, idExpedienteEmb, 18);
                db.ejecutar("dbo.pa_Cons_EmbargoId", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarxIDExpedienteEmbargo", ex.Source, "idExpedienteEmb: " + idExpedienteEmb);
            return null;
        }
    }


    public DataTable consultarHistoriaEmbargo(Int32 idCliente, Int64 idExpedienteEmb)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_cliente", SqlDbType.NVarChar, ParameterDirection.Input, idCliente, 18);
                db.agregarParametro("@in_id_expediente_emb", SqlDbType.NVarChar, ParameterDirection.Input, idExpedienteEmb, 18);
                db.ejecutar("dbo.pa_Cons_HistoriaEmbargo", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarHistoriaEmbargo", ex.Source, "in_cliente: " + idCliente + "idExpediente: " + idExpedienteEmb);
            return null;
        }
    }


    public DataTable consultarPendientesCierreManual(bool esSoloActos, string cdp_idf_acto)
    {
        try
        {
            DataTable dt = null;
            using (IAccesoBD db = new DataSql(clsValoresGlobales.cadena_con_cdp_eco))
            {
                db.abrirConexion(false);
                db.agregarParametro("@in_solo_actos", SqlDbType.Bit, ParameterDirection.Input, esSoloActos, 6);
                db.agregarParametro("@in_cdp_idf_acto", SqlDbType.VarChar, ParameterDirection.Input, cdp_idf_acto, 10);

                db.ejecutar("dbo.pa_Cons_PendienteCierreManual", System.Data.CommandType.StoredProcedure, 1000);
                dt = db.obtenerDatatable(0);
                db.cerrarConexion();
            }
            return dt;
        }
        catch (Exception ex)
        {
            registraError(ex.ToString(), 1, 1, "consultarPendientesCierreManual", ex.Source, "");
            return null;
        }
    }

}