const form = document.getElementById("searchForm");
const ipInput = document.getElementById("searchIP");
const hostInput = document.getElementById("searchHost");
const userNameInput = document.getElementById("searchUser");
const searchOSInput = document.getElementById("searchOS");
const serialNumberInput = document.getElementById("searchSN");
const modelInput = document.getElementById("searchModel");
const emptyCheckBox = document.getElementById("skipEmptyCheckBox");

MAX_ROWS_TOGGLE = 500


// guard in case elements aren't on some pages
if (form) {
    let timer;

    function triggerSearch() {
        clearTimeout(timer);

        const isAllEmpty =
            !ipInput?.value &&
            !hostInput?.value &&
            !userNameInput?.value &&
            !searchOSInput?.value &&
            !serialNumberInput?.value &&
            !modelInput?.value &&
            !emptyCheckBox?.checked;

        if (isAllEmpty) return;

        timer = setTimeout(() => {
            form.submit();
        }, 1000);
    }

    ipInput?.addEventListener("input", triggerSearch);
    hostInput?.addEventListener("input", triggerSearch);
    userNameInput?.addEventListener("input", triggerSearch);
    searchOSInput?.addEventListener("input", triggerSearch);
    serialNumberInput?.addEventListener("input", triggerSearch);
    modelInput?.addEventListener("input", triggerSearch);
    emptyCheckBox?.addEventListener("change", triggerSearch);
}

// Expanding table rows
function toggleRow(row, count) {

    console.log(count)
    if (count > MAX_ROWS_TOGGLE) {

        showAlert();
        return
    }
    //document.getElementById("errorAlert").classList.add("hidden");
    const detailRow = row.nextElementSibling;
    detailRow?.classList.toggle("hidden");
}

function showAlert() {

    const alertBox = document.getElementById("errorAlert")
    const alertSpan = document.getElementById("errorSpan")
    // make visible
    alertBox.classList.remove("hidden");
    alertSpan.textContent = "Error! To much rows is displayed to show details (max: " + MAX_ROWS_TOGGLE + ")."
    requestAnimationFrame(() => {
        alertBox.classList.remove("translate-x-full", "opacity-0");
        alertBox.classList.add("animate-shake");
    });

    // auto hide after 5s
    setTimeout(() => {
        alertBox.classList.add("translate-x-full", "opacity-0");

        setTimeout(() => {
            alertBox.classList.add("hidden");
            alertBox.classList.remove("animate-shake");
        }, 5000);

    }, 5000);
}

// remember focused input
document.querySelectorAll("#searchForm input").forEach(el => {
    el.addEventListener("focus", () => sessionStorage.setItem("focusedInput", el.id));
});

// restore focus after reload
window.addEventListener("load", () => {
    const id = sessionStorage.getItem("focusedInput");
    if (!id) return;
    const el = document.getElementById(id);
    if (el) {
        el.focus();
        if (el.type !== "checkbox") {
            el.selectionStart = el.selectionEnd = el.value.length;
        }
    }
});

// IP conversion (TOP-LEVEL so sorting can use it)
function ipToNumber(ip) {
    const parts = (ip ?? "").trim().split(".");
    if (parts.length !== 4) return NaN;

    let n = 0;
    for (let i = 0; i < 4; i++) {
        const p = Number(parts[i]);
        if (!Number.isInteger(p) || p < 0 || p > 255) return NaN;
        n = n * 256 + p;
    }
    return n;
}

function getSortValue(text, type) {
    const v = (text ?? "").trim();

    if (type === "ip") return ipToNumber(v);
    if (type === "number") {
        const num = Number(v.replace(",", "."));
        return Number.isFinite(num) ? num : NaN;
    }
    return v.toLowerCase();
}

function sortTable(th, index) {

    const table = th.closest("table");
    const tbody = table.tBodies[0];
    if (!tbody) return;

    // determine sort direction
    const key = `sortDir_${index}`;
    const asc = table.dataset[key] !== "asc";
    table.dataset[key] = asc ? "asc" : "desc";

    // reset header styles
    table.querySelectorAll("thead th").forEach(h =>
        h.classList.remove("asc", "desc")
    );

    th.classList.add(asc ? "asc" : "desc");

    const type = th.dataset.type || "text";

    // collect rows as PAIRS (main + detail)
    const rows = [];

    for (let i = 0; i < tbody.rows.length; i += 2) {
        rows.push({
            main: tbody.rows[i],
            detail: tbody.rows[i + 1]
        });
    }

    // sort pairs using the main row
    rows.sort((a, b) => {

        const Araw = a.main.cells[index]?.textContent ?? "";
        const Braw = b.main.cells[index]?.textContent ?? "";

        const A = getSortValue(Araw, type);
        const B = getSortValue(Braw, type);

        const Aok = (type === "text") ? true : Number.isFinite(A);
        const Bok = (type === "text") ? true : Number.isFinite(B);

        if (type !== "text") {

            if (!Aok && !Bok) return 0;
            if (!Aok) return 1;
            if (!Bok) return -1;

            return asc ? (A - B) : (B - A);
        }

        return asc ? A.localeCompare(B) : B.localeCompare(A);
    });

    // reinsert rows keeping pairs together
    rows.forEach(pair => {
        tbody.appendChild(pair.main);
        tbody.appendChild(pair.detail);
    });
}

// attach header click handlers 
document.querySelectorAll("table.table thead th[data-property]").forEach(th => {

    th.style.cursor = "pointer";

    th.addEventListener("click", () => {

        const property = th.dataset.property;

        document.getElementById("OrderBy").value = property;

        const ascInput = document.getElementById("Asc");
        ascInput.value = ascInput.value === "true" ? "false" : "true";

        triggerSearch();
    });

});

// dropdown sort 
const dropdown = document.querySelector(".dropdown-hover");

dropdown.addEventListener("click", (e) => {

    const item = e.target.closest("a[data-column]");
    if (!item) return;

    const index = Number(item.dataset.column);

    const th = document.querySelectorAll("table.table thead th")[index];

    if (th) {
        
        sortTable(th, index);
    }

});