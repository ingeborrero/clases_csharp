using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Clase para definir las variables globales a todo el sitio web
/// </summary>
public class clsValoresGlobales
{
    //public const string prefijoSession = "SESSION_NOMBRE_PROYECTO"; //'Prefijo de session para que los osbjetos se nombren prefijo.nombreObjeto
    public static string separadorArchivosTexto = ""; // Modifiable in Code
    public static string prefijoSession = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();
    public static string rutasEstaticos = ObtenerLlaveConfig("rutasEstaticos");
    public static string RutaPubli = ObtenerLlaveConfig("RutaPubli");
    public static string rutaConnect = ObtenerLlaveConfig("RutaConnect");
    public static string urlRedireccion = ObtenerLlaveConfig("urlAsignarClaveRedireccion");
    public static string sufijoClaveGeneradaSistema = "xZ/U3"; //Este sufijo se usa para detectar cuando el sistema se genero una contraseña nueva Este Valor no debe ser modificado sin previa consulta a BPM.
    public static string idProducto = ObtenerLlaveConfig("idProducto");
    public static string idProyecto = ObtenerLlaveConfig("idProyecto");
    public static string idAplicacion = ObtenerLlaveConfig("idAplicacion");
    public static string fotoUsuarioBPM = ObtenerLlaveConfig("fotoUsuarioBPM");
    public static string mensajeBienvenida = "Por favor seleccione el perfil con el cual desea realizar acciones al interior del Ecosistema.";
    public static string AmbApp = ObtenerLlaveConfig("AmbApp");
    public static string urlSession = ObtenerLlaveConfig("urlSession");
    public static string cnxECO = "cnx_eco";

    public const string tipoMenu = "";
    public const string tipoMenuEmpty = "empty";
    public const string tipoMenuEmpresas = "empresas";
    public const string tipoMenuEmpresasPerfil = "empresasPerfil";

    public const string modoMenu = "";
    public const string modoMenuLeft = "left";
    public const string modoMenuMobile = "mobile";
    public const string modoMenuBottom = "bottom";

    public const string agrupaPerfiles = "";
    public const string agrupaPerfilesTrue = "true";

    public static string cadena_con_cdp_eco = Funciones.ObtenerCadenaConexion("cadena_con_cdp_eco", "cadena_con_cdp_eco", System.Configuration.ConfigurationManager.AppSettings["idProducto"], "SQL");
    //public static string cadena_con_cdp_eco = "Data source = SSYCGESDOCBD.SYC.LOC; initial catalog = DB_ECO_FISCAL; application name = DB_ECO_FISCAL; user id = usr_eco_fiscal; password =syc*2019a";
    public static string SAN_Cdp = "\\\\ssycv7kprd01.syc.loc\\RNT_Control_Proc1\\Fiscalizacion\\"; //"\\\\172.19.20.20\\Iuva\\Fiscalizacion\\";
    public static string SAN_iuva = "\\\\ssycv7kprd01\\RNT_Iuva\\";

	/// <summary>
    /// Obtiene una llave del archivo web.config.
    /// </summary>
    /// <param name="nombreLlave">Nombre de la llave a obtener su valor.</param>
    /// <returns></returns>
    public static string ObtenerLlaveConfig(string nombreLlave)
    {
        string valorLlave = string.Empty;

        if (ConfigurationManager.AppSettings[nombreLlave] != null)
            valorLlave = ConfigurationManager.AppSettings[nombreLlave].ToString();
        else
            throw new Exception("No se encontró la llave " + nombreLlave + ". Por favor revise el archivo web.config.");

        return valorLlave;
    }

    /// <summary>
    /// Obtiene una cadena de conexión del archivo web.config.
    /// </summary>
    /// <param name="nombreCadena">Nombre de la cadena a obtener su valor.</param>
    /// <returns></returns>
    public static string ObtenerCadeaConexionConfig(string nombreCadena)
    {
        string valorCadena = string.Empty;

        if (ConfigurationManager.ConnectionStrings[nombreCadena] != null)
            valorCadena = ConfigurationManager.ConnectionStrings[nombreCadena].ConnectionString;
        else
            throw new Exception("No se encontró la cadena de conexión " + nombreCadena + ". Por favor revise el archivo web.config.");

        return valorCadena;
    }

    /// <summary>
    /// Obtiene la URL para redirigir al Login.
    /// </summary>
    /// <param name="tagCliente">Tag del cliente.</param>
    /// <returns></returns>
    public static string UrlRedireccionLogin(string tagCliente)
    {
        return PageSession.urlRedireccion(tagCliente);
    }

    public static Dictionary<string, string> getDictionaryDatosProcesados()
    {
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        dataDict.Add("DIRE_PER", "Direcci&oacute;n");
        dataDict.Add("NOMB_MUN{,}NOMB_DEP", "Ciudad y departamento de residencia");
        dataDict.Add("CELU_PER{o}TEL1_PER{o}TEL2_PER", "Tel&eacute;fono");
        dataDict.Add("EMAIL", "Correo electr&oacute;nico");
        return dataDict;
    }

    public static Dictionary<string, string> getDictionaryDatosExpediente()
    {
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        //dataDict.Add("NOMB_EST_Y_OBSERVA", "Estado actual");
        dataDict.Add("NOMB_GRUPO", "Grupo actual");
        dataDict.Add("ID_EXPEDIENTE", "Id expediente");
        dataDict.Add("ANIO_EXPEDIENTE{-}NRO_EXPEDIENTE", "A&ntilde;o y n&uacute;mero de expediente");
        dataDict.Add("FECH_PRO", "Fecha apertura");
        dataDict.Add("FECH_EST", "Fecha estado actual");
        dataDict.Add("CODI_PRO_ANT", "Proceso anterior");
        dataDict.Add("FECHA_PRES", "Fecha de prescripci&oacute;n");
        dataDict.Add("FECHA_REG", "Fecha de registro");
        return dataDict;
    }

    public static Dictionary<string, string> getDictionaryHistoria()
    {
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        //dataDict.Add("FECH_OPCION", "Fecha estado");
        dataDict.Add("FECH_CRE", "Fecha registro en el sistema");
        dataDict.Add("NOMB_USUARIO", "Usuario");
        dataDict.Add("OBSERVA", "Observaci&oacute;n");
        return dataDict;
    }

    public static Dictionary<string, string> getDictionaryDocumento()
    {
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        //dataDict.Add("FECHA_DOC", "Fecha generaci&oacute;n");
        dataDict.Add("FECH_CRE", "Fecha registro");
        dataDict.Add("NOMB_TIPO_DOCUMENTO", "Tipo");
        dataDict.Add("NRO_RADICADO", "Radicado");
        

        return dataDict;
    }

    public static Dictionary<string, string> getDictionaryDatosCasosEmbargo()
    {
        Dictionary<string, string> dataDict = new Dictionary<string, string>();
        dataDict.Add("NOMB_GRUPO", "Estado actual");
        //dataDict.Add("ID_EXPEDIENTE_EMB", "Id embargo");
        dataDict.Add("ANIO_EXPEDIENTE{-}NRO_EXPEDIENTE", "A&ntilde;o y n&uacute;mero de expediente");
        dataDict.Add("FECH_GEN", "Fecha generaci&oacute;n");
        dataDict.Add("FECH_EST", "Fecha estado");
        dataDict.Add("NOMB_ESTA_CAS", "Estado embargo");
        
        return dataDict;
    }



    public static Dictionary<string,string[]> getListaCamposExportCasosDict()
    {
        return new Dictionary<string, string[]> {
                                                    { "ID_PROCESO",          new string[] {"ID_PROCESO",         "TEXTO" } },
                                                    { "NOMB_PROCESO",        new string[] {"NOMB_PROCESO",       "TEXTO" } },
                                                    { "ID_EXPEDIENTE",       new string[] {"EXPEDIENTE",         "TEXTO" } },                                                    
                                                    { "FECH_GEN",            new string[] {"FECHA_GENERACION",   "FECHA" } },
                                                    { "NRO_EXPEDIENTE",      new string[] {"NRO_EXPEDIENTE",     "TEXTO" } },
                                                    { "NOMB_GRUPO",          new string[] {"NOMB_GRUPO",         "TEXTO" } },
                                                    { "ACTO",                new string[] {"ACTO",               "TEXTO" } },
                                                    { "CODI_ESTADO",         new string[] {"CODI_ESTADO",        "TEXTO" } },
                                                    { "NOMB_ESTADO",         new string[] {"NOMB_ESTADO",        "TEXTO" } },
                                                    { "FECH_EST",            new string[] {"FECHA_DEL_ESTADO",   "FECHA" } },
                                                    { "FECH_GENERACION_CASO",new string[] {"FECHA_DEL_PROCESO",  "FECHA" } },
                                                    { "FECHA_REG",           new string[] {"FECHA_DEL_REGISTRO", "FECHA" } },
                                                    { "CON_RIESGO",          new string[] {"EN_RIESGO",          "TEXTO" } },
                                                    { "ID_ESTADO_ANT",       new string[] {"ID_ESTADO_ANT",      "TEXTO" } },
                                                    { "NOMB_ESTADO_ANT",     new string[] {"NOMB_ESTADO_ANT",    "TEXTO" } },
                                                    { "TIPO_DOC",            new string[] {"TIPO_DOC",           "TEXTO" } },
                                                    { "CC_PER",              new string[] {"CC_PER",             "TEXTO" } },
                                                    { "DV",                  new string[] {"DV",                 "TEXTO" } },
                                                    { "NOMB_PER",            new string[] {"NOMB_PER",           "TEXTO" } },                                                                                                        
                                                    { "DIRE_PER",            new string[] {"DIRE_PER",           "TEXTO" } },
                                                    { "DEPTO_PER",           new string[] {"COD_DEPTO",          "TEXTO" } },
                                                    { "NOMB_DEP",            new string[] {"DEPARTAMENTO",       "TEXTO" } },
                                                    { "MUNI_PER",            new string[] {"COD_MUNI",           "TEXTO" } },                                                    
                                                    { "NOMB_MUN",            new string[] {"MUNICIPIO",          "TEXTO" } },                                                    
                                                    
                                                };           
    }


    public static Dictionary<string, string[]> getListaCamposExportCasosDetaCoactivoDict()
    {
        return new Dictionary<string, string[]> {
                                                    { "ID_PROCESO",          new string[] {"ID_PROCESO",         "TEXTO" } },
                                                    { "NOMB_PROCESO",        new string[] {"NOMB_PROCESO",       "TEXTO" } },
                                                    { "ID_EXPEDIENTE",       new string[] {"EXPEDIENTE",         "TEXTO" } },
                                                    { "FECH_GEN",            new string[] {"FECHA_GENERACION",   "FECHA" } },
                                                    { "NRO_EXPEDIENTE",      new string[] {"NRO_EXPEDIENTE",     "TEXTO" } },
                                                    { "NOMB_GRUPO",          new string[] {"NOMB_GRUPO",         "TEXTO" } },
                                                    { "ACTO",                new string[] {"ACTO",               "TEXTO" } },
                                                    { "CODI_ESTADO",         new string[] {"CODI_ESTADO",        "TEXTO" } },
                                                    { "NOMB_ESTADO",         new string[] {"NOMB_ESTADO",        "TEXTO" } },
                                                    { "ESTADO_VIGENCIA",     new string[] { "ESTADO_VIGENCIA",   "TEXTO" } },
                                                    { "FECH_EST",            new string[] {"FECHA_DEL_ESTADO",   "FECHA" } },
                                                    { "FECH_GENERACION_CASO",new string[] {"FECHA_DEL_PROCESO",  "FECHA" } },
                                                    { "FECHA_REG",           new string[] {"FECHA_DEL_REGISTRO", "FECHA" } },
                                                    { "CON_RIESGO",          new string[] {"EN_RIESGO",          "TEXTO" } },
                                                    { "ID_ESTADO_ANT",       new string[] {"ID_ESTADO_ANT",      "TEXTO" } },                                                    
                                                    { "NOMB_ESTADO_ANT",     new string[] {"NOMB_ESTADO_ANT",    "TEXTO" } },
                                                    { "FECHA_EJECUTORIA",    new string[] {"FECHA_EJECUTORIA",   "FECHA" } },
                                                    { "TIPO_DOC",            new string[] {"TIPO_DOC",           "TEXTO" } },
                                                    { "CC_PER",              new string[] {"CC_PER",             "TEXTO" } },
                                                    { "DV",                  new string[] {"DV",                 "TEXTO" } },
                                                    { "NOMB_PER",            new string[] {"NOMB_PER",           "TEXTO" } },
                                                    { "DIRE_PER",            new string[] {"DIRE_PER",           "TEXTO" } },
                                                    { "DEPTO_PER",           new string[] {"COD_DEPTO",          "TEXTO" } },
                                                    { "NOMB_DEP",            new string[] {"DEPARTAMENTO",       "TEXTO" } },
                                                    { "MUNI_PER",            new string[] {"COD_MUNI",           "TEXTO" } },
                                                    { "NOMB_MUN",            new string[] {"MUNICIPIO",          "TEXTO" } }
                                                };
    }


    public static Dictionary<string, string[]> getListaCamposExportCasosConsolidadosCoactivoDict()
    {
        return new Dictionary<string, string[]> {
                                                    { "ID_PROCESO",          new string[] {"ID_PROCESO",         "TEXTO" } },
                                                    { "NOMB_PROCESO",        new string[] {"NOMB_PROCESO",       "TEXTO" } },
                                                    { "ID_EXPEDIENTE",       new string[] {"EXPEDIENTE",         "TEXTO" } },
                                                    { "FECH_GEN",            new string[] {"FECHA_GENERACION",   "FECHA" } },
                                                    { "NRO_EXPEDIENTE",      new string[] {"NRO_EXPEDIENTE",     "TEXTO" } },
                                                    { "NOMB_GRUPO",          new string[] {"NOMB_GRUPO",         "TEXTO" } },
                                                    { "ACTO",                new string[] {"ACTO",               "TEXTO" } },
                                                    { "CODI_ESTADO",         new string[] {"CODI_ESTADO",        "TEXTO" } },
                                                    { "NOMB_ESTADO",         new string[] {"NOMB_ESTADO",        "TEXTO" } },                                                    
                                                    { "FECH_EST",            new string[] {"FECHA_DEL_ESTADO",   "FECHA" } },
                                                    { "FECH_GENERACION_CASO",new string[] {"FECHA_DEL_PROCESO",  "FECHA" } },
                                                    { "FECHA_REG",           new string[] {"FECHA_DEL_REGISTRO", "FECHA" } },
                                                    { "CON_RIESGO",          new string[] {"EN_RIESGO",          "TEXTO" } },
                                                    { "ID_ESTADO_ANT",       new string[] {"ID_ESTADO_ANT",      "TEXTO" } },
                                                    { "NOMB_ESTADO_ANT",     new string[] {"NOMB_ESTADO_ANT",    "TEXTO" } },                                                    
                                                    { "FECHA_EJECUTORIA",    new string[] {"FECHA_EJECUTORIA",   "FECHA" } },
                                                    { "TIPO_DOC",            new string[] {"TIPO_DOC",           "TEXTO" } },
                                                    { "CC_PER",              new string[] {"CC_PER",             "TEXTO" } },
                                                    { "DV",                  new string[] {"DV",                 "TEXTO" } },
                                                    { "NOMB_PER",            new string[] {"NOMB_PER",           "TEXTO" } },
                                                    { "DIRE_PER",            new string[] {"DIRE_PER",           "TEXTO" } },
                                                    { "DEPTO_PER",           new string[] {"COD_DEPTO",          "TEXTO" } },
                                                    { "NOMB_DEP",            new string[] {"DEPARTAMENTO",       "TEXTO" } },
                                                    { "MUNI_PER",            new string[] {"COD_MUNI",           "TEXTO" } },
                                                    { "NOMB_MUN",            new string[] {"MUNICIPIO",          "TEXTO" } },
                                                    { "VIGENCIAS",           new string[] { "VIGENCIAS",         "TEXTO" } }
                                                };
    }


    public static Dictionary<string, string[]> getListaCamposExportCasosActoDict()
    {
        return new Dictionary<string, string[]> {
                                                    { "ID_PROCESO",          new string[] {"ID_PROCESO",         "TEXTO" } },
                                                    { "NOMB_PROCESO",        new string[] {"NOMB_PROCESO",       "TEXTO" } },
                                                    { "ID_EXPEDIENTE",       new string[] {"EXPEDIENTE",         "TEXTO" } },
                                                    { "FECH_GEN",            new string[] {"FECHA_GENERACION",   "FECHA" } },
                                                    { "NRO_EXPEDIENTE",      new string[] {"NRO_EXPEDIENTE",     "TEXTO" } },
                                                    { "NOMB_GRUPO",          new string[] {"NOMB_GRUPO",         "TEXTO" } },
                                                    { "ACTO",                new string[] {"ACTO",               "TEXTO" } },
                                                    { "CODI_ESTADO",         new string[] {"CODI_ESTADO",        "TEXTO" } },
                                                    { "NOMB_ESTADO",         new string[] {"NOMB_ESTADO",        "TEXTO" } },
                                                    { "FECH_EST",            new string[] {"FECHA_DEL_ESTADO",   "FECHA" } },
                                                    { "FECH_GENERACION_CASO",new string[] {"FECHA_DEL_PROCESO",  "FECHA" } },
                                                    { "FECHA_REG",           new string[] {"FECHA_DEL_REGISTRO", "FECHA" } },
                                                    { "CON_RIESGO",          new string[] {"EN_RIESGO",          "TEXTO" } },
                                                    { "ID_ESTADO_ANT",       new string[] {"ID_ESTADO_ANT",      "TEXTO" } },
                                                    { "NOMB_ESTADO_ANT",     new string[] {"NOMB_ESTADO_ANT",    "TEXTO" } },
                                                    { "TIPO_DOC",            new string[] {"TIPO_DOC",           "TEXTO" } },
                                                    { "CC_PER",              new string[] {"CC_PER",             "TEXTO" } },
                                                    { "DV",                  new string[] {"DV",                 "TEXTO" } },
                                                    { "NOMB_PER",            new string[] {"NOMB_PER",           "TEXTO" } },
                                                    { "DIRE_PER",            new string[] {"DIRE_PER",           "TEXTO" } },
                                                    { "DEPTO_PER",           new string[] {"COD_DEPTO",          "TEXTO" } },
                                                    { "NOMB_DEP",            new string[] {"DEPARTAMENTO",       "TEXTO" } },
                                                    { "MUNI_PER",            new string[] {"COD_MUNI",           "TEXTO" } },
                                                    { "NOMB_MUN",            new string[] {"MUNICIPIO",          "TEXTO" } }
                                                };
    }

       

    public static Dictionary<string, string[]> getListaCamposExportEmbargosDict()
    {
        return new Dictionary<string, string[]> {
                                                    { "ID_PROCESO",              new string[] {"ID_PROCESO",         "TEXTO" } },
                                                    { "NOMB_PROCESO",            new string[] {"NOMB_PROCESO",       "TEXTO" } },
                                                    { "ID_EXPEDIENTE_EMB",       new string[] {"EXPEDIENTE",         "TEXTO" } },
                                                    { "PLACA",                   new string[] {"PLACA",              "TEXTO" } },                                                    
                                                    { "NOMBRE_ENTIDAD",          new string[] {"ENTIDAD",            "TEXTO" } },
                                                    { "NOMBRE_SUBENTIDAD",       new string[] {"SUBENTIDAD",         "TEXTO" } },                                                    
                                                    { "FECH_GEN",                new string[] {"FECHA_GENERACION",   "FECHA" } },
                                                    { "NRO_EXPEDIENTE",          new string[] {"NRO_EXPEDIENTE",     "TEXTO" } },
                                                    { "NOMB_GRUPO",              new string[] {"NOMB_GRUPO",         "TEXTO" } },
                                                    { "ACTO",                    new string[] {"ACTO",               "TEXTO" } },
                                                    { "FECH_EST",                new string[] {"FECHA_DEL_ESTADO",   "FECHA" } },
                                                    { "FECH_GENERACION_CASO",    new string[] {"FECHA_DEL_REGISTRO", "FECHA" } },
                                                    { "CODI_ESTADO",             new string[] {"CODI_ESTADO",        "TEXTO" } },
                                                    { "NOMB_ESTADO",             new string[] {"NOMB_ESTADO",        "TEXTO" } },                                                    
                                                    { "ID_ESTADO_ANT",           new string[] {"ID_ESTADO_ANT",      "TEXTO" } },
                                                    { "NOMB_ESTADO_ANT",         new string[] {"NOMB_ESTADO_ANT",    "TEXTO" } },
                                                    { "TIPO_DOC",                new string[] {"TIPO_DOC",           "TEXTO" } },
                                                    { "CC_PER",                  new string[] {"CC_PER",             "TEXTO" } },
                                                    { "DV",                      new string[] {"DV",                 "TEXTO" } },
                                                    { "NOMB_PER",                new string[] {"NOMB_PER",           "TEXTO" } },
                                                    { "DIRE_PER",                new string[] {"DIRE_PER",           "TEXTO" } },
                                                    { "DEPTO_PER",               new string[] {"COD_DEPTO",          "TEXTO" } },
                                                    { "NOMB_DEP",                new string[] {"DEPARTAMENTO",       "TEXTO" } },
                                                    { "MUNI_PER",                new string[] {"COD_MUNI",           "TEXTO" } },
                                                    { "NOMB_MUN",                new string[] {"MUNICIPIO",          "TEXTO" } },
                                                    { "ANIO_EXPEDIENTE",         new string[] { "ANIO_EXPEDIENTE",   "TEXTO" } }
                                                };                               
    }

}
