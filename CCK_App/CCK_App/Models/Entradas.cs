using System;

namespace CCK_App.Models
{
    public class Entradas
    {
        public int idEntradas { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaV { get; set; }
        public DateTime Fecha { get; set; }
        public string email { get; set; }
        public string Evento { get; set; }
        public int Visitantes { get; set; }
        public int idEventos { get; set; }
        public object idProvincia { get; set; }
        public object idLocalidad { get; set; }
        public object Preshow { get; set; }
        public object Show { get; set; }
        public object PreSid { get; set; }
        public object Sid { get; set; }
    }
}