# Currency Exchange Office

**Course:** Network Application Development  
**Project title:** Currency Exchange Office System  
**Author:** Stephane Bwirukiro  
**Student ID:** 59158  

## Description

A network-based currency exchange office built with **WCF**, **WPF**, **SQL Server LocalDB**, and the **National Bank of Poland (NBP)** public API.

The system supports:

- User registration with persisted accounts
- Simulated PLN top-up (deposit)
- Live exchange rates from NBP (current and historical)
- Buy/sell foreign currency using live NBP mid rates
- Balance tracking and transaction history stored in SQL Server

## Repository structure

```
├── Client-Application/ExchangeOffice.Client/   # WPF client (Labs 8–10, 12–13)
├── Database/                                   # SQL schema scripts
├── Documentation/                              # Architecture and test plan
├── NbpRatesService/                            # Early lab: standalone NBP WCF service (Labs 2–4)
├── Wcf-Service/
│   ├── HelloWcfService/                        # Lab 1: WCF introduction
│   └── ExchangeOffice/ExchangeOffice/        # Final solution (Labs 5–14)
│       ├── ExchangeOffice.Contracts/
│       ├── ExchangeOffice.Business/
│       ├── ExchangeOffice.Data/
│       ├── ExchangeOffice.Nbp/
│       ├── ExchangeOffice.Service/
│       └── ExchangeOffice.TestClient/
└── README.md
```

## Prerequisites

- Windows with **Visual Studio 2026** 
- Workload: **.NET desktop development**
- **.NET Framework 4.8**
- **SQL Server LocalDB** (via Visual Studio Installer)
- Internet access (NBP API: https://api.nbp.pl)

## Database setup (required for final project)

1. Open **SQL Server Object Explorer** in Visual Studio.
2. Connect to `(localdb)\ProjectModels` (or update connection string — see Troubleshooting).
3. Create database `ExchangeOfficeDb` if it does not exist.
4. Run script: `Database/schema.sql`

Connection string is in `Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.Service/Web.config`:

```xml
Data Source=(localdb)\ProjectModels;Initial Catalog=ExchangeOfficeDb;Integrated Security=True;
```

## How to run the final project (Labs 5–14)

1. Open solution:  
   `Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.sln`
2. **Build → Rebuild Solution**
3. Right-click solution → **Set Startup Projects** → **Multiple startup projects**:
   - `ExchangeOffice.Service` → **Start**
   - `ExchangeOffice.Client` → **Start**
4. Press **F5**

### Demo flow (for presentation)

| Step | Tab | Action |
|------|-----|--------|
| 1 | Account Registration | Register new user (unique username) |
| 2 | Market Rates | Get current rate for `USD` |
| 3 | Market Rates | Load historical rates (last 7 days) |
| 4 | Trading Desk | Deposit `1000` PLN |
| 5 | Trading Desk | Buy `100` USD |
| 6 | Trading Desk | Sell `40` USD |
| 7 | Trading Desk | Refresh History — verify TopUp, Buy, Sell rows |
| 8 | Restart app | Register or use DB — confirm data persisted |

## How to run early labs

### Lab 1 — WCF calculator

- Projects: `Wcf-Service/HelloWcfService` + `Client-Application/HelloWcfClient`
- Multiple startup → F5 → console shows `35` for `10 + 25`

### Labs 2–4 — Standalone NBP rate service

- Project: `NbpRatesService` (+ optional console client if added)
- Calls NBP API for current currency rate by code

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Cannot open database `ExchangeOfficeDb` | Create DB on your LocalDB instance; run `Database/schema.sql`; match `Web.config` instance name |
| WCF client cannot connect | Start `ExchangeOffice.Service` first; check port in browser URL; update `Client-Application/ExchangeOffice.Client/App.config` endpoint if port changed |
| Service reference out of date | Right-click **ExchangeOfficeRef** → **Update Service Reference** |
| Buy fails: insufficient PLN | Top up PLN before buying |
| Historical rates empty | Use valid table A code (USD, EUR); avoid weekends-only ranges |
| NBP errors | Check internet connection |

## Implemented features by lab

| Lab | Feature |
|-----|---------|
| 1 | WCF service + console client |
| 2–4 | NBP current rate WCF service |
| 5 | Solution architecture + contracts |
| 6–7 | Exchange logic + live NBP rates |
| 8–10 | WPF client (rates, register, trade) |
| 11–12 | SQL persistence + transaction history |
| 13 | Historical NBP rates report |
| 14 | Documentation, test plan, final polish |

## Documentation

- `Documentation/architecture.md` — system design
- `Documentation/test-plan.md` — Lab 14 test checklist

## License / academic use

Project created for university coursework. Public repository for grading until evaluation is complete.
