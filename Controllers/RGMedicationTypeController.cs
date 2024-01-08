using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGPatients.Models;

namespace RGPatients.Controllers
{
    public class RGMedicationTypeController : Controller
    {
        private readonly PatientsContext _context;

        public RGMedicationTypeController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RGMedicationType
        /// <summary>
        /// Name: Index
        /// Call: Default action for controller which is called when a user navigates to the base URL. 
        /// Use: It fetches the list of all records from database and pass it to the user view. Here the returned records are sorted based on name
        /// </summary>
        /// <returns>The default view for home with parameter as the list of records fetched from the database</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicationTypes.OrderBy(mt => mt.Name).ToListAsync());
        }

        // GET: RGMedicationType/Details/5
        /// <summary>
        /// Name: Details
        /// Call: when a user clicks on the details link for a particular row in the list of records
        /// Use: To retrieve the specific record from the database based on some unique identifier such as id in this case
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for details with parameter as the requested record</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // GET: RGMedicationType/Create
        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view
        /// Use: To render the create view to take user inputs
        /// </summary>
        /// <returns>the default view for create</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: RGMedicationType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Name: Create
        /// Call: When user clicks on the create new link on the view.
        /// Use: To receive the values entered by user and store the record entry in the database.
        /// </summary>
        /// <param name="medicationType"></param>
        /// <returns>The default view for Create with parameter as user entered values in case of validation error</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        // GET: RGMedicationType/Edit/5
        /// </summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: to render the edit view for record with given id to take user inputs
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for edit with parameter as the record with id equal to target id where edit was clicked</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        // POST: RGMedicationType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Name: Edit
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: to take user input and update the value back to database for the record with matching id where edit was clicked
        /// </summary>
        /// <param name="id"></param>
        /// <param name="medicationType"></param>
        /// <returns>The default view for edit in case of error, with parameter as the record where user was making the updates</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }

        // GET: RGMedicationType/Delete/5
        /// <summary>
        /// Name: Delete
        /// Call: When user clicks on the edit link for some record in the list
        /// Use: To render the delete view for record with given id to display the delete button to user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The default view for delete, with parameter as the record that user wants to delete</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // POST: RGMedicationType/Delete/5
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
            if (_context.MedicationTypes == null)
            {
                return Problem("Entity set 'PatientsContext.MedicationTypes'  is null.");
            }
            var medicationType = await _context.MedicationTypes.FindAsync(id);
            if (medicationType != null)
            {
                _context.MedicationTypes.Remove(medicationType);
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
        private bool MedicationTypeExists(int id)
        {
            return _context.MedicationTypes.Any(e => e.MedicationTypeId == id);
        }
    }
}
