﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.JavnoNadmetanje
{
    /// <summary>
    /// Model za potvrdu kreiranja javnog nadmetanja
    /// </summary>
    public class JavnoNadmetanjeCreationDto : IValidatableObject
    {
        /// <summary>
        /// Pocetna cena po hektaru
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti pocetnu cenu hektara.")]
        public double PocetnaCenaHektar { get; set; }
        /// <summary>
        /// Period zakupa u godinama
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti period zakupa.")]
        public int PeriodZakupa { get; set; }
        /// <summary>
        /// Izlicitirana cena
        /// </summary>
        public int IzlicitiranaCena { get; set; }
        /// <summary>
        /// Broj ucesnika na javnom nadmetanju
        /// </summary>
        public int BrojUcesnika { get; set; }
        /// <summary>
        /// Krug po redu
        /// </summary>
        public int Krug { get; set; }
        /// <summary>
        /// Da li je javno nadmetanje izuzeto
        /// </summary>
        public bool Izuzeto { get; set; }
        /// <summary>
        /// ID statusa javnog nadmetanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id statusa javnog nadmetanja.")]
        public Guid StatusId { get; set; }
        /// <summary>
        /// ID tipa javnog nadmetanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id tipa javnog nadmetanja.")]
        public Guid TipId { get; set; }

        /// <summary>
        /// ID najboljeg kupca javnog nadmetanja
        /// </summary>
        public Guid? KupacId { get; set; }
        /// <summary>
        /// ID mesta odrzavanja javnog nadmetanja
        /// </summary>
        public Guid? AdresaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IzlicitiranaCena < PocetnaCenaHektar)
            {
                yield return new ValidationResult(
                     "Izlicitirana cena mora biti veca od pocetne cene!",
                     new[] { "JavnoNadmetanjeCreationDto" });
            }
        }
    }
}
