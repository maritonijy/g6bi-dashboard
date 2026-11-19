# Kova Inc. · FY2025 Finance BI (ASP.NET Core MVC · Razor)

A responsive, Power BI–style presentation: a slide deck, three interactive dashboard pages, and a detail page for every dashboard element.

## Run it in VS Code

Requirements
- .NET 8 SDK (check with `dotnet --version`) · https://dotnet.microsoft.com/download
- VS Code with the **C# Dev Kit** extension (optional, for IntelliSense)

Steps
```bash
cd KovaDashboard
dotnet restore
dotnet watch run
```
Open the URL printed in the terminal (for example `http://localhost:5xxx`).
`dotnet watch run` hot-reloads: saving a `.cshtml`, `.css` or `.js` file refreshes the browser. Saving a `.cs` file rebuilds and restarts.

## Pages

| URL | What it is |
|---|---|
| `/` | Presentation deck: title, business problem, dashboard overview, key numbers, decisions, element index, conclusion. Use the arrows, dots, or ← → keys. |
| `/dashboard/1` | Executive Financial Health: KPI cards, revenue vs expenses, net income, cumulative gap. Quarter slicer. Click a month to cross-filter. |
| `/dashboard/2` | Profitability & Budget: department slicer, cost by department, COGS split, opex trend, variance vs budget. Click a department to cross-filter. |
| `/dashboard/3` | Cash, Receivables & Payables: bank slicer, cash by bank, AR and AP aging, working-capital waterfall. |
| `/element/{id}` | Detail page for one element: what it shows, the insight, the decision, and how to build it in Power BI. Prev/next navigation. |
| `/Home/Schema` | Star schema, tables and measures. |

Element ids: `d1-kpi d1-trend d1-net d1-cum d1-slicers d2-kpi d2-dept d2-trend d2-cogs d2-budget d3-kpi d3-bank d3-ar d3-ap d3-wc`

## Where to change things

| To change | Edit |
|---|---|
| Numbers used in charts and KPIs | `Models/FinanceData.cs` |
| Detail-page text (insight, decision, build steps) | `Models/ElementCatalog.cs` |
| Slide content | `Views/Home/Index.cshtml` |
| Dashboard layouts | `Views/Dashboard/*.cshtml` |
| Chart behaviour and cross-filtering | `wwwroot/js/app.js` |
| Colours, spacing, breakpoints | `wwwroot/css/site.css` |

## Data notes (please check before presenting)

- Monthly revenue and expenses come from the group's deck and tie to the KPI cards (revenue $12.59M, expenses $14.62M, net −$2.03M).
- The Power BI screenshots were treated as the source of truth where they disagree with the HTML deck. For example, AR is $2.86M and DSO is 83 days in the report, not $0.92M and 27 days.
- Department totals, opex trend lines, and AR/AP aging buckets were read from the Power BI charts. They are approximate, so replace them with exact model values.
- The working-capital definition differs: the NWC card is AR − AP ($2.00M), while the bridge total is cash + AR − AP ($6.30M). Pick one.
- The cash-by-currency donut in the report totals $25.03M, not the $4.30M cash balance. Its measure needs review.

## Deploy

Publish a self-contained folder:
```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet KovaDashboard.dll
```
On a server, set the listening address with `ASPNETCORE_URLS=http://0.0.0.0:8080`.

For Azure App Service (with the Azure CLI installed):
```bash
az webapp up --runtime "DOTNETCORE:8.0" --name <your-unique-app-name> --sku F1
```
