namespace RGPatients.Models
{
    public partial class Patient
    {
        public Patient()
        {
            PatientDiagnoses = new HashSet<PatientDiagnosis>();
        }

        /// <summary>
        /// random patient number
        /// </summary>
        public int PatientId { get; set; }
        /// <summary>
        /// patient&apos;s first or given name
        /// </summary>
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// patient&apos;s surname or family name
        /// </summary>
        public string LastName { get; set; } = null!;
        /// <summary>
        /// street address
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// city
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// 2-character province code
        /// </summary>
        public string? ProvinceCode { get; set; }
        /// <summary>
        /// postal code: A9A 9A9
        /// </summary>
        public string? PostalCode { get; set; }
        /// <summary>
        /// 12-character provincial medical
        /// </summary>
        public string? Ohip { get; set; }
        /// <summary>
        /// date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// if yes, date of death required else, ignore date of death
        /// </summary>
        public bool Deceased { get; set; }
        /// <summary>
        /// date of death (null if alive)
        /// </summary>
        public DateTime? DateOfDeath { get; set; }
        /// <summary>
        /// 10-digit home phone number
        /// </summary>
        public string? HomePhone { get; set; }
        /// <summary>
        /// M or F
        /// </summary>
        public string? Gender { get; set; }

        public virtual Province? ProvinceCodeNavigation { get; set; }
        public virtual ICollection<PatientDiagnosis> PatientDiagnoses { get; set; }
    }
}
