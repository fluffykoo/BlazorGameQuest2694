namespace Models;

public class Joueur
{

    public Guid Id { get; set; } = Guid.NewGuid();// identifiant unique
    public string Nom { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int ScoreTotal { get; set; }
    public List<Partie> Historique { get; set; } = new();//historique des parties
    public DateTime DerniereConnexion { get; set; } = DateTime.UtcNow;
    public bool PeutReprendrePartie { get; set; } //True si une partie est en cours
}

