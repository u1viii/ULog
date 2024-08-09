let mainDiv = document.getElementById("main");
let area = document.createElement("sidebar");
const tableArea = document.createElement("section");
const attributeSvg = `
    <svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M17.5 4.99992H15.8333M17.5 9.99992H13.3333M17.5 14.9999H13.3333M5.83333 16.6666V11.3009C5.83333 11.1276 5.83333 11.0409 5.81632 10.958C5.80122 10.8845 5.77626 10.8133 5.7421 10.7464C5.70359 10.6711 5.64945 10.6034 5.54116 10.468L2.79218 7.0318C2.68388 6.89644 2.62974 6.82876 2.59123 6.75339C2.55707 6.68653 2.53211 6.61535 2.51702 6.5418C2.5 6.4589 2.5 6.37223 2.5 6.19888V4.66659C2.5 4.19988 2.5 3.96652 2.59082 3.78826C2.67072 3.63146 2.79821 3.50398 2.95501 3.42408C3.13327 3.33325 3.36662 3.33325 3.83333 3.33325H11.1667C11.6334 3.33325 11.8668 3.33325 12.045 3.42408C12.2018 3.50398 12.3292 3.63146 12.4092 3.78826C12.5 3.96652 12.5 4.19988 12.5 4.66659V6.19888C12.5 6.37223 12.5 6.4589 12.483 6.5418C12.4679 6.61535 12.4429 6.68653 12.4087 6.75339C12.3702 6.82876 12.3161 6.89644 12.2078 7.0318L9.45883 10.468C9.35058 10.6034 9.29642 10.6711 9.25792 10.7464C9.22375 10.8133 9.19875 10.8845 9.18367 10.958C9.16667 11.0409 9.16667 11.1276 9.16667 11.3009V14.1666L5.83333 16.6666Z" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
    </svg>`;
const manualSvg = `
    <svg width="19" height="19" viewBox="0 0 19 19" fill="none" xmlns="http://www.w3.org/2000/svg">
        <g clip-path="url(#clip0_2548_1287)">
        <path d="M4.75004 1.58357C4.75004 2.45803 4.04116 3.16691 3.16671 3.16691C2.29226 3.16691 1.58337 2.45803 1.58337 1.58357C1.58337 0.709127 2.29226 0.000244141 3.16671 0.000244141C4.04116 0.000244141 4.75004 0.709127 4.75004 1.58357Z" fill="#00002E"/>
        <path d="M14.6459 2.37524C15.3017 2.37524 15.8334 1.84358 15.8334 1.18774C15.8334 0.531906 15.3017 0.000244141 14.6459 0.000244141C13.9901 0.000244141 13.4584 0.531906 13.4584 1.18774C13.4584 1.84358 13.9901 2.37524 14.6459 2.37524Z" fill="#00002E"/>
        <path fill-rule="evenodd" clip-rule="evenodd" d="M10.2916 5.14591V7.16467C10.4195 7.13871 10.5519 7.12508 10.6874 7.12508C11.3909 7.12508 12.0087 7.49212 12.3597 8.04512C12.5781 7.96216 12.815 7.91672 13.0624 7.91672C13.7659 7.91672 14.3837 8.28381 14.7347 8.83679C14.9531 8.75382 15.19 8.70838 15.4374 8.70838C16.5305 8.70838 17.4166 9.5945 17.4166 10.6875V12.8162C17.4166 16.2314 14.648 19 11.2327 19C9.14694 19 7.1713 18.0638 5.85049 16.4495L2.13721 11.9111C1.06379 10.5991 1.85563 8.61877 3.53766 8.4085C4.24765 8.31975 4.95539 8.58924 5.42656 9.12765L6.33328 10.1639V5.14591C6.33328 4.05285 7.21938 3.16675 8.31244 3.16675C9.4055 3.16675 10.2916 4.05285 10.2916 5.14591ZM7.91661 5.14591C7.91661 4.9273 8.09386 4.75008 8.31244 4.75008C8.53102 4.75008 8.70828 4.9273 8.70828 5.14591V10.2917C8.70828 10.729 9.06271 11.0834 9.49994 11.0834C9.93718 11.0834 10.2916 10.729 10.2916 10.2917V9.10422C10.2916 8.88564 10.4689 8.70838 10.6874 8.70838C10.906 8.70838 11.0833 8.88564 11.0833 9.10422V10.2917C11.0833 10.729 11.4377 11.0834 11.8749 11.0834C12.3122 11.0834 12.6666 10.729 12.6666 10.2917V9.89588C12.6666 9.6773 12.8439 9.50005 13.0624 9.50005C13.281 9.50005 13.4583 9.6773 13.4583 9.89588V10.2917C13.4583 10.729 13.8127 11.0834 14.2499 11.0834C14.6872 11.0834 15.0416 10.729 15.0416 10.2917C15.0416 10.2917 15.2189 10.2917 15.4374 10.2917C15.656 10.2917 15.8333 10.469 15.8333 10.6875V12.8162C15.8333 15.357 13.7735 17.4167 11.2327 17.4167C9.62186 17.4167 8.096 16.6937 7.07592 15.4469L3.36265 10.9085C3.07798 10.5606 3.28797 10.0354 3.73405 9.97956C3.92234 9.95605 4.11002 10.0275 4.23498 10.1704L6.25166 12.4751C6.8293 13.1352 7.91661 12.7267 7.91661 11.8496V5.14591Z" fill="#00002E"/>
        <path d="M15.8334 5.146C15.8334 5.80184 15.3017 6.3335 14.6459 6.3335C13.9901 6.3335 13.4584 5.80184 13.4584 5.146C13.4584 4.49016 13.9901 3.9585 14.6459 3.9585C15.3017 3.9585 15.8334 4.49016 15.8334 5.146Z" fill="#00002E"/>
        <path d="M9.10413 2.37524C9.75994 2.37524 10.2916 1.84358 10.2916 1.18774C10.2916 0.531906 9.75994 0.000244141 9.10413 0.000244141C8.44831 0.000244141 7.91663 0.531906 7.91663 1.18774C7.91663 1.84358 8.44831 2.37524 9.10413 2.37524Z" fill="#00002E"/>
        <path d="M3.16667 6.33358C3.60389 6.33358 3.95833 5.97914 3.95833 5.54191C3.95833 5.10469 3.60389 4.75024 3.16667 4.75024C2.72944 4.75024 2.375 5.10469 2.375 5.54191C2.375 5.97914 2.72944 6.33358 3.16667 6.33358Z" fill="#00002E"/>
        </g>
        <defs>
        <clipPath id="clip0_2548_1287">
        <rect width="19" height="19" fill="white"/>
        </clipPath>
        </defs>
    </svg>`;
const arrowDown = `
    <svg width="20" height="20" fill="none" stroke="#36369E" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
        <path d="m4 8 8 8 8-8"></path>
    </svg>`;
const arrowLeft = `
    <svg width="19" height="19" fill="none" stroke="#a3a3a3" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
    <path d="m16 4-8 8 8 8"></path>
    </svg>`;
const arrowRight = `
    <svg width="19" height="19" fill="none" stroke="#a3a3a3" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
    <path d="m8 4 8 8-8 8"></path>
    </svg>`;
const searchSvg = `
    <svg width="20" height="20" fill="none" stroke="#36369E" stroke-linecap="round" stroke-width="1.5" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
        <path d="M10.364 3a7.364 7.364 0 1 0 0 14.727 7.364 7.364 0 0 0 0-14.727v0Z"></path>
        <path d="M15.857 15.86 21 21.001"></path>
    </svg>`;
let btns = [];
const takeOptions = [25, 50, 100];

function createButtons(datas) {
    let index = 0;
    for (const d in datas) {
        let btn = document.createElement("button");
        btn.setAttribute("UIndex", index);
        btn.classList.add("button");
        btn.classList.add(index == 0 ? "button_main" : "button_second");
        btn.innerText = d;
        btn.innerHTML += index == 0 ? attributeSvg.trim() : manualSvg.trim();
        area.appendChild(btn);
        btns.push(btn);
        index++;
    }
}


function createTokenForm() {
    let form = document.createElement("form");
    let inputContainer = document.createElement("div");
    let tokenLabel = document.createElement("label");
    let tokenInput = document.createElement("input");
    let submitBtn = document.createElement("button");

    form.classList.add("token_form");
    inputContainer.classList.add('tokenFormInput_container');
    submitBtn.classList.add('token_form_btn');

    tokenLabel.textContent = "Token";
    tokenLabel.setAttribute("for", "token");

    tokenInput.setAttribute("type", "text");
    tokenInput.setAttribute("placeholder", "token daxil edin...");
    tokenInput.setAttribute("name", "token");
    tokenInput.setAttribute("id", "token");
    tokenInput.setAttribute("autocomplete", "off");
    tokenInput.setAttribute("required", true);

    submitBtn.textContent = "Login ol";
    submitBtn.type = "submit";

    function handleSubmit(event) {
        event.preventDefault();
        const tokenValue = tokenInput.value;
        if (tokenValue.trim() === "") return;

        setCookie("token", tokenValue.trim());
        location.reload();
    }

    form.addEventListener("submit", handleSubmit);

    tokenInput.addEventListener("keypress", function(event) {
        if (event.key === "Enter") {
            event.preventDefault();
            handleSubmit(event);
        }
    });

    form.appendChild(tokenLabel);
    inputContainer.appendChild(tokenInput);
    inputContainer.appendChild(submitBtn);
    form.appendChild(inputContainer);

    return form;
}


function createModal(content) {
    let modal = document.createElement("div");
    let modalContainer = document.createElement("div");

    modal.classList.add("modal_custom");
    modalContainer.classList.add("modal_container");

    modalContainer.appendChild(content);
    modal.appendChild(modalContainer);

    document.body.appendChild(modal);
    mainDiv.classList.add("main_open_modal")
    return modal;
}

function getCookie(name){
    const cDecoded = decodeURIComponent(document.cookie);
    
    const cArray = cDecoded.split("; ");
    let result = null;
    
    cArray.forEach(element => {
        if(element.indexOf(name) == 0){
            result = element.substring(name.length + 1)
        }
    })

    return result;
}

function setCookie(name, value){
    document.cookie = `${decodeURIComponent(name)}=${decodeURIComponent(value)};`
}

function deleteCookie(name){
    document.cookie = name +'=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

function getDatas(datas) {
    index = 0;
    let MAIN = document.createElement("div");
    MAIN.classList.add("main_container");
    mainDiv.append(MAIN);
    area.classList.add("sidebar", "cards");
    tableArea.classList.add("main_card", "cards");
    MAIN.append(area);
    MAIN.append(tableArea);
    let area2 = document.createElement("div");
    //if auth token not correct
    if (typeof datas === 'string' || !datas) {
        deleteCookie("token", null);
        const tokenForm = createTokenForm();
        createModal(tokenForm);
        return;
    }

    createButtons(datas);
    const typeId = getQueryParamsUrl("type");
    const tableName = getQueryParamsUrl("tableName");

    if (parseInt(typeId) === 0 || parseInt(typeId) === 1) {
        const sortedDates = sortDatesLatestFirst(Object.values(datas)[typeId])
        var searchTables= searchTableNames(sortedDates)
        area.append(searchTables)
    }
    area2.classList.add("column");
    area.append(area2);
    btns.forEach(btn => {
        btn.addEventListener("click", () => {
            area2.innerHTML = "";
            tableArea.innerHTML = "";
            window.history.pushState({}, document.title, window.location.pathname);
            let uindex = btn.getAttribute("UIndex");
            setQueryparamsOnUrl("type", uindex);
            window.location.reload();
        });
    });

    if (parseInt(typeId) === 0) {
        const sortedDates = sortDatesLatestFirst(Object.values(datas)[typeId])
        generateSubButtons(sortedDates, area2);

        tableName && generateTable(tableArea);
    }
}

function generateMenuForJsonData(jsonData, container) {
    const jsonFullData = document.createElement("pre");
    jsonFullData.classList.add("json-datas");
    jsonFullData.innerText = JSON.stringify(jsonData, undefined, 4);

    container.append(jsonFullData);
    return jsonFullData;
}

// json data context menusu baglamaq ucun
document.addEventListener("click", event => {
    if (!event.target.closest(".json-data-container")) {
        const expandedJsonElements = document.querySelectorAll(".json-datas");
        expandedJsonElements.forEach(element => element.classList.remove("active"));
    }
});

function generateSubButtons(datas, area2) {
    let list = document.createElement("ul");
    list.classList.add("date_list");
    datas.forEach(data => {
        let item = document.createElement("li");
        item.classList.add("sub_button");
        item.innerText = data;
        item.setAttribute("data-tablename", data);
        list.append(item);
        item.addEventListener("click", () => {
            setQueryparamsOnUrl("page", 1);
            const encodedQuery = encodeURIComponent(item.innerText);
            setQueryparamsOnUrl("tableName", encodedQuery);
            location.reload();
        });

    })
    area2.append(list);
}

function sortDatesLatestFirst(dateArray) {
    const dateObjects = dateArray;
    return dateObjects.sort();
}

function generateTable(tableArea) {
    const tableBtns = document.querySelectorAll("ul.date_list .sub_button");
    const decodedQueryTablename = decodeURIComponent(
        getQueryParamsUrl("tableName")
    );

    tableBtns.forEach(element => {
        const btnAttrName = element.getAttribute("data-tablename");

        if (decodedQueryTablename.toLowerCase() == btnAttrName) {
            element.classList.add("active");
        }
    });

    tableArea.innerHTML = "";

    let filterContainer = document.createElement("div");
    let paginationContainer = document.createElement("div");
    filterContainer.classList.add("take_select_container");

    var takeSelect = takeSelector();
    var searchInput = searchDatas();
    const currentPage = getQueryParamsUrl('page') || 1;
    const currentTake = getQueryParamsUrl('take') || 25;

    const totalPage = Math.ceil(totalCount / parseInt(currentTake));
    const pageCount = totalPage > 0 ? totalPage : 1;
    if (currentPage > pageCount) {
        setQueryparamsOnUrl('page', 1);
        window.location.reload()
    }
    var pagination = paginationDatas(currentPage, pageCount);

    filterContainer.append(takeSelect);
    filterContainer.append(searchInput);
    paginationContainer.append(pagination);


    var newTable = populateTable(table);
    tableArea.append(filterContainer);
    tableArea.append(newTable);
    tableArea.append(paginationContainer);
}

function takeSelector() {
    let select = document.createElement("select");
    select.classList.add("take_select");

    takeOptions.forEach(opt => {
        let option = document.createElement("option");
        option.value = opt;
        option.innerText = opt;
        if (opt === takeOptions[0]) {
            select.value = opt;
        }
        select.append(option);
    });

    let takeOnUrl = getQueryParamsUrl("take");
    !takeOnUrl && setQueryparamsOnUrl("take", "25");
    if (takeOptions.some(op => parseInt(takeOnUrl) === op)) {
        select.value = takeOnUrl;
    }

    select.addEventListener("change", e => {
        setQueryparamsOnUrl("take", e.target.value);
        window.location.reload();
    });

    return select;
}

function searchDatas() {
    let inputValue = "";
    const searchForm = document.createElement("form");
    const searchInput = document.createElement("input");
    const searchLabel = document.createElement("label");
    const searchBtn = document.createElement("button");
    const inputMarker = "search";

    searchForm.classList.add("search_container");
    searchInput.classList.add("search_input");
    searchLabel.classList.add("search_icon_label");
    searchBtn.classList.add("search_btn");

    searchBtn.innerHTML += searchSvg.trim();

    searchInput.type = "search";
    searchInput.placeholder = "Axtar...";
    searchInput.id = inputMarker;
    searchLabel.htmlFor = inputMarker;
    searchBtn.type = "submit";

    searchLabel.append(searchBtn);
    searchForm.append(searchInput);
    searchForm.append(searchLabel);

    const searchOnUrl = getQueryParamsUrl("search");
    if (searchOnUrl) {
        searchInput.value = searchOnUrl;
        inputValue = searchOnUrl;
    }

    searchInput.addEventListener("input", e => {
        inputValue = e.target.value;
        if (e.target.value === "") {
            setQueryparamsOnUrl("search", "");
        }
    });

    searchInput.addEventListener("keypress", e => {
        if (e.key === "Enter") {
            e.preventDefault();

            if (!(inputValue === "" || inputValue.trim() === "")) {
                if (searchOnUrl === inputValue) return;

                setQueryparamsOnUrl("search", inputValue);
                window.location.reload();
            }
        }
    });

    searchForm.addEventListener("submit", e => {
        e.preventDefault();
        e.stopPropagation();

        if (!(inputValue.trim() === "")) {
            setQueryparamsOnUrl("search", inputValue);
            window.location.reload();
        } 
    });

    return searchForm;
}
function searchTableNames(tableButtonDates) {
    const searchForm = document.createElement("form");
    const searchInput = document.createElement("input");
    const searchLabel = document.createElement("label");
    const searchBtn = document.createElement("button");
    const inputMarker = "tableSearch";

    searchForm.classList.add("search_container", "search_table");
    searchInput.classList.add("search_input");
    searchLabel.classList.add("search_icon_label");
    searchBtn.classList.add("search_btn");

    searchBtn.innerHTML = searchSvg.trim();

    searchInput.type = "search";
    searchInput.placeholder = "Axtar...";
    searchInput.id = inputMarker;
    searchLabel.htmlFor = inputMarker;
    searchBtn.type = "submit";

    searchLabel.append(searchBtn);
    searchForm.append(searchInput);
    searchForm.append(searchLabel);
    
    function tableFilter(input) {
        const btnsContainer = document.querySelector('.sidebar .column');
    
        if (!btnsContainer) return;
    

        btnsContainer.innerHTML = '';
    

        const inputLower = input.toLowerCase();
        let filteredDates = []
        console.log(input === '' || input.trim() === '');
        if (input === '' || input.trim() === '') {
            filteredDates = tableButtonDates;
        } else {
            filteredDates = tableButtonDates.filter(date => date.includes(inputLower));
        }
        generateSubButtons(filteredDates, btnsContainer);
    }
    
    searchInput.addEventListener("input", e => {
        const inputValue = e.target.value;
        tableFilter(inputValue);
    });

    searchForm.addEventListener("submit", e => {
        e.preventDefault();
        const inputValue = searchInput.value.trim();

        if (!(inputValue === "")) {
            tableFilter(inputValue);
        } 
    });

    return searchForm;
}

function getQueryParamsUrl(queryParam) {
    let url = new URL(window.location.href);
    return url.searchParams.get(queryParam);
}

function setQueryparamsOnUrl(queryParam, value) {
    let url = new URL(window.location.href);

    url.searchParams.set(queryParam, value);

    window.history.pushState({}, "", url);
}
function paginationDatas(currentPage, pageCount) {
    const showedPages = 5;
    const paginationWrapper = document.createElement("div");
    const pagination = document.createElement("div");
    const paginationCeils = document.createElement("ul");
    const prev = document.createElement("li");
    const next = document.createElement("li");

    paginationWrapper.classList.add("pagination_wrapper");
    pagination.classList.add("pagination");
    paginationCeils.classList.add("pagination_ceils");

    prev.classList.add("pagination_prev");
    next.classList.add("pagination_next");
    prev.innerHTML = arrowLeft.trim();
    next.innerHTML = arrowRight.trim();

    const setPageOnQuery = currentPage => {
        setQueryparamsOnUrl("page", currentPage);
    };

    prev.addEventListener("click", () => {
        if (currentPage > 1) {
            currentPage--;
            setPageOnQuery(currentPage);
            renderPagination();
            window.location.reload();
        }
    });

    next.addEventListener("click", () => {
        if (currentPage < pageCount) {
            currentPage++;
            setPageOnQuery(currentPage);
            renderPagination();
            window.location.reload();
        }
    });

    pagination.append(prev);
    pagination.append(next);

    function renderPagination() {
        pagination.innerHTML = "";
        paginationCeils.innerHTML = "";
        pagination.append(prev);


        const currenPageOnQuery = parseInt(getQueryParamsUrl("page")) || 1;
        currentPage = currenPageOnQuery;

        const createPageItem = pageNum => {
            const paginationCeil = document.createElement("li");
            paginationCeil.classList.add("pagination_ceil");

            if (pageNum === currentPage) {
                paginationCeil.classList.add("active");
            }

            paginationCeil.innerText = pageNum;
            paginationCeil.addEventListener("click", () => {
                currentPage = pageNum;

                if (currenPageOnQuery !== currentPage) {
                    setPageOnQuery(currentPage);
                    window.location.reload();
                }
            });
            paginationCeils.append(paginationCeil);
        };

        let startPage, endPage;
        if (pageCount <= showedPages) {
            startPage = 1;
            endPage = pageCount;
        } else {
            const maxPagesBeforeCurrentPage = Math.ceil(showedPages / 2);
            const maxPagesAfterCurrentPage = Math.ceil(showedPages / 2) - 1;
            if (currentPage <= maxPagesBeforeCurrentPage) {
                startPage = 1;
                endPage = showedPages;
            } else if (currentPage + maxPagesAfterCurrentPage >= pageCount) {
                startPage = pageCount - showedPages + 1;
                endPage = pageCount;
            } else {
                startPage = currentPage - maxPagesBeforeCurrentPage;
                endPage = currentPage + maxPagesAfterCurrentPage;
            }
        }

        if (startPage > 1) {
            createPageItem(1);
            if (startPage > 2) {
                const ellipsis = document.createElement("li");
                ellipsis.classList.add("pagination_ceil");
                ellipsis.innerText = "...";
                paginationCeils.append(ellipsis);
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            createPageItem(i);
        }

        if (endPage < pageCount) {
            if (endPage < pageCount - 1) {
                const ellipsis = document.createElement("li");
                ellipsis.classList.add("pagination_ceil");
                ellipsis.innerText = "...";
                paginationCeils.append(ellipsis);
            }
            createPageItem(pageCount);
        }

        pagination.append(paginationCeils);
        pagination.append(next);
    }

    renderPagination();
    paginationWrapper.append(pagination);
    return paginationWrapper;
}

function populateTable(data) {
    const taleContainer = document.createElement("div");
    taleContainer.classList.add("table_container");
    const table = document.createElement("table");
    table.classList.add("table");
    table.id = "newTable";

    const headers = [
        "№",
        "Data",
        "User",
        "EndPoint",
        "RequestTime",
        "StatusCode",
        "Message",
        "ResponseTime",
        "SecondDiff",
    ];
    const headerRow = document.createElement("tr");
    const tHead = document.createElement("thead");
    tHead.appendChild(headerRow);
    headers.forEach(header => {
        const th = document.createElement("th");
        th.innerText = header;
        headerRow.appendChild(th);
    });
    table.appendChild(tHead);
    const tBody = document.createElement("tbody");
    table.appendChild(tBody);
    data.forEach((item, index) => {
        const row = document.createElement("tr");

        // Sıra numarası
        const noCell = document.createElement("td");
        noCell.innerText = index + 1;
        row.appendChild(noCell);

        // Data
        const dataCell = document.createElement("td");

        const jsonDat = JSON.stringify(item.Data, null, "\t");
        const jsonData = Object.keys(item.Data);
        if (jsonData.length > 2) {
            const dataCont = document.createElement("div");
            const dataText = document.createElement("p");

            dataCont.classList.add("json-data-container");
            dataCont.classList.add("context-menu");
            dataText.innerText += "{";
            for (let i = 0; i < 2; i++) {
                dataText.innerText +=
                    '"' + jsonData[i] + '": "' + item.Data[jsonData[i]] + '"';
            }
            dataText.innerText += " ...}";
            dataText.innerHTML += arrowDown.trim()
            dataCont.append(dataText);
            dataCell.append(dataCont);
            const jsonDataContext = document.createElement("div");
            const jsonFullData = document.createElement("pre");

            jsonDataContext.classList.add("json-datas");

            jsonFullData.innerText = JSON.stringify(item.Data, undefined, 4);

            jsonDataContext.append(jsonFullData)
            dataCont.append(jsonDataContext);

            dataText.addEventListener("click", () => {
                const expandedJsonElements = document.querySelectorAll(".json-datas");
                expandedJsonElements.forEach(element =>
                    element.classList.remove("active")
                );

                jsonDataContext.classList.add("active");
            });
            // dataCell.innerText = JSON.stringify(item.Data, null, "\t");
        } else {
            dataCell.innerText = JSON.stringify(item.Data);
        }
        row.appendChild(dataCell);

        // User
        const userCell = document.createElement("td");
        userCell.innerText = item.User;
        row.appendChild(userCell);

        // EndPoint
        const endPointCell = document.createElement("td");
        endPointCell.innerText = item.EndPoint;
        row.appendChild(endPointCell);

        // RequestTime
        const requestTimeCell = document.createElement("td");
        requestTimeCell.innerText = item.DateTime;

        row.appendChild(requestTimeCell);
        if (item.Response != null) {
            // StatusCode
            const statusCodeCell = document.createElement("td");
            statusCodeCell.innerText = item.Response.StatusCode;
            row.appendChild(statusCodeCell);

            // Message
            const messageCell = document.createElement("td");
            messageCell.innerText = item.Response.Message;
            row.appendChild(messageCell);

            // ResponseTime
            const responseTimeCell = document.createElement("td");
            responseTimeCell.innerText = item.Response.DateTime;
            row.appendChild(responseTimeCell);

            // SecondDiff
            const secondDiffCell = document.createElement("td");
            secondDiffCell.innerText = item.Response.SecondDiff;
            row.appendChild(secondDiffCell);
        } else {
            for (let index = 0; index < 4; index++) {
                row.appendChild(document.createElement("td"));
            }
        }

        tBody.appendChild(row);
    });
    taleContainer.append(table);
    return taleContainer;
}


getDatas(datas);
