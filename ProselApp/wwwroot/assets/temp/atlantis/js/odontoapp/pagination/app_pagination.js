let conf = {
    startHTML: null,
    searchHTML: null,
    endHTML: null,
    recordPerPageHTML: null,
    totalPagesString: null,
    totalRecordsString: null,
    currentPage : 1,
    visiblePages : 2,
    urlPag: null,
    timer : null
}

const ulPage = $('ul.pagination');
const divLoading = $('div#loading');
const divElements = $('div#ElementsList');

function AddFunction() {
    if (conf.searchHTML != null) {
        conf.searchHTML.keydown(debounce(RefreshPage, 500));
    }
    if (conf.recordPerPageHTML != null) {
        conf.recordPerPageHTML.change(debounce(RefreshPage, 0));
    }
    if (conf.endHTML != null && startHTML != null) {
        conf.endHTML.change(debounce(RefreshPage, 0));
        conf.startHTML.change(debounce(RefreshPage, 0));
    }
}

function AppViewModel(appView) {
    $.ajax({
        type: "GET",
        url: conf.urlPag,
        async: true,
        data: appView,
        contentType: 'application/json',
        success: function (partialView) {
            divLoading.removeClass('d-flex');
            divLoading.addClass('d-none');
            divElements.show();
            divElements.html(partialView);
            AddLiElementsPage();
        },
        error: function (code) {
            divElements.html(`<h4 class="display-3 mt-5 text-center font-weight-bold">Algo deu errado Erro${code.status}, se o problema persistir entre em contato conosco</h4>`);
        }
    })
}

function ConstructorPagination(totalPagesArg = null, totalRecordsArg = null) {
    conf.totalPagesString = totalPagesArg;
    conf.totalRecordsString = totalRecordsArg;
}

function ConstructorAppView(searchArg = null, recordperpageArg = null, startArg = null, endArg = null) {
    conf.startHTML = startArg;
    conf.searchHTML = searchArg;
    conf.endHTML = endArg;
    conf.recordPerPageHTML = recordperpageArg;
    AddFunction();
    RefreshPage();
}

function ConstructorUrl(url) {
    conf.urlPag = url;
}

function debounce(func, wait) {
    return function () {
        conf.currentPage = 1;
        clearTimeout(conf.timer);
        conf.timer = setTimeout(func, wait);
    }
}

function ChangePage(changedPage) {

    ulPage.find('li.page-item.active').removeClass('active');
    ulPage.find(`li.page-item#${changedPage}`).addClass('active');

    conf.currentPage = Number(changedPage);
    RefreshPage();
}

function AddLiElementsPage() {
    let previousPage = conf.currentPage - conf.visiblePages;
    let nextPage = conf.currentPage + conf.visiblePages;

    CurrentPage(conf.currentPage);
    PreviousPages(previousPage);
    NextPages(nextPage);
}

function PreviousPages(previousPage) {
    if (conf.currentPage > 1)
        ulPage.find('li.page-item.active').before(`<li class="page-item" id="1"><a class="page-link text-primary" href="javascript:void(0);" onclick ="ChangePage(1)"><i class="fa fa-angle-double-left"></i></a></li>`);
    for (previousPage; previousPage <= conf.currentPage - 1; previousPage++) {
        if (previousPage >= 1) {
            ulPage.find('li.page-item.active').before(`<li class="page-item" id="${previousPage}"><a class="page-link text-primary font-weight-bold" href="javascript:void(0);" onclick ="ChangePage(${previousPage})">${previousPage}</a></li>`);
        }
    }
}

function CurrentPage(currentPage) {
    if (Number($(conf.totalRecordsString).val()) > 0 && Number($(conf.totalPagesString).val()) > 1)
        ulPage.append(`<li class="page-item active" id="${currentPage}"><span class="page-link font-weight-bold">${currentPage}</span></li>`);
}

function NextPages(nextPage) {
    let totalPages = parseInt($(conf.totalPagesString).val())
    for (nextPage; nextPage >= conf.currentPage + 1; nextPage--) {
        if (nextPage <= totalPages) {
            ulPage.find('li.page-item.active').after(`<li class="page-item" id="${nextPage}"><a class="page-link text-primary font-weight-bold" href="javascript:void(0);" onclick ="ChangePage(${nextPage})">${nextPage}</a></li>`);
        }
    }
    if (totalPages > 1 && conf.currentPage < totalPages)
        ulPage.append(`<li class="page-item" id="${totalPages}"><a class="page-link text-primary" href="javascript:void(0);" onclick ="ChangePage(${totalPages})"><i class="fa fa-angle-double-right"></i></a></li>`);
}

function RefreshPage() {

    divElements.hide();
    divLoading.removeClass('d-none');
    divLoading.addClass('d-flex');
    ulPage.find('li.page-item').remove();

    let appView = {
        Search: String(conf.searchHTML.val()) || null,
        NumberPag: conf.currentPage,
        RecordPerPage: conf.recordPerPageHTML != null ? parseInt(conf.recordPerPageHTML.val()) : null,
        Start: conf.startHTML != null ? Date.parse(conf.startHTML.val()) : null,
        End: conf.endHTML != null ? Date.parse(conf.endHTML.val()) : null
    };

    AppViewModel(appView);
}
