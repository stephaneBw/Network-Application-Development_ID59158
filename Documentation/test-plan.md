# Lab 14 — Test Plan

**Project:** Currency Exchange Office  
**Author:** Stephane Bwirukiro (59158)  
**Date:** 2026

Use this checklist before Moodle submission and Lab 15 presentation.

## Build verification

- [ ] Open `Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.sln`
- [ ] **Build → Rebuild Solution** — 0 errors
- [ ] Database `ExchangeOfficeDb` exists; `Database/schema.sql` applied

## End-to-end functional tests

### Registration & persistence (Lab 9, 11)

- [ ] Register user `testuser1` — success, user ID shown
- [ ] Register same username again — error (duplicate)
- [ ] Stop app, restart — new registration still works (DB alive)

### Current rate (Lab 7, 8)

- [ ] Market Rates → `USD` → Get Current Rate — numeric PLN value
- [ ] Invalid code `XXX` — friendly error

### Historical rates (Lab 13)

- [ ] From = 7 days ago, To = today, currency `USD`
- [ ] Load Historical Rates — grid has multiple rows
- [ ] Dates are in ascending order

### Trading (Lab 6, 10, 12)

- [ ] Register fresh user
- [ ] Deposit 1000 PLN — balance PLN ≈ 1000
- [ ] Buy 100 USD — PLN decreases, USD increases
- [ ] Sell 40 USD — PLN increases, USD decreases
- [ ] Refresh History — 3 rows: TopUp, Buy, Sell

### Data persistence (Lab 11–12)

- [ ] Close Visual Studio debugging completely
- [ ] Run again, query SQL:

```sql
SELECT * FROM Users ORDER BY Id DESC;
SELECT * FROM Balances ORDER BY UserId, CurrencyCode;
SELECT * FROM Transactions ORDER BY Id DESC;
```

- [ ] Rows match last test session

## Regression (early labs)

- [ ] Lab 1 calculator still builds (optional)
- [ ] `NbpRatesService` still builds (optional)

## UI checks

- [ ] Trading buttons disabled before registration
- [ ] Status bar shows balances after login
- [ ] Errors appear in red status area

## Submission checklist (Moodle)

- [ ] GitHub repo is **public**
- [ ] README contains course name, title, author, ID, description, run steps
- [ ] Multiple commits visible (not single upload)
- [ ] Link submitted on Moodle

## Presentation talking points (Lab 15)

1. Architecture: WPF → WCF → Business → SQL + NBP
2. Live demo using table above (2–3 minutes)
3. Mention optional grade features: WPF client (+1), database (+1)
4. One challenge solved: LocalDB instance mismatch (`ProjectModels` vs `MSSQLLocalDB`)
