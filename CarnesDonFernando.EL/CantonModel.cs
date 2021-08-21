using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarnesDonFernando.EL
{
    public class CantonModel
    {
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Descripcion { get; set; }
    }
}
