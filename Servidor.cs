using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartNetwork
{
    public class Servidor
    {
        // MAC → IP (reserva de IP fixo)
        private readonly Dictionary<string, string> _reservasPorMac = new();

        /// <summary>
        /// Registra uma reserva de IP fixo para um MAC.
        /// Retorna false se o MAC já tem reserva ou o IP já está em uso.
        /// </summary>
        public bool RegistrarReserva(string macAddress, string enderecoIP)
        {
            // 1. null → exceção (erro de programação)
            //ArgumentNullException.ThrowIfNull(macAddress);
            //ArgumentNullException.ThrowIfNull(enderecoIP);



            if (macAddress is null)
                throw new ArgumentNullException(nameof(macAddress));
            if (enderecoIP is null)
                throw new ArgumentNullException(nameof(enderecoIP));

            // 2. string vazia → ArgumentException (entrada inválida)
            if (string.IsNullOrWhiteSpace(macAddress))
                throw new ArgumentException("MAC não pode ser vazio.", nameof(macAddress));
            if (string.IsNullOrWhiteSpace(enderecoIP))
                throw new ArgumentException("IP não pode ser vazio.", nameof(enderecoIP));



            // 4.Validar formato do IP via IPAddress.TryParse e Normalizar MAC (maiúsculo) — consistência com Dispositivo


            if (!IPAddress.TryParse(enderecoIP, out var ipNormalizadoObj))
                throw new ArgumentException($"IP '{enderecoIP}' é inválido.", nameof(enderecoIP));



            string ipNormalizado = ipNormalizadoObj.ToString();

            string macNormalizado = macAddress.ToUpperInvariant();

            // 5. Verificar duplicidade de MAC
            if (_reservasPorMac.ContainsKey(macNormalizado))
                return false;

            // 6. Verificar duplicidade de IP (percorre valores)
            // 3. Validar formato do IP e normalizar


            // 6. Verificar duplicidade de IP — 1 loop só, parse 1 vez fora

            foreach (var ipExistente in _reservasPorMac.Values)
            {
                if (IPAddress.TryParse(ipExistente, out var ipObj) &&
                    ipObj.Equals(ipNormalizadoObj))
                    return false;
            }


            /*  foreach (var ipExistente in _reservasPorMac.Values) &&
            ipObj.Equals(ipNormalizadoObj))
            {
                if (IPAddress.Parse(ipExistente).Equals(ipNormalizadoObj))
                    return false;
            }*/


            /*

            foreach (var ipExistente in _reservasPorMac.Values)
            {
                //f (string.Equals(ipExistente, enderecoIP, StringComparison.Ordinal))
                  //return false;

                IPAddress ipNormalizado = IPAddress.Parse(enderecoIP);
                foreach (var ipExistente in _reservasPorMac.Values)
                {
                    if (IPAddress.Parse(ipExistente).Equals(ipNormalizado))
                        return false;
                }

                

            }

            */

            // 7. Inserir
            _reservasPorMac.Add(macNormalizado, ipNormalizado);
            return true;
        }

        /// <summary>
        /// Busca o IP reservado para um MAC.
        /// Retorna null se não houver reserva.
        /// </summary>
        public string? ObterIPPorMac(string macAddress)
        {
            // ArgumentNullException.ThrowIfNull(macAddress);

            if (macAddress is null)
                throw new ArgumentNullException(nameof(macAddress));

            if (string.IsNullOrWhiteSpace(macAddress))
                return null;   // vazio não é erro, é "não encontrado"

            return _reservasPorMac.TryGetValue(macAddress.ToUpperInvariant(), out var ip)
                ? ip
                : null;
        }

        /// <summary>
        /// Remove a reserva de um MAC.
        /// </summary>
        public bool RemoverReserva(string macAddress)
        {
            //  ArgumentNullException.ThrowIfNull(macAddress);
            if (macAddress is null)
                throw new ArgumentNullException(nameof(macAddress));

            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            return _reservasPorMac.Remove(macAddress.ToUpperInvariant());
        }

        /// <summary>
        /// Lista todas as reservas (MAC → IP).
        /// </summary>
        public IReadOnlyDictionary<string, string> ListarReservas()
        {
            return _reservasPorMac;
        }

        /// <summary>
        /// Quantidade de reservas ativas.
        /// </summary>
        public int TotalReservas => _reservasPorMac.Count;
    }
}
