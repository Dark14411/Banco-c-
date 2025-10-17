# 🏦 Sistema de Gestión de Cola Bancaria

Sistema web interactivo para gestionar colas de atención en un banco, desarrollado con **ASP.NET Core 9.0** y **Razor Pages**.

## 🎯 Características

### ✨ Funcionalidades CRUD Completas
- **Crear** tickets de atención con información del cliente
- **Leer** y visualizar tickets organizados por estado
- **Actualizar** información de tickets en espera
- **Eliminar** tickets individuales o limpiar completados

### 🚀 Sistema de Prioridades
- **Normal** (⚪): Atención en orden FIFO estándar
- **Preferente** (⭐): Botón de atención directa
- **Urgente** (🚨): Botón de atención inmediata

### 📊 Panel de Control
- Estadísticas en tiempo real
- Dashboard con contadores dinámicos
- Auto-actualización cada 5 segundos
- Notificaciones de acciones

### 🎨 Interfaz de Usuario
- Diseño moderno con gradientes
- Totalmente responsive
- Animaciones suaves
- Sistema de pestañas (En Espera, Atendiendo, Completados, Todos)

## 🛠️ Tecnologías Utilizadas

- **Backend**: ASP.NET Core 9.0
- **Frontend**: Razor Pages, HTML5, CSS3, JavaScript (ES6+)
- **Arquitectura**: MVC Pattern
- **API**: RESTful API
- **Gestión de Estado**: Singleton Service Pattern

## 📋 Tipos de Operaciones

1. Depósito
2. Retiro
3. Consulta
4. Apertura de Cuenta
5. Pago de Servicios
6. Solicitud de Crédito
7. Transferencia
8. **Robo** (Atención prioritaria)

## 🚀 Instalación y Ejecución

### Requisitos Previos
- .NET 9.0 SDK
- Navegador web moderno

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/Dark14411/Banco-c-.git
cd Banco-c-
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Compilar el proyecto**
```bash
dotnet build
```

4. **Ejecutar la aplicación**
```bash
dotnet run
```

5. **Abrir en el navegador**
```
http://localhost:5132
```

## 📁 Estructura del Proyecto

```
BancoWebApp/
├── Controllers/
│   └── BancoController.cs       # API REST endpoints
├── Models/
│   ├── Ticket.cs                # Modelo de datos del ticket
│   └── Estadisticas.cs          # Modelo de estadísticas
├── Services/
│   └── BancoQueueService.cs     # Lógica de gestión de cola
├── Pages/
│   └── Index.cshtml             # Interfaz de usuario
└── Program.cs                   # Configuración de la aplicación
```

## 🔌 API Endpoints

### Tickets
- `GET /api/banco/tickets` - Obtener todos los tickets
- `GET /api/banco/tickets/{id}` - Obtener ticket por ID
- `POST /api/banco/tickets` - Crear nuevo ticket
- `PUT /api/banco/tickets/{id}` - Actualizar ticket
- `DELETE /api/banco/tickets/{id}` - Eliminar ticket

### Estados
- `GET /api/banco/espera` - Tickets en espera
- `GET /api/banco/atendiendo` - Tickets en atención
- `GET /api/banco/completados` - Tickets completados

### Acciones
- `POST /api/banco/atender` - Atender siguiente en cola
- `POST /api/banco/atender/{id}` - Atender ticket específico (prioritario)
- `POST /api/banco/completar/{id}` - Completar ticket
- `DELETE /api/banco/limpiar-completados` - Limpiar completados

### Utilidades
- `GET /api/banco/estadisticas` - Obtener estadísticas
- `POST /api/banco/datos-prueba` - Cargar datos de prueba

## 💡 Uso

### Crear un Ticket
1. Ingresa el nombre del cliente
2. Selecciona el tipo de operación
3. Asigna la prioridad
4. Click en "Generar Ticket"

### Atender Clientes
- **Orden normal**: Click en "▶️ Atender Siguiente"
- **Atención prioritaria**: Click en "▶️ Atender" (solo en tickets Preferentes/Urgentes)

### Gestionar Tickets
- **Editar**: Click en "✏️ Editar" (solo en espera)
- **Eliminar**: Click en "🗑️ Eliminar" (solo en espera)
- **Completar**: Click en "✅ Completar" (solo en atención)

## 🎓 Conceptos de Estructuras de Datos

Este proyecto implementa los siguientes conceptos:

- **Cola (Queue)**: FIFO para gestión de tickets en espera
- **Lista (List)**: Para tickets en atención y completados
- **Diccionario (Dictionary)**: Para acceso rápido por ID
- **Singleton Pattern**: Para mantener estado global
- **Thread Safety**: Lock para operaciones concurrentes

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto es de código abierto y está disponible bajo la licencia MIT.

## 👨‍💻 Autor

**Dark14411**
- GitHub: [@Dark14411](https://github.com/Dark14411)

## 🙏 Agradecimientos

- Proyecto desarrollado como ejemplo de implementación de estructuras de datos (Colas)
- Diseño UI inspirado en principios de Material Design
- Patrones de arquitectura basados en buenas prácticas de ASP.NET Core

---

⭐ Si te gusta este proyecto, dale una estrella en GitHub!
