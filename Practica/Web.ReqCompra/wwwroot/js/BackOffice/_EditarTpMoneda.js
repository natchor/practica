EditarMoneda = function () {
    let selectEstado = $("#txtEstado");
    let hdnId = $("#MonedaId");
    let txtNombre = $("#txtNombre");
    let botonEditar = $("#btnMonedaEditar");
    let txtCodigo = $("#txtCodigo");
    let txtValor = $("#txtValor");
    let txtFechaReferencia = $("#txtFechaReferencia");
    let txtFechaSolicitud = $("#txtFechaSolicitud");
    let txtUrlCMF = $("#txtUrlCMF");
    let botonRegresarMoneda = $("#btnRegresarMoneda");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");
    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaMoneda.urlPostCargar, "Moneda", hdnId.val());

        debugger;
        if (obj != null) {
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            selectEstado.attr("data-valor", obj.estado);
            txtCodigo.val(obj.codigo); 
            txtValor.val(obj.valor); 
            txtFechaReferencia.val(obj.fechaReferencia); 
            txtFechaSolicitud.val(obj.fechaSolicitud); 
            txtUrlCMF.val(obj.urlCMF);

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
        jObj['codigo'] = txtCodigo.val();
        jObj['valor'] = txtValor.val();
        jObj['fechaReferencia'] = txtFechaReferencia.val();
        jObj['fechaSolicitud'] = txtFechaSolicitud.val();
        jObj['urlCMF'] = txtUrlCMF.val();
        return jObj;
    }

    botonRegresarMoneda.click(function () {
        window.location.href = "../../Moneda";
    });
    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });
    botonEditar.click(function () {
        let jMoneda = obj();
        let response;

        try {
            response = $.post({
                url: EditaMoneda.urlPostGrabar,
                data: jMoneda,
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Tipo de Moneda guardado',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../Moneda"; }, 3000);

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