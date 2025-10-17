using BancoWebApp.Models;
using System.Collections.Concurrent;

namespace BancoWebApp.Services
{
    public class BancoQueueService
    {
        private readonly Queue<Ticket> _colaEspera = new();
        private readonly List<Ticket> _ticketsAtendiendo = new();
        private readonly List<Ticket> _ticketsCompletados = new();
        private readonly Dictionary<int, Ticket> _todosTickets = new();
        private int _contadorTickets = 0;
        private readonly object _lock = new object();

        // CREATE - Generar nuevo ticket
        public Ticket GenerarTicket(string nombreCliente, string tipoOperacion, int prioridad = 0)
        {
            lock (_lock)
            {
                var ticket = new Ticket
                {
                    Id = ++_contadorTickets,
                    NombreCliente = nombreCliente,
                    TipoOperacion = tipoOperacion,
                    HoraCreacion = DateTime.Now,
                    Estado = "En Espera",
                    Prioridad = prioridad
                };

                _colaEspera.Enqueue(ticket);
                _todosTickets[ticket.Id] = ticket;
                return ticket;
            }
        }

        // READ - Obtener todos los tickets en espera
        public List<Ticket> ObtenerTicketsEnEspera()
        {
            lock (_lock)
            {
                return _colaEspera.ToList();
            }
        }

        // READ - Obtener ticket por ID
        public Ticket? ObtenerTicketPorId(int id)
        {
            lock (_lock)
            {
                return _todosTickets.ContainsKey(id) ? _todosTickets[id] : null;
            }
        }

        // READ - Obtener todos los tickets
        public List<Ticket> ObtenerTodosLosTickets()
        {
            lock (_lock)
            {
                return _todosTickets.Values.OrderBy(t => t.Id).ToList();
            }
        }

        // READ - Obtener tickets atendiendo
        public List<Ticket> ObtenerTicketsAtendiendo()
        {
            lock (_lock)
            {
                return _ticketsAtendiendo.ToList();
            }
        }

        // READ - Obtener tickets completados
        public List<Ticket> ObtenerTicketsCompletados()
        {
            lock (_lock)
            {
                return _ticketsCompletados.ToList();
            }
        }

        // UPDATE - Actualizar datos de un ticket
        public bool ActualizarTicket(int id, string? nombreCliente = null, string? tipoOperacion = null, int? prioridad = null)
        {
            lock (_lock)
            {
                if (!_todosTickets.ContainsKey(id))
                    return false;

                var ticket = _todosTickets[id];
                
                if (nombreCliente != null)
                    ticket.NombreCliente = nombreCliente;
                
                if (tipoOperacion != null)
                    ticket.TipoOperacion = tipoOperacion;
                
                if (prioridad != null)
                    ticket.Prioridad = prioridad.Value;

                return true;
            }
        }

        // UPDATE - Atender siguiente ticket
        public Ticket? AtenderSiguiente()
        {
            lock (_lock)
            {
                if (_colaEspera.Count == 0)
                    return null;

                var ticket = _colaEspera.Dequeue();
                ticket.Estado = "Atendiendo";
                _ticketsAtendiendo.Add(ticket);
                return ticket;
            }
        }

        // UPDATE - Atender ticket específico por ID (para tickets urgentes)
        public Ticket? AtenderTicketPorId(int id)
        {
            lock (_lock)
            {
                var ticket = _colaEspera.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                    return null;

                // Remover el ticket de la cola
                var nuevaCola = new Queue<Ticket>(_colaEspera.Where(t => t.Id != id));
                _colaEspera.Clear();
                foreach (var t in nuevaCola)
                {
                    _colaEspera.Enqueue(t);
                }

                // Cambiar estado y agregar a atendiendo
                ticket.Estado = "Atendiendo";
                _ticketsAtendiendo.Add(ticket);
                return ticket;
            }
        }

        // UPDATE - Completar un ticket
        public bool CompletarTicket(int id)
        {
            lock (_lock)
            {
                var ticket = _ticketsAtendiendo.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                    return false;

                ticket.Estado = "Completado";
                _ticketsAtendiendo.Remove(ticket);
                _ticketsCompletados.Add(ticket);
                return true;
            }
        }

        // DELETE - Eliminar ticket (solo si está en espera)
        public bool EliminarTicket(int id)
        {
            lock (_lock)
            {
                if (!_todosTickets.ContainsKey(id))
                    return false;

                var ticket = _todosTickets[id];
                
                // Solo se puede eliminar si está en espera
                if (ticket.Estado != "En Espera")
                    return false;

                // Crear una nueva cola sin el ticket eliminado
                var nuevaCola = new Queue<Ticket>(_colaEspera.Where(t => t.Id != id));
                _colaEspera.Clear();
                foreach (var t in nuevaCola)
                {
                    _colaEspera.Enqueue(t);
                }

                _todosTickets.Remove(id);
                return true;
            }
        }

        // DELETE - Limpiar todos los tickets completados
        public int LimpiarCompletados()
        {
            lock (_lock)
            {
                int count = _ticketsCompletados.Count;
                foreach (var ticket in _ticketsCompletados)
                {
                    _todosTickets.Remove(ticket.Id);
                }
                _ticketsCompletados.Clear();
                return count;
            }
        }

        // ESTADÍSTICAS
        public Estadisticas ObtenerEstadisticas()
        {
            lock (_lock)
            {
                return new Estadisticas
                {
                    ClientesEnEspera = _colaEspera.Count,
                    ClientesAtendiendo = _ticketsAtendiendo.Count,
                    ClientesCompletados = _ticketsCompletados.Count,
                    TotalTickets = _todosTickets.Count,
                    TiempoPromedioEspera = CalcularTiempoPromedioEspera()
                };
            }
        }

        private double CalcularTiempoPromedioEspera()
        {
            if (_ticketsCompletados.Count == 0)
                return 0;

            var tiempos = _ticketsCompletados
                .Select(t => (DateTime.Now - t.HoraCreacion).TotalMinutes)
                .ToList();

            return tiempos.Any() ? tiempos.Average() : 0;
        }

        // Método para cargar datos de prueba
        public void CargarDatosPrueba()
        {
            lock (_lock)
            {
                GenerarTicket("Juan Pérez", "Depósito", 0);
                GenerarTicket("María García", "Retiro", 0);
                GenerarTicket("Carlos López", "Consulta", 0);
                GenerarTicket("Ana Martínez", "Apertura de Cuenta", 1);
                GenerarTicket("David Rodríguez", "Pago de Servicios", 0);
                GenerarTicket("Elena Sánchez", "Solicitud de Crédito", 2);
            }
        }
    }
}
