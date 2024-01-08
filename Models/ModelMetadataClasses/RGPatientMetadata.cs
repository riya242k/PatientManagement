using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using RGClassLibrary;
using System.Text.RegularExpressions;

namespace RGPatients.Models
{
    [ModelMetadataType(typeof(RGPatientMetadata))]

    public partial class Patient : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Province province = new Province();
            PatientsContext patientsContext = new PatientsContext();

            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            Address = !string.IsNullOrWhiteSpace(Address) ? Address.Trim() : ""; 
            City = !string.IsNullOrWhiteSpace(City) ? City.Trim() : "";
            ProvinceCode = !string.IsNullOrWhiteSpace(ProvinceCode) ? ProvinceCode.Trim() : "";
            PostalCode = !string.IsNullOrWhiteSpace(PostalCode) ? PostalCode.Trim() : "";
            Ohip = !string.IsNullOrWhiteSpace(Ohip) ? Ohip.Trim() : "";
            HomePhone = !string.IsNullOrWhiteSpace(HomePhone) ? HomePhone.Trim() : "";
            Gender = Gender.Trim();


            FirstName = RGValidations.RGCapitalize(FirstName);
            LastName = RGValidations.RGCapitalize(LastName);
            Address = RGValidations.RGCapitalize(Address);
            City = RGValidations.RGCapitalize(City);
            Gender = RGValidations.RGCapitalize(Gender);

            string countryCode = string.Empty;

            //To validate province code
            if (!string.IsNullOrWhiteSpace(ProvinceCode))
            {
                
                ProvinceCode = ProvinceCode.ToUpper();

                //fetching provinceCode from database by patientsContext
                province = patientsContext.Provinces.Where(p => p.ProvinceCode == ProvinceCode).FirstOrDefault();

                if (province != null)
                {
                    countryCode = province.CountryCode;
                }
                else
                {
                    yield return new ValidationResult("Province Code is not on file", new[] { nameof(ProvinceCode) });
                    yield return new ValidationResult("Province Code is required to validate Postal Code",
                      new[] { nameof(PostalCode) });
                }
            }
            else
            {
                yield return ValidationResult.Success;
            }

            //To validate PostalCode
            if (string.IsNullOrWhiteSpace(PostalCode))
            {
                yield return ValidationResult.Success;
            }
            else
            {
                if (countryCode == "CA")
                {
                    //To check null or Empty
                    if (string.IsNullOrWhiteSpace(ProvinceCode))
                    {
                        //show error message
                        yield return new ValidationResult("Province Code is required to validate a Postal Code", new[] { (nameof(PostalCode)) });
                        yield return new ValidationResult("Province Code is missing", new[] { (nameof(ProvinceCode)) });

                    }
                    else
                    {
                        if (RGValidations.RGPostalCodeValidation(PostalCode))
                        {
                            if (!string.IsNullOrWhiteSpace(province.FirstPostalLetter))
                            {
                                if (province.FirstPostalLetter.Length == 1)
                                {
                                    if (PostalCode.ToLower().StartsWith(province.FirstPostalLetter.ToLower()))
                                    {
                                        PostalCode = RGValidations.RGPostalCodeFormat(PostalCode);
                                        yield return ValidationResult.Success;
                                    }
                                    else
                                    {
                                        yield return new ValidationResult("First letter of Postal Code not valid for given Province", new[] { (nameof(PostalCode)) });
                                        yield return new ValidationResult("Enter valid Postal Code as per this province", new[] { (nameof(ProvinceCode)) });

                                    }
                                }
                                else
                                {
                                    var firstLetterArray = province.FirstPostalLetter.ToCharArray()
                                        .Select(p => p.ToString()).ToArray();
                                    if (firstLetterArray.Any(p => PostalCode[0].ToString().Contains(p)))
                                    {
                                        PostalCode = RGValidations.RGPostalCodeFormat(PostalCode);
                                        yield return ValidationResult.Success;
                                    }
                                    else
                                    {
                                        yield return new ValidationResult("First Letter of Postal Code is invalid as per given Province", new[] { (nameof(PostalCode)) });
                                        yield return new ValidationResult("Enter valid Postal Code as per this province", new[] { (nameof(ProvinceCode)) });
                                    }
                                }

                            }

                        }
                        else
                        {
                            yield return new ValidationResult("Postal Code not match cdn pattern: A3A 3A3", new[] { (nameof(PostalCode)) });
                        }

                    }
                }

            }

            //US ZipCode Validation
            if (countryCode == "US")
            {
                string zipCode = PostalCode;
                if (RGValidations.RGZipCodeValidation(ref zipCode))
                {
                    PostalCode = zipCode;

                }
                else
                {
                    if (PostalCode.Length != 5 || PostalCode.Length != 9)
                    {
                        yield return new ValidationResult("American zip code should be either 5 or 9 digits.",
                                 new[] { nameof(PostalCode) });
                    }
                }
            }

            //Ohip regex for validation
            Regex OhipPattern = new Regex(@"^\d\d\d\d-\d\d\d-\d\d\d-[A-Z][A-Z]$", RegexOptions.IgnoreCase);
            if (Ohip == null || OhipPattern.IsMatch(Ohip.ToString()) || Ohip.ToString() == "")
            {
                Ohip = Ohip.ToUpper();
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult("Ohip if provided, must match pattern: 1234-123-123-XX", new[] { (nameof(Ohip)) });
            }

            //HomePhone Validation Regex
            if (!string.IsNullOrWhiteSpace(HomePhone))
            {
                if (HomePhone.Length == 10)
                {
                    HomePhone = HomePhone.Substring(0, 3) + "-" + HomePhone.Substring(3, 3) + "-" + HomePhone.Substring(6, 4);
                    yield return ValidationResult.Success;
                }
                else
                {
                    yield return new ValidationResult("Home Phone, if provided must be 10 digits", new[] { (nameof(HomePhone)) });

                }
            }
            else
            {
                yield return ValidationResult.Success;
            }

            DateTime todaysDate = DateTime.Now;

            //Date of birth validation

            if (DateOfBirth == null)
            {
                yield return ValidationResult.Success;

            }
            else
            {
                if (DateOfBirth.Value > todaysDate)
                {
                    yield return new ValidationResult("Date of birth cannot be in the future", new[] { (nameof(DateOfBirth)) });
                }
                else
                {
                    yield return ValidationResult.Success;
                }
            }

            //validation for date of birth as deceased is true or false
            if (Deceased == true)
            {
                if (DateOfDeath == null)
                {
                    yield return new ValidationResult("DateOfDeath is required, if deceased is true.",
                     new[] { nameof(DateOfDeath) });
                }
                else
                {
                    if (DateOfDeath.Value > todaysDate)
                    {
                        yield return new ValidationResult("Date of Death cannot be in the future", new[] { (nameof(DateOfDeath)) });
                    }
                    else
                    {
                        yield return ValidationResult.Success;
                    }

                    if (DateOfDeath.Value > DateOfBirth.Value)
                    {
                        yield return ValidationResult.Success;
                    }
                    else
                    {
                        yield return new ValidationResult("Date of Death cannot be before than Date of Birth (if it is provided)", new[] { (nameof(DateOfDeath)) });
                    }
                }
            }
            else
            {
                if (DateOfDeath == null)
                {
                    yield return ValidationResult.Success;
                }
                else
                {
                    yield return new ValidationResult("DateOfDeath is required, if deceased is true.",
                     new[] { nameof(DateOfDeath) });
                    yield return new ValidationResult("DateOfDeath is should remain empty, if deceased is unchecked.",
                     new[] { nameof(DateOfDeath) });
                }
            }

            //Gender Validation
            string[] genderLetters = { "M", "F", "X" };

            if (!genderLetters.Any(g => Gender.Contains(g)))
            {
                yield return new ValidationResult("Gender must be M, F or X",
                     new[] { nameof(Gender) });
            }

            yield return ValidationResult.Success;
        }
    }

    public class RGPatientMetadata
    {
        /// <summary>
        /// random patient number
        /// </summary>
        public int PatientId { get; set; }
        /// <summary>
        /// patient&apos;s first or given name
        /// </summary>
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// patient&apos;s surname or family name
        /// </summary>
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; } = null!;
        /// <summary>
        /// street address
        /// </summary>
        [Display(Name = "Street Address")]
        public string? Address { get; set; }
        /// <summary>
        /// city
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// 2-character province code
        /// </summary>
        [Display(Name = "Province Code")]
        public string? ProvinceCode { get; set; }
        /// <summary>
        /// postal code: A9A 9A9
        /// </summary>
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }
        /// <summary>
        /// 12-character provincial medical
        /// </summary>
        [Display(Name = "OHIP")]
        public string? Ohip { get; set; }
        /// <summary>
        /// date of birth
        /// </summary>
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// if yes, date of death required else, ignore date of death
        /// </summary>

        public bool Deceased { get; set; }
        /// <summary>
        /// date of death (null if alive)
        /// </summary>
        [Display(Name = "Date of Death")]
        public DateTime? DateOfDeath { get; set; }
        /// <summary>
        /// 10-digit home phone number
        /// </summary>
        [Display(Name = "Home Phone")]
        public string? HomePhone { get; set; }
        /// <summary>
        /// M or F
        /// </summary>
        [Required]
        public string? Gender { get; set; }

        public virtual Province? ProvinceCodeNavigation { get; set; }
        public virtual ICollection<PatientDiagnosis> PatientDiagnoses { get; set; }
    }
    
}
