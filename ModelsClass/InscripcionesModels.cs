using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using prSistemaAC.Data;
using prSistemaAC.Models;

namespace prSistemaAC.ModelsClass
{
    public class InscripcionesModels
    {
        private ApplicationDbContext context;
        private List<IdentityError> errorList = new List<IdentityError>();
        private string code = "", des = "";

        public InscripcionesModels(ApplicationDbContext context)
        {
            this.context = context;
        }

        public String filtrarEstudiantes(string valor)
        {
            String dataFilter = "";
            if (valor != "null")
            {
                var estudiantes = context.Estudiante.OrderBy(p => p.Nombres).ToList();
                var query = estudiantes.Where(p => p.Documento.StartsWith(valor) || p.Nombres.StartsWith(valor) || p.Apellidos.StartsWith(valor));
                foreach (var item in query)
                {
                    dataFilter += "<tr>" +
                       "<td>" + "<input type='checkbox'name='cboxEstudiante[]' id='cboxEstudiante' value='" + item.ID + "'>" + "</td>" +
                       "<td>" + item.Apellidos + " " + item.Nombres + "</td>" +
                       "<td>" + item.Documento + "</td>" +
                        "<td>" + item.Email + "</td>" +
                       "<td>" + item.Telefono + "</td>" +
                   "</tr>";
                }
            }
            return dataFilter;
        }

        internal List<IdentityError> guardarCursos(List<Inscripcion> listCursos)
        {
            try
            {
                for (int i = 0; i < listCursos.Count; i++)
                {
                    context.Add(listCursos[i]);
                    context.SaveChanges();
                }
                code = "Save";
                des = "Save";
            }
            catch (Exception e)
            {

                code = "error";
                des = e.Message;
            }
            errorList.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return errorList;
        }

        internal List<Curso> getCursos(int id)
        {
            return context.Curso.Where(c => c.CursoID == id).ToList();
        }

        internal string filtrarCurso(string valor)
        {
            String dataFilter = "";
            if (valor != "null")
            {
                var listCorsos = from c in context.Curso
                                 join a in context.Asignacion on c.CursoID equals a.CursoID
                                 select new
                                 {
                                     c.CursoID,
                                     c.Nombre,
                                     c.CategoriaID,
                                     c.Creditos,
                                     c.Horas,
                                     c.Costo,

                                 };
                var curso = listCorsos.OrderBy(p => p.Nombre).ToList();
                var query = curso.Where(p => p.Nombre.StartsWith(valor));
                foreach (var item in query)
                {
                    dataFilter += "<tr>" +
                       "<td>" + "<input type='checkbox'name='cboxCurso[]'  id='cboxCurso' value='" + item.CursoID + "'>" + "</td>" +
                       "<td>" + item.Nombre + "</td>" +
                       "<td>" + getCategorias(item.CategoriaID) + "</td>" +
                        "<td>" + item.Creditos + "</td>" +
                       "<td>" + item.Horas + "</td>" +
                       "<td>" + item.Costo + "</td>" +
                   "</tr>";
                }
            }
            return dataFilter;
        }

        public List<Estudiante> getEstudiante(int id)
        {
            return context.Estudiante.Where(c => c.ID == id).ToList();
        }
        public String getCategorias(int id)
        {
            var data = context.Categoria.Where(c => c.CategoriaID == id).ToList();
            return data[0].Nombre;
        }
    }
}
