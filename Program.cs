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
            //        Dispositivo d1 = new Dispositivo("PC-RH", "AA:BB:CC:11:22:33", "192.168.0.10", "Dell", 15.5m);
            //      Dispositivo d2 = new Dispositivo("Notebook-Diretoria", "AA:BB:CC:44:55:66", "192.168.0.11", "Apple", 45.0m);

            // 1. Enfileirando os dispositivos
            //            seguranca.EnfileirarDispositivo(d1);
            //          seguranca.EnfileirarDispositivo(d2);

            Console.WriteLine($"\nDispositivos na fila: {seguranca.QuantidadeNaFila}");


            int opcao = -1;

            while (opcao != 0)
            {
                Console.Clear(); // Limpa a tela para o menu ficar bonito
                Console.WriteLine("=====================================");
                Console.WriteLine("      SMARTNET MANAGER - SEGURANÇA   ");
                Console.WriteLine("=====================================");
                Console.WriteLine($" Dispositivos aguardando na fila: {seguranca.QuantidadeNaFila}");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(" 1. Adicionar dispositivo à fila");
                Console.WriteLine(" 2. Processar fila (Autenticar)");
                Console.WriteLine(" 3. Listar Dispositivos Ativos");
                Console.WriteLine(" 4. Exibir Dashboard (LINQ)");
                Console.WriteLine(" 0. Sair");
                Console.WriteLine("=====================================");
                Console.Write(" Escolha uma opção: ");

                // O TryParse evita que o programa quebre se o usuário digitar uma letra
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    opcao = -1; // Força continuar no loop
                }

                switch (opcao)
                {
                    case 1:
                        Console.WriteLine("\n--- CADASTRAR NOVO DISPOSITIVO ---");
                        try
                        {
                            Console.Write("Nome: ");
                            string nome = Console.ReadLine() ?? string.Empty;

                            Console.Write("MAC (ex: AA:BB:CC:DD:EE:FF): ");
                            string mac = Console.ReadLine() ?? string.Empty;

                            Console.Write("IP (ex: 192.168.0.10): ");
                            string ip = Console.ReadLine() ?? string.Empty;

                            Console.Write("Fabricante: ");
                            string fabricante = Console.ReadLine() ?? string.Empty;

                            Console.Write("Consumo de Banda (Mbps): ");
                            decimal consumo = decimal.Parse(Console.ReadLine() ?? string.Empty);

                            // A própria classe Dispositivo já valida o MAC e o IP
                            Dispositivo novo = new Dispositivo(nome, mac, ip, fabricante, consumo);
                            seguranca.EnfileirarDispositivo(novo);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"\n[ERRO DE VALIDAÇÃO]: {ex.Message}");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("\n[ERRO]: O consumo de banda deve ser um número válido (use vírgula ou ponto).");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n[ERRO INESPERADO]: {ex.Message}");
                        }
                        break;

                    case 2:
                        Console.WriteLine("\n--- PROCESSANDO FILA DE AUTENTICAÇÃO ---");
                        seguranca.ProcessarProximoDaFila();
                        break;

                    case 3:
                        Console.WriteLine("\n--- LISTA DE DISPOSITIVOS ATIVOS ---");
                        var ativos = seguranca.ObterAtivos();
                        if (ativos.Count == 0)
                        {
                            Console.WriteLine("Nenhum dispositivo ativo na rede.");
                        }
                        else
                        {
                            foreach (var par in ativos)
                            {
                                Console.WriteLine(par.Value.ToString());
                            }
                        }
                        break;



                    case 4:
                        Console.WriteLine("\n--- PAINEL ESTATÍSTICO(DASHBOARD) ---");
                        seguranca.ExibirDashboard();
                        break;

                    case 0:
                        Console.WriteLine("\nSaindo do sistema...");
                        break;

                    default:
                        Console.WriteLine("\n[ERRO] Opção inválida! Tente novamente.");
                        break;
                }

                if (opcao != 0)
                {
                    Console.WriteLine("\nPressione Enter para voltar ao menu...");
                    Console.ReadLine();
                }
            }




            /*
            // 2. Verificando o próximo (sem remover)
            Dispositivo? proximo = seguranca.EspiarProximo();
            Console.WriteLine($"O próximo a ser verificado é: {proximo?.Nome}");

            // 3. Processando a fila (removendo um por um)
            Console.WriteLine("\nIniciando verificações...");
            while (seguranca.QuantidadeNaFila > 0)
            {
                //no retorno null, cria um dispositivo "vazio" para evitar exceção
                Dispositivo emVerificacao = seguranca.DesenfileirarProximo()  ?? new Dispositivo("Vazio", "00:00:00:00:00:00", "0.0.0.0", "Nenhum", 0m);

                // Aqui você faria a lógica de segurança (ex: bloquear MAC desconhecido, etc.)
                Console.WriteLine($"-> Analisando {emVerificacao.Nome}... Aprovado!");
            }

            Console.WriteLine($"\nDispositivos na fila agora: {seguranca.QuantidadeNaFila}");
            Console.ReadLine(); */
        }
    }
}