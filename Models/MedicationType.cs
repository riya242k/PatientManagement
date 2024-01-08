namespace RGPatients.Models
{
    public partial class MedicationType
    {
        public MedicationType()
        {
            Medications = new HashSet<Medication>();
        }

        public int MedicationTypeId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Medication> Medications { get; set; }
    }
}
