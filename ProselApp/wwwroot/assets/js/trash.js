let checkboxs = [];
const botoesAll = document.querySelector("div.msgAll");
const checkboxHTML = document.querySelectorAll("input.msg");
const titleText = document.querySelector('div#modal2 h2#titleText');
const bodyText = document.querySelector('div#modal2 h2#bodyText');
const buttonMarkALL = document.querySelector('input[id="buttonMarkALL"]');

checkboxHTML.forEach((input) =>
    input.addEventListener("change", function () {
        let divcard = this.closest("div.card");
        if (this.checked) {
            checkboxHTML.forEach((input) => {
                if (input.value == this.value) {
                    let divcard2 = input.closest("div.card");
                    input.checked = true;
                    divcard2.classList.add("bg-custom");
                }
            });
            checkboxs.push(Number(this.value));
            divcard.classList.add("bg-custom");
        } else {
            checkboxHTML.forEach((input) => {
                if (input.value == this.value) {
                    let divcard2 = input.closest("div.card");
                    input.checked = false;
                    divcard2.classList.remove("bg-custom");
                }
            });
            checkboxs.splice(checkboxs.indexOf(Number(this.value)), 1);
            divcard.classList.remove("bg-custom");
        }
        verifyInputs();
    })
);
function apagar(code) {
    chamadaAjax(`/message/deleteTrash?messagecode=${code}`, "GET");
}
function marcarcomolida(code) {
    chamadaAjax(`/message/markasreadTrash?messagecode=${code}`, "GET");
}
function marcarvariascomolida() {
    titleText.innerText = "Marcar mensagens";
    bodyText.innerText = "Deseja marcar como lida todas as mensagens marcadas?";
    buttonMarkALL.value = "MARCAR TODOS";
    $("#Tmodal2").modal();
}
function apagarvarios() {
    titleText.innerText = "Apagar mensagens";
    bodyText.innerText = "Deseja apagar todas as mensagens marcadas?";
    buttonMarkALL.value = "APAGAR TODOS";
    $("#Tmodal2").modal();
}
function submitALL() {
    if (buttonMarkALL.value == "MARCAR TODOS") {
        $.ajax({
            type: 'POST',
            url: "/message/MarkMultipleTrash",
            dataType: 'json',
            data: { messagecodes: checkboxs },
        }).done(resp => window.location.replace(resp))
    }
    else
        if (buttonMarkALL.value == "APAGAR TODOS") {
            $.ajax({
                type: 'POST',
                url: "/message/DeleteMultipleTrash",
                dataType: 'json',
                data: { messagecodes: checkboxs },
            }).done(resp => window.location.replace(resp))
        }
}
function chamadaAjax(url, type, data = null) {
    $.ajax({
        type: type,
        url: url,
        async: true,
        data: data,
        contentType: "application/json",
        success: function (partial) {
            $("#modal").html(partial);
            $("#Tmodal").modal();
        },
        error: function (code) {
            $("#modal")
                .html(`<div class="text-center"><h4>Algo deu errado, o erro ${code.status} aconteceu, se o problema persistir entre em contato</h4>
            <input type="button" class="btn btn-sm btn-outline-light text-muted font-weight-bold border-0" data-dismiss="modal" value="FECHAR" /></div>`);
            $("#Tmodal").modal();
        },
    });
}
document
    .querySelector('input[id="pesquisa"]')
    .addEventListener("keyup", function (e) {
        let key = e.which || e.keyCode;
        if (key == 13) {
            if (this.value.length == 0) window.location.replace("/Trash");
            else window.location.href = "?pesquisa=" + this.value;
        }
    });
document.querySelectorAll("div.card").forEach((card) =>
    card.addEventListener("mouseenter", function () {
        this.querySelector("div.info").hidden = true;
        this.querySelector("div.botoes").hidden = false;
    })
);
document.querySelectorAll("div.card").forEach((card) =>
    card.addEventListener("mouseleave", function () {
        this.querySelector("div.botoes").hidden = true;
        this.querySelector("div.info").hidden = false;
    })
);
function marcar_todas(element) {
    if (element.innerText == "Marcar todas") {
        element.innerText = "Desmarcar todas";
        checkboxHTML.forEach((input) => {
            let divcard = input.closest("div.card");
            if (!input.checked) {
                input.checked = !input.checked;
                divcard.classList.add("bg-custom");
                checkboxs.push(Number(input.value));
            }
        });
    } else {
        element.innerText = "Marcar todas";
        checkboxHTML.forEach((input) => {
            let divcard = input.closest("div.card");
            if (input.checked) {
                input.checked = !input.checked;
                divcard.classList.remove("bg-custom");
                checkboxs.splice(checkboxs.indexOf(Number(input.value)), 1);
            }
        });
    }
    checkboxs = checkboxs.filter((checkbox, pos) => checkboxs.indexOf(checkbox) === pos);
    verifyInputs();
}
function verifyInputs() {
    if (checkboxs.length == 0) {
        botoesAll.classList.add('d-none')
        botoesAll.classList.remove('d-flex', 'd-block')
    }
    else {
        botoesAll.classList.add('d-flex', 'd-block')
        botoesAll.classList.remove('d-none')
    }
}