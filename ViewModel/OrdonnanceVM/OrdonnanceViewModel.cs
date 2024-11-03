﻿using MedManager.Attributs;
using MedManager.Models;
using System.ComponentModel.DataAnnotations;

namespace MedManager.ViewModel.OrdonnanceVM
{
    public class OrdonnanceViewModel
    {
        public int? OrdonnanceId { get; set; }

        [Display(Name = "Date de début")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Le date de fin est obligatoire")]
        public DateTime DateDebut { get; set; }

        [Display(Name ="Date de fin")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Le date de fin est obligatoire")]
        public DateTime DateFin { get; set; }

        [Display(Name = "Informations suplémentaires")]
        public string? InfoSupplementaire { get; set; }

        [Required]
        public required List<Medicament> Medicaments { get; set; } = new List<Medicament>();

		[RequiredListeNonVide(ErrorMessage = "Veuillez sélectionner au moins un médicament")]
		[Display(Name="Médicaments disponibles")]
        public required List<int> MedicamentIdSelectionnes { get; set; } = new List<int>();

        public byte[]? Pdf { get; set; }

		[RequiredPatient(ErrorMessage = "Veuillez sélectionner un patient")]
		[Display(Name ="Sélectionner un patient")]
        public int? PatientId { get; set; }

        public required List<Patient> patients { get; set; } = new List<Patient>();

    }
}


