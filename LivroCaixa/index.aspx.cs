using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjLivroCaixa
{
    public partial class index : System.Web.UI.Page
    {
        public string empresa;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Faça isso para que funcione o __doPostBack
            ClientScript.GetPostBackEventReference(this, String.Empty);

           
            // -----------------------------------------------------------

            String sair = Request.QueryString["sair"];

            if (sair != null && sair == "sair")
            {
                Session["usuario"] = null;
                pnTrocaSenha.Visible = false;
                pnLogin.Visible = true;
                txUsuario.Text = txSenha.Text = String.Empty;
                Response.Redirect("index.aspx", false);
            }

            // Atende solicitação do usuário de trocar o nome da empresa
            String novaEmpresa = Request.QueryString["novaEmpresa"];
            if (novaEmpresa != null && novaEmpresa == "sim")
            {
                HttpCookie cookie = new HttpCookie("empresa");
                empresa = cookie.Value = String.Empty;     
               
                cookie.Expires = DateTime.Now;
                Response.Charset = "UTF-8";
                Response.Cookies.Add(cookie);               
                txUsuario.Text = txSenha.Text = String.Empty;
                pnLogin.Visible = true;
                Session["usuario"] = null;
                Response.Redirect("index.aspx", false);
                return;
            }

            String evento = (this.Request["__EVENTTARGET"] == null) ? String.Empty : this.Request["__EVENTTARGET"];
            String resposta = (this.Request["__EVENTARGUMENT"] == null) ? String.Empty : this.Request["__EVENTARGUMENT"];

            if (evento == "digiteNome")
            {
                if (resposta == String.Empty)
                {
                    lbMensagem.Text = "Por favor, digite o nome da Instituição!";
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("empresa");
                    Session["empresa"] = empresa = cookie.Value = resposta;
                    DateTime dtNow = DateTime.Now;
                    TimeSpan tsMinute = new TimeSpan(1000, 1, 0, 0);
                    cookie.Expires = dtNow + tsMinute;
                    Response.Charset = "UTF-8";
                    Response.Cookies.Add(cookie);                
                }
            }

            Usuario usuario = (Usuario)Session["usuario"];

            if (usuario != null )
            {
                pnLogin.Visible = false;
                if (usuario.perfil == "ADM")
                {
                    pnMenu.Visible = true;
                }
                else
                {
                    Response.Redirect("fluxoDeCaixa.aspx", false);
                    return;
                }
            }            
        }

        protected void btOk_Click(object sender, EventArgs e)    {
            pnTrocaSenha.Visible = false;
            lbMensagem.Text = String.Empty;
            using (Conexao con = new Conexao(null)) {
                con.open();
                Usuario usuario = Usuario.busca(txUsuario.Text.Trim(), txSenha.Text.Trim(), con);
                if (usuario == null)  {
                    lbMensagem.Text = "Usuário / senha não cadastrado!";
                    return;
                }
                Session["usuario"] = usuario;
                if (usuario.senha == usuario.cpf)  { // Primeiro acesso                
                    lbMensagem.Text = "Este é o seu primeiro acesso, favor definir" + 
                        " uma nova senha com no mínimo 6 e no máximo 10 caracteres";
                    pnTrocaSenha.Visible = true;
                    return;
                } 
                if (usuario != null && usuario.senha != usuario.cpf) {
                    HttpCookie cookie = Request.Cookies["empresa"];
                    if (cookie == null || cookie.Value.ToString() == String.Empty) 
                    {
                        boxOkCancelaTextoJavaScript("Digite o nome da instituição", "digiteNome");
                    }
                    else {
                        Session["empresa"] = empresa = cookie.Value;
                        pnLogin.Visible = false;
                        if (usuario.perfil == "ADM")  {
                            pnMenu.Visible = true;
                        }
                        else {
                            Response.Redirect("fluxoDeCaixa.aspx", false);
                            return;
                        }
                    }
                }
            }
        }

        protected void btTrocaSenha_Click(object sender, EventArgs e)
        {
            lbMensagem.Text = String.Empty;
            try
            {
                if (txSenha1.Text.Trim().CompareTo(txSenha2.Text.Trim()) != 0)
                {
                    lbMensagem.Text = "Senha de confirmação não confere!";
                    return;
                }
                if (txSenha1.Text.Trim().Length < 6 || txSenha1.Text.Trim().Length > 10)
                {
                    lbMensagem.Text = "A nova senha digitada deve ter no mínimo 6 e no máximo 10 dígitos!";
                    return;
                }
                Usuario usuario = (Usuario)Session["usuario"];
                usuario.senha = txSenha1.Text.Trim();
                using (Conexao con = new Conexao(usuario))
                {
                    con.open();
                    usuario.atualiza(con);
                    pnTrocaSenha.Visible = false;
                    lbMensagem.Text = "Senha reiniciada com sucesso!";
                }
            }
            catch (Exception)
            {
                lbMensagem.Text = "Erro inesperado redefinindo senha!";
            }
        }

        /// <summary>
        /// Monta uma caixa javascript onde o usuário pode digitar um texto;
        /// </summary>
        /// <param name="mens"></param>
        /// <param name="funcao"></param>
        public void boxOkCancelaTextoJavaScript(string mens, string funcao)
        {
          StringBuilder script = new StringBuilder(String.Empty);
          script.Append("<script type='text/javascript' language='javascript'> ");
          script.Append("var retorno = prompt('" + mens.Replace("<br />", "\\n") + "', ''); ");
          script.Append("__doPostBack('" + funcao + "', retorno); ");
          script.Append("</script> ");
          ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", script.ToString(),
                false);
        }
    }
}