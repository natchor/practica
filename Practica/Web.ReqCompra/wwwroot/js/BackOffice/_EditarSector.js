EditarSector = function () {
    let selectEstado = $("#txtEstado");
    let hdnId = $("#SectorId");
    let txtNombre = $("#txtNombre");
    let txtCodigo = $("#txtCodigo");
    let selectSector = $("#txtSector");
    let botonEditar = $("#btnSectorEditar");
    let botonRegresarSector = $("#btnRegresar");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");

    

    $(document).ready(async function () {
        
        let obj = await CargarDatosMantenedores(EditaSector.urlPostCargar, "Sector", hdnId.val());
       
        debugger;
        if (obj != null) {
            hdnId.val(obj.id);
            txtNombre.val(obj.nombre);
            txtCodigo.val(obj.codigo);
            selectEstado.attr("data-valor", obj.estado);
            selectSector.val("data-valor", obj.sectorPadre);

            if (obj.estado === false) {
                $("#txtEstado option[value='False']").attr("selected", true);
            }
            else {
                $("#txtEstado option[value='True']").attr("selected", true);
            }

            estado = obj.estado;

        }
        await CargarSelect(selectSector, EditaSector.urlGetSector);
        await selectSector.editableSelect().on('select.editable-select', function (e, li) {
            sectorPadre = li.val();
        });

    });

    function obj() {
        var jObj = {};
        jObj['id'] = hdnId.val();
        jObj['estado'] = selectEstado.val();
        jObj['nombre'] = txtNombre.val();
        jObj['codigo'] = txtCodigo.val();
        jObj['sectorPadre'] = selectSector.val();//siblings('.es-list').find('li.selected').data('value');
        return jObj;
    }

    botonRegresarSector.click(function () {
        window.location.href = "../../Sector";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });

    botonEditar.click(function () {
        let jSector = obj();
        let response;

        try {
            response = $.post({
                url: EditaSector.urlPostGrabar,
                data: jSector
            }).done(function () {
                    Swal.fire({
                        //position: 'top-end',
                        icon: 'success',
                        title: 'Sector guardado',
                        text: 'Será redireccionado.',
                        //showConfirmButton: true,
                        timer: 3000
                    })
                    setTimeout(function () { window.location.href = "../../Sector"; }, 3000);
              
            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Sector no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            });

        }
        catch (e) {
            console.log(e);
        }
    });
  
}();