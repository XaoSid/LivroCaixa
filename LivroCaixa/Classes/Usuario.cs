using prjLivroCaixa.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public class Usuario
{
    private static string nomeTabela = "Usuario";
    private static string nomeId = "id" + nomeTabela;
    private static string campos = "nome, login, senha, perfil, cpf";

    public int idUsuario;
    public String login { get; set; }
    public String nome { get; set; }
    public String senha { get; set; }
    public String perfil { get; set; }
    public String cpf { get; set; }
    public Usuario(String nome, String login, String senha, String perfil, String cpf)
    {
        this.nome = nome.Trim();
        this.login = login.Trim();
        this.senha = senha.Trim();
        this.perfil = perfil.Trim();
        this.cpf = cpf.Trim();
    }

    public static List<Usuario> listaUsuarios(Conexao con)
    {
        List<Usuario> lista = new List<Usuario>();

        string sql = String.Concat("SELECT " , nomeId , " from " , nomeTabela , " ORDER BY nome");
      

        DataTable dt = Conexao.executaSelect(con, sql,null);
        DataRow[] r = dt.Select();

        foreach (DataRow d in r)
        {
            lista.Add(Usuario.busca(d[0].ToString(), con));
        }     

        return lista;
    }

    public static List<Usuario> listaUsuarios(string like, Conexao con)
    {
        List<Usuario> lista = new List<Usuario>();

        string [] args = {"%" + like + "%"};

        string sql = String.Concat("SELECT ", nomeId, " from ", nomeTabela, " Where nome like @1 OR login like @1 ");


        DataTable dt = Conexao.executaSelect(con, sql,args);
        DataRow[] r = dt.Select();

        foreach (DataRow d in r)
        {
            lista.Add(Usuario.busca(d[0].ToString(), con));
        }

        return lista;
    }

    public string montaTabela(String titulo, Conexao con) // "nome, login, senha, perfil, cpf";
    {

        List<Usuario> lista = listaUsuarios(con);

        Tabela tabela = new Tabela(lista.Count, 6, titulo);

        String[] titulos = { "Seq.", "Nome", "Login", "Senha", "Perfil", "CPF" };

        int i = 0;
        

        foreach (Usuario l in lista)
        {           
            tabela.celula[i, 0] = l.idUsuario.ToString("D4");
            tabela.celula[i, 1] = l.nome;
            tabela.celula[i, 2] = l.login;
            tabela.celula[i, 3] = l.senha == l.cpf ? l.cpf:"Já trocada";
            tabela.celula[i, 4] = l.perfil;
            tabela.celula[i, 5] = l.cpf;
            i++;
        }

        return tabela.tabela(titulos);
    }

    public static Usuario busca(string id, Conexao con)
    {
        int iId;
        if (Int32.TryParse(id, out iId))
        {
            return busca(iId, con);
        }
        else
        {
            throw new Exception("Erro inesperado, id passado como texto inválido: " + nomeTabela);
        }
    }

    public static Usuario busca(String login, String senha, Conexao con)
    {
        try
        {
            string []args = {login, senha};
            String sql = String.Concat("SELECT ", campos, ",idUsuario FROM ", nomeTabela,
                " WHERE login=@1  AND senha=@2");
            DataTable dt = Conexao.executaSelect(con, sql,args);
            if (dt.Rows.Count == 0) return null;
            DataRow[] r = dt.Select();
            Usuario item = new Usuario(r[0][0].ToString(), r[0][1].ToString(), r[0][2].ToString(), 
                r[0][3].ToString(), r[0][4].ToString());
            item.idUsuario = Int16.Parse(r[0][5].ToString());
            return item;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static Usuario busca(Conexao con, String login)
    {
        try
        {
            String []args = {login};

            String sql = String.Concat("SELECT ", campos, ", idUsuario FROM ", nomeTabela, 
                " WHERE login=@1");
            DataTable dt = Conexao.executaSelect(con, sql,args);
            if (dt.Rows.Count == 0) return null;
            DataRow[] r = dt.Select();
            Usuario item = new Usuario(r[0][0].ToString(), r[0][1].ToString(), r[0][2].ToString(), 
                r[0][3].ToString(), r[0][4].ToString());
            item.idUsuario = Int16.Parse(r[0][5].ToString());
            return item;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int insere(Conexao con)
    {
        try
        {
            String[] args = { nome, login, senha, perfil, cpf };
            String sql = String.Concat("INSERT INTO ", nomeTabela, 
                " (", campos, ") Values (@1,@2,@3,@4,@5)");
            idUsuario = Conexao.executaQuery(con, sql, nomeTabela,args);
            return idUsuario;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int exclui(Conexao con)
    {
        try
        {
            String sql = String.Concat("DELETE FROM ", nomeTabela, 
                " WHERE idUsuario=" , idUsuario);
            return Conexao.executaQuery(con, sql, nomeTabela,null);            
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int atualiza(Conexao con)
    {
        try
        {
            string[] args = { nome, login, senha, perfil, cpf };

            StringBuilder sql = new StringBuilder("UPDATE " + nomeTabela + " SET ");
            sql.Append("nome=@1,");
            sql.Append("login=@2,");
            sql.Append("senha=@3,");
            sql.Append("perfil=@4,");
            sql.Append("cpf=@5 ");
            sql.Append(" WHERE id" + nomeTabela + "=" + idUsuario);

            Conexao.executaQuery(con, sql.ToString(), nomeTabela,args);

            return idUsuario;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static Usuario busca(int id, Conexao con)
    {
        try
        {
            String sql = String.Concat("SELECT ", campos, " FROM ", nomeTabela, " WHERE ", 
                nomeId, "=", id);
            DataTable dt = Conexao.executaSelect(con, sql,null);
            if (dt.Rows.Count == 0) return null;
            DataRow[] r = dt.Select();
            Usuario item = new Usuario(r[0][0].ToString(), r[0][1].ToString(), 
                                 r[0][2].ToString(), r[0][3].ToString(), r[0][4].ToString());
            item.idUsuario = id;
            return item;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
 
