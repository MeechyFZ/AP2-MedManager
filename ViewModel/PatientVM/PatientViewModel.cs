﻿using MedManager.Models;


namespace MedManager.ViewModel.PatientVM
{
    public class PatientViewModel
    {
        public Patient? patient { get; set; }
        public List<Antecedent>? Antecedents { get; set; }
        public List<Allergie>? Allergies { get; set; }
        public List<int> SelectedAntecedentIds { get; set; } = new List<int>();
        public List<int> SelectedAllergieIds { get; set; } = new List<int>();

        public required string IdMedecin { get; set; }
    }
}
