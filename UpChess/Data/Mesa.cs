using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Data
{
    public class Mesa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MesaId { get; set; }
        public string Nome { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime UltimoLance { get; set; }
        public string Historico { get; set; } // PGN
        public string Configuracao { get; set; } // white
        public string Estado { get; set; } // FEN
        public ICollection<MesaUsuario> MesasUsuarios { get; set; }
    }
    public class MesaUsuario {
        public int MesaId { get; set; }
        public Mesa Mesa { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
