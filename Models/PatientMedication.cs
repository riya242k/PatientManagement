namespace RGPatients.Models
{
    public partial class PatientMedication
    {
        public int PatientMedicationId { get; set; }
        /// <summary>
        /// link back to the procedure for this patient
        /// </summary>
        public int PatientTreatmentId { get; set; }
        /// <summary>
        /// link to medication by drug identification number
        /// </summary>
        public string Din { get; set; } = null!;
        /// <summary>
        /// number of dispensing units at a time
        /// </summary>
        public double? Dose { get; set; }
        /// <summary>
        /// number of doses per day/week/month; zero if as-required
        /// </summary>
        public int? Frequency { get; set; }
        /// <summary>
        /// period of frequency: per day, week, month or as-required
        /// </summary>
        public string? FrequencyPeriod { get; set; }
        /// <summary>
        /// dosage frequency is exactly x periods, minimum of, or maximum of
        /// </summary>
        public string? ExactMinMax { get; set; }
        public string? Comments { get; set; }

        public virtual Medication DinNavigation { get; set; } = null!;
        public virtual PatientTreatment PatientTreatment { get; set; } = null!;
    }
}
