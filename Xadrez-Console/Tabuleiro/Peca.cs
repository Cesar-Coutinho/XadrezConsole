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

        public abstract bool[,] MovimentosPossiveis();
        
    }
}
