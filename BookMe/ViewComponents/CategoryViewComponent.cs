using BookMe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace BookMe.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private readonly BookMeDbContext _context;
        public CategoryViewComponent(BookMeDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Index", _context.ServiceCategories.ToList());
        }
    }
}
