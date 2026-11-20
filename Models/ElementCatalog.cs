namespace KovaDashboard.Models;

/// <summary>
/// Detail-page content for every dashboard element. Order matters: it drives prev/next navigation.
/// </summary>
public static class ElementCatalog
{
    public static readonly List<ElementDetail> All = new()
    {
        // ───────────── DASHBOARD 1 · EXECUTIVE FINANCIAL HEALTH ─────────────
        new ElementDetail
        {
            Id = "d1-kpi", Dashboard = 1,
            Title = "KPI strip: six numbers that size the business",
            ChartType = "KPI cards + measure table",
            Question = "Are we making money?",
            WhatItShows = "Six headline numbers for FY2025. Each card is one measure. The table shows how each one is built and what a manager should take from it.",
            Insights = new[]
            {
                "Revenue of $12.59M against expenses of $14.62M gives a net loss of −$2.03M, about 16% of revenue.",
                "Gross margin is 77.5%, so the product is profitable. The loss comes from operating costs, not pricing.",
                "Cash of $4.30M at a burn of about $169K a month covers roughly 25 months."
            },
            Decision = "Make revenue minus expenses the first number in every management review, and set a monthly breakeven target.",
            PbiBuild = "Six KPI card visuals, one measure each: [Total Revenue], [Total Expenses], [Net Income], [Gross Margin %], [Opex vs Budget], [Cash Balance]. Keep the Quarter slicer on the page so the cards re-scope.",
            Rows = new[]
            {
                new KpiRow("Total revenue", "[Total Revenue]", "$12.59M", "Sizes the business and is the denominator for every margin."),
                new KpiRow("Total expenses", "[Total Expenses]", "$14.62M", "Set against revenue, it shows spending above income."),
                new KpiRow("Net income", "[Net Income]", "−$2.03M", "The headline result. It overrides any positive trend on the page."),
                new KpiRow("Gross margin %", "[Gross Margin %]", "77.5%", "Product economics are healthy. The loss is in operating cost."),
                new KpiRow("Opex vs budget", "[Opex vs Budget]", "+1.37%", "Unfavourable, but small and controllable."),
                new KpiRow("Cash balance", "[Cash Balance]", "$4.30M", "Answers 'can we absorb this loss?' At −$169K a month, about 25 months.")
            }
        },
        new ElementDetail
        {
            Id = "d1-trend", Dashboard = 1,
            Title = "Monthly revenue vs expenses",
            ChartType = "Clustered column",
            Question = "Is revenue catching up with spending?",
            WhatItShows = "Revenue and expenses for each month of FY2025, side by side, with the axis starting at zero. Click a month to filter the page.",
            Insights = new[]
            {
                "Revenue rises from $878K in January to $1.25M in December, up 42.1%.",
                "Expenses rise from $1.03M to $1.42M, up 37.7%, so they grow almost as fast in percentage terms.",
                "Expenses are above revenue in all 12 months, so the gap never closes."
            },
            Decision = "Cap operating-cost growth below revenue growth for at least three quarters, then re-check whether the gap is closing.",
            PbiBuild = "Clustered column chart. X: dim_month[month_name], sorted by month number. Values: [Total Revenue] and [Total Expenses]. Y-axis starts at 0. Data labels off, tooltips on."
        },
        new ElementDetail
        {
            Id = "d1-net", Dashboard = 1,
            Title = "Net income by month",
            ChartType = "Column chart with a zero line",
            Question = "Which months lose the most?",
            WhatItShows = "The profit or loss for each month, measured from a fixed zero line. Losing months drop below the line in coral.",
            Insights = new[]
            {
                "All twelve months are losses. The best is November at −$150K and the worst is May at −$188K.",
                "The range is narrow, about $38K, which points to a structural loss rather than one bad month.",
                "December loses $170K even at record revenue of $1.25M, because expenses peak at $1.42M that month."
            },
            Decision = "Set a monthly breakeven target. At December's cost base the business needs about $1.42M of revenue, roughly 13.6% above December's actual.",
            PbiBuild = "Column chart. X: dim_month[month_name]. Values: [Net Income]. Colour negatives coral and positives mint. Constant reference line at 0."
        },
        new ElementDetail
        {
            Id = "d1-cum", Dashboard = 1,
            Title = "Cumulative revenue gap",
            ChartType = "Area chart (running total)",
            Question = "Is the deficit getting worse?",
            WhatItShows = "The running total of the monthly gap between expenses and revenue. It shows the deficit as a path, not as separate months.",
            Insights = new[]
            {
                "The running total reaches −$2.03M by December, the same figure as the net income card.",
                "The line is almost straight, so the loss is steady at about $169K a month. It is neither improving nor worsening.",
                "That works out to roughly $5.6K of loss per day."
            },
            Decision = "Use the slope as an early-warning indicator. A flatter line next quarter means cost controls are working. A steeper one means the plan needs re-cutting now.",
            PbiBuild = "Area chart. X: dim_month[month_name]. Values: [Cumulative Gap], a running sum of (Expenses − Revenue) across the month sequence."
        },
        new ElementDetail
        {
            Id = "d1-slicers", Dashboard = 1,
            Title = "Slicers, cross-filtering and the takeaway panel",
            ChartType = "Interactivity",
            Question = "Does the page respond when a manager clicks?",
            WhatItShows = "The behaviour that makes a dashboard interactive rather than just a picture.",
            Insights = new[]
            {
                "Click a month and the KPI cards, net income and cumulative charts re-scope to that month. Click it again to clear.",
                "The Quarter slicer re-scopes every visual on the page at once.",
                "Drill-down goes Month to Day. Drill-through opens an account-detail page for a month, and tooltips show that month's revenue, expenses and net."
            },
            Decision = "Write every takeaway as fact, then interpretation, then action, so a manager knows what to do after reading the page.",
            PbiBuild = "Quarter slicer on dim_month[quarter]. Month visuals cross-filter each other (use Edit interactions where a visual should not filter another). Drill-through page 'Account detail' on dim_month. Tooltip page showing revenue, expenses and net per month. Keep all filters on."
        },

        // ───────────── DASHBOARD 2 · PROFITABILITY & BUDGET CONTROL ─────────────
        new ElementDetail
        {
            Id = "d2-kpi", Dashboard = 2,
            Title = "Cost KPIs: split the loss into its two halves",
            ChartType = "KPI cards + measure table",
            Question = "Where does the loss live?",
            WhatItShows = "The two halves of the cost base, checked against the approved budget.",
            Insights = new[]
            {
                "Operating expenses are 81% of total cost, so any turnaround has to start there.",
                "The overspend is $159K, about 1.4% of plan. That is a control problem, not a runaway cost problem."
            },
            Decision = "Fix the operating-cost line, not the price list.",
            PbiBuild = "Five KPI cards, one measure each. Set the card subtitle to 'Unfavourable' or 'Favourable' from the sign of the variance.",
            Callout = "Sign convention: a positive cost variance is unfavourable. Label it that way on the card so nobody reads the page backwards.",
            Rows = new[]
            {
                new KpiRow("COGS", "[COGS]", "$2.83M", "19% of total cost. The product is cheap to deliver."),
                new KpiRow("Operating expenses", "[Operating Expenses]", "$11.78M", "81% of cost. This is where the loss lives."),
                new KpiRow("Budgeted opex", "[Budgeted Opex]", "$11.62M", "The approved plan. Without it, $11.78M has no reference point."),
                new KpiRow("Opex variance", "[Opex Variance]", "+$159.45K", "Unfavourable, 1.37% over plan."),
                new KpiRow("Gross margin %", "[Gross Margin %]", "77.5%", "Confirms the fix belongs in operating costs, not pricing.")
            }
        },
        new ElementDetail
        {
            Id = "d2-dept", Dashboard = 2,
            Title = "Total cost by department",
            ChartType = "Stacked bar",
            Question = "Which departments cost the most?",
            WhatItShows = "Total cost for each department, with the cost types inside each bar. Largest first. Click a bar to filter the page.",
            Insights = new[]
            {
                "People is the largest cost centre at roughly $6.0M, about 41% of all spend.",
                "Marketing (about $2.8M) and Engineering (about $2.1M) come next. The top three make up about three quarters of spend.",
                "Cost is concentrated, so a savings plan should focus where the money is."
            },
            Decision = "Allocate any savings target by department size, not evenly. A 5% cut in People alone is worth about $300K, more than the whole budget overspend.",
            PbiBuild = "Stacked bar. Y: dim_department[department]. X: [Total Cost]. Legend: cost type. Sort by total, descending. Total labels on.",
            Callout = "Department totals were read from the Power BI bar chart, about ±$0.1M. Replace them with exact model values before presenting."
        },
        new ElementDetail
        {
            Id = "d2-trend", Dashboard = 2,
            Title = "Opex trend by department",
            ChartType = "Multi-line",
            Question = "Which costs are accelerating?",
            WhatItShows = "Monthly operating cost for each department, January to December. The slope matters more than the level.",
            Insights = new[]
            {
                "Marketing grows fastest, from about $169K in January to $288K in December, up 70%. Revenue grew 42% over the same year.",
                "People grows about 19%, from about $457K to $544K.",
                "Technology and the smaller departments grow more slowly."
            },
            Decision = "Ask Marketing to justify incremental spend against pipeline, and hold the remaining months flat until that review is done.",
            PbiBuild = "Line chart. X: dim_month[month_name]. Values: [Opex]. Legend: dim_department[department]. Y-axis starts at 0. Filter to the top departments by cost.",
            Callout = "Monthly trend points were approximated from the Power BI line chart. Start and end values match the chart; months in between are interpolated."
        },
        new ElementDetail
        {
            Id = "d2-cogs", Dashboard = 2,
            Title = "Cost of sales split",
            ChartType = "Donut (two parts)",
            Question = "Is the product itself too expensive to deliver?",
            WhatItShows = "COGS split into its two delivery streams: software and services.",
            Insights = new[]
            {
                "Software is $2.10M (74.2%) and services is $0.73M (25.8%).",
                "With revenue of $12.59M, that gives a 77.5% gross margin, which is strong for this type of business.",
                "The −$2.03M loss is not a product-cost problem. Raising prices would not close it."
            },
            Decision = "Leave pricing alone. Put the improvement effort into operating expenses of $11.78M, where a 5% reduction is worth about $589K.",
            PbiBuild = "Donut. Legend: cost-of-sales subtype. Values: [COGS]. Keep it to two slices, since a donut only reads well with two or three."
        },
        new ElementDetail
        {
            Id = "d2-budget", Dashboard = 2,
            Title = "Opex vs budget variance",
            ChartType = "Bar with variance",
            Question = "Which departments are over budget?",
            WhatItShows = "Opex variance by department against the approved plan. Coral means overspent. Mint means under budget.",
            Insights = new[]
            {
                "Total variance is +$159.5K, unfavourable.",
                "People (+$87.2K) and Marketing (+$53.0K) make up about 88% of the overrun.",
                "Technology (+$16.5K) and General (+$10.5K) add a little more. Legal is $8.3K under budget."
            },
            Decision = "Fix the two largest overruns with targeted action, not a company-wide freeze. Require approval for any department that goes above budget.",
            PbiBuild = "Bar chart. Y: dim_department[department]. X: [Opex Variance]. Colour by sign (positive coral, negative mint). Zero reference line."
        },

        // ───────────── DASHBOARD 3 · CASH, RECEIVABLES & PAYABLES ─────────────
        new ElementDetail
        {
            Id = "d3-kpi", Dashboard = 3,
            Title = "Liquidity KPIs",
            ChartType = "KPI cards + measure table",
            Question = "Can we pay our bills, and is cash actually being collected?",
            WhatItShows = "The six liquidity numbers a treasurer checks first, with the definition behind each one.",
            Insights = new[]
            {
                "Customers take about 83 days to pay, while suppliers are paid in about 22 days. Working capital is tied up in receivables.",
                "Cash of $4.30M is the real liquidity figure. Collections are the lever.",
                "The NWC card ($2.00M = AR − AP) and the bridge total ($6.30M = cash + AR − AP) use different definitions. Pick one before presenting."
            },
            Decision = "Make collections the priority in the cash plan. Faster follow-up on customer invoices would release more cash than stretching supplier payments.",
            PbiBuild = "Six KPI cards, one measure each: [Cash Balance], [AR Outstanding], [AP Outstanding], [DSO], [DPO], [Net Working Capital]. Use the same definition for NWC here and in the bridge.",
            Callout = "Check the NWC definition on this page and in the bridge. They should match before you present.",
            Rows = new[]
            {
                new KpiRow("Cash balance", "[Cash Balance]", "$4.30M", "Cash across all bank accounts."),
                new KpiRow("AR outstanding", "[AR Outstanding]", "$2.86M", "Invoiced but not yet collected."),
                new KpiRow("AP outstanding", "[AP Outstanding]", "$0.86M", "Owed to suppliers."),
                new KpiRow("DSO", "[DSO]", "83 days", "How long customers take to pay. This is long."),
                new KpiRow("DPO", "[DPO]", "22 days", "How long we take to pay suppliers. This is short."),
                new KpiRow("Net working capital", "[Net Working Capital]", "$2.00M", "Card value, equal to AR − AP.")
            }
        },
        new ElementDetail
        {
            Id = "d3-bank", Dashboard = 3,
            Title = "Cash balance by bank",
            ChartType = "Horizontal bar",
            Question = "Where is our cash held?",
            WhatItShows = "Cash balance by bank account. Use the bank slicer to highlight one bank.",
            Insights = new[]
            {
                "JPMorgan Chase holds $3.5M, about 81% of cash. Bank of America holds $0.8M, about 19%.",
                "Most cash sits in one bank, so the company has concentration risk."
            },
            Decision = "Confirm deposit protection and credit limits with JPMorgan. Decide whether a second operating account is needed for resilience.",
            PbiBuild = "Bar chart. Y: dim_bank[bank_name]. X: [Cash Balance]. Slicer: bank_name.",
            Callout = "The cash-by-currency donut in the Power BI report totals $25.03M, which does not match the $4.30M cash balance. Review its measure before showing it."
        },
        new ElementDetail
        {
            Id = "d3-ar", Dashboard = 3,
            Title = "Accounts receivable aging",
            ChartType = "Horizontal bar by aging bucket",
            Question = "Are customers paying on time?",
            WhatItShows = "Open receivables grouped by how long they have been outstanding. Buckets run from current to 90+ days, always in that order.",
            Insights = new[]
            {
                "About $2.2M of the $2.86M receivables, roughly 77%, is over 90 days old.",
                "Only about $0.6M sits in the 1 to 90 day buckets. The problem is the old tail, not new invoices.",
                "The 83-day DSO reflects this aged tail. The average hides how old the book really is."
            },
            Decision = "Escalate the 90+ balance to formal collections now. Review the largest accounts one by one, and decide whether a bad-debt provision is needed.",
            PbiBuild = "Bar chart. Y: aging bucket with a fixed sort order, not alphabetical. X: [AR Outstanding]. Bucket each invoice by its age using a calculated column.",
            Callout = "Bucket amounts were read from the Power BI bar chart and rounded. Confirm the exact values in the model."
        },
        new ElementDetail
        {
            Id = "d3-ap", Dashboard = 3,
            Title = "Accounts payable aging",
            ChartType = "Horizontal bar by aging bucket",
            Question = "Are old bills stuck, or are we paying suppliers quickly?",
            WhatItShows = "Open payables by age, from current to 90+ days.",
            Insights = new[]
            {
                "About $0.55M of the $0.86M payables, roughly 64%, is over 90 days old.",
                "A large 90+ balance sits oddly beside a 22-day DPO. DPO is an average, so this is not automatically a contradiction, but it needs checking.",
                "Possible causes are disputed bills, stale bills, or a date problem in the bill data."
            },
            Decision = "Reconcile the 90+ payables with suppliers. Pay the valid ones, and dispute or clear the rest with a documented reason.",
            PbiBuild = "Bar chart. Y: aging bucket, fixed order. X: [AP Outstanding]. Use the same bucket logic as receivables.",
            Callout = "Check the bill dates and open amounts against this DPO card before presenting."
        },
        new ElementDetail
        {
            Id = "d3-wc", Dashboard = 3,
            Title = "Working capital bridge",
            ChartType = "Waterfall",
            Question = "Where does the liquidity number come from?",
            WhatItShows = "How cash, receivables and payables combine into the working-capital figure.",
            Insights = new[]
            {
                "Cash of $4.30M plus receivables of $2.86M minus payables of $0.86M gives $6.30M.",
                "Receivables are not cash yet. Most of the $2.86M is over 90 days old, so it is weak headroom.",
                "Use the $4.30M cash balance, not the $6.30M bridge total, when arguing liquidity headroom."
            },
            Decision = "Present cash of $4.30M as the headroom figure until receivables are collected.",
            PbiBuild = "Waterfall built as a stacked column with an invisible base. Categories: Cash, + AR, − AP, Net WC.",
            Callout = "The bridge uses cash + AR − AP. The NWC card uses AR − AP. Align them before the presentation."
        },
        new ElementDetail
        {
            Id = "d3-fx", Dashboard = 3,
            Title = "Cash by currency",
            ChartType = "Doughnut chart · legend in the report: SEK, USD, EUR, GBP",
            Question = "How is the cash balance spread across currencies?",
            WhatItShows = "The cash balance split into four currency slices as displayed on the Power BI page. The legend lists SEK, USD, EUR and GBP, and the donut reports a total of $25.03M.",
            Insights = new[]
            {
                "The report shows four slices: $14.20M (56.7%), $4.30M (17.2%), $3.68M (14.7%) and $2.85M (11.4%).",
                "The donut totals $25.03M, which does not match the $4.30M cash balance on the same page. Review the measure before quoting the number.",
                "The legend lists SEK, USD, EUR and GBP, but the slice-to-currency mapping was not readable in the report, so currency exposure cannot be attributed yet."
            },
            Decision = "Reconcile the donut measure to the cash balance and confirm which slice is which currency before presenting FX exposure.",
            PbiBuild = "Doughnut chart. Legend: cash update 2026[currency]. Values: [Cash Balance] converted to USD, using the same filter context as the cash KPI card.",
            Callout = "Verify before presenting: the $25.03M total and the slice-to-currency mapping both need confirming in the model."
        }
    };
}
