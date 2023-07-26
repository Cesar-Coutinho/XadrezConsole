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
        public Peca VulneravelEnPassant { get; private set; }

        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8,8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            xeque = false;
            Terminada = false;
            VulneravelEnPassant = null;
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

            //jogada especial roque pequeno
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);

                Peca T = tab.RemoverPeca(origemTorre);
                T.IncrementarQuantidadeMovimentos();

                tab.ColocarPeca(T, destinoTorre);
            }
            //jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);

                Peca T = tab.RemoverPeca(origemTorre);
                T.IncrementarQuantidadeMovimentos();

                tab.ColocarPeca(T, destinoTorre);
            }

            //jogada especial enPassant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posPeao;
                    if(p.cor == Cor.Branca)
                    {
                        posPeao = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posPeao = new Posicao(destino.Linha - 1, destino.Coluna);
                    }

                    pecaCapturada = tab.RemoverPeca(posPeao);
                    capturadas.Add(pecaCapturada);
                }
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

            //jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);

                Peca T = tab.RemoverPeca(destinoTorre);
                T.DecrementarQuantidadeMovimentos();

                tab.ColocarPeca(T, origemTorre);
            }

            //jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);

                Peca T = tab.RemoverPeca(destinoTorre);
                T.DecrementarQuantidadeMovimentos();

                tab.ColocarPeca(T, origemTorre);
            }

            //Jogada especial en Passant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = tab.RemoverPeca(destino);
                    Posicao posPeao;
                    if(p.cor == Cor.Branca)
                    {
                        posPeao = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posPeao = new Posicao(4, destino.Coluna);
                    }
                    tab.ColocarPeca(peao, posPeao);
                }
            }
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
            Peca p = tab.peca(destino);

            if(p is Peao)
            {
                if(p.cor == Cor.Branca && destino.Linha == 0 || p.cor == Cor.Preta && destino.Linha == 7)
                {
                    p = tab.RemoverPeca(destino);
                    pecas.Remove(p);

                    Peca dama = new Dama(p.cor, tab);
                    tab.ColocarPeca(dama, destino);
                    pecas.Add(dama);
                }
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

            // não poderia testar a quantidade de movimentos do peão? && (p.qteMovimentos == 1)
            //Jogada especial enPassant
            if (p is Peao && (destino.Linha == origem.Linha + 2 || destino.Linha == origem.Linha - 2) )
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
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
            #region Brancas
            ColocarNovaPeca('a', 1, new Torre(Cor.Branca, tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('d', 1, new Dama(Cor.Branca, tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branca, tab, this));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, tab));

            ColocarNovaPeca('a', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('b', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('c', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('d', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('e', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('f', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('g', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('h', 2, new Peao(Cor.Branca, tab, this));
            #endregion

            #region Pretas
            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('d', 8, new Dama(Cor.Preta, tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preta, tab, this));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preta,tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, tab));

            ColocarNovaPeca('a', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('b', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('c', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('d', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('e', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('f', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('g', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('h', 7, new Peao(Cor.Preta, tab, this));
            #endregion
        }
    }
}
