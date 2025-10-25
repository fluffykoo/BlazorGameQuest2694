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
| **AuthenticationServices** | API et gestion de la base de données (PostgreSQL + EF Core). |
| **Models** | Classes métiers (Joueur, Partie, Salle). |
| **BlazorGame.Tests** | Tests unitaires (xUnit). |
---
<details>
<summary>Version 1 – Base du projet (Blazor + structure)</summary>

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
</details>

---
<details open>
<summary>Version 2 – Base de données et API</summary>

# Version 2 – BlazorGameQuest

## Objectif de la version 2
L’objectif de cette version est d’ajouter **la persistance des données** grâce à **Entity Framework Core** et **PostgreSQL**,  
et d’exposer les **API REST** permettant d’interagir avec les entités du jeu (`Joueur`, `Partie`, `Salle`) via **Swagger**.

---

## 1. Modélisation et Base de Données

### Modèles déjà présents (version 1)
Les classes principales (`Joueur`, `Partie`, `Salle`) existaient déjà dans la version 1.  
Elles définissaient la structure du jeu : les joueurs, les parties et les salles d’un donjon.

###  Nouveautés version 2
Dans cette version, nous avons ajouté **les attributs et relations EF Core** pour permettre la génération automatique  
des tables et des clés étrangères.

#### Exemple :
```csharp
// Partie.cs
[ForeignKey(nameof(Joueur))]
public Guid JoueurId { get; set; }

[InverseProperty(nameof(Salle.Partie))]
public List<Salle> Salles { get; set; } = new();

// Salle.cs
[ForeignKey(nameof(Partie))]
public Guid PartieId { get; set; }

[InverseProperty(nameof(Partie.Salles))]
public Partie? Partie { get; set; }
```

Ces ajouts permettent à **Entity Framework Core** de reconnaître les relations :
- `Joueur` → plusieurs `Parties`  
- `Partie` → plusieurs `Salles`

---

## 2. Configuration EF Core et PostgreSQL

###  Fichiers concernés
- `/AuthenticationServices/Data/AventureDbContext.cs`  
- `/AuthenticationServices/Data/AventureDbContextFactory.cs`  
- `/AuthenticationServices/Program.cs`

### Étapes réalisées

1. **Installation des dépendances**
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   dotnet add package Swashbuckle.AspNetCore
   ```

2. **Configuration du DbContext**
   ```csharp
   builder.Services.AddDbContext<AventureDbContext>(options =>
       options.UseNpgsql("Host=127.0.0.1;Port=5432;Database=AventureDB;Username=postgres;Password=postgres"));
   ```

3. **Création des migrations**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Vérification dans PostgreSQL (Docker)**
   Les tables suivantes ont été créées :
   - `Joueurs`
   - `Parties`
   - `Salles`
   - `__EFMigrationsHistory`

*(capture terminal PostgreSQL affichant les tables)*

---

## 3. Création des Microservices REST

### Fichiers concernés
- `/AuthenticationServices/Controllers/JoueursController.cs`

###  Fonctionnalités CRUD
- `GET /api/Joueurs` → liste tous les joueurs  
- `GET /api/Joueurs/{id}` → récupère un joueur précis  
- `POST /api/Joueurs` → ajoute un joueur  
- `PUT /api/Joueurs/{id}` → met à jour un joueur  
- `DELETE /api/Joueurs/{id}` → supprime un joueur  

*(capture Swagger affichant les endpoints )*

---

## 4. Ajout et test de Swagger

### Fichier modifié
- `/AuthenticationServices/Program.cs`

### Code ajouté
```csharp
builder.Services.AddSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();
```

### Test
Swagger est accessible à :  
`http://localhost:5040/swagger`

Tous les endpoints CRUD ont été testés avec succès.  
Les requêtes POST créent bien des entrées visibles dans PostgreSQL (via Docker).

*(capture à insérer : Swagger avec réponse JSON d’un joueur créé)*

---

## 5. Tests Unitaires
- Les tests unitaires de la version 1 (`BlazorGame.Tests`) ont été conservés.  
- Des tests d’intégration sur les endpoints seront ajoutés dans la version 3.

---
</details>
