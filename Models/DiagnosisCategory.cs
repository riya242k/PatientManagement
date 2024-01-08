namespace RGPatients.Models
{
    public partial class DiagnosisCategory
    {
        public DiagnosisCategory()
        {
            Diagnoses = new HashSet<Diagnosis>();
        }

        /// <summary>
        /// random key, allowing category to be renamed
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// major medical categories: cardiology, respiratory, etc.
        /// </summary>
        public string Name { get; set; } = null!;

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
    }
}
