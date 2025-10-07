namespace Models;

public class Partie
{
    public Guid Id { get; set; } = Guid.NewGuid();// Identifiant unique de la partie
    public Guid JoueurId { get; set; }// Référence au joueur
    public List<Salle> Salles { get; set; } = new();// def du Donjon composé de plusieurs salles
    public int ScoreFinal { get; set; }// Score obtenu à la fin de la partie 
    public DateTime Date { get; set; } = DateTime.UtcNow; // Date de la partie
    public bool EstTerminee { get; set; }// True si la partie est finie
}