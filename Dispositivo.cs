using System;
using System.Collections.Generic;
//ing System.Linq;
//using System.Net.Mail;
//ing System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Net;

namespace SmartNetwork
{
    public class Dispositivo
    {

        private static readonly Regex MacRegex =
            new(@"^([0-9A-Fa-f]{2}[:-]){5}[0-9A-Fa-f]{2}$", RegexOptions.Compiled);
        private static readonly Regex IpRegex =
        new(@"^(\d{1,3}\.){3}\d{1,3}$", RegexOptions.Compiled);


        private string _macAddress  = string.Empty; // Ex: "AA:BB:CC:11:22"
        private string _enderecoIP = string.Empty;// Ex: "192.168.0.10"



        public string MacAddress
        {
            get => _macAddress;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("MAC não pode ser vazio.", nameof(MacAddress));
                if (!MacRegex.IsMatch(value))
                    throw new ArgumentException("MAC inválido. Use o formato AA:BB:CC:DD:EE:FF.", nameof(MacAddress));
                _macAddress = value.ToUpperInvariant();
            }
        }



        





        public string EnderecoIP
        {
            get => _enderecoIP;



        set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("IP não pode ser vazio.", nameof(EnderecoIP));
                if (!IpRegex.IsMatch(value))
                    throw new ArgumentException(
                        "IP inválido. Use o formato 192.168.0.10.", nameof(EnderecoIP));



                _enderecoIP = value;
            }
        }





        public string Nome { get; set; } = string.Empty;      // Ex: "PC-Contabilidade"
        public string Fabricante { get; set; } = string.Empty; // Ex: "Dell", "Apple", "Samsung"
        public decimal ConsumoBanda { get; set; } // Em Mbps
        public DateTime DataConexao { get; set; }//= DateTime.Now;



        public Dispositivo(string nome, string macAddress, string enderecoIP, string fabricante, decimal consumoBanda) // DateTime dataConexao
        {
            Nome = nome;
               MacAddress = macAddress;
               EnderecoIP = enderecoIP;
            //Dicionario = dicionario;
            Fabricante = fabricante;
            ConsumoBanda = consumoBanda;
            DataConexao = DateTime.Now;
        }


        /*
        foreach(IPAddress)
        Dictionary<string, string> dicionario = new Dictionary<string, string>() = {IPAddress, macadress}
        */




        public override string ToString()
        {
            return $"{Nome} | MAC: {MacAddress} | IP: {EnderecoIP} | Fabricante: {Fabricante} | Consumo de Banda: {ConsumoBanda} Mbps | Data de Conexão: {DataConexao:dd/MM/yyyy HH:mm}";
        }


    }
}
