using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8,8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            xeque = false;
            Terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RemoverPeca(origem);
            if(p != null)
                p.IncrementarQuantidadeMovimentos();
            Peca pecaCapturada = tab.RemoverPeca(destino);

            tab.ColocarPeca(p, destino);

            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RemoverPeca(destino);
            p.DecrementarQuantidadeMovimentos();
            if(pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);
        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça nesta Posição escolhida");
            }
            if (JogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("A peça escolhida não é sua.");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existe movimentos possiveis para essa peça.");
            }
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida.");
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException($"Você não pode se colocar em xeque.");
            }

            if (EstaEmXeque(Adversario(JogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(Adversario(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();

                for(int i = 0; i < tab.Linhas; i++)
                {
                    for(int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);

                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preta;
            else
                JogadorAtual = Cor.Branca;
        }

        public HashSet<Peca> PecasCapturas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach(Peca x in capturadas)
            {
                if(x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturas(cor));
            return aux;
        }

        private Cor Adversario(Cor cor)
        {
            return cor == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach(Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                    return x;
            }

            return null;
        }


        public bool EstaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não há rei no tabuleiro.");
            }
            foreach(Peca x in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.posicao.Linha, R.posicao.Coluna])
                {
                    return true;
                }
            }

            return false;
        }
        public void ColocarNovaPeca(char coluna, int linha, Peca peca) 
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna,linha).ToPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas() 
        {
            ColocarNovaPeca('c', 1, new Torre(Cor.Branca, tab));
            ColocarNovaPeca( 'c', 2, new Torre(Cor.Branca, tab));
            ColocarNovaPeca( 'd', 2, new Torre(Cor.Branca, tab));
            ColocarNovaPeca( 'e', 2, new Torre(Cor.Branca, tab));
            ColocarNovaPeca( 'e', 1, new Torre(Cor.Branca, tab));
            ColocarNovaPeca( 'd', 1, new Rei(Cor.Branca, tab));
            ColocarNovaPeca( 'c', 7, new Torre(Cor.Preta, tab));
            ColocarNovaPeca( 'c', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca( 'd', 7, new Torre(Cor.Preta, tab));
            ColocarNovaPeca( 'e', 7, new Torre(Cor.Preta, tab));
            ColocarNovaPeca( 'e', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca( 'd', 8, new Rei(Cor.Preta, tab));
        }
    }
}
