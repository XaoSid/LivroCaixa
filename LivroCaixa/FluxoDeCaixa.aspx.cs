using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjLivroCaixa
{
    public partial class FluxoDeCaixa : System.Web.UI.Page
    {
        LivroCaixa livro = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Faça isso para que funcione o __doPostBack
            ClientScript.GetPostBackEventReference(this, String.Empty);

            Usuario usuario = (Usuario)Session["usuario"];
            if (usuario == null)
            {
                // Caso a sessão esteja expirada ou o usuário digitou a URL diretamente no navegar (sem se logar)
                Response.Redirect("index.aspx?sair=sair", false);
                return;
            }

         

            if (Session["anoMes"] == null)
            {
                Session["anoMes"] = DateTime.Now.Year + "/" + DateTime.Now.Month.ToString("D2");
            }

            lbUsuarioLogado.Text = "Bem vindo " + usuario.nome;

            try
            {
                using (Conexao con = new Conexao(usuario))
                {
                    con.open();
                    bool mudou = false;
                    string selAnoMes = (String)Session["anoMes"];
                    montaComboAnoMes(comboAnoMes, con);
                    if (comboAnoMes.SelectedValue != selAnoMes)
                    {
                        mudou = true;
                        Session["anoMes"] = comboAnoMes.SelectedValue;
                    }
                    if (livro == null || mudou)
                    {
                        String[] anoMes = comboAnoMes.SelectedValue.Split('/');
                        livro = new LivroCaixa(anoMes[0], anoMes[1], con);
                    }
                    situacao.Text = livro.stLivroFechado ? "<B>Fechado</B>" : "<B>Aberto</B>";  
                    String evento = (this.Request["__EVENTTARGET"] == null) ? String.Empty : this.Request["__EVENTTARGET"];                    
                    if (evento == "fecharPeriodo")
                    {
                        livro.fechaPeriodo(con);
                        btFecharPeriodo.Enabled = false;
                        comboAnoMes.Items.Clear();
                        montaComboAnoMes(comboAnoMes, con);
                    }
                    if (evento == "lancamento")
                    {
                        Lancamento l = new Lancamento(txDescricao.Text, livro.idLivroCaixa, Double.Parse(txValor.Text),
                                                                     rbCredito.Checked ? 'C' : 'D', usuario, DateTime.Now);
                        l.insere(con);
                        livro.listaLancamentos(con);
                        txValor.Text = txDescricao.Text = String.Empty;
                        rbDebito.Checked = rbCredito.Checked = false;
                    }
                    relatorioLancamentos.Text = livro.montaTabela((String) Session["empresa"], con);
                    btLancar.Enabled = btFecharPeriodo.Enabled = !livro.stLivroFechado;
                }
            }
            catch (Exception e1)
            {
                if (usuario.perfil == "ADM")
                {
                    lbMensagem.Text = e1.Message;
                }
                else
                {
                    lbMensagem.Text = "Erro Em Livro Caixa";
                }
            }
        }

        private bool valida()
        {
            if (txDescricao.Text.Trim() == String.Empty)
            {
                lbMensagem.Text = "Descrição é campo obrigatório!";
                return false; 
            }
            if (txValor.Text.Trim() == String.Empty)
            {
                lbMensagem.Text = "Valor é campo obrigatório!";
                return false; 
            }
            double valor;
            if (!Double.TryParse(txValor.Text, out valor))
            {
                lbMensagem.Text = "Valor digitado não é um número válido!";
                return false; 
            }
            if (valor <= 0)
            {
                lbMensagem.Text = "Valor digitado deve ser maior que zero e positivo!";
                return false;
            }
            if (rbCredito.Checked == false && rbDebito.Checked == false)
            {
                lbMensagem.Text = "Selecione se o lançamento é de crédito ou débito";
                return false;
            }
            if (rbDebito.Checked && valor > livro.saldo)
            {
                lbMensagem.Text = "Saldo em caixa insuficiente para retirada!";
                return false;
            }
            return true;
        }

        protected void montaComboAnoMes(DropDownList combo, Conexao con)
        {
            try
            {
                string anoMesSel = (String)Session["anoMes"];

                if (combo.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();

                    List<String> itens = LivroCaixa.listaAnoMes(con);

                    foreach (String s in itens)
                    {
                        listItem = new ListItem();
                        listItem.Text = s;
                        listItem.Value = s;

                        if (listItem.Value == anoMesSel)
                        {
                            listItem.Selected = true;
                        }

                        combo.Items.Add(listItem);
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["usuario"];

            if (usuario.perfil == "USU")
            {
                Session["usuario"] = null;
                Response.Redirect("index.aspx?sair=sair", false);
                return;
            }
            Response.Redirect("index.aspx", false);
        }

        protected void btFecharPeriodo_Click(object sender, EventArgs e)
        {
            StringBuilder myScript = new StringBuilder(String.Empty);
            myScript.Append("<script type='text/javascript' language='javascript'> ");
            myScript.Append("var result = window.confirm('Confirma fechar o periodo atual? Uma vez " + 
                "fechado não aceitará mais lançamentos!'); ");
            myScript.Append("__doPostBack('fecharPeriodo', result); ");
            myScript.Append("</script> ");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", myScript.ToString(), false);
        }

        protected void btLancar_Click(object sender, EventArgs e)
        {
            if (!valida()) return;

            StringBuilder myScript = new StringBuilder(String.Empty);
            myScript.Append("<script type='text/javascript' language='javascript'> ");
            myScript.Append("var result = window.confirm('Confirma realizar o lançamento? Valor: R$" + 
                txValor.Text + " - " + (rbCredito.Checked ? "Crédito":"Débito") + "'); ");
            myScript.Append("__doPostBack('lancamento', result); ");
            myScript.Append("</script> ");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", myScript.ToString(), false);
        }


    }
}

