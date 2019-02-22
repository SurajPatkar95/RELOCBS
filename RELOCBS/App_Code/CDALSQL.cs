using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace RELOCBS.App_Code
{
    public class COMMON
    {
        protected Boolean vIsConnected = false;
        protected Boolean vIsTransacted = false;
        protected Boolean vIsDisposed = false;
        protected IsolationLevel vIsolationLevel;

        protected String vSelectQuery;
        protected String vProcedureQuery;
        protected String vErrorMessage;

        protected Int32 vTimeOutValue;
    }

    public class CDALSQL : COMMON, IDisposable
    {
        protected SqlConnection vConn;
        protected SqlTransaction vTran;
        protected SqlCommand vCmdQuery;
        protected SqlCommand vCmdProc;
        protected SqlCommand vCmdCommon;
        protected SqlDataAdapter vDataAdapter;
        protected SqlDataReader vDataReader;

        #region Initialization

        public CDALSQL(String PrmConn)
        {
            try
            {
                vTimeOutValue = 0;
                vIsTransacted = false;
                vIsolationLevel = IsolationLevel.ReadCommitted;
                vConn = new SqlConnection(PrmConn);
            }
            catch (Exception ex) { }
        }

        public CDALSQL(String PrmConn, bool PrmIsTransacted, IsolationLevel PrmIsolation)
        {
            try
            {
                vTimeOutValue = 0;
                vIsTransacted = PrmIsTransacted;
                vIsolationLevel = PrmIsolation;
                vConn = new SqlConnection(PrmConn);
            }
            catch (Exception ex) { }
        }

        ~CDALSQL()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (vIsDisposed == false)
            {
                if (vIsTransacted == true)
                {
                    try
                    {
                        vTran.Rollback();
                        vTran.Dispose();
                    }
                    catch { }
                }

                CloseReader();

                try
                {
                    vDataReader.Dispose();
                    vDataAdapter.Dispose();
                    vCmdQuery.Dispose();
                    vCmdProc.Dispose();
                    vCmdCommon.Dispose();
                }
                catch { }

                try
                {
                    if (vConn != null)
                    {
                        if (vConn.State == ConnectionState.Open) vConn.Close();
                    }
                    vConn.Dispose();
                }
                catch { }
                vIsDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Properties

        public Int32 CommandTimeout
        {
            get { return vTimeOutValue; }
            set { vTimeOutValue = value; }
        }

        public Boolean IsConnected
        {
            get { return vIsConnected; }
        }

        public String ErrorMessage
        {
            get { return vErrorMessage; }
        }

        public bool IsError
        {
            get { return (String.IsNullOrWhiteSpace(vErrorMessage)) ? false : true; }
        }

        #endregion

        #region Private Methods

        private void InitilizeClass()
        {
            vCmdQuery = new SqlCommand();
            vCmdProc = new SqlCommand();
            vCmdCommon = new SqlCommand();

            vCmdQuery.Connection = vConn;
            vCmdProc.Connection = vConn;
            vCmdCommon.Connection = vConn;

            if (vIsTransacted == true)
            {
                vTran = vConn.BeginTransaction(vIsolationLevel);
                vCmdQuery.Transaction = vTran;
                vCmdProc.Transaction = vTran;
                vCmdCommon.Transaction = vTran;
            }

            vCmdQuery.CommandType = CommandType.Text;
            vCmdProc.CommandType = CommandType.StoredProcedure;
            vCmdCommon.CommandType = CommandType.Text;

            vCmdQuery.CommandTimeout = vTimeOutValue;
            vCmdProc.CommandTimeout = vTimeOutValue;
            vCmdCommon.CommandTimeout = vTimeOutValue;

            vDataAdapter = new SqlDataAdapter();

            CloseReader();
        }

        private DataTable GetDataTable(SqlCommand PrmSqlCommand, String PrmTableName)
        {
            DataTable dtTemp = new DataTable(PrmTableName);
            try
            {
                vDataAdapter.SelectCommand = PrmSqlCommand;
                vDataAdapter.Fill(dtTemp);
            }
            catch (Exception ex)
            {
                throw;
            }

            return (dtTemp);
        }

        private DataSet GetDataSet(SqlCommand PrmSqlCommand, String PrmTableName)
        {
            DataSet dtTemp = new DataSet(PrmTableName);
            try
            {
                vDataAdapter.SelectCommand = PrmSqlCommand;
                vDataAdapter.Fill(dtTemp);
            }
            catch (Exception ex)
            {
                throw;
            }

            return (dtTemp);
        }

        private Object GetSingleValue(SqlCommand PrmSqlCommand, String PrmCalledfrom)
        {
            Object varTemp = null;
            try
            {
                if (PrmCalledfrom == "ExecuteQuery")
                {
                    if (vSelectQuery.Substring(0, 1).ToUpper() == "S")
                        varTemp = PrmSqlCommand.ExecuteScalar();
                    else
                        varTemp = PrmSqlCommand.ExecuteNonQuery();
                }
                else
                {
                    varTemp = PrmSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return (varTemp);
        }

        #endregion

        #region Public Methods

        public Boolean Connect()
        {
            try
            {
                vConn.Open();
                InitilizeClass();
                vIsConnected = true;
            }
            catch (Exception ex)
            {
                vIsConnected = false;
                vErrorMessage = ex.Message;
            }

            return vIsConnected;
        }

        public void AddCommand(String PrmStrSql, [Optional, DefaultParameterValue(QueryType.QueryText)] QueryType PrmQueryType)
        {
            if (PrmQueryType == QueryType.QueryText)
                vSelectQuery = PrmStrSql.Trim();
            else
                vProcedureQuery = PrmStrSql.Trim();
        }

        public Object ExecuteQuery([Optional, DefaultParameterValue(QueryReturnType.DataTable)] QueryReturnType PrmReturnType, [Optional, DefaultParameterValue("DATA")] String PrmTableName)
        {
            Object varTemp = null;
            vErrorMessage = "";
            try
            {
                if (vSelectQuery != "")
                {
                    vCmdQuery.CommandText = vSelectQuery;
                    if (PrmReturnType == QueryReturnType.DataTable)
                        varTemp = GetDataTable(vCmdQuery, PrmTableName);
                    else if (PrmReturnType == QueryReturnType.DataSet)
                        varTemp = GetDataSet(vCmdQuery, PrmTableName);
                    else if (PrmReturnType == QueryReturnType.SingleValue)
                        varTemp = GetSingleValue(vCmdQuery, "ExecuteQuery");
                }
                else
                {
                    Exception ex = new Exception("No SQL Statements found");
                }
            }
            catch (Exception ex)
            {
                vErrorMessage = ex.Message;
            }

            return (varTemp);
        }

        public Object ExecuteProcedure([Optional, DefaultParameterValue(ProcedureReturnType.DataTable)] ProcedureReturnType PrmReturnType, [Optional, DefaultParameterValue("DATA")] String PrmTableName)
        {
            Object varTemp = null;
            vErrorMessage = "";
            try
            {
                if (vProcedureQuery != "")
                {
                    vCmdProc.CommandText = vProcedureQuery;
                    if (PrmReturnType == ProcedureReturnType.DataTable)
                        varTemp = GetDataTable(vCmdProc, PrmTableName);
                    else if (PrmReturnType == ProcedureReturnType.DataSet)
                        varTemp = GetDataSet(vCmdProc, PrmTableName);
                    else if (PrmReturnType == ProcedureReturnType.SingleValue)
                        varTemp = GetSingleValue(vCmdProc, "ExecuteProcedure");
                }
                else
                {
                    Exception ex = new Exception("No SQL Statements found");
                }
            }
            catch (Exception ex)
            {
                vErrorMessage = ex.Message;
            }

            return (varTemp);
        }

        public Boolean CheckValue(String PrmStrSql)
        {
            SqlDataReader sRdr = null;
            Boolean varTemp = false;
            PrmStrSql = PrmStrSql.Trim();
            try
            {
                if (PrmStrSql.Length > 0 && PrmStrSql.Substring(0, 1).ToUpper() == "S")
                {
                    vCmdCommon.CommandText = PrmStrSql;
                    sRdr = vCmdCommon.ExecuteReader();
                    varTemp = sRdr.HasRows;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (sRdr != null) sRdr.Close();
            }

            return (varTemp);
        }

        public Object GetValue(String PrmStrSql)
        {
            Object varTemp = null;
            PrmStrSql = PrmStrSql.Trim();
            try
            {
                if (PrmStrSql.Length > 0)
                {
                    vCmdCommon.CommandText = PrmStrSql;
                    varTemp = vCmdCommon.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return (varTemp);
        }

        public SqlDataReader GetReader(String PrmStrSql)
        {
            CloseReader();
            PrmStrSql = PrmStrSql.Trim();
            try
            {
                if (PrmStrSql.Length > 0 && PrmStrSql.Substring(0, 1).ToUpper() == "S")
                {
                    vCmdCommon.CommandText = PrmStrSql;
                    vDataReader = vCmdCommon.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return (vDataReader);
        }

        public void InitReader(String PrmStrSql)
        {
            CloseReader();
            PrmStrSql = PrmStrSql.Trim();
            try
            {
                if (PrmStrSql.Length > 0 && PrmStrSql.Substring(0, 1).ToUpper() == "S")
                {
                    vCmdCommon.CommandText = PrmStrSql;
                    vDataReader = vCmdCommon.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Boolean ReaderHasRows()
        {
            Boolean varTemp = false;
            try
            {
                varTemp = vDataReader.HasRows;
            }
            catch { }
            return (varTemp);
        }

        public Boolean ReadNextRow()
        {
            Boolean varTemp = false;
            try
            {
                varTemp = vDataReader.Read();
            }
            catch { }
            return (varTemp);
        }

        public Object GetValueReader(Int16 PrmIndex)
        {
            Object varTemp = null;
            try
            {
                varTemp = vDataReader.GetValue(PrmIndex);
            }
            catch { }
            return (varTemp);
        }

        public void CloseReader()
        {
            try
            {
                if (vDataReader != null)
                {
                    if (vDataReader.IsClosed == false)
                        vDataReader.Close();
                }
            }
            catch { }
            vDataReader = null;
        }

        public void AddParameter(ParameterOF PrmParameterOf, String PrmParamName, SqlDbType PrmType, [Optional, DefaultParameterValue((Int16)0)] Int16 PrmSize, [Optional, DefaultParameterValue(ParameterDirection.Input)] ParameterDirection PrmDirection, [Optional, DefaultParameterValue(null)] Object PrmValue, [Optional, DefaultParameterValue((Byte)18)] Byte PrmPrecision, [Optional, DefaultParameterValue((Byte)0)] Byte PrmScale)
        {
            try
            {
                if (PrmParameterOf == ParameterOF.PROCEDURE)
                {
                    if (PrmSize == 0)
                        vCmdProc.Parameters.Add(PrmParamName, PrmType);
                    else
                        vCmdProc.Parameters.Add(PrmParamName, PrmType, PrmSize);

                    vCmdProc.Parameters[PrmParamName].Direction = PrmDirection;
                    vCmdProc.Parameters[PrmParamName].Precision = PrmPrecision;
                    vCmdProc.Parameters[PrmParamName].Scale = PrmScale;

                    if (PrmValue == null || PrmValue == DBNull.Value)
                        vCmdProc.Parameters[PrmParamName].Value = DBNull.Value;
                    else
                    {
                        if (String.IsNullOrEmpty((String)PrmValue.ToString()) == true)
                            vCmdProc.Parameters[PrmParamName].Value = DBNull.Value;
                        else
                            vCmdProc.Parameters[PrmParamName].Value = PrmValue;
                    }
                }
                else if (PrmParameterOf == ParameterOF.QUERY)
                {
                    if (PrmSize == 0)
                        vCmdQuery.Parameters.Add(PrmParamName, PrmType);
                    else
                        vCmdQuery.Parameters.Add(PrmParamName, PrmType, PrmSize);

                    vCmdQuery.Parameters[PrmParamName].Direction = PrmDirection;

                    if (PrmValue == null || PrmValue == DBNull.Value)
                        vCmdQuery.Parameters[PrmParamName].Value = DBNull.Value;
                    else
                    {
                        if (String.IsNullOrEmpty((String)PrmValue.ToString()) == true)
                            vCmdQuery.Parameters[PrmParamName].Value = DBNull.Value;
                        else
                            vCmdQuery.Parameters[PrmParamName].Value = PrmValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public Object GetParameterValue(ParameterOF PrmParameterOf, Int16 PrmIndex)
        {
            Object varTemp = null;
            try
            {
                if (PrmParameterOf == ParameterOF.PROCEDURE)
                    varTemp = vCmdProc.Parameters[PrmIndex].Value;
                else if (PrmParameterOf == ParameterOF.QUERY)
                    varTemp = vCmdQuery.Parameters[PrmIndex].Value;
            }
            catch (Exception ex)
            {
                throw;
            }

            return (varTemp);
        }

        public Object GetParameterValue(ParameterOF PrmParameterOf, String PrmName)
        {
            Object varTemp = null;
            try
            {
                if (PrmParameterOf == ParameterOF.PROCEDURE)
                    varTemp = vCmdProc.Parameters[PrmName].Value;
                else if (PrmParameterOf == ParameterOF.QUERY)
                    varTemp = vCmdQuery.Parameters[PrmName].Value;
            }
            catch (Exception ex)
            {
                throw;
            }

            return (varTemp);
        }

        public void SetParameterValue(ParameterOF PrmParameterOf, Int16 PrmIndex, Object PrmValue)
        {
            try
            {
                if (PrmParameterOf == ParameterOF.PROCEDURE)
                {
                    if (PrmValue == null || PrmValue == DBNull.Value)
                        vCmdProc.Parameters[PrmIndex].Value = DBNull.Value;
                    else
                        vCmdProc.Parameters[PrmIndex].Value = PrmValue;
                }
                else if (PrmParameterOf == ParameterOF.QUERY)
                {
                    if (PrmValue == null || PrmValue == DBNull.Value)
                        vCmdQuery.Parameters[PrmIndex].Value = DBNull.Value;
                    else
                        vCmdQuery.Parameters[PrmIndex].Value = PrmValue;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SetParameterValue(ParameterOF PrmParameterOf, String PrmName, Object PrmValue)
        {
            try
            {
                if (PrmParameterOf == ParameterOF.PROCEDURE)
                {
                    if (PrmValue == null || PrmValue == DBNull.Value)
                        vCmdProc.Parameters[PrmName].Value = DBNull.Value;
                    else
                        vCmdProc.Parameters[PrmName].Value = PrmValue;
                }
                else if (PrmParameterOf == ParameterOF.QUERY)
                {
                    if (PrmValue == null || PrmValue == DBNull.Value)
                        vCmdQuery.Parameters[PrmName].Value = DBNull.Value;
                    else
                        vCmdQuery.Parameters[PrmName].Value = PrmValue;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CommitTransaction()
        {
            vErrorMessage = "";
            try
            {
                if (vIsTransacted == true)
                {
                    vTran.Commit();
                    vTran = vConn.BeginTransaction(vIsolationLevel);
                    vCmdQuery.Transaction = vTran;
                    vCmdProc.Transaction = vTran;
                    vCmdCommon.Transaction = vTran;
                }
            }
            catch { }
        }

        public void RollbackTransaction()
        {
            vErrorMessage = "";
            try
            {
                if (vIsTransacted == true)
                {
                    vTran.Rollback();
                    vTran = vConn.BeginTransaction(vIsolationLevel);
                    vCmdQuery.Transaction = vTran;
                    vCmdProc.Transaction = vTran;
                    vCmdCommon.Transaction = vTran;
                }
            }
            catch { }
        }

        public void ClearParameters(ParameterOF PrmParameterOf)
        {
            vErrorMessage = "";
            try
            {
                if (PrmParameterOf == ParameterOF.PROCEDURE)
                    vCmdProc.Parameters.Clear();
                else if (PrmParameterOf == ParameterOF.QUERY)
                    vCmdQuery.Parameters.Clear();
            }
            catch { }
        }

        #endregion

    }
}