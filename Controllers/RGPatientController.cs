using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RGPatients.Models;

namespace RGPatients.Controllers
{
    public class RGPatientController : Controller
    {
        private readonly PatientsContext _context;

        public RGPatientController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RGPatient
        public async Task<IActionResult> Index()
        {
            var patientsContext = _context.Patients.Include(p => p.ProvinceCodeNavigation)
                                                   .OrderBy(p => p.LastName)
                                                   .ThenBy(x => x.FirstName);
            return View(await patientsContext.ToListAsync());
        }

        // GET: RGPatient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: RGPatient/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Provinces, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: RGPatient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view
        /// Use: To render the create view to take user inputs and create a View bag for patient name and diagnosis name to display in title
        /// <param name="patient"></param>
        /// </summary>
        /// <returns>the default view for create</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,FirstName,LastName,Address,City,ProvinceCode,PostalCode,Ohip,DateOfBirth,Deceased,DateOfDeath,HomePhone,Gender")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Patient record is created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Error inserting new patient's record: {e.GetBaseException().Message}");
            }

            ViewData["ProvinceCode"] = new SelectList(_context.Provinces, "ProvinceCode", "ProvinceCode", patient.ProvinceCode);
            return View(patient);
        }

        // GET: RGPatient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Provinces.OrderBy(p => p.Name), "ProvinceCode", "Name", patient.ProvinceCode);
            return View(patient);
        }

        // POST: RGPatient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To render the edit view for record with given id to take user inputs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patient"></param>
        /// <returns>The default view for edit with parameter as the record with id equal to target id where edit was clicked</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,FirstName,LastName,Address,City,ProvinceCode,PostalCode,Ohip,DateOfBirth,Deceased,DateOfDeath,HomePhone,Gender")] Patient patient)
        {
            try
            {
                if (id != patient.PatientId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(patient);
                        await _context.SaveChangesAsync();
                        TempData["message"] = "Patient record updated successfully";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PatientExists(patient.PatientId))
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
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error occured while updating the patient record", e.GetBaseException().Message);
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Provinces.OrderBy(p => p.Name), "ProvinceCode", "Name", patient.ProvinceCode);
            return View(patient);
        }

        // GET: RGPatient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: RGPatient/Delete/5
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
            try
            {
                if (_context.Patients == null)
                {
                    return Problem("Entity set 'PatientsContext.Patients'  is null.");
                }

                var patient = await _context.Patients.FindAsync(id);
                if (patient != null)
                {
                    _context.Patients.Remove(patient);
                }


                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                TempData["message"] = "Error occured while deleting the patient record";
            }
            TempData["message"] = "Patient record deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
