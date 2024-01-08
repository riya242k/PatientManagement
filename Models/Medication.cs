namespace RGPatients.Models
{
    public partial class Medication
    {
        public Medication()
        {
            PatientMedications = new HashSet<PatientMedication>();
            Treatments = new HashSet<Treatment>();
        }

        /// <summary>
        /// 8-digit drug identification number
        /// </summary>
        public string Din { get; set; } = null!;
        /// <summary>
        /// name of drug as branded by manufacturer
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// file-name of product image
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// type of drug ... anticoagulant, antihistimine, etc.
        /// </summary>
        public int MedicationTypeId { get; set; }
        /// <summary>
        /// dispensing units: pills, capsils, mg, tablespoons, etc.
        /// </summary>
        public string DispensingCode { get; set; } = null!;
        /// <summary>
        /// concentration quantity, n concentration units, zero if n/a
        /// </summary>
        public double Concentration { get; set; }
        public string ConcentrationCode { get; set; } = null!;

        public virtual ConcentrationUnit ConcentrationCodeNavigation { get; set; } = null!;
        public virtual DispensingUnit DispensingCodeNavigation { get; set; } = null!;
        public virtual MedicationType MedicationType { get; set; } = null!;
        public virtual ICollection<PatientMedication> PatientMedications { get; set; }

        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}
