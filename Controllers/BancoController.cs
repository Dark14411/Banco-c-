using BancoWebApp.Models;
using BancoWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancoController : ControllerBase
    {
        private readonly BancoQueueService _bancoService;

        public BancoController(BancoQueueService bancoService)
        {
            _bancoService = bancoService;
        }

        // GET: api/banco/tickets
        [HttpGet("tickets")]
        public ActionResult<List<Ticket>> ObtenerTodosTickets()
        {
            return Ok(_bancoService.ObtenerTodosLosTickets());
        }

        // GET: api/banco/tickets/{id}
        [HttpGet("tickets/{id}")]
        public ActionResult<Ticket> ObtenerTicket(int id)
        {
            var ticket = _bancoService.ObtenerTicketPorId(id);
            if (ticket == null)
                return NotFound(new { mensaje = $"Ticket {id} no encontrado" });
            
            return Ok(ticket);
        }

        // GET: api/banco/espera
        [HttpGet("espera")]
        public ActionResult<List<Ticket>> ObtenerTicketsEnEspera()
        {
            return Ok(_bancoService.ObtenerTicketsEnEspera());
        }

        // GET: api/banco/atendiendo
        [HttpGet("atendiendo")]
        public ActionResult<List<Ticket>> ObtenerTicketsAtendiendo()
        {
            return Ok(_bancoService.ObtenerTicketsAtendiendo());
        }

        // GET: api/banco/completados
        [HttpGet("completados")]
        public ActionResult<List<Ticket>> ObtenerTicketsCompletados()
        {
            return Ok(_bancoService.ObtenerTicketsCompletados());
        }

        // GET: api/banco/estadisticas
        [HttpGet("estadisticas")]
        public ActionResult<Estadisticas> ObtenerEstadisticas()
        {
            return Ok(_bancoService.ObtenerEstadisticas());
        }

        // POST: api/banco/tickets
        [HttpPost("tickets")]
        public ActionResult<Ticket> CrearTicket([FromBody] CrearTicketRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NombreCliente) || 
                string.IsNullOrWhiteSpace(request.TipoOperacion))
            {
                return BadRequest(new { mensaje = "Nombre de cliente y tipo de operación son requeridos" });
            }

            var ticket = _bancoService.GenerarTicket(
                request.NombreCliente, 
                request.TipoOperacion, 
                request.Prioridad);

            return CreatedAtAction(nameof(ObtenerTicket), new { id = ticket.Id }, ticket);
        }

        // PUT: api/banco/tickets/{id}
        [HttpPut("tickets/{id}")]
        public ActionResult ActualizarTicket(int id, [FromBody] ActualizarTicketRequest request)
        {
            var resultado = _bancoService.ActualizarTicket(
                id, 
                request.NombreCliente, 
                request.TipoOperacion, 
                request.Prioridad);

            if (!resultado)
                return NotFound(new { mensaje = $"Ticket {id} no encontrado" });

            return Ok(new { mensaje = "Ticket actualizado correctamente", ticketId = id });
        }

        // POST: api/banco/atender
        [HttpPost("atender")]
        public ActionResult<Ticket> AtenderSiguiente()
        {
            var ticket = _bancoService.AtenderSiguiente();
            if (ticket == null)
                return NotFound(new { mensaje = "No hay tickets en espera" });

            return Ok(ticket);
        }

        // POST: api/banco/atender/{id}
        [HttpPost("atender/{id}")]
        public ActionResult<Ticket> AtenderTicketPorId(int id)
        {
            var ticket = _bancoService.AtenderTicketPorId(id);
            if (ticket == null)
                return NotFound(new { mensaje = $"Ticket {id} no encontrado en la cola de espera" });

            return Ok(ticket);
        }

        // POST: api/banco/completar/{id}
        [HttpPost("completar/{id}")]
        public ActionResult CompletarTicket(int id)
        {
            var resultado = _bancoService.CompletarTicket(id);
            if (!resultado)
                return NotFound(new { mensaje = $"Ticket {id} no encontrado o no está siendo atendido" });

            return Ok(new { mensaje = $"Ticket {id} completado correctamente" });
        }

        // DELETE: api/banco/tickets/{id}
        [HttpDelete("tickets/{id}")]
        public ActionResult EliminarTicket(int id)
        {
            var resultado = _bancoService.EliminarTicket(id);
            if (!resultado)
                return BadRequest(new { mensaje = "No se puede eliminar el ticket. Solo se pueden eliminar tickets en espera" });

            return Ok(new { mensaje = $"Ticket {id} eliminado correctamente" });
        }

        // DELETE: api/banco/limpiar-completados
        [HttpDelete("limpiar-completados")]
        public ActionResult LimpiarCompletados()
        {
            var cantidad = _bancoService.LimpiarCompletados();
            return Ok(new { mensaje = $"{cantidad} tickets completados han sido eliminados" });
        }

        // POST: api/banco/datos-prueba
        [HttpPost("datos-prueba")]
        public ActionResult CargarDatosPrueba()
        {
            _bancoService.CargarDatosPrueba();
            return Ok(new { mensaje = "Datos de prueba cargados correctamente" });
        }
    }

    public class CrearTicketRequest
    {
        public string NombreCliente { get; set; } = string.Empty;
        public string TipoOperacion { get; set; } = string.Empty;
        public int Prioridad { get; set; } = 0;
    }

    public class ActualizarTicketRequest
    {
        public string? NombreCliente { get; set; }
        public string? TipoOperacion { get; set; }
        public int? Prioridad { get; set; }
    }
}
