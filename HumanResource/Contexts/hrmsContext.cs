using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using HumanResource.Models.HRMS;
using HumanResource.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;

namespace HumanResource.Contexts;

public partial class hrmsContext : DbContext
{
    public hrmsContext() {    }

    public hrmsContext(DbContextOptions<hrmsContext> options) : base(options) { }

    public virtual DbSet<NavigationMenu> NavigationMenus { get; set; }
    public virtual DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
    public virtual DbSet<StrCategoryCode> StrCategoryCodes { get; set; }
    public virtual DbSet<StrItemCode> StrItemCodes { get; set; }
    public virtual DbSet<StrReqMstr> StrReqMstrs { get; set; }
    public virtual DbSet<StrRequestDetails> StrRequestDetails { get; set; }
    public virtual DbSet<VempDtl> VempDtls { get; set; }
    public virtual DbSet<MangerProc> GetMangers  { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = orcl)));User Id=HR;Password=hrmsnew", m => m.UseOracleSQLCompatibility("11"));

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.HasDefaultSchema("HR");

    //    modelBuilder.Entity<NavigationMenu>(entity =>
    //    {
    //        entity.HasKey(e => e.PageId).HasName("NavigationMenu_PK");

    //    });

    //    modelBuilder.Entity<RoleMenuPermission>(entity =>
    //    {
    //        entity.HasKey(e => new { e.EmpNo, e.PageId }).HasName("RoleMenuPermission_PK");
    //        entity.HasOne(d => d.Page).WithMany(p => p.RoleMenuPermissions).HasConstraintName("ROLE_PERMISSION_FK2");
    //    });

    //    modelBuilder.Entity<StrCategoryCode>(entity =>
    //    {
    //        entity.HasKey(e => e.CategoryCode).HasName("STR_CAT_CODE_PK");
    //        entity.Property(e => e.CategoryDeactivated).HasDefaultValueSql("0");

    //    });

    //    modelBuilder.Entity<StrItemCode>(entity =>
    //    {
    //        entity.HasKey(e => e.ItemCode).HasName("STR_Item_Code_PK");
    //        entity.HasOne(d => d.StrCategory).WithMany(p => p.StrItemCodes).HasConstraintName("ROLE_STR_Code_FK1");
    //        entity.Property(e => e.ItemDetDeactivated).ValueGeneratedOnAdd().HasDefaultValueSql("0");


    //    });

    //    modelBuilder.Entity<StrReqMstr>(entity =>
    //    {
    //        entity.HasKey(e => e.ReqNo).HasName("Str_Req_Mstr_PK");
    //        //entity.HasOne(e => e.VempDtl).WithMany(e => e.StrReqMstrs).HasConstraintName("Request_Mstr_FK1");

    //        entity.Property(e => e.CancelFlage).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.StrManagrAppr).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.ManagrAppr).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.DeliveryAppr).ValueGeneratedOnAdd().HasDefaultValueSql("0");


    //    });

    //    modelBuilder.Entity<StrRequestDetails>(entity =>
    //    {
    //        entity.HasKey(e => e.ReqDetailNo).HasName("Str_Request_Details_PK");
    //        entity.HasOne(d => d.StrCategory).WithMany(p => p.StrRequestDetails).HasConstraintName("Request_Details_FK1");
    //        entity.HasOne(d => d.StrItem).WithMany(p => p.StrRequestDetails).HasConstraintName("Request_Details_FK3");
    //        entity.HasOne(d => d.StrReqMstr).WithMany(p => p.StrRequestDetails).HasConstraintName("Request_Details_FK4");

    //        entity.Property(e => e.CancelFlage).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.ItemDelivery).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.StrManagerAppr).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.IsItemRetrieve).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.QutRetrieve).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //        entity.Property(e => e.DirectToPur).ValueGeneratedOnAdd().HasDefaultValueSql("0");
    //     });


    //    modelBuilder.Entity<VempDtl>(entity =>
    //    {
    //        entity.HasKey(e => e.EmpNo).HasName("VEMP_DTLS_PK");
    //        entity.Property(g => g.EmailId).IsRequired();


    //    });


    //    OnModelCreatingPartial(modelBuilder);
    //}

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder); 
}
