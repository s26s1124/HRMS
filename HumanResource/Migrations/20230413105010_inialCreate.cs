using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResource.Migrations
{
    /// <inheritdoc />
    public partial class inialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NavigationMenus",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PageName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    PagePath = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    ParentMenuId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IsActivated = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ControllerName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    ActionName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationMenus", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "StrCategoryCodes",
                columns: table => new
                {
                    CategoryCode = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CategoryName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CategoryDeactivated = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrCategoryCodes", x => x.CategoryCode);
                });

            migrationBuilder.CreateTable(
                name: "VempDtls",
                columns: table => new
                {
                    EMPNO = table.Column<int>(name: "EMP_NO", type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UNITCODE = table.Column<int>(name: "UNIT_CODE", type: "NUMBER(10)", nullable: true),
                    UNITDESPA = table.Column<string>(name: "UNIT_DESP_A", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    PUNITCODE = table.Column<int>(name: "P_UNIT_CODE", type: "NUMBER(10)", nullable: true),
                    PEMPNO = table.Column<int>(name: "P_EMP_NO", type: "NUMBER(10)", nullable: true),
                    APLYEAR = table.Column<int>(name: "APL_YEAR", type: "NUMBER(10)", nullable: true),
                    APLNO = table.Column<int>(name: "APL_NO", type: "NUMBER(10)", nullable: true),
                    EMPNAMEA = table.Column<string>(name: "EMP_NAME_A", type: "NVARCHAR2(110)", maxLength: 110, nullable: true),
                    EMPNAMEE = table.Column<string>(name: "EMP_NAME_E", type: "NVARCHAR2(109)", maxLength: 109, nullable: true),
                    DGCODE = table.Column<int>(name: "DG_CODE", type: "NUMBER(10)", nullable: true),
                    DGDESPA = table.Column<string>(name: "DG_DESP_A", type: "NVARCHAR2(60)", maxLength: 60, nullable: true),
                    DEPTCODE = table.Column<int>(name: "DEPT_CODE", type: "NUMBER(10)", nullable: true),
                    DEPTDESPA = table.Column<string>(name: "DEPT_DESP_A", type: "NVARCHAR2(70)", maxLength: 70, nullable: true),
                    SECTIONCODE = table.Column<int>(name: "SECTION_CODE", type: "NUMBER(10)", nullable: true),
                    DESGNCODE = table.Column<int>(name: "DESGN_CODE", type: "NUMBER(10)", nullable: true),
                    CATCODE = table.Column<int>(name: "CAT_CODE", type: "NUMBER(10)", nullable: true),
                    SUBCATCODE = table.Column<int>(name: "SUB_CAT_CODE", type: "NUMBER(10)", nullable: true),
                    GRADERANKCODE = table.Column<int>(name: "GRADE_RANK_CODE", type: "NUMBER(10)", nullable: true),
                    WORKPLACE = table.Column<string>(name: "WORK_PLACE", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    OMANPHONENO = table.Column<long>(name: "OMAN_PHONE_NO", type: "NUMBER(19)", nullable: true),
                    CURADRS1A = table.Column<string>(name: "CUR_ADRS1_A", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    CURADRS1E = table.Column<string>(name: "CUR_ADRS1_E", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    PERMADRS1A = table.Column<string>(name: "PERM_ADRS1_A", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    PERMADRS1E = table.Column<string>(name: "PERM_ADRS1_E", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    RESPHONENO = table.Column<long>(name: "RES_PHONE_NO", type: "NUMBER(19)", nullable: true),
                    SEXCODE = table.Column<string>(name: "SEX_CODE", type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    MARSTATCODE = table.Column<string>(name: "MAR_STAT_CODE", type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    COUNTRYCODE = table.Column<string>(name: "COUNTRY_CODE", type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    NATIONALITYCODE = table.Column<string>(name: "NATIONALITY_CODE", type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    APPOINTDATE = table.Column<DateTime>(name: "APPOINT_DATE", type: "TIMESTAMP(7)", nullable: true),
                    CEMPSTAT = table.Column<int>(name: "C_EMP_STAT", type: "NUMBER(10)", nullable: true),
                    STATONDATE = table.Column<DateTime>(name: "STAT_ON_DATE", type: "TIMESTAMP(7)", nullable: true),
                    DESGNTYPE = table.Column<int>(name: "DESGN_TYPE", type: "NUMBER(10)", nullable: true),
                    LASTPAIDDATE = table.Column<DateTime>(name: "LAST_PAID_DATE", type: "TIMESTAMP(7)", nullable: true),
                    PAYREVIEWDATE = table.Column<DateTime>(name: "PAY_REVIEW_DATE", type: "TIMESTAMP(7)", nullable: true),
                    PAYLOCNCODE = table.Column<int>(name: "PAY_LOCN_CODE", type: "NUMBER(10)", nullable: true),
                    PAYMODE = table.Column<int>(name: "PAY_MODE", type: "NUMBER(10)", nullable: true),
                    BANKCODE = table.Column<int>(name: "BANK_CODE", type: "NUMBER(10)", nullable: true),
                    BRANCHCODE = table.Column<int>(name: "BRANCH_CODE", type: "NUMBER(10)", nullable: true),
                    BANKACCNO = table.Column<long>(name: "BANK_ACC_NO", type: "NUMBER(19)", nullable: true),
                    DGDESPE = table.Column<string>(name: "DG_DESP_E", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DEPTDESPE = table.Column<string>(name: "DEPT_DESP_E", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    BIRTHDATE = table.Column<DateTime>(name: "BIRTH_DATE", type: "TIMESTAMP(7)", nullable: true),
                    EMPNAMEENG = table.Column<string>(name: "EMP_NAME_ENG", type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    GRADERANK = table.Column<string>(name: "GRADE_RANK", type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    DESGCODE = table.Column<string>(name: "DESG_CODE", type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    DESGENG = table.Column<string>(name: "DESG_ENG", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DESGTYPE = table.Column<int>(name: "DESG_TYPE", type: "NUMBER(10)", nullable: true),
                    EMPFIRSTNAME = table.Column<string>(name: "EMP_FIRST_NAME", type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    GRDDT = table.Column<DateTime>(name: "GRD_DT", type: "TIMESTAMP(7)", nullable: true),
                    EMPNAMEARA = table.Column<string>(name: "EMP_NAME_ARA", type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    MARDESC = table.Column<string>(name: "MAR_DESC", type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    NATIONALITYIDNO = table.Column<long>(name: "NATIONALITY_ID_NO", type: "NUMBER(19)", nullable: true),
                    BOSS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EMAILID = table.Column<string>(name: "EMAIL_ID", type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    SUBJDESPA = table.Column<string>(name: "SUBJ_DESP_A", type: "NVARCHAR2(60)", maxLength: 60, nullable: true),
                    QUALTODATE = table.Column<string>(name: "QUAL_TO_DATE", type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    IDEXPIRYDATE = table.Column<DateTime>(name: "ID_EXPIRY_DATE", type: "TIMESTAMP(7)", nullable: true),
                    PASSPORTNOE = table.Column<string>(name: "PASSPORT_NO_E", type: "NVARCHAR2(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VempDtls", x => x.EMPNO);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenuPermissions",
                columns: table => new
                {
                    EmpNo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PageId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsActivated = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ToDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenuPermissions", x => new { x.EmpNo, x.PageId });
                    table.ForeignKey(
                        name: "FK_RoleMenuPermissions_PageId",
                        column: x => x.PageId,
                        principalTable: "NavigationMenus",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrItemCodes",
                columns: table => new
                {
                    ItemCode = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ItemName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ItemDesc = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: false),
                    ItemBrand = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    ItemModel = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CategoryCode = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    UnitCode = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TotalQuaAva = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MaxQau = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MinQau = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NoToReq = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ItemDetDeactivated = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrItemCodes", x => x.ItemCode);
                    table.ForeignKey(
                        name: "FK_StrItemCodes_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "StrCategoryCodes",
                        principalColumn: "CategoryCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrReqMstrs",
                columns: table => new
                {
                    ReqNo = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ReqDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EmpNo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ManagrAppr = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ManagrApprFlag = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ManagrApprDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    StrManagrAppr = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    StrManagrApprDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DirectorApproval = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DirectorApprovalDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CancelFlage = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DeliveryAppr = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VempDtlEmpNo = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrReqMstrs", x => x.ReqNo);
                    table.ForeignKey(
                        name: "FK_StrReqMstrs_VempDtlEmpNo",
                        column: x => x.VempDtlEmpNo,
                        principalTable: "VempDtls",
                        principalColumn: "EMP_NO");
                });

            migrationBuilder.CreateTable(
                name: "StrRequestDetails",
                columns: table => new
                {
                    ReqDetailNo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ReqNo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CategoryCode = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DirectToPur = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ItemCode = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ItemQuantity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ItemRemark = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    ItemDelivery = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ItemDeliveryDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CancelFlage = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    StrManagerAppr = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    StrManagerApprReson = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    ItemQutReceived = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    StrApprDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IsItemRetrieve = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    QutRetrieve = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RetrieveReason = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrRequestDetails", x => new { x.ReqNo, x.ReqDetailNo });
                    table.ForeignKey(
                        name: "FK_StrDetails_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "StrCategoryCodes",
                        principalColumn: "CategoryCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrDetails_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "StrItemCodes",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrDetails_ReqNo",
                        column: x => x.ReqNo,
                        principalTable: "StrReqMstrs",
                        principalColumn: "ReqNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenu_PageId",
                table: "RoleMenuPermissions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_StrItem_CategoryCode",
                table: "StrItemCodes",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_StrReqMstrs_VempDtl",
                table: "StrReqMstrs",
                column: "VempDtlEmpNo");

            migrationBuilder.CreateIndex(
                name: "IX_StRequestDetails_Category",
                table: "StrRequestDetails",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_StrDetails_ItemCode",
                table: "StrRequestDetails",
                column: "ItemCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMenuPermissions");

            migrationBuilder.DropTable(
                name: "StrRequestDetails");

            migrationBuilder.DropTable(
                name: "NavigationMenus");

            migrationBuilder.DropTable(
                name: "StrItemCodes");

            migrationBuilder.DropTable(
                name: "StrReqMstrs");

            migrationBuilder.DropTable(
                name: "StrCategoryCodes");

            migrationBuilder.DropTable(
                name: "VempDtls");
        }
    }
}
