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



            Console.WriteLine($"\nDispositivos na fila: {seguranca.QuantidadeNaFila}");


            int opcao = -1;
            int opcaoFirewall = -1;

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
                Console.WriteLine(" 5. Desfazer Última Autenticação ou Regra(Rollback)");
                Console.WriteLine(" 6. Imprimir Relatório");
                Console.WriteLine(" 7. Definir regras de Firewall");
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


                    case 5:
                        Console.WriteLine("\n--- ROLLBACK DE SEGURANÇA ---");
                        seguranca.DesfazerUltimaAcao();
                        break;

                    case 6:
                        Console.WriteLine("\n--- IMPRIMIR RELATÓRIO ---");
                        seguranca.ImprimirRelatorio();
                        break;

                    case 7:
                        
                        while(opcaoFirewall != 0)
                        {
                            Console.Clear();
                            Console.WriteLine("=====================================");
                            Console.WriteLine("      SMARTNET MANAGER - Firewall   ");
                            Console.WriteLine("=====================================");
                            Console.WriteLine($" Dispositivos aguardando na fila: {seguranca.QuantidadeNaFila}");
                            Console.WriteLine("-------------------------------------");
                            Console.WriteLine(" 1. Bloquear Dispositivo por consumo");
                            Console.WriteLine(" 2. Bloquear Dispositivo por MAC");
                            Console.WriteLine(" 3. Bloquear Dispositivo por trecho de IP");
                            Console.WriteLine(" 4. Bloquear Dispositivo por fabricante");
                            Console.WriteLine(" 0. Sair");
                            Console.WriteLine("=====================================");
                            Console.Write(" Escolha uma opção: ");
                            if (!int.TryParse(Console.ReadLine(), out opcaoFirewall))
                            {
                                opcaoFirewall = -1; // Força continuar no loop
                            }
                            switch (opcaoFirewall)
                            {
                                case 1:
                                    Console.WriteLine("\n--- BLOQUEAR POR CONSUMO ---");
                                    Console.WriteLine("\nInsira o limite de consumo:");
                                    int consumoLimite = int.Parse(Console.ReadLine());
                                    seguranca.AplicarFirewall(d => d.ConsumoBanda > consumoLimite);
                                    break;
                                case 2:
                                    Console.WriteLine("\n--- BLOQUEAR POR MAC ---");
                                    Console.WriteLine("\nInsira o endereço MAC:");
                                    string macAddress = Console.ReadLine();
                                    seguranca.AplicarFirewall(d => d.MacAddress.Equals(macAddress, StringComparison.OrdinalIgnoreCase));
                                    break;
                                case 3:
                                    Console.WriteLine("\n--- BLOQUEAR POR TRECHO DE IP ---");
                                    Console.WriteLine("\nInsira o trecho de IP:");
                                    string trechoIp = Console.ReadLine();
                                    seguranca.AplicarFirewall(d => d.EnderecoIP.Contains(trechoIp));
                                    break;
                                case 4:
                                    Console.WriteLine("\n--- BLOQUEAR POR FABRICANTE ---");
                                    Console.WriteLine("\nInsira o fabricante:");
                                    string fabricante = Console.ReadLine();
                                    seguranca.AplicarFirewall(d => d.Fabricante.Equals(fabricante, StringComparison.OrdinalIgnoreCase));
                                    break;
                                case 0:
                                    break;
                                default:
                                    Console.WriteLine("\n[ERRO] Opção inválida! Tente novamente.");
                                    break;
                            }
                            if (opcaoFirewall != 0)
                            {
                                Console.WriteLine("\nPressione Enter para voltar ao menu do Firewall...");
                                Console.ReadLine();
                            }
                        }

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





        }
    }
}