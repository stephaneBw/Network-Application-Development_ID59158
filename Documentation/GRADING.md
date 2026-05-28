# Notes for reviewers / grading

## Single entry point

Open **`CurrencyExchangeOffice.slnx`** at the repository root.  
Startup: `ExchangeOffice.Service` + `ExchangeOffice.Client`.

## Addressing common review points

### Historical rates (Lab 13)

`GetHistoricalRates` is implemented in:

- `ExchangeOffice.Nbp/NbpClient.cs` — NBP HTTP API
- `ExchangeOffice.Service/ExchangeOfficeService.svc.cs` — WCF endpoint
- WPF tab *Market Rates* — date range + grid

### Database connection

- **Connection string name:** `ExchangeOfficeDb`
- **Database name:** `ExchangeOfficeDb`
- **Default instance:** `(localdb)\MSSQLLocalDB`

All data access reads `ConfigurationManager.ConnectionStrings["ExchangeOfficeDb"]` from the WCF service `Web.config`.

### Repository structure

- Final code: `Wcf-Service/ExchangeOffice/ExchangeOffice/` + `Client-Application/ExchangeOffice.Client/`
- Labs 1–4: archived under `Labs-Early/`, `NbpRatesService/`, `HelloWcfService/` (marked with `ARCHIVE` files; excluded from root solution)
- Removed obsolete top-level `Wcf-Service/Service1.cs` template

### Contracts project

`Class1.cs` placeholder removed; contract is `IExchangeOfficeService.cs` only.
