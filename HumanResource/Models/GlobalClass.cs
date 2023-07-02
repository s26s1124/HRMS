using HumanResource.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Models
{
   public class GlobalClass
    {
        private readonly hrmsContext _context;

        public GlobalClass(hrmsContext context)
        {
            _context = context;
        }


        public bool StrCatCodeExists(int id)
        {
            return (_context.StrCategoryCodes?.Any(e => e.CategoryCode == id)).GetValueOrDefault();
        }




        public bool StrCatDetExists(int id)
        {
            return (_context.StrItemCodes?.Any(e => e.ItemCode == id)).GetValueOrDefault();
        }


     

    }
}
