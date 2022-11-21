using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjLivroCaixa.Classes
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;


    public class Tabela
    {
        private int linhas;
        private int colunas;

        private string titulo;

        public string[,] celula;

        public Tabela(int linhas, int colunas, String titulo)
        {
            celula = new string[linhas, colunas];
            this.linhas = linhas;
            this.colunas = colunas;
            this.titulo = titulo;
        }

        public string tabela(string[] titulos)
        {
            StringBuilder str = new StringBuilder("<meta charset='UTF-8'/>");
            str.Append("<table>");

            if (titulo != null && titulo != String.Empty)
            {  
                str.Append("<TR><TD colspan='" + colunas + "'><hr /></TD></TR>");
                str.Append("<TR><TD style='text-align:center;font-size:larger;font-weight:bold;' colspan='" + colunas + "'>" + titulo + "</TD></TR>");
            }

            str.Append("<TR><TD colspan='" + colunas + "'><hr /></TD></TR>");
            str.Append("<tr>");
            for (int col = 0; col < colunas; col++)
            {
                str.Append("<TD>&nbsp;<B>" + titulos[col] + "</B>&nbsp;</TD>");
            }
            str.Append("</tr>");
            str.Append("<TR><TD colspan='" + colunas + "'><hr /></TD></TR>");
            for (int lin = 0; lin < linhas; lin++)
            {
                str.Append("<tr>");
                for (int col = 0; col < colunas; col++)
                {
                    double valor;
                    if (Double.TryParse(celula[lin, col].Replace(",","."), out valor))
                    {
                        str.Append("<TD style='text-align:right'>&nbsp;" + celula[lin, col] + "&nbsp;</TD>");
                    }
                    else
                    {
                        str.Append("<TD>&nbsp;" + celula[lin, col] + "&nbsp;</TD>");
                    }
                }
                str.Append("</tr>");
            }
            str.Append("<TR><TD colspan='" + colunas + "'><hr /></TD></TR>");
            str.Append("</table>");
            return str.ToString();
        }
    }

}