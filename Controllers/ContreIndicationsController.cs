﻿using Microsoft.AspNetCore.Mvc;
using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers
{
    public class ContreIndicationsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<Patient> _logger;

        public ContreIndicationsController(ApplicationDbContext dbContext, ILogger<Patient> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string searchStringAllergies, string searchStringAntecedents)
        {
            List<Allergie> allergies = await _dbContext.Allergies.ToListAsync();
            List<Antecedent> antecedents = await _dbContext.Antecedents.ToListAsync();

	
			if (!string.IsNullOrEmpty(searchStringAllergies))
			{
				allergies = allergies.Where(a => a.Nom.ToUpper().Contains(searchStringAllergies.ToUpper())).ToList();
			}

			
			if (!string.IsNullOrEmpty(searchStringAntecedents))
			{
				antecedents = antecedents.Where(a => a.Nom.ToUpper().Contains(searchStringAntecedents.ToUpper())).ToList();
			}

			var model = (Allergies: allergies, Antecedents: antecedents);
			ViewData["CurrentFilterAllergie"] = searchStringAllergies;
			ViewData["CurrentFilterAntecedent"] = searchStringAntecedents;

			return View(model);
        }

        public async Task<IActionResult> Ajouter(string type)
        {
            List<Medicament> medicaments = await _dbContext.Medicaments.ToListAsync();
            var viewModel = new ContreIndicationViewModel {
                Medicaments = medicaments,
                Type = type
            };
            return View("Action", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Ajouter(ContreIndicationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Type == "Allergie")
                {

                    Allergie allergie = new Allergie
                    {
                        Nom = viewModel.Nom
                    };

                    if (viewModel.SelectedMedicamentIds != null)
                    {
                        var MedicamentSelectionnes = await _dbContext.Medicaments
                                .Where(m => viewModel.SelectedMedicamentIds.Contains(m.MedicamentId))
                                .ToListAsync();
                        foreach (var medicament in MedicamentSelectionnes)
                        {
                            allergie.Medicaments.Add(medicament);
                        }
                    }

                    await _dbContext.Allergies.AddAsync(allergie);

                }
                else if (viewModel.Type == "Antecedent")
                {
                    Antecedent antecedent = new Antecedent
                    {
                        Nom = viewModel.Nom
                    };

                    if (viewModel.SelectedMedicamentIds != null)
                    {
                        var MedicamentSelectionnes = await _dbContext.Medicaments
                                .Where(m => viewModel.SelectedMedicamentIds.Contains(m.MedicamentId))
                                .ToListAsync();
                        foreach (var medicament in MedicamentSelectionnes)
                        {
                            antecedent.Medicaments.Add(medicament);
                        }
                    }
                    await _dbContext.Antecedents.AddAsync(antecedent);
                }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Modifier(int id, string type)
        {
            List<Medicament> medicaments = await _dbContext.Medicaments.ToListAsync();
            if (type == "Allergie")
            {
                var allergie = await _dbContext.Allergies
                    .Include(a => a.Medicaments)
                    .FirstOrDefaultAsync(a => a.AllergieId == id);
                if (allergie == null)
                    return NotFound();
                var viewModel = new ContreIndicationViewModel
                {
                    Id = allergie.AllergieId,
                    Nom = allergie.Nom,
                    Type = "Allergie",
                    Medicaments = medicaments, 
                    SelectedMedicamentIds =  allergie.Medicaments.Select(m => m.MedicamentId).ToList() ?? new List<int>()
                };
                return View("Action", viewModel);
            }
            else if (type == "Antecedent")
            {
                var antecedent = await _dbContext.Antecedents
                        .Include(a => a.Medicaments)
                        .FirstOrDefaultAsync(a => a.AntecedentId == id);
                if (antecedent == null)
                    return NotFound();
                var viewModel = new ContreIndicationViewModel
                {
                    Id = antecedent.AntecedentId,
                    Nom = antecedent.Nom,
                    Type = "Antecedent",
                    Medicaments = medicaments,
                    SelectedMedicamentIds = antecedent.Medicaments.Select(m => m.MedicamentId).ToList() ?? new List<int>()
                };
                return View("Action", viewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Modifier(ContreIndicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (model.Type == "Allergie")
                    {
                        var allergie = await _dbContext.Allergies
                                .Include(a => a.Medicaments)
                                .FirstOrDefaultAsync(a => a.AllergieId == model.Id);

                        if (allergie == null)
                            return NotFound();
                        allergie.Nom = model.Nom;

                        allergie.Medicaments.Clear();
                        if (model.SelectedMedicamentIds != null)
                        {
                            var MedicamentSelectionnes = await _dbContext.Medicaments
                                    .Where(m => model.SelectedMedicamentIds.Contains(m.MedicamentId))
                                    .ToListAsync();
                            foreach (var medicament in MedicamentSelectionnes)
                            {
                                allergie.Medicaments.Add(medicament);
                            }
                        }
                        _dbContext.Entry(allergie).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                   
                }
                else if (model.Type == "Antecedent")
                {
                    var antecedent = await _dbContext.Antecedents
                        .Include(a => a.Medicaments)
                        .FirstOrDefaultAsync(a => a.AntecedentId == model.Id);
                    if (antecedent == null)
                        return NotFound();
                    antecedent.Nom = model.Nom;

                    antecedent.Medicaments.Clear();
                    if (model.SelectedMedicamentIds != null)
                    {
                        var MedicamentSelectionnes = await _dbContext.Medicaments
                                .Where(m => model.SelectedMedicamentIds.Contains(m.MedicamentId))
                                .ToListAsync();
                        foreach (var medicament in MedicamentSelectionnes)
                        {
                            antecedent.Medicaments.Add(medicament);
                        }
                    }
                    _dbContext.Entry(antecedent).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            return View("Action", model);
        }

        public async Task<IActionResult> Supprimer(int id, string type)
        {
            if (type == "Allergie")
            {
                var allergie = await _dbContext.Allergies.FindAsync(id);
                if (allergie != null)
                    _dbContext.Allergies.Remove(allergie);
            }
            else if (type == "Antecedent")
            {
                var antecedent = await _dbContext.Antecedents.FindAsync(id);
                if (antecedent != null)
                    _dbContext.Antecedents.Remove(antecedent);
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
