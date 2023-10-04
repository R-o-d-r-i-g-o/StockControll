function submitForm(submit) {
    event.preventDefault();

    Swal.fire({
        title: 'Uma pergunta ...',
        text: 'Deseja enviar o formulário de criação?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Não',
        confirmButtonText: 'Sim'
    }).then(result => {
        var modal = submit.closest('.modal');
        var form = submit.closest('form');

        if (!result.isConfirmed) {
            $(`#${modal.id}`).modal("hide");
            return;
        }

        $(`#${modal.id}`).modal("hide");
        form.submit()
    })
}

function downloadExcelFile(reqUrl, reqBody, fileName) {

    Swal.fire({
        title: "Atenção!",
        text: "O formulário será gerado dos valores em tela",
        icon: "info",
    }).then(() => {
        $.ajax({
            url: reqUrl,
            type: 'POST',
            data: reqBody,
            xhrFields: {
                responseType: 'arraybuffer'
            },
            success: function (data) {
                var blob = new Blob([data], { type: 'application/octet-stream' });
                var url = window.URL.createObjectURL(blob);
                var linkElement = document.createElement('a');
                linkElement.href = url;
                linkElement.download = `${fileName}.xlsx`;
                linkElement.click();
                window.URL.revokeObjectURL(url);

                Swal.fire("Download concluído!", "relatório baixado com sucesso", "success");
            },
            error: (err) => {
                console.log('erro ', err);
                Swal.fire("Ops ...", "Erro ao processar o download", "error");
            },
        });
    });
};