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
                PartidaDeXadrez Partida = new PartidaDeXadrez();
                while (!Partida.Terminada)
                {
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirPartida(Partida);
                        Console.WriteLine();
                        Console.Write("Origem:");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        Partida.ValidarPosicaoOrigem(origem);

                        bool[,] posicoesPossiveis = Partida.tab.peca(origem).MovimentosPossiveis();
                        Console.Clear();
                        Tela.ImprimirTabuleiro(Partida.tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino:");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        Partida.ValidarPosicaoDestino(origem, destino);

                        Partida.RealizaJogada(origem, destino);
                    }
                    catch(TabuleiroException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                    }
                }
                Console.Clear();
                Tela.ImprimirPartida(Partida);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}