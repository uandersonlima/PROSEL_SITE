function edit(id) {
    $.ajax({
        url: '/token/edittoken/' + id,
        type: 'GET'
    }).done(resp => {
        console.log(resp);
        $("#modal").html(resp);
        $("#Tmodal").modal();
    })
}
function apagar(id) {
    $.ajax({
        url: '/token/deletetoken/' + id,
        type: 'GET'
    }).done(resp => {
        $("#modal").html(resp);
        $("#Tmodal").modal();
    })
}
function CriarNovoToken() {
    $.ajax({
        url: '/token/adicionartoken/',
        type: 'GET'
    }).done(resp => {
        $("#modal").html(resp);
        $("#Tmodal").modal();
    })
}
function newToken(element) {
    const inputToken = element.parentNode.parentNode.querySelector('#inputToken');
    $.ajax({
        url: '/token/newtoken',
        type: 'GET'
    }).done(resp => inputToken.value = resp.securityToken);
}