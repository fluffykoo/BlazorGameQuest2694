using System;

namespace BlazorGame.Client.Services;

public class PlayerState
{
    public Guid? CurrentJoueurId { get; private set; }
    public string? CurrentJoueurNom { get; private set; }

    public void SetPlayer(Guid id, string nom)
    {
        CurrentJoueurId = id;
        CurrentJoueurNom = nom;
    }

    public void Clear() => SetPlayer(Guid.Empty, string.Empty);
}
