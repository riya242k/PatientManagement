using Microsoft.EntityFrameworkCore;

namespace RGPatients.Models
{
    public partial class PatientsContext : DbContext
    {
        public PatientsContext()
        {
        }

        public PatientsContext(DbContextOptions<PatientsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConcentrationUnit> ConcentrationUnits { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Diagnosis> Diagnoses { get; set; } = null!;
        public virtual DbSet<DiagnosisCategory> DiagnosisCategories { get; set; } = null!;
        public virtual DbSet<DispensingUnit> DispensingUnits { get; set; } = null!;
        public virtual DbSet<Medication> Medications { get; set; } = null!;
        public virtual DbSet<MedicationType> MedicationTypes { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<PatientDiagnosis> PatientDiagnoses { get; set; } = null!;
        public virtual DbSet<PatientMedication> PatientMedications { get; set; } = null!;
        public virtual DbSet<PatientTreatment> PatientTreatments { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Treatment> Treatments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-NN27E3IQ\\SQLEXPRESS19;Database=Patients;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConcentrationUnit>(entity =>
            {
                entity.HasKey(e => e.ConcentrationCode);

                entity.ToTable("concentrationUnit");

                entity.Property(e => e.ConcentrationCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("concentrationCode");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryCode);

                entity.ToTable("country");

                entity.HasComment("list of countries and data pertinent to them");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("countryCode")
                    .IsFixedLength()
                    .HasComment("2-character short form for country");

                entity.Property(e => e.FederalSalesTax).HasColumnName("federalSalesTax");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .HasComment("formal name of country");

                entity.Property(e => e.PhonePattern)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("phonePattern")
                    .HasComment("regular expression used to validate a phone number in this country, includes ^ and $");

                entity.Property(e => e.PostalPattern)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("postalPattern")
                    .HasComment("regular expression used to validate the postal or zip code for this country, includes ^ and $");
            });

            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(e => e.DiagnosisId)
                    .HasName("aaaaadiagnosis_PK")
                    .IsClustered(false);

                entity.ToTable("diagnosis");

                entity.HasIndex(e => e.DiagnosisId, "ailmentId");

                entity.HasIndex(e => e.DiagnosisCategoryId, "categorydiagnosis");

                entity.HasIndex(e => e.DiagnosisCategoryId, "diseasecategoryId");

                entity.Property(e => e.DiagnosisId)
                    .HasColumnName("diagnosisId")
                    .HasComment("random key");

                entity.Property(e => e.DiagnosisCategoryId)
                    .HasColumnName("diagnosisCategoryId")
                    .HasComment("link to major categories");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .HasComment("medical name for ailment");

                entity.HasOne(d => d.DiagnosisCategory)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.DiagnosisCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_diagnosis_diagnosisCategory");
            });

            modelBuilder.Entity<DiagnosisCategory>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("aaaaacategory_PK")
                    .IsClustered(false);

                entity.ToTable("diagnosisCategory");

                entity.HasIndex(e => e.Id, "categoryId");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("random key, allowing category to be renamed");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .HasComment("major medical categories: cardiology, respiratory, etc.");
            });

            modelBuilder.Entity<DispensingUnit>(entity =>
            {
                entity.HasKey(e => e.DispensingCode);

                entity.ToTable("dispensingUnit");

                entity.Property(e => e.DispensingCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dispensingCode");
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.HasKey(e => e.Din)
                    .HasName("aaaaamedication_PK")
                    .IsClustered(false);

                entity.ToTable("medication");

                entity.Property(e => e.Din)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("din")
                    .HasComment("8-digit drug identification number");

                entity.Property(e => e.Concentration)
                    .HasColumnName("concentration")
                    .HasComment("concentration quantity, n concentration units, zero if n/a");

                entity.Property(e => e.ConcentrationCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("concentrationCode");

                entity.Property(e => e.DispensingCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dispensingCode")
                    .HasComment("dispensing units: pills, capsils, mg, tablespoons, etc.");

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("image")
                    .HasComment("file-name of product image");

                entity.Property(e => e.MedicationTypeId)
                    .HasColumnName("medicationTypeId")
                    .HasComment("type of drug ... anticoagulant, antihistimine, etc.");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .HasComment("name of drug as branded by manufacturer");

                entity.HasOne(d => d.ConcentrationCodeNavigation)
                    .WithMany(p => p.Medications)
                    .HasForeignKey(d => d.ConcentrationCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_medication_concentrationUnit");

                entity.HasOne(d => d.DispensingCodeNavigation)
                    .WithMany(p => p.Medications)
                    .HasForeignKey(d => d.DispensingCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_medication_dispensingUnit");

                entity.HasOne(d => d.MedicationType)
                    .WithMany(p => p.Medications)
                    .HasForeignKey(d => d.MedicationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_medication_medicationType");
            });

            modelBuilder.Entity<MedicationType>(entity =>
            {
                entity.ToTable("medicationType");

                entity.Property(e => e.MedicationTypeId).HasColumnName("medicationTypeId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId)
                    .HasName("aaaaapatient_PK")
                    .IsClustered(false);

                entity.ToTable("patient");

                entity.HasIndex(e => e.HomePhone, "homePhone");

                entity.HasIndex(e => e.PatientId, "patientId");

                entity.HasIndex(e => e.PostalCode, "postalCode");

                entity.HasIndex(e => e.ProvinceCode, "provinceCode");

                entity.HasIndex(e => e.ProvinceCode, "provincepatient");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patientId")
                    .HasComment("random patient number");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address")
                    .HasComment("street address");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city")
                    .HasComment("city");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("dateOfBirth")
                    .HasComment("date of birth");

                entity.Property(e => e.DateOfDeath)
                    .HasColumnType("datetime")
                    .HasColumnName("dateOfDeath")
                    .HasComment("date of death (null if alive)");

                entity.Property(e => e.Deceased)
                    .HasColumnName("deceased")
                    .HasComment("if yes, date of death required else, ignore date of death");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName")
                    .HasComment("patient's first or given name");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("gender")
                    .IsFixedLength()
                    .HasComment("M or F");

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("homePhone")
                    .HasComment("10-digit home phone number");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName")
                    .HasComment("patient's surname or family name");

                entity.Property(e => e.Ohip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OHIP")
                    .HasComment("12-character provincial medical");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("postalCode")
                    .HasComment("postal code: A9A 9A9");

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("provinceCode")
                    .IsFixedLength()
                    .HasComment("2-character province code");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK_patient_province");
            });

            modelBuilder.Entity<PatientDiagnosis>(entity =>
            {
                entity.ToTable("patientDiagnosis");

                entity.Property(e => e.PatientDiagnosisId).HasColumnName("patientDiagnosisId");

                entity.Property(e => e.Comments).HasColumnName("comments");

                entity.Property(e => e.DiagnosisId).HasColumnName("diagnosisId");

                entity.Property(e => e.PatientId).HasColumnName("patientId");

                entity.HasOne(d => d.Diagnosis)
                    .WithMany(p => p.PatientDiagnoses)
                    .HasForeignKey(d => d.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientDiagnosis_diagnosis");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientDiagnoses)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientDiagnosis_patient");
            });

            modelBuilder.Entity<PatientMedication>(entity =>
            {
                entity.HasKey(e => e.PatientMedicationId)
                    .HasName("aaaaapatientMedication_PK")
                    .IsClustered(false);

                entity.ToTable("patientMedication");

                entity.HasIndex(e => e.Din, "medicationpatientMedication");

                entity.HasIndex(e => e.PatientTreatmentId, "patientTreatmentpatientMedication");

                entity.Property(e => e.PatientMedicationId).HasColumnName("patientMedicationId");

                entity.Property(e => e.Comments)
                    .IsUnicode(false)
                    .HasColumnName("comments");

                entity.Property(e => e.Din)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("din")
                    .HasComment("link to medication by drug identification number");

                entity.Property(e => e.Dose)
                    .HasColumnName("dose")
                    .HasDefaultValueSql("((0))")
                    .HasComment("number of dispensing units at a time");

                entity.Property(e => e.ExactMinMax)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("exactMinMax")
                    .HasDefaultValueSql("(N'exact')")
                    .HasComment("dosage frequency is exactly x periods, minimum of, or maximum of");

                entity.Property(e => e.Frequency)
                    .HasColumnName("frequency")
                    .HasDefaultValueSql("((0))")
                    .HasComment("number of doses per day/week/month; zero if as-required");

                entity.Property(e => e.FrequencyPeriod)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("frequencyPeriod")
                    .HasComment("period of frequency: per day, week, month or as-required");

                entity.Property(e => e.PatientTreatmentId)
                    .HasColumnName("patientTreatmentId")
                    .HasComment("link back to the procedure for this patient");

                entity.HasOne(d => d.DinNavigation)
                    .WithMany(p => p.PatientMedications)
                    .HasForeignKey(d => d.Din)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientMedication_medication");

                entity.HasOne(d => d.PatientTreatment)
                    .WithMany(p => p.PatientMedications)
                    .HasForeignKey(d => d.PatientTreatmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientMedication_patientTreatment");
            });

            modelBuilder.Entity<PatientTreatment>(entity =>
            {
                entity.HasKey(e => e.PatientTreatmentId)
                    .HasName("aaaaapatientTreatment_PK")
                    .IsClustered(false);

                entity.ToTable("patientTreatment");

                entity.HasIndex(e => e.PatientTreatmentId, "patientProcedureId");

                entity.HasIndex(e => e.TreatmentId, "procedureId");

                entity.HasIndex(e => e.TreatmentId, "procedurepatientProcedure");

                entity.Property(e => e.PatientTreatmentId)
                    .HasColumnName("patientTreatmentId")
                    .HasComment("random key for treatment on this patient");

                entity.Property(e => e.Comments)
                    .IsUnicode(false)
                    .HasColumnName("comments")
                    .HasComment("general free-form comments about treatment");

                entity.Property(e => e.DatePrescribed)
                    .HasColumnType("datetime")
                    .HasColumnName("datePrescribed")
                    .HasComment("date treatment prescribed to patient");

                entity.Property(e => e.PatientDiagnosisId).HasColumnName("patientDiagnosisId");

                entity.Property(e => e.TreatmentId)
                    .HasColumnName("treatmentId")
                    .HasComment("link back to treatment");

                entity.HasOne(d => d.PatientDiagnosis)
                    .WithMany(p => p.PatientTreatments)
                    .HasForeignKey(d => d.PatientDiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientTreatment_patientDiagnosis");

                entity.HasOne(d => d.Treatment)
                    .WithMany(p => p.PatientTreatments)
                    .HasForeignKey(d => d.TreatmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("patientTreatment_FK01");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ProvinceCode)
                    .HasName("aaaaaprovince_PK")
                    .IsClustered(false);

                entity.ToTable("province");

                entity.HasIndex(e => e.ProvinceCode, "ProvinceCode");

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("provinceCode")
                    .IsFixedLength()
                    .HasComment("2-character province code ... ON, BC, etc");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("countryCode")
                    .IsFixedLength();

                entity.Property(e => e.FirstPostalLetter)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstPostalLetter");

                entity.Property(e => e.IncludesFederalTax).HasColumnName("includesFederalTax");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .HasComment("full province name ... Ontario, etc.");

                entity.Property(e => e.SalesTax).HasColumnName("salesTax");

                entity.Property(e => e.SalesTaxCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("salesTaxCode");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Provinces)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_province_country");
            });

            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.HasKey(e => e.TreatmentId)
                    .HasName("aaaaatreatment_PK")
                    .IsClustered(false);

                entity.ToTable("treatment");

                entity.HasIndex(e => e.DiagnosisId, "diagnosisId");

                entity.HasIndex(e => e.DiagnosisId, "diagnosisprocedure");

                entity.HasIndex(e => e.TreatmentId, "procedureId");

                entity.Property(e => e.TreatmentId)
                    .HasColumnName("treatmentId")
                    .HasComment("random key");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasComment("free-form decription of the procedure");

                entity.Property(e => e.DiagnosisId)
                    .HasColumnName("diagnosisId")
                    .HasComment("link back to diagnosis");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .HasComment("formal name of the procedure");

                entity.HasOne(d => d.Diagnosis)
                    .WithMany(p => p.Treatments)
                    .HasForeignKey(d => d.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_treatment_diagnosis");

                entity.HasMany(d => d.Dins)
                    .WithMany(p => p.Treatments)
                    .UsingEntity<Dictionary<string, object>>(
                        "TreatmentMedication",
                        l => l.HasOne<Medication>().WithMany().HasForeignKey("Din").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_treatmentMedication_medication"),
                        r => r.HasOne<Treatment>().WithMany().HasForeignKey("TreatmentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_treatmentMedication_treatment"),
                        j =>
                        {
                            j.HasKey("TreatmentId", "Din").HasName("aaaaatreatmentMedication_PK").IsClustered(false);

                            j.ToTable("treatmentMedication");

                            j.HasIndex(new[] { "Din" }, "medicationtreatmentMedication");

                            j.HasIndex(new[] { "TreatmentId" }, "treatmentId");

                            j.HasIndex(new[] { "TreatmentId" }, "treatmenttreatmentMedication");

                            j.IndexerProperty<int>("TreatmentId").HasColumnName("treatmentId").HasComment("link to treatment this record is for");

                            j.IndexerProperty<string>("Din").HasMaxLength(8).IsUnicode(false).HasColumnName("din").HasComment("link to medication for this treatment");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
