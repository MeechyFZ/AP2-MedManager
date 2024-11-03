﻿using MedManager.Models;
using MedManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
namespace MedManager.Controllers
{
    [Authorize]
    public class MedicamentController : Controller
	{
		private readonly ILogger<Medecin> _logger;
		private readonly ApplicationDbContext _dbContext;

		public MedicamentController(ILogger<Medecin> logger, ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task<IActionResult> Index(string Filtre)
		{

			List<Medicament> medicaments = await _dbContext.Medicaments.ToListAsync();

			if (!string.IsNullOrEmpty(Filtre))
			{
				medicaments = medicaments
					.Where(m => m.Nom.ToUpper().Contains(Filtre.ToUpper()))
					.ToList();
			}

			ViewData["FiltreActuel"] = Filtre;
			return View(medicaments);
		}


		[HttpGet]
		public async Task<IActionResult> Modifier(int id)
		{

			Medicament? medicament = await _dbContext.Medicaments.FindAsync(id);
			return View(medicament);
		}

		[HttpPost]
		public async Task<IActionResult> Modifier(Medicament model)
		{

			try
			{
				if (ModelState.IsValid)
				{
					Medicament? medicament = await _dbContext.Medicaments.FindAsync(model.MedicamentId);

					if (medicament == null)
						return NotFound();

					medicament.Nom = model.Nom;
					medicament.Quantite = model.Quantite;
					medicament.Posologie = model.Posologie;
					medicament.Composition = model.Composition;
					medicament.Categorie = model.Categorie;
					_dbContext.Entry(medicament).State = EntityState.Modified;
					await _dbContext.SaveChangesAsync();
					return RedirectToAction("Index", "Medicament");
				}
                return View(model);
            }
			catch (DbException ex)
			{
				_logger.LogError(ex, "Une erreur est apparue pendant la modification du médicament.");
				return RedirectToAction("Error");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Une erreur innatendue est survenue.");
				return RedirectToAction("Error");
			}
		}

		public async Task<IActionResult> Supprimer(int id)
		{
			try
			{
				Medicament? medicament = await _dbContext.Medicaments.FindAsync(id);

				if (medicament == null)
					return NotFound();

                _dbContext.Remove(medicament);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Medicament");
			}
			catch (DbException ex)
			{
				_logger.LogError(ex, "Une erreur est apparue pendant la suppression du médicament.");
				return RedirectToAction("Error");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Une erreur innatendue est survenue.");
				return RedirectToAction("Error");
			}
		}
		[HttpGet]
		public IActionResult Ajouter()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Ajouter(Medicament model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					await _dbContext.Medicaments.AddAsync(model);
					await _dbContext.SaveChangesAsync();
					return RedirectToAction("Index", "Medicament");
				}
                return View(model);

            }
			catch (DbException ex)
			{
				_logger.LogError(ex, "Une erreur est apparue pendant l'ajout du médicament.");
				return RedirectToAction("Error");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Une erreur innatendue est survenue.");
				return RedirectToAction("Error");
			}
		}
	}
}
