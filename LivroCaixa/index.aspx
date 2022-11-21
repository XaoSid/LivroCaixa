<%@ Page title="Login" Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="prjLivroCaixa.index" %>  
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>   

<body>
    <form id="login" runat="server">
        <div>
            <p></p>
            <p></p>
            <h2>Programação Back End</h2>
            <h3>LIVRO CAIXA - <a href='index.aspx?novaEmpresa=sim' title="clique aqui para trocar o nome da Instituição"><%=empresa%></a></h3>
            <p></p>
            <p></p>
            <asp:Panel runat="server" ID="pnLogin" Visible="true">
                <table style="width: 400px">
                    <tr>
                        <td>&nbsp;&nbsp;<asp:Label ID="lbUsuario" runat="server" Text="Usuário" />
                        </td>
                        <td>&nbsp;&nbsp;<asp:TextBox ID="txUsuario" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;&nbsp;<asp:Label ID="lbSenha" runat="server" Text="Senha"></asp:Label>
                        </td>
                        <td>&nbsp;&nbsp;<asp:TextBox ID="txSenha" runat="server" TextMode="Password" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;&nbsp;&nbsp;<asp:Button ID="btOk" runat="server" Text="Ok" OnClick="btOk_Click" Width="120px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>          
            <p></p>
            <p></p>
            <asp:Panel runat="server" ID="pnTrocaSenha" Visible="false">
                <h3>Redefinição de Senha</h3>
                <table style="width: 400px">
                    <tr>
                        <td>&nbsp;&nbsp;<asp:Label ID="lbSenha1" runat="server" Text="Nova senha"></asp:Label>
                        </td>
                        <td>&nbsp;&nbsp;<asp:TextBox ID="txSenha1" runat="server" TextMode="Password" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;&nbsp;<asp:Label ID="lbSenha2" runat="server" Text="Confirmação" Width="120px"></asp:Label>
                        </td>
                        <td>&nbsp;&nbsp;<asp:TextBox ID="txSenha2" runat="server" TextMode="Password" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;&nbsp;&nbsp;<asp:Button ID="btTrocaSenha" runat="server" Text="Ok" OnClick="btTrocaSenha_Click" Width="120px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnMenu" Visible="false">
                <table style="width: 300px">
                    <tr>
                        <td style="text-align: center">
                            <h2>Menu</h2>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <a href='cadastroUsuarios.aspx' title="clique aqui para cadastro de usuários.">Cadastro de Usuários</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <a href='fluxoDeCaixa.aspx' title="clique aqui lançamentos do Fluxo de Caixa.">Fluxo de Caixa</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <a href='index.aspx?sair=sair' title="Sair">Sair</a>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p></p>
            <p></p>
            <p></p>
            &nbsp;&nbsp;&nbsp;<asp:Label ID="lbMensagem" runat="server" Style="font-size: large; color: brown; font-weight: bold;"></asp:Label>
        </div>
    </form>
</body>
</html>
