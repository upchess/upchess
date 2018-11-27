using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication4.Business;
using WebApplication4.Data;

namespace WebApplication4.Pages.Mesas
{
    public class ListaBuscaModel : PageModel
    {
        IJogoService _jogoService;

        [BindProperty]
        [RegularExpression(@"\d{9}-\d{2}", ErrorMessage ="CPF inválido")]
        public string Cpf { get; set; }
        [BindProperty]
        public string NomeUsuario { get; set; }
        public List<Mesa> mesas { get; set; }

        public ListaBuscaModel(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Cpf != null && Cpf.Length > 0)
                {
                    // busca por CPF
                    mesas = _jogoService.ListarMesasDoUsuario(Cpf);
                }
                else if (NomeUsuario != null && NomeUsuario.Length > 0)
                {
                    // busca por Nome
                    mesas = _jogoService.ListarMesasDoUsuarioPeloNome(NomeUsuario);
                }
                else
                {
                    ModelState.AddModelError("", "Preencha ao menos um dos campos");
                    mesas = new List<Mesa>();
                }
            }
            else
            {
                mesas = new List<Mesa>();
            }
        }
    }
}