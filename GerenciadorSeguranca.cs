using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartNetwork
{
    public class GerenciadorSeguranca
    {
        // Fila (Queue) que armazena os dispositivos aguardando análise
        private readonly Queue<Dispositivo> _filaDeEspera = new Queue<Dispositivo>();


        // Lista de Ativos (Já autenticados e na rede)
        private readonly Dictionary<string, Dispositivo> _dispositivosAtivos = new Dictionary<string, Dispositivo>();


        // Pilha (Stack) que armazena o histórico de ações para Rollback
        private readonly Stack<Dispositivo> _historicoAcoes = new Stack<Dispositivo>();


        /// Enfileira um dispositivo para verificação de segurança.

        public void EnfileirarDispositivo(Dispositivo dispositivo)
        {
            if (dispositivo == null)
                throw new ArgumentNullException(nameof(dispositivo));

            _filaDeEspera.Enqueue(dispositivo);
            Console.WriteLine($"[FILA] Dispositivo {dispositivo.Nome} (MAC: {dispositivo.MacAddress}) adicionado à fila de verificação.");

        }


        /// Remove o próximo da fila, autentica e move para a Lista de Ativos.
        public void ProcessarProximoDaFila()
        {
            if (_filaDeEspera.Count == 0)
            {
                Console.WriteLine("[AVISO] A fila de verificação está vazia. Ninguém para processar.");
                return;
            }

            /// Remove o mais antigo da fila (FIFO)
            Dispositivo dispositivo = _filaDeEspera.Dequeue();

            /// Adiciona à lista de ativos (usando o MAC como chave única)
            if (!_dispositivosAtivos.ContainsKey(dispositivo.MacAddress))
            {
                _dispositivosAtivos.Add(dispositivo.MacAddress, dispositivo);

                //registro no histórico de ações para possível rollback
                _historicoAcoes.Push(dispositivo);
                Console.WriteLine($"[SUCESSO] Dispositivo '{dispositivo.Nome}' (MAC: {dispositivo.MacAddress}) AUTENTICADO e movido para a Lista de Ativos.");
            }
            else
            {
                Console.WriteLine($"[ERRO] Dispositivo com MAC {dispositivo.MacAddress} já está na Lista de Ativos.");
            }
        }



        /// Desfaz a última autenticação, removendo o dispositivo dos ativos.

        public void DesfazerUltimaAcao()
        {
            // Verifica se há algo no histórico
            if (_historicoAcoes.Count == 0)
            {
                Console.WriteLine("[AVISO] Não há ações recentes para desfazer.");
                return;
            }

            // Remove o último dispositivo do topo da pilha (LIFO)
            Dispositivo dispositivoRevertido = _historicoAcoes.Pop();

            // Remove ele da lista de ativos
            if (_dispositivosAtivos.Remove(dispositivoRevertido.MacAddress))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[ROLLBACK] A autenticação do dispositivo '{dispositivoRevertido.Nome}' (MAC: {dispositivoRevertido.MacAddress}) foi DESFEITA.");
                Console.WriteLine("Ele foi removido da lista de ativos.");
                Console.ResetColor();
            }
        }




        /// Apenas visualiza quem é o próximo da fila, sem removê-lo.

        public Dispositivo? EspiarProximo()
        {
            if (_filaDeEspera.Count == 0)
                return null;

            return _filaDeEspera.Peek();
        }


        /// Retorna a quantidade de dispositivos aguardando na fila.

        public int QuantidadeNaFila => _filaDeEspera.Count;



        /// Retorna a lista de dispositivos que já foram autenticados.
        public IReadOnlyDictionary<string, Dispositivo> ObterAtivos()
        {
            return _dispositivosAtivos;
        }



        /// <summary>
        /// Retorna uma lista (List) dos dispositivos que estão online (ativos).
        /// </summary>
        public List<Dispositivo> ObterDispositivosOnline()
        {
            // Converte os valores do Dictionary em uma List
            return new List<Dispositivo>(_dispositivosAtivos.Values);
        }

        /// <summary>
        /// Gera o painel estatístico usando LINQ.
        /// </summary>
        public void ExibirDashboard()
        {
            List<Dispositivo> online = ObterDispositivosOnline();

            if (online.Count == 0)
            {
                Console.WriteLine("Não há dispositivos online no momento.");
                return;
            }

            // 1. Total de dispositivos
            int total = online.Count();

            // 2. Consumo total de banda (Soma)
            decimal consumoTotal = online.Sum(d => d.ConsumoBanda);

            // 3. Consumo médio de banda (Average)
            decimal consumoMedio = online.Average(d => d.ConsumoBanda);

            // 4. Quantidade de dispositivos por Fabricante (GroupBy)
            var porFabricante = online
                .GroupBy(d => d.Fabricante)
                .Select(g => new { Fabricante = g.Key, Quantidade = g.Count() })
                .OrderByDescending(g => g.Quantidade); // Ordena do que tem mais para o que tem menos

            // 5. Dispositivo que mais consome banda (OrderByDescending + First)
            Dispositivo topConsumidor = online.OrderByDescending(d => d.ConsumoBanda).First();

            // 6. Lista dos MACs ordenados alfabeticamente (Select + OrderBy)
            var macsOrdenados = online.Select(d => d.MacAddress).OrderBy(mac => mac).ToList();

            // ===== EXIBINDO O PAINEL =====
            Console.WriteLine("\n=====================================");
            Console.WriteLine("PAINEL ESTATÍSTICO DA REDE(Dashboard)     ");
            Console.WriteLine("=====================================");
            Console.WriteLine($" Total de Dispositivos Online : {total}");
            Console.WriteLine($" Consumo Total de Banda       : {consumoTotal:F2} Mbps");
            Console.WriteLine($" Consumo Médio de Banda       : {consumoMedio:F2} Mbps");
            Console.WriteLine($" Top Consumidor               : {topConsumidor.Nome} ({topConsumidor.ConsumoBanda} Mbps)");
            // Substitua o seu if(consumoTotal > 500) por este:
            var dispositivosSuspeitos = online.Where(d => d.ConsumoBanda > 500).ToList();

            if (dispositivosSuspeitos.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n [ALERTA DE SEGURANÇA] Dispositivos com consumo anormal (>500 Mbps):");
                foreach (var disp in dispositivosSuspeitos)
                {
                    Console.WriteLine($"  - {disp.Nome} (MAC: {disp.MacAddress}) consumindo {disp.ConsumoBanda} Mbps");
                }
                Console.ResetColor();
            }
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(" Distribuição por Fabricante:");



            foreach (var item in porFabricante)
            {
                Console.WriteLine($"  - {item.Fabricante,-15} : {item.Quantidade} dispositivo(s)");
            }

            Console.WriteLine("-------------------------------------");
            Console.WriteLine(" MACs na rede (ordem alfabética):");
            foreach (string mac in macsOrdenados)
            {
                Console.WriteLine($"  - {mac}");
            }
            Console.WriteLine("=====================================");
        }

    }
}