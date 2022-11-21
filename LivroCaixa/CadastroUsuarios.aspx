<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroUsuarios.aspx.cs" Inherits="prjLivroCaixa.CadastroUsuarios" %>

<!DOCTYPE html>

<style>
    hr {
        margin-bottom: 0px;
        margin-top: 0px;
        margin-left: 0px;
        margin-right: 0px;
        border: 0px solid #ccc;
        color: #ccc;
        background-color: #ccc;
        height: 2px;
        vertical-align: middle;
        padding-top: 0px;
        padding-bottom: 0px;
    }

    .semBorda {
        border-style: none;
        border-color: inherit;
        border-width: 0px;
        border-spacing: 0px;
        border-bottom: 0px;
        border-left: 0px;
        border-top: 0px;
        padding-bottom: 0px;
        padding-top: 0px;
        padding: 0px;
        padding-left: 0px;
        padding-right: 0px;
    }
</style>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de Usuários</title>
</head>
<body>
    <form id="formCadastroDeUsuarios" runat="server">
        <div>
           
            <table style="width: 1000px;">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lbUsuarioLogado" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr style="width: 1000px;" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <h3>Cadastro de Usuários</h3>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr  style="width: 1000px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbBuscarUsuario" runat="server" Text="Buscar usuário por Nome/Login ou Sequencial" ToolTip="Digite o sequencial, nome ou login em parte ou completo."></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txNomeBusca" runat="server" Width="100px" ToolTip="Digite o sequencial, nome ou login em parte ou completo."></asp:TextBox>&nbsp;&nbsp;
                        <asp:Button ID="btOkBusca" runat="server" Text=" Buscar " OnClick="btOkBusca_Click" ToolTip="Digite o sequencial, nome ou login em parte ou completo." />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <hr  style="width: 1000px;" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lbNome" runat="server" Text="Nome*"></asp:Label>&nbsp;<asp:TextBox ID="txNome" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbLogin" runat="server" Text="Login*"></asp:Label>&nbsp;<asp:TextBox ID="txLogin" runat="server" Width="120px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbCpf" runat="server" Text="CPF*"></asp:Label>&nbsp;<asp:TextBox ID="txCPF" runat="server" Width="120px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbPerfil" runat="server" Text="Perfil*: ADM "></asp:Label>&nbsp;<asp:RadioButton ID="rbAdm" runat="server" GroupName="perfil" />&nbsp;&nbsp;
                        <asp:Label ID="lbUSU" runat="server" Text="USU" ></asp:Label><asp:RadioButton ID="rbUsu" runat="server" GroupName="perfil" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btAtualizar" runat="server" Text=" Atualizar " visible="false" OnClick="btAtualizar_Click" />
                        <asp:Button ID="btNovo" runat="server" Text=" Incluir " OnClick="btNovo_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btExcluir" runat="server" Text=" Excluir "  visible="false" OnClick="btExcluir_Click" />
                      </td>
                </tr>
                <tr>
                    <td>
                        <hr  style="width: 1000px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="relatorioUsuarios" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr  />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                         <asp:Button ID="btSair" runat="server" Text=" Sair " OnClick="btSair_Click"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr  />
                    </td>
                </tr>
            </table>
             <p></p>
            <p></p>
            <p></p>
            &nbsp;&nbsp;&nbsp;<asp:Label ID="lbMensagem" runat="server" Style="font-size: large; color: brown; font-weight: bold;"></asp:Label>
        </div>
    </form>
</body>
</html>
