namespace KovaDashboard.Models;

public record NamedValue(string Name, double Value);

public record Series(string Name, double[] Values);

public record KpiRow(string Name, string Measure, string Value, string Meaning);

public sealed class ElementDetail
{
    public required string Id { get; init; }
    public required int Dashboard { get; init; }
    public required string Title { get; init; }
    public required string ChartType { get; init; }
    public required string Question { get; init; }
    public required string WhatItShows { get; init; }
    public required string[] Insights { get; init; }
    public required string Decision { get; init; }
    public required string PbiBuild { get; init; }
    public KpiRow[]? Rows { get; init; }
    public string? Callout { get; init; }
}

public record ElementPage(
    ElementDetail Item,
    string Position,
    ElementDetail? Prev,
    ElementDetail? Next);
