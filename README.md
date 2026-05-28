# Currency Exchange Office

**Course:** Network Application Development  
**Project title:** Currency Exchange Office System  
**Author:** Stephane Bwirukiro  
**Student ID:** 59158  

## Description

Online currency exchange office: **WCF** business service, **WPF** client, **SQL Server LocalDB** persistence, and **NBP** API for live and historical exchange rates.

## Quick start 

1. Clone this repository.
2. Create the database (see [Database setup](#database-setup) below).
3. Open **`CurrencyExchangeOffice.slnx`** in the repository root (Visual Studio 2022+).
4. Set **multiple startup projects**:
   - `ExchangeOffice.Service` → Start
   - `ExchangeOffice.Client` → Start
5. Press **F5**.

### Demo checklist

| Step | Where | Action |
|------|--------|--------|
| 1 | Tab *Account Registration* | Register a new user |
| 2 | Tab *Market Rates* | Get current rate for `USD` |
| 3 | Tab *Market Rates* | Load historical rates (last 7 days) |
| 4 | Tab *Trading Desk* | Deposit 1000 PLN |
| 5 | Tab *Trading Desk* | Buy 100 USD, Sell 40 USD |
| 6 | Tab *Trading Desk* | Refresh History — TopUp, Buy, Sell rows |

## Repository layout 

```
CurrencyExchangeOffice.slnx          ← OPEN THIS (final product)
Client-Application/
  ExchangeOffice.Client/             ← WPF client
Wcf-Service/
  ExchangeOffice/ExchangeOffice/
    ExchangeOffice.Service/          ← WCF host
    ExchangeOffice.Business/
    ExchangeOffice.Data/
    ExchangeOffice.Nbp/
    ExchangeOffice.Contracts/
Database/
  schema.sql
Documentation/
Labs-Early/                          ← Labs 1–4 
NbpRatesService/                     ← Lab 2–4 archive
Wcf-Service/HelloWcfService/         ← Lab 1 archive
```

Early lab code is **not** part of the final solution. See `Labs-Early/README.md`.

## Database setup

| Setting | Value |
|---------|--------|
| **SQL instance** | `(localdb)\MSSQLLocalDB` |
| **Database name** | `ExchangeOfficeDb` |
| **Connection name** | `ExchangeOfficeDb` (in `Web.config`) |

1. Visual Studio → **SQL Server Object Explorer** → connect to `(localdb)\MSSQLLocalDB`.
2. Create database **`ExchangeOfficeDb`** (if missing).
3. Run **`Database/schema.sql`** on that database.

The connection string in `Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.Service/Web.config` must match your LocalDB instance:

```xml
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExchangeOfficeDb;Integrated Security=True;
```

If your database was created on `(localdb)\ProjectModels`, change only `Data Source` in that file to match.

## Implemented WCF operations

| Operation | Status |
|-----------|--------|
| `GetCurrentRate` | NBP live rate |
| `GetHistoricalRates` | NBP date range (Lab 13) |
| `RegisterUser` | SQL persistence |
| `TopUpPln` / `BuyCurrency` / `SellCurrency` | SQL + transactions |
| `GetBalance` | SQL |
| `GetTransactionHistory` | SQL (Lab 12) |

## Troubleshooting

| Issue | Fix |
|-------|-----|
| Cannot open database | Create `ExchangeOfficeDb` on the instance in `Web.config`; run `schema.sql` |
| WCF client connection failed | Start `ExchangeOffice.Service` first; update client `App.config` endpoint port if IIS Express port changed |
| Historical rates empty | Use table A codes (`USD`, `EUR`); pick a range with business days |
| Buy fails | Top up PLN first |

## Prerequisites

- Visual Studio 2022+ with **.NET Framework 4.8** and **.NET desktop development**
- SQL Server LocalDB
- Internet (NBP API)

## Further documentation

- `Documentation/architecture.md` — design
- `Documentation/test-plan.md` — test checklist
- `Documentation/GRADING.md` — notes for reviewers
