using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        // http://localhost:123/mvc/Home/Index
        // http://localhost:123/mvc/Home
        public IActionResult Index()
        {
            MeuModel mm = new MeuModel
            {
                nome = "josé",
                sexo = 0
            };
            var array = new[]
            {
                new {value = 0, text = "Masculino"},
                new {value = 1, text = "Feminino"}
            };
            SelectList opcoes = new SelectList(array, "value", "text", 0);
            ViewData["opcoes"] = opcoes;
            return View(mm);
        }
        // http://localhost:123/mvc/Home/Index
        // http://localhost:123/mvc/Home
        [HttpPost]
        public IActionResult Index(MeuModel meumodel)
        {
            var array = new[]
            {
                new {value = 0, text = "Masculino"},
                new {value = 1, text = "Feminino"}
            };
            SelectList opcoes = new SelectList(array, "value", "text", 0);
            ViewData["opcoes"] = opcoes;
            return View(meumodel);
        }
        // http://localhost:123/mvc/Home/Soma?id=1&b=2
        // http://localhost:123/mvc/Home/Soma/1?b=2
        public IActionResult Soma(int id, int b)
        {
            int c = id + b;
            Resposta resp = new Resposta
            {
                a = id,
                b = b,
                s = c
            };
            return View(resp);
        }
    }
    public class Resposta
    {
        public int a { get; set; }
        public int b { get; set; }
        public int s { get; set; }
    }
    public class MeuModel
    {
        public string nome { get; set; }
        // 0 - masculino
        // 1 - feminino
        public int sexo { get; set; }
    }
}