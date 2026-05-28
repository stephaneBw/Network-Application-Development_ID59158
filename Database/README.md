# Database setup

## Create database

1. Visual Studio → **View → SQL Server Object Explorer**
2. Connect to your LocalDB instance (example: `(localdb)\ProjectModels`)
3. Right-click **Databases → Add New Database**
4. Name: `ExchangeOfficeDb`

## Apply schema

1. Right-click `ExchangeOfficeDb` → **New Query**
2. Open `schema.sql` from this folder, paste, execute

## Verify tables

You should see:

- `dbo.Users`
- `dbo.Balances`
- `dbo.Transactions`

## Connection string

Must match your LocalDB instance in:

`Wcf-Service/ExchangeOffice/ExchangeOffice/ExchangeOffice.Service/Web.config`

Default used in this project:

```
Data Source=(localdb)\ProjectModels;Initial Catalog=ExchangeOfficeDb;Integrated Security=True;
```

If your DB is on `(localdb)\MSSQLLocalDB`, change `Data Source` accordingly.
