(function () {
    "use strict";

    var C = {
        blue: "#6FA8FF", violet: "#A78BFA", mint: "#3DDC97", coral: "#FF6B7A",
        amber: "#FFC24B", orange: "#FF8A5B", grid: "#1B2740", muted: "#8CA0BE"
    };

    var SERIES_COLOR = { People: C.blue, Marketing: C.violet, Technology: C.mint, General: C.amber };

    var DATA = null;
    function data() {
        if (!DATA) DATA = JSON.parse(document.getElementById("kova-data").textContent);
        return DATA;
    }

    // Power BI style: items outside the current selection are faded.
    function dim(hex, on) { return on ? hex : hex + "40"; }

    // $K in, readable string out: 2028 -> "$2.03M", -152 -> "−$152K"
    function usdK(k) {
        var a = Math.abs(k);
        var s = a >= 1000 ? (a / 1000).toFixed(2) + "M" : Math.round(a) + "K";
        return (k < 0 ? "−$" : "$") + s;
    }

    function setDefaults() {
        Chart.defaults.color = C.muted;
        Chart.defaults.font.family = "'Segoe UI', system-ui, -apple-system, Arial, sans-serif";
        Chart.defaults.font.size = 11;
        Chart.defaults.borderColor = C.grid;
        Chart.defaults.plugins.legend.labels.boxWidth = 10;
        Chart.defaults.plugins.tooltip.backgroundColor = "#0E1727";
        Chart.defaults.plugins.tooltip.borderColor = "#2A3B57";
        Chart.defaults.plugins.tooltip.borderWidth = 1;
    }

    function base(o) {
        return Object.assign({
            responsive: true,
            maintainAspectRatio: false,
            animation: { duration: 250 },
            plugins: { legend: { display: false } }
        }, o);
    }

    function xAxis(extra) {
        return Object.assign({ grid: { display: false } }, extra || {});
    }

    function yAxis(fmt, extra) {
        var y = { grid: { color: C.grid }, ticks: { callback: function (v) { return fmt ? fmt(v) : v; } } };
        return Object.assign(y, extra || {});
    }

    /* ───────────────── Presentation deck ───────────────── */
    function initDeck() {
        var deck = document.getElementById("deck");
        if (!deck) return;

        var slides = Array.prototype.slice.call(deck.querySelectorAll(".slide"));
        var dots = document.getElementById("dots");
        var cnt = document.getElementById("cnt");
        var cur = 0;

        slides.forEach(function (_, i) {
            var b = document.createElement("i");
            b.addEventListener("click", function () { go(i); });
            dots.appendChild(b);
        });

        function go(i) {
            if (i < 0 || i >= slides.length) return;
            slides[cur].classList.remove("active");
            dots.children[cur].classList.remove("on");
            cur = i;
            slides[cur].classList.add("active");
            dots.children[cur].classList.add("on");
            cnt.textContent = (cur + 1) + " / " + slides.length;
        }

        document.getElementById("prev").addEventListener("click", function () { go(cur - 1); });
        document.getElementById("next").addEventListener("click", function () { go(cur + 1); });

        document.addEventListener("keydown", function (e) {
            var t = e.target.tagName;
            if (t === "INPUT" || t === "SELECT" || t === "TEXTAREA") return;
            if (e.key === "ArrowRight" || e.key === "PageDown" || e.key === " ") { e.preventDefault(); go(cur + 1); }
            if (e.key === "ArrowLeft" || e.key === "PageUp") { e.preventDefault(); go(cur - 1); }
        });

        slides[0].classList.add("active");
        dots.children[0].classList.add("on");
        cnt.textContent = "1 / " + slides.length;
    }

    /* ───────────────── Dashboard 1 · Executive ───────────────── */
    function initExecutive() {
        var root = document.getElementById("exec-root");
        if (!root) return;

        var d = data();
        var quarter = document.getElementById("exec-quarter");
        var chip = document.getElementById("exec-chip");
        var chipVal = document.getElementById("exec-sel");
        var clear = document.getElementById("exec-clear");
        var scope = document.getElementById("exec-scope");
        var sel = null; // clicked month index

        function isOn(i) {
            if (sel !== null) return i === sel;
            if (quarter.value !== "all") return Math.floor(i / 3) === Number(quarter.value);
            return true;
        }
        function flags() { return d.months.map(function (_, i) { return isOn(i); }); }
        function total(arr, f) {
            var t = 0;
            arr.forEach(function (v, i) { if (f[i]) t += v; });
            return t;
        }

        function onMonth(evt, els) {
            if (!els.length) return;
            var i = els[0].index;
            sel = (sel === i) ? null : i;
            refresh();
        }

        var trend = new Chart(document.getElementById("c-trend"), {
            type: "bar",
            data: {
                labels: d.months,
                datasets: [
                    { label: "Revenue", data: d.revenue, backgroundColor: C.blue, borderRadius: 3 },
                    { label: "Expenses", data: d.expenses, backgroundColor: C.orange, borderRadius: 3 }
                ]
            },
            options: base({
                onClick: onMonth,
                interaction: { mode: "index", intersect: false },
                plugins: { legend: { display: true, position: "top", align: "end" } },
                scales: { x: xAxis(), y: yAxis(usdK) }
            })
        });

        var net = new Chart(document.getElementById("c-net"), {
            type: "bar",
            data: {
                labels: d.months,
                datasets: [{ label: "Net income", data: d.net, backgroundColor: C.coral, borderRadius: 3 }]
            },
            options: base({
                onClick: onMonth,
                interaction: { mode: "index", intersect: false },
                scales: {
                    x: xAxis({ position: "bottom" }),
                    y: yAxis(usdK, { max: 0 })
                }
            })
        });

        var cum = new Chart(document.getElementById("c-cum"), {
            type: "line",
            data: {
                labels: d.months,
                datasets: [{
                    label: "Cumulative gap",
                    data: d.cumulative,
                    borderColor: C.coral,
                    backgroundColor: "rgba(255,107,122,0.18)",
                    fill: "origin",
                    tension: 0.25,
                    pointRadius: 3,
                    pointBackgroundColor: C.coral
                }]
            },
            options: base({
                interaction: { mode: "index", intersect: false },
                scales: {
                    x: xAxis({ position: "bottom" }),
                    y: yAxis(usdK, { max: 0 })
                }
            })
        });

        function refresh() {
            var f = flags();
            var rev = total(d.revenue, f);
            var exp = total(d.expenses, f);
            var nt = rev - exp;

            document.getElementById("k-rev").textContent = usdK(rev);
            document.getElementById("k-exp").textContent = usdK(exp);
            document.getElementById("k-net").textContent = usdK(nt);

            if (sel !== null) {
                scope.textContent = d.months[sel] + " 2025";
                chipVal.textContent = d.months[sel];
            } else if (quarter.value === "all") {
                scope.textContent = "FY2025 · all months";
            } else {
                scope.textContent = "Q" + (Number(quarter.value) + 1) + " 2025";
            }
            chip.hidden = sel === null;

            trend.data.datasets[0].backgroundColor = f.map(function (x) { return dim(C.blue, x); });
            trend.data.datasets[1].backgroundColor = f.map(function (x) { return dim(C.orange, x); });
            trend.update();

            net.data.datasets[0].backgroundColor = f.map(function (x) { return dim(C.coral, x); });
            net.update();

            cum.data.datasets[0].pointRadius = f.map(function (x) { return x ? 4 : 2; });
            cum.update();
        }

        quarter.addEventListener("change", function () { sel = null; refresh(); });
        clear.addEventListener("click", function () { sel = null; refresh(); });
        refresh();
    }

    /* ───────────────── Dashboard 2 · Profitability ───────────────── */
    function initProfitability() {
        var root = document.getElementById("prof-root");
        if (!root) return;

        var d = data();
        var sel = document.getElementById("prof-dept");
        var scope = document.getElementById("prof-scope");
        var kVar = document.getElementById("k-var");
        var chosen = "all";

        var names = d.departments.map(function (x) { return x.name; });
        var vNames = d.variance.map(function (x) { return x.name; });
        var vVals = d.variance.map(function (x) { return x.value; });
        var varByName = {};
        d.variance.forEach(function (x) { varByName[x.name] = x.value; });

        names.forEach(function (n) {
            var o = document.createElement("option");
            o.value = n; o.textContent = n;
            sel.appendChild(o);
        });

        function isOn(name) { return chosen === "all" || chosen === name; }
        function pick(name) { chosen = (chosen === name) ? "all" : name; refresh(); }

        var dept = new Chart(document.getElementById("c-dept"), {
            type: "bar",
            data: {
                labels: names,
                datasets: [{
                    label: "Total cost",
                    data: d.departments.map(function (x) { return x.value; }),
                    backgroundColor: C.blue,
                    borderRadius: 3
                }]
            },
            options: base({
                indexAxis: "y",
                onClick: function (evt, els) { if (els.length) pick(names[els[0].index]); },
                scales: {
                    x: yAxis(usdK),
                    y: { grid: { display: false } }
                }
            })
        });

        var cogs = new Chart(document.getElementById("c-cogs"), {
            type: "doughnut",
            data: {
                labels: d.cogs.map(function (x) { return x.name; }),
                datasets: [{
                    data: d.cogs.map(function (x) { return x.value; }),
                    backgroundColor: [C.blue, C.violet],
                    borderColor: "#121C2E",
                    borderWidth: 3
                }]
            },
            options: base({
                cutout: "66%",
                plugins: { legend: { display: true, position: "bottom" } }
            })
        });

        var trend = new Chart(document.getElementById("c-trend"), {
            type: "line",
            data: {
                labels: d.months,
                datasets: d.opexTrend.map(function (s) {
                    return {
                        label: s.name,
                        data: s.values,
                        borderColor: SERIES_COLOR[s.name] || C.muted,
                        backgroundColor: SERIES_COLOR[s.name] || C.muted,
                        borderWidth: 2,
                        pointRadius: 0,
                        tension: 0.35
                    };
                })
            },
            options: base({
                interaction: { mode: "index", intersect: false },
                plugins: { legend: { display: true, position: "top", align: "end" } },
                scales: {
                    x: xAxis(),
                    y: yAxis(usdK, { beginAtZero: true })
                }
            })
        });

        var variance = new Chart(document.getElementById("c-var"), {
            type: "bar",
            data: {
                labels: vNames,
                datasets: [{
                    label: "Variance vs budget",
                    data: vVals,
                    backgroundColor: vVals.map(function (v) { return v > 0 ? C.coral : (v < 0 ? C.mint : C.muted); }),
                    borderRadius: 3
                }]
            },
            options: base({
                indexAxis: "y",
                onClick: function (evt, els) { if (els.length) pick(vNames[els[0].index]); },
                scales: {
                    x: yAxis(function (v) { return (v > 0 ? "+" : "") + v + "K"; }),
                    y: { grid: { display: false } }
                }
            })
        });

        function refresh() {
            sel.value = chosen;
            scope.textContent = chosen === "all" ? "All departments" : chosen;

            dept.data.datasets[0].backgroundColor = names.map(function (n) { return dim(C.blue, isOn(n)); });
            dept.update();

            variance.data.datasets[0].backgroundColor = vNames.map(function (n, i) {
                var baseColor = vVals[i] > 0 ? C.coral : (vVals[i] < 0 ? C.mint : C.muted);
                return dim(baseColor, isOn(n));
            });
            variance.update();

            trend.data.datasets.forEach(function (ds) {
                var c = SERIES_COLOR[ds.label] || C.muted;
                ds.borderColor = dim(c, isOn(ds.label));
                ds.borderWidth = chosen === ds.label ? 3.5 : 2;
            });
            trend.update();

            cogs.data.datasets[0].backgroundColor = [
                dim(C.blue, chosen === "all" || chosen === "Engineering"),
                dim(C.violet, chosen === "all" || chosen === "Services")
            ];
            cogs.update();

            if (chosen === "all") {
                kVar.textContent = "+$159.45K";
            } else if (varByName.hasOwnProperty(chosen)) {
                var v = varByName[chosen];
                kVar.textContent = (v > 0 ? "+$" : (v < 0 ? "−$" : "$")) + Math.abs(v).toFixed(1) + "K";
            } else {
                kVar.textContent = "n/a";
            }
        }

        sel.addEventListener("change", function () { chosen = sel.value; refresh(); });
        refresh();
    }

    /* ───────────────── Dashboard 3 · Cash ───────────────── */
    function initCash() {
        var root = document.getElementById("cash-root");
        if (!root) return;

        var d = data();
        var sel = document.getElementById("cash-bank");
        var scope = document.getElementById("cash-scope");
        var kCash = document.getElementById("k-cash");
        var chosen = "all";

        var bankNames = d.banks.map(function (b) { return b.name; });
        var bankColors = [C.blue, C.violet];

        function bankOn(name) { return chosen === "all" || chosen === name; }

        var bank = new Chart(document.getElementById("c-bank"), {
            type: "bar",
            data: {
                labels: bankNames,
                datasets: [{
                    data: d.banks.map(function (b) { return b.value; }),
                    backgroundColor: bankColors,
                    borderRadius: 3
                }]
            },
            options: base({
                indexAxis: "y",
                onClick: function (evt, els) {
                    if (!els.length) return;
                    var name = bankNames[els[0].index];
                    chosen = chosen === name ? "all" : name;
                    refresh();
                },
                scales: { x: yAxis(usdK), y: { grid: { display: false } } }
            })
        });

        function bucketChart(id, rows, colors) {
            return new Chart(document.getElementById(id), {
                type: "bar",
                data: {
                    labels: rows.map(function (r) { return r.name; }),
                    datasets: [{
                        data: rows.map(function (r) { return r.value; }),
                        backgroundColor: colors,
                        borderRadius: 3
                    }]
                },
                options: base({
                    indexAxis: "y",
                    scales: { x: yAxis(usdK), y: { grid: { display: false } } }
                })
            });
        }

        var bucketColors = [C.mint, C.blue, C.amber, C.orange, C.coral];
        bucketChart("c-ar", d.ar, bucketColors);
        bucketChart("c-ap", d.ap, bucketColors);

        // Waterfall: invisible base + visible step
        var wc = new Chart(document.getElementById("c-wc"), {
            type: "bar",
            data: {
                labels: ["Cash", "+ AR", "− AP", "Net WC"],
                datasets: [
                    { data: [0, 4300, 6300, 0], backgroundColor: "rgba(0,0,0,0)", stack: "s" },
                    {
                        data: [4300, 2860, 860, 6300],
                        backgroundColor: [C.blue, C.mint, C.coral, C.blue],
                        stack: "s",
                        borderRadius: 3
                    }
                ]
            },
            options: base({
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        filter: function (item) { return item.datasetIndex === 1; },
                        callbacks: { label: function (c) { return usdK(c.parsed.y); } }
                    }
                },
                scales: {
                    x: { stacked: true, grid: { display: false } },
                    y: yAxis(usdK, { stacked: true })
                }
            })
        });

        function refresh() {
            sel.value = chosen;
            scope.textContent = chosen === "all" ? "All banks" : chosen;

            bank.data.datasets[0].backgroundColor = bankNames.map(function (n, i) {
                return dim(bankColors[i], bankOn(n));
            });
            bank.update();

            if (chosen === "all") {
                kCash.textContent = "$4.30M";
            } else {
                var row = d.banks.find(function (b) { return b.name === chosen; });
                kCash.textContent = row ? usdK(row.value) : "$4.30M";
            }
        }

        sel.addEventListener("change", function () { chosen = sel.value; refresh(); });
        refresh();
    }

    document.addEventListener("DOMContentLoaded", function () {
        initDeck();
        if (!window.Chart) return;
        setDefaults();
        initExecutive();
        initProfitability();
        initCash();
    });
})();
