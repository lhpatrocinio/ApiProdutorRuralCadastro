using Microsoft.EntityFrameworkCore;
using ProdutorRuralCadastro.Domain.Entities;

namespace ProdutorRuralCadastro.Infrastructure.DataBase.EntityFramework.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Cultura> Culturas { get; set; }
        public DbSet<Propriedade> Propriedades { get; set; }
        public DbSet<Talhao> Talhoes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cultura Configuration
            builder.Entity<Cultura>(entity =>
            {
                entity.ToTable("Culturas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.UmidadeIdealMin).HasColumnType("decimal(5,2)");
                entity.Property(e => e.UmidadeIdealMax).HasColumnType("decimal(5,2)");
                entity.Property(e => e.TempIdealMin).HasColumnType("decimal(5,2)");
                entity.Property(e => e.TempIdealMax).HasColumnType("decimal(5,2)");
            });

            // Propriedade Configuration
            builder.Entity<Propriedade>(entity =>
            {
                entity.ToTable("Propriedades");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Endereco).HasMaxLength(500);
                entity.Property(e => e.Cidade).HasMaxLength(100);
                entity.Property(e => e.Estado).HasMaxLength(2);
                entity.Property(e => e.CEP).HasMaxLength(10);
                entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");
                entity.Property(e => e.AreaTotalHa).HasColumnType("decimal(10,2)");
                
                entity.HasIndex(e => e.ProdutorId);
            });

            // Talhao Configuration
            builder.Entity<Talhao>(entity =>
            {
                entity.ToTable("Talhoes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Codigo).HasMaxLength(20);
                entity.Property(e => e.AreaHa).HasColumnType("decimal(10,2)");
                entity.Property(e => e.StatusDescricao).HasMaxLength(200);
                entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");

                entity.HasIndex(e => e.PropriedadeId);
                entity.HasIndex(e => e.CulturaId);
                entity.HasIndex(e => e.Status);

                entity.HasOne(e => e.Propriedade)
                    .WithMany(p => p.Talhoes)
                    .HasForeignKey(e => e.PropriedadeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Cultura)
                    .WithMany(c => c.Talhoes)
                    .HasForeignKey(e => e.CulturaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
