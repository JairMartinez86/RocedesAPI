using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocedesAPI.Models.Cls.PLN
{
    public class PlanningCustom
    {
        public int IdPlaningSwing = 0;
        public string Week = string.Empty;
        public int IdCliente = 0;
        public string Cliente = string.Empty;
        public string Modulo = string.Empty;
        public string Linea = string.Empty;
        public Nullable<DateTime> Cut_date_all_component = null;
        public string Ct = string.Empty;
        public string Marker = string.Empty;
        public decimal Largo = 0;
        public string NotasEspeciales = string.Empty;
        public string Origen_segun_wip = string.Empty;
        public string Cutting_plan = string.Empty;
        public string Cut = string.Empty;
        public string Style = string.Empty;
        public Nullable<DateTime> Cut_date_body = null;
        public Nullable<DateTime> foleo_date_body = null;
        public Nullable<DateTime> In_plant = null;
        public decimal Quant = 0;
        public string Status_comp = string.Empty;
        public string Status_cuerpo = string.Empty;
        public string Foleo = string.Empty;
        public string Status_envio = string.Empty;
        public string Fabric = string.Empty;
        public string Pocketing = string.Empty;
        public string Fuse1 = string.Empty;
        public string Fuse2 = string.Empty;
        public string Cordura = string.Empty;
        public string Quilt = string.Empty;
        public string Dracon = string.Empty;
        public string Linning = string.Empty;
        public string Binding1 = string.Empty;
        public string Binding2 = string.Empty;
        public string Sherpa = string.Empty;
        public string Rib = string.Empty;
        public decimal Price = 0;
        public decimal Total = 0;
    }
}