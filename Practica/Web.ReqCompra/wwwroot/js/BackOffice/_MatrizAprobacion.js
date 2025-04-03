MatrizAprobacion = function () {

    //let hdnId = $("#CargoId");
    let tablaAprobaciones = $("#divAprobacion");
    let txtBuscar = $("#solicitudBuscarId");
    let botonBuscar = $("#btnAprobacionId");
    let txtAprobacionId = $("#txtAprobacionId");
    let txtUserAprobadorId = $("#txtUserAprobadorId");
    let txtOrden = $("#txtOrden");
    let btnCreaAprobacion = $("#btnCreaAprobacionId");
    //let swtActual = $("input[name='swtActual']");

    
    
    

    let btnGuardaApro = $("#btnGuardaApro");
    let btnEditarApro = $("#btnEditarApro");
    
    let btnLimpiar = $("#btnLimpiar");
    let aprobacion = null;
    let useraproba = null;
    let Aprobacion = null;
    let Solicitud = null;


    $(document).ready(async function () {
        txtBuscar.tooltip({ 'trigger': 'focus', 'title': 'Ingrese número y año de solicitud los 0 son opcionales' });
        btnCreaAprobacion.hide();
        $('#MdlEditarAprobacion').modal({ backdrop: 'static', keyboard: false });
        $('#MdlEditar').modal({ backdrop: 'static', keyboard: false });
        await CargarSelect(txtAprobacionId, Index.urlGetSelectConfig);
        await CargarSelect(txtUserAprobadorId, Index.urlGetUsuarioSolicitud);
        
        await $('#txtAprobacionId').editableSelect().on('select.editable-select', function (e, li) {
            aprobacion = li.val();
        });

        await $('#txtUserAprobadorId').editableSelect().on('select.editable-select', function (e, li) {
            useraproba = li.val();
            
        });

       


    });

    $("#btnCreaAprobacionId").click(function () {
        $("#MdlEditarAprobacion").modal("show");
        //$("#MdlEditarAprobacion").modal();
        
        limpiar();

    }); 




    $(document).on('click', ".BtnEliminar", async function () {
        debugger;
        
        let btn = $(this).text();
        let tdApro = $(this).closest('td').attr('IdAprobacion');
        let tdSol = $(this).closest('td').attr('IdSolicitud');
        let trApro = $(this).closest('tr').attr('Aprobid');

        let question = await Swal.fire({

            title: 'Eliminar Aprobación',
            html: '¿Esta seguro de eliminar el aprobador? <br><small>Recuerde verificar que la solicitud tenga activa un "Aprobador Actual"</small>',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Si, Eliminar',
            cancelButtonText: 'No',
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',

        });

        if (!question.isConfirmed)
            return;

        let error = await EliminarValores(tdApro);

        if (error > 0) {
            await Swal.fire('El aprobador fue eliminado satisfactoriamente.', '', 'success');
        }
        else {
            Swal.fire('Error', 'Error al eliminar el aprobador', 'error');
        }

    });

    async function EliminarValores(id) {
        debugger;
        let error = 0;
        let response;
        try {

            response = await $.get({
                url: Index.urlGetEliminarAprobacion,
                data: { id: id }, 
            });
            debugger;

            await CargarTabla(tablaAprobaciones, Index.urlGetBuscaSolicitud, txtBuscar.val());
            return response;


        } catch (e) {
            console.log(e);
        }
        return error;
    }
    

    $(document).on('click', ".BtnCancelar", async function () {
        //debugger;
        CargarTabla(tablaAprobaciones, Index.urlGetBuscaSolicitud, txtBuscar.val());


    });

    $(document).on('click', ".BtnGuardar", async function () {
        //debugger;
        Solicitud = $(this).closest('td').attr('IdSolicitud');
        Aprobacion = $(this).closest('td').attr('IdAprobacion');

        $("#MdlEditar").modal("show");
        

    });

  
    $(document).on('click', ".BtnAprobacion",  function () {
        debugger;
        $('.BtnAprobacion').hide();
        $('.BtnEliminar').hide();
       
        
        let idSol = $(this).closest('td').attr('IdSolicitud');
        let idApr = $(this).closest('td').attr('IdAprobacionConfig');
        let idAprob = $(this).closest('td').attr('IdAprobador');
        let idOrd = $(this).closest('td').attr('IdOrden');
        
        
        //let htmlMod = CargarDatos(Index.urlGetModalAprobacion,idApr)

        let fila = $(this).closest('tr');
        

       
        //fila.css('font-weight', 'bold');

        let divOrdenId = fila.find('.divOrdenId');
        let divAprobadorId = fila.find('.divAprobadorId');
        let divAprobacionId = fila.find('.divAprobacionId');
        let divAcciones = fila.find('.divAcciones');
        //txtUserAprobadorId.editableSelect("destroy");
        divOrdenId.html(txtOrden);
        divAprobadorId.html(txtUserAprobadorId);
        divAprobacionId.html(txtAprobacionId);
        //txtUserAprobadorId.val(idAprob);
        txtAprobacionId.val(idApr);
        txtOrden.val(idOrd);
        
        txtUserAprobadorId.removeAttr("selected");
        //txtUserAprobadorId.attr("data-valor", idAprob);
       // await CargarSelect(txtUserAprobadorId, Index.urlGetUsuarioSolicitud);
        txtUserAprobadorId.children("option[value='" + idAprob + "']").attr("selected", "selected");
        useraproba = idAprob;
        fila.css('background-color', '#ff963e');
        fila.css('border', '3px solid #B34F19');
        fila.css('border-left', '8px solid #B34F19');

        divAcciones.html("<div class='col-6'><button type='button' class='btn btn-success BtnGuardar' title='Guardar'> <i class='far fa-save'></i></button></div> <div class='col-6'><button type='button' class='btn btn-primary BtnCancelar' title='Cancelar'> <i class='fas fa-undo-alt'></i></button></div>");


        txtUserAprobadorId.editableSelect();
        
        var lista = $("ul");
        fila.find(lista).css("position", "inherit");

        
        $(document).ready(async function () {


            await $('#txtUserAprobadorId').editableSelect().on('select.editable-select', function (e, li) {
                useraproba = li.val();
                
            });

            


        });

    });



    btnGuardaApro.click(async function  () {
        var aprob = obj(0, txtBuscar.val(), aprobacion, useraproba, txtOrden.val());
        Guardar(aprob,"creada");
    });

    btnEditarApro.click(async function () {
        debugger;
        //.children("option[value='" + idAprob + "']").attr("selected", "selected");
        var aprob = obj(Aprobacion, Solicitud, txtAprobacionId.val(), useraproba, txtOrden.val());
        Guardar(aprob, "editada");
    });

    async function Guardar(aprob,tipo) {
        let resul = null;
        try {
            resul = await guardarTransaccion(Index.urlPostCrearAprobacion, aprob);
            if (resul > 0) {
                debugger;
                $("#MdlEditarAprobacion").modal("hide");
                $("#MdlEditar").modal("hide");
                CargarTabla(tablaAprobaciones, Index.urlGetBuscaSolicitud, txtBuscar.val());
                Swal.fire('La aprobación fue '+tipo, '', 'success');
            }
            else {
                Swal.fire('Error', 'La aprobación no fue ' + tipo, 'error');
            }
        }
        catch {
            Swal.fire('Error', 'La aprobación no fue ' + tipo, 'error');
        }
    }





    function obj(id,solic,config,user,orden) {
        var jobj = {};
        if (solic == null || config == null || user == null || orden == null) {
            Swal.fire('Favor completar todos los campos obligatorios', '', 'error');
            return;
        }

        jobj['id'] = id;
        jobj['solicitudId'] = solic;
        jobj['aprobacionConfigId'] = config;
        jobj['fechaAprobacion'] = null;
        jobj['userAprobadorId'] = user;
        jobj['estaAprobado'] = null;
        jobj['observacion'] = solic;
        jobj['orden'] = orden;

      


        return jobj;
    }


    btnLimpiar.click(function () {
        limpiar();
        txtOrden.val("");
        txtAprobacionId.editableSelect("destroy");
        txtUserAprobadorId.editableSelect("destroy");
        txtAprobacionId.editableSelect();
        txtUserAprobadorId.editableSelect();

    });

    $(document).on("click", "input[name='swtActual']", async function () {
        debugger;

        try {

            $("input[name='swtActual']").prop("checked", false);

            let idaprobador = $(this).parents("tr").attr("aprobid");

            const response = await $.get({
                url: Index.urlSetAprobadorActual,
                data: { sigAprobador: idaprobador, nroSolicitud: txtBuscar.val() }
            });

            $(this).prop("checked", true);

            Toast.fire({
                icon: 'success',
                title: 'Aprobador actual actualizado'
            });


        } catch (e) {
            Toast.fire({
                icon: 'error',
                title: 'Error al actualizar el aprobador'
            });
        }

        

    });

    botonBuscar.click(function () { 
        CargarTabla(tablaAprobaciones, Index.urlGetBuscaSolicitud, txtBuscar.val());
    });

    async function CargarTabla(elemento, urlPath,id) {
        debugger;
        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id: id }
            });

            if (response.length > 800) { 
                elemento.html(response);
                btnCreaAprobacion.show();
        }
            else {
                elemento.html("");
                Swal.fire('Error', 'Solicitud no encontrada', 'error');
                btnCreaAprobacion.hide();
            }

            ////$(document).ready(function () {
            //if (!$.fn.DataTable.isDataTable(solicitudesId)) {
            //    $(solicitudesId).DataTable();
            //}



        } catch (e) {
            elemento.html("");
            Swal.fire('Error', 'Solicitud no encontrada', 'error');
            btnCreaAprobacion.hide();
            console.log(e);
        }


    }


    async function CargarDatos(urlPath, id) {
        debugger;
        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id: id }
            });
            return response;

        } catch (e) {
            console.log(e);
        }


    }
    


}();