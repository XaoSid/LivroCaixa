using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjLivroCaixa
{
    public partial class CadastroUsuarios : System.Web.UI.Page
    {       

        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuario = (Usuario)Session["usuario"];
            if (usuario == null)
            {
                // Caso a sessão esteja expirada ou o usuário digitou a URL diretamente no navegar (sem se logar)
                Response.Redirect("index.aspx?sair=sair", false);
                return;
            }

            lbUsuarioLogado.Text = "Bem vindo: " + usuario.nome;

            using (Conexao con = new Conexao(usuario))
            {
                con.open();
                relatorioUsuarios.Text = usuario.montaTabela((String) Session["empresa"], con);
            }
        }
        protected void btOkBusca_Click(object sender, EventArgs e) {
            Usuario usuario = (Usuario)Session["usuario"];
            Usuario sel = null;
            lbMensagem.Text = String.Empty;
            btExcluir.Visible = btAtualizar.Visible = false;
            btNovo.Visible = true;
            limpa();
            if (txNomeBusca.Text.Trim() == String.Empty)  {
                lbMensagem.Text = "Digite um texto para busca (nome, login ou sequencial)";  return;
            }
            using (Conexao con = new Conexao(usuario)) {             
                con.open();
                Int16 id; 
                if (Int16.TryParse(txNomeBusca.Text, out id)) { // Será que o usuário digitou um número?                
                    sel = Usuario.busca(id, con);
                    if (sel == null)  {
                        lbMensagem.Text = "Nenhum usuário foi encontrado com o sequencial " + id + "!";  return;
                    }
                }
                else { // Não é numero, então é substring               
                    List<Usuario> lista = Usuario.listaUsuarios(txNomeBusca.Text, con); // Traz uma lista de usuários
                    if (lista.Count == 0) { // Lista vazia? não encontrei ninguém!                    
                        lbMensagem.Text = "Nenhum usuário foi encontrado com o nome/login informado!";  return;
                    }
                    if (lista.Count > 1) { // Mais que um na lista? não vale.. preciso achar só um                   
                        lbMensagem.Text = "Mais de um usuário foi encontrado com o nome/login informado!";
                        return;
                    }
                    sel = lista[0];
                }
                Session["usuarioProcurado"] = sel; // Achei um usuário!
                txNome.Text = sel.nome; // Colocamos os campos do usuário encontrado na tela
                txLogin.Text = sel.login;
                txCPF.Text = sel.cpf;
                if (sel.perfil == "ADM") { // se for ADM, arrumamos os radiobuttons              
                    rbAdm.Checked = true;      rbUsu.Checked = false;
                }  else { // se não é USU, arrumamos os radiobuttons               
                    rbAdm.Checked = false;     rbUsu.Checked = true;
                }             
                btExcluir.Visible = btAtualizar.Visible = true;
                btNovo.Visible = false;
                lbMensagem.Text = "Usuário encontrado";
            }
        }

        private bool validarCampos()
        {
            if (rbAdm.Checked == false && rbUsu.Checked == false)
            {
                lbMensagem.Text = "Selecione o perfil do usuário";
                return false;
            }
            if (txNome.Text.Trim()  == String.Empty)
            {
                lbMensagem.Text = "Digite o nome do Usuário";
                return false;
            }
            if (txLogin.Text.Trim() == String.Empty)
            {
                lbMensagem.Text = "Digite o login do usuário";
                return false;
            }
            if (txCPF.Text.Trim() == String.Empty)
            {
                lbMensagem.Text = "Digite o CPF do usuário";
                return false;
            } 
            return true;
        }

        protected void btAtualizar_Click(object sender, EventArgs e)  {           
            Usuario usu = (Usuario)Session["usuarioProcurado"];
            if (usu == null)  {
                lbMensagem.Text = "Erro inesperado. Usuário para editar ainda não selecionado!";
                return;
            }
            if (!validarCampos()) return;
            txCPF.Text = txCPF.Text.Replace(".", "").Replace("/", "");
            Usuario usuario = (Usuario)Session["usuario"];
            try {
                using (Conexao con = new Conexao(usuario)) {
                    con.open();
                    usu.nome = txNome.Text.Trim();
                    usu.login = txLogin.Text.Trim();
                    usu.cpf = txCPF.Text.Trim();
                    usu.perfil = rbAdm.Checked ? "ADM" : "USU";
                    usu.atualiza(con);
                    limpa();
                    relatorioUsuarios.Text = usuario.montaTabela((String)Session["empresa"], con);
                    lbMensagem.Text = "Usuário atualizado com sucesso!";
                }
            }
            catch (Exception e1) {
                if (usuario.perfil == "ADM") {
                    lbMensagem.Text = e1.Message;
                }
                else  {
                    lbMensagem.Text = "Erro excluindo usuario";
                }
            }
        }

        private void limpa()
        {
            Session["usuarioProcurado"] = null;
            txNome.Text =
            txLogin.Text =
            txCPF.Text = String.Empty;

            rbAdm.Checked = rbUsu.Checked = false;

            btExcluir.Visible = btAtualizar.Visible = false;
            btNovo.Visible = true;
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;
            Usuario usuario = (Usuario)Session["usuario"];
            try
            {
                using (Conexao con = new Conexao(usuario))
                {
                    con.open();
                    Usuario u = Usuario.busca(con, txLogin.Text);
                    if (u != null && u.idUsuario > 0)
                    {
                        lbMensagem.Text = "Já existe um usuário com este login: " + txLogin.Text;
                        return;
                    }
                    txCPF.Text = txCPF.Text.Replace(".", "").Replace("/", "");
                    u = new Usuario(txNome.Text, txLogin.Text, txCPF.Text, rbAdm.Checked ? "ADM" : "USU", txCPF.Text);
                    u.insere(con);
                    limpa();
                    lbMensagem.Text = "Novo usuário cadastrado com sucesso!";
                    relatorioUsuarios.Text = usuario.montaTabela((String)Session["empresa"], con);
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
                    lbMensagem.Text = "Erro cadastrando usuario.";
                }
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
             Usuario usuario = (Usuario)Session["usuario"];
            try
            { 
                Usuario usu = (Usuario)Session["usuarioProcurado"];
                if (usu == null)
                {
                    lbMensagem.Text = "Erro inesperado. Usuário para excluir ainda não selecionado!";
                    return;
                }
                using (Conexao con = new Conexao(usuario))
                {
                    con.open();
                    usu.exclui(con);
                    relatorioUsuarios.Text = usuario.montaTabela((String)Session["empresa"], con);
                }
                limpa();
                lbMensagem.Text = "Usuário excluido com sucesso!";               
            }
            catch (Exception e1)
            {
                if (usuario.perfil == "ADM")
                {
                    lbMensagem.Text = e1.Message;
                }
                else
                {
                    lbMensagem.Text = "Erro excluindo usuário";
                }
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
    }
}