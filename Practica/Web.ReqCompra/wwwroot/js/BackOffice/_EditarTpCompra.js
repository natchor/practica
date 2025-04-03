EditarCompra = function () {

    let selectEstado = $("#txtEstado");
    let hdnId = $("#CompraId");
    let txtNombre = $("#txtNombre");
    let botonEditar = $("#btnCompraEditar");
    let botonRegresarCompra = $("#btnRegresarCompra");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");
 

    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaCompra.urlPostCargar, "Compra", hdnId.val());

        debugger;
        if (obj != null) {
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            selectEstado.attr("data-valor", obj.estado);

            if (obj.estado === false) {
                $("#txtEstado option[value='False']").attr("selected", true);
            }
            else {
                $("#txtEstado option[value='True']").attr("selected", true);
            }

            estado = obj.estado;

        }


    });

    function obj() {
        var jObj = {};
        jObj['id'] = hdnId.val();
        jObj['estado'] = selectEstado.val();
        jObj['nombre'] = txtNombre.val();
        return jObj;
    }


    botonRegresarCompra.click(function () {
        window.location.href = "../../Compra";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });

    botonEditar.click(function () {
        let jCompra = obj();
        let response;

        try {
            response = $.post({
                url: EditaCompra.urlPostGrabar,
                data: jCompra,
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Tipo de Compra guardado',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../Compra"; }, 3000);

            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Tipo de Compra no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;
        } catch (e) {
            console.log(e);
        }
    });

}();