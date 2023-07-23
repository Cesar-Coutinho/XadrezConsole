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

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8,8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RemoverPeca(origem);
            p.IncrementarQuantidadeMovimentos();
            Peca pecaCapturada = tab.RemoverPeca(destino);

            tab.ColocarPeca(p, destino);

            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
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
            if (!tab.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida.");
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
            
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preto;
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

            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturas(cor));
            return aux;
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
            ColocarNovaPeca( 'c', 7, new Torre(Cor.Preto, tab));
            ColocarNovaPeca( 'c', 8, new Torre(Cor.Preto, tab));
            ColocarNovaPeca( 'd', 7, new Torre(Cor.Preto, tab));
            ColocarNovaPeca( 'e', 7, new Torre(Cor.Preto, tab));
            ColocarNovaPeca( 'e', 8, new Torre(Cor.Preto, tab));
            ColocarNovaPeca( 'd', 8, new Rei(Cor.Preto, tab));
        }
    }
}
