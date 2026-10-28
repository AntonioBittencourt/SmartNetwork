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


        /// Registra uma reserva de IP fixo para um MAC.
        /// Retorna false se o MAC já tem reserva ou o IP já está em uso.

        public bool RegistrarReserva(string macAddress, string enderecoIP)
        {




            if (macAddress is null)
                throw new ArgumentNullException(nameof(macAddress));
            if (enderecoIP is null)
                throw new ArgumentNullException(nameof(enderecoIP));

            // string vazia → ArgumentException (entrada inválida)
            if (string.IsNullOrWhiteSpace(macAddress))
                throw new ArgumentException("MAC não pode ser vazio.", nameof(macAddress));
            if (string.IsNullOrWhiteSpace(enderecoIP))
                throw new ArgumentException("IP não pode ser vazio.", nameof(enderecoIP));



            //Validar formato do IP via IPAddress.TryParse e Normalizar MAC (maiúsculo) — consistência com Dispositivo


            if (!IPAddress.TryParse(enderecoIP, out var ipNormalizadoObj))
                throw new ArgumentException($"IP '{enderecoIP}' é inválido.", nameof(enderecoIP));



            string ipNormalizado = ipNormalizadoObj.ToString();

            string macNormalizado = macAddress.ToUpperInvariant();

            // Verificar duplicidade de MAC
            if (_reservasPorMac.ContainsKey(macNormalizado))
                return false;




            // Verificar duplicidade de IP — 1 loop só, parse 1 vez fora

            foreach (var ipExistente in _reservasPorMac.Values)
            {
                if (IPAddress.TryParse(ipExistente, out var ipObj) &&
                    ipObj.Equals(ipNormalizadoObj))
                    return false;
            }




            // Inserir
            _reservasPorMac.Add(macNormalizado, ipNormalizado);
            return true;
        }


        /// Busca o IP reservado para um MAC.
        /// Retorna null se não houver reserva.

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


        /// Remove a reserva de um MAC.

        public bool RemoverReserva(string macAddress)
        {
            //  ArgumentNullException.ThrowIfNull(macAddress);
            if (macAddress is null)
                throw new ArgumentNullException(nameof(macAddress));

            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            return _reservasPorMac.Remove(macAddress.ToUpperInvariant());
        }


        /// Lista todas as reservas (MAC → IP).

        public IReadOnlyDictionary<string, string> ListarReservas()
        {
            return _reservasPorMac;
        }


        /// Quantidade de reservas ativas.

        public int TotalReservas => _reservasPorMac.Count;
    }
}
