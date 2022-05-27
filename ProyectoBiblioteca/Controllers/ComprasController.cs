using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class ComprasController : Controller
    {
        // GET: Compra
        public ActionResult RegistrarCompra()
        {
            return View();
        }

        public ActionResult ConsultarCompras()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GuardarCompra(Compras objeto)
        {
            bool _respuesta = false;
            string _mensaje = string.Empty;

            _respuesta = CompraLogica.Instancia.Existe(objeto);

            if (_respuesta)
            {
                _respuesta = false;
                _mensaje = "El lector ya tiene una compra pendiente con el mismo libro";
            }
            else {
                _respuesta = CompraLogica.Instancia.RegistrarCompra(objeto);
                _mensaje = _respuesta ? "Compra Exitosa" : "No se pudo registrar la compra";
            }

            
            return Json(new { resultado = _respuesta, mensaje = _mensaje }, JsonRequestBehavior.AllowGet);
        }

        //////////Controller de la tabla consultar compras


        [HttpGet]
        public JsonResult ListarEstados()
        {
            List<EstadoCompra> oLista = new List<EstadoCompra>();
            oLista = CompraLogica.Instancia.ListarEstados();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult Listar(int idestadoprestamo, int idpersona)
        //{
        //    List<Prestamo> oLista = new List<Prestamo>();
        //    oLista = PrestamoLogica.Instancia.Listar(idestadoprestamo, idpersona);
        //    return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Devolver(string estadorecibido, int idprestamo)
        //{
        //    bool respuesta = false;
        //    respuesta = PrestamoLogica.Instancia.Devolver(estadorecibido, idprestamo);
        //    return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        //}
    }
}