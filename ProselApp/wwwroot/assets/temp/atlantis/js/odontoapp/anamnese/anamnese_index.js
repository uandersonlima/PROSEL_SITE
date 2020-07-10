let buscaHTML = $('input#buscarAnamnese');
//let registroPorPaginaHTML = $('select#RegistroPorPagina');
let url = '/Anamneses/ListAnamneses';
let totalPagString = 'input[id="Pagination_TotalPages"]';
let totalRegString = 'input[id="Pagination_TotalRecords"]';
ConstructorUrl(url);
ConstructorAppView(buscaHTML, null);
ConstructorPagination(totalPagString, totalRegString)