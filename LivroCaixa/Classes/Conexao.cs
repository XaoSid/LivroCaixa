
using Microsoft.Win32.SafeHandles;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;

public class Conexao : System.Web.UI.Page, IDisposable
{

    private SqlConnection conn;
    public Usuario usuario { get; set; }
    public static string stringConexao()
    {
        // Este string de conexão deverá ser modificado, no seu caso o banco não estará aqui. este é o endereço do banco no "meu" caso.
        // Na aba Server Exporer clique no ícone Conect to Database (uma tomadinha com um + do lado esquerdo) (Se a aba 'Server explorer' não estiver aparecendo clique em TOOLS -> Conect to Database)
        // Navegue até encontrar o banco na pasta Banco do seu projeto e confirme
        // Com o botão esquerdo do mouse sobre o banco "conectado" selecione a opção  "Properties"
        // Copie a propriedade Connection String e substitua a string abaixo pela nova;
        // Não esqueça de acrescentar no final:  ";Language=Portuguese"

        return @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Helio\Documents\Visual Studio 2013\Projects\LivroCaixa\LivroCaixa\Banco\LivroCaixa.mdf;Integrated Security=True;Connect Timeout=30;Language=Portuguese";
     // return @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Helio\Documents\Visual Studio 2013\Projects\LivroCaixa\LivroCaixa\Banco\LivroCaixa.mdf;Integrated Security=True;Connect Timeout=30;Language=Portuguese";
    
    }

    private SqlTransaction transaction;    

    public void beginTransaction()
    {
        try
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
            transaction = conn.BeginTransaction("FluxoCaixa");           
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void rollback()
    {
        try
        {
            transaction.Rollback();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void commit()
    {
        try
        {
            transaction.Commit();
        }
        catch (Exception)
        {
            throw;
        }
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
    public static DataTable executaSelect(Conexao con, string sql, String[] args)  {
        try
        {
            DataTable dt = new DataTable();
            SqlCommand comando = new SqlCommand(sql, con.con());
            comando.Transaction = con.getTransaction();
            comando.CommandType = CommandType.Text;
            comando.Prepare();      
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comando;
            if (args != null)  {
                int chave = 1;
                foreach (string s in args) {
                    string seq = "@" + chave++;
                    comando.Parameters.AddWithValue(seq, s);
                }
            }
            try   {
                da.Fill(dt);
            }            
            finally  {
                dt.Dispose();
                da.Dispose();
                comando.Dispose();
            }
            return dt;
        }
        catch (Exception)  {
            throw;
        }
    }

    public SqlTransaction getTransaction()
    {      
        return transaction;
    }

    private static int executaQuery(Conexao con, string sql, string[] args)
    {
        int ret = 0;
       
        try
        {
            SqlCommand comando = new SqlCommand(sql, con.con());
            comando.CommandType = CommandType.Text;
            comando.Prepare();            
            comando.Transaction = con.getTransaction();          
            if (args != null)
            {
                int chave = 1;
                foreach (string s in args)
                {
                    string seq = "@" + chave++;
                    comando.Parameters.AddWithValue(seq, s);
                }
            }
            ret = comando.ExecuteNonQuery();
        }       
        catch (Exception)
        {
            throw;
        }       

        return ret;
    }

    public static int executaQuery(Conexao con, string sql, String nomeTabelaCompleto, string[] args)
    {
        int ret = 0;

        try
        {
             

            SqlCommand comando = new SqlCommand(sql, con.con());
            comando.CommandType = CommandType.Text;
            comando.Transaction = con.getTransaction();
            comando.Prepare();
           
            if (args != null)
            {
                int chave = 1;
                foreach (string s in args)
                {
                    string seq = "@" + chave++;
                    comando.Parameters.AddWithValue(seq, s);
                }
            }
          
            ret = comando.ExecuteNonQuery();

            if (ret > 0)
            {
                DataTable dt = Conexao.executaSelect(con, "SELECT IDENT_CURRENT('" + nomeTabelaCompleto + "')",null);
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