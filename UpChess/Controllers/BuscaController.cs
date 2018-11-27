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
    public class BuscaController : ControllerBase
    {
        IJogoService _jogoService;
        public BuscaController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        [HttpPost]
        public ActionResult<EntrarMesaResp> Entrar(EntrarMesaReq req)
        {
            int? usuarioId =
                HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                return Unauthorized();
            }
            EntrarMesaResp resp = new EntrarMesaResp
            {
                Status = 0,
                Mensagem = "",
                Url = "/Mesas/MinhasMesas"
            };
            try
            {
                _jogoService.EntrarMesa(usuarioId ?? 0, req.MesaId);
            }
            catch (JaEstaNaMesaException e)
            {
                resp.Status = 1;
                resp.Mensagem = "Usuário já está na mesa";
            }
            catch (MesaCheiaException e)
            {
                resp.Status = 2;
                resp.Mensagem = "A mesa está cheia";
            }
            return resp;
        }
    }
    public class EntrarMesaReq
    {
        public int MesaId { get; set; }
    }
    public class EntrarMesaResp
    {
        public string Mensagem { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
    }
}