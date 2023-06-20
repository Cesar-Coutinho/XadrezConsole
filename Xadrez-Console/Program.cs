using System.Net.Http.Headers;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaDeXadrez Partida = new PartidaDeXadrez();
                while (!Partida.Terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(Partida.tab);

                    Console.WriteLine();
                    Console.Write("Origem:");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                    
                    bool[,] posicoesPossiveis = Partida.tab.peca(origem).MovimentosPossiveis();
                    Console.Clear();
                    Tela.ImprimirTabuleiro(Partida.tab, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destino:");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                    Partida.ExecutaMovimento(origem, destino);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}