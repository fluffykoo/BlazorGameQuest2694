# BlazorGameQuest – README

## Présentation générale
BlazorGameQuest est un jeu d’aventure développé en **.NET 9**, **C#**, **Blazor WebAssembly** et une API **ASP.NET Core** sécurisée avec **Keycloak**. Le joueur explore un donjon généré aléatoirement, face à des salles de combat ou des coffres, et tente d’obtenir le meilleur score. Un administrateur accède à un tableau de bord complet (classement, stats, exports JSON/CSV).

---
# Structure de la solution
| Projet | Rôle |
|--------|-------|
| **BlazorGame.Client** | Frontend Blazor WebAssembly (UI + Auth OIDC) |
| **BlazorGame.Api** | API REST + EF Core + PostgreSQL |
| **BlazorGame.Domain** | Modèles métiers (Joueur, Partie, Salle…) |
| **BlazorGame.Tests** | Tests unitaires et API |

---

<details>
<summary><strong>Version 1 – Structure Blazor & Modèles de base <strong></summary>

## Fonctionnalités
- Pages : Accueil, Nouvelle Aventure, Classement, Admin.
- Composant `Salle` statique.
- Modèles métiers initiaux : Joueur, Partie, Salle.
- Navigation + premières maquettes UI.

## Tests présents
- SalleTests : cohérence modèle
- PartieTests : nombre de salles, état initial
- ActionResultatTests : cohérence enums et valeurs
- ControllerTests : CRUD EF Core

Exécution :
```bash
dotnet test
```

Lancement client :
```bash
cd BlazorGame.Client
dotnet run
```
</details>

---

<details>
<summary><strong>Version 2 – Base de données PostgreSQL + API REST</strong></summary>

## Nouveautés
- PostgreSQL + EF Core
- Migrations et persistance
- Relations complètes (Joueur ↔ Partie ↔ Salle)
- CRUD complets via contrôleurs
- Swagger intégré

Swagger : http://localhost:5040/swagger

Contrôleurs principaux : Joueurs, Parties, Salles, Donjons, Administrateurs.
</details>

---

<details>
<summary><strong>Version 3 – Gameplay, Donjons & Logique de jeu</strong></summary>

## Ajouts majeurs
- Génération procédurale de donjons.
- Types de salles : **combat** et **coffre**.
- Actions disponibles en fonction de la salle.
- Calcul complet du score + sauvegarde.
- Page *Salle* interactive.

## Gameplay
### Combat
- Combattre : +30 (ou +20×niveau), échec −15
- Fuir : +5
- Fouiller : trésor +25 ou piège −10

### Coffre
- Fouiller : +40 ou −20
- Fuir : 0

### Fin de partie
- Dernière salle visitée → partie terminée
- Score < 0 → mort
- Score final enregistré

## Pages Blazor ajoutées
- `/nouvelle-aventure`
- `/salle/{partieId}`
- `/historique`
- `/classement`

## API enrichie
- Historique joueur
- Reprise de partie en cours
- PATCH fin de partie
</details>

---

<details>
<summary><strong>Version 4 – Dashboard Admin, Classement & Exports</strong></summary>

## Fonctionnalités
- Classement public + classement complet réservé à l'admin.
- Dashboard admin :
  - Stats (joueurs actifs/inactifs, parties, score cumulé…)
  - Gestion des joueurs (toggle actif/inactif)
  - Liste des parties + détail des salles
  - Exports JSON & CSV

## Endpoints notables
- `GET api/Joueurs/classement`
- `GET api/Joueurs/classement-admin`
- `PATCH api/Joueurs/{id}/toggle`
- `GET api/Joueurs/export`
- `GET api/Partie` avec salles + donjon + joueur
</details>

---

<details open>
<summary><strong>Version 5 – Authentification & Sécurisation (Keycloak OIDC)</strong></summary>

## Keycloak (Realm GameQuest)
**Rôles** : `joueur`, `admin`  
**Utilisateurs** : user1/1234, user2/1234, admin/admin

Clients :
- `blazor-client` (front Blazor WASM)
- `blazorgame-api` (API protégée)

## Sécurisation API
| Endpoint | Accès |
|----------|--------|
| `/api/Donjons`, `/api/Administrateurs`, exports | admin uniquement |
| `/api/Partie`, `/api/Salle` | joueur ou admin |

### Tester un token via curl
```bash
curl -X POST \  
  -d "client_id=blazorgame-api" \  
  -d "grant_type=password" \  
  -d "username=user1" \  
  -d "password=1234" \  
  http://localhost:8080/realms/GameQuest/protocol/openid-connect/token
```

## Sécurisation du front Blazor
- Redirection automatique vers Keycloak si non authentifié.
- Token automatiquement inclus dans les requêtes API.
- Navigation conditionnelle selon rôle.
- `RemoteAuthenticatorView` gère login/logout/callback.

---
## Limitations Techniques Version 5
## 1. La page Admin n’est pas protégée côté UI
Blazor WASM télécharge toute l’app → on ne peut **pas empêcher l’accès à la route `/admin` côté client**.  

  **Actuellement :** 
 tout utilisateur connecté Keycloak peut ouvrir la page.  

 **Cependant : la sécurité est assurée côté API** → toutes les routes admin renvoient **403 Forbidden** pour un non-admin.

 **Conclusion : la sécurité réelle est correcte**, même si l’UI n’est pas verrouillable.

---
## 2. Problème connu Keycloak : la déconnexion échoue
Erreur fréquente :
```
The logout was not initiated from within the page
```
Ce bug Keycloak cause :
- session non détruite
- impossibilité de se reconnecter avec un autre utilisateur

### Contournement
1. Vider `localStorage` et `sessionStorage` du navigateur.  
2. Recharger l'application.  
3. Se reconnecter.

---
# Tests – Définition et couverture
## Joueur
- Score initial = 0
- Historique vide
- Date de connexion correcte

## Partie
- ID + date OK
- nb de salles entre 1 et 5
- partie non terminée au départ

## Salle
- Champs valides
- Choix possibles corrects
- Résultat cohérent

## ActionResultat
- Gains/pertes cohérents
- Détection piège correcte

---
# Lancer le projet
## API
```bash
cd BlazorGame.Api
dotnet run
```

## Client
```bash
cd BlazorGame.Client
dotnet run
```

</details>

