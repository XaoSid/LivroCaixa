using System;
using System.Data;
using System.Text;

public class Lancamento : IComparable<Lancamento>
{
    private static string nomeTabela = "Lancamento";
    private static string nomeId = "id" + nomeTabela;
    private static string campos = "descricao,idLivroCaixa,valor,tipoLancamento,idUsuarioResponsavel,data";

    public int idLancamento { get; set; }
    public String descricao { get; set; }
    public int idLivroCaixa { get; set; }
    public double valor { get; set; }
    public char tipoLancamento { get; set; }
    public Usuario usuarioResponsavel { get; set; }

    public DateTime data { get; set; }
    public Lancamento(String descricao, int idLivroCaixa, double valor, char tipoLancamento, Usuario usuarioResponsavel, DateTime data)
    {
        this.descricao = descricao.Trim();
        this.idLivroCaixa = idLivroCaixa;
        this.valor = valor;
        this.tipoLancamento = tipoLancamento;
        this.usuarioResponsavel  = usuarioResponsavel;
        this.data = data;
    }

    public int CompareTo(Lancamento y)
    {
        return data.CompareTo(y.data);
    }

    public int insere(Conexao con)  
    {
        try
        {       
            String []args = {descricao};
            String sql = String.Concat("INSERT INTO ", nomeTabela, " (", campos, ") Values (@1,'", idLivroCaixa, "','", 
                (valor+"").Replace(",","."), "','", tipoLancamento , "','", usuarioResponsavel.idUsuario, "','", 
                data.ToString("dd/MM/yyyy hh:mm:ss") , "')");
            idLancamento = Conexao.executaQuery(con, sql, nomeTabela,args);
            return idLancamento;
        }
        catch (Exception)
        {
            throw;
        }
    }
   
    public static Lancamento busca(string id, Conexao con)  
    {
        try
        {
            String sql = String.Concat("SELECT ", campos, " FROM ", nomeTabela, " WHERE ", nomeId, "=", id);
            DataTable dt = Conexao.executaSelect(con, sql,null);
            if (dt.Rows.Count == 0) throw new Exception("Erro inesperado, id" + nomeTabela + " não encontrado");
            DataRow[] r = dt.Select();  
            int idLivroCaixa = Int32.Parse(r[0][1].ToString());
            Usuario usuarioResponsavel = Usuario.busca(Int32.Parse(r[0][4].ToString()), con);  
            Lancamento item = new Lancamento(r[0][0].ToString(), idLivroCaixa, Double.Parse(r[0][2].ToString()), r[0][3].ToString().ToCharArray()[0], usuarioResponsavel , DateTime.Parse(r[0][5].ToString()));
            item.idLancamento = Int16.Parse(id);
            return item;
        }
        catch (Exception)
        {
            throw;
        }
    }
}

