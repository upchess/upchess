using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Business;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinhasMesasController : ControllerBase
    {
        IJogoService _jogoService;
        public MinhasMesasController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        [HttpPost]
        public ActionResult Criar(CriarMesaReq req)
        {
            int? usuarioId =
               HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                return Unauthorized();
            }
            if (req.Nome == null || req.Nome.Length == 0)
            {
                return BadRequest("Nome deve ser preenchido");
            }
            _jogoService.MontarMesa(req.Nome, usuarioId ?? 0);
            return Ok(new { resp = "Ok" });
        }
        [HttpPost("sair")]
        public ActionResult Sair([FromForm]int mesaId)
        {
            int? usuarioId =
               HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                return Unauthorized();
            }
            if (mesaId == 0)
            {
                return BadRequest("Id da Mesa é inválido");
            }
            try
            {
                _jogoService.SairMesa(usuarioId ?? 0, mesaId);
            }
            catch( UsuarioNaoEstaNaMesa e)
            {
                return BadRequest("Usuário não está na mesa");
            }
            return Ok(new { resp = "Ok" });
        }
        [HttpPost("excluir")]
        public ActionResult Excluir([FromForm]int mesaId)
        {
            int? usuarioId =
               HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                return Unauthorized();
            }
            if (mesaId == 0)
            {
                return BadRequest("Id da Mesa é inválido");
            }
            try
            {
                _jogoService.ExcluirMesa(mesaId);
            }
            catch (MesaInexistente e)
            {
                return BadRequest("Mesa inexistente");
            }
            return Ok(new { resp = "Ok" });
        }
    }
    public class CriarMesaReq
    {
        public string Nome { get; set; }
    }
}