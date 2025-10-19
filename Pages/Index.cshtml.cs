using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BancoWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private static Queue<int> cola = new Queue<int>();
        private const int CAPACIDAD_MAXIMA = 10;

        public string EstadoCola { get; set; } = string.Empty;
        public int TamanoCola { get; set; }
        public bool EstaVacia { get; set; }
        public bool EstaLlena { get; set; }
        public string Resultado { get; set; } = string.Empty;
        public List<int> ElementosCola { get; set; } = new List<int>();

        [BindProperty]
        public int Valor { get; set; }

        public void OnGet()
        {
            ActualizarEstado();
        }

        public IActionResult OnPostEncolar()
        {
            if (cola.Count >= CAPACIDAD_MAXIMA)
            {
                Resultado = $"❌ Error: Cola llena (capacidad máxima: {CAPACIDAD_MAXIMA})";
            }
            else
            {
                cola.Enqueue(Valor);
                Resultado = $"✅ Encolado: {Valor}";
            }
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostDesencolar()
        {
            if (cola.Count == 0)
            {
                Resultado = "❌ Error: Cola vacía";
            }
            else
            {
                int valor = cola.Dequeue();
                Resultado = $"► Desencolado: {valor}";
            }
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostPeek()
        {
            if (cola.Count == 0)
            {
                Resultado = "❌ Error: Cola vacía";
            }
            else
            {
                int valor = cola.Peek();
                Resultado = $"👁️ Frente de la cola: {valor}";
            }
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostPeekFinal()
        {
            if (cola.Count == 0)
            {
                Resultado = "❌ Error: Cola vacía";
            }
            else
            {
                int[] elementos = cola.ToArray();
                int valor = elementos[elementos.Length - 1];
                Resultado = $"👁️ Final de la cola: {valor}";
            }
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostMostrar()
        {
            if (cola.Count == 0)
            {
                Resultado = "📋 Cola vacía: []";
            }
            else
            {
                ElementosCola = cola.ToList();
                Resultado = $"📋 Cola: [{string.Join(", ", ElementosCola)}]";
            }
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostLimpiar()
        {
            int cantidad = cola.Count;
            cola.Clear();
            Resultado = $"🧹 Cola limpiada ({cantidad} elementos eliminados)";
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostEstaVacia()
        {
            bool vacia = cola.Count == 0;
            Resultado = vacia ? "✅ La cola ESTÁ VACÍA" : "❌ La cola NO está vacía";
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostEstaLlena()
        {
            bool llena = cola.Count >= CAPACIDAD_MAXIMA;
            Resultado = llena ? "✅ La cola ESTÁ LLENA" : "❌ La cola NO está llena";
            ActualizarEstado();
            return Page();
        }

        public IActionResult OnPostTamano()
        {
            Resultado = $"📊 Tamaño de la cola: {cola.Count} / {CAPACIDAD_MAXIMA}";
            ActualizarEstado();
            return Page();
        }

        private void ActualizarEstado()
        {
            TamanoCola = cola.Count;
            EstaVacia = cola.Count == 0;
            EstaLlena = cola.Count >= CAPACIDAD_MAXIMA;
            EstadoCola = EstaVacia ? "Vacía" : EstaLlena ? "Llena" : "Activa";
            ElementosCola = cola.ToList();
        }
    }
}
