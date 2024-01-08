namespace RGPatients.Models
{
    public partial class Diagnosis
    {
        public Diagnosis()
        {
            PatientDiagnoses = new HashSet<PatientDiagnosis>();
            Treatments = new HashSet<Treatment>();
        }

        /// <summary>
        /// random key
        /// </summary>
        public int DiagnosisId { get; set; }
        /// <summary>
        /// medical name for ailment
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// link to major categories
        /// </summary>
        public int DiagnosisCategoryId { get; set; }

        public virtual DiagnosisCategory DiagnosisCategory { get; set; } = null!;
        public virtual ICollection<PatientDiagnosis> PatientDiagnoses { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}
