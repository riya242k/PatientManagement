using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RGPatients.Models
{
    [ModelMetadataTypeAttribute(typeof(PatientTreatmentMetadata))]
    public partial class PatientTreatment
    {

    }
    public class PatientTreatmentMetadata
    {
        /// <summary>
        /// random key for treatment on this patient
        /// </summary>
        public int PatientTreatmentId { get; set; }
        /// <summary>
        /// link back to treatment
        /// </summary>
        public int TreatmentId { get; set; }
        /// <summary>
        /// date treatment prescribed to patient
        /// </summary>

        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DatePrescribed { get; set; }
        /// <summary>
        /// general free-form comments about treatment
        /// </summary>
        public string? Comments { get; set; }
        public int PatientDiagnosisId { get; set; }
    }
}
