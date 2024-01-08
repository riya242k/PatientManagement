namespace RGPatients.Models
{
    public partial class PatientTreatment
    {
        public PatientTreatment()
        {
            PatientMedications = new HashSet<PatientMedication>();
        }

        /// <summary>
        /// random key for treatment on this patient
        /// </summary>
        public int PatientTreatmentId { get; set; }
        /// <summary>
        /// link back to treatment
        /// </summary>
        public int TreatmentId { get; set; }
        /// <summary>
        /// date treatment prescribed to patient
        /// </summary>
        public DateTime DatePrescribed { get; set; }
        /// <summary>
        /// general free-form comments about treatment
        /// </summary>
        public string? Comments { get; set; }
        public int PatientDiagnosisId { get; set; }

        public virtual PatientDiagnosis PatientDiagnosis { get; set; } = null!;
        public virtual Treatment Treatment { get; set; } = null!;
        public virtual ICollection<PatientMedication> PatientMedications { get; set; }
    }
}
