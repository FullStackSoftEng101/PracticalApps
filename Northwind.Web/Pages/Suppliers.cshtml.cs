using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography.X509Certificates;
using Northwind.EntityModels; // To use NorthwindContext.



namespace Northwind.Web.Pages
{
    public class SuppliersModel : PageModel
    {
        private NorthwindContext _db;
        [BindProperty]
        public Supplier? Supplier { get; set; }

        public SuppliersModel(NorthwindContext db)
        {
            _db = db;
        }
            
        //public IEnumerable<string>? Suppliers { get; set; }
        public IEnumerable<Supplier>? Suppliers { get; set; }
        public string? DayName { get; set; }
        public string? MonthName { get; set; }
        public int? YearNumber { get; set; }
        

        public void OnGet()
        {
            ViewData["Title"] = "ThinkCloud - Suppliers Page";
            /*Suppliers = new[]
            {
                "Acme Inc", "Alpha Co", "Beta Limited", "Gamma Corp"
            };*/
            Suppliers = _db.Suppliers.OrderBy(c => c.Country).ThenBy(c => c.CompanyName);
            ViewData["MonthName"] = DateTime.Now.Month.ToString();
            ViewData["YearNumber"] = DateTime.Now.Year;
        }
        public IActionResult OnPost()
        {
            if (Supplier is not null && ModelState.IsValid)
            {
                _db.Suppliers.Add(Supplier);
                _db.SaveChanges();
                    return RedirectToPage("/suppliers");
            }
            else
            {
                return Page(); // Return to original page.
            }
        }
    }
}
