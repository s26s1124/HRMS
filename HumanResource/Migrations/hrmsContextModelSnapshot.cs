﻿// <auto-generated />
using System;
using HumanResource.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace HumanResource.Migrations
{
    [DbContext(typeof(hrmsContext))]
    partial class hrmsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HumanResource.Models.HRMS.NavigationMenu", b =>
                {
                    b.Property<int>("PageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PageId"));

                    b.Property<string>("ActionName")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("ControllerName")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<int?>("IsActivated")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("PageName")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("PagePath")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<int?>("ParentMenuId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("PageId");

                    b.ToTable("NavigationMenus");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.RoleMenuPermission", b =>
                {
                    b.Property<int>("EmpNo")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("PageId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int?>("IsActivated")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("EmpNo", "PageId");

                    b.HasIndex("PageId");

                    b.ToTable("RoleMenuPermissions");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrCategoryCode", b =>
                {
                    b.Property<int>("CategoryCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryCode"));

                    b.Property<int?>("CategoryDeactivated")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)");

                    b.HasKey("CategoryCode");

                    b.ToTable("StrCategoryCodes");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrItemCode", b =>
                {
                    b.Property<int>("ItemCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemCode"));

                    b.Property<int>("CategoryCode")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("ItemBrand")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("ItemDesc")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("NVARCHAR2(300)");

                    b.Property<int?>("ItemDetDeactivated")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("ItemModel")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<int?>("MaxQau")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("MinQau")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("NoToReq")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("TotalQuaAva")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("ItemCode");

                    b.HasIndex("CategoryCode");

                    b.ToTable("StrItemCodes");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrReqMstr", b =>
                {
                    b.Property<int>("ReqNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReqNo"));

                    b.Property<int?>("CancelFlage")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("DeliveryAppr")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("DirectorApproval")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("DirectorApprovalDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("EmpNo")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("ManagrAppr")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("ManagrApprDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int?>("ManagrApprFlag")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("ReqDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int?>("StrManagrAppr")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("StrManagrApprDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int?>("StrManagrApprFlag")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("UserId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("VempDtlEmpNo")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("ReqNo");

                    b.HasIndex("VempDtlEmpNo");

                    b.ToTable("StrReqMstrs");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrRequestDetails", b =>
                {
                    b.Property<int>("ReqNo")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("ReqDetailNo")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("CancelFlage")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("CategoryCode")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("DirectToPur")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("IsItemRetrieve")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("ItemCode")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("ItemDelivery")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("ItemDeliveryDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int?>("ItemQuantity")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("ItemQutReceived")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("ItemRemark")
                        .HasMaxLength(200)
                        .HasColumnType("NVARCHAR2(200)");

                    b.Property<int?>("QutRetrieve")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("RetrieveReason")
                        .HasMaxLength(200)
                        .HasColumnType("NVARCHAR2(200)");

                    b.Property<DateTime?>("StrApprDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int?>("StrManagerAppr")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("StrManagerApprReson")
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR2(150)");

                    b.HasKey("ReqNo", "ReqDetailNo");

                    b.HasIndex("CategoryCode");

                    b.HasIndex("ItemCode");

                    b.ToTable("StrRequestDetails");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.VempDtl", b =>
                {
                    b.Property<int>("EmpNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("EMP_NO");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmpNo"));

                    b.Property<int?>("AplNo")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("APL_NO");

                    b.Property<int?>("AplYear")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("APL_YEAR");

                    b.Property<DateTime?>("AppointDate")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("APPOINT_DATE");

                    b.Property<long?>("BankAccNo")
                        .HasColumnType("NUMBER(19)")
                        .HasColumnName("BANK_ACC_NO");

                    b.Property<int?>("BankCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("BANK_CODE");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("BIRTH_DATE");

                    b.Property<int?>("Boss")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("BOSS");

                    b.Property<int?>("BranchCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("BRANCH_CODE");

                    b.Property<int?>("CEmpStat")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("C_EMP_STAT");

                    b.Property<int?>("CatCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("CAT_CODE");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)")
                        .HasColumnName("COUNTRY_CODE");

                    b.Property<string>("CurAdrs1A")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("CUR_ADRS1_A");

                    b.Property<string>("CurAdrs1E")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("CUR_ADRS1_E");

                    b.Property<int?>("DeptCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("DEPT_CODE");

                    b.Property<string>("DeptDespA")
                        .HasMaxLength(70)
                        .HasColumnType("NVARCHAR2(70)")
                        .HasColumnName("DEPT_DESP_A");

                    b.Property<string>("DeptDespE")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("DEPT_DESP_E");

                    b.Property<string>("DesgCode")
                        .HasMaxLength(120)
                        .HasColumnType("NVARCHAR2(120)")
                        .HasColumnName("DESG_CODE");

                    b.Property<string>("DesgEng")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("DESG_ENG");

                    b.Property<int?>("DesgType")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("DESG_TYPE");

                    b.Property<int?>("DesgnCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("DESGN_CODE");

                    b.Property<int?>("DesgnType")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("DESGN_TYPE");

                    b.Property<int?>("DgCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("DG_CODE");

                    b.Property<string>("DgDespA")
                        .HasMaxLength(60)
                        .HasColumnType("NVARCHAR2(60)")
                        .HasColumnName("DG_DESP_A");

                    b.Property<string>("DgDespE")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("DG_DESP_E");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)")
                        .HasColumnName("EMAIL_ID");

                    b.Property<string>("EmpFirstName")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("EMP_FIRST_NAME");

                    b.Property<string>("EmpNameA")
                        .HasMaxLength(110)
                        .HasColumnType("NVARCHAR2(110)")
                        .HasColumnName("EMP_NAME_A");

                    b.Property<string>("EmpNameAra")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)")
                        .HasColumnName("EMP_NAME_ARA");

                    b.Property<string>("EmpNameE")
                        .HasMaxLength(109)
                        .HasColumnType("NVARCHAR2(109)")
                        .HasColumnName("EMP_NAME_E");

                    b.Property<string>("EmpNameEng")
                        .HasMaxLength(120)
                        .HasColumnType("NVARCHAR2(120)")
                        .HasColumnName("EMP_NAME_ENG");

                    b.Property<string>("GradeRank")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)")
                        .HasColumnName("GRADE_RANK");

                    b.Property<int?>("GradeRankCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("GRADE_RANK_CODE");

                    b.Property<DateTime?>("GrdDt")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("GRD_DT");

                    b.Property<DateTime?>("IdExpiryDate")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("ID_EXPIRY_DATE");

                    b.Property<DateTime?>("LastPaidDate")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("LAST_PAID_DATE");

                    b.Property<string>("MarDesc")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)")
                        .HasColumnName("MAR_DESC");

                    b.Property<string>("MarStatCode")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)")
                        .HasColumnName("MAR_STAT_CODE");

                    b.Property<string>("NationalityCode")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)")
                        .HasColumnName("NATIONALITY_CODE");

                    b.Property<long?>("NationalityIdNo")
                        .HasColumnType("NUMBER(19)")
                        .HasColumnName("NATIONALITY_ID_NO");

                    b.Property<long?>("OmanPhoneNo")
                        .HasColumnType("NUMBER(19)")
                        .HasColumnName("OMAN_PHONE_NO");

                    b.Property<int?>("PEmpNo")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("P_EMP_NO");

                    b.Property<int?>("PUnitCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("P_UNIT_CODE");

                    b.Property<string>("PassportNoE")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)")
                        .HasColumnName("PASSPORT_NO_E");

                    b.Property<int?>("PayLocnCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("PAY_LOCN_CODE");

                    b.Property<int?>("PayMode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("PAY_MODE");

                    b.Property<DateTime?>("PayReviewDate")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("PAY_REVIEW_DATE");

                    b.Property<string>("PermAdrs1A")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("PERM_ADRS1_A");

                    b.Property<string>("PermAdrs1E")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("PERM_ADRS1_E");

                    b.Property<string>("QualToDate")
                        .HasMaxLength(120)
                        .HasColumnType("NVARCHAR2(120)")
                        .HasColumnName("QUAL_TO_DATE");

                    b.Property<long?>("ResPhoneNo")
                        .HasColumnType("NUMBER(19)")
                        .HasColumnName("RES_PHONE_NO");

                    b.Property<int?>("SectionCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("SECTION_CODE");

                    b.Property<string>("SexCode")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)")
                        .HasColumnName("SEX_CODE");

                    b.Property<DateTime?>("StatOnDate")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("STAT_ON_DATE");

                    b.Property<int?>("SubCatCode")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("SUB_CAT_CODE");

                    b.Property<string>("SubjDespA")
                        .HasMaxLength(60)
                        .HasColumnType("NVARCHAR2(60)")
                        .HasColumnName("SUBJ_DESP_A");

                    b.Property<int?>("UNIT_CODE")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("UnitDespA")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("UNIT_DESP_A");

                    b.Property<string>("WorkPlace")
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR2(50)")
                        .HasColumnName("WORK_PLACE");

                    b.HasKey("EmpNo");

                    b.ToTable("VempDtls");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.RoleMenuPermission", b =>
                {
                    b.HasOne("HumanResource.Models.HRMS.NavigationMenu", "Page")
                        .WithMany("RoleMenuPermissions")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrItemCode", b =>
                {
                    b.HasOne("HumanResource.Models.HRMS.StrCategoryCode", "StrCategory")
                        .WithMany("StrItemCodes")
                        .HasForeignKey("CategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StrCategory");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrReqMstr", b =>
                {
                    b.HasOne("HumanResource.Models.HRMS.VempDtl", null)
                        .WithMany("StrReqMstrs")
                        .HasForeignKey("VempDtlEmpNo");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrRequestDetails", b =>
                {
                    b.HasOne("HumanResource.Models.HRMS.StrCategoryCode", "StrCategory")
                        .WithMany("StrRequestDetails")
                        .HasForeignKey("CategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HumanResource.Models.HRMS.StrItemCode", "StrItem")
                        .WithMany()
                        .HasForeignKey("ItemCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HumanResource.Models.HRMS.StrReqMstr", "StrReqMstr")
                        .WithMany("StrRequestDetails")
                        .HasForeignKey("ReqNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StrCategory");

                    b.Navigation("StrItem");

                    b.Navigation("StrReqMstr");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.NavigationMenu", b =>
                {
                    b.Navigation("RoleMenuPermissions");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrCategoryCode", b =>
                {
                    b.Navigation("StrItemCodes");

                    b.Navigation("StrRequestDetails");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.StrReqMstr", b =>
                {
                    b.Navigation("StrRequestDetails");
                });

            modelBuilder.Entity("HumanResource.Models.HRMS.VempDtl", b =>
                {
                    b.Navigation("StrReqMstrs");
                });
#pragma warning restore 612, 618
        }
    }
}
