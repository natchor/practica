EditarCargo = function () {
    let hdnId = $("#CargoId");
    let txtNombre = $("#txtNombre");
    let botonEditar = $("#btnCargoEditar");
    let botonRegresarCargo = $("#btnRegresarCargo");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor")

    $(document).ready(async function () {

        let obj = await CargarDatosMantenedores(EditaCargo.urlPostCargar,"Cargo", hdnId.val());
       

        debugger;
        if (obj != null) {
            debugger;
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
        }


    });
    function cargo() {
        var jCargo = {};
        jCargo['id'] = hdnId.val();
        jCargo['nombre'] = txtNombre.val();


        return jCargo;
    }
    botonRegresarCargo.click(function () {
        window.location.href = "../../Cargo";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });
    botonEditar.click(function () {
        let jCargo = cargo();

        let response;

        try {
            response = $.post({
                url: EditaCargo.urlPostGrabar,
                data: jCargo,
                // type: "json"
            }).done(function () {
                    Swal.fire({
                        //position: 'top-end',
                        icon: 'success',
                        title: 'Cargo guardado',
                        text: 'Será redireccionado.',
                        //showConfirmButton: true,
                        timer: 3000
                    })
                    setTimeout(function () { window.location.href = "../../Cargo"; }, 3000);
            
            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Cargo no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                ;

        } catch (e) {
            console.log(e);
        }
 
    });
    
  

}();