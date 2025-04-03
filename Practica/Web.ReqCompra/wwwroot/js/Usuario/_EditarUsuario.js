EditarUsuario = function () {
    let selectEstado = $("#txtEstado");
    let selectCargo = $("#txtCargo");
    let txtMail = $("#txtEmail");
    let hdnId = $("#UserId");
    let txtNombre = $("#txtNombre");
    let txtApellido = $("#txtApellido");
    let txtPassword = $("#txtPassword");
    let SelectRol = $("#txtRol");
    let selectSector = $("#txtSector");
    let botonEditar = $("#btnUserEditar");
    let botonRegresarUsuario = $("#btnRegresarUsuario");
    let botonRegresarMantenedor = $("#btnRegresarMantenedor");
    let selectJefeDirecto = $("#txtJefeDirecto");
    let rol = null;
    let sector = null;
    let cargo = null;
    let jefeDirectoId = null;
  

    $(document).ready(async function () {

        let user = await CargarDatos(EditaUsuario.urlPostUsuario);
        let role = await CargarDatos(EditaUsuario.urlPostRol);    

        debugger;
        if (user != null) {
            debugger;
            if (role != null) { 
                SelectRol.attr("data-valor", role.roleId);
                rol = role.roleId;
            }

            txtMail.val(user.email); 
            hdnId.val(user.id);
            txtNombre.val(user.nombre);
            txtApellido.val(user.apellido);
            txtPassword.val(user.password);
            //txtUserName.val(user.userName);
            //selectSector.val(user.cargoId);
            //SelectRol.attr("data-valor", user.roleId);
            selectSector.attr("data-valor", user.sectorId);
            selectCargo.attr("data-valor", user.cargoId);
            selectJefeDirecto.attr("data-valor", user.jefeDirectoId);
            selectEstado.attr("data-valor", user.estado);
            //alert(user.estado);
            if (user.estado === false)
            {
                $("#txtEstado option[value='False']").attr("selected", true);
            }
            else
            {
                $("#txtEstado option[value='True']").attr("selected", true);
            }
            
            sector = user.sectorId;
            cargo = user.cargoId;
            jefeDirectoId = user.jefeDirectoId;
            estado = user.estado;
            
        }
        debugger;
        
        //await CargarSelect(selectJefeDirecto, EditaUsuario.urlGetRol);
        await CargarSelectNoEditable(SelectRol, EditaUsuario.urlGetRol);
        await CargarSelectNoEditable(selectCargo, EditaUsuario.urlGetCargo);


        await CargarSelect(selectSector, EditaUsuario.urlGetSector);
        await CargarSelect(selectJefeDirecto, EditaUsuario.urlGetFuncionarios);

        

        //await SelectRol.editableSelect().on('select.editable-select', function (e, li) {
        //    rol = li.val();
        //});
        await selectSector.editableSelect().on('select.editable-select', function (e, li) {
            sector = li.val();
        });
        //await selectCargo.editableSelect().on('select.editable-select', function (e, li) {
        //    debugger;
        //    cargo = li.val();
        //});

        await selectJefeDirecto.editableSelect().on('select.editable-select', function (e, li) {
            jefeDirectoId = li.val();
        });
    });

    txtMail.keyup(function () {
        if (hdnId.val() == 0) {
            let jUser = user();

            let response;

            try {
                response = $.post({
                    url: EditaUsuario.urlPostValidaMail,
                    data: jUser,
                })
                    .done(function (respuesta) {
                        //if (respuesta == "nuevo")
                        //    //icono verde
                        //else
                        //    //cruz roja
                    }).fail(function () {
                    })

            } catch (e) {
                console.log(e);
            }
        }

    });

    function user() {
        var jUser = {};
        jUser['id'] = hdnId.val();
        jUser['estado'] = selectEstado.val();
        jUser['cargoId'] = selectCargo.val();//selectCargo.siblings('.es-list').find('li.selected').data('value');
        jUser['nombre'] = txtNombre.val();
        jUser['email'] = txtMail.val();
        jUser['apellido'] = txtApellido.val();
        jUser['password'] = txtPassword.val();
        jUser['userName'] = txtMail.val().split("@")[0];
        jUser['sectorId'] = sector;//selectSector.siblings('.es-list').find('li.selected').data('value');
        jUser['rolId'] = SelectRol.val();//SelectRol.siblings('.es-list').find('li.selected').data('value');
        jUser['jefeDirectoId'] = selectJefeDirecto.val();

        return jUser;
    }

    botonRegresarUsuario.click(function () {
        window.location.href = "../../Usuario/Index";
    });

    botonRegresarMantenedor.click(function () {
        window.location.href = "../BackOffice";
    });


    botonEditar.click(function () {
        debugger;
        let jUser = user();
      
        //validar que esten los campos
        let response;

        try {
            response = $.post({
                url: EditaUsuario.urlPostGrabar,
                data: jUser,
                // type: "json"
            }).done(function () {
                
              

                //response = $.post({
                //    url: EditaUsuario.urlPostGrabarRol,
                //    data: jRole,
                //    // type: "json"
                //})
                //    .done(function () {
                    Swal.fire({
                        //position: 'top-end',
                        icon: 'success',
                        title: 'Usuario guardado',
                        text: 'Será redireccionado.',
                        //showConfirmButton: true,
                        timer: 3000
                    })
                    setTimeout(function () { window.location.href = "../../Usuario/Index"; }, 3000);


                //}).fail(function () {
                //    Swal.fire({
                //        icon: 'error',
                //        title: 'Oops...',
                //        html: '<h3>Usuario no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                //    })
                //})
                //    //.always(function () {
                //    //alert("finished");
                //    //})
                //    ;                
            }).fail(function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    html: '<h3>Usuario no guardado</h3>  <p>verifique la información e intente nuevamente<p>',

                })
            })
                //.always(function () {
                //alert("finished");
                //})
                ;
                     
            //return response

        } catch (e) {
            console.log(e);
        }
        
        

        //$(this).slideUp();
    });
    



    //CargarSelect(selectTipoMoneda, Index.urlGetTipoMoneda);

    async function CargarDatos(urlPath) {

        let response;

        try {

            response = await $.post({
                url: urlPath,
                data: { ids: hdnId.val()}
            });
            debugger;
            return response;


        } catch (e) {
            console.log(e);
        }


    }

}();