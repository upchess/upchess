using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Pages.Usuarios
{
    public class DetailsModel : PageModel
    {
        private readonly WebApplication4.Models.WebApplication4Context _context;

        public DetailsModel(WebApplication4.Models.WebApplication4Context context)
        {
            _context = context;
        }

        public Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Usuario = await _context.Usuario.FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (Usuario == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
