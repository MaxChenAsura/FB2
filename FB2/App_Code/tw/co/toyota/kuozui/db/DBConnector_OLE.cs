using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

/// <summary>
/// DBConnector_OLE 的摘要描述
/// </summary>
public class DBConnector_OLE
{

    private OleDbConnection connect;
    private OleDbTransaction trans;
    private OleDbCommand comm;
    private string connectString;
    private const int dbExecTime = 500;
    ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    ILog Tracelogger = LogManager.GetLogger("TraceSQL");


    public DBConnector_OLE(string connstr)
    {
        createConnection(connstr);
    }



    private void createConnection(string connstr)
    {
        connectString = connstr;
        connect = new OleDbConnection(connectString);
        comm = new OleDbCommand();
    }

    private bool checkConn()
    {
        //檢查DB狀態
        if (connect != null)
        {
            if (connect.ConnectionString == "")
                connect.ConnectionString = connectString;
            if (connect.State == System.Data.ConnectionState.Closed)
            {
                connect.Open();

            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //建立transaction
    public void beginTransaction()
    {
        try
        {
            if (checkConn())
            {
                trans = connect.BeginTransaction();
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }

    }

    //認可交易
    public void commitTransaction()
    {
        try
        {
            trans.Commit();
            
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }
    }

    //取消交易
    public void rollbackTransaction()
    {
        try
        {
            trans.Rollback();
            
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }
    }


    public void connectionClose()
    {
        try
        {
            if (connect != null)
            {
                connect.Close();
                connect.Dispose();
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
    }

    public bool executeNonQuery(OleDbCommand sqlcomm)
    {
        try
        {
            if (checkConn())
            {
                Tracelogger.Info(sqlcomm.CommandText);
                comm = sqlcomm;
                comm.Connection = connect;
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                comm.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                if (executionTime.Milliseconds > dbExecTime)
                    logger.Warn(sqlcomm.CommandText);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }
    }

    public bool executeScalar(OleDbCommand sqlcomm, ref string rtnval)
    {
        try
        {
            if (checkConn())
            {
                comm = sqlcomm;
                comm.Connection = connect;
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                rtnval = Convert.ToString(comm.ExecuteScalar());
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                if (executionTime.Milliseconds > dbExecTime)
                    logger.Warn(sqlcomm.CommandText);

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }
    }

    public bool executeNonQueryWithTrans(OleDbCommand sqlcomm)
    {
        try
        {
            if (checkConn())
            {

                comm = sqlcomm;
                comm.Connection = connect;
                comm.Transaction = trans;
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                comm.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                if (executionTime.Milliseconds > dbExecTime)
                    logger.Warn(sqlcomm.CommandText);

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
    }

    public bool executeScalarWithTrans(OleDbCommand sqlcomm, ref string rtnval)
    {
        try
        {
            if (checkConn())
            {
                comm = sqlcomm;
                comm.Connection = connect;
                comm.Transaction = trans;
                comm.ExecuteScalar();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
    }

    public DataTable getDataTable(OleDbCommand sqlcomm)
    {
        try
        {
            if (checkConn())
            {
                comm = sqlcomm;
                comm.Connection = connect;

                OleDbDataAdapter sda = new OleDbDataAdapter(comm);
                DataTable dt = new DataTable();
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                sda.Fill(dt);
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                if (executionTime.Milliseconds > dbExecTime)
                    logger.Warn(sqlcomm.CommandText);
                //ConnectionClose();

                return dt;
            }
            else
                return null;


        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }
    }

    public List<string> getStrList(OleDbCommand sqlcomm)
    {
        List<string> rtnVal = new List<string>();
        OleDbDataReader dr = null;
        try
        {
            if (checkConn())
            {
                comm = sqlcomm;
                comm.Connection = connect;

                dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    rtnVal.Add(dr[0].ToString());
                }

                dr.Close();

                return rtnVal;
            }
            else
                return null;


        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }

    }

    
    public int pageCount(OleDbCommand sqlcomm)
    {
        try
        {
            if (checkConn())
            {
                comm = sqlcomm;
                comm.Connection = connect;
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                comm.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                if (executionTime.Milliseconds > dbExecTime)
                    logger.Warn(sqlcomm.CommandText);
                int rtnval = (int)comm.Parameters["@total_count"].Value;

                connectionClose();

                return rtnval;
            }
            else
                return 0;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            throw;
        }
        finally
        {
            connectionClose();
        }
    }

    public bool executeMultiWithTrans(List<OleDbCommand> sqlcomm, ref string ErrorMsg)
    {
        try
        {
            if (sqlcomm.Count() > 0)
            {
                beginTransaction();
                foreach (OleDbCommand comm in sqlcomm)
                {
                    executeNonQueryWithTrans(comm);
                }

                commitTransaction();
            }
            else
                ErrorMsg = "無可執行sql";
            return true;
        }
        catch (Exception ex)
        {
            rollbackTransaction();
            //logger.Error(ex.Message);
            ErrorMsg = ex.Message;
            return false;
        }

    }
}