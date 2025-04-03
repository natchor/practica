Index = function () {

    let botonRegresarMantenedor = $("#btnRegresarMantenedor");


    $(document).ready(async function () {
        debugger;
        await CargarTabla($('#divUsuarios'), IndexUrl.urlGetUsuarios);

        //$('#tblUsuario').DataTable();
        //$('#divIndicadoresEconomicos').css('display', 'none');
        //$('#divIndicadoresEconomicos').hide();
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });

    async function CargarTabla(elemento, urlPath, filtros = undefined) {

        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: filtros
            });
            elemento.html(response);
            //alert("aqui");

            $('#tblUsuario').DataTable({
                //responsive: true,
                //orderCellsTop: true,
                //fixedHeader: true
            });


        } catch (e) {
            console.log(e);
        }

    }

}();