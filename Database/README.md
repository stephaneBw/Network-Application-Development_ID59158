# Database setup

## Values (must match Web.config)

| Item | Value |
|------|--------|
| Connection string **name** | `ExchangeOfficeDb` |
| SQL **database name** | `ExchangeOfficeDb` |
| SQL **instance** (default) | `(localdb)\MSSQLLocalDB` |

## Steps

1. Visual Studio → **SQL Server Object Explorer**
2. Connect to `(localdb)\MSSQLLocalDB`
3. **Databases** → Add New Database → `ExchangeOfficeDb`
4. Right-click `ExchangeOfficeDb` → **New Query**
5. Paste and execute `schema.sql`

## Verify

Tables: `Users`, `Balances`, `Transactions`

## Different LocalDB instance?

If you use e.g. `(localdb)\ProjectModels`, update **only** `Data Source` in:

`Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.Service/Web.config`

Keep `Initial Catalog=ExchangeOfficeDb` and the connection name `ExchangeOfficeDb`.
