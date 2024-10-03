﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedManager.Models
{
    public enum Sexe { Homme, Femme, Autre }

    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 50 caractères.")]
        public required string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 50 caractères.")]
        public required string Prenom { get; set; }

        [Required(ErrorMessage = "Le numéro de sécurité sociale est obligatoire")]
        [RegularExpression("^[1-3][0-9]{2}(0[1-9]|1[0-2])[0-9]{2}[0-9]{3}[0-9]{3}[0-9]{2}$", ErrorMessage = "Le numéro de sécurité sociale n'est pas valide.")]
        public required string NuméroSécuritéSociale { get; set; }

        [Required(ErrorMessage = "Le sexe est obligatoire")]
        public required Sexe Sexe { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date, ErrorMessage = "La date de naissance n'est pas valide.")]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères.")]
        public required string Adresse { get; set; }

        [Required(ErrorMessage = "La ville est obligatoire")]
        [StringLength(50, ErrorMessage = "La ville ne peut pas dépasser 50 caractères.")]
        public required string Ville { get; set; }
        public string? MedecinID { get; set; }
        public Medecin? Medecin { get; set; }

        public List<Allergie> Allergies { get; set; } = new List<Allergie>();

        public List<Antecedent> Antecedents { get; set; } = new List<Antecedent>();
        public List<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
    }
}
