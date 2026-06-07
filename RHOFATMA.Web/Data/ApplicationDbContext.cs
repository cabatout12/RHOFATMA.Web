using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAbsentRetard> TblAbsentRetards { get; set; }

    public virtual DbSet<TblAuditGenerale> TblAuditGenerales { get; set; }

    public virtual DbSet<TblBureau> TblBureaus { get; set; }

    public virtual DbSet<TblConge> TblConges { get; set; }

    public virtual DbSet<TblContrat> TblContrats { get; set; }

    public virtual DbSet<TblDeduction> TblDeductions { get; set; }

    public virtual DbSet<TblDirection> TblDirections { get; set; }

    public virtual DbSet<TblDocumentEmploye> TblDocumentEmployes { get; set; }

    public virtual DbSet<TblEmploye> TblEmployes { get; set; }

    public virtual DbSet<TblFormation> TblFormations { get; set; }

    public virtual DbSet<TblPaie> TblPaies { get; set; }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblPoste> TblPostes { get; set; }

    public virtual DbSet<TblPresence> TblPresences { get; set; }

    public virtual DbSet<TblPrim> TblPrims { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; }

    public virtual DbSet<TblSection> TblSections { get; set; }

    public virtual DbSet<TblService> TblServices { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=RHO-FATMA;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAbsentRetard>(entity =>
        {
            entity.HasKey(e => e.IdAbsentRetard).HasName("PK__tbl_Abse__8562F44AAA9B8B8E");

            entity.ToTable("tbl_AbsentRetard");

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.ModifierPar).HasMaxLength(100);
            entity.Property(e => e.Motif).HasMaxLength(255);
            entity.Property(e => e.Statut).HasMaxLength(50);

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblAbsentRetards)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AbsentRetard_Employe");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblAbsentRetards)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_AbsentRetard_User");
        });

        modelBuilder.Entity<TblAuditGenerale>(entity =>
        {
            entity.HasKey(e => e.IdAuditGenerale).HasName("PK__tbl_Audi__E302005972897805");

            entity.ToTable("tbl_AuditGenerale");

            entity.Property(e => e.ActionType).HasMaxLength(10);
            entity.Property(e => e.ChampModifier).HasMaxLength(100);
            entity.Property(e => e.DateAction).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NomTable).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.TblAuditGenerales)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Audit_User");
        });

        modelBuilder.Entity<TblBureau>(entity =>
        {
            entity.HasKey(e => e.IdBureau).HasName("PK__tbl_Bure__6C6E8426D65BA1E5");

            entity.ToTable("tbl_Bureau");

            entity.Property(e => e.Adresse).HasMaxLength(255);
            entity.Property(e => e.NomBureau).HasMaxLength(100);
        });

        modelBuilder.Entity<TblConge>(entity =>
        {
            entity.HasKey(e => e.IdConge).HasName("PK__tbl_Cong__D59837B24CF839C1");

            entity.ToTable("tbl_Conge");

            entity.HasIndex(e => e.IdEmploye, "IX_Conge_Employe");

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.ApprouvePar).HasMaxLength(100);
            entity.Property(e => e.DateDemande).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.ModifierPar).HasMaxLength(100);
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .HasDefaultValue("En attente");
            entity.Property(e => e.TypeConge).HasMaxLength(50);

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblConges)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conge_Employe");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblConges)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Conge_User");
        });

        modelBuilder.Entity<TblContrat>(entity =>
        {
            entity.HasKey(e => e.IdContrat).HasName("PK__tbl_Cont__253DE8F104B0B88D");

            entity.ToTable("tbl_Contrat");

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.ModifierPar).HasMaxLength(100);
            entity.Property(e => e.SalaireBase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Statut)
                .HasMaxLength(30)
                .HasDefaultValue("Actif");
            entity.Property(e => e.TypeContrat).HasMaxLength(50);

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblContrats)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contrat_Employe");
        });

        modelBuilder.Entity<TblDeduction>(entity =>
        {
            entity.HasKey(e => e.IdDeduction).HasName("PK__tbl_Dedu__DC3D189D50F36231");

            entity.ToTable("tbl_Deduction");

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MontantDeduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Motif).HasMaxLength(255);
            entity.Property(e => e.TypeDeduction).HasMaxLength(100);

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblDeductions)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Deduction_Employe");
        });

        modelBuilder.Entity<TblDirection>(entity =>
        {
            entity.HasKey(e => e.IdDirection).HasName("PK__tbl_Dire__7780E2B2D39A138C");

            entity.ToTable("tbl_Direction");

            entity.Property(e => e.NomDirection).HasMaxLength(100);
        });

        modelBuilder.Entity<TblDocumentEmploye>(entity =>
        {
            entity.HasKey(e => e.IdDocumentEmploye).HasName("PK__tbl_Docu__EA2C2D0EE1C8024E");

            entity.ToTable("tbl_DocumentEmploye");

            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.CheminFichier).HasMaxLength(500);
            entity.Property(e => e.DateAjout).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NomFichier).HasMaxLength(255);
            entity.Property(e => e.TypeDocument).HasMaxLength(100);

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblDocumentEmployes)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentEmploye_Employe");
        });

        modelBuilder.Entity<TblEmploye>(entity =>
        {
            entity.HasKey(e => e.IdEmploye).HasName("PK__tbl_Empl__2ED32064002362C3");

            entity.ToTable("tbl_Employe");

            entity.HasIndex(e => e.IdDirection, "IX_Employe_Direction");

            entity.HasIndex(e => new { e.NomEmploye, e.PrenomEmploye }, "IX_Employe_NomPrenom");

            entity.HasIndex(e => e.IdService, "IX_Employe_Service");

            entity.HasIndex(e => e.CinNif, "UQ__tbl_Empl__0576B513561BB594").IsUnique();

            entity.HasIndex(e => e.CodeEmploye, "UQ__tbl_Empl__9D98C57120DA0EC1").IsUnique();

            entity.Property(e => e.Adresse).HasMaxLength(255);
            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.CinNif)
                .HasMaxLength(50)
                .HasColumnName("CIN_NIF");
            entity.Property(e => e.CodeEmploye).HasMaxLength(50);
            entity.Property(e => e.Diplome).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LieuNaissance).HasMaxLength(150);
            entity.Property(e => e.ModifierPar).HasMaxLength(100);
            entity.Property(e => e.NomEmploye).HasMaxLength(100);
            entity.Property(e => e.PrenomEmploye).HasMaxLength(100);
            entity.Property(e => e.Salaire).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sexe)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Statut)
                .HasMaxLength(30)
                .HasDefaultValue("Actif");
            entity.Property(e => e.Telephone).HasMaxLength(20);

            entity.HasOne(d => d.IdBureauNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdBureau)
                .HasConstraintName("FK_Employe_Bureau");

            entity.HasOne(d => d.IdDirectionNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdDirection)
                .HasConstraintName("FK_Employe_Direction");

            entity.HasOne(d => d.IdFormationNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdFormation)
                .HasConstraintName("FK_Employe_Formation");

            entity.HasOne(d => d.IdPosteNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdPoste)
                .HasConstraintName("FK_Employe_Poste");

            entity.HasOne(d => d.IdSectionNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdSection)
                .HasConstraintName("FK_Employe_Section");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("FK_Employe_Service");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblEmployes)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Employe_User");
        });

        modelBuilder.Entity<TblFormation>(entity =>
        {
            entity.HasKey(e => e.IdFormation).HasName("PK__tbl_Form__FEF2C23B8F779845");

            entity.ToTable("tbl_Formation");

            entity.Property(e => e.NomFormation).HasMaxLength(100);
        });

        modelBuilder.Entity<TblPaie>(entity =>
        {
            entity.HasKey(e => e.IdPaie).HasName("PK__tbl_Paie__FC850A65E96D12B9");

            entity.ToTable("tbl_Paie");

            entity.HasIndex(e => e.IdEmploye, "IX_Paie_Employe");

            entity.HasIndex(e => new { e.IdEmploye, e.Mois, e.Annee }, "UQ_Paie_Employe_Mois_Annee").IsUnique();

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.SalaireBase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SalaireNet)
                .HasComputedColumnSql("(([SalaireBase]+[TotalPrimes])-[TotalDeductions])", true)
                .HasColumnType("decimal(20, 2)");
            entity.Property(e => e.Statut)
                .HasMaxLength(30)
                .HasDefaultValue("En attente");
            entity.Property(e => e.TotalDeductions).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrimes).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblPaies)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Paie_Employe");
        });

        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.HasKey(e => e.IdPermission).HasName("PK__tbl_Perm__17C26EA2E600CDC0");

            entity.ToTable("tbl_Permission");

            entity.HasIndex(e => e.CodePermission, "UQ__tbl_Perm__4BA93E27612018F8").IsUnique();

            entity.Property(e => e.CodePermission).HasMaxLength(100);
            entity.Property(e => e.DescriptionPermission).HasMaxLength(255);
        });

        modelBuilder.Entity<TblPoste>(entity =>
        {
            entity.HasKey(e => e.IdPoste).HasName("PK__tbl_Post__C7658475E13DDBCC");

            entity.ToTable("tbl_Poste");

            entity.Property(e => e.DescPoste).HasMaxLength(400);
            entity.Property(e => e.NomPoste).HasMaxLength(100);
        });

        modelBuilder.Entity<TblPresence>(entity =>
        {
            entity.HasKey(e => e.IdPresence).HasName("PK__tbl_Pres__50FB6F5992A3E957");

            entity.ToTable("tbl_Presence");

            entity.HasIndex(e => new { e.IdEmploye, e.DatePresence }, "IX_Presence_Employe_Date");

            entity.HasIndex(e => new { e.IdEmploye, e.DatePresence }, "UQ_Presence_Employe_Date").IsUnique();

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.Remarque).HasMaxLength(255);
            entity.Property(e => e.Statut)
                .HasMaxLength(30)
                .HasDefaultValue("Present");

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblPresences)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Presence_Employe");
        });

        modelBuilder.Entity<TblPrim>(entity =>
        {
            entity.HasKey(e => e.IdPrim).HasName("PK__tbl_Prim__E40DE6D322214886");

            entity.ToTable("tbl_Prim");

            entity.Property(e => e.AjouterLe).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AjouterPar).HasMaxLength(100);
            entity.Property(e => e.ModifierPar).HasMaxLength(100);
            entity.Property(e => e.MontantPrime).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TypePrim).HasMaxLength(100);

            entity.HasOne(d => d.IdEmployeNavigation).WithMany(p => p.TblPrims)
                .HasForeignKey(d => d.IdEmploye)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prim_Employe");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblPrims)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Prim_User");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__tbl_Role__B4369054AA29391A");

            entity.ToTable("tbl_Role");

            entity.HasIndex(e => e.NomRole, "UQ__tbl_Role__ADB14FA667BF6AA3").IsUnique();

            entity.Property(e => e.DescriptionRole).HasMaxLength(255);
            entity.Property(e => e.NomRole).HasMaxLength(100);
        });

        modelBuilder.Entity<TblRolePermission>(entity =>
        {
            entity.HasKey(e => e.IdRolePermission).HasName("PK__tbl_Role__BF07A893B43C8A7C");

            entity.ToTable("tbl_RolePermission");

            entity.HasIndex(e => new { e.IdRole, e.IdPermission }, "UQ_RolePermission").IsUnique();

            entity.HasOne(d => d.IdPermissionNavigation).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.IdPermission)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.TblRolePermissions)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<TblSection>(entity =>
        {
            entity.HasKey(e => e.IdSection).HasName("PK__tbl_Sect__ED9583571E3D1C2A");

            entity.ToTable("tbl_Section");

            entity.Property(e => e.NomSection).HasMaxLength(100);
        });

        modelBuilder.Entity<TblService>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__tbl_Serv__474DDE0054E45153");

            entity.ToTable("tbl_Service");

            entity.Property(e => e.NomService).HasMaxLength(100);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__tbl_User__B7C92638856F9985");

            entity.ToTable("tbl_User");

            entity.HasIndex(e => e.UserEmail, "UQ__tbl_User__08638DF8AAFFF584").IsUnique();

            entity.Property(e => e.AjoutePar).HasMaxLength(100);
            entity.Property(e => e.DateCreation).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ModifierPar).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .HasDefaultValue("Actif");
            entity.Property(e => e.UserEmail).HasMaxLength(100);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
