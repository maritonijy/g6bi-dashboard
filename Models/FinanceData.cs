using System.Text.Json;

namespace KovaDashboard.Models;

/// <summary>
/// FY2025 figures for Kova Inc.
/// Monthly revenue and expenses are in $K and tie to the KPI cards.
/// Department totals, opex trend lines and AR/AP buckets were read from the
/// Power BI charts, so they are approximate. Replace them with exact values
/// from the .pbix when you have them.
/// </summary>
public static class FinanceData
{
    public static readonly string[] Months =
        { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    public static readonly int[] Revenue =
        { 878, 882, 915, 955, 992, 1030, 1060, 1100, 1140, 1180, 1210, 1248 };

    public static readonly int[] Expenses =
        { 1030, 1060, 1100, 1130, 1180, 1200, 1230, 1260, 1310, 1340, 1360, 1418 };

    public static readonly int[] Net =
        Enumerable.Range(0, 12).Select(i => Revenue[i] - Expenses[i]).ToArray();

    public static readonly int[] Cumulative =
        Net.Select((_, i) => Net.Take(i + 1).Sum()).ToArray();

    // Total cost by department ($K), read from the bar chart (approx.)
    public static readonly NamedValue[] DepartmentCost =
    {
        new("People", 6000),
        new("Marketing", 2850),
        new("Engineering", 2100),
        new("Technology", 1300),
        new("Services", 730),
        new("General", 500),
        new("Facilities", 500),
        new("Finance", 300),
        new("Legal", 300)
    };

    // Opex variance vs 100% budget by department ($K), from the PBI page 2 labels
    public static readonly NamedValue[] OpexVariance =
    {
        new("People", 87.2),
        new("Marketing", 53.0),
        new("Technology", 16.5),
        new("General", 10.5),
        new("Facilities", 0.6),
        new("Finance", 0.0),
        new("Legal", -8.3)
    };

    // Monthly opex by department ($K). Start and end match the chart; months are interpolated.
    public static readonly Series[] OpexTrend =
    {
        new("People",     new double[] { 457, 465, 473, 481, 489, 497, 505, 513, 520, 528, 536, 544 }),
        new("Marketing",  new double[] { 169, 180, 191, 202, 212, 223, 234, 245, 256, 266, 277, 288 }),
        new("Technology", new double[] {  99, 101, 104, 106, 108, 111, 113, 115, 118, 120, 122, 124 }),
        new("General",    new double[] {  29,  31,  32,  34,  35,  37,  38,  40,  41,  43,  44,  46 })
    };

    // Cash by bank ($K), from PBI page 3
    public static readonly NamedValue[] CashByBank =
    {
        new("JPMorgan Chase", 3500),
        new("Bank of America", 800)
    };

    // Open AR by aging bucket ($K), read from the chart; total ties to $2.86M
    public static readonly NamedValue[] ArAging =
    {
        new("Current", 0),
        new("1–30 days", 220),
        new("31–60 days", 210),
        new("61–90 days", 200),
        new("90+ days", 2230)
    };

    // Open AP by aging bucket ($K), read from the chart; total ties to $0.86M
    public static readonly NamedValue[] ApAging =
    {
        new("Current", 10),
        new("1–30 days", 100),
        new("31–60 days", 100),
        new("61–90 days", 100),
        new("90+ days", 550)
    };

    // COGS split ($K), from PBI page 2 donut
    public static readonly NamedValue[] CogsSplit =
    {
        new("Software", 2100),
        new("Services", 730)
    };

    // Cash-by-currency doughnut ($K), read from the report. The report total is
    // $25.03M, which does not reconcile to the $4.30M cash balance, and the
    // slice-to-currency mapping could not be read from the chart.
    public static readonly NamedValue[] CashByCurrency =
    {
        new("Slice 1", 14200),
        new("Slice 2", 4300),
        new("Slice 3", 3680),
        new("Slice 4", 2850)
    };

    public static string ToJson() => JsonSerializer.Serialize(new
    {
        months = Months,
        revenue = Revenue,
        expenses = Expenses,
        net = Net,
        cumulative = Cumulative,
        departments = DepartmentCost,
        variance = OpexVariance,
        opexTrend = OpexTrend,
        banks = CashByBank,
        ar = ArAging,
        ap = ApAging,
        cogs = CogsSplit,
        fx = CashByCurrency
    }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
}
