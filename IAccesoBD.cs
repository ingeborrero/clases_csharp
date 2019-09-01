using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

    interface IAccesoBD:IDisposable
    {

        void abrirConexion(string connStr, bool usaTransaccion = false);

        void agregarParametro(string nombreParam, object BDtipoParam, System.Data.ParameterDirection direccionParam, object valorParam = null, object longitudParam = null, object precision = null, object escala = null);
        void agregarParametro(string nombreParam, System.Data.ParameterDirection direccionParam, object BDtipoParam, object valorParam = null, object longitudParam = null, object precision = null, object escala = null);

        void agregarParametro(string nombreParam, object BDtipoParam, System.Data.ParameterDirection direccionParam, object valorParam = null, Int32 longitudParam = -1, object precision = null, object escala = null);
        void agregarParametro(string nombreParam, System.Data.ParameterDirection direccionParam, object BDtipoParam, object valorParam = null, Int32 longitudParam = -1, object precision = null, object escala = null);

        string ejecutar(string procName, System.Data.CommandType tipodeComando = CommandType.StoredProcedure, int cmdTimeout = 1000);

        string iniciarTransaccion();
        string asignarConexion(string nuevaCadenaConn);
        string abrirConexion(bool usaTransaccion = false);
        string cerrarConexion();

        List<object> obtenerParams();
        object obtenerParam(int indice);
        object obtenerParam(string nombreParam);

        List<DataTable> obtenerDatatables();
        DataTable obtenerDatatable(int indice);
        DataTable obtenerDatatable(string nombreParam);
        bool conexionTieneTransaccion();
        string hacerCommit();
        string hacerRollback();
    }

