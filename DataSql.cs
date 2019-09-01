using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;


public class DataSql: IAccesoBD ,IDisposable
{
    public class Resultados
    {
        private Dictionary<string, object> _parametros;

        private List<DataTable> _tablas;
        public Dictionary<string, object> parametros
        {
            get { return _parametros; }
            set { _parametros = value; }
        }

        public List<DataTable> tablas
        {
            get { return _tablas; }
            set { _tablas = value; }
        }


        public object obtenerParametro(string item)
        {
            return _parametros[item];
        }

        public DataTable obtenerTabla(int index)
        {
            return _tablas[index];
        }

        public DataSet obtenerTablas()
        {
            DataSet dtsTablas = new DataSet();
            foreach (DataTable tabla in _tablas)
            {
                dtsTablas.Tables.Add(tabla.Copy());
            }
            return dtsTablas;
        }
    }

    private Collection<SqlParameter> _parametros = new Collection<SqlParameter>();
    private SqlConnection _conexion = new SqlConnection();
    private bool _transaccion;
    private Resultados _resultados;
    private SqlTransaction _dataTransaccion;
    private List<DataTable> gv_tablas = new List<DataTable>();
    private Dictionary<string, object> gv_paramsDeSalida = new Dictionary<string, object>();

 
    public DataSql(string CadenaConexion)
    {
        _conexion.ConnectionString = CadenaConexion;
    }

    public DataSql()
    {
    }

    public DataSql(string CadenaConexion, bool transaccion = false)
    {
        abrirConexion(CadenaConexion, transaccion);
    }



    public void abrirConexion(string CadenaConexion, bool transaccion = false)
    {
        try
        {
            _conexion.ConnectionString = CadenaConexion;
            _conexion.Open();
            _transaccion = transaccion;
            if (transaccion == true)
            {
                _dataTransaccion = _conexion.BeginTransaction();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    /// <summary>
    /// Le asigna una transacción a la conexión del objeto si y solo si existe una conexión abierta
    /// </summary>
    public string iniciarTransaccion()
    {
        string resp = "";
        try
        {
            if (_conexion.State != ConnectionState.Open)
            {
                throw new Exception("Debe existir una conexión abierta para iniciar una transacción.");
            }
            if (_transaccion)
            {
                throw new Exception("La conexión actual ya tiene una transacción activa.");
            }
            else
            {
                _transaccion = true;
                _dataTransaccion = _conexion.BeginTransaction();
                resp = "OK";
            }
        }
        catch (Exception ex)
        {
            resp = ex.Message;
        }
        return resp;
    }

    public string asignarConexion(string CadenaConexion)
    {
        string resp = "";
        try
        {
            _conexion.ConnectionString = CadenaConexion;
        }
        catch (Exception ex)
        {
            resp = ex.Message;
        }
        return resp;
    }

    public string abrirConexion(bool transaccion = false)
    {
        string resp = "";
        try
        {
            if (_conexion.State != ConnectionState.Open)
            {
                _conexion.Open();
            }
            _transaccion = transaccion;
            if (transaccion == true)
            {
                _dataTransaccion = _conexion.BeginTransaction();
            }
            resp = "OK";
        }
        catch (Exception ex)
        {
            resp = ex.Message;
        }
        return resp;
    }

    public void rollback()
    {
        try
        {
            if (_transaccion == true)
            {
                if (_dataTransaccion.Connection != null)
                {
                    _dataTransaccion.Rollback();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void commit()
    {
        try
        {
            if (_transaccion == true)
            {
                if (_dataTransaccion.Connection != null)
                {
                    _dataTransaccion.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string cerrarConexion()
    {
        string resp = "";
        try
        {
            if (_conexion.State != ConnectionState.Closed)
            {
                _conexion.Close();
            }
            resp = "OK";
        }
        catch (Exception ex)
        {
            resp = ex.Message;
        }
        return resp;
    }

    public void agregarParametros(string nombre, SqlDbType tipoDato, object valor, int longitud, ParameterDirection Direccion)
    {
        try
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombre;
            parametro.SqlDbType = tipoDato;
            parametro.Value = valor;
            if (longitud > 0)
            {
                parametro.Size = longitud;
            }                      
            parametro.Direction = Direccion;
            _parametros.Add(parametro);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void agregarParametros(string nombre, SqlDbType tipoDato, object valor, int longitud, ParameterDirection Direccion, byte escala, byte prec)
    {
        try
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombre;
            parametro.SqlDbType = tipoDato;
            parametro.Value = valor;
            parametro.Size = longitud;
            parametro.Scale = escala;
            parametro.Precision = prec;
            parametro.Direction = Direccion;
            _parametros.Add(parametro);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Ejecuta un procedimiento en SQL con parametros de salida
    /// </summary>
    /// <param name="nombreProcedimiento">Nombre del procedimiento almacenado</param>
    /// <param name="TipoConsulta">Tipo de Comando</param>
    /// <param name="timeout">Tiempo de espera de la consulta (Opcional)</param>
    /// <remarks></remarks>
    private Resultados procedimientoParametros(string nombreProcedimiento, CommandType tipoConsulta, int timeOut = 1000)
    {

        SqlCommand comando = new SqlCommand();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Resultados Resultado = new Resultados();
        try
        {
            comando.CommandTimeout = timeOut;
            comando.Connection = this._conexion;
            comando.CommandText = nombreProcedimiento;
            comando.CommandType = tipoConsulta;
            foreach (SqlParameter Parametro in _parametros)
            {
                comando.Parameters.Add(Parametro);
            }
            if (_transaccion == true)
            {
                comando.Transaction = _dataTransaccion;
            }
            comando.ExecuteNonQuery();
            foreach (SqlParameter Parametro in _parametros)
            {
                Parametros.Add(Parametro.ParameterName, Parametro);
            }
            Resultado.parametros = Parametros;
            return Resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            comando = null;
            _parametros.Clear();
        }

    }

    /// <summary>
    /// Ejecuta un procedimiento o función en SQL con parametros y retorna un DataTable
    /// </summary>
    /// <param name="nombreProcedimietno">Nombre del procedimiento almacenado o función</param>
    /// <param name="TipoConsulta">Tipo de Comando</param>
    /// <param name="timeout">Tiempo de espera de la consulta (Opcional)</param>
    /// <returns></returns>
    /// <remarks></remarks>
    private Resultados procedimientoTablas(string nombreProcedimietno, CommandType tipoConsulta, int timeOut = 1000)
    {

        SqlCommand comando = new SqlCommand();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        DataSet Datos = new DataSet();
        List<DataTable> Tablas = new List<DataTable>();
        Resultados Resultado = new Resultados();
        try
        {
            comando.CommandTimeout = timeOut;
            comando.Connection = this._conexion;
            comando.CommandText = nombreProcedimietno;
            comando.CommandType = tipoConsulta;
            foreach (SqlParameter Parametro in _parametros)
            {
                comando.Parameters.Add(Parametro);
            }
            if (_transaccion == true)
            {
                comando.Transaction = _dataTransaccion;
            }
            adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(Datos);
            foreach (DataTable Tabla in Datos.Tables)
            {
                Tablas.Add(Tabla);
            }
            Resultado.tablas = Tablas;
            return Resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            comando = null;
            _parametros.Clear();
        }
    }

    /// <summary>
    /// Ejecuta un procedimiento o función en SQL con parametros y retorna un DataTable
    /// </summary>
    /// <param name="nombreProcedimietno">Nombre del procedimiento almacenado o función</param>
    /// <param name="TipoConsulta">Tipo de Comando</param>
    /// <param name="timeout">Tiempo de espera de la consulta (Opcional)</param>
    /// <returns></returns>
    /// <remarks></remarks>
    private Resultados procedimientoMixto(string nombreProcedimietno, CommandType tipoConsulta, int timeOut = 1000)
    {
        if (this._conexion.State != ConnectionState.Open)
        {
            throw new Exception("La conexión a la base de datos debe estar abierta y se encuentra en estado \"" + this._conexion.State + "\".");
        }

        SqlCommand comando = new SqlCommand();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        DataSet Datos = new DataSet();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        List<DataTable> Tablas = new List<DataTable>();
        Resultados Resultado = new Resultados();

        try
        {
            comando.CommandTimeout = timeOut;
            comando.Connection = this._conexion;
            comando.CommandText = nombreProcedimietno;
            comando.CommandType = tipoConsulta;
            foreach (SqlParameter Parametro in _parametros)
            {
                comando.Parameters.Add(Parametro);
            }
            if (_transaccion == true)
            {
                comando.Transaction = _dataTransaccion;
            }
            adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(Datos);
            foreach (SqlParameter Parametro in _parametros)
            {
                Parametros.Add(Parametro.ParameterName.ToString(), Parametro);
            }
            foreach (DataTable Tabla in Datos.Tables)
            {
                Tablas.Add(Tabla);
            }
            Resultado.parametros = Parametros;
            Resultado.tablas = Tablas;
            return Resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            comando = null;
            _parametros.Clear();
        }
    }

    public void pa_agrParam(string ps_paramName, object po_paramSqlSrvDbType, ParameterDirection po_paramDirecccion, object po_paramValor = null, Int32 ps_paramSize = -1, object ps_paramPrec = null, object ps_paramScal = null)
    {
        System.Data.SqlDbType lo_paramSqlSrvDbType = (System.Data.SqlDbType)po_paramSqlSrvDbType;
        //Dim ls_paramSize As Integer
        this.agregarParametros(ps_paramName, lo_paramSqlSrvDbType, po_paramValor, ps_paramSize, po_paramDirecccion);
    }
    void IAccesoBD.agregarParametro(string nombreParam, object BDtipoParam, ParameterDirection direccionParam, object valorParam, Int32 longitudParam, object precision, object escala)
    {
        this.pa_agrParam(nombreParam, BDtipoParam, direccionParam, valorParam, longitudParam, precision, escala);             
    }



    public void pa_agrParam(string ps_paramName, ParameterDirection po_paramDirecccion, object po_paramSqlSrvDbType, object po_paramValor = null, Int32 ps_paramSize = -1, object ps_paramPrec = null, object ps_paramScal = null)
    {
        System.Data.SqlDbType lo_paramSqlSrvDbType = (System.Data.SqlDbType)po_paramSqlSrvDbType;
        //Dim ls_paramSize As Integer  
        this.agregarParametros(ps_paramName, lo_paramSqlSrvDbType, po_paramValor, ps_paramSize, po_paramDirecccion);
    }



    void IAccesoBD.agregarParametro(string ps_paramName, ParameterDirection po_paramDirecccion, object po_paramSqlSrvDbType, object po_paramValor = null, Int32 ps_paramSize = -1, object ps_paramPrec = null, object ps_paramScal = null)
    {
        System.Data.SqlDbType lo_paramSqlSrvDbType = (System.Data.SqlDbType)po_paramSqlSrvDbType;
        this.pa_agrParam(ps_paramName, po_paramDirecccion, lo_paramSqlSrvDbType, po_paramValor, ps_paramSize, ps_paramPrec, ps_paramScal);
    }


    public List<DataTable> prOutTablas()
    {
        if (gv_tablas.Count > 0)
        {
            return gv_tablas;
        }
        else
        {
            throw new Exception("ERROR: No exísten datatables como parámetros de salida.");
            return null;
        }
    }
    List<DataTable> IAccesoBD.obtenerDatatables()
    {
        return prOutTablas();
    }

    public DataTable prOutTabla(string ps_nombreTabla) 
    {
        if (gv_tablas.Count > 0)
        {
            bool lb_encontroTabla = false;
            foreach (DataTable lo_tbl in gv_tablas)
            {
                if (lo_tbl.TableName == ps_nombreTabla)
                {
                    lb_encontroTabla = true;
                    return lo_tbl;
                }
            }
            if (!lb_encontroTabla)
            {
                throw new Exception("ERROR: Aunque exísten tablas en la lista, la tabla o conjunto de datos \"" + ps_nombreTabla + "\" no existe dentro de los parámetros de salida.");
            }
        }
        else
        {
            throw new Exception("ERROR: No exísten datatables como parámetros de salida.");
        }
        return null;
    }
    DataTable IAccesoBD.obtenerDatatable(string ps_nombreTabla)
    {
        return prOutTabla(ps_nombreTabla);
    }

    public DataTable prOutTabla(int pi_indiceTabla)
    {
        if (gv_tablas.Count > 0)
        {
            bool lb_encontroTabla = false;
            for (int i = 0; i <= gv_tablas.Count - 1; i++)
            {
                if (i == pi_indiceTabla)
                {
                    lb_encontroTabla = true;
                    return gv_tablas[i];
                }
            }
            if (!lb_encontroTabla)
            {
                throw new Exception("ERROR: Aunque exísten tablas en la lista, la tabla con el índice \"" + pi_indiceTabla.ToString() + "\" no existe dentro de los parámetros de salida.");
            }
        }
        else
        {
            throw new Exception("ERROR: No exísten datatables como parámetros de salida.");
        }
        return null;
    }
    DataTable IAccesoBD.obtenerDatatable(int pi_indiceTabla)
    {
        return prOutTabla(pi_indiceTabla);
    }

    public List<object> prOutParams()
    {
        if (gv_paramsDeSalida.Count > 0)
        {
            List<object> lv_obj = new List<object>();
            foreach (string key in gv_paramsDeSalida.Keys)
            {                
                lv_obj.Add(gv_paramsDeSalida[key]);
            }
            return lv_obj;
        }
        else
        {
            throw new Exception("ERROR: No exísten parámetros de salida.");
            return null;
        }
    }
    List<object> IAccesoBD.obtenerParams()
    {
        return prOutParams();
    }

    public object prOutParam(string ps_paramName)
    {
        if (gv_paramsDeSalida.Count > 0)
        {
            bool lb_encontroParam = false;
            List<object> lv_obj = new List<object>();
            foreach (string key in gv_paramsDeSalida.Keys)
            {
                
                if (ps_paramName == key)
                {
                    lb_encontroParam = true;
                    return gv_paramsDeSalida[key];
                }
            }
            if (!lb_encontroParam)
            {
                throw new Exception("ERROR: Aunque exísten parámetros de salida, el parámetro \"" + ps_paramName + "\" no existe dentro de los parámetros de salida.");
            }
        }
        else
        {
            throw new Exception("ERROR: No exísten parámetros de salida.");
        }
        return null;
    }
    object IAccesoBD.obtenerParam(string ps_paramName)
    {
        return prOutParam(ps_paramName);
    }

    public object prOutParam(int ps_paramIndex)
    {
        if (gv_paramsDeSalida.Count > 0)
        {
            bool lb_encontroParam = false;
            List<object> lv_obj = new List<object>();
            int li_c = 0;
            foreach (string key in gv_paramsDeSalida.Keys)
            {                
                if (ps_paramIndex == li_c)
                {
                    return gv_paramsDeSalida[key];
                }
                li_c += 1;
            }
            if (!lb_encontroParam)
            {
                throw new Exception("ERROR: Aunque exísten parámetros de salida, el parámetro con índice \"" + ps_paramIndex.ToString() + "\" no existe dentro de los parámetros de salida.");
            }
        }
        else
        {
            throw new Exception("ERROR: No exísten parámetros de salida.");
        }
        return null;
    }
    object IAccesoBD.obtenerParam(int ps_paramIndex)
    {
        return prOutParam(ps_paramIndex);
    }

    public bool conexionTieneTransaccion()
    {
        return _transaccion;
    }

    public string pa_Commit()
    {
        if (_transaccion)
        {
            this.commit();
            _transaccion = false;
            return "OK";
        }
        else
        {
            throw new Exception("ERROR: La instancia actual de la clase DataSql no se le especificó transacciones (intento de \"commit\").");
        }
        return "ERR";
    }
    string IAccesoBD.hacerCommit()
    {
        return pa_Commit();
    }

    public string pa_Rollback()
    {
        if (_transaccion)
        {
            this.rollback();
            _transaccion = false;
            return "OK";
        }
        else
        {
            throw new Exception("ERROR: La instancia actual de la clase DataSql no se le especificó transacciones (intento de \"rollback\").");
        }
        return "ERR";
    }
    string IAccesoBD.hacerRollback()
    {
        return pa_Rollback();
    }

    public string prEjecutar(string ps_procName, System.Data.CommandType po_tipodeComando = CommandType.StoredProcedure, int pi_cmdTimeout = 1000)
    {
        string resp = "OK";
        try
        {
            gv_tablas = new List<DataTable>();
            gv_paramsDeSalida = new Dictionary<string, object>();
            if (po_tipodeComando == CommandType.StoredProcedure | po_tipodeComando == CommandType.Text)
            {
                Resultados resultados = this.procedimientoMixto(ps_procName, po_tipodeComando, pi_cmdTimeout);
                foreach (DataTable tbl in resultados.tablas)
                {
                    gv_tablas.Add(tbl);
                }
                foreach (KeyValuePair<string, object> @params in resultados.parametros)
                {
                    if (((System.Data.SqlClient.SqlParameter)@params.Value).Direction != ParameterDirection.Input)
                    {
                        gv_paramsDeSalida.Add(@params.Key, ((System.Data.SqlClient.SqlParameter)@params.Value).Value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            resp = ex.Message;
        }
        return resp;
    }
    string IAccesoBD.ejecutar(string ps_procName, System.Data.CommandType po_tipodeComando = CommandType.StoredProcedure, int pi_cmdTimeout = 1000)
    {
        return prEjecutar(ps_procName, po_tipodeComando, pi_cmdTimeout);
    }




    #region "Dispose"
    bool disposedValue = false;
    public void Dispose()
    {
        Dispose(true);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
                _conexion.Dispose();
                if (_dataTransaccion != null)
                {
                    _dataTransaccion.Dispose();
                }
                disposedValue = true;
            }
            _parametros = null;
            _conexion = null;
            _resultados = null;
            _dataTransaccion = null;
            gv_tablas = null;
            gv_paramsDeSalida = null;
            //this.finalize();
        }
    }

    public void agregarParametro(string nombreParam, object BDtipoParam, ParameterDirection direccionParam, object valorParam = null, object longitudParam = null, object precision = null, object escala = null)
    {
        throw new NotImplementedException();
    }

    public void agregarParametro(string nombreParam, ParameterDirection direccionParam, object BDtipoParam, object valorParam = null, object longitudParam = null, object precision = null, object escala = null)
    {
        throw new NotImplementedException();
    }
    #endregion

}