namespace BancoWebApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string TipoOperacion { get; set; } = string.Empty;
        public DateTime HoraCreacion { get; set; }
        public string Estado { get; set; } = "En Espera"; // "En Espera", "Atendiendo", "Completado"
        public int Prioridad { get; set; } = 0; // 0=Normal, 1=Preferente, 2=Urgente
        
        public string HoraCreacionFormato => HoraCreacion.ToString("HH:mm:ss");
        
        public override string ToString()
        {
            return $"Ticket {Id}: {NombreCliente} - {TipoOperacion} ({HoraCreacionFormato}) [{Estado}]";
        }
    }
}
