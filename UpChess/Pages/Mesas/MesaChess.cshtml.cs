using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApplication4.Business;
using WebApplication4.Data;

namespace WebApplication4.Pages.Mesas
{
    public class MesaChessModel : PageModel
    {
        IJogoService _jogoService;
        public MesaChessModel(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        [BindProperty(SupportsGet = true)]
        public int MesaId { get; set; }

        public MesaChessConfig MesaConfig { get; set; }
        public Mesa MesaState { get; set; }

        public IActionResult OnGet()
        {
            int usuarioId = HttpContext.Session.GetInt32("usuarioId") ?? 0;
            // obtém estado atual do jogo
            MesaState = _jogoService.DadosMesa(MesaId);
            if (MesaState == null)
            {
                return RedirectToPage("/erro");
            }
            // verifica se algum usuário já escolheu a cor
            if (MesaState.Configuracao != null)
            {
                MesaConfig = JsonConvert.DeserializeObject<MesaChessConfig>(MesaState.Configuracao);
            }
            if (MesaConfig == null)
            {
                // se não tem config, o primeiro jogador que abrir a mesa é o branco
                MesaConfig = new MesaChessConfig { WhiteUserId = usuarioId };
                MesaState.Configuracao = JsonConvert.SerializeObject(MesaConfig);
                _jogoService.SalvarEstadoMesa();
            }

            return Page();
        }
        public class MesaChessConfig
        {
            public int WhiteUserId { get; set; }
        }
    }
}