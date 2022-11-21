using prjLivroCaixa.Classes;
using System;
using System.Collections.Generic;
using System.Data;

public class LivroCaixa
{

    private static string nomeTabela = "LivroCaixa";
    private static string nomeId = "id" + nomeTabela;
    private static string campos = "valorTransportado, mes, ano, stLivroFechado";
    public int idLivroCaixa { get; set; }

    public double valorTransportado { get; set; }
    public string mes { get; set; }
    public string ano { get; set; }
    public bool stLivroFechado { get; set; }

    public double saldo  { get; set; } 

    private List<Lancamento> lancamentos;
  
    public static LivroCaixa busca(string id, Conexao con)
    {
        return new LivroCaixa(id, con);
    }

    /// <summary>
    /// Popula a lista de lançamentos do livro
    /// </summary>
    /// <param name="con"></param>
    /// <returns></returns>
    public List<Lancamento> listaLancamentos(Conexao con)
    {
        string sql = String.Concat("SELECT idLancamento from Lancamento where idLivroCaixa=" + idLivroCaixa + " ORDER BY data");
        lancamentos = new List<Lancamento>();

        try
        {
            DataTable dt = Conexao.executaSelect(con, sql,null);
            DataRow[] r = dt.Select();
            foreach (DataRow d in r)
            {
                lancamentos.Add(Lancamento.busca(d[0].ToString(), con));
            }
        }
        catch (Exception)
        {
            throw;
        }

        return lancamentos;
    }

    public void fechaPeriodo(Conexao con)  
    {
      try
      {
        con.beginTransaction(); //Afeta dois registros, por isso foi implementada transação
        string sql = String.Concat("UPDATE " , nomeTabela , " SET stLivroFechado=1 Where ",
            nomeId , "=" ,  idLivroCaixa);
               Conexao.executaQuery(con, sql, nomeTabela, null);
        DateTime proximoPeriodo = new DateTime(Int16.Parse(ano), 
                                                         Int16.Parse(mes), 1).AddMonths(1);
        sql = String.Concat("INSERT INTO  ", nomeTabela, " (", campos, ") Values (",
                (saldo + "").Replace(",", "."),",", proximoPeriodo.Month , ",", 
                proximoPeriodo.Year , ",0)");
        Conexao.executaQuery(con, sql, nomeTabela,null);            
        con.commit();
      }
      catch (Exception)
      {
         con.rollback();
         throw;
      }
    }

    public void add(Lancamento lancamento)
    {
        lancamentos.Add(lancamento);
        lancamentos.Sort();
    }

	public LivroCaixa(string ano, string mes, Conexao con)
	{
        string sql = String.Concat("SELECT idLivroCaixa," + campos + " from LivroCaixa where mes=", mes, " AND ano=", ano);

        try
        {            
            DataTable dt = Conexao.executaSelect(con, sql,null);
            if (dt.Rows.Count == 0) throw new Exception("Erro inesperado, Livro ano: " + ano + ", mes: " + mes +" não encontrado!");
            DataRow[] r = dt.Select();

            idLivroCaixa = Int16.Parse(r[0][0].ToString());
            valorTransportado = Double.Parse(r[0][1].ToString());
            this.mes = mes;
            this.ano = ano;
            stLivroFechado = "1".Equals(r[0][4].ToString());

            listaLancamentos(con);
           
        }
        catch (Exception)
        {
            throw;
        }
	}
    public LivroCaixa(string idLivroCaixa, Conexao con)
    {
        try
        {
            string sql = String.Concat("SELECT " + campos + " from LivroCaixa where idLivroCaixa=", idLivroCaixa);
            DataTable dt = Conexao.executaSelect(con, sql,null);
            if (dt.Rows.Count == 0) throw new Exception("Erro inesperado, id" + nomeTabela + " não encontrado!");
            DataRow[] r = dt.Select();
            valorTransportado = Double.Parse(r[0][0].ToString());
            mes = Int16.Parse(r[0][1].ToString()).ToString("D2");
            ano = r[0][2].ToString();
            stLivroFechado = "1".Equals(r[0][3].ToString());
            this.idLivroCaixa = Int16.Parse(idLivroCaixa);

            listaLancamentos(con);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static List<String> listaAnoMes(Conexao con)
    {
        try
        {
            List<String> lista = new List<String>();
            string sql = String.Concat("SELECT ano, mes from LivroCaixa order by ano, mes");
            DataTable dt = Conexao.executaSelect(con, sql,null);
            DataRow[] r = dt.Select();

            foreach (DataRow d in r)
            {
                lista.Add(d[0].ToString() + "/" + Int16.Parse(d[1].ToString()).ToString("D2"));
            }

            return lista;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string montaTabela(String titulo, Conexao con)
    {
        try
        {
            listaLancamentos(con);

            Tabela tabela = new Tabela(lancamentos.Count + 1, 7, titulo);

            String[] titulos = { " Seq. ", " Descrição ", " Responsável ", " Crédito ", " Débito ", " Data ", " Saldo " };

            int i = 0;

            saldo = valorTransportado;

            tabela.celula[i, 0] = "";
            tabela.celula[i, 1] = "<B>Valor Transportado do mês anterior</B>";
            tabela.celula[i, 2] = "";
            tabela.celula[i, 3] = String.Format("{0:###,###,##0.00}", valorTransportado);
            tabela.celula[i, 4] = "";
            tabela.celula[i, 5] = "";
            tabela.celula[i, 6] = String.Format("{0:###,###,##0.00}", valorTransportado);

            i++;

            foreach (Lancamento l in lancamentos)
            {
                if (l.tipoLancamento == 'C')
                {
                    saldo += l.valor;
                }
                else
                {
                    saldo -= l.valor;
                }
                tabela.celula[i, 0] = l.idLancamento.ToString("D4");
                tabela.celula[i, 1] = l.descricao;
                tabela.celula[i, 2] = l.usuarioResponsavel.nome;
                tabela.celula[i, 3] = l.tipoLancamento.ToString() == "C" ? String.Format("{0:###,###,##0.00}", l.valor) : "";
                tabela.celula[i, 4] = l.tipoLancamento.ToString() == "D" ? String.Format("{0:###,###,##0.00}", l.valor) : "";
                tabela.celula[i, 5] = l.data.ToString("dd/MM/yyyy hh:mm");
                tabela.celula[i, 6] = String.Format("{0:###,###,##0.00}", saldo);
                i++;
            }

            return tabela.tabela(titulos);
        }
        catch (Exception)
        {
            throw;
        }
    }

     
}
