EditarPrograma = function () {

    let hdnId = $("#ProgramaId");
    let txtNombre = $("#txtNombre");
    let selectEstado = $("#txtEstado");
    let botonEditar = $("#btnProgramaEditar");
    let botonRegresarPrograma = $("#btnRegresarPrograma");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");

    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaPrograma.urlPostCargar, "Programa", hdnId.val());


        debugger;
        if (obj != null) {
            debugger;
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
        var jobj = {};
        jobj['id'] = hdnId.val();
        jobj['nombre'] = txtNombre.val();
        jobj['estado'] = selectEstado.val();


        return jobj;
    }

    botonRegresarPrograma.click(function () {
        window.location.href = "../../Programa";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });

    botonEditar.click(function () {
        let jPrograma = obj ();

        let response;

        try {
            response = $.post({
                url: EditaPrograma.urlPostGrabar,
                data: jPrograma,
                // type: "json"
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Programa guardado',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../Programa"; }, 3000);

            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Programa no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;

        } catch (e) {
            console.log(e);
        }

    });



}();