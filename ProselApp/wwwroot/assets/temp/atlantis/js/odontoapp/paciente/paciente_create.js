document.querySelector('#planochecked').addEventListener('change', function () {
    if (this.checked) {
        document.querySelector('#hasplano').hidden = false
    }
    else {
        document.querySelector('#hasplano').hidden = true
        document.querySelector('#Plano_NomePlano').value = ''
        document.querySelector('#Plano_NumeroPlano').value = ''
        document.querySelector('#Plano_CpfResponsavelPlano').value = ''
    }
})