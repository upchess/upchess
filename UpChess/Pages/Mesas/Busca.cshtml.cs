using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Business;
using WebApplication4.Data;

namespace WebApplication4.Pages.Mesas
{
    public class BuscaModel : PageModel
    {
        IJogoService _jogoService;
        public BuscaModel(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        public void OnGet()
        {
        }
    }
}