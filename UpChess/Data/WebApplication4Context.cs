using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;

namespace WebApplication4.Models
{
    public class WebApplication4Context : DbContext
    {
        public WebApplication4Context (DbContextOptions<WebApplication4Context> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<MesaUsuario> MesasUsuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MesaUsuario>()
                .HasKey(um => new { um.MesaId, um.UsuarioId });

            modelBuilder.Entity<MesaUsuario>()
                .HasOne(um => um.Mesa)
                .WithMany(m => m.MesasUsuarios)
                .HasForeignKey(um => um.MesaId);

            modelBuilder.Entity<MesaUsuario>()
                .HasOne(um => um.Usuario)
                .WithMany(u => u.MesasUsuarios)
                .HasForeignKey(um => um.UsuarioId);
        }

    }
}
