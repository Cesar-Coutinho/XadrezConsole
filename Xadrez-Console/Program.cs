﻿using System.Net.Http.Headers;
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
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.ColocarPeca(new Torre(Cor.Preto, tab), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(Cor.Preto, tab), new Posicao(1, 3));
                tab.ColocarPeca(new Rei(Cor.Preto, tab), new Posicao(2, 4));


                Tela.ImprimirTabuleiro(tab);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}