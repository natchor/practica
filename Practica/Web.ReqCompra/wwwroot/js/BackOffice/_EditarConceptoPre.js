EditarConcepto = function () {

    let hdnId = $("#ConceptoId");
    let txtNombre = $("#txtNombre");
    let selectEstado = $("#txtEstado");
    let botonRegresarConcepto = $("#btnRegresarConcepto");
    let botonEditar = $("#btnConceptoEditar");
    let selectSector = $("#txtPertinencia");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");

    CargarSelect(selectSector, EditaConcepto.urlGetSector);

    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaConcepto.urlPostCargar, "Concepto", hdnId.val());


        debugger;
        if (obj != null) {
            debugger;
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            selectEstado.attr("data-valor", obj.estado);
            selectSector.val(obj.esPertinencia);

            //alert(obj.estado);

            if (obj.estado === false) {
                $("#txtEstado option[value='False']").attr("selected", true);
            }
            else {
                $("#txtEstado option[value='True']").attr("selected", true);
            }

            estado = obj.estado;
        }
    });

    function armaObjeto() {
        var objeto = {};
        objeto['id'] = hdnId.val();
        objeto['nombre'] = txtNombre.val();
        objeto['estado'] = selectEstado.val();
        objeto['esPertinencia'] = selectSector.val();
        return objeto;
    }

    botonRegresarConcepto.click(function () {
        window.location.href = "../../Concepto";
    });
    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });

    botonEditar.click(function () {
        let objeto = armaObjeto();
        let response;

        try {
            response = $.post({
                url: EditaConcepto.urlPostGrabar,
                data: objeto,
                // type: "json"
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Concepto guardado',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../Concepto"; }, 3000);

            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Concepto Presupuestario no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;

        } catch (e) {
            console.log(e);
        }

    });

}();