namespace RGPatients.Models
{
    /// <summary>
    /// list of countries and data pertinent to them
    /// </summary>
    public partial class Country
    {
        public Country()
        {
            Provinces = new HashSet<Province>();
        }

        /// <summary>
        /// 2-character short form for country
        /// </summary>
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// formal name of country
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// regular expression used to validate the postal or zip code for this country, includes ^ and $
        /// </summary>
        public string? PostalPattern { get; set; }
        /// <summary>
        /// regular expression used to validate a phone number in this country, includes ^ and $
        /// </summary>
        public string? PhonePattern { get; set; }
        public double FederalSalesTax { get; set; }

        public virtual ICollection<Province> Provinces { get; set; }
    }
}
