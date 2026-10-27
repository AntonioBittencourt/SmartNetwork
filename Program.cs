using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SmartNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instanciando o gerenciador de segurança
            GerenciadorSeguranca seguranca = new GerenciadorSeguranca();

            // Criando alguns dispositivos de teste
            Dispositivo d1 = new Dispositivo("PC-RH", "AA:BB:CC:11:22:33", "192.168.0.10", "Dell", 15.5m);
            Dispositivo d2 = new Dispositivo("Notebook-Diretoria", "AA:BB:CC:44:55:66", "192.168.0.11", "Apple", 45.0m);

            // 1. Enfileirando os dispositivos
            seguranca.EnfileirarDispositivo(d1);
            seguranca.EnfileirarDispositivo(d2);

            Console.WriteLine($"\nDispositivos na fila: {seguranca.QuantidadeNaFila}");

            // 2. Verificando o próximo (sem remover)
            Dispositivo? proximo = seguranca.EspiarProximo();
            Console.WriteLine($"O próximo a ser verificado é: {proximo?.Nome}");

            // 3. Processando a fila (removendo um por um)
            Console.WriteLine("\nIniciando verificações...");
            while (seguranca.QuantidadeNaFila > 0)
            {
                Dispositivo emVerificacao = seguranca.DesenfileirarProximo();

                // Aqui você faria a lógica de segurança (ex: bloquear MAC desconhecido, etc.)
                Console.WriteLine($"-> Analisando {emVerificacao.Nome}... Aprovado!");
            }

            Console.WriteLine($"\nDispositivos na fila agora: {seguranca.QuantidadeNaFila}");
            Console.ReadLine();
        }
    }
}