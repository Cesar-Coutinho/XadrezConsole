﻿using Tabuleiro;
namespace Xadrez_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Posicao P = new Posicao(1,1);
            Console.WriteLine($"Posição {P}");
            Console.ReadKey();
        }
    }
}