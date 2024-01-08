namespace RGPatients.Models
{
    public partial class PatientDiagnosis
    {
        public PatientDiagnosis()
        {
            PatientTreatments = new HashSet<PatientTreatment>();
        }

        public int PatientDiagnosisId { get; set; }
        public int PatientId { get; set; }
        public int DiagnosisId { get; set; }
        public string? Comments { get; set; }

        public virtual Diagnosis Diagnosis { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<PatientTreatment> PatientTreatments { get; set; }
    }
}
