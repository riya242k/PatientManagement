namespace RGPatients.Models
{
    public partial class ConcentrationUnit
    {
        public ConcentrationUnit()
        {
            Medications = new HashSet<Medication>();
        }

        public string ConcentrationCode { get; set; } = null!;

        public virtual ICollection<Medication> Medications { get; set; }
    }
}
