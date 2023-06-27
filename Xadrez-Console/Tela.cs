using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for(int i = 0; i < tab.Linhas; i++)
            {
                Console.Write($"{tab.Linhas - i} ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    ImprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }
        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor corOriginal = Console.BackgroundColor;
            ConsoleColor corAlterada = ConsoleColor.DarkGray;
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write($"{tab.Linhas - i} ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = corAlterada;
                    }
                    else
                    {
                        Console.BackgroundColor = corOriginal;
                    }
                    ImprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
                Console.BackgroundColor = corOriginal;
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = corOriginal;
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.cor.Equals(Cor.Branca))
                {
                    Console.Write($"{peca} ");
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{peca} ");
                    Console.ForegroundColor = aux;

                }
                //Console.WriteLine();
            }
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {

            string s = Console.ReadLine();

            char coluna = s[0];
            int linha = int.Parse(s[1]+"");
            return new PosicaoXadrez(coluna, linha);
        }
    }
}
