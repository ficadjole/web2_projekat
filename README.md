# TripPlanner — Web aplikacija za planiranje putovanja
 
> Predmetni projekat — Primena veb programiranja u infrastrukturnim sistemima
 
---
 
## Opis projekta
 
TripPlanner je web aplikacija za planiranje putovanja koja korisniku omogućava da na jednom mestu organizuje sve važne informacije: destinacije, dnevni raspored aktivnosti, evidenciju troškova i budžet, packing listu, kao i deljenje planova putovanja sa drugim osobama putem QR koda.
 
---
 
## Arhitektura sistema
 
Sistem je izgrađen na osnovu **mikroservisne arhitekture** korišćenjem **Microsoft Service Fabric** platforme. Sastoji se od React frontenda, WebAPI gateway-a i četiri mikroservisa.

![Arhitektura sistema](https://github.com/user-attachments/assets/bbf46319-67c6-46ad-bc76-e6452b5040c6)
 
---
 
## Mikroservisi
 
### UserService — Stateless
Odgovoran za autentifikaciju i upravljanje korisnicima.
 
- Registracija i prijava korisnika
- Generisanje i validacija JWT tokena
- CRUD operacije nad korisničkim nalozima
- Upravljanje korisničkim ulogama (User / Admin)
- Pri brisanju korisnika šalje zahtev TripService-u za brisanje svih njegovih putovanja
- **Baza podataka:** SQL Server — `UsersDB`
 
---
 
### TripService — Stateless
Centralni servis odgovoran za sve u vezi sa planiranjem putovanja.
 
- CRUD operacije za planove putovanja (Trip)
- Upravljanje destinacijama (Destination)
- Organizacija aktivnosti po danima (Activity) sa kalendarskim prikazom
- Evidencija troškova i praćenje budžeta (Expense)
- Deljenje plana putovanja putem QR koda
- Generisanje PDF izveštaja plana putovanja
- Pri deljenju plana triggeruje EmailEvent koji MailingService obrađuje
- Pri brisanju putovanja šalje zahtev CheckListService-u za brisanje povezane checkliste
- **Baza podataka:** SQL Server — `TripsDB`
 
---
 
### CheckListService — Stateful
Upravlja packing listom i checklistom za svako putovanje.
 
- Podaci se čuvaju u **Reliable Dictionary** (ključ: `tripId`, vrednost: `ChecklistDto`)
- Pri pokretanju servisa podaci se učitavaju iz SQL baze u Reliable Dictionary
- Sve operacije izmene (dodavanje, brisanje, toggle stavke) upisuju se u **Reliable Queue** (SQL Queue)
- Background worker u `RunAsync` metodi asinhrono obrađuje queue i upisuje u SQL bazu
- SQL baza služi kao source of truth u slučaju pada klastera
**Reliable Collections:**
- `IReliableDictionary<Guid, ChecklistDto>` — in-memory radni podaci
- `IReliableQueue<QueueItem>` — asinhroni upis u bazu
**QueueItem model:**
```
- userId
- OperationType (Create / Delete / Toggle / DeleteItem)
- Payload (JSON)
```
 
- **Baza podataka:** SQL Server — `CheckListsDB`
 
---
 
### MailingService — Stateless
Odgovoran za slanje email obaveštenja korisnicima.
 
- Prima `EmailEvent` od TripService-a koji sadrži `TripShareDto` i email adresu primaoca
- Koristi **Reliable Queue** (EmailEvent Queue) za asinhrono procesiranje email zahtjeva
- Background worker obrađuje queue i šalje emailove putem Google SMTP servera
- Email sadrži QR kod za pristup deljenom planu putovanja i tip pristupa (View / Edit)
---
 
## Komunikacija između servisa
 
| Od | Do | Tip komunikacije | Razlog |
|---|---|---|---|
| WebAPI | Svi servisi | Service Fabric Remoting | Obrada korisničkih zahteva |
| UserService | TripService | Service Fabric Remoting | Brisanje tripova pri brisanju korisnika |
| TripService | CheckListService | Service Fabric Remoting | Brisanje checkliste pri brisanju tripa |
| TripService | MailingService | Email Event (async) | Slanje QR koda pri dijeljenju plana |
 
---
 
## Tehnički stack
 
### Frontend
- **React** + **TypeScript**
- **Tailwind CSS**
- **Axios** za HTTP komunikaciju
- **React Router** za navigaciju
- **jwt-decode** za parsiranje tokena
- Context API za upravljanje stanjem
### Backend
- **ASP.NET Core** — WebAPI Gateway
- **Microsoft Service Fabric** — mikroservisna platforma
- **Entity Framework Core** — ORM za SQL Server
- **FluentValidation** — validacija zahteva
- **QuestPDF** — generisanje PDF izveštaja
- **QRCoder** — generisanje QR kodova
- **MailKit** — slanje emailova putem SMTP-a
- **BCrypt** — heširanje lozinki
### Baze podataka
- **Microsoft SQL Server** — UsersDB, TripsDB, CheckListsDB
---

## Use case

[View use case diagram](docs/UseCase.png)

---

## Kreiranje baze podataka

Da biste pokrenuli odgovarajući MSSQL server, potrebno je da odete u `database` folder, otvorite terminal (CMD/PowerShell) i unesete sledeću komandu:

```bash
docker compose up
```

Ovo će kreirati odgovarajući SQL Server kontejner.

Nakon toga otvorite SQL Server Management Studio (SSMS) i unesite connection string kako biste se povezali na server.

![SSMS Connection](https://github.com/user-attachments/assets/70f1488b-fb0b-4e1f-b398-c3b683f358e8)


Biće potrebno da unutar foldera **Databases** kreirate baze podataka sa nazivom:

```text
- UsersDB
- TripsDB
- CheckListsDB
```

---

## Kreiranje i izmena tabela

Kada kreirate bazu podataka, unutar Visual Studio-a dodajte **local.settings.json** u svaki servis posebno.

> **Hint:** Potrebno je da unutar **local.settings.json** bude connection string ka bazi ste upravo napravili.

Nakon toga otvorite **Package Manager Console**, postavite mikroservis koji želite (User, Trip ili CheckList) kao Default project

![PMC Default project](https://github.com/user-attachments/assets/8315f98d-25f3-4914-9ce8-d9e3b34626ba)

> **Hint:** Možda će biti potrebno da postavite i odabrani mikroservis kao Startup Project.

i unesite sledeću komandu:

```powershell
Update-Database
```

Ova komanda će primeniti postojeće migracije i automatski kreirati tabele u bazi podataka.

Ako želite da menjate entitete (a samim tim i strukturu tabela u bazi), potrebno je da izvršite:

```powershell
Add-Migration ImeMigracije
```

Nakon toga pokrenite:

```powershell
Update-Database
```

kako bi izmene bile primenjene na bazu podataka.

---

## Pokretanje projekta

Backend

Potrebno je da postaviti WebProjekat kao Startup Project

Pokretanjem WebProjekta otvoriće se Swagger stranica na kojoj možete testirati i videti šta naši API endpointi prihvataju.

Frontend

Potrebno je instalirati sve potrebne pakete unošenjem sledeće komande:

```powershell
npm install
```

kako biste pokrenuli unesite:

```powershell
npm run dev
```

Da biste mogli koristiti funkcionalnost deljenja planova potrebno je da imate instaliran **ngrok**.
> **Hint:** Ngrok je revers proxy koji služi da kreira siguran tunel, omogućavajući da se zahtevi poslati na njegovu javnu adresu proslede direktno na moj localhost.

Kada pokrenete **ngrok** potrebno je da unesete odgovarajuci **auth_token** koji je povezan sa nalogom da biste uvek imali isti javni url.

```powershell
ngrok config add-authtoken $YOUR_AUTHTOKEN
```

Nakon toga unosite komandu za dobijanje public url-a:

```powershell
ngrok http --url=take-submarine-collapse.ngrok-free.dev 5137
```

Komanda se sastoji od:
- vrste protokola: http / https
- url
- localhost port frontenda

Nakon podesavanja **ngroka** moguce je koristiti funkcionalnost deljenja planova.
---
