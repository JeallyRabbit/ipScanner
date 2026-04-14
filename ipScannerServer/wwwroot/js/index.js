

const form = document.getElementById("searchForm");
const ipInput = document.getElementById("searchIP");
const hostInput = document.getElementById("searchHost");
const userNameInput = document.getElementById("searchUser");
const searchOSInput = document.getElementById("searchOS");
const serialNumberInput = document.getElementById("searchSN");
const modelInput = document.getElementById("searchModel");
const emptyCheckBox = document.getElementById("skipEmptyCheckBox");

const ALERT_TOGGLE = 50000
const MAX_DISK_USAGE=0.90

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
window.toggleRow = function toggleRow(rowIndex, count) {

    
    if (count > ALERT_TOGGLE) {

        showAlert();
        return
    }
    //document.getElementById("errorAlert").classList.add("hidden");
    const row = document.getElementById("tableRowHidden{"+rowIndex+"}");
    if (row != null) {
        

        row.classList.toggle("hidden");

    }
    
   
}
//window.toggleRow = toggleRow;


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

    var ascChar = "▼"
    var ascText=th
    if (asc == "true") { ascChar ="▲"}

    

    if (th == "Ip") {
        ascText="Ip Address"
        if (asc == "true") {

            pageData.records.sort((a, b) => ipToNumber(a[th]) - ipToNumber(b[th]))
        }
        else {
            pageData.records.sort((a, b) => ipToNumber(b[th]) - ipToNumber(a[th]))
        }
    }
    else if (th == "LastFoundDate") {

        ascText="Last found date"
        if (asc == "true") {

            pageData.records.sort((a, b) => Date.parse(a[th]) - Date.parse(b[th]))
        }
        else {
            pageData.records.sort((a, b) => Date.parse(b[th]) - Date.parse(a[th]))
        }
    }
    else if (th == "ProcGen")
    {
        ascText="Processor Gen."
        if (asc == "true") {
            pageData.records.sort((a, b) => {
                const A = a[th]
                const B = b[th]
                if (A < B) { return -1 }
                if (A > B) { return 1; }
                return 0;
            })
        }
        else {
            pageData.records.sort((a, b) => {
                const A = a[th]
                const B = b[th]
                if (A < B) { return 1; }
                if (A > B) { return -1; }
                return 0;
            })
        }
    }
    else if (th == "DiskSize")
    {
        ascText="Disk size"
        if (asc == "true") {
            pageData.records.sort((a, b) => a["DiskSize"] - b["DiskSize"])
        }
        else {
            pageData.records.sort((a, b) => b["DiskSize"] - a["DiskSize"])
        }
    }
    else if (th == "DiskUsage")
    {
        ascText="Disk usage (GB)"
        if (asc == "true") {
            pageData.records.sort((a, b) => (a["DiskSize"] - a["DiskFreeSpace"]) - (b["DiskSize"] - b["DiskFreeSpace"]))
        }
        else {
            pageData.records.sort((a, b) => (b["DiskSize"] - b["DiskFreeSpace"]) - (a["DiskSize"] - a["DiskFreeSpace"]))
        }
    }
    else if (th == "DiskUsageP")
    {
        ascText="Disk usage (%)"
        if (asc == "true") {
            pageData.records.sort((a, b) => {
                var dividerA = a["DiskSize"] == 0 ? 1 : a["DiskSize"]
                var dividerB = b["DiskSize"] == 0 ? 1 : b["DiskSize"]
                return ((a["DiskSize"] - a["DiskFreeSpace"]) / dividerA) - ((b["DiskSize"] - b["DiskFreeSpace"]) / dividerB)
            })
        }
        else {

            pageData.records.sort((a, b) => {
                
                if (b["DiskSize"] == 0 || a["DiskSize"] == 0) {
                    return -1
                }
                return ((b["DiskSize"] - b["DiskFreeSpace"]) / b["DiskSize"]) - ((a["DiskSize"] - a["DiskFreeSpace"]) / a["DiskSize"])
            })
        }
        
    }

    else {
        if (asc == "true" ) {
            pageData.records.sort((a, b) => {
                const A = a[th] == null ? "" : a[th].toLowerCase()
                const B = b[th] == null ? "" : b[th].toLowerCase()
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

    if (th == "LastLoggedUser") {
        ascText = "Last logged user"
    }
    else if (th == "SerialNumber") {
        ascText = "Serial Number"
    }
        var dropdown = document.getElementById("dropDown")
        dropdown.innerHTML = "Order by:\n"+ascChar+" "+ ascText
        dropdown.style.whiteSpace = "pre"
    


    var i = 0;
    var overLimit = 0
    for (var r of pageData.records) {


        var ipField = document.getElementById("ipRow{" + i + "}")
        ipField.innerHTML = r.Ip

        var hostnameField = document.getElementById("hostnameRow{" + i + "}")
        hostnameField.innerHTML = r.Hostname

        var userField = document.getElementById("userRow{" + i + "}")
        userField.innerHTML = r.LastLoggedUser

        var foundField = document.getElementById("foundRow{" + i + "}")
        var a = new Date(r.LastFoundDate).toLocaleString("pl-PL") // Not found
        foundField.innerHTML = a == "1.01.1970, 01:00:00" ? "" : a

        var osField = document.getElementById("osRow{" + i + "}")
        osField.innerHTML = r.OperatingSystem

        var snField = document.getElementById("snRow{" + i + "}")
        snField.innerHTML = r.SerialNumber

        var modelField = document.getElementById("modelRow{" + i + "}")
        modelField.innerHTML = r.Model

        var procGenField = document.getElementById("procGenRow{" + i + "}")
        procGenField.innerHTML = r.ProcGen

        var hiddenRow = document.getElementById("tableRowHidden{" + i + "}")

        
        if (r["DiskSize"]!=0 && (r["DiskSize"] - r["DiskFreeSpace"]) / r["DiskSize"] > MAX_DISK_USAGE) {
                overLimit++;
            }
        

        

        rebuildHiddenRow(r, hiddenRow);
     
        
        i++;
    }
    if (overLimit > 0 && th.startsWith("Disk")) {
        var alertToast = document.createElement("div")
        alertToast.classList.add("toast","toast-end","toast-top")
        var subAlertToast = document.createElement("duv")
        subAlertToast.classList.add("alert", "alert-error")
        var span = document.createElement("span")
        span.textContent = overLimit + " devices exceed " + MAX_DISK_USAGE*100 + "% disk limit !"
        subAlertToast.appendChild(span)
        alertToast.appendChild(subAlertToast)

        document.body.appendChild(alertToast)

        // auto hide after 5s
        setTimeout(() => {
            alertToast.classList.add("translate-x-full", "opacity-0", "animate-shake");
            document.body.removeChild(alertToast)
            setTimeout(() => {
                alertToast.classList.add("hidden");
                alertToast.classList.remove("animate-shake");
            }, 4000);

        }, 4000);

        
    }
    console.log("Over limit: ",overLimit)

    return 0;
}

// attach table header click handlers to table headers
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
if (slider != null) {


    slider.addEventListener("mouseup", () => {
        console.clear()
        console.log("slider value: ", slider.value)


        pageData.maxRowsPrint = slider.value

        var auxTabNum = Math.ceil(pageData.records.length / pageData.maxRowsPrint);

        var tabsDiv = document.getElementById("tabsDiv");

        
        while (tabsDiv.hasChildNodes()) {
            var child = tabsDiv.firstChild;
            tabsDiv.removeChild(child);
        }
        

        console.log("rows per tab: ", pageData.maxRowsPrint, " tabsNum: ", auxTabNum)
        console.log("tabsDiv before ", tabsDiv)

        let html = ""
        

        for (var i = 0; i < auxTabNum; i++) {

           

            var radioId = "tab{" + i + "}"

            var tabRadio = document.createElement("input");
            tabRadio.classList.add("tab");
            tabRadio.setAttribute("name", "radioName")
            tabRadio.setAttribute("type", "radio")
            tabRadio.setAttribute("class", "tab")
            tabRadio.setAttribute("aria-label", (i + 1));
            tabRadio.setAttribute("id", radioId);
            tabsDiv.appendChild(tabRadio);

            if (i == 0) {

                //html += '<input type="radio" name="radio" id="' + radioId + '"  checked="checked" class="tab" aria-label="' + (i + 1) + '" />';
                tabRadio.setAttribute("checked",true)
            }
            else {
                //html += '<input type="radio" name="radio" id ="' + radioId + '"class="tab  [--tab-bg:var(--color-accent)]" aria-label=\"' + (i + 1) + '\" />'
                tabRadio.style["--tab-bg"]= "var(--color-accent)"

            }

            const tabsSubDiv = document.createElement("div")
            tabsSubDiv.classList.add("sticky", "tab-content", "h-120", "overflow-x-auto", "border-base-300","bg-base-100")

          

            const tableHead = document.createElement("thead");
            tableHead.classList.add("sticky","top-0", "bg-base-100")

                ///creating headers
                const table = document.createElement("table");
                table.classList.add("table", "table-pin-rows", "table-pin-cols");

                const headerRow = document.createElement("tr");

                var th = document.createElement("th");
                th.setAttribute("data-property", "Ip");
                th.classList.add("ipHeader");
                th.textContent = "Ip Address";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "Hostname");
                th.textContent = "Hostname";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "LastLoggedUser");
                th.textContent = "Last Logged User";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "LastFoundDate");
                th.textContent = "Last Found Date";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "OperatingSystem");
                th.textContent = "OperatingSystem";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "SerialNumber");
                th.textContent = "Serial Number";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "Model");
                th.textContent = "Model";
                headerRow.appendChild(th)

                var th = document.createElement("th");
                th.setAttribute("data-property", "ProcGen");
                th.textContent = "ProcGen";
                headerRow.appendChild(th)

                tableHead.appendChild(headerRow)
                table.appendChild(tableHead);
            
                ////////////////////////
            const tableBody = document.createElement("tbody")
            tableBody.setAttribute("id", "table-body");

            for (var j = 0; j < slider.value; j++) 
            {
                if ((i * slider.value) + j == pageData.records.length) {
                    break;
                }
                var index = (i * slider.value) + j;
                //console.log(pageData.records[debug])
                var ip = pageData.records[index];

                var hostnameToPrint = ip.Hostname == null ? "" : ip.Hostname;
                var lastLoggedUserToPrint = ip.LastLoggedUser == null ? "" : ip.LastLoggedUser;
                var lastFoundDateToPrint = ip.LastFoundDate == null ? "" : new Date(ip.LastFoundDate).toLocaleString("pl-PL"); // Not found
                var operatingSystemToPrint = ip.OperatingSystem == null ? "" : ip.OperatingSystem;
                var serialNumberToPrint = ip.SerialNumber == null ? "" : ip.SerialNumber;
                var modelToPrint = ip.Model == null ? "" : ip.Model;
                var procGenToPrint = ip.ProcGen == null ? "" : ip.ProcGen;


                var row = document.createElement("tr")
                row.classList.add("hover:bg-base-300", "bg-base-100", ("tableRow{" + index + "}"));
                row.setAttribute("onclick", "toggleRow(" + index + "," + pageData.records.length + ")");

                var td = document.createElement("td");
                td.setAttribute("id", "ipRow{" + index + "}");
                td.textContent = ip.Ip;
                row.appendChild(td);

                var td = document.createElement("td");
                td.setAttribute("id", "hostnameRow{" + index + "}");
                td.textContent = hostnameToPrint;
                row.appendChild(td);
                var td = document.createElement("td");
                td.setAttribute("id", "userRow{" + index + "}");
                td.textContent = lastLoggedUserToPrint;
                row.appendChild(td);
                var td = document.createElement("td");
                td.setAttribute("id", "foundRow{" + index + "}");
                td.textContent = lastFoundDateToPrint;
                row.appendChild(td);
                var td = document.createElement("td");
                td.setAttribute("id", "osRow{" + index + "}");
                td.textContent = operatingSystemToPrint;
                row.appendChild(td);
                var td = document.createElement("td");
                td.setAttribute("id", "snRow{" + index + "}");
                td.textContent = serialNumberToPrint;
                row.appendChild(td);
                var td = document.createElement("td");
                td.setAttribute("id", "modelRow{" + index + "}");
                td.textContent = modelToPrint;
                row.appendChild(td);
                var td = document.createElement("td");
                td.setAttribute("id", "procGenRow{" + index + "}");
                td.textContent = procGenToPrint;
                row.appendChild(td);

                tableBody.appendChild(row)

                


                var hiddenRow = document.createElement("tr")
                hiddenRow.classList.add("hidden")
                hiddenRow.setAttribute("id", "tableRowHidden{" + index + "}");

                rebuildHiddenRow(ip,hiddenRow)

               
                tableBody.appendChild(hiddenRow);
                

            }
            table.appendChild(tableBody);
            tabsSubDiv.appendChild(table)
            tabsDiv.appendChild(tabsSubDiv);
            //html += "</tbody></table></div>"
        }
        //tabsDiv.innerHTML = html;

        if (pageData.sortingBy != "") {
            //sortTable(pageData.sortingBy)
        }
        console.log("tabsDiv after: ", tabsDiv)

        // reattach table header click handlers to table headers
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


    const sliderPrintValue = document.getElementById("sliderPrintValue")
    if (sliderPrintValue != null) {
        slider.addEventListener("input", () => {
            sliderPrintValue.innerHTML = slider.value

        })
    }

}

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

function rebuildHiddenRow(r, hiddenRow) {

    var td = document.createElement("td");
    td.setAttribute("colspan", "9");

    var hiddenRowDiv = document.createElement("div");
    hiddenRowDiv.classList.add("card", "card-border", "card-body", "mx-auto", "relative", "bg-base-200", "md:flex-row");
    

    

    var diskDiv = document.createElement("div")
    diskDiv.classList.add("stat", "stat-figure", "mx-auto", "relative", "bg-base-200", "md:flex-row");
    var byteConv = 1024.0 * 1024.0 * 1024.0;
    var usedSpace = Math.round((r.DiskSize - r.DiskFreeSpace) / byteConv, 2);
    var allSpace = Math.round(r.DiskSize / byteConv, 2);
    var percentage = Math.round((usedSpace / Math.max(allSpace, 1)) * 100, 2);

    var diskTitleDiv = document.createElement("div");
    diskTitleDiv.classList.add("stat-title");
    diskTitleDiv.innerText = "HardDrive:";
    var diskCaptionDiv = document.createElement("div");
    diskCaptionDiv.classList.add("stat-value");
    diskCaptionDiv.style.fontSize="22px"
    diskCaptionDiv.innerText = r.DiskCaption;
    var diskSpaceDiv = document.createElement("div");
    diskSpaceDiv.classList.add("stat-desc");
    diskSpaceDiv.style.fontSize = "12px"
    diskSpaceDiv.innerText = usedSpace + " GB \\ " + allSpace + " GB (" + percentage+"%)";
    diskDiv.appendChild(diskTitleDiv)
    diskDiv.appendChild(diskCaptionDiv)
    diskDiv.appendChild(diskSpaceDiv)
    hiddenRowDiv.appendChild(diskDiv)


    var cpuDiv = document.createElement("div")
    cpuDiv.classList.add("stat", "stat-figure", "mx-auto", "relative", "bg-base-200", "md:flex-row");
    var cpuTitleDiv = document.createElement("div")
    cpuTitleDiv.classList.add("stat-title");
    cpuTitleDiv.innerText = "Cpu:";
    var cpuCaptionDiv = document.createElement("div");
    cpuCaptionDiv.classList.add("stat-value");
    cpuCaptionDiv.style.fontSize = "22px"
    var uptime = diffFromNow(r.LastBootUpTime);
    var uptimePrint = (uptime != null) ? (uptime) : "-";
    cpuCaptionDiv.innerText = r.ProcName;
    var cpuUptimeDiv = document.createElement("div");
    cpuUptimeDiv.classList.add("stat-desc");
    cpuUptimeDiv.style.fontSize = "12px"
    cpuUptimeDiv.innerText = "Uptime: " + uptimePrint;
    cpuDiv.appendChild(cpuTitleDiv)
    cpuDiv.appendChild(cpuCaptionDiv)
    cpuDiv.appendChild(cpuUptimeDiv)
    hiddenRowDiv.appendChild(cpuDiv)


    var ramDiv = document.createElement("div")
    ramDiv.classList.add("stat", "stat-figure", "mx-auto", "relative", "bg-base-200", "md:flex-row");
    var ramTitleDiv = document.createElement("div")
    ramTitleDiv.classList.add("stat-title");
    ramTitleDiv.innerText = "RAM:";
    var ramCaptionDiv = document.createElement("div");
    ramCaptionDiv.classList.add("stat-value");
    ramCaptionDiv.style.fontSize = "22px"
    ramCaptionDiv.innerText = r.RamSize + " GB";
    /*
    var ramDescDiv = document.createElement("div");
    ramDescDiv.classList.add("stat-desc");
    ramDescDiv.style.fontSize = "12px"
    ramDescDiv.innerText = "ramDescription";
    ramDiv.appendChild(ramDescDiv)
    */
    ramDiv.appendChild(ramTitleDiv)
    ramDiv.appendChild(ramCaptionDiv)
    
    hiddenRowDiv.appendChild(ramDiv)





    

    var divButtons = document.createElement("div");
    divButtons.classList.add("stat", "stat-figure", "relative", "bg-base-200", "md:flex-row");
    divButtons.style = "max-width: 200px;";

    var buttonJson = document.createElement("button");
    buttonJson.classList.add("btn", "btn-primary");
    buttonJson.type = "button";
    buttonJson.id = "jsonFileButton";
    buttonJson.textContent = "Export to JSON";

    var buttonCsv = document.createElement("button");
    buttonCsv.classList.add("btn", "btn-primary");
    buttonCsv.type = "button";
    buttonCsv.id = "csvFileButton";
    buttonCsv.textContent = "Export to CSV";

    divButtons.appendChild(buttonJson);
    divButtons.appendChild(buttonCsv);
    hiddenRowDiv.appendChild(divButtons);

    td.appendChild(hiddenRowDiv);


    while (hiddenRow.hasChildNodes()) {
        var child = hiddenRow.firstChild;
        hiddenRow.removeChild(child);
    }
    hiddenRow.appendChild(td);
}

function diffFromNow(dateString) {

    if (dateString == null || dateString == "0001-01-01T00:00:00") {
        return "-"
    }
    const target = new Date(dateString);
    const now = new Date();

    let diff = Math.abs(now - target); // ms

    const seconds = Math.floor(diff / 1000) % 60;
    const minutes = Math.floor(diff / (1000 * 60)) % 60;
    const hours = Math.floor(diff / (1000 * 60 * 60)) % 24;
    const days = Math.floor(diff / (1000 * 60 * 60 * 24));

    const parts = [];

    if (days) parts.push(`${days}d`);
    if (hours) parts.push(`${hours}h`);
    if (minutes) parts.push(`${minutes}m`);
    if (seconds) parts.push(`${seconds}s`);

    return parts.join(" ");
}
