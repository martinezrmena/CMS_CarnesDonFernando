using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Models
{
    public class SucursalesModel
    {
        public string FotoUrl { get; set; }
        public string Nombre { get; set; }
        public string EnlaceGoogleMaps { get; set; }
        public string IconoGoogleMaps { get; set; }
        public string EnlaceWaze { get; set; }
        public string IconoWaze { get; set; }
        public string TelefonoTienda { get; set; }
        public string TelefonoRestaurante { get; set; }
        public string HorarioTienda { get; set; }
        public string HorarioRestaurante { get; set; }
        public string Telefonos { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

    }

    public class DatosSucursal
    {
        public string nombre { get; set; }
        public string FotoUrl { get; set; }
        public IFormFile foto { get; set; }
        public string direccionGoogleMaps { get; set; }
        public IFormFile iconoGoogleMaps { get; set; }
        public string direccionWaze { get; set; }
        public IFormFile iconoWaze { get; set; }
        public string telefonoTienda { get; set; }
        public string telefonoRestaurante { get; set; }
        public string accion { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string imagenPathWaze{ get; set; }
        public string imagenPathMaps { get; set; }
        public string imagenPath { get; set; }
        public string horarioTienda { get; set; }
        public string horarioRestaurante { get; set; }
    }

    public enum Horarios
    {
        [Display(Name = "12:00 AM")]
        DoceAM,
        [Display(Name = "12:30 AM")]
        DoceMediaAM,
        [Display(Name = "1:00 AM")]
        UnaAM,
        [Display(Name = "1:30 AM")]
        UnaMediaAM,
        [Display(Name = "2:00 AM")]
        DosAM,
        [Display(Name = "2:30 AM")]
        DosMediaAM,
        [Display(Name = "3:00 AM")]
        TresAM,
        [Display(Name = "3:30 AM")]
        TresMediaAM,
        [Display(Name = "4:00 AM")]
        CuatroAM,
        [Display(Name = "4:30 AM")]
        CuatroMediaAM,
        [Display(Name = "5:00 AM")]
        CincoAM,
        [Display(Name = "5:30 AM")]
        CincoMediaAM,
        [Display(Name = "6:00 AM")]
        SeisAM,
        [Display(Name = "6:30 AM")]
        SeisMediaAM,
        [Display(Name = "7:00 AM")]
        SieteAM,
        [Display(Name = "7:30 AM")]
        SieteMediaAM,
        [Display(Name = "8:00 AM")]
        OchoAM,
        [Display(Name = "8:30 AM")]
        OchoMediaAM,
        [Display(Name = "9:00 AM")]
        NueveAM,
        [Display(Name = "9:30 AM")]
        NueveMediaAM,
        [Display(Name = "10:00 AM")]
        DiezAM,
        [Display(Name = "10:30 AM")]
        DiezMediaAM,
        [Display(Name = "11:00 AM")]
        OnceAM,
        [Display(Name = "11:30 AM")]
        OnceMediaAM,

        [Display(Name = "12:00 PM")]
        DocePM,
        [Display(Name = "12:30 PM")]
        DoceMediaPM,
        [Display(Name = "1:00 PM")]
        UnaPM,
        [Display(Name = "1:30 PM")]
        UnaMediaPM,
        [Display(Name = "2:00 PM")]
        DosPM,
        [Display(Name = "2:30 PM")]
        DosMediaPM,
        [Display(Name = "3:00 PM")]
        TresPM,
        [Display(Name = "3:30 PM")]
        TresMediaPM,
        [Display(Name = "4:00 PM")]
        CuatroPM,
        [Display(Name = "4:30 PM")]
        CuatroMediaPM,
        [Display(Name = "5:00 PM")]
        CincoPM,
        [Display(Name = "5:30 PM")]
        CincoMediaPM,
        [Display(Name = "6:00 PM")]
        SeisPM,
        [Display(Name = "6:30 PM")]
        SeisMediaPM,
        [Display(Name = "7:00 PM")]
        SietePM,
        [Display(Name = "7:30 PM")]
        SieteMediaPM,
        [Display(Name = "8:00 PM")]
        OchoPM,
        [Display(Name = "8:30 PM")]
        OchoMediaPM,
        [Display(Name = "9:00 PM")]
        NuevePM,
        [Display(Name = "9:30 PM")]
        NueveMediaPM,
        [Display(Name = "10:00 PM")]
        DiezPM,
        [Display(Name = "10:30 PM")]
        DiezMediaPM,
        [Display(Name = "11:00 PM")]
        OncePM,
        [Display(Name = "11:30 PM")]
        OnceMediaPM,
    }
}


  