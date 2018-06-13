using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using prSistemaAC.Data;
using prSistemaAC.Models;

namespace prSistemaAC.ModelsClass
{
    public class MisCursosModels : ListObject
    {
        private string dataFilter = "", paginador = "", curso;
        private int count = 0, cant, numRegistros = 0, inicio = 0, reg_por_pagina = 2;
        private int can_paginas, pagina;
        private string code = "", des = "";

        public MisCursosModels(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<object[]> filtrarMisCurso(int numPagina, string valor)
        {
            count = 0;
            var inscripcion = context.Inscripcion.OrderBy(c => c.Fecha).ToList();
            numRegistros = inscripcion.Count;
            if ((numRegistros % reg_por_pagina) > 0)
            {
                numRegistros += 1;
            }
            inicio = (numPagina - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);
            if (valor == "null")
            {
                // Significa que no estamos filtrando ningun dato
                dataInscripcion = inscripcion.Skip(inicio).Take(reg_por_pagina).ToList();
            }
            else
            {
                cursos = getCursos(valor);
                cursos.ForEach(item => {
                    var data = inscripcion.Where(c => c.CursoID == item.CursoID).Skip(inicio).Take(reg_por_pagina).ToList();
                    if (0 < data.Count)
                    {
                        var inscripciones = new Inscripcion
                        {
                            Grado = data[0].Grado,
                            CursoID = data[0].CursoID,
                            EstudianteID = data[0].EstudianteID,
                            Fecha = data[0].Fecha,
                            Pago = data[0].Pago
                        };
                        dataInscripcion.Add(inscripciones);
                    }
                });
            }
            foreach (var item in dataInscripcion)
            {
                if (0 < cursos.Count)
                    curso = cursos[count].Nombre;
                else
                    curso = getCurso(item.CursoID);
                object[] dataCurso = {
                    curso,
                    getEstudiante(item.EstudianteID),
                    getInstructor(getAsignacion(item.CursoID)),
                    item.Grado,
                    item.Pago,
                    item.Fecha
                };
                dataFilter += "<tr>" +
                   "<td>" + curso + "</td>" +
                   "<td>" + getEstudiante(item.EstudianteID) + "</td>" +
                   "<td>" + getInstructor(getAsignacion(item.CursoID)) + "</td>" +
                   "<td>" + item.Grado + "</td>" +
                   "<td>" + '$' + item.Pago + "</td>" +
                   "<td>" + item.Fecha + " </td>" +
                   "<td>" +
                   "<a data-toggle='modal' data-target='#modalMisCS' onclick='getMisCurso(" + 
                   JsonConvert.SerializeObject(dataCurso) + ',' + 
                   item.InscripcionID + ")'  class='btn btn-success'>Edit</a>" +
                   "</td>" +
               "</tr>";
                count++;
            }
            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;
        }

        internal List<IdentityError> actualizarMisCurso(DataCurso model)
        {
            var curso = context.Curso.Where(c => c.Nombre.Equals(model.Curso)).ToList();
            var estudiantes = model.Estudiante.Split(); // (quedara como un arreglo de 2 posiciones)Separar el nombre y el apellido del estudiante
            var estudiante = context.Estudiante.Where(c => c.Nombres.Equals(estudiantes[0]) || c.Apellidos.Equals(estudiantes[1])).ToList();
            var docentes = model.Estudiante.Split();
            var docente = context.Instructor.Where(c => c.Nombres.Equals(estudiantes[0]) || c.Apellidos.Equals(estudiantes[1])).ToList();

            var inscripcion = new Inscripcion
            {
                InscripcionID = model.InscripcionID,
                Grado = model.Grado,
                CursoID = curso[0].CursoID,
                EstudianteID = estudiante[0].ID,
                Fecha = model.Fecha,
                Pago = model.Pago
            };

            try
            {

                context.Update(inscripcion);
                context.SaveChanges();
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

        internal List<Instructor> getMisDocentes(string query)
        {
            return context.Instructor.Where(c => c.Nombres.StartsWith(query) || c.Apellidos.StartsWith(query)).ToList();
        }

        internal List<Estudiante> getMisEstudiantes(string query)
        {
            return context.Estudiante.Where(c => c.Nombres.StartsWith(query) || c.Apellidos.StartsWith(query)).ToList();
        }

        internal List<Curso> getMisCursos(string query)
        {
            cursos = getCursos(query);
            cursos.ForEach(item =>
            {
                if (getAsignacion2(item.CursoID))
                {
                    // Significa que el curso contiene el ID que esta registrado en la tabla Asignacion
                    misCursos.Add(new Curso
                    {
                        CursoID = item.CursoID,
                        Nombre = item.Nombre
                    });
                }
            });
            return misCursos;
        }
        private bool getAsignacion2(int id)
        {
            var asignacion = context.Asignacion.Where(c => c.CursoID == id).ToList();
            if (0 < asignacion.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Curso> getCursos(String curso)
        {
            return context.Curso.Where(c => c.Nombre.StartsWith(curso)).ToList();
        }
        public String getCurso(int id)
        {
            var curso = context.Curso.Where(c => c.CursoID == id).ToList();
            return curso[0].Nombre;
        }
        private string getEstudiante(int estudianteId)
        {
            var estudiante = context.Estudiante.Where(c => c.ID == estudianteId).ToList();
            return estudiante[0].Nombres + " " + estudiante[0].Apellidos;
        }
        private int getAsignacion(int id)
        {
            var asignacion = context.Asignacion.Where(c => c.CursoID == id).ToList();
            return asignacion[0].InstructorID;
        }
        private string getInstructor(int id)
        {
            var instructor = context.Instructor.Where(c => c.ID == id).ToList();
            return instructor[0].Nombres + " " + instructor[0].Apellidos;
        }
    }
}
