# Currency Exchange Office

**Course:** Network Application Development  
**Author:** Stephane Bwirukiro   
**Student ID:** 59158  

## Description
Gradual build of a currency exchange office: WCF services, NBP rates, later WPF client and database.

## How to run (Lab 1–5)

### Lab 1 — Calculator
1. Open `HelloWcfService` solution or project.
2. Set startup: HelloWcfService + HelloWcfClient.
3. Press F5. Console shows 35 for 10+25.

### Labs 2–4 — NBP rates
1. Set startup: NbpRatesService + NbpRatesClient.
2. Press F5. Console prints USD and EUR rates.

### Lab 5–12 — Exchange office (WCF + WPF + SQL)
1. Open `Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.sln`.
2. Ensure SQL database `ExchangeOfficeDb` exists on `(localdb)\ProjectModels` and run `Database/schema.sql`.
3. Set multiple startup projects: `ExchangeOffice.Service` + `ExchangeOffice.Client`.
4. Press F5.
5. Register → Top up → Buy/Sell → Refresh History on Trading Desk tab.

### Lab 5 — Exchange office (stub)
1. Open `ExchangeOffice.sln`.
2. Run `ExchangeOffice.Service` — service starts; methods not implemented until Lab 6+.

### Labs 8–10 — WPF client
1. Open `ExchangeOffice.sln`.
2. Set multiple startup: `ExchangeOffice.Service` + `ExchangeOffice.Client`.
3. Press F5.
4. Register → Top up → Buy/Sell → Refresh balances.
