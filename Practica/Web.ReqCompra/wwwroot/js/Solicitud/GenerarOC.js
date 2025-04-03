GenerarOC = function () {
    let rutProv = $("#rutProv");

    let frmGenerarOC = $("#frmGenerarOC");
    let frmSolicitud = $("#frmSolicitud");

    let btnGenerar = $("#btnGenerar");
    let btnFinalizar = $("#btnFinalizar");
    let btnCancelar = $("#btnCancelar");

    let tbRequerimeintoSIGFE = $("#txtFolioRequerimientoSIGFE");
    let tbCompromisoSIGFE = $("#txtFolioCompromisoSIGFE");
    let tbNumCompra = $("#NumOrdenCompra");
    let tbRutProveedor = $("#RutProveedor");

    let tbFechaOrdenCompra = $("#FechaOrdenCompra");
    let divGuardar = $("#divGuardar");
    let divFinalizar = $("#divFinalizar");
    let btnCDP = $("#btnCDP");

    let selectTipoMoneda = $("#txtTipoMoneda");
    let tbMontoAprox = $("#txtMontoAprox");

    let btnConfirm = $("#btnGuardarArchivo");
    let hdnExtrapresupuestario = $("#hdnExtrapresupuestario");
    let hdntieneContrato = $("#hdntieneContrato");
    let fechaContratoId = $("#fechaContratoId");
    let cardOrdenCompra = $("#cardOrdenCompra");
    let cardContrato = $("#cardContrato");

    let inicioContrato = $("#fechainicioContratoID");
    let finContrato = $("#fechafinContratoID");









    $(document).ready(function () {
        debugger;
        divGuardar.hide();
        divFinalizar.hide();

        let extraPres = hdnExtrapresupuestario;
        
     

        let esVer = window.location.pathname.indexOf("/Ver/") > 0;

        if (esVer && frmSolicitud.data("rutaorigen") == "PF") {
            

            if (btnCDP.data("fasecdp") == "FASE 2" || (tbCompromisoSIGFE.length > 0 && tbCompromisoSIGFE.val().length > 0)) {
                tbCompromisoSIGFE.removeAttr("disabled");
                divFinalizar.show();
            }
            else if (extraPres.val() === "True") {
                tbCompromisoSIGFE.removeAttr("disabled");
                divFinalizar.show();
            }

            else {
                //debugger;
                let cargoId = $("#hdnCargoId").val();

                if (["2", "14"].includes(cargoId)) {
                    tbCompromisoSIGFE.removeAttr("disabled");
                }
                tbFechaOrdenCompra.attr("disabled", "disabled");
                divGuardar.show();
            }
        }

        let valorCompromiso = (tbCompromisoSIGFE.val() != null) ? tbCompromisoSIGFE.val() : "";

        if (valorCompromiso.length > 0)
            tbCompromisoSIGFE.attr("disabled", "disabled");


        if (frmSolicitud.data("estafinalizada").toUpperCase() == "TRUE") {
            tbCompromisoSIGFE.attr("disabled", "disabled");
            tbFechaOrdenCompra.attr("disabled", "disabled");
        }

    });

    tbFechaOrdenCompra.change(function () {



    });

    tbRutProveedor.rut({
        formatOn: 'keyup',
        minimumLength: 8, // validar largo mínimo; default: 2
        validateOn: 'change' // si no se quiere validar, pasar null
    });

    // muestra un mensaje de error cuando el rut es inválido
    tbRutProveedor.rut().on('rutInvalido', function (e) {

        this.setCustomValidity("invalid");

        $(this).removeClass("is-valid");
        $(this).addClass("is-invalid");

    });

    tbRutProveedor.rut().on('rutValido', function (e) {
        this.setCustomValidity("");

        $(this).removeClass("is-invalid");
        $(this).addClass("is-valid");
    });

    tbCompromisoSIGFE.change(function () {

        if ($(this).val().length == 0) {
            return;
        }

        divFase1.hide();
        divFase2.show();


        tbFechaOrdenCompra.removeAttr("disabled");



    });

    btnFinalizar.click(async function () {

        //await Valida("Finalizado", "Compra finalizada correctamente", "FINALIZADA", $(this));

        await Guardar("Finalizado", "Compra finalizada correctamente", "FINALIZADA", $(this));

    });

    btnGenerar.click(async function () {

        await Guardar("Agregado", "Orden de compra generada. Esta solicitud aún no ha finalizado debe esperar que el Departamento de Presupuestos ingrese Compromiso SIGFE y luego Finalizarla", "GENERAR OC", $(this));

    });

    rutProv.change(function () {
        if (this.checked) {
            $(".rutProveedor").val("1-9");
        } else {
            $(".rutProveedor").val("");
        }

    });


    function NuevoObjeto(response) {
        //var cantidad = obj['Cantidad']; 
        var obj = JSON.parse(response);
        var listado = obj['Listado'][0];
        var fechas = listado["Fechas"];
        var proveedor = listado["Proveedor"];
        var comprador = listado["Comprador"];
        var items = listado["Items"]["Listado"][0];


        var nuevoObj = {};
        nuevoObj['id'] = 0;
        nuevoObj['CodigoOC'] = listado["Codigo"];
        nuevoObj['Nombre'] = listado["Nombre"];
        nuevoObj['Descripcion'] = listado["Descripcion"];
        nuevoObj['CodigoTipo'] = listado["CodigoTipo"];
        nuevoObj['Tipo'] = listado["Tipo"];
        nuevoObj['TipoMoneda'] = listado["TipoMoneda"];
        nuevoObj['CodigoEstadoProveedor'] = listado["CodigoEstadoProveedor"];
        nuevoObj['EstadoProveedor'] = listado["EstadoProveedor"];
        nuevoObj['FechaCreacion'] = fechas["FechaCreacion"];
        nuevoObj['FechaEnvio'] = fechas["FechaEnvio"];
        nuevoObj['FechaAceptacion'] = fechas["FechaAceptacion"];
        nuevoObj['FechaCancelacion'] = fechas["FechaCancelacion"];
        nuevoObj['FechaUltimaModificacion'] = fechas["FechaUltimaModificacion"];
        nuevoObj['TieneItems'] = listado["TieneItems"];
        nuevoObj['PromedioCalificacion'] = listado["PromedioCalificacion"];
        nuevoObj['CantidadEvaluacion'] = listado["CantidadEvaluacion"];
        nuevoObj['Descuentos'] = listado["Descuentos"];
        nuevoObj['Cargos'] = listado["Cargos"];
        nuevoObj['TotalNeto'] = listado["TotalNeto"].toString().replace(".", ",");
        nuevoObj['PorcentajeIva'] = listado["PorcentajeIva"].toString().replace(".", ",");
        nuevoObj['Impuestos'] = listado["Impuestos"].toString().replace(".", ",");
        nuevoObj['Total'] = listado["Total"].toString().replace(".", ",");
        nuevoObj['Financiamiento'] = listado["Financiamiento"];
        nuevoObj['FormaPago'] = listado["FormaPago"];
        nuevoObj['NombreContacto'] = comprador["NombreContacto"];
        nuevoObj['CargoContacto'] = comprador["CargoContacto"];
        nuevoObj['FonoContacto'] = comprador["FonoContacto"];
        nuevoObj['MailContacto'] = comprador["MailContacto"];
        nuevoObj['ProveedorCodigo'] = proveedor["Codigo"];
        nuevoObj['NombreProveedor'] = proveedor["Nombre"];
        nuevoObj['ActividadProveedor'] = proveedor["Actividad"];
        nuevoObj['CodigoSucursal'] = proveedor["CodigoSucursal"];
        nuevoObj['NombreSucursal'] = proveedor["NombreSucursal"];
        nuevoObj['RutSucursal'] = proveedor["RutSucursal"];
        nuevoObj['DireccionProveedor'] = proveedor["Direccion"];
        nuevoObj['ComunaProveedor'] = proveedor["Comuna"];
        nuevoObj['RegionProveedor'] = proveedor["Region"];
        nuevoObj['PaisProveedor'] = proveedor["Pais"];
        nuevoObj['NombreContactoProveedor'] = proveedor["NombreContacto"];
        nuevoObj['CargoContactoProveedor'] = proveedor["CargoContacto"];
        nuevoObj['FonoContactoProveedor'] = proveedor["FonoContacto"];
        nuevoObj['MailContactoProveedor'] = proveedor["MailContacto"];
        nuevoObj['ListadoCorrelativo'] = items["Correlativo"];
        nuevoObj['CodigoCategoria'] = items["CodigoCategoria"];
        nuevoObj['Categoria'] = items["Categoria"];
        nuevoObj['Producto'] = items["Producto"];
        nuevoObj['CodigoProducto'] = items["CodigoProducto"];
        nuevoObj['EspecificacionComprador'] = items["EspecificacionComprador"];
        nuevoObj['EspecificacionProveedor'] = items["EspecificacionProveedor"];
        nuevoObj['Cantidad'] = items["Cantidad"];
        nuevoObj['Unidad'] = items["Unidad"];
        nuevoObj['Moneda'] = items["Moneda"];
        nuevoObj['PrecioNeto'] = items["PrecioNeto"].toString().replace(".", ",");
        nuevoObj['TotalCargos'] = items["TotalCargos"].toString().replace(".", ",");
        nuevoObj['TotalDescuentos'] = items["TotalDescuentos"].toString().replace(".", ",");
        nuevoObj['TotalImpuestos'] = items["TotalImpuestos"].toString().replace(".", ",");
        nuevoObj['Total2'] = items["Total"].toString().replace(".", ",");

        //var obj = nuevoObj;
        return nuevoObj;
    }



    btnConfirm.click(async function () {

        await GuardarArchivo();



    });

    DropZoneConfig.frmDropZone.on("success", async function (file, response) {






        btnConfirm.show();



    });

    async function GuardarArchivo() {

        try {

            let data = {};
            data["archivos"] = DropZoneConfig.frmDropZone.files.map(f => f.objetoArchivo);

            let response = await guardarTransaccion(Index.urlGuardarArchivo, data); //valida con datos de mencado publico

            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 5000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'success',
                title: 'Archivo guardado satisfactoriamente'
            })


            btnConfirm.hide();

        } catch (e) {
            await Swal.fire("Error Archivos Guardados", "Error al guardar los archivos", 'error');
        }

    }

    async function Guardar(titulo, texto, estado, btn) {
        debugger;
        try {
            let form = frmGenerarOC;
            //let x = .val();
            if (typeof tbCompromisoSIGFE.val() === 'undefined') {

            } else {
                if (estado == "FINALIZADA") {
                    if (tbCompromisoSIGFE.val().length == 0) {
                        tbCompromisoSIGFE.removeClass("is-valid");
                        tbCompromisoSIGFE.addClass("is-invalid");
                        tbCompromisoSIGFE.focus();

                        return;

                    } else {
                        tbCompromisoSIGFE.removeClass("is-invalid");
                        tbCompromisoSIGFE.addClass("is-valid");
                    }
                }

            }
            let estaOk = ValidarForm(form);

            if (!estaOk) {
                let tieneCont = $("#hdntieneContrato").val();
                if (tieneCont === "True" && (inicioContrato.val() == "" || finContrato.val() == "")) {
                    marcarActiva(fechaContratoId);
                    fechaContratoId.toggleClass("active");
                    cardOrdenCompra.css("display", "none");
                    cardContrato.css("display", "block");
                    //Swal.fire('Complete los campos fecha inicio y fin de contrato')
                    //return;
                }
                return;
            }

            btn.cargando();




            let data = DatosForm(form);
            debugger;
            data["archivos"] = DropZoneConfig.frmDropZone.files.map(f => f.objetoArchivo);
            data["estadoStr"] = estado;
            data["montoDivisa"] = Solicitud.convertirMonedaPesosChilenos(tbMontoAprox.val(), selectTipoMoneda.val()).montoDivisa.toString().replace(".", ",");
            let newObj;
            let valida = false;
            let fechaCreacion = "";
            let response = await guardarTransaccion(Index.urlValidarOC, data); //valida con datos de mencado publico
            //debugger;

            if (response == "") {
                cantidad = 0;
            } else {

                var obj = JSON.parse(response);
                cantidad = obj['Cantidad'];
                fechaCreacion = obj['FechaCreacion'];
            }

            let html = "";
            if (response == "" || cantidad == 0 || cantidad === undefined) {
                if (!fechaCreacion || fechaCreacion == undefined || fechaCreacion == "" || fechaCreacion.length == 0)
                    html = "<p>La API Mercado público ha respondido:</p><p><b>Los parámetros no son válidos.</b> </p><p>Valide los datos primero y asegúrese que la OC este enviada en Mercado Público .</p>"
                else
                    html = "<p>La API Mercado público ha respondido:</p><p><b>Fecha de creación: " + fechaCreacion + "</b></p><p>La OC existe pero aún no contiene datos. Valide los datos primero y asegúrese que la OC este enviada en Mercado Público.</p>"
            } else {

                newObj = NuevoObjeto(response);
                newObj["SolicitudId"] = data["SolicitudId"];
                valida = true;
                //debugger;
                html = "<table class='table table-striped' style='text-align: left;' >" +
                    "<tr><td width='40%'>Compra</td><td width='60%'>" + newObj['Nombre'] + "</td></tr>" +
                    "<tr><td>Orden de compra:</td><td>" + newObj['CodigoOC'] + "</td></tr>" +
                    "<tr><td>Proveedor:</td><td>" + newObj['NombreProveedor'] + "</td></tr>" +
                    "<tr><td>Rut:</td><td>" + newObj['RutSucursal'] + "</td></tr>" +
                    "<tr><td>Monto:</td><td>" + newObj['Total'] + "</td></tr>" +
                    "<tr><td>Moneda:</td><td>" + newObj['TipoMoneda'] + "</td></tr>" +
                    "</table>  <br />  <p>Esta seguro de ingresar esta OC a esta solicitud.</p> ";
                // if (response) {
            }


            let confirmResult = await Swal.fire({
                //position: 'left',
                title: 'Confirmar orden de compra',
                html: html,
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Si, Confirmar',
                cancelButtonText: 'No, voy a revisar',
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
            });

            if (!confirmResult.value) {
                await Swal.fire('Ok! verifique la información y vuelva a guardar', '', 'info');
                return;
            } else {
                if (valida == true)
                    var response2 = await guardarTransaccion(Index.urlGuardarOC, newObj);




                let response = await guardarTransaccion(Index.urlGenerarOC, data);

                if (response) {
                    //jsonPretty = JSON.stringify(JSON.parse(newObj), null, 2);  
                    //alert(jsonPretty);
                    await Swal.fire(titulo, texto, 'success');
                    window.location.href = "/"
                }

            };




        } catch (e) {
            await Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Algo salio mal',
                footer: e.message
            });
        } finally {

            btn.reiniciarCarga();
        }
    }

    tbNumCompra.on('keydown', function (e) {
        var k = e ? e.which : window.event.keyCode;
        if (k == 32) return false;
    });


    function marcarActiva(btn) {
        $(".bandejas").removeClass("active");
        //btn.addClass("active");
    }


}();