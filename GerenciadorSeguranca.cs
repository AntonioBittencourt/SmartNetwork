using System;
using System.Collections.Generic;

namespace SmartNetwork
{
    public class GerenciadorSeguranca
    {
        // Fila (Queue) que armazena os dispositivos aguardando análise
        private readonly Queue<Dispositivo> _filaDeEspera = new Queue<Dispositivo>();


        /// Enfileira um dispositivo para verificação de segurança.

        public void EnfileirarDispositivo(Dispositivo dispositivo)
        {
            if (dispositivo == null)
                throw new ArgumentNullException(nameof(dispositivo));

            _filaDeEspera.Enqueue(dispositivo);
            Console.WriteLine($"[FILA] Dispositivo {dispositivo.Nome} (MAC: {dispositivo.MacAddress}) adicionado à fila de verificação.");
        }


        /// Remove e retorna o próximo dispositivo da fila para ser verificado.

        public Dispositivo? DesenfileirarProximo()
        {
            if (_filaDeEspera.Count == 0)
            {
                Console.WriteLine("[FILA] A fila de verificação está vazia.");
                return null; // Ou você pode lançar uma exceção, se preferir.
            }

            Dispositivo proximo = _filaDeEspera.Dequeue();
            Console.WriteLine($"[FILA] Dispositivo {proximo.Nome} removido da fila para verificação.");
            return proximo;
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
    }
}