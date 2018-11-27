using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Business;
using WebApplication4.Data;

namespace WebApplication4.Pages.Mesas
{
    public class ListaMinhasMesasModel : PageModel
    {
        IJogoService _jogoService;
        public List<Mesa> mesas { get; set; }
        public ListaMinhasMesasModel(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        public void OnGet()
        {
            int? usuarioId =
               HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                throw new Exception("Erro de permissão");
            }
            mesas = _jogoService.ListarMesa(usuarioId??0);
        }
    }
}