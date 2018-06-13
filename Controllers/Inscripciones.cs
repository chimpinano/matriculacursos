using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using prSistemaAC.Data;
using prSistemaAC.Models;
using prSistemaAC.ModelsClass;

namespace prSistemaAC.Controllers
{
    public class Inscripciones : Controller
    {
        private InscripcionesModels inscripcion;

        public Inscripciones(ApplicationDbContext context)
        {
            inscripcion = new InscripcionesModels(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public String filtrarEstudiantes(string valor)
        {
            return inscripcion.filtrarEstudiantes(valor);
        }
        public List<Estudiante> getEstudiante(int id)
        {
            return inscripcion.getEstudiante(id);
        }
        public String filtrarCurso(string valor)
        {
            return inscripcion.filtrarCurso(valor);
        }
        public List<Curso> getCurso(int id)
        {
            return inscripcion.getCursos(id);
        }
        public List<IdentityError> guardarCursos(List<Inscripcion> listCursos)
        {
            return inscripcion.guardarCursos(listCursos);
        }
    }
}