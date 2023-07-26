using System.Globalization;
using tabuleiro;

namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao posicao { get; set; }
        public Cor cor { get; set; }
        public int qteMovimentos { get; private set; }
        public Tabuleiro tab { get; set; }

        public Peca(Cor cor, Tabuleiro tabuleiro)
        {
            this.posicao = null;
            this.cor = cor;
            this.tab = tabuleiro;
            qteMovimentos = 0;
        }

        public void IncrementarQuantidadeMovimentos() 
        {
            qteMovimentos++;
        }
        public void DecrementarQuantidadeMovimentos()
        {
            qteMovimentos--;
        }

        public bool ExisteMovimentosPossiveis() 
        {
            bool[,] mat = MovimentosPossiveis();

            for(int i = 0; i < tab.Linhas; i++)
            {
                for(int j = 0; j < tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha,pos.Coluna];
        }
        public abstract bool[,] MovimentosPossiveis();
        
    }
}
