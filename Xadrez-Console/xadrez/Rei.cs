using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei(Cor cor,Tabuleiro tab) : base(cor, tab){}


        public override string ToString()
        {
            return "R";
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
            if(tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha,pos.Coluna] = true;
            }

            //NE
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //NO
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna -1);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //Esquerda
            pos.DefinirValores(posicao.Linha , posicao.Coluna -1);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //Direita
            pos.DefinirValores(posicao.Linha , posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //Abaixo
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }

            //SE
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            //SO
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && this.PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            #endregion

            return mat;
        }

        
    }
}
