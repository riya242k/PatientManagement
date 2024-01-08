using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RGPatients.Models;

namespace RGPatients.Controllers
{
    public class RGMedicationController : Controller
    {
        private readonly PatientsContext _context;

        public RGMedicationController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RGMedication
        /// <summary>
        /// Call: Default action for controller which is called when a user navigates to the base URL. 
        /// Use: Save MedicationTypeId to cookies from the URL or a QueryString or cookie itself.
        /// </summary>
        /// <param name="MedicationTypeId"></param>
        /// <returns>The filtered list of Medications based on the medication type id passed from Medication type index view.</returns>
        public async Task<IActionResult> Index(int MedicationTypeId)
        {
            if (MedicationTypeId > 0)
            {
                //store MedicationTypeId in cookie or session
                Response.Cookies.Append("MedicationTypeId", MedicationTypeId.ToString());
            }
            else if (Request.Query["MedicationTypeId"].Any())
            {
                //Save MedicationTypeId in cookie or session
                MedicationTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
                Response.Cookies.Append("MedicationTypeId", MedicationTypeId.ToString());
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                MedicationTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }
            else
            {
                // show error message in case no medTypeId found in cookies or queryString
                TempData["message"] = "Please select a Medication Type";
                return RedirectToAction("Index", "RGMedicationType");
            }
            // Filter and Order the medication list by MedicationTypeId
            var patientsContext = _context.Medications.Include(m => m.ConcentrationCodeNavigation)
                                                       .Include(m => m.DispensingCodeNavigation)
                                                       .Include(m => m.MedicationType)
                                                       .Where(mt => mt.MedicationTypeId == MedicationTypeId)
                                                        .OrderBy(mt => mt.Name)
                                                        .ThenBy(mt => mt.Concentration);

            var medType = _context.MedicationTypes.Where(mt => mt.MedicationTypeId == MedicationTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            return View(await patientsContext.ToListAsync());
        }

        // GET: RGMedication/Details/5
        ///<summary>
        /// Name: Details
        /// Call: when a user clicks on the details link for a particular row in the list of records
        /// Use: To retrieve the specific record from the database based on some unique identifier such as id in this case.
        ///      Create a View bag for medication type name to display in title
        /// Name: Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for details with parameter as the requested record</returns>
        public async Task<IActionResult> Details(string id)
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                //store in cookies or session
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                //Save in cookie or session
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }

            var medType = _context.MedicationTypes.Where(x => x.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: RGMedication/Create
        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view
        /// Use: To render the create view to take user inputs and create a View bag for medication type name to display in title
        /// </summary>
        /// <returns>the default view for create</returns>
        public IActionResult Create()
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }

            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }
            else
            {
                TempData["message"] = "Select medication type to see its medications.";
                return RedirectToAction("Index", "RGMedicationType");
            }

            var medType = _context.MedicationTypes.Where(mt => mt.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(mt => mt.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(mt => mt.DispensingCode), "DispensingCode", "DispensingCode");
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId");
            return View();
        }

        // POST: RGMedication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view.
        /// Use: - To receive the values entered by user and store the record entry in the database.
        ///      - Create a View bag for medication type name to display in title
        ///      - Validate for existence of duplicate records with same name, conentration, concentration code
        ///      
        /// </summary>
        /// <param name="medication"></param>
        /// <returns>The default view for Create with parameter as user entered values in case of validation error</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }
            medication.MedicationTypeId = medTypeId;
            var medType = _context.MedicationTypes.Where(mt => mt.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            var isDuplicate = _context.Medications.Where(mt => mt.Name == medication.Name
                                                         && mt.Concentration == medication.Concentration
                                                         && mt.ConcentrationCode == medication.ConcentrationCode);
            // add error if duplicate records exist
            if (isDuplicate.Any())
            {
                ModelState.AddModelError("", "Duplicate entry found");
            }
            // In case no errors or duplicte record found
            if (ModelState.IsValid)
            {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                TempData["message"] = "Record saved successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: RGMedication/Edit/5

        /// </summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To render the edit view for record with given id to take user inputs
        ///      Create a View bag for medication type name to display in title
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for edit with parameter as the record with id equal to target id where edit was clicked</returns>
        public async Task<IActionResult> Edit(string id)
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                //store in cookies or session
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                //Save in cookie or session
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }

            var medType = _context.MedicationTypes.Where(x => x.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // POST: RGMedication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To take user input and update the value back to database for the record with matching id where edit was clicked
        ///      Create a View bag for medication type name to display in title
        /// </summary>
        /// <param name="id"></param>
        /// <param name="medication"></param>
        /// <returns>The default view for edit in case of error, with parameter as the record where user was making the updates</returns>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                //store in cookies or session
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                //Save in cookie or session
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }

            medication.MedicationTypeId = medTypeId;
            var medType = _context.MedicationTypes.Where(x => x.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            if (id != medication.Din)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: RGMedication/Delete/5

        /// <summary>
        /// Name: Delete
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To render the delete view for record with given id to display the delete button to user
        ///      Create a View bag for medication type name to display in title
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for delete, with parameter as the record that user wants to delete</returns>
        public async Task<IActionResult> Delete(string id)
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                //store in cookies or session
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                //Save in cookie or session
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }

            var medType = _context.MedicationTypes.Where(x => x.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: RGMedication/Delete/5

        /// <summary>
        /// Name: DeleteConfirmed
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To delete the target record from database with id equal to the id of record where user clicked delete
        ///      Create a View bag for medication type name to display in title
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for delete, with parameter as the record that user wants to delete</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            int medTypeId = 0;
            if (Request.Query["MedicationTypeId"].Any())
            {
                //store in cookies or session
                medTypeId = Convert.ToInt32(Request.Query["MedicationTypeId"]);
            }
            else if (Request.Cookies["MedicationTypeId"] != null)
            {
                //Save in cookie or session
                medTypeId = Convert.ToInt32(Request.Cookies["MedicationTypeId"]);
            }

            var medType = _context.MedicationTypes.Where(x => x.MedicationTypeId == medTypeId).FirstOrDefault();
            ViewBag.MTName = medType.Name;

            if (_context.Medications == null)
            {
                return Problem("Entity set 'PatientsContext.Medications'  is null.");
            }
            var medication = await _context.Medications.FindAsync(id);
            if (medication != null)
            {
                _context.Medications.Remove(medication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Name: MedicationTypeExists
        /// Call: Internally called by the actions to check if some record with given exists or not
        /// Use: Validates and tell whether a record with some particular id exist in the database or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true/false based on if record with given id exists in DB or not</returns>
        private bool MedicationExists(string id)
        {
            return _context.Medications.Any(e => e.Din == id);
        }
    }
}
