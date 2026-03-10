const form = document.getElementById("searchForm");
const ipInput = document.getElementById("searchIP");
const hostInput = document.getElementById("searchHost");
const userNameInput = document.getElementById("searchUser");
const searchOSInput = document.getElementById("searchOS");
const serialNumberInput = document.getElementById("searchSN");
const modelInput = document.getElementById("searchModel");
const emptyCheckBox = document.getElementById("skipEmptyCheckBox");

ALERT_TOGGLE = 50000

const raw = document.getElementById("page-data")?.textContent ?? "{}";
var pageData = JSON.parse(raw);

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

    
    if (count > ALERT_TOGGLE) {

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
    alertSpan.textContent = "Error! To much rows is displayed to show details (max: " + ALERT_TOGGLE + ")."
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




function ipToNumber(ip) {
    return ip
        .split('.')
        .map(part => part.padStart(3, '0'))
        .join('');
}

function sortTable(th, asc) {

    console.log("sorting header: ",th)
    
    if (th == "Ip") {
        if (asc == "true") {

            pageData.records.sort((a, b) => ipToNumber(a[th]) - ipToNumber(b[th]))
        }
        else {
            pageData.records.sort((a, b) => ipToNumber(b[th]) - ipToNumber(a[th]))
        }
    }
    else {
        if (asc == "true" ) {
            pageData.records.sort((a, b) => {
                const A = a[th]
                const B = b[th]
                if (A < B) {return -1 }
                if (A > B) { return 1; }
                return 0;
            })
        }
        else {
            pageData.records.sort((a, b) => {
                const A = a[th]==null? "" : a[th].toLowerCase()
                const B = b[th]==null? "": b[th].toLowerCase()
                if (A < B) { return 1; }
                if (A > B) { return -1; }
                return 0;
            })
        }
        
    }
    var i = 0;
    for (var r of pageData.records) {
        var ipField = document.getElementById("ipRow{" + i + "}")
        ipField.innerHTML = r.Ip

        var hostnameField = document.getElementById("hostnameRow{" + i + "}")
        hostnameField.innerHTML = r.Hostname

        var userField = document.getElementById("userRow{" + i + "}")
        userField.innerHTML = r.LastLoggedUser

        var foundField = document.getElementById("foundRow{" + i + "}")
        foundField.innerHTML = r.LastFoundDate

        var osField = document.getElementById("osRow{" + i + "}")
        osField.innerHTML = r.OperatingSystem

        var snField = document.getElementById("snRow{" + i + "}")
        snField.innerHTML = r.SerialNumber

        var modelField = document.getElementById("modelRow{" + i + "}")
        modelField.innerHTML = r.Model

        var procGenField = document.getElementById("procGenRow{" + i + "}")
        procGenField.innerHTML = r.ProcGen

        var debug = "tableRowHidden{" + i + "}"
        var hiddenRow = document.getElementById("tableRowHidden{" + i+"}")
        hiddenRow.innerHTML = ("More information about " + r.Ip +" here")
    

        
        i++;
    }
    

    return 0;
}

// attach table header click handlers 
document.querySelectorAll("table.table thead th[data-property]").forEach(th => {

    th.style.cursor = "pointer";

    th.addEventListener("click", () => {

        const property = th.dataset.property;

       document.getElementById("OrderBy").value = property;

        const ascInput = document.getElementById("Asc");
        ascInput.value = ascInput.value === "true" ? "false" : "true";

        sortTable(property, ascInput.value)
    });

});




// Customization of tabs to slider value
const slider=document.getElementById("rowsSlider")

slider.addEventListener("mouseup", () => {
    console.clear()
    console.log("slider value: ", slider.value)

    
    pageData.maxRowsPrint = slider.value

    var recordsAmount = pageData.records.length
    var auxTabNum = Math.ceil(pageData.records.length / pageData.maxRowsPrint);

    var tabsDiv = document.getElementById("tabsDiv");

    console.log("rows per tab: ", pageData.maxRowsPrint, " tabsNum: ", auxTabNum)
    console.log("tabsDiv before ",tabsDiv)

    let html=""
    for (var i = 0; i < auxTabNum; i++) {
        radioId = "tab{" + i + "}"
        if (i == 0) {
            
            html += '<input type="radio" name="radio" id="' + radioId + '"checked="checked" class="tab" aria-label="' + (i + 1) + '" />';
        }
        else {
            html += '<input type="radio" name="radio" id ="' + radioId +'"class="tab  [--tab-bg:var(--color-accent)]" aria-label=\"' + (i + 1)+'\" />'

        }


        html += "<div  class=\"sticky h-70 overflow-x-auto tab-content border-base-300 bg-base-100 p-2\">"

        html += "<table class=\"table table-pin-rows table-pin-cols \">"
        html+= "<thead class=\"sticky top-0 bg-base-100\">"
        html+= "<tr>"
        html+= "<th data-property=\"Ip\" class=\"ipHeader\">Ip Address</th>"
        html+= "<th data-property=\"Hostname\">Hostname</th>"
        html+= "<th data-property=\"LastLoggedUser\">Last Logged User</th>"
        html+= "<th data-property=\"LastFoundDate\">Last Found Date</th>"
        html+= "<th data-property=\"OperatingSystem\">OperatingSystem</th>"
        html+= "<th data-property=\"SerialNumber\">Serial</th>"
        html+= "<th data-property=\"Model\">Model</th>"
        html+= "<th data-property=\"ProcGen\">ProcGen</th>"
        html+= "</tr>"
        html+= "</thead><tbody>"
        for (var j = 0; j < slider.value; j++) {
            if ((i * slider.value) + j == pageData.records.length) {
                break;
            }
            var index = (i * slider.value) + j;
            //console.log(pageData.records[debug])
            var ip = pageData.records[index];
            html += "<tr class=\"hover:bg-base-300  tableRow{" + index + "}\" onclick=\"toggleRow(this," + pageData.records.length + ")\">"
            html += "<td id = \"ipRow{" + index + "}\">"+ip.Ip+"</td>"
            html += "<td id=\"hostnameRow{" + index + "}\">" + ip.Hostname+"</td>"
            html += "<td id=\"userRow{" + index + "}\">" + ip.LastLoggedUser +"</td>"
            html += "<td id=\"foundRow{" + index + "}\">" + ip.LastFoundDate +"</td>"
            html += "<td id=\"osRow{" + index + "}\">" + ip.OperatingSystem +"</td>"
            html += "<td id=\"snRow{" + index + "}\">" + ip.SerialNumber +"</td>"
            html += "<td id=\"modelRow{" + index + "}\">" + ip.Model +"</td>"
            html += "<td id=\"procGenRow{" + index + "}\">" + ip.ProcGen +"</td>"
            html += "</tr>"


            html+= "<tr class=\"hidden\">"
            html+= "<td colspan=\"9\">"
            html += "<div class=\"card card-border p-4 bg-base-200\" id=\"tableRowHidden{" + index + "}\">"
            html+= "More information about " +ip.Ip + " here"
            html+= "</div>"
            html+= "</td>"
            html+= "</tr>"

        }
        html += "</tbody></table></div>"
    }
    tabsDiv.innerHTML = html;

    if (pageData.sortingBy != "") {
        //sortTable(pageData.sortingBy)
    }
    console.log("tabsDiv after: ", tabsDiv)

    // reattach table header click handlers 
    document.querySelectorAll("table.table thead th[data-property]").forEach(th => {

        th.style.cursor = "pointer";

        th.addEventListener("click", () => {

            const property = th.dataset.property;

            document.getElementById("OrderBy").value = property;

            const ascInput = document.getElementById("Asc");
            ascInput.value = ascInput.value === "true" ? "false" : "true";

            sortTable(property, ascInput.value)
        });

    });
})


const sliderPrintValue=document.getElementById("sliderPrintValue")
slider.addEventListener("input",()=>{
    sliderPrintValue.innerHTML = slider.value

})


// dropdown sort 
const dropdown = document.querySelector(".dropdown-hover");

dropdown.addEventListener("click", (e) => {

    const item = e.target.closest("a[data-property]");
    if (!item) return;

    const property = item.dataset.property;

       document.getElementById("OrderBy").value = property;

        const ascInput = document.getElementById("Asc");
        ascInput.value = ascInput.value === "true" ? "false" : "true";



    if (property) {
        sortTable(property, ascInput.value);
        pageData.sortingBy=property
    }

});