using tabuleiro;

namespace xadrez
{
    class Torre : Peca
    {
        public Torre(Cor cor, Tabuleiro tab) : base(cor, tab) { }


        public override string ToString()
        {
            return "T";
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);

            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[tab.Linhas, tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            #region Posições possiveis
            //Acima
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna);
            while(tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if(tab.peca(pos) != null && tab.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.Linha = pos.Linha-1;
            }
            //Abaixo
            pos.DefinirValores(posicao.Linha +1, posicao.Coluna);
            while (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.Linha = pos.Linha+1;
            }
            //Direita
            pos.DefinirValores(posicao.Linha , posicao.Coluna+1);
            while (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.Coluna = pos.Coluna+1;

            }
            //Esquerda
            pos.DefinirValores(posicao.Linha , posicao.Coluna-1);
            while (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != this.cor)
                {
                    break;
                }
                pos.Coluna = pos.Coluna -1;
            }
            #endregion

            return mat;
        }
    }
}
