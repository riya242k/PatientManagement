namespace RGPatients.Models
{
    public partial class Treatment
    {
        public Treatment()
        {
            PatientTreatments = new HashSet<PatientTreatment>();
            Dins = new HashSet<Medication>();
        }

        /// <summary>
        /// random key
        /// </summary>
        public int TreatmentId { get; set; }
        /// <summary>
        /// formal name of the procedure
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// free-form decription of the procedure
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// link back to diagnosis
        /// </summary>
        public int DiagnosisId { get; set; }

        public virtual Diagnosis Diagnosis { get; set; } = null!;
        public virtual ICollection<PatientTreatment> PatientTreatments { get; set; }

        public virtual ICollection<Medication> Dins { get; set; }
    }
}
