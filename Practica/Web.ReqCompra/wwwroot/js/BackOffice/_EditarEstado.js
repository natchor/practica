EditarEstado = function () {

    let hdnId = $("#EstadoId");
    let txtNombre = $("#txtNombre");
    let selectEstado = $("#txtPermiteGenerarOC");
    let botonEditar = $("#btnEstadoEditar");
    let botonRegresarEstado = $("#btnRegresar");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor")


    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaEstado.urlPostCargar, "Estado", hdnId.val());


        debugger;
        if (obj != null) {
            debugger;
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            selectEstado.attr("data-valor", obj.permiteGenerarOC);

            alert(obj.permiteGenerarOC);

            if (obj.permiteGenerarOC === false) {
                $("#txtPermiteGenerarOC option[value='False']").attr("selected", true);
            }
            else {
                $("#txtPermiteGenerarOC option[value='True']").attr("selected", true);
            }

            permiteGenerarOC = obj.permiteGenerarOC;
        }
    });

    function obj() {
        var jobj = {};
        jobj['id'] = hdnId.val();
        jobj['nombre'] = txtNombre.val();
        jobj['permiteGenerarOC'] = selectEstado.val();


        return jobj;
    }

    botonRegresarEstado.click(function () {
        window.location.href = "../../Estado";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });


    botonEditar.click(function () {
        let jObj = obj();

        let response;

        try {
            response = $.post({
                url: EditaEstado.urlPostGrabar,
                data: jObj,
                // type: "json"
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Estado guardado',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../Estado"; }, 3000);

            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Estado no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;

        } catch (e) {
            console.log(e);
        }

    });
}();
