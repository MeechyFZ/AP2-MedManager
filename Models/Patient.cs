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
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 50 caractères.")]
        public string? Prenom { get; set; }

        [Range(20, 250, ErrorMessage = "La taille doit être comprise entre 20cm et 250cm")]
        [Required(ErrorMessage = "La taille est obligatoire")]
        public int? Taille { get; set; }

        [Range(1, 300, ErrorMessage = "Le poids doit être compris entre 1kg et 300kg")]
        [Required(ErrorMessage = "La taille est obligatoire")]
        public float? Poids { get; set; }


        [Required(ErrorMessage = "Le sexe est obligatoire")]
        public Sexe? Sexe { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date, ErrorMessage = "La date de naissance n'est pas valide.")]
        public DateTime? DateNaissance { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères.")]
        public string? Adresse { get; set; }

        [Required(ErrorMessage = "Le numéro de sécurité sociale est obligatoire")]
        //[RegularExpression("^[1-3][0-9]{2}(0[1-9]|1[0-2])[0-9]{2}[0-9]{3}[0-9]{3}[0-9]{2}$", ErrorMessage = "Le numéro de sécurité sociale n'est pas valide.")]
        public string? NumeroSecuriteSocial { get; set; }

        [Required(ErrorMessage = "La ville est obligatoire")]
        [StringLength(50, ErrorMessage = "La ville ne peut pas dépasser 50 caractères.")]
        public string? Ville { get; set; }

        [MaxLength(1048576, ErrorMessage = "La taille de la photo ne doit pas dépasser 1 Mo.")]
        public byte[]? Photo { get; set; }
        public string? MedecinID { get; set; }
        public Medecin? medecin { get; set; }

        public List<Allergie> Allergies { get; set; } = new();

        public List<Antecedent> Antecedents { get; set; } = new();
        public List<Ordonnance> Ordonnances { get; set; } = new();
        [DataType(DataType.Date)]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public string NomComplet
        {
            get { return $"{Prenom} {Nom}"; }
        }

        public int Age
        {
            get
            {
                if (DateNaissance.HasValue)
                {
                    var age = DateTime.Now.Year - DateNaissance.Value.Year;
                    if (DateTime.Now < DateNaissance.Value.AddYears(age))
                    {
                        age--;
                    }
                    return age;
                }
                return 0;
            }
        }
    }
}
