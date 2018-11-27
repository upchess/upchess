using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Pages.Usuarios
{
    public class ConsultarModel : PageModel
    {
        WebApplication4Context _context;
        public ConsultarModel(WebApplication4Context context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }
        [HttpPost]
        public void OnPost(string cpf)
        {
            Usuario u = _context.Usuario
                .Where(u1 => u1.Cpf == cpf).FirstOrDefault();
            if (u != null)
            {
                ViewData["usuario"] = u;
            }
        }
    }
}