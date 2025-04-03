EditarSystem = function () {

    let hdnId = $("#SystemId");
    let txtNombre = $("#txtNombre");
    let txtCodigo = $("#txtCodigo");
    let txtValor = $("#txtValor");
    let botonEditar = $("#btnSystemEditar");
    let botonRegresarSystem = $("#btnRegresarSystem");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");

    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaSystem.urlPostCargar, "System", hdnId.val());
        debugger;
        if (obj != null) {
            debugger;
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            txtCodigo.val(obj.codigo);
            txtValor.val(obj.valor);
        }
    });

    function obj() {
        var jobj = {};
        jobj['id'] = hdnId.val();
        jobj['nombre'] = txtNombre.val();
        jobj['codigo'] = txtCodigo.val();
        jobj['valor'] = txtValor.val();
        return jobj;
    }

    function guarda(jObj) {
        let response;
        try {
            response = $.post({
                url: EditaSystem.urlPostGrabar,
                data: jObj,
                // type: "json"
            }).done(function () {
                Swal.fire({
                    //position: 'top-end',
                    icon: 'success',
                    title: 'Propiedad guardada',
                    text: 'Será redireccionado.',
                    //showConfirmButton: true,
                    timer: 3000
                })
                setTimeout(function () { window.location.href = "../../System"; }, 3000);

            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Propiedad no guardada</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;

        } catch (e) {
            console.log(e);
        }

    }


    txtValor.on('input', function () {
        if (txtCodigo.val()=="ANHO"){
            this.value = this.value.replace(/[^0-9]/g, '');
        }
        
    });

    botonRegresarSystem.click(function () {
        window.location.href = "../../System";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });

    botonEditar.click(function () {

        let jObj = obj();
        if (jObj['codigo'] == "ANHO") {
            Swal.fire({
                title: '¿Desea Guardar Los cambios al Año? Esta acción también reiniciará la numeración de CDP y número de solicitudes',
                showDenyButton: true,
                showCancelButton: true,
                confirmButtonText: 'Cambiar',
                denyButtonText: `No`,
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    guarda(jObj);
                } else if (result.isDenied) {
                    Swal.fire('Cambios no guardados', '', 'info')
                }
            })

        } else {
            guarda(jObj);
        }
        

    });
}();
