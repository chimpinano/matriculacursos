using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using prSistemaAC.Data;
using prSistemaAC.ModelsClass;

namespace prSistemaAC.Controllers
{
    public class ReportesController : Controller
    {
        private ApplicationDbContext _context;
        private CursoModels cursoModels;
        private MisCursosModels inscripcion;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
            cursoModels = new CursoModels(context);
            inscripcion = new MisCursosModels(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<object[]> reportesCursos(String valor, int numPagina, String order, int funcion)
        {
            String thead = "<tr>" +
                    "<th>Nombre</th>" +
                    "<th>Descripcion</th>" +
                    "<th>Creditos</th>" +
                    "<th>Horas</th>" +
                    "<th>Costo</th>" +
                    "<th>Estado</th>" +
                    "<th>Categoria</th>" +
                "</tr> ";
            object[] dataObj = { thead };
            var reportes = cursoModels.filtrarCursoReporte(numPagina, valor, order, funcion);
            reportes.Add(dataObj);
            return reportes;
        }
        public int[] estadosCursos()
        {
            return cursoModels.estadosCursos();
        }

    }
}