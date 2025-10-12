# BlazorGameQuest

## Description
BlazorGameQuest est un jeu d’aventure développé avec **.NET 8 / C#** et **Blazor WebAssembly**.  
Le joueur explore des donjons générés aléatoirement, fait des choix (combattre, fuir, fouiller) et marque des points.  
Un administrateur gère les joueurs et les scores via une interface dédiée.

---

## Structure du projet
| Projet | Rôle |
|--------|------|
| **BlazorGame.Client** | Frontend Blazor WebAssembly (pages, navigation, composant Salle). |
| **AuthenticationServices** | Future API d’authentification (Keycloak). |
| **Models** | Classes métiers (Joueur, Partie, Salle). |
| **BlazorGame.Tests** | Tests unitaires (xUnit). |

---

## Fonctionnalités de la Version 1
- Structure complète de la solution .NET.  
- Pages Blazor : Accueil, Nouvelle Aventure, Classement, Admin.  
- Navigation et icônes Bootstrap.  
- Composant **Salle** statique affiché dans “Nouvelle Aventure”.  
- Modèles de base : `Joueur`, `Partie`, `Salle`.  
- Premiers tests unitaires (xUnit). 
- Premiers visuels 

---

## Tests
Les tests se trouvent dans `BlazorGame.Tests/`.  
- SalleTests.cs : création d’une salle, description non vide, difficulté correcte
- PartieTests.cs : identifiant unique, nombre de salles entre 1 et 5, EstTerminee = false
- EnumsAndActionResultatTests.cs : valeurs des enums valides, test du comportement d’ActionResultat
 

Pour lancer les tests :
```bash
dotnet test
```

## Lancer le projet
```bash
dotnet run --project BlazorGame.Client
```
ou

```bash
cd BlazorGame.Client
dotnet run
```