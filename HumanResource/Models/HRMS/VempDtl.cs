using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Models.HRMS;


public partial class VempDtl
{
    [Key]
    [Column("EMP_NO")]
    public int EmpNo { get; set; }

    public int? UNIT_CODE { get; set; }

    [Column("UNIT_DESP_A")]
    [StringLength(50)]
    public string? UnitDespA { get; set; }


    [Column("P_UNIT_CODE")]
    public int? PUnitCode { get; set; }

    [Column("P_EMP_NO")]
    public int? PEmpNo { get; set; }

    [Column("APL_YEAR")]
    public int? AplYear { get; set; }

    [Column("APL_NO")]
    public int? AplNo { get; set; }

    [Column("EMP_NAME_A")]
    [StringLength(110)]
    public string? EmpNameA { get; set; }

    [Column("EMP_NAME_E")]
    [StringLength(109)]
    public string? EmpNameE { get; set; }

    [Column("DG_CODE")]
    public int? DgCode { get; set; }

    [Column("DG_DESP_A")]
    [StringLength(60)] 
    public string? DgDespA { get; set; }

    [Column("DEPT_CODE")]
    public int? DeptCode { get; set; }

    [Column("DEPT_DESP_A")]
    [StringLength(70)]
    public string? DeptDespA { get; set; }

    [Column("SECTION_CODE")]
    public int? SectionCode { get; set; }

    [Column("DESGN_CODE")]
    public int? DesgnCode { get; set; }

    [Column("CAT_CODE")]
    public int? CatCode { get; set; }

    [Column("SUB_CAT_CODE")]
    public int? SubCatCode { get; set; }

    [Column("GRADE_RANK_CODE")]
    public int? GradeRankCode { get; set; }

    [Column("WORK_PLACE")]
    [StringLength(50)]
    public string? WorkPlace { get; set; }

    [Column("OMAN_PHONE_NO")]
    public long? OmanPhoneNo { get; set; }

    [Column("CUR_ADRS1_A")]
    [StringLength(50)]
    public string? CurAdrs1A { get; set; }

    [Column("CUR_ADRS1_E")]
    [StringLength(50)]
    public string? CurAdrs1E { get; set; }

    [Column("PERM_ADRS1_A")]
    [StringLength(50)]
    public string? PermAdrs1A { get; set; }

    [Column("PERM_ADRS1_E")]
    [StringLength(50)]
    public string? PermAdrs1E { get; set; }

    [Column("RES_PHONE_NO")]
    public long? ResPhoneNo { get; set; }

    [Column("SEX_CODE")]
    [StringLength(20)]
    public string? SexCode { get; set; }

    [Column("MAR_STAT_CODE")]
    [StringLength(20)]
    public string? MarStatCode { get; set; }

    [Column("COUNTRY_CODE")]
    [StringLength(20)] 
    public string? CountryCode { get; set; }

    [Column("NATIONALITY_CODE")]
    [StringLength(20)] 
    public string? NationalityCode { get; set; }

    [Column("APPOINT_DATE")]
    public DateTime? AppointDate { get; set; }

    [Column("C_EMP_STAT")]
    public int? CEmpStat { get; set; }

    [Column("STAT_ON_DATE")]
    public DateTime? StatOnDate { get; set; }

    [Column("DESGN_TYPE")]
    public int? DesgnType { get; set; }

    [Column("LAST_PAID_DATE")]
    public DateTime? LastPaidDate { get; set; }

    [Column("PAY_REVIEW_DATE")]
    public DateTime? PayReviewDate { get; set; }

    [Column("PAY_LOCN_CODE")]
    public int? PayLocnCode { get; set; }

    [Column("PAY_MODE")]
    public int? PayMode { get; set; }

    [Column("BANK_CODE")]
    public int? BankCode { get; set; }

    [Column("BRANCH_CODE")]
    public int? BranchCode { get; set; }

    [Column("BANK_ACC_NO")]
    public long? BankAccNo { get; set; }

    [Column("DG_DESP_E")]
    [StringLength(50)]
    public string? DgDespE { get; set; }

    [Column("DEPT_DESP_E")]
    [StringLength(50)]
    public string? DeptDespE { get; set; }

    [Column("BIRTH_DATE")]
    public DateTime? BirthDate { get; set; }

    [Column("EMP_NAME_ENG")]
    [StringLength(120)]
    public string? EmpNameEng { get; set; }

    [Column("GRADE_RANK")]
    [StringLength(100)]
    public string? GradeRank { get; set; }

    [Column("DESG_CODE")]
    [StringLength(120)]
    public string? DesgCode { get; set; }

    [Column("DESG_ENG")]
    [StringLength(50)]
    public string? DesgEng { get; set; }

    [Column("DESG_TYPE")]
    public int? DesgType { get; set; }

    [Column("EMP_FIRST_NAME")]
    [StringLength(50)] 
    public string? EmpFirstName { get; set; }

    [Column("GRD_DT")]
    public DateTime? GrdDt { get; set; }

    [Column("EMP_NAME_ARA")]
    [StringLength(100)] 
    public string? EmpNameAra { get; set; }

    [Column("MAR_DESC")]
    [StringLength(20)]
    public string? MarDesc { get; set; }

    [Column("NATIONALITY_ID_NO")]
    public long? NationalityIdNo { get; set; }

    [Column("BOSS")]
    public int? Boss { get; set; }

    [Column("EMAIL_ID")]
    [StringLength(30)]
    [Required]
    public string EmailId { get; set; } = string.Empty;
  
    [Column("SUBJ_DESP_A")]
    [StringLength(60)]
    public string? SubjDespA { get; set; }

    [Column("QUAL_TO_DATE")]
    [StringLength(120)]
    public string? QualToDate { get; set; }

    [Column("ID_EXPIRY_DATE")]
    public DateTime? IdExpiryDate { get; set; }

    [Column("PASSPORT_NO_E")]
    [StringLength(20)]
 
    public string? PassportNoE { get; set; }


    public virtual ICollection<StrReqMstr>? StrReqMstrs { get; set; }



}
