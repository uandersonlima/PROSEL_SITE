let buscaHTML = $('input#buscarPaciente');
let registroPorPaginaHTML = $('select#RegistroPorPagina');
let url = '/Pacientes/ListaPacientes';
let totalPagString = 'input[id="Pagination_TotalPages"]';
let totalRegString = 'input[id="Pagination_TotalRecords"]';
ConstructorUrl(url);
ConstructorAppView(buscaHTML, registroPorPaginaHTML);
ConstructorPagination(totalPagString, totalRegString)

function LoadmodalCreate() {
    $.ajax({
        type: 'GET',
        url: 'Pacientes/Create/',
        async: true,
        contentType: 'application/json',
        success: function (partial) {
            $('#modal').html(partial);
            $('#Tmodal').modal();
        },
        error: function (code) {
            $('#modal').html(`<div class="text-center"><h4>Algo deu errado, o erro ${code.status} aconteceu, se o problema persistir entre em contato</h4>
            <input type="button" class="btn btn-sm btn-outline-light text-muted font-weight-bold border-0" data-dismiss="modal" value="FECHAR" /></div>`);
            $('#Tmodal').modal();
        }
    })
}
$(".create").click(LoadmodalCreate)
