
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMvcApp.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // Client List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();

            return View(clients);
        }

        // Create - GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            client.CreatedAt = DateTime.UtcNow;

            await _clientService.AddAsync(client);

            TempData["SuccessMessage"] =
                "Client added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Edit - GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Client client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            await _clientService.UpdateAsync(client);

            TempData["SuccessMessage"] =
                "Client updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteAsync(id);

            TempData["SuccessMessage"] =
                "Client deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}

