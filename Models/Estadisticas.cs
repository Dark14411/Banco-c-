namespace BancoWebApp.Models
{
    public class Estadisticas
    {
        public int ClientesEnEspera { get; set; }
        public int ClientesAtendiendo { get; set; }
        public int ClientesCompletados { get; set; }
        public int TotalTickets { get; set; }
        public double TiempoPromedioEspera { get; set; }
    }
}
