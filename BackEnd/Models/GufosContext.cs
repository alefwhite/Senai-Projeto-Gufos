using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BackEnd.Models
{
    public partial class GufosContext : DbContext
    {
        public GufosContext()
        {
        }

        public GufosContext(DbContextOptions<GufosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Localizacao> Localizacao { get; set; }
        public virtual DbSet<Presenca> Presenca { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-LKMH5JG\\SQLEXPRESS; Database=Gufos; User Id=sa; Password=132");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasIndex(e => e.Titulo)
                    .HasName("UQ__Categori__7B406B56621F81F8")
                    .IsUnique();

                entity.Property(e => e.Titulo).IsUnicode(false);
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasIndex(e => e.Titulo)
                    .HasName("UQ__Evento__7B406B5661D0F4E5")
                    .IsUnique();

                entity.Property(e => e.AcessoLivre).HasDefaultValueSql("((1))");

                entity.Property(e => e.Titulo).IsUnicode(false);

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK__Evento__Categori__46E78A0C");

                entity.HasOne(d => d.Localizacao)
                    .WithMany(p => p.Evento)
                    .HasForeignKey(d => d.LocalizacaoId)
                    .HasConstraintName("FK__Evento__Localiza__48CFD27E");
            });

            modelBuilder.Entity<Localizacao>(entity =>
            {
                entity.HasIndex(e => e.Cnpj)
                    .HasName("UQ__Localiza__AA57D6B4ED548C70")
                    .IsUnique();

                entity.HasIndex(e => e.RazaoSocial)
                    .HasName("UQ__Localiza__7DD02876A5F469C6")
                    .IsUnique();

                entity.Property(e => e.Cnpj)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Endereco).IsUnicode(false);

                entity.Property(e => e.RazaoSocial).IsUnicode(false);
            });

            modelBuilder.Entity<Presenca>(entity =>
            {
                entity.HasIndex(e => e.PresencaStatus)
                    .HasName("UQ__Presenca__22F4AC3819BC99AB")
                    .IsUnique();

                entity.Property(e => e.PresencaStatus).IsUnicode(false);

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.Presenca)
                    .HasForeignKey(d => d.EventoId)
                    .HasConstraintName("FK__Presenca__Evento__4CA06362");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Presenca)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK__Presenca__Usuari__4D94879B");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasIndex(e => e.Titulo)
                    .HasName("UQ__Tipo_usu__7B406B5687FB701D")
                    .IsUnique();

                entity.Property(e => e.Titulo).IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Usuario__A9D10534B20AA668")
                    .IsUnique();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.Property(e => e.Senha).IsUnicode(false);

                entity.HasOne(d => d.TipoUsuario)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.TipoUsuarioId)
                    .HasConstraintName("FK__Usuario__Tipo_us__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
