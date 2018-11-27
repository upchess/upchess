using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Business;
using WebApplication4.Data;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovesController : ControllerBase
    {
        IJogoService _jogoService;
        public MovesController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpPost]
        public IActionResult EnviarJogada(Jogada jogada)
        {
            // salva o novo estado
            Mesa mesa = _jogoService.DadosMesa(jogada.MesaId);
            mesa.Historico = jogada.Historico;
            mesa.Estado = jogada.Estado;
            _jogoService.SalvarEstadoMesa();
            return Ok("Ok");
        }
        [HttpGet]
        public IActionResult ObterJogada(int playerId, int mesaId)
        {
            Mesa mesa = _jogoService.DadosMesa(mesaId);
            Jogada jogada = new Jogada
            {
                MesaId = mesa.MesaId,
                Estado = mesa.Estado,
                Historico = mesa.Historico
            };
            return Ok(jogada);
        }
    }
    public class Jogada
    {
        public string Historico { get; set; }
        public string Estado { get; set; }
        public int MesaId { get; set; }
    }
}