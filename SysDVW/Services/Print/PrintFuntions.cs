using Microsoft.AspNetCore.Http;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System;

namespace SysDVW.Services.Print
{
    public class PrintFuntions : IPrintFuntions
    {
        public PrintViewModel PrintView { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PrintFuntions(IHttpContextAccessor httpContextAccessor)
        {
            PrintView = new PrintViewModel();
            _httpContextAccessor = httpContextAccessor;
        }
        public void tickFuntion()
        {
            CrearTiket ticket = new CrearTiket();
            string nombre = "";
            string cedula = "";

            var Empresa = _httpContextAccessor.HttpContext.Session.Get<NomEmp>(ConstactSession.DataBussiness);
            var Empleado = _httpContextAccessor.HttpContext.Session.Get<Empleado>(ConstactSession.Employee);
            PrintView = _httpContextAccessor.HttpContext.Session.Get<PrintViewModel>(ConstactSession.Print);
            if (PrintView != null)
            {
                //cabecera del ticket.
                if (PrintView.ReImpresion)
                {
                    ticket.TextoDerecha("Copia de Factura");
                }

                //System.Drawing.Image img = System.Drawing.Image.FromFile("LogoCepeda.png");
                //ticket.HeaderImage = img;
                ticket.TextoCentro(Empresa.NombreEmp);
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda(Empresa.DirEmp);
                ticket.TextoIzquierda("Tel: " + Empresa.Tel1 + "/" + Empresa.Tel2);
                ticket.TextoIzquierda("Correo: " + Empresa.Correo);
                ticket.TextoIzquierda("Tipo de Comprobante: " + PrintView.Sales.TipoDocumento);
                ticket.TextoIzquierda("Tipo de Factura: " + PrintView.Sales.TipoFactura.ToUpper());
                ticket.TextoIzquierda("Numero de Comprobante: " + PrintView.Sales.NroDocumento);
                ticket.TextoIzquierda("RNC: " + Empresa.RNC);
                ticket.TextoExtremos("CAJA #1", "ID VENTA: " + PrintView.Sales.IdVenta);
                ticket.lineasGuio();

                if (PrintView.Sales.IdCliente != null || PrintView.Sales.IdCliente > 0)
                {
                    nombre = PrintView.Sales.NombreCliente;
                    cedula = PrintView.DocClient;
                }
                else
                {
                    nombre = PrintView.Sales.NombreCliente;
                    cedula = "Sin Identificación";
                }

                //SUB CABECERA.
                ticket.TextoIzquierda("Atendido Por: " + Empleado.Nombres + " " + Empleado.Apellidos);
                ticket.TextoIzquierda("Cliente: " + nombre);
                ticket.TextoIzquierda("Documento de Identificación: " + cedula);
                ticket.TextoIzquierda("Fecha: " + PrintView.Sales.FechaVenta);
                ticket.TextoIzquierda("Hora: " + DateTime.Now.ToShortTimeString());

                //ARTICULOS A VENDER.
                ticket.EncabezadoVenta();// NOMBRE DEL ARTICULO, CANT, PRECIO, IMPORTE
                ticket.lineasGuio();
                //SI TIENE UN DATAGRIDVIEW DONDE ESTAN SUS ARTICULOS A VENDER PUEDEN USAR ESTA MANERA PARA AGREARLOS
                foreach (var fila in PrintView.DetailSalesList)
                {
                    ticket.AgregaArticulo((fila.detalles_P).Trim(), int.Parse((fila.Cantidad.ToString()).Trim()),
                    decimal.Parse((fila.SubTotal.ToString()).Trim()), decimal.Parse((fila.Igv.ToString()).Trim()));
                }

                ticket.TextoIzquierda(" ");
                //resumen de la venta
                ticket.AgregarTotales("TOTAL    : ", decimal.Parse(PrintView.Sales.Total.ToString()));
                ticket.AgregarTotales("RESTANTE : ", decimal.Parse(PrintView.Sales.Restante.ToString()));
                ticket.TextoIzquierda(" ");
                ticket.TextoCentro("__________________________________");

                //TEXTO FINAL DEL TICKET
                ticket.TextoIzquierda("EXTRA");
                ticket.TextoIzquierda("FAVOR REVISE SU MERCANCIA AL RECIBIRLA");
                ticket.TextoCentro("!GRACIAS POR SU COMPRA!");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.CortaTicket();//CORTAR TICKET
                ticket.ImprimirTicket(Empresa.PrintName);//NOMBRE DE LA IMPRESORA
            }
        }

        ///PDF Funtions
        public void PDFfuntion()
        {

        }
    }
}
