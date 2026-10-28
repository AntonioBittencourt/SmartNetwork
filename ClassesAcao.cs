using System;
using System.Collections.Generic;

namespace SmartNetwork
{
    // Interface base para qualquer ação no sistema
    public interface IAcao
    {
        void Desfazer();
        string Descricao { get; }
    }

    // Ação: Autenticar um dispositivo
    public class AcaoAutenticar : IAcao
    {
        private readonly Dispositivo _dispositivo;
        private readonly Dictionary<string, Dispositivo> _listaDeAtivos;

        public AcaoAutenticar(Dispositivo dispositivo, Dictionary<string, Dispositivo> listaDeAtivos)
        {
            _dispositivo = dispositivo;
            _listaDeAtivos = listaDeAtivos;
        }

        public string Descricao => $"Autenticação do dispositivo '{_dispositivo.Nome}' (MAC: {_dispositivo.MacAddress})";

        public void Desfazer()
        {
            // Remove o dispositivo da rede
            _listaDeAtivos.Remove(_dispositivo.MacAddress);
            Console.WriteLine($"[ROLLBACK] O dispositivo '{_dispositivo.Nome}' foi removido da rede.");
        }
    }

    // Ação: Aplicar uma regra de Firewall
    public class AcaoFirewall : IAcao
    {
        private readonly List<Dispositivo> _dispositivosBloqueados;
        private readonly Dictionary<string, Dispositivo> _listaDeAtivos;

        public AcaoFirewall(List<Dispositivo> dispositivosBloqueados, Dictionary<string, Dispositivo> listaDeAtivos)
        {
            _dispositivosBloqueados = dispositivosBloqueados;
            _listaDeAtivos = listaDeAtivos;
        }

        public string Descricao => $"Regra de Firewall que bloqueou {_dispositivosBloqueados.Count} dispositivo(s).";

        public void Desfazer()
        {
            // Devolve os dispositivos bloqueados para a rede
            foreach (var disp in _dispositivosBloqueados)
            {
                _listaDeAtivos.Add(disp.MacAddress, disp);
                Console.WriteLine($"[ROLLBACK] O dispositivo '{disp.Nome}' foi RESTAURADO na rede.");
            }
        }
    }
}