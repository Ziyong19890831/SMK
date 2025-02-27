using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SMK.Data.Entity;

namespace SMK.Data
{
    public partial class SMKWEBContext : DbContext
    {
        //public SMKWEBContext()
        //    : base()
        //{
        //}
        public SMKWEBContext(DbContextOptions<SMKWEBContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(3000);
        }
        public virtual DbSet<GenEmpData> GenEmpData { get; set; }
        public virtual DbSet<RoleEmpMapping> RoleEmpMapping { get; set; }
        public virtual DbSet<Privilege> Privilege { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePrivilegeMapping> RolePrivilegeMapping { get; set; }

        public virtual DbSet<GenBranch> GenBranch { get; set; }
        public virtual DbSet<GenEndReason> GenEndReason { get; set; }
        public virtual DbSet<GenHospCont> GenHospCont { get; set; }
        public virtual DbSet<GenLicenceType> GenLicenceType { get; set; }
        public virtual DbSet<GenPrsnType> GenPrsnType { get; set; }
        public virtual DbSet<GenSmkcontract> GenSmkcontract { get; set; }
        public virtual DbSet<GenSpecial> GenSpecial { get; set; }
        public virtual DbSet<HospBasic> HospBasic { get; set; }
        public virtual DbSet<HospBscAll> HospBscAll { get; set; }
        public virtual DbSet<HospContract> HospContract { get; set; }
        public virtual DbSet<HospContractType> HospContractType { get; set; }
        public virtual DbSet<PrsnBasic> PrsnBasic { get; set; }
        public virtual DbSet<PrsnContract> PrsnContract { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<PrsnEmail> PrsnEmail { get; set; }
        public virtual DbSet<PrsnLicence> PrsnLicence { get; set; }
        public virtual DbSet<GenPrsnEndReason> GenPrsnEndReason { get; set; }
        public virtual DbSet<QsLicenceMap> QsLicenceMap { get; set; }

        /// <summary>
        /// 藥局門診處方及治療明細資料-原始檔
        /// </summary>
        public virtual DbSet<IniDrDtl> IniDrDtl { get; set; }
        /// <summary>
        /// 藥局門診處方及治療明細資料-更新檔
        /// </summary>
        public virtual DbSet<IniDrOrd> IniDrOrd { get; set; }
        /// <summary>
        /// 健保門診處方及治療明細資料-原始檔
        /// </summary>
        public virtual DbSet<IniOpDtl> IniOpDtl { get; set; }
        /// <summary>
        /// 健保門診處方及治療明細資料-更新檔
        /// </summary>
        public virtual DbSet<IniOpOrd> IniOpOrd { get; set; }
        /// <summary>
        /// 上傳檔案log
        /// </summary>
        public virtual DbSet<FileUploadLog> FileUploadLog { get; set; }

        public virtual DbSet<DtlWithSet> DtlWithSet { get; set; }
        public virtual DbSet<MhbtAgentPatient> MhbtAgentPatient { get; set; }
        public virtual DbSet<MhbtQsCure> MhbtQsCure { get; set; }
        public virtual DbSet<MhbtQsData> MhbtQsData { get; set; }
        public virtual DbSet<MhbtQsData2> MhbtQsData2 { get; set; }
        public virtual DbSet<MhbtQsState> MhbtQsState { get; set; }
        public virtual DbSet<SamplingData> SamplingData { get; set; }
        public virtual DbSet<SamplingList> SamplingList { get; set; }
        public virtual DbSet<UpdDrDtl> UpdDrDtl { get; set; }
        public virtual DbSet<UpdOpDtl> UpdOpDtl { get; set; }
        public virtual DbSet<VwDrData> VwDrData { get; set; }
        public virtual DbSet<VwOpData> VwOpData { get; set; }
        public virtual DbSet<DataInsertLog> DataInsertLog { get; set; }
        public virtual DbSet<QuitDataAll> QuitDataAll { get; set; }
        public virtual DbSet<IniFileInCtrl> IniFileInCtrl { get; set; }
        public virtual DbSet<IniExportInCtrl> IniExportInCtrl { get; set; }
        public virtual DbSet<CorrectionSlip> CorrectionSlip { get; set; }
        public virtual DbSet<GenDrugBasic> GenDrugBasic { get; set; }
        public virtual DbSet<OrdOfB7> OrdOfB7 { get; set; }
        public virtual DbSet<GenOrderCode> GenOrderCode { get; set; }
        public virtual DbSet<QsQuota> QsQuota { get; set; }
        public virtual DbSet<PrsnContactReport> PrsnContactReport { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public virtual DbSet<GenSubDivision> GenSubDivision { get; set; }
        public virtual DbSet<ICCardData> ICCardData { get; set; }
        public virtual DbSet<ICcardByMonth> ICcardByMonth { get; set; }
        public virtual DbSet<ICNotFound> ICNotFound { get; set; }
        public virtual DbSet<ICRateLately> ICRateLately { get; set; }
        public virtual DbSet<ICRateByMonth> ICRateByMonth { get; set; }
        public virtual DbSet<GenLoginLog> GenLoginLog { get; set; }
        public virtual DbSet<ScheduleTxtLog> ScheduleTxtLog { get; set; }
        /// <summary>
        /// 月報表-年概況統計表
        /// </summary>
        public virtual DbSet<IniMonthDetail> IniMonthDetail { get; set; }
        public virtual DbSet<ApplyHospChange> ApplyHospChange { get; set; }
        public virtual DbSet<ApplyHospEnd> ApplyHospEnd { get; set; }
        public virtual DbSet<ApplyHospNew> ApplyHospNew { get; set; }
        public virtual DbSet<ApplyPrsnNew> ApplyPrsnNew { get; set; }
        public virtual DbSet<ApplyPrsnEnd> ApplyPrsnEnd { get; set; }
        public virtual DbSet<ApplyPrsnChange> ApplyPrsnChange { get; set; }
        public virtual DbSet<QuotaHosp> QuotaHosp { get; set; }
        /// <summary>
        /// 首頁公告欄
        /// </summary>
        public virtual DbSet<CallBoard> CallBoard { get; set; }
        /// <summary>
        /// 主約+調製藥局
        /// </summary>
        public virtual DbSet<ApplyContract> ApplyContract { get; set; }

#if DEBUG
        public static readonly ILoggerFactory _myLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddFilter("SMK", LogLevel.Debug)
                   .AddFilter("EFCore", LogLevel.Debug)
                   .AddConsole();
        });
#endif
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LocalHost;Database=SMKWEB;Trusted_Connection=True;");
            }
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ICcardByMonth>(entity => entity.HasKey(e => new { e.HospID, e.ICCard_YM }));
            modelBuilder.Entity<ICNotFound>(entity => entity.HasKey(e => new { e.FeeYM, e.Data_id,e.CureType}));
            modelBuilder.Entity<ICRateLately>(entity => entity.HasKey(e => new { e.HospID, e.HospSeqNo,e.FeeYM}));
            modelBuilder.Entity<ICRateByMonth>(entity => entity.HasKey(e => new { e.HospID, e.HospSeqNo,e.FeeYM,e.CureType}));
            modelBuilder.Entity<MhbtQsData2>(entity => entity.HasKey(e => new { e.HospId,e.ID,e.Birthday,e.FuncDate,e.CureStage,e.ExamYear, e.Cure_Type,e.HospSeqNo}));
            modelBuilder.Entity<ApplyContract>(entity => entity.HasKey(e => new { e.HOSP_ID,e.HOSP_SEQ_NO,e.Cure_Type,e.CONT_S_DATE,}));


            modelBuilder.Entity<ICCardData>(entity =>
            {
                entity.HasKey(e => e.ID);
            });
            modelBuilder.Entity<ExceptionLog>(entity =>
            {
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.CreatedAt);

                entity.Property(e => e.Message).HasColumnType("text");
                entity.Property(e => e.StackTrace).HasColumnType("text");
                entity.Property(e => e.ExtraData).HasColumnType("text");
            });

            modelBuilder.Entity<PrsnContactReport>(entity =>
                {
                    entity.HasKey(t => t.身分證號);
                    entity.ToView("PrsnContactReport");
                }
            );
            modelBuilder.Entity<QsQuota>().HasKey(entity =>
            new { entity.YEARS, entity.HOSP_ID, entity.HOSP_SEQ_NO, entity.CURE_TYPE, entity.VALID_S_DATE, entity.VALID_E_DATE });

            modelBuilder.Entity<GenDrugBasic>(entity =>
            {
                entity.HasIndex(e => e.DrugNo);

                entity.Property(e => e.DrugCompany);

                entity.Property(e => e.DrugContent);

                entity.Property(e => e.DrugIngredient);

                entity.Property(e => e.DrugType);

                entity.Property(e => e.OrderChiName);

                entity.Property(e => e.OrderEndDate);

                entity.Property(e => e.OrderEngName);

                entity.Property(e => e.OrderStartDate);

                entity.Property(e => e.prescription);

                entity.Property(e => e.UnitPrice);

                entity.Property(e => e.HealthCarePrice);

            });
            modelBuilder.Entity<CorrectionSlip>(entity =>
            {
                entity.HasKey(e => new { e.CaseNo, e.ReceiveDate });

                entity.HasIndex(e => e.CaseNo);

                entity.Property(e => e.Birthday);

                entity.Property(e => e.CorrectBasic);

                entity.Property(e => e.CorrectHealth);

                entity.Property(e => e.CorrectHosp);

                entity.Property(e => e.CorrectItems);

                entity.Property(e => e.CorrectItems2);

                entity.Property(e => e.CorrectOther);

                entity.Property(e => e.HospId);

                entity.Property(e => e.HospName);

                entity.Property(e => e.ID);

                entity.Property(e => e.Memo);

                entity.Property(e => e.Name);

                entity.Property(e => e.ReceiveDate);

                entity.Property(e => e.source);

                entity.Property(e => e.UpdateAt);

                entity.Property(e => e.UpdatedBy);
            });

            modelBuilder.Entity<GenBranch>(entity =>
            {
                entity.HasKey(e => e.BranchNo)
                    .HasName("PK_GenBranch_1");

                entity.Property(e => e.BranchNo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BranchName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<GenEmpData>(entity =>
            {
                entity.HasIndex(e => e.Account)
                    .IsUnique();
            });
            modelBuilder.Entity<RoleEmpMapping>(entity =>
            {
                entity.HasIndex(
                            nameof(Entity.RoleEmpMapping.EmpId),
                            nameof(Entity.RoleEmpMapping.RoleId)
                        )
                        .IsUnique();
            });
            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.HasIndex(e => e.ParentId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<RolePrivilegeMapping>(entity =>
            {
                entity.HasIndex(
                    nameof(Entity.RolePrivilegeMapping.PrivilegeId),
                    nameof(Entity.RolePrivilegeMapping.RoleId))
                    .IsUnique();
            });

            modelBuilder.Entity<GenEndReason>(entity =>
            {
                entity.HasKey(e => e.EndReasonNo)
                    .HasName("PK_GenEndReason_1");

                entity.Property(e => e.EndReasonNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EndReasonName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });
            modelBuilder.Entity<GenPrsnEndReason>(entity =>
            {
                entity.HasKey(e => e.EndReasonNo)
                    .HasName("PK_GenPrsnEndReason_1");

                entity.Property(e => e.EndReasonNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EndReasonName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });
            modelBuilder.Entity<GenHospCont>(entity =>
            {
                entity.HasKey(e => e.HospContType)
                    .HasName("PK_GenHospCont_1");

                entity.Property(e => e.HospContType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospContName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.QualityDefaultCount)
                      .HasColumnName("QualityDefaultCount");

                entity.Property(e => e.QualityImproveCount)
                      .HasColumnName("QualityImproveCount");
            });

            modelBuilder.Entity<GenLicenceType>(entity =>
            {
                entity.HasKey(e => e.LicenceType)
                    .HasName("PK_GenLicenceType_1");

                entity.Property(e => e.LicenceType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LicenceName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<GenPrsnType>(entity =>
            {
                entity.HasKey(e => e.PrsnType)
                    .HasName("PK_GenPrsnType_1");

                entity.Property(e => e.PrsnType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrsnTypeName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<GenSmkcontract>(entity =>
            {
                entity.HasKey(e => e.SmkcontractType)
                    .HasName("PK_GenSMKContract_1");

                entity.ToTable("GenSMKContract");

                entity.Property(e => e.SmkcontractType)
                    .HasColumnName("SMKContractType")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SmkcontractName)
                    .HasColumnName("SMKContractName")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<GenSpecial>(entity =>
            {
                entity.HasKey(e => e.SpecialistNo)
                    .HasName("PK_GenSpecial_2");

                entity.Property(e => e.SpecialistNo)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialistName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<HospBasic>(entity =>
            {
                entity.HasKey(e => new { e.HospId, e.HospSeqNo })
                    .HasName("PK_SMKHospBasic");

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BranchNo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ChFlg1)
                    .HasColumnName("chFlg1")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ChFlg2)
                    .HasColumnName("chFlg2")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ChFlg3)
                    .HasColumnName("chFlg3")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contact1)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contact2)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactEmail1)
                    .HasMaxLength(60)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactEmail2)
                    .HasMaxLength(60)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactFax1)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactFax2)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactTel1)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactTel2)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DivisionNo)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstHospId)
                    .HasColumnName("FirstHospID")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstHospSeqNo)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospAbbr)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospAddress)
                    .HasMaxLength(80)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospEmail)
                    .HasMaxLength(60)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospFax)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospName)
                    .HasMaxLength(80)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospOwnId)
                    .HasColumnName("HospOwnID")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospOwnName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospStatus)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospTel)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastContType)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastHospId)
                    .HasColumnName("LastHospID")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastHospSeqNo)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrevHospID)
                    .HasColumnName("PrevHospID")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrevHospSeqNo)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SubDivisionNo)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Zip)
                    .HasColumnName("ZIP")
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreateDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModifyDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");
            });
            modelBuilder.Entity<HospBscAll>(entity =>
            {
                entity.HasKey(e => e.HospId);

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BranchNo)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContType)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospAddress)
                    .HasMaxLength(80)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospEndDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospKind)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospName)
                    .HasMaxLength(80)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospTel)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospTelArea)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospType)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<HospContract>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .IsRequired();

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .IsRequired();

                entity.Property(e => e.SmkcontractType)
                    .HasColumnName("SMKContractType")
                    .HasMaxLength(2);

                entity.Property(e => e.HospStartDate)
                    .HasMaxLength(8);

                entity.Property(e => e.CreateDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EndReasonNo)
                    .HasMaxLength(2)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospEndDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModifyDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
                entity.HasIndex(e => new { e.HospId, e.HospSeqNo, e.SmkcontractType, e.HospStartDate }).IsUnique();
            });


            modelBuilder.Entity<PrsnBasic>(entity =>
            {
                entity.HasKey(e => e.PrsnId);

                entity.Property(e => e.PrsnId)
                    .HasColumnName("PrsnID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MajorSpecialistNo)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnBirthday)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnName)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnType)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SubSpecialistNo)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.Pemail)
                  .HasColumnName("PEmail")
                  .HasMaxLength(80);
            });

            modelBuilder.Entity<PrsnContract>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrsnId)
                    .HasColumnName("PrsnID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SmkcontractType)
                    .HasColumnName("SMKContractType")
                    .HasMaxLength(10);

                entity.Property(e => e.PrsnStartDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CouldInstruct)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CouldTreat)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreateDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModifyDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnEndDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");
                entity.Property(e => e.EndReasonNo)
                   .HasMaxLength(2)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.HasIndex(e => new { e.HospId, e.HospSeqNo, e.PrsnId, e.SmkcontractType, e.PrsnStartDate }).IsUnique();
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(e => e.RecordId);
                entity.HasIndex(e => e.Account);
                entity.HasIndex(e => e.SourceTable);

                entity.Property(e => e.OriginalRecord).HasColumnType("text");
                entity.Property(e => e.CurrentRecord).HasColumnType("text");
            });

            modelBuilder.Entity<PrsnEmail>(entity =>
            {
                entity.HasKey(e => new { e.PrsnId, e.Pemail });

                entity.Property(e => e.PrsnId)
                    .HasColumnName("PrsnID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Pemail)
                    .HasColumnName("PEmail")
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrsnLicence>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.PrsnId)
                    .HasColumnName("PrsnID")
                    .HasMaxLength(10);

                entity.Property(e => e.LicenceType)
                    .HasMaxLength(2);

                entity.Property(e => e.LicenceNo)
                    .HasMaxLength(30);

                entity.Property(e => e.CertPublicDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CertStartDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CertEndDate)
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                            .HasMaxLength(200);

                entity.HasIndex(e => new { e.PrsnId, e.LicenceType, e.LicenceNo }).IsUnique();
            });

            modelBuilder.Entity<QsLicenceMap>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LicenceType)
                    .HasMaxLength(2);

                entity.Property(e => e.CTypeSNO)
                    .HasMaxLength(10);

                entity.Property(e => e.CTypeName)
                    .HasMaxLength(50);

                entity.HasIndex(e => new { e.LicenceType, e.CTypeSNO }).IsUnique();
            });

            modelBuilder.Entity<IniDrDtl>(entity =>
            {
                entity.HasKey(e => new { e.DataId, e.FeeYm });

                entity.ToTable("iniDrDtl");

                entity.HasIndex(e => new { e.Id, e.FeeYm, e.HospId, e.FuncDate, e.Birthday, e.MedApply, e.WeekCount, e.DrugDays, e.OrigHospId, e.TranDate })
                    .HasName("INX_DrDtl");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ApplCauseMark)
                    .HasColumnName("appl_cause_mark")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDate)
                    .HasColumnName("appl_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDot).HasColumnName("appl_dot");

                entity.Property(e => e.ApplType)
                    .HasColumnName("appl_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AreaService)
                    .HasColumnName("area_service")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CaseType)
                    .HasColumnName("case_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CorrHospId)
                    .HasColumnName("corr_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureDot).HasColumnName("cure_dot");

                entity.Property(e => e.CureItem1)
                    .HasColumnName("cure_item1")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem2)
                    .HasColumnName("cure_item2")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem3)
                    .HasColumnName("cure_item3")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem4)
                    .HasColumnName("cure_item4")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugDays).HasColumnName("drug_days");

                entity.Property(e => e.DrugDot).HasColumnName("drug_dot");

                entity.Property(e => e.DrugPrsnId)
                    .HasColumnName("drug_prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcCode)
                    .HasColumnName("dsvc_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcDot).HasColumnName("dsvc_dot");

                entity.Property(e => e.ExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExpDot).HasColumnName("exp_dot");

                entity.Property(e => e.FirstInstructDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstTreatDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncDate)
                    .HasColumnName("func_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncSeqNo)
                    .HasColumnName("func_seq_no")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncType)
                    .HasColumnName("func_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode2)
                    .HasColumnName("icd10cm_code2")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode3)
                    .HasColumnName("icd10cm_code3")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode4)
                    .HasColumnName("icd10cm_code4")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode)
                    .HasColumnName("icd9cm_code")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode1)
                    .HasColumnName("icd9cm_code1")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode2)
                    .HasColumnName("icd9cm_code2")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdSex)
                    .HasColumnName("Id_Sex")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MedApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.OrigCaseType)
                    .HasColumnName("orig_case_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrigHospId)
                    .HasColumnName("orig_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherPartAmt).HasColumnName("other_part_amt");

                entity.Property(e => e.PartAmt).HasColumnName("part_amt");

                entity.Property(e => e.PartCode)
                    .HasColumnName("part_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PayType)
                    .HasColumnName("pay_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnId)
                    .HasColumnName("prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RelDate)
                    .HasColumnName("rel_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReleaseApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SeqNo).HasColumnName("seq_no");

                entity.Property(e => e.TraceApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospSeqNo)
                    .HasColumnName("HospSeqNo")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<IniDrOrd>(entity =>
            {
                entity.HasKey(e => new { e.DataId, e.OrderSeqNo, e.FeeYm })
                    .HasName("PK_iniDrOrd_1");

                entity.ToTable("iniDrOrd");

                entity.HasIndex(e => new { e.FeeYm, e.OrderCode })
                    .HasName("INX_DrOrd");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.OrderSeqNo).HasColumnName("order_seq_no");

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DrugFre)
                    .HasColumnName("drug_fre")
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugNum)
                    .HasColumnName("drug_num")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugPath)
                    .HasColumnName("drug_path")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExePrsnId)
                    .HasColumnName("exe_prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderCode)
                    .HasColumnName("order_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderDot).HasColumnName("order_dot");

                entity.Property(e => e.OrderDrugDay).HasColumnName("order_drug_day");

                entity.Property(e => e.OrderQty)
                    .HasColumnName("order_qty")
                    .HasColumnType("decimal(7, 2)");

                entity.Property(e => e.OrderType)
                    .HasColumnName("order_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderUprice)
                    .HasColumnName("order_uprice")
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IniOpDtl>(entity =>
            {
                entity.HasKey(e => new { e.DataId, e.FeeYm });

                entity.ToTable("iniOpDtl");

                entity.HasIndex(e => new { e.Id, e.FeeYm, e.HospId, e.FuncDate, e.Birthday, e.MedApply, e.WeekCount, e.DrugDays, e.TranDate })
                    .HasName("INX_OpDtl");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.AgencyPartAmt)
                    .HasColumnName("agency_part_amt")
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ApplCauseMark)
                    .HasColumnName("appl_cause_mark")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDate)
                    .HasColumnName("appl_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDot).HasColumnName("appl_dot");

                entity.Property(e => e.ApplType)
                    .HasColumnName("appl_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AreaService)
                    .HasColumnName("area_service")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CaseType)
                    .HasColumnName("case_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CorrHospId)
                    .HasColumnName("corr_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureDot).HasColumnName("cure_dot");

                entity.Property(e => e.CureEDate)
                    .HasColumnName("cure_e_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem1)
                    .HasColumnName("cure_item1")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem2)
                    .HasColumnName("cure_item2")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem3)
                    .HasColumnName("cure_item3")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem4)
                    .HasColumnName("cure_item4")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DiagCode)
                    .HasColumnName("diag_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DiagDot).HasColumnName("diag_dot");

                entity.Property(e => e.DrugDays).HasColumnName("drug_days");

                entity.Property(e => e.DrugDot).HasColumnName("drug_dot");

                entity.Property(e => e.DrugPrsnId)
                    .HasColumnName("drug_prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcCode)
                    .HasColumnName("dsvc_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcDot).HasColumnName("dsvc_dot");

                entity.Property(e => e.ExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExpDot).HasColumnName("exp_dot");

                entity.Property(e => e.FirstInstructDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstTreatDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncDate)
                    .HasColumnName("func_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncSeqNo)
                    .HasColumnName("func_seq_no")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncType)
                    .HasColumnName("func_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospDataType)
                    .HasColumnName("hosp_data_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode3)
                    .HasColumnName("icd10cm_code3")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode4)
                    .HasColumnName("icd10cm_code4")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode)
                    .HasColumnName("icd9cm_code")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode1)
                    .HasColumnName("icd9cm_code1")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode2)
                    .HasColumnName("icd9cm_code2")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdSex)
                    .HasColumnName("Id_Sex")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MedApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MetDot).HasColumnName("met_dot");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PartAmt).HasColumnName("part_amt");

                entity.Property(e => e.PartCode)
                    .HasColumnName("part_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PayType)
                    .HasColumnName("pay_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnId)
                    .HasColumnName("prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RealHospId)
                    .HasColumnName("real_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RelMode)
                    .HasColumnName("rel_mode")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReleaseApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SeqNo).HasColumnName("seq_no");

                entity.Property(e => e.SuppArea)
                    .HasColumnName("supp_area")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TraceApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospSeqNo)
                   .HasColumnName("HospSeqNo")
                   .HasMaxLength(2)
                   .IsUnicode(false)
                   .IsFixedLength();
            });

            modelBuilder.Entity<IniOpOrd>(entity =>
            {
                entity.HasKey(e => new { e.DataId, e.OrderSeqNo, e.FeeYm });

                entity.ToTable("iniOpOrd");

                entity.HasIndex(e => new { e.FeeYm, e.OrderCode })
                    .HasName("INX_OpOrd");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.OrderSeqNo).HasColumnName("order_seq_no");

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ChrMark)
                    .HasColumnName("chr_mark")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CurePath)
                    .HasColumnName("cure_path")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugFre)
                    .HasColumnName("drug_fre")
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugNum)
                    .HasColumnName("drug_num")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugPath)
                    .HasColumnName("drug_path")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExePrsnId)
                    .HasColumnName("exe_prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderCode)
                    .HasColumnName("order_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderDot).HasColumnName("order_dot");

                entity.Property(e => e.OrderDrugDay).HasColumnName("order_drug_day");

                entity.Property(e => e.OrderQty)
                    .HasColumnName("order_qty")
                    .HasColumnType("decimal(7, 2)");

                entity.Property(e => e.OrderType)
                    .HasColumnName("order_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrderUprice)
                    .HasColumnName("order_uprice")
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.RelMode)
                    .HasColumnName("rel_mode")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DataInsertLog>(entity =>
            {
                entity.HasKey(e => e.ISNO)
                    .HasName("PK_ISNO_1");

                entity.Property(e => e.FileName);
                entity.Property(e => e.FinishDate);
                entity.Property(e => e.RecordCount);
            });

            modelBuilder.Entity<DtlWithSet>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ApplDot).HasColumnName("Appl_Dot");

                entity.Property(e => e.Birthday)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Bupropion)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Correction)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DataId)
                    .IsRequired()
                    .HasColumnName("Data_ID")
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DentistTreat)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DrTreat)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Drug)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DrugDays).HasColumnName("Drug_Days");

                entity.Property(e => e.EduTreat)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExePrsnId)
                    .HasColumnName("ExePrsnID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ExpDot).HasColumnName("Exp_Dot");

                entity.Property(e => e.FeeYm)
                    .IsRequired()
                    .HasColumnName("Fee_YM")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FirstInstructDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FirstTreatDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FuncDate)
                    .HasColumnName("Func_Date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FuncMonth)
                    .IsRequired()
                    .HasColumnName("Func_Month")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FuncType)
                    .HasColumnName("Func_Type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GumLozenge)
                    .IsRequired()
                    .HasColumnName("Gum_Lozenge")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospContType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IdSex)
                    .HasColumnName("ID_Sex")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Inhaler)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InsQlty)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InstructApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InstructExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LastHospId)
                    .HasColumnName("LastHospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LastHospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LowIncome)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MedApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MedQlty)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.OrigHospId)
                    .HasColumnName("Orig_Hosp_ID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PartAmt).HasColumnName("Part_Amt");

                entity.Property(e => e.PartCode)
                    .HasColumnName("Part_Code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PatchN)
                    .IsRequired()
                    .HasColumnName("Patch_N")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PatchS)
                    .IsRequired()
                    .HasColumnName("Patch_S")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PharTreat)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrsnId)
                    .HasColumnName("Prsn_ID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrsnType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Referral)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RelMode)
                    .HasColumnName("Rel_Mode")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ReleaseApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TraceApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UnCount)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Varenicline)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<MhbtAgentPatient>(entity =>
            {
                entity.HasKey(e => new { HospID = e.HospID, e.HospAgentCode, ID = e.ID, e.Birthday, e.BranchCode, e.TxtDate });

                entity.Property(e => e.HospID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HospAgentCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TxtDate)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.FuncMark)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.InformADDR)
                    .HasMaxLength(120)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SeqNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TelD)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TelM)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TelN)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TownCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TownName)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");
            });

            modelBuilder.Entity<MhbtQsCure>(entity =>
            {
                entity.HasKey(e => new { e.HospID, e.ID, e.Birthday, e.FuncDate, e.CureItem, e.HospSeqNo });

                entity.Property(e => e.HospID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday);

                entity.Property(e => e.FuncDate);

                entity.Property(e => e.CureItem)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AdjustUserID)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CureNum)
                    .HasColumnType("numeric(5, 1)");

                entity.Property(e => e.TxtDate)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");
            });

            modelBuilder.Entity<MhbtQsData>(entity =>
            {
                entity.HasKey(e => new { e.HospId, e.ID, e.Birthday, e.FuncDate, e.CureStage, e.ExamYear, e.CurtState, e.CureType, e.HospSeqNo })
                    .HasName("PK_MhbtQsData_1");

                entity.Property(e => e.HospId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday);

                entity.Property(e => e.FuncDate);

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TxtDate)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CureType)
                    .HasColumnName("Cure_Type")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AdjustUserId)
                    .IsRequired()
                    .HasColumnName("AdjustUserID")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.BaseWeight)
                    .HasColumnType("numeric(18, 1)");

                entity.Property(e => e.CaseKind)
                    .HasColumnName("Case_Kind")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CaseSource)
                    .HasColumnName("Case_Source")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CoCheck)
                    .HasColumnType("numeric(10, 0)")
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CureAgree)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CureStage)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CurtState)
                    .IsUnicode(false);

                entity.Property(e => e.CureState2)
                    .HasColumnName("Cure_State2")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CureWeek)
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.ExamYear)
                    .IsUnicode(false);

                entity.Property(e => e.FeeMark)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.PrsnID)
                    .HasColumnName("PrsnID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeBed)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeDayNum)
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.SmokeFirst)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeMon)
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.SmokeMuch)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeNoGp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeScore)
                    .HasColumnType("numeric(2, 0)")
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeSick)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeStop)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.SmokeYear)
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.TraceCoCheck)
                    .HasColumnType("numeric(2, 0)")
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TraceCoCheck2)
                    .HasColumnName("Trace_Co_Check2")
                    .HasColumnType("numeric(2, 0)")
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TraceDate)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TraceDate2)
                    .HasColumnName("Trace_Date2")
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TraceState)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TraceState2)
                    .HasColumnName("Trace_State2")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.WeekTot)
                    .HasColumnName("WeekTot")
                    .HasColumnType("numeric(2, 0)");
            });

            modelBuilder.Entity<MhbtQsState>(entity =>
            {
                entity.HasKey(e => new { e.HospID, Id = e.ID, e.Birthday, e.FuncDate, e.CureState, e.CureType, e.HospSeqNo });

                entity.Property(e => e.HospID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday);

                entity.Property(e => e.FuncDate);

                entity.Property(e => e.CureState)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CureType)
                    .HasColumnName("Cure_Type")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HospSeqNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AdjustUserID)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.CureStateOther)
                    .HasMaxLength(60)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");

                entity.Property(e => e.TxtDate)
                    .HasDefaultValueSql("(('') collate Chinese_Taiwan_Stroke_CI_AS)");
            });

            modelBuilder.Entity<SamplingData>(entity =>
            {
                entity.HasKey(e => new { e.FeeYm, e.DataId, e.OrderSeqNo }).HasName("PK_SamplingData");

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.OrderSeqNo).HasColumnName("order_seq_no");

                entity.Property(e => e.Appeals)
                    .HasColumnName("appeals")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Appealsamt).HasColumnName("appealsamt");

                entity.Property(e => e.Appealsdate)
                    .HasColumnName("appealsdate")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Appealsremark)
                    .HasColumnName("appealsremark")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Review)
                    .HasColumnName("review")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Reviewamt).HasColumnName("reviewamt");

                entity.Property(e => e.Reviewdate)
                    .HasColumnName("reviewdate")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Reviewremark)
                    .HasColumnName("reviewremark")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SamplingList>(entity =>
            {
                entity.HasKey(e => new { e.FeeYm, e.DataId })
                      .HasName("PK_SamplingList");

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.Accessdate)
                    .HasColumnName("accessdate")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Accessno)
                    .HasColumnName("accessno")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ChkFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Replydate)
                    .HasColumnName("replydate")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Replyno)
                    .HasColumnName("replyno")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.SamplingNo)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<UpdDrDtl>(entity =>
            {
                entity.HasKey(e => new { e.DataId, e.FeeYm });

                entity.ToTable("updDrDtl");

                entity.HasIndex(e => new { e.Id, e.FeeYm, e.HospId, e.FuncDate, e.Birthday, e.MedApply, e.WeekCount, e.DrugDays, e.OrigHospId })
                    .HasName("INX_UDrDtl");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ApplCauseMark)
                    .HasColumnName("appl_cause_mark")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDate)
                    .HasColumnName("appl_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDot).HasColumnName("appl_dot");

                entity.Property(e => e.ApplType)
                    .HasColumnName("appl_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AreaService)
                    .HasColumnName("area_service")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CaseType)
                    .HasColumnName("case_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CorrHospId)
                    .HasColumnName("corr_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureDot).HasColumnName("cure_dot");

                entity.Property(e => e.CureItem1)
                    .HasColumnName("cure_item1")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem2)
                    .HasColumnName("cure_item2")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem3)
                    .HasColumnName("cure_item3")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem4)
                    .HasColumnName("cure_item4")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DrugDays).HasColumnName("drug_days");

                entity.Property(e => e.DrugDot).HasColumnName("drug_dot");

                entity.Property(e => e.DrugPrsnId)
                    .HasColumnName("drug_prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcCode)
                    .HasColumnName("dsvc_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcDot).HasColumnName("dsvc_dot");

                entity.Property(e => e.ExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExpDot).HasColumnName("exp_dot");

                entity.Property(e => e.FirstInstructDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstTreatDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncDate)
                    .HasColumnName("func_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncSeqNo)
                    .HasColumnName("func_seq_no")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncType)
                    .HasColumnName("func_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode2)
                    .HasColumnName("icd10cm_code2")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode3)
                    .HasColumnName("icd10cm_code3")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode4)
                    .HasColumnName("icd10cm_code4")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode)
                    .HasColumnName("icd9cm_code")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode1)
                    .HasColumnName("icd9cm_code1")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode2)
                    .HasColumnName("icd9cm_code2")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdSex)
                    .HasColumnName("Id_Sex")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MedApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModifyDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.OrigCaseType)
                    .HasColumnName("orig_case_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrigHospId)
                    .HasColumnName("orig_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OtherPartAmt).HasColumnName("other_part_amt");

                entity.Property(e => e.PartAmt).HasColumnName("part_amt");

                entity.Property(e => e.PartCode)
                    .HasColumnName("part_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PayType)
                    .HasColumnName("pay_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnId)
                    .HasColumnName("prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RelDate)
                    .HasColumnName("rel_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReleaseApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SeqNo).HasColumnName("seq_no");

                entity.Property(e => e.TraceApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<UpdOpDtl>(entity =>
            {
                entity.HasKey(e => new { e.DataId, e.FeeYm });

                entity.ToTable("updOpDtl");

                entity.HasIndex(e => new { e.Id, e.FeeYm, e.HospId, e.FuncDate, e.Birthday, e.MedApply, e.WeekCount, e.DrugDays })
                    .HasName("INX_UOpDtl");

                entity.Property(e => e.DataId)
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.FeeYm)
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.AgencyPartAmt)
                    .HasColumnName("agency_part_amt")
                    .HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ApplCauseMark)
                    .HasColumnName("appl_cause_mark")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDate)
                    .HasColumnName("appl_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApplDot).HasColumnName("appl_dot");

                entity.Property(e => e.ApplType)
                    .HasColumnName("appl_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AreaService)
                    .HasColumnName("area_service")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CaseType)
                    .HasColumnName("case_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CorrHospId)
                    .HasColumnName("corr_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureDot).HasColumnName("cure_dot");

                entity.Property(e => e.CureEDate)
                    .HasColumnName("cure_e_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem1)
                    .HasColumnName("cure_item1")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem2)
                    .HasColumnName("cure_item2")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem3)
                    .HasColumnName("cure_item3")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CureItem4)
                    .HasColumnName("cure_item4")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DiagCode)
                    .HasColumnName("diag_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DiagDot).HasColumnName("diag_dot");

                entity.Property(e => e.DrugDays).HasColumnName("drug_days");

                entity.Property(e => e.DrugDot).HasColumnName("drug_dot");

                entity.Property(e => e.DrugPrsnId)
                    .HasColumnName("drug_prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcCode)
                    .HasColumnName("dsvc_code")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DsvcDot).HasColumnName("dsvc_dot");

                entity.Property(e => e.ExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExpDot).HasColumnName("exp_dot");

                entity.Property(e => e.FirstInstructDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstTreatDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncDate)
                    .HasColumnName("func_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncSeqNo)
                    .HasColumnName("func_seq_no")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FuncType)
                    .HasColumnName("func_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospDataType)
                    .HasColumnName("hosp_data_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HospId)
                    .HasColumnName("HospID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode3)
                    .HasColumnName("icd10cm_code3")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd10cmCode4)
                    .HasColumnName("icd10cm_code4")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode)
                    .HasColumnName("icd9cm_code")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode1)
                    .HasColumnName("icd9cm_code1")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icd9cmCode2)
                    .HasColumnName("icd9cm_code2")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdSex)
                    .HasColumnName("Id_Sex")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InstructExamYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MedApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MetDot).HasColumnName("met_dot");

                entity.Property(e => e.ModifyDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PartAmt).HasColumnName("part_amt");

                entity.Property(e => e.PartCode)
                    .HasColumnName("part_code")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PayType)
                    .HasColumnName("pay_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PrsnId)
                    .HasColumnName("prsn_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RealHospId)
                    .HasColumnName("real_hosp_id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RelMode)
                    .HasColumnName("rel_mode")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReleaseApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SeqNo).HasColumnName("seq_no");

                entity.Property(e => e.SuppArea)
                    .HasColumnName("supp_area")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TraceApply)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TranDate)
                    .HasColumnName("tran_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<VwDrData>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwDrData");

                entity.Property(e => e.ApplDate)
                    .HasColumnName("appl_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ApplDot).HasColumnName("appl_dot");

                entity.Property(e => e.ApplType)
                    .HasColumnName("appl_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CaseType)
                    .HasColumnName("case_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CureDot).HasColumnName("cure_dot");

                entity.Property(e => e.DataId)
                    .IsRequired()
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.DrugDays).HasColumnName("drug_days");

                entity.Property(e => e.DrugDot).HasColumnName("drug_dot");

                entity.Property(e => e.Examtime).HasColumnName("examtime");

                entity.Property(e => e.Examyear)
                    .HasColumnName("examyear")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FeeYm)
                    .IsRequired()
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Firsttreatdate)
                    .HasColumnName("firsttreatdate")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FuncDate)
                    .HasColumnName("func_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Hospid)
                    .HasColumnName("hospid")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Medapply)
                    .HasColumnName("medapply")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.RelDate)
                    .HasColumnName("rel_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Weekcount).HasColumnName("weekcount");
            });

            modelBuilder.Entity<VwOpData>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwOpData");

                entity.Property(e => e.ApplDate)
                    .HasColumnName("appl_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ApplDot).HasColumnName("appl_dot");

                entity.Property(e => e.ApplType)
                    .HasColumnName("appl_type")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CaseType)
                    .HasColumnName("case_type")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CureDot).HasColumnName("cure_dot");

                entity.Property(e => e.DataId)
                    .IsRequired()
                    .HasColumnName("data_id")
                    .HasMaxLength(28)
                    .IsUnicode(false);

                entity.Property(e => e.DrugDays).HasColumnName("drug_days");

                entity.Property(e => e.DrugDot).HasColumnName("drug_dot");

                entity.Property(e => e.Examtime).HasColumnName("examtime");

                entity.Property(e => e.Examyear)
                    .HasColumnName("examyear")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FeeYm)
                    .IsRequired()
                    .HasColumnName("fee_ym")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Firsttreatdate)
                    .HasColumnName("firsttreatdate")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FuncDate)
                    .HasColumnName("func_date")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Hospid)
                    .HasColumnName("hospid")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Medapply)
                    .HasColumnName("medapply")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.RelDate).HasColumnName("rel_date");

                entity.Property(e => e.Weekcount).HasColumnName("weekcount");
            });

            modelBuilder.Entity<HospContractType>()
                .HasKey(entity => new { entity.HospId, entity.HospSeqNo, entity.HospContType, entity.CntSDate });

            modelBuilder.Entity<QuitDataAll>()
                .HasKey(e => new { e.CaseNo, e.FirstMonth, e.TimeSpan });

            modelBuilder.Entity<IniFileInCtrl>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_IniFileInCtrl");
                entity.Property(e => e.Status)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<IniExportInCtrl>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_IniExportInCtrl");
                entity.Property(e => e.Status)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<IniMonthDetail>(entity =>
            {
                entity.HasIndex(e => new { e.ContractYM }).IsUnique();
            });
            OnModelCreatingPartial(modelBuilder);
#if DEBUG
            seed(modelBuilder);
#endif
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GenEmpData>()
                .HasData(new Entity.GenEmpData()
                {
                    Id = Guid.Empty.ToString(),
                    Account = "Admin",
                    Name = "系統管理員",
                    Pwd = "ZHACEBb7ESmBmYj7XqLotw==",
                    Enable = true,
                    CreatedAt = new DateTime(2021, 7, 6, 13, 39, 5, 627, DateTimeKind.Local).AddTicks(8438),
                });

            modelBuilder.Entity<Role>()
                .HasData(new Role()
                {
                    Id = Guid.Empty.ToString(),
                    Name = "SuperAdmin",
                    CreatedAt = new DateTime(2021, 7, 6, 13, 39, 5, 632, DateTimeKind.Local).AddTicks(1710),
                });

            modelBuilder.Entity<RoleEmpMapping>()
               .HasData(new RoleEmpMapping()
               {
                   Id = Guid.Empty.ToString(),
                   EmpId = Guid.Empty.ToString(),
                   RoleId = Guid.Empty.ToString(),
                   CreatedAt = new DateTime(2021, 7, 6, 13, 39, 5, 632, DateTimeKind.Local).AddTicks(3958),
               });

        }
    }
}
