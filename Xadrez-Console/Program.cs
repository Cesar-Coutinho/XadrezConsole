using tabuleiro;
namespace Xadrez_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Posicao P = new Posicao(1,1);
            //Console.WriteLine($"Posição {P}");

            

            Tela.ImprimirTabuleiro(new Tabuleiro(8,8));
            Console.ReadKey();
        }
    }
}