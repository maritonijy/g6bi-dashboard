namespace KovaDashboard.Models;

/// <summary>
/// Content for the PowerPoint-style presentation: the Power BI table metadata,
/// and the per-element field/measure notes used on the element explanation slides.
/// Element prose (what it shows, insights, decision) is reused from ElementCatalog.
/// </summary>
public static class PresentationCatalog
{
    // ───────────── Tables (exact columns, data types and visibility) ─────────────

    public static readonly PowerBiTable[] Tables =
    {
        new()
        {
            Name = "ledger update 2026",
            Kind = "Fact",
            Purpose = "Every accounting line. Drives revenue, expenses, net income and aging.",
            Columns = new[]
            {
                new TableColumn("account_code", "Text"),
                new TableColumn("account_name", "Text"),
                new TableColumn("Aging Bucket", "Text"),
                new TableColumn("Aging Days", "Integer"),
                new TableColumn("Aging Order", "Integer"),
                new TableColumn("amount", "Currency"),
                new TableColumn("category", "Text"),
                new TableColumn("department", "Text"),
                new TableColumn("description", "Text"),
                new TableColumn("IsNonOperating", "True/False"),
                new TableColumn("month_start", "Date"),
                new TableColumn("pl_amount", "Currency"),
                new TableColumn("transaction_date", "Date")
            }
        },
        new()
        {
            Name = "budget update 2026",
            Kind = "Fact",
            Purpose = "Approved budget by account, department and month.",
            Columns = new[]
            {
                new TableColumn("account_code", "Text"),
                new TableColumn("account_name", "Text"),
                new TableColumn("budget_amount", "Currency"),
                new TableColumn("category", "Text"),
                new TableColumn("department", "Text"),
                new TableColumn("month", "Text"),
                new TableColumn("month_start", "Date")
            }
        },
        new()
        {
            Name = "invoices update 2026",
            Kind = "Fact",
            Purpose = "Customer invoices. Basis for revenue, receivables and VAT.",
            Columns = new[]
            {
                new TableColumn("customer_country", "Text"),
                new TableColumn("customer_name", "Text"),
                new TableColumn("invoice_date", "Date"),
                new TableColumn("invoice_number", "Text"),
                new TableColumn("month_start", "Date"),
                new TableColumn("net_amount", "Currency"),
                new TableColumn("payment_status", "Text"),
                new TableColumn("product_type", "Text"),
                new TableColumn("total_amount", "Currency"),
                new TableColumn("vat_amount", "Currency"),
                new TableColumn("vat_rate", "Number", Hidden: true)
            }
        },
        new()
        {
            Name = "cash update 2026",
            Kind = "Fact",
            Purpose = "Bank balances and account terms by entity and currency.",
            Columns = new[]
            {
                new TableColumn("account_type", "Text"),
                new TableColumn("balance", "Currency"),
                new TableColumn("bank_name", "Text"),
                new TableColumn("country", "Text"),
                new TableColumn("currency", "Text"),
                new TableColumn("entity_id", "Text"),
                new TableColumn("entity_name", "Text"),
                new TableColumn("interest_rate", "Number"),
                new TableColumn("minimum_balance", "Currency"),
                new TableColumn("overdraft_limit", "Currency"),
                new TableColumn("relationship_manager", "Text")
            }
        },
        new()
        {
            Name = "dim_account",
            Kind = "Dimension",
            Purpose = "Account list and category grouping.",
            Columns = new[]
            {
                new TableColumn("account_code", "Text"),
                new TableColumn("account_name", "Text"),
                new TableColumn("category", "Text")
            }
        },
        new()
        {
            Name = "dim_department",
            Kind = "Dimension",
            Purpose = "Department list used to slice cost and budget.",
            Columns = new[] { new TableColumn("department", "Text") }
        },
        new()
        {
            Name = "dim_month",
            Kind = "Dimension",
            Purpose = "Month and quarter labels for time analysis.",
            Columns = new[]
            {
                new TableColumn("month_name", "Text"),
                new TableColumn("month_start", "Date"),
                new TableColumn("quarter", "Text")
            }
        },
        new()
        {
            Name = "WC Steps",
            Kind = "Supporting",
            Purpose = "Drives the steps of the working-capital bridge.",
            Columns = new[]
            {
                new TableColumn("Order", "Integer"),
                new TableColumn("Step", "Text"),
                new TableColumn("WC Bridge", "Number")
            }
        },
        new()
        {
            Name = "_Measures",
            Kind = "Supporting",
            Purpose = "DAX measures used across the three dashboards. The placeholder measure is hidden in the model.",
            Columns = new[]
            {
                new TableColumn("AP Outstanding", "DAX measure"),
                new TableColumn("AR Outstanding", "DAX measure"),
                new TableColumn("Bank Balance USD", "DAX measure"),
                new TableColumn("Budget Operating Expenses", "DAX measure"),
                new TableColumn("Budget Revenue", "DAX measure"),
                new TableColumn("Budget Utilisation %", "DAX measure"),
                new TableColumn("Cash Balance", "DAX measure"),
                new TableColumn("COGS", "DAX measure"),
                new TableColumn("Cost", "DAX measure"),
                new TableColumn("Cumulative Revenue Gap", "DAX measure"),
                new TableColumn("DPO", "DAX measure"),
                new TableColumn("DSO", "DAX measure"),
                new TableColumn("Gross Margin %", "DAX measure"),
                new TableColumn("Invoices Collected %", "DAX measure"),
                new TableColumn("Invoices Paid %", "DAX measure"),
                new TableColumn("Net Income", "DAX measure"),
                new TableColumn("Net Income (Loss)", "DAX measure"),
                new TableColumn("Net Income Card", "DAX measure"),
                new TableColumn("Net Loss Label", "DAX measure"),
                new TableColumn("Net Margin %", "DAX measure"),
                new TableColumn("Net Working Capital", "DAX measure"),
                new TableColumn("Operating Expenses", "DAX measure"),
                new TableColumn("Opex Variance (Over)/Under", "DAX measure"),
                new TableColumn("Opex Variance %", "DAX measure"),
                new TableColumn("Revenue", "DAX measure"),
                new TableColumn("Revenue Gap", "DAX measure"),
                new TableColumn("Revenue Variance %", "DAX measure"),
                new TableColumn("Total Expenses", "DAX measure"),
                new TableColumn("Total Revenue", "DAX measure"),
                new TableColumn("placeholder", "DAX measure", Hidden: true)
            }
        },
        new()
        {
            Name = "DateTableTemplate_e2d6d5bf-3fd7-48bc-ae26-844885e9a81e",
            Kind = "Auto date",
            Purpose = "Auto date/time table generated by Power BI. All columns hidden.",
            Columns = AutoDateColumns()
        },
        new()
        {
            Name = "LocalDateTable_60727176-865c-41c7-81a0-d51b98f31e07",
            Kind = "Auto date",
            Purpose = "Auto date/time table generated by Power BI. All columns hidden.",
            Columns = AutoDateColumns()
        },
        new()
        {
            Name = "LocalDateTable_7e9c71b9-1a4e-4038-8f05-e75b6c557f2e",
            Kind = "Auto date",
            Purpose = "Auto date/time table generated by Power BI. All columns hidden.",
            Columns = AutoDateColumns()
        },
        new()
        {
            Name = "LocalDateTable_8b94bc10-7deb-4cce-ad9c-08e75eba2551",
            Kind = "Auto date",
            Purpose = "Auto date/time table generated by Power BI. All columns hidden.",
            Columns = AutoDateColumns()
        }
    };

    private static TableColumn[] AutoDateColumns() => new[]
    {
        new TableColumn("Date", "Date", Hidden: true),
        new TableColumn("Day", "Integer", Hidden: true),
        new TableColumn("month", "Text", Hidden: true),
        new TableColumn("MonthNo", "Integer", Hidden: true),
        new TableColumn("Quarter", "Text", Hidden: true),
        new TableColumn("QuarterNo", "Integer", Hidden: true),
        new TableColumn("Year", "Integer", Hidden: true)
    };

    public static string[] Kinds { get; } = { "Fact", "Dimension", "Supporting", "Auto date" };

    // ───────────── Element visuals ─────────────

    public static readonly Dictionary<string, VisualMeta> Visuals = new()
    {
        ["d1-kpi"] = new VisualMeta
        {
            ChartKey = "kpi",
            Caption = "FY2025 · all months · USD",
            Fields = new[]
            {
                new FieldRow("Measures", "[Total Revenue] · [Total Expenses] · [Net Income] · [Gross Margin %] · [Opex Variance] · [Cash Balance]"),
                new FieldRow("Visual", "Six KPI cards, one measure each"),
                new FieldRow("Slicer", "Quarter (dim_month) — the cards re-scope")
            },
            Note = "Values shown are FY2025 with the Quarter slicer on All."
        },
        ["d1-trend"] = new VisualMeta
        {
            ChartKey = "column-cluster",
            Caption = "USD · monthly · Jan → Dec 2025",
            Fields = new[]
            {
                new FieldRow("Axis", "dim_month[month_name], ordered by month"),
                new FieldRow("Values", "[Total Revenue] · [Total Expenses]"),
                new FieldRow("Interaction", "Click a month to cross-filter every visual")
            },
            Note = "Expenses sit above revenue in all twelve months."
        },
        ["d1-net"] = new VisualMeta
        {
            ChartKey = "column-net",
            Caption = "USD · monthly · zero baseline",
            Fields = new[]
            {
                new FieldRow("Axis", "dim_month[month_name]"),
                new FieldRow("Values", "[Net Income] = Total Revenue − Total Expenses"),
                new FieldRow("Baseline", "Constant zero line; losses fall below it")
            },
            Note = "Dashboard 1 uses this net income column chart — there is no stacked column chart on the page."
        },
        ["d1-cum"] = new VisualMeta
        {
            ChartKey = "area",
            Caption = "USD · running total of (Expenses − Revenue)",
            Fields = new[]
            {
                new FieldRow("Axis", "dim_month[month_name]"),
                new FieldRow("Values", "[Cumulative Gap] — a running sum of (Expenses − Revenue)"),
                new FieldRow("Reference", "Ends at −$2.03M, matching the net income card")
            },
            Note = "A nearly straight line means a steady loss of about $169K a month."
        },
        ["d2-kpi"] = new VisualMeta
        {
            ChartKey = "kpi",
            Caption = "FY2025 · all departments · USD",
            Fields = new[]
            {
                new FieldRow("Measures", "[COGS] · [Operating Expenses] · [Budgeted Opex] · [Opex Variance] · [Gross Margin %]"),
                new FieldRow("Visual", "Five KPI cards, one measure each"),
                new FieldRow("Slicer", "Department (dim_department)")
            },
            Note = "A positive cost variance is unfavourable — the card is labelled that way."
        },
        ["d2-dept"] = new VisualMeta
        {
            ChartKey = "bar-dept",
            Caption = "USD · total cost by department · largest first",
            Fields = new[]
            {
                new FieldRow("Axis", "dim_department[department]"),
                new FieldRow("Values", "[Total Cost] — COGS and opex together"),
                new FieldRow("Interaction", "Click a bar to filter the page")
            },
            Note = "Bar values were read from the report chart and rounded."
        },
        ["d2-cogs"] = new VisualMeta
        {
            ChartKey = "donut",
            Caption = "USD · COGS split into its two delivery streams",
            Fields = new[]
            {
                new FieldRow("Legend", "Cost-of-sales subtype"),
                new FieldRow("Values", "[COGS] — $2.83M total"),
                new FieldRow("Shares", "Software 74.2% · Services 25.8%")
            },
            Note = "Two slices keep the donut readable."
        },
        ["d2-trend"] = new VisualMeta
        {
            ChartKey = "line-opex",
            Caption = "USD · monthly operating cost by department",
            Fields = new[]
            {
                new FieldRow("Axis", "dim_month[month_name]"),
                new FieldRow("Values", "[Opex]"),
                new FieldRow("Legend", "dim_department[department] — People, Marketing, Technology, General")
            },
            Note = "Start and end months match the report; the points between are interpolated."
        },
        ["d2-budget"] = new VisualMeta
        {
            ChartKey = "bar-variance",
            Caption = "USD · variance against the 100% budget",
            Fields = new[]
            {
                new FieldRow("Axis", "dim_department[department]"),
                new FieldRow("Values", "[Opex Variance] vs approved budget"),
                new FieldRow("Colour", "Coral = overspent · mint = under budget")
            },
            Note = "People and Marketing make up about 88% of the overrun."
        },
        ["d3-kpi"] = new VisualMeta
        {
            ChartKey = "kpi",
            Caption = "FY2025 · all banks · USD",
            Fields = new[]
            {
                new FieldRow("Measures", "[Cash Balance] · [AR Outstanding] · [AP Outstanding] · [DSO] · [DPO] · [Net Working Capital]"),
                new FieldRow("Visual", "Six KPI cards, one measure each"),
                new FieldRow("Slicer", "Bank (cash update 2026)")
            },
            Note = "The NWC card is AR − AP ($2.00M); the bridge below uses cash + AR − AP."
        },
        ["d3-bank"] = new VisualMeta
        {
            ChartKey = "bar-bank",
            Caption = "USD · cash balance by bank",
            Fields = new[]
            {
                new FieldRow("Axis", "cash update 2026[bank_name]"),
                new FieldRow("Values", "[Cash Balance] in USD"),
                new FieldRow("Interaction", "Click a bank or use the Bank slicer")
            },
            Note = "JPMorgan Chase holds about 81% of cash — a concentration to review."
        },
        ["d3-ar"] = new VisualMeta
        {
            ChartKey = "bar-ar",
            Caption = "USD · open receivables by aging bucket",
            Fields = new[]
            {
                new FieldRow("Axis", "Aging bucket (fixed order)"),
                new FieldRow("Values", "[AR Outstanding]"),
                new FieldRow("Buckets", "Current · 1–30 · 31–60 · 61–90 · 90+ days")
            },
            Note = "Bucket amounts were read from the report chart and rounded."
        },
        ["d3-ap"] = new VisualMeta
        {
            ChartKey = "bar-ap",
            Caption = "USD · open payables by aging bucket",
            Fields = new[]
            {
                new FieldRow("Axis", "Aging bucket (fixed order)"),
                new FieldRow("Values", "[AP Outstanding]"),
                new FieldRow("Buckets", "Current · 1–30 · 31–60 · 61–90 · 90+ days")
            },
            Note = "A large 90+ balance sits oddly beside a 22-day DPO — reconcile the bill dates."
        },
        ["d3-wc"] = new VisualMeta
        {
            ChartKey = "waterfall",
            Caption = "USD · cash + AR − AP",
            Fields = new[]
            {
                new FieldRow("Categories", "Cash · + AR · − AP · Net WC"),
                new FieldRow("Source", "WC Steps (Order, Step, WC Bridge)"),
                new FieldRow("Result", "$4.30M + $2.86M − $0.86M = $6.30M")
            },
            Note = "Use the $4.30M cash balance, not $6.30M, for liquidity headroom."
        },
        ["d3-fx"] = new VisualMeta
        {
            ChartKey = "d3-fx",
            Caption = "USD · four slices · total $25.03M as shown in the report",
            Fields = new[]
            {
                new FieldRow("Legend", "Currency: SEK · USD · EUR · GBP (slice order unconfirmed)"),
                new FieldRow("Values", "[Cash Balance] by currency"),
                new FieldRow("Slices", "$14.20M · $4.30M · $3.68M · $2.85M"),
                new FieldRow("Shares", "56.7% · 17.2% · 14.7% · 11.4%")
            },
            Note = "Totals $25.03M, which does not reconcile to the $4.30M cash balance — review the measure."
        }
    };

    // ───────────── Slide grouping ─────────────

    public sealed record DashboardDeck(int Number, string Name, string Group, string[] ElementIds);

    public static readonly DashboardDeck[] Decks =
    {
        new(1, "Executive Financial Health", "Dashboard 1 · Executive",
            new[] { "d1-kpi", "d1-trend", "d1-net", "d1-cum" }),
        new(2, "Profitability & Budget Analysis", "Dashboard 2 · Profitability",
            new[] { "d2-kpi", "d2-dept", "d2-cogs", "d2-trend", "d2-budget" }),
        new(3, "Cash, Receivables & Payables", "Dashboard 3 · Cash",
            new[] { "d3-kpi", "d3-bank", "d3-ar", "d3-ap", "d3-wc", "d3-fx" })
    };
}

public record TableColumn(string Name, string DataType, bool Hidden = false);

public sealed class PowerBiTable
{
    public required string Name { get; init; }
    public required string Kind { get; init; }
    public required string Purpose { get; init; }
    public required TableColumn[] Columns { get; init; }
}

public record FieldRow(string Label, string Value);

public sealed class VisualMeta
{
    public required string ChartKey { get; init; }
    public required string Caption { get; init; }
    public required FieldRow[] Fields { get; init; }
    public string? Note { get; init; }
}

public sealed record ElementSlideModel(
    ElementDetail Item,
    VisualMeta Visual,
    string Position,
    string Group);
