
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

public class Conexao  
{
    private SqlConnection conn;
    public Usuario usuario { get; set; }
    public static string stringConexao()
    {
        return @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Helio\Documents\LivroCaixa.mdf;Integrated Security=True;Connect Timeout=30;Language=Portuguese";
    }

    override public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    bool disposed = false;
    SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            handle.Dispose();
            close();
        }

        disposed = true;
    }

    public Conexao(Usuario usuario)
    {
        try
        {
            this.usuario = usuario;
            conn = new SqlConnection(stringConexao());
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void open()
    {
        conn.Open();
    }
    public void close()
    {
        conn.Close();
    }
    public SqlConnection con()
    {
        return conn;
    }
    public static DataTable executaSelect(Conexao con, string sql)
    {

        try
        {
            DataTable dt = null;

            dt = new DataTable();

            SqlCommand comando = new SqlCommand(sql, con.con());

            comando.CommandType = CommandType.Text;

            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = comando;

            try
            {
                da.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dt.Dispose();
                da.Dispose();
                comando.Dispose();
            }

            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static int executaQuery(Conexao con, string sql, String nomeTabelaCompleto)
    {
        int ret = 0;

        try
        {
            SqlCommand comando = new SqlCommand(sql, con.con());
            comando.CommandType = CommandType.Text;
            comando.Prepare();
            ret = comando.ExecuteNonQuery();

            if (ret > 0)
            {
                DataTable dt = Conexao.executaSelect(con, "SELECT IDENT_CURRENT('" + nomeTabelaCompleto + "')");
                DataRow[] result = dt.Select();
                return result.Count() > 0 ? Convert.ToInt32(result[0][0].ToString()) : -1;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return ret;
    }
}