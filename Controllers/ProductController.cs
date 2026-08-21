using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMvcApp.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly IProductService _service;

    public ProductController(IProductService service) => _service = service;

    public async Task<IActionResult> Index() =>
        View(await _service.GetAllAsync());

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new ProductViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _service.CreateAsync(model);
        TempData["SuccessMessage"] = "Product saved successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product is null) return NotFound();

        return View(new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(ProductViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (!await _service.UpdateAsync(model)) return NotFound();

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id)) return NotFound();

        TempData["SuccessMessage"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
