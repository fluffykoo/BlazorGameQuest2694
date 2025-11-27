using System;

namespace BlazorGame.Client.Models;

public class ClassementEntry
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int ScoreTotal { get; set; }
    public int PartiesTerminees { get; set; }
    public DateTime DerniereConnexion { get; set; }
}
