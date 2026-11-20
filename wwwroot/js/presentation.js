(function () {
    "use strict";

    var root = document.getElementById("pp");
    var frame = document.getElementById("ppFrame");
    if (!root || !frame) return;

    var stage = document.getElementById("ppStage");
    var nav = document.getElementById("ppNav");
    var thumbs = document.getElementById("ppThumbs");
    var counter = document.getElementById("ppCounter");
    var slides = Array.prototype.slice.call(frame.querySelectorAll(".pp-slide"));
    var current = 0;

    var C = {
        blue: "#6FA8FF", violet: "#A78BFA", mint: "#3DDC97", coral: "#FF6B7A",
        amber: "#FFC24B", orange: "#FF8A5B", grid: "#1B2740", muted: "#8CA0BE"
    };

    function usdK(k) {
        var a = Math.abs(k);
        var s = a >= 1000 ? (a / 1000).toFixed(2) + "M" : Math.round(a) + "K";
        return (k < 0 ? "\u2212$" : "$") + s;
    }

    function yAxis(fmt, extra) {
        var axis = {
            grid: { color: C.grid },
            border: { display: false },
            ticks: { callback: function (v) { return fmt ? fmt(v) : v; } }
        };
        return Object.assign(axis, extra || {});
    }

    /* ───────── Left panel and thumbnails, built from the slides ───────── */

    var navItems = [];
    var thumbItems = [];

    function buildChrome() {
        var lastGroup = null;

        slides.forEach(function (slide, index) {
            var group = slide.dataset.group || "Slides";
            if (group !== lastGroup) {
                var heading = document.createElement("div");
                heading.className = "pp-grp";
                heading.textContent = group;
                nav.appendChild(heading);
                lastGroup = group;
            }

            var item = document.createElement("button");
            item.type = "button";
            item.className = "pp-navitem";
            item.innerHTML = '<span class="n">' + (index + 1) + '</span><span class="t"></span>';
            item.querySelector(".t").textContent = slide.dataset.nav || slide.dataset.title || "Slide " + (index + 1);
            item.title = (index + 1) + ". " + (slide.dataset.title || "");
            item.addEventListener("click", function () { show(index); });
            nav.appendChild(item);
            navItems.push(item);

            var thumb = document.createElement("button");
            thumb.type = "button";
            thumb.className = "pp-thumb";
            thumb.textContent = String(index + 1);
            thumb.title = (index + 1) + ". " + (slide.dataset.title || "");
            thumb.setAttribute("aria-label", "Go to slide " + (index + 1));
            thumb.addEventListener("click", function () { show(index); });
            thumbs.appendChild(thumb);
            thumbItems.push(thumb);
        });
    }

    function centreThumb(index) {
        var thumb = thumbItems[index];
        if (!thumb) return;
        var wrap = thumb.parentElement.parentElement;
        var left = thumb.offsetLeft - wrap.clientWidth / 2 + thumb.clientWidth / 2;
        if (wrap.scrollTo) wrap.scrollTo({ left: Math.max(0, left), behavior: "smooth" });
    }

    /* ───────── Navigation ───────── */

    function show(index) {
        if (index < 0 || index >= slides.length || index === current && slides[index].classList.contains("on")) return;

        slides.forEach(function (slide, i) {
            slide.classList.toggle("on", i === index);
            slide.setAttribute("aria-hidden", i === index ? "false" : "true");
        });

        current = index;

        navItems.forEach(function (item, i) { item.classList.toggle("on", i === index); });
        thumbItems.forEach(function (thumb, i) { thumb.classList.toggle("on", i === index); });
        counter.textContent = (index + 1) + " of " + slides.length;
        centreThumb(index);

        var slide = slides[index];

        // The interactive report is only loaded when its slide is first opened.
        var iframe = slide.querySelector("iframe[data-src]");
        if (iframe && !iframe.getAttribute("src")) {
            iframe.src = iframe.dataset.src;
            iframe.addEventListener("load", fitReports);
        }

        // Charts are drawn once, when first shown.
        slide.querySelectorAll("canvas[id^='pp-chart-']").forEach(drawChart);

        fitReports();

        if (history.replaceState) history.replaceState(null, "", "#slide-" + (index + 1));
    }

    function syncFullscreenUi(on) {
        document.getElementById("ppFs").textContent = on ? "Exit" : "Full screen";
        document.getElementById("ppFsTop").textContent = on ? "Exit" : "Present";
    }

    // If the browser refuses the Fullscreen API (embedded panels do), expand the
    // deck to fill the window instead, so Present always works.
    function setFauxFullscreen(on) {
        root.classList.toggle("pp-faux-fs", on);
        document.body.classList.toggle("pp-faux-fs", on);
        syncFullscreenUi(on);
        setTimeout(fit, 60);
    }

    function toggleFullscreen() {
        if (document.fullscreenElement) {
            if (document.exitFullscreen) document.exitFullscreen();
            return;
        }
        if (root.classList.contains("pp-faux-fs")) {
            setFauxFullscreen(false);
            return;
        }
        if (root.requestFullscreen) {
            var settled = false;
            // Some embedded browsers never settle the promise; fall back anyway.
            var guard = setTimeout(function () {
                if (!settled && !document.fullscreenElement) setFauxFullscreen(true);
            }, 700);

            try {
                var request = root.requestFullscreen();
                if (request && request.then) {
                    request.then(function () {
                        settled = true;
                        clearTimeout(guard);
                        syncFullscreenUi(true);
                        setTimeout(fit, 60);
                    }).catch(function () {
                        settled = true;
                        clearTimeout(guard);
                        setFauxFullscreen(true);
                    });
                }
            } catch (error) {
                clearTimeout(guard);
                setFauxFullscreen(true);
            }
        } else {
            setFauxFullscreen(true);
        }
    }

    /* ───────── Sizing: fit the 16:9 frame, scale each 1280×720 canvas ───────── */

    function fit() {
        if (!stage) return;
        var cs = getComputedStyle(stage);
        var padX = (parseFloat(cs.paddingLeft) || 0) + (parseFloat(cs.paddingRight) || 0);
        var padY = (parseFloat(cs.paddingTop) || 0) + (parseFloat(cs.paddingBottom) || 0);
        var availW = stage.clientWidth - padX;
        var availH = stage.clientHeight - padY;
        if (availW < 40 || availH < 40) return;

        var width = Math.min(availW, availH * 16 / 9);
        var height = width * 9 / 16;

        frame.style.width = width + "px";
        frame.style.height = height + "px";
        frame.style.setProperty("--pps", (width / 1280).toFixed(5));

        fitReports();
    }

    function fitReports() {
        frame.querySelectorAll(".pp-slide.on .pp-report").forEach(function (box) {
            var fitBox = box.querySelector(".pp-report-fit");
            if (!fitBox) return;
            var w = box.clientWidth;
            var h = box.clientHeight;
            if (w < 10 || h < 10) return;
            fitBox.style.setProperty("--pprs", Math.min(w / 1600, h / 760).toFixed(5));
        });
    }

    /* ───────── Element visuals (same data and palette as the dashboards) ───────── */

    var DATA = null;
    var chartsDrawn = {};

    function data() {
        if (!DATA) DATA = JSON.parse(document.getElementById("kova-data").textContent);
        return DATA;
    }

    function chartDefaults() {
        Chart.defaults.color = C.muted;
        Chart.defaults.font.family = "'Segoe UI', system-ui, -apple-system, Arial, sans-serif";
        Chart.defaults.font.size = 11.5;
        Chart.defaults.borderColor = C.grid;
        Chart.defaults.plugins.legend.labels.boxWidth = 10;
        Chart.defaults.plugins.legend.labels.color = "#B9C9E0";
        Chart.defaults.plugins.tooltip.backgroundColor = "#0E1727";
        Chart.defaults.plugins.tooltip.borderColor = "#2A3B57";
        Chart.defaults.plugins.tooltip.borderWidth = 1;
        Chart.defaults.plugins.tooltip.titleColor = "#E9EFF9";
        Chart.defaults.plugins.tooltip.bodyColor = "#C9D7EC";
    }

    function base(extra) {
        var options = Object.assign({
            responsive: true,
            maintainAspectRatio: false,
            animation: { duration: 220 },
            plugins: {}
        }, extra || {});
        options.plugins = Object.assign({ legend: { display: false } }, options.plugins || {});
        return options;
    }

    var builders = {
        "d1-trend": function (canvas) {
            var d = data();
            return new Chart(canvas, {
                type: "bar",
                data: {
                    labels: d.months,
                    datasets: [
                        { label: "Revenue", data: d.revenue, backgroundColor: C.blue, borderRadius: 3 },
                        { label: "Expenses", data: d.expenses, backgroundColor: C.orange, borderRadius: 3 }
                    ]
                },
                options: base({
                    interaction: { mode: "index", intersect: false },
                    plugins: {
                        legend: { display: true, position: "top", align: "end" },
                        tooltip: { callbacks: { label: function (c) { return c.dataset.label + ": " + usdK(c.parsed.y); } } }
                    },
                    scales: {
                        x: { grid: { display: false }, border: { display: false } },
                        y: yAxis(usdK, { beginAtZero: true })
                    }
                })
            });
        },
        "d1-net": function (canvas) {
            var d = data();
            return new Chart(canvas, {
                type: "bar",
                data: {
                    labels: d.months,
                    datasets: [{ label: "Net income", data: d.net, backgroundColor: C.coral, borderRadius: 3 }]
                },
                options: base({
                    interaction: { mode: "index", intersect: false },
                    plugins: {
                        tooltip: { callbacks: { label: function (c) { return "Net income: " + usdK(c.parsed.y); } } }
                    },
                    scales: {
                        x: { grid: { display: false }, border: { display: false } },
                        y: yAxis(usdK, { max: 0 })
                    }
                })
            });
        },
        "d1-cum": function (canvas) {
            var d = data();
            return new Chart(canvas, {
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
                    plugins: {
                        tooltip: { callbacks: { label: function (c) { return "Cumulative gap: " + usdK(c.parsed.y); } } }
                    },
                    scales: {
                        x: { grid: { display: false }, border: { display: false } },
                        y: yAxis(usdK, { max: 0 })
                    }
                })
            });
        },
        "d2-dept": function (canvas) {
            var d = data();
            return new Chart(canvas, {
                type: "bar",
                data: {
                    labels: d.departments.map(function (x) { return x.name; }),
                    datasets: [{
                        label: "Total cost",
                        data: d.departments.map(function (x) { return x.value; }),
                        backgroundColor: C.blue,
                        borderRadius: 3
                    }]
                },
                options: base({
                    indexAxis: "y",
                    plugins: {
                        tooltip: { callbacks: { label: function (c) { return usdK(c.parsed.x); } } }
                    },
                    scales: {
                        x: yAxis(usdK, { beginAtZero: true }),
                        y: { grid: { display: false }, border: { display: false } }
                    }
                })
            });
        },
        "d2-cogs": function (canvas) {
            var d = data();
            return new Chart(canvas, {
                type: "doughnut",
                data: {
                    labels: d.cogs.map(function (x) { return "Cost of sales \u2013 " + x.name; }),
                    datasets: [{
                        data: d.cogs.map(function (x) { return x.value; }),
                        backgroundColor: [C.blue, C.violet],
                        borderColor: "#0E1727",
                        borderWidth: 3
                    }]
                },
                options: base({
                    cutout: "62%",
                    plugins: {
                        legend: { display: true, position: "bottom" },
                        tooltip: { callbacks: { label: function (c) { return usdK(c.parsed); } } }
                    }
                })
            });
        },
        "d2-trend": function (canvas) {
            var d = data();
            var colors = { People: C.blue, Marketing: C.violet, Technology: C.mint, General: C.amber };
            return new Chart(canvas, {
                type: "line",
                data: {
                    labels: d.months,
                    datasets: d.opexTrend.map(function (s) {
                        return {
                            label: s.name,
                            data: s.values,
                            borderColor: colors[s.name] || C.muted,
                            backgroundColor: colors[s.name] || C.muted,
                            borderWidth: 2,
                            pointRadius: 0,
                            tension: 0.35
                        };
                    })
                },
                options: base({
                    interaction: { mode: "index", intersect: false },
                    plugins: {
                        legend: { display: true, position: "top", align: "end" },
                        tooltip: { callbacks: { label: function (c) { return c.dataset.label + ": " + usdK(c.parsed.y); } } }
                    },
                    scales: {
                        x: { grid: { display: false }, border: { display: false } },
                        y: yAxis(usdK, { beginAtZero: true })
                    }
                })
            });
        },
        "d2-budget": function (canvas) {
            var d = data();
            var values = d.variance.map(function (x) { return x.value; });
            return new Chart(canvas, {
                type: "bar",
                data: {
                    labels: d.variance.map(function (x) { return x.name; }),
                    datasets: [{
                        label: "Variance vs budget",
                        data: values,
                        backgroundColor: values.map(function (v) { return v > 0 ? C.coral : (v < 0 ? C.mint : C.muted); }),
                        borderRadius: 3
                    }]
                },
                options: base({
                    indexAxis: "y",
                    plugins: {
                        tooltip: { callbacks: { label: function (c) { return (c.parsed.x > 0 ? "+" : "") + c.parsed.x.toFixed(1) + "K"; } } }
                    },
                    scales: {
                        x: yAxis(function (v) { return (v > 0 ? "+" : "") + v + "K"; }),
                        y: { grid: { display: false }, border: { display: false } }
                    }
                })
            });
        },
        "d3-bank": function (canvas) {
            var d = data();
            return new Chart(canvas, {
                type: "bar",
                data: {
                    labels: d.banks.map(function (b) { return b.name; }),
                    datasets: [{
                        label: "USD",
                        data: d.banks.map(function (b) { return b.value; }),
                        backgroundColor: [C.blue, C.violet],
                        borderRadius: 3
                    }]
                },
                options: base({
                    indexAxis: "y",
                    plugins: {
                        legend: { display: true, position: "top", align: "start" },
                        tooltip: { callbacks: { label: function (c) { return usdK(c.parsed.x); } } }
                    },
                    scales: {
                        x: yAxis(usdK, { beginAtZero: true }),
                        y: { grid: { display: false }, border: { display: false } }
                    }
                })
            });
        },
        "d3-ar": function (canvas) {
            var d = data();
            return agingChart(canvas, d.ar);
        },
        "d3-ap": function (canvas) {
            var d = data();
            return agingChart(canvas, d.ap);
        },
        "d3-wc": function (canvas) {
            return new Chart(canvas, {
                type: "bar",
                data: {
                    labels: ["Cash", "+ AR", "\u2212 AP", "Net WC"],
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
                        legend: {
                            display: true,
                            position: "bottom",
                            onClick: function () { },
                            labels: {
                                color: "#FFFFFF",
                                generateLabels: function () {
                                    return [
                                        { text: "Increase", fillStyle: C.mint, strokeStyle: C.mint, lineWidth: 0, fontColor: "#FFFFFF" },
                                        { text: "Decrease", fillStyle: C.coral, strokeStyle: C.coral, lineWidth: 0, fontColor: "#FFFFFF" },
                                        { text: "Total", fillStyle: C.blue, strokeStyle: C.blue, lineWidth: 0, fontColor: "#FFFFFF" }
                                    ];
                                }
                            }
                        },
                        tooltip: {
                            filter: function (item) { return item.datasetIndex === 1; },
                            callbacks: { label: function (c) { return usdK(c.parsed.y); } }
                        }
                    },
                    scales: {
                        x: { stacked: true, grid: { display: false }, border: { display: false } },
                        y: yAxis(usdK, { stacked: true, beginAtZero: true })
                    }
                })
            });
        },
        "d3-fx": function (canvas) {
            var d = data();
            var total = d.fx.reduce(function (sum, row) { return sum + row.value; }, 0);
            return new Chart(canvas, {
                type: "doughnut",
                data: {
                    labels: ["SEK", "USD", "EUR", "GBP"],
                    datasets: [{
                        data: d.fx.map(function (row) { return row.value; }),
                        backgroundColor: [C.blue, C.violet, C.mint, C.amber],
                        borderColor: "#0E1727",
                        borderWidth: 3
                    }]
                },
                options: base({
                    cutout: "58%",
                    plugins: {
                        legend: { display: true, position: "bottom" },
                        tooltip: {
                            callbacks: {
                                label: function (c) {
                                    var pct = total ? (c.parsed / total * 100).toFixed(1) : "0.0";
                                    return c.label + ": " + usdK(c.parsed) + " (" + pct + "%)";
                                }
                            }
                        }
                    }
                })
            });
        }
    };

    function agingChart(canvas, rows) {
        return new Chart(canvas, {
            type: "bar",
            data: {
                labels: rows.map(function (r) { return r.name; }),
                datasets: [{
                    data: rows.map(function (r) { return r.value; }),
                    backgroundColor: [C.mint, C.blue, C.amber, C.orange, C.coral],
                    borderRadius: 3
                }]
            },
            options: base({
                indexAxis: "y",
                plugins: {
                    tooltip: { callbacks: { label: function (c) { return usdK(c.parsed.x); } } }
                },
                scales: {
                    x: yAxis(usdK, { beginAtZero: true }),
                    y: { grid: { display: false }, border: { display: false } }
                }
            })
        });
    }

    function drawChart(canvas) {
        if (chartsDrawn[canvas.id] || !window.Chart) return;
        var id = canvas.id.replace("pp-chart-", "");
        var build = builders[id];
        if (!build) return;
        chartsDrawn[canvas.id] = true;
        chartDefaults();
        build(canvas);
    }

    /* ───────── Data-table page: pick a table to see its columns ───────── */

    function initTables() {
        var rows = Array.prototype.slice.call(document.querySelectorAll(".pp-trow"));
        var panels = Array.prototype.slice.call(document.querySelectorAll(".pp-tdetail"));
        rows.forEach(function (row) {
            row.addEventListener("click", function () {
                rows.forEach(function (r) { r.classList.toggle("on", r === row); });
                panels.forEach(function (p) {
                    p.classList.toggle("on", p.dataset.table === row.dataset.table);
                });
            });
        });
    }

    /* ───────── Wiring ───────── */

    buildChrome();
    initTables();

    document.getElementById("ppPrev").addEventListener("click", function () { show(current - 1); });
    document.getElementById("ppNext").addEventListener("click", function () { show(current + 1); });
    document.getElementById("ppFs").addEventListener("click", toggleFullscreen);
    document.getElementById("ppFsTop").addEventListener("click", toggleFullscreen);

    document.querySelectorAll("[data-goto]").forEach(function (button) {
        button.addEventListener("click", function () {
            show(Number(button.dataset.goto) - 1);
        });
    });

    document.addEventListener("fullscreenchange", function () {
        if (document.fullscreenElement && root.classList.contains("pp-faux-fs")) setFauxFullscreen(false);
        syncFullscreenUi(Boolean(document.fullscreenElement) || root.classList.contains("pp-faux-fs"));
        setTimeout(fit, 30);
    });

    document.addEventListener("keydown", function (event) {
        var tag = event.target && event.target.tagName;
        if (tag === "INPUT" || tag === "SELECT" || tag === "TEXTAREA") return;
        if (event.target && event.target.isContentEditable) return;

        switch (event.key) {
            case "ArrowRight":
            case "PageDown":
            case " ":
                event.preventDefault();
                show(current + 1);
                break;
            case "ArrowLeft":
            case "PageUp":
                event.preventDefault();
                show(current - 1);
                break;
            case "Home":
                event.preventDefault();
                show(0);
                break;
            case "End":
                event.preventDefault();
                show(slides.length - 1);
                break;
            case "Escape":
                if (document.fullscreenElement && document.exitFullscreen) document.exitFullscreen();
                else if (root.classList.contains("pp-faux-fs")) setFauxFullscreen(false);
                break;
            case "f":
            case "F":
                if (!event.metaKey && !event.ctrlKey) toggleFullscreen();
                break;
            default:
                break;
        }
    });

    var resizeFrame = 0;
    window.addEventListener("resize", function () {
        cancelAnimationFrame(resizeFrame);
        resizeFrame = requestAnimationFrame(fit);
    });

    fit();

    var hash = parseInt((location.hash || "").replace("#slide-", ""), 10);
    if (hash >= 1 && hash <= slides.length) {
        current = -1;
        show(hash - 1);
    } else {
        show(0);
    }
})();
