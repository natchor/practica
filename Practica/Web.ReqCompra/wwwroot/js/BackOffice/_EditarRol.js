EditarRol = function () {

    let hdnId = $("#RolId");
    let txtNombre = $("#txtNombre");
    let selectEstado = $("#txtEstado");
    let botonEditar = $("#btnRolEditar");
    let botonRegresarRol = $("#btnRegresar");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor")

    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaRol.urlPostCargar, "Rol", hdnId.val());


        debugger;
        if (obj != null) {
            debugger;
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            selectEstado.val(obj.estado);
        }


    });


    function obj() {
        var jobj = {};
        jobj['id'] = hdnId.val();
        jobj['nombre'] = txtNombre.val();
        jobj['estado'] = selectEstado.val();


        return jobj;
    }

    botonRegresarRol.click(function () {
        window.location.href = "../../Rol";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../../BackOffice";
    });

    botonEditar.click(function () {
        let jObj = obj();

        let response;

        try {
            response = $.post({
                url: EditaRol.urlPostGrabar,
                data: jObj,
                // type: "json"
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Rol guardado',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../Rol"; }, 3000);

            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Rol no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;

        } catch (e) {
            console.log(e);
        }

    });
}();
