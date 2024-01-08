using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RGPatients.Models;

namespace RGPatients.Controllers
{
    public class RGPatientTreatmentController : Controller
    {
        private readonly PatientsContext _context;

        public RGPatientTreatmentController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RGPatientTreatment
        /// <summary>
        /// Call: Default action for controller which is called when a user navigates to the base URL. 
        /// Use: Save PatientDiagnosisId to cookies from the URL or a QueryString or cookie itself.
        ///      Create a View bag for patient name and diagnosis name to display in title
        /// </summary>
        /// <param name="PatientDiagnosisId"></param>
        /// <param name="PatientName"></param>
        /// <param name="DiagnosisName"></param>
        /// <returns>The filtered list of patient treatments based on the PatientDiagnosisId passed from Patient Diagnosis index view.</returns>
        public async Task<IActionResult> Index(int PatientDiagnosisId, String DiagnosisName, String PatientName)
        {

            if (PatientDiagnosisId > 0)
            {
                //store MedicationTypeId in cookie or session
                Response.Cookies.Append("PatientDiagnosisId", PatientDiagnosisId.ToString());
            }
            else if (Request.Query["PatientDiagnosisId"].Any())
            {
                //Save MedicationTypeId in cookie or session
                PatientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
                Response.Cookies.Append("PatientDiagnosisId", PatientDiagnosisId.ToString());
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }
            else
            {
                // show error message in case no medTypeId found in cookies or queryString
                TempData["message"] = "Please select a Patient's Diagnosis";
                return RedirectToAction("Index", "RGPatientDiagnosis");
            }

            var patientsContext = _context.PatientTreatments.Include(p => p.PatientDiagnosis)
                                                            .Include(p => p.Treatment)
                                                            .Where(p => p.PatientDiagnosisId == PatientDiagnosisId)
                                                            .OrderByDescending(p => p.DatePrescribed);

            if ((!String.IsNullOrEmpty(DiagnosisName)) && (!String.IsNullOrEmpty(PatientName)))
            {
                ViewBag.DiagnosisName = DiagnosisName;
                ViewBag.PatientName = PatientName;
            }
            else
            {
                var patientDiagnoses = _context.PatientDiagnoses.Include(p => p.Diagnosis)
                                                                .Include(p => p.Patient)
                                                                .Where(p => p.PatientDiagnosisId == PatientDiagnosisId)
                                                                .FirstOrDefault();

                ViewBag.PatientName = patientDiagnoses.Patient.LastName + ", " + patientDiagnoses.Patient.FirstName;
                ViewBag.DiagnosisName = patientDiagnoses.Diagnosis.Name;
            }

            return View(await patientsContext.ToListAsync());
        }

        // GET: RGPatientTreatment/Details/5
        ///<summary>
        /// Name: Details
        /// Call: when a user clicks on the details link for a particular row in the list of records
        /// Use: To retrieve the specific record from the database based on some unique identifier such as id in this case.
        ///      Create a View bag for patient name and diagnosis name to display in title
        /// Name: Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="PatientName"></param>
        /// <param name="DiagnosisName"></param>
        /// <returns>The default view for details with parameter as the requested record</returns>
        public async Task<IActionResult> Details(int? id, String PatientName, String DiagnosisName)
        {
            int patientDiagnosisId = 0;
            if (Request.Query["PatientDiagnosisId"].Any())
            {
                //store in cookies or session
                patientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }

            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);

            if (patientTreatment == null)
            {
                return NotFound();
            }

            if ((!String.IsNullOrEmpty(DiagnosisName)) && (!String.IsNullOrEmpty(PatientName)))
            {
                ViewBag.DiagnosisName = DiagnosisName;
                ViewBag.PatientName = PatientName;
            }
            else
            {
                var patientDiagnoses = _context.PatientDiagnoses.Include(p => p.Diagnosis)
                                                                .Include(p => p.Patient)
                                                                .Where(p => p.PatientDiagnosisId == patientDiagnosisId)
                                                                .FirstOrDefault();

                ViewBag.PatientName = patientDiagnoses.Patient.LastName + ", " + patientDiagnoses.Patient.FirstName;
                ViewBag.DiagnosisName = patientDiagnoses.Diagnosis.Name;

            }

            return View(patientTreatment);
        }

        // GET: RGPatientTreatment/Create
        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view
        /// Use: To render the create view to take user inputs and create a View bag for patient name and diagnosis name to display in title
        /// <param name="PatientName"></param>
        ///<param name = "DiagnosisName" ></ param >
        /// </summary>
        /// <returns>the default view for create</returns>
        public IActionResult Create(String PatientName, String DiagnosisName)
        {
            int patientDiagnosisId = 0;
            if (Request.Query["PatientDiagnosisId"].Any())
            {
                //store in cookies or session
                patientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }
            else
            {
                // show error message in case no medTypeId found in cookies or queryString
                TempData["message"] = "Please select a Patient's Diagnosis";
                return RedirectToAction("Index", "RGPatientDiagnosis");
            }
            if ((!String.IsNullOrEmpty(DiagnosisName)) && (!String.IsNullOrEmpty(PatientName)))
            {
                ViewBag.DiagnosisName = DiagnosisName;
                ViewBag.PatientName = PatientName;
            }
            else
            {
                var patientDiagnoses = _context.PatientDiagnoses.Include(p => p.Diagnosis)
                                                                .Include(p => p.Patient)
                                                                .Where(p => p.PatientDiagnosisId == patientDiagnosisId)
                                                                .FirstOrDefault();

                ViewBag.PatientName = patientDiagnoses.Patient.LastName + ", " + patientDiagnoses.Patient.FirstName;
                ViewBag.DiagnosisName = patientDiagnoses.Diagnosis.Name;
            }

            int diagnosisId = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == patientDiagnosisId).FirstOrDefault().DiagnosisId;

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == diagnosisId), "TreatmentId", "Name");
            return View();
        }

        // POST: RGPatientTreatment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view.
        /// Use: - To receive the values entered by user and store the record entry in the database.
        ///      - Create a View bag for patient name and diagnosis name to display in title
        /// </summary>
        /// <param name="patientTreatment"></param>
        /// <returns>The default view for Create with parameter as user entered values in case of validation error</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            int patientDiagnosisId = 0;
            if (Request.Query["PatientDiagnosisId"].Any())
            {
                //store in cookies or session
                patientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }

            patientTreatment.PatientDiagnosisId = patientDiagnosisId;

            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            int diagnosisId = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == patientDiagnosisId).FirstOrDefault().DiagnosisId;

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == diagnosisId), "TreatmentId", "Name");
            return View(patientTreatment);
        }

        // GET: RGPatientTreatment/Edit/5

        /// <summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To render the edit view for record with given id to take user inputs
        ///      Create a View bag for patient name and diagnosis name to display in title
        /// </summary>
        /// <param name="id"></param>
        /// <param name="PatientName"></param>
        ///<param name = "DiagnosisName" ></ param >
        /// <returns>The default view for edit with parameter as the record with id equal to target id where edit was clicked</returns>
        public async Task<IActionResult> Edit(int? id, String PatientName, String DiagnosisName)
        {
            int patientDiagnosisId = 0;
            if (Request.Query["PatientDiagnosisId"].Any())
            {
                //store in cookies or session
                patientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }
            if ((!String.IsNullOrEmpty(DiagnosisName)) && (!String.IsNullOrEmpty(PatientName)))
            {
                ViewBag.DiagnosisName = DiagnosisName;
                ViewBag.PatientName = PatientName;
            }
            else
            {
                var patientDiagnoses = _context.PatientDiagnoses.Include(p => p.Diagnosis)
                                                                .Include(p => p.Patient)
                                                                .Where(p => p.PatientDiagnosisId == patientDiagnosisId)
                                                                .FirstOrDefault();

                ViewBag.PatientName = patientDiagnoses.Patient.LastName + ", " + patientDiagnoses.Patient.FirstName;
                ViewBag.DiagnosisName = patientDiagnoses.Diagnosis.Name;
            }


            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments.FindAsync(id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            int diagnosisId = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == patientDiagnosisId).FirstOrDefault().DiagnosisId;


            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == diagnosisId), "TreatmentId", "Name");
            return View(patientTreatment);
        }

        // POST: RGPatientTreatment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To take user input and update the value back to database for the record with matching id where edit was clicked
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patientTreatment"></param>
        /// <returns>The default view for edit in case of error, with parameter as the record where user was making the updates</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {

            int patientDiagnosisId = 0;
            if (Request.Query["PatientDiagnosisId"].Any())
            {
                //store in cookies or session
                patientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }

            patientTreatment.PatientDiagnosisId = patientDiagnosisId;

            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            int diagnosisId = _context.PatientDiagnoses.Where(p => p.PatientDiagnosisId == patientDiagnosisId).FirstOrDefault().DiagnosisId;

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnoses, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == diagnosisId), "TreatmentId", "Name");
            return View(patientTreatment);
        }

        // GET: RGPatientTreatment/Delete/5

        /// <summary>
        /// Name: Delete
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To render the delete view for record with given id to display the delete button to user
        ///      Create a View bag for patient name and diagnosis name to display in title
        /// </summary>
        /// <param name="id"></param>
        /// <param name="PatientName"></param>
        ///<param name = "DiagnosisName" ></ param >
        /// <returns>The default view for delete, with parameter as the record that user wants to delete</returns>
        public async Task<IActionResult> Delete(int? id, String PatientName, String DiagnosisName)
        {

            int patientDiagnosisId = 0;
            if (Request.Query["PatientDiagnosisId"].Any())
            {
                //store in cookies or session
                patientDiagnosisId = Convert.ToInt32(Request.Query["PatientDiagnosisId"]);
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                patientDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }

            if ((!String.IsNullOrEmpty(DiagnosisName)) && (!String.IsNullOrEmpty(PatientName)))
            {
                ViewBag.DiagnosisName = DiagnosisName;
                ViewBag.PatientName = PatientName;
            }
            else
            {
                var patientDiagnoses = _context.PatientDiagnoses.Include(p => p.Diagnosis)
                                                                .Include(p => p.Patient)
                                                                .Where(p => p.PatientDiagnosisId == patientDiagnosisId)
                                                                .FirstOrDefault();

                ViewBag.PatientName = patientDiagnoses.Patient.LastName + ", " + patientDiagnoses.Patient.FirstName;
                ViewBag.DiagnosisName = patientDiagnoses.Diagnosis.Name;
            }

            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: RGPatientTreatment/Delete/5

        /// <summary>
        /// Name: DeleteConfirmed
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To delete the target record from database with id equal to the id of record where user clicked delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for delete, with parameter as the record that user wants to delete</returns>

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientTreatments == null)
            {
                return Problem("Entity set 'PatientsContext.PatientTreatments'  is null.");
            }
            var patientTreatment = await _context.PatientTreatments.FindAsync(id);
            if (patientTreatment != null)
            {
                _context.PatientTreatments.Remove(patientTreatment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Name: PatientTreatmentExists
        /// Call: Internally called by the actions to check if some record with given exists or not
        /// Use: Validates and tell whether a record with some particular id exist in the database or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true/false based on if record with given id exists in DB or not</returns>
        private bool PatientTreatmentExists(int id)
        {
            return _context.PatientTreatments.Any(e => e.PatientTreatmentId == id);
        }
    }
}
