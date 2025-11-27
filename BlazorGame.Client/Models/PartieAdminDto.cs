using System;
using System.Collections.Generic;

namespace BlazorGame.Client.Models;

public class PartieAdminDto
{
    public Guid Id { get; set; }
    public Guid JoueurId { get; set; }
    public string JoueurNom { get; set; } = string.Empty;
    public string DonjonNom { get; set; } = string.Empty;
    public int ScoreFinal { get; set; }
    public bool EstTerminee { get; set; }
    public DateTime Date { get; set; }
    public List<SalleAdminDto> Salles { get; set; } = new();
}

public class SalleAdminDto
{
    public Guid Id { get; set; }
    public int Position { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Niveau { get; set; } = string.Empty;
    public bool EstVisitee { get; set; }
    public string NomMonstre { get; set; } = string.Empty;
    public int PvMonstre { get; set; }
    public int ForceMonstre { get; set; }
    public string? ChoixFait { get; set; }
    public int Points { get; set; }
}
