# BlazorGameQuest (Oumou Camara & Lucas Baury)

## Description
BlazorGameQuest est un jeu d’aventure développé avec **.NET 9 / C#** et **Blazor WebAssembly**.  
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
## Définitions des tests

## Joueur

| **Cas de test** | **Objectif** | **Données / Conditions** | **Résultat attendu** |
|-----------------|---------------|---------------------------|----------------------|
| Création du joueur | Vérifier que le joueur a un identifiant unique | Instancier un nouvel objet `Joueur` | `Id` est unique |
| Score initial | Vérifier la valeur initiale du score | Création d’un joueur sans action | `ScoreTotal = 0` |
| Historique vide | Vérifier l’état de l’historique à la création | Nouveau joueur | `Historique.Count = 0` |
| Dernière connexion | Vérifier la date enregistrée à la création | Nouveau joueur | `DerniereConnexion = Date du jour` |

---

## Partie

| **Cas de test** | **Objectif** | **Données / Conditions** | **Résultat attendu** |
|-----------------|---------------|---------------------------|----------------------|
| Création d’une partie | Vérifier que la partie possède un identifiant et une date | Créer un nouvel objet `Partie` | `Id` ≠ null et `Date = Date actuelle` |
| Nombre de salles | Vérifier que le donjon contient entre 1 et 5 salles | Génération d’une partie | `Salles.Count` entre 1 et 5 |
| Partie non terminée | Vérifier l’état initial | Nouvelle partie | `EstTerminee = false` |
| Score final | Vérifier la valeur initiale du score final | Nouvelle partie | `ScoreFinal = 0` |

---

## Salle

| **Cas de test** | **Objectif** | **Données / Conditions** | **Résultat attendu** |
|-----------------|---------------|---------------------------|----------------------|
| Création d’une salle | Vérifier que la salle a une position, une description et un niveau | Créer une `Salle` | Champs renseignés (`Position`, `Description`, `Niveau`) |
| Choix disponibles | Vérifier que les actions disponibles sont valides | Nouvelle salle | `ChoixPossible` contient Combattre, Fuir, Fouiller |
| Choix effectué | Vérifier que le joueur peut choisir une action | Affecter une valeur à `ChoixFait` | `ChoixFait` correspond à une des actions possibles |
| Résultat d’action | Vérifier qu’un résultat est associé à l’action | Affecter un `Resultat` à une salle | `Resultat` non nul et cohérent |

---

## Action / Résultat

| **Cas de test** | **Objectif** | **Données / Conditions** | **Résultat attendu** |
|-----------------|---------------|---------------------------|----------------------|
| Création d’un résultat | Vérifier que l’objet `ActionResultat` se crée avec des valeurs valides | Instancier un `ActionResultat` | Champs (`Action`, `Points`, `Message`) non vides |
| Gain de points | Vérifier que l’action attribue correctement les points | Choix "Combattre" | `Points` > 0 |
| Perte de points | Vérifier la pénalité sur une mauvaise action | Choix "Fouiller" → Piège | `Points` < 0 |
| Détection de piège | Vérifier la valeur du booléen `EstPiege` | Action piégée | `EstPiege = true` |

