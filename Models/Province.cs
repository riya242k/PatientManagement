namespace RGPatients.Models
{
    public partial class Province
    {
        public Province()
        {
            Patients = new HashSet<Patient>();
        }

        /// <summary>
        /// 2-character province code ... ON, BC, etc
        /// </summary>
        public string ProvinceCode { get; set; } = null!;
        /// <summary>
        /// full province name ... Ontario, etc.
        /// </summary>
        public string Name { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string SalesTaxCode { get; set; } = null!;
        public double SalesTax { get; set; }
        public bool IncludesFederalTax { get; set; }
        public string? FirstPostalLetter { get; set; }

        public virtual Country CountryCodeNavigation { get; set; } = null!;
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
