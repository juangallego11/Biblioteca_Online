using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Compras
    {
        public int IdCompra { get; set; }
        public EstadoCompra oEstadoCompra { get; set; }
        public Persona oPersona { get; set; }
        public Libro oLibro { get; set; }
        public string Precio { get; set; }
        public string Direccion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; } 
    }
}