using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoBiblioteca.Logica
{
    public class CompraLogica
    {
        private static CompraLogica instancia = null;

        public CompraLogica()
        {

        }

        public static CompraLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new CompraLogica();
                }

                return instancia;
            }
        }

        public bool RegistrarCompra(Compras objeto)
        {   
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCompra", oConexion);
                    cmd.Parameters.AddWithValue("IdEstadoCompra", objeto.oEstadoCompra.IdEstadoCompra);
                    cmd.Parameters.AddWithValue("IdPersona", objeto.oPersona.IdPersona);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.oLibro.IdLibro);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio);
                    cmd.Parameters.AddWithValue("Direccion", objeto.Direccion);
                    cmd.Parameters.Add("Estado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Estado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Existe(Compras objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_existeCompra", oConexion);
                    cmd.Parameters.AddWithValue("IdPersona", objeto.oPersona.IdPersona);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.oLibro.IdLibro);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public List<EstadoCompra> ListarEstados()
        {
            List<EstadoCompra> Lista = new List<EstadoCompra>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select IdEstadoCompra,Descripcion from ESTADO_COMPRA", oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new EstadoCompra()
                            {
                                IdEstadoCompra = Convert.ToInt32(dr["IdEstadoCompra"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<EstadoCompra>();
                }
            }
            return Lista;
        }


        //public List<Prestamo> Listar(int idestadoprestamo, int idpersona)
        //{
        //    List<Prestamo> Lista = new List<Prestamo>();
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            StringBuilder query = new StringBuilder();

        //            query.AppendLine("set dateformat dmy");
        //            query.AppendLine("select p.IdPrestamo,ep.IdEstadoPrestamo,ep.Descripcion,pe.Codigo,pe.Nombre,pe.Apellido,li.Titulo,convert(char(10),p.FechaDevolucion,103)[FechaDevolucion],convert(char(10),p.FechaConfirmacionDevolucion,103)[FechaConfirmacionDevolucion],p.EstadoEntregado,p.EstadoRecibido from prestamo p");
        //            query.AppendLine("inner join estado_prestamo ep on ep.IdEstadoPrestamo = p.IdEstadoPrestamo");
        //            query.AppendLine("inner join PERSONA pe on pe.IdPersona = p.IdPersona");
        //            query.AppendLine("inner join LIBRO li on li.IdLibro = p.IdLibro");
        //            query.AppendLine("where ep.IdEstadoPrestamo = iif(@idestadoprestamo=0,ep.IdEstadoPrestamo,@idestadoprestamo) and pe.IdPersona = iif(@idpersona=0,pe.IdPersona,@idpersona)");


        //            SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
        //            cmd.Parameters.AddWithValue("@idestadoprestamo", idestadoprestamo);
        //            cmd.Parameters.AddWithValue("@idpersona", idpersona);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    Lista.Add(new Prestamo()
        //                    {
        //                        IdPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
        //                        oEstadoPrestamo = new EstadoPrestamo() { IdEstadoPrestamo= Convert.ToInt32(dr["IdEstadoPrestamo"]),  Descripcion = dr["Descripcion"].ToString() },
        //                        oPersona = new Persona() { Codigo = dr["Codigo"].ToString(), Nombre = dr["Nombre"].ToString(), Apellido = dr["Apellido"].ToString() },
        //                        oLibro= new Libro() { Titulo = dr["Titulo"].ToString() },
        //                        TextoFechaDevolucion = dr["FechaDevolucion"].ToString(),
        //                        TextoFechaConfirmacionDevolucion = dr["FechaConfirmacionDevolucion"].ToString(),
        //                        EstadoEntregado = dr["EstadoEntregado"].ToString(),
        //                        EstadoRecibido = dr["EstadoRecibido"].ToString()
        //                    });
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Lista = new List<Prestamo>();
        //        }
        //    }
        //    return Lista;
        //}

        //public bool Devolver(string estadorecibido, int idprestamo)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            StringBuilder query = new StringBuilder();
        //            query.AppendLine("update prestamo set IdEstadoPrestamo = 2 ,FechaConfirmacionDevolucion = GETDATE(),EstadoRecibido =@estadorecibido");
        //            query.AppendLine("where IdPrestamo = @idprestamo");

        //            SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
        //            cmd.Parameters.AddWithValue("@estadorecibido", estadorecibido);
        //            cmd.Parameters.AddWithValue("@idprestamo", idprestamo);
        //            cmd.CommandType = CommandType.Text;
        //            oConexion.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            respuesta = false;
        //        }
        //    }
        //    return respuesta;
        //}



    }
}