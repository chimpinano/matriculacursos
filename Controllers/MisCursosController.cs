using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prSistemaAC.Data;
using prSistemaAC.Models;
using prSistemaAC.ModelsClass;

namespace prSistemaAC.Controllers
{
    public class MisCursosController : Controller
    {
        private MisCursosModels misCursos;

        public MisCursosController(ApplicationDbContext context)
        {
            misCursos = new MisCursosModels(context);
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<object[]> filtrarMisCurso(int numPagina, string valor)
        {
            return misCursos.filtrarMisCurso(numPagina, valor);
        }
        public List<Curso> getMisCursos(String query)
        {
            return misCursos.getMisCursos(query);
        }
        public List<Estudiante> getMisEstudiantes(string query)
        {
            return misCursos.getMisEstudiantes(query);
        }
        public List<Instructor> getMisDocentes(string query)
        {
            return misCursos.getMisDocentes(query);
        }
        public List<IdentityError> actualizarMisCurso(String data)
        {
            var array = JArray.Parse(data);
            var dataCurso = array[0];
            DataCurso model = JsonConvert.DeserializeObject<DataCurso>(dataCurso.ToString());
            return misCursos.actualizarMisCurso(model);
        }
    }
}