function apagar(cpf) {
    $.ajax({
        url: '/User/DeleteUser/' + cpf,
        type: 'GET'
    }).done(resp => {
        $("#modal").html(resp);
        $("#Tmodal").modal();
    })
}