AjustarCDP = function () {

    //let hdnId = $("#CargoId");
    let divisaSeleccionda = "";
    let tablaSolicitud = $("#divSolicitud");
    let txtBuscar = $("#solicitudBuscarId");
    let botonBuscar = $("#btnAprobacionId");
    
    let btnAprobacionId = $("#btnAprobacionId");
    let IDNombreSolicitante = $("#IDNombreSolicitante");
    let IDDepartamentoSolicitante = $("#IDDepartamentoSolicitante");
    let IDFechaSolicitud = $("#IDFechaSolicitud");
    let txtNombreCompra = $("#txtNombreCompra");
    let txtObjetivoJustificacion = $("#txtObjetivoJustificacion");
    let txtObservacionGeneral = $("#txtObservacionGeneral");
    let txtContraparteTecnica = $("#txtContraparteTecnica");
    let txtUnidadDemandante = $("#txtUnidadDemandante");
    let txtProgramaPresupuestario = $("#txtProgramaPresupuestario");
    let IDIniciativaVigente = $("#IDIniciativaVigente");
    let IDNombreIniciativaVigente = $("#IDNombreIniciativaVigente");
    let txtConceptoPresupuestario = $("#txtConceptoPresupuestario");
    let txtFolioRequerimientoSIGFE = $("#txtFolioRequerimientoSIGFE");
    let txtFolioCompromisoSIGFE = $("#txtFolioCompromisoSIGFE");
    let btnCDP = $("#btnCDP");
    let btnConfirm = $("#btnGuardarArchivo");
    //let frmDropZone = $("#frmDropZone");
    let txtModalidadCompra = $("#txtModalidadCompra");
    let txtTipoCompra = $("#txtTipoCompra");
    let txtTipoMoneda = $("#txtTipoMoneda");
    let txtMontoAprox = $("#txtMontoAprox");
    let txtMontoUTM = $("#txtMontoUTM");
    let txtMontoOC = $("#txtMontoOC");
    let txtArrastre = $("#txtArrastre");
    

    
    let txtMontoAnhoActual = $("#txtMontoAnhoActual")
   // let txtMontoAprox = $("#txtMontoAprox")
    let btnAjustarCDP = $("#btnAjustarCDP")
    //let txtMontoAnhoActual = $("#txtMontoAnhoActual")
    let btnMasAnios = $("#btnMasAnios")

    let tblBitacora = $("#BitacoraCuerpo");
    let tblAprobaciones = $("#AprobacionCuerpo");

    
    let contentAjusteCDP = $("#contentAjusteCDP");

    let IdSolicitud = $("#IdSolicitud");
    let NroSolicitud = $("#NroSolicitud");

    let jsolicitud={ };

    $(document).ready(async function () {
        txtBuscar.tooltip({ 'trigger': 'focus', 'title': 'Ingrese año y número de solicitud los 0 son opcionales' });

    });

    $(document).on('click', ".RechazaCDP", async function () {
        //debugger;
        //let seleccionadosTodos = false;
        //let seleccionados = false;
        //let usuario = $(this).text();
        let idsolici = $(this).closest('td').attr('solicitudId');
        let fila = $(this).closest('tr');


        const { value: obs } = await Swal.fire({

            input: 'textarea',
            //inputLabel: '¿Esta seguro de rechazar ajuste en el CDP?',
            inputPlaceholder: 'Si tienes una observación, escribir aqui...',
            inputAttributes: {
                'aria-label': 'Si tienes una observación, escribir aqui...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para rechazar los cambios al CDP presione el boton "Guardar" `,
            title: 'Rechazar ajuste CDP',

        });

        if (obs || obs == "") {
            let id = {};
            id["solicitudId"] = idsolici;
            id["observacion"] = obs;
            let response = await CargarDatos(Index.urlRechazarAjusteCDP, id)

            if (response == "OK")
                fila.remove();
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
                title: 'Ajuste CDP rechazado'
            })

        }
    });



    $(document).on('click', ".AutorizaCDP", async function () {
        //debugger;
        //let seleccionadosTodos = false;
        //let seleccionados = false;
        //let usuario = $(this).text();
        let idsolici = $(this).closest('td').attr('solicitudId');
        let fila = $(this).closest('tr');

       
        const { value: obs } = await Swal.fire({

            input: 'textarea',
            //inputLabel: '¿Esta seguro de hacer ajuste en el CDP?',
            inputPlaceholder: 'Si tienes una observación, escribir aqui...',
            inputAttributes: {
                'aria-label': 'Si tienes una observación, escribir aqui...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para autorizar los cambios al CDP presione el boton "Guardar" `,
            title: 'Autorizar ajuste CDP',

        });

        if (obs || obs == "") {
            let id = {};
            id["solicitudId"] = idsolici;
            id["observacion"] = obs;
            let response = await CargarDatos(Index.urlAprobacionAjusteCDP, id)

            if (response == "OK")
                fila.remove();
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
                title: 'Ajuste CDP autorizado'
            })

        }
    });

    

    $(document).on('blur', "#txtMontoOC", async function () {
   
        let item = $(this);

        let valor = FormateaMonto(item.val(), 2);
        if (valor == 0) {
            valor = DesFormatearMonto(item.val());
            valor = FormateaMonto(valor, 2);
        }
        item.val(valor);
        
    });



    $(document).on('blur', ".Ajuste", async function () {
        debugger;
        //let seleccionadosTodos = false;
        //let seleccionados = false;
        //let usuario = $(this).text();
        //let idsolici = $(this).closest('td').attr('solicitudId');
        let item = $(this);

        let valor = FormateaMonto(item.val(), 2);
        if (valor == 0) {
            valor = DesFormatearMonto(item.val());
            valor = FormateaMonto(valor, 2);
        }
        item.val(valor);




    
    });

    btnAjustarCDP.click( async function  () {
        

        let validaMon = await validaMontos();

        if (validaMon)
            return;

        const { value: obs } = await Swal.fire({

            input: 'textarea',
            //inputLabel: '¿Esta seguro de hacer ajuste en el CDP?',
            inputPlaceholder: 'Si tienes una observación, escribir aqui...',
            inputAttributes: {
                'aria-label': 'Si tienes una observación, escribir aqui...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para guardar los cambios al CDP con o sin observaciones presione el boton "Guardar", esta acción es irreversible. ¿Esta seguro del cambio?  `,
            title: 'Ajuste CDP',

        });

        if (obs || obs == "") {
            await guardaDatosPresupuesto(obs);

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
                title: 'Montos CDP ajustados'
            })

        }



        

    });

    function convertirMoneda(montoAprox, tpMoneda) {
        //debugger;
        let valor = 0;

        let simboloDivisa = "";


        switch (tpMoneda) {
            case '1':
            case 'UTM':
                //valor = montoAprox;
                simboloDivisa = "UTM";
                break;
            case '4':
            case 'Euro':
                //valor = ((montoAprox * euro) / utm);
                simboloDivisa = "€";
                break;
            case '2':
            case 'UF':
                //valor = ((montoAprox * uf) / utm);
                simboloDivisa = "UF";
                break;
            case '3':
            case 'Dolar':
                //valor = ((montoAprox * dolar) / utm);
                simboloDivisa = "USD";
                break;
            case '5':
            case 'Peso Chileno':
                //valor = (montoAprox / utm);
                simboloDivisa = "$";
                break;

            default:
                valor = 0;
        }



        let ret = { valorMonto: valor, simboloDivisa: simboloDivisa };

        return ret;
        //return valor;

    }

    function armaJsonAjusteCDP() {
        let o = 0;
        var jobj = [];
        
             			       

        $('.Ajuste').each(function () {
            debugger;
            //let monto = convertirMonedaPesosChilenos($(this).val(), divisa).valorMonto;
            let monto = DesFormatearMonto($(this).val());
            let detalle = {
                //montoPresupuestado: $(this).val(),
                //montoPresupuestado: monto.toString().replace(".", ","),
              
                //id:0,
               // solicitudId: IdSolicitud.val(),
               // montoPresupuestado:0,
            anio : $(this).data("anio"),
                //montoFinal:0,
                //montoMonedaSel:0,
                //esAjuste:0,
                montoMonedaSelFinal: (monto).toString().replace(".", ",")

            };

            jobj.push(detalle);
            //}
            o += 1;
        });
        return jobj;
    }

    async function CargarMontoMultiAnual(id) {

        let response = await $.get({
            url: Index.urlDetalleSolicitud + "/" +id
        });

        console.log(response);

        //$("input[name='SolicitudDetalle']").parent().remove();
        $("#divMultianual").empty();
        $("#divMultianual2").empty();

        let soloVisualizacion = true;
        //debugger;
        $.each(response, function (index, item) {

            campoMontosMultiAnual(item, soloVisualizacion, index);

            campoMontosMultiAnualAjuste(item, soloVisualizacion, index);

        });

       


        let divisaSeleccionda = $("#txtTipoMoneda").val();
        let valSimbolo = convertirMoneda(0, divisaSeleccionda)
        $("span[name='simboloDivisa']").html(valSimbolo.simboloDivisa);
    }

//    $('#columnas_excel').find('input').each(function () {
    
//});
    $(document).on('click', "#btnMasAnios", function () {
        //btnMasAnios.click(function () {
        //if (selectModalidadCompra.val() == 1) {
        //    $("#btnMasAnios").attr("disabled", "disabled");
        //    return;
        //}
        campoMontosMultiAnualAjuste();

        let divisaSeleccionda = $("#txtTipoMoneda").val();
        let valSimbolo = convertirMoneda(0, divisaSeleccionda)
        $("span[name='simboloDivisa']").html(valSimbolo.simboloDivisa);

    });


    txtArrastre.change(function () {
   //let checkIn2 = txtArrastre.val();

        let on = txtArrastre.is(':checked');
        if (on) {
            $("#btnMasAnios").prop("disabled", false);
        } else {
            CargarTabla(tablaSolicitud, Index.urlGetBuscaSolicitud, txtBuscar.val());
        }
        
        

    });



    function campoMontosMultiAnualAjuste(item, esVisualizacion, index) {
        //debugger;
        let anio = $("input[name='AjusteDetalle']").last().data("anio") + 1;
        let valor = "";



        if (item != undefined) {
            anio = item.anio;
            valor = FormateaMonto(item.montoMonedaSel, 2);
            ajuste = FormateaMonto(item.montoMonedaSelFinal, 2);
        }

        let divAnio = $("<div>").attr({ id: "divAnio" + anio });
        //let divAnio2 = $("<div>").attr({ id: "divAnio2" + anio });

        let lbl = $("<label>").html($("<b>").html("Monto año " + anio));

        let divTb = $("<div>").attr({
            class: "input-group mb-3"
        });

        let span = $("<span>").attr({
            class: "input-group-text",
            name: 'simboloDivisa'
        });

        let tb = $('<input/>').attr({
            type: 'text',
            "data-type": "monto",
            "data-anio": anio,
            class: "form-control fa-sort-numeric-asc Ajuste",
            name: "AjusteDetalle",
            value: ajuste,

            required: "required",
            placeholder: "Monto año " + anio
        });

        let btn = "";
        if (index == 0) {
            btn = $("<button>").attr({
                type: "button",
                id: "btnMasAnios",
                name: "masAnio",
                class: "btn btn-primary",
                "data-anio": anio
            }).html($("<i>").attr({ class: "fas fa-plus" }));

        }

        


        divTb.append(span).append(tb);


        divAnio.append(lbl).append(divTb)
        //divAnio2.append(lbl).append(divTb)
        divTb.append(btn);


        $("#divMultianual2").append(divAnio);
       
        //if (esVisualizacion) {
        $("#btnMasAnios").prop("disabled", true);
        
        //    $("input[name='SolicitudDetalle']").attr("disabled", "disabled");
        //    //$(".Anuales").attr('disabled', "disabled");

        //}


    }


    function campoMontosMultiAnual(item, esVisualizacion, index) {
        debugger;
        let anio = $("input[name='SolicitudDetalle']").last().data("anio") + 1;
        let valor = "";



        if (item != undefined) {
            anio = item.anio;
            valor = FormateaMonto(item.montoMonedaSel, 2);
            ajuste = FormateaMonto(item.montoMonedaSelFinal, 2);
        }

        let divAnio = $("<div>").attr({ id: "divAnio" + anio });
        //let divAnio2 = $("<div>").attr({ id: "divAnio2" + anio });

        let lbl = $("<label>").html($("<b>").html("Monto año " + anio));

        let divTb = $("<div>").attr({
            class: "input-group mb-3"
        });

        let span = $("<span>").attr({
            class: "input-group-text",
            name: 'simboloDivisa'
        });

        let tb = $('<input/>').attr({
            type: 'text',
            "data-type": "monto",
            "data-anio": anio,
            class: "form-control fa-sort-numeric-asc Anuales",
            name: "SolicitudDetalle",
            value: valor,
            "data-ajuste": ajuste ,
            required: "required",
            placeholder: "Monto año " + anio
        });

        divTb.append(span).append(tb);

 
        divAnio.append(lbl).append(divTb)
        //divAnio2.append(lbl).append(divTb)



        $("#divMultianual").append(divAnio);
        //$("#divMultianual2").html($("#divMultianual").html().replace('SolicitudDetalle', 'AjusteDetalle').replaceAll('Anuales', 'Ajustes')).replaceAll('SolicitudDetalle','AjusteDetalle');

        //if (esVisualizacion) {
            //$("#btnMasAnios").remove();
            $("input[name='SolicitudDetalle']").attr("disabled", "disabled");
        //    //$(".Anuales").attr('disabled', "disabled");

        //}


    }

    botonBuscar.click(function () {
        //debugger;
        CargarTabla(tablaSolicitud, Index.urlGetBuscaSolicitud, txtBuscar.val());

        
    });

    async function CargarTabla(elemento, urlPath, id) {
        debugger;
        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id: id }
            });
            debugger;
            
            if (response != null)
                Solicitud(response);
            else {
                               
                Swal.fire('Error', "Solicitud no encontrada", 'error');
            }

            //elemento.html(response);
            //

            ////$(document).ready(function () {
            //if (!$.fn.DataTable.isDataTable(solicitudesId)) {
            //    $(solicitudesId).DataTable();
            //}



        } catch (e) {
            contentAjusteCDP.css("display", "none");

            Swal.fire('Error', response, 'error');
        }


    }

    async function CargarDatos(urlPath, id) {

        let response;

        try {

            response = await $.post({
                url: urlPath,
                data: { id: id }

            });
            //debugger;
            return response;


        } catch (e) {
            console.log(e);
        }
    }

    async function Solicitud(solicitud) {
        debugger;
        $("#txtContraparteTecnica").data("valor", solicitud.contraparteTecnica.id).addClass("form-control-sm");
        await CargarSelect(txtContraparteTecnica, Index.urlGetContraparteTecnica, true);

        jsolicitud = solicitud;
        //btnAprobacionId.va =
        IDNombreSolicitante.val(solicitud.solicitante.fullName).addClass("form-control-sm");
        IDDepartamentoSolicitante.val(solicitud.solicitante.sector.nombre).addClass("form-control-sm");
        IDFechaSolicitud.val(solicitud.fechaCreacion).addClass("form-control-sm");
        txtNombreCompra.val(solicitud.nombreCompra).attr("disabled", "disabled").addClass("form-control-sm");
        txtObjetivoJustificacion.val(solicitud.objetivoJustificacion).attr("disabled", "disabled").addClass("form-control-sm");  //
        txtObservacionGeneral.val(solicitud.observacionGeneral).attr("disabled", "disabled").addClass("form-control-sm").removeAttr("placeholder");

        //txtContraparteTecnica.val(solicitud.contraparteTecnica.fullName);
        txtUnidadDemandante.val(solicitud.unidadDemandante.nombre);
        txtProgramaPresupuestario.val(solicitud.programaPresupuestario.nombre);
        IDIniciativaVigente.val(solicitud.iniciativaVigenteId);
        IDNombreIniciativaVigente.val(solicitud.iniciativaVigente);
        txtConceptoPresupuestario.val(solicitud.conceptoPresupuestario.nombre);
        txtFolioRequerimientoSIGFE.val(solicitud.folioRequerimientoSIGFE);
        txtFolioCompromisoSIGFE.val(solicitud.foliocompromisoSIGFE);
        //btnCDP
        //btnGuardarArchivo
        //frmDropZone
        txtModalidadCompra.val((solicitud.modalidadCompraId== 2) ?"Multi Anual":"Anual"); 
        txtTipoCompra.val(solicitud.tipoCompra.nombre);
        txtTipoMoneda.val(solicitud.tipoMoneda.nombre);
        txtMontoAprox.val(FormateaMonto(solicitud.montoAprox, 2));
        txtMontoUTM.val(solicitud.montoUTM);
        txtMontoOC.val(FormateaMonto(solicitud.montoAnhoActual, 2));


        if (solicitud.montoAnhoActual > 0)
            txtMontoOC.prop("disabled", true);
        else
            txtMontoOC.prop("disabled", false);
        //txtMontoAnhoActual
        //txtMontoAprox
        //btnAjustarCDP
        //txtMontoAnhoActual
        //btnMasAnios
        IdSolicitud.val(solicitud.id);
        NroSolicitud.val(solicitud.nroSolicitud);

        //tablaSolicitud.html(texto);



        CargarMontoMultiAnual(solicitud.id);
        bitacora(solicitud.id);
        bitacoraArchivo(solicitud.id);
        aprobaciones(solicitud.id);




        divisaSeleccionda = solicitud.tipoMonedaId;
        
            
        contentAjusteCDP.css("display", "block");
        


    }

    async function bitacora(id) {
        //debugger;
        let objeto = await CargarDatos(Index.urlBitacora, id)
        let total = objeto.length;
        let texto = "";
        var i = 0;
        if (total == null)
            return;

        for (let item in objeto) {

            let obs = (objeto[item].observacion == null) ? "" : objeto[item].observacion;
            texto += "<tr> <td> " + objeto[item].user.fullName + " </td><td> " + objeto[item].fecha.substring(0, 19).replace("T", " ") + " </td><td> " + obs + " </td></tr>";
        }

        tblBitacora.html(texto);

    }

    async function aprobaciones(id) {

        let objeto = await CargarDatos(Index.urlAprobaciones, id)
        let total = objeto.length;
        let texto = "";
        var i = 0;
        if (total == null)
            return;

        for (let item in objeto) {

            let aprobado = (objeto[item].estaAprobado == true) ? "<span style='font-size: 2em; color: LightGreen; '><i class='far fa-check-circle'> </i></span>" : "<span style='font-size: 2em; color: Tomato; '><i class='far fa-times-circle'></i></span>";
            let obs = (objeto[item].observacion == null) ? "" : objeto[item].observacion;
            texto += "<tr> <td> " + objeto[item].userAprobador.fullName + " </td><td> " + aprobado + " </td><td> " + obs + " </td></tr>"

        }


        tblAprobaciones.html(texto);

    }

    async function bitacoraArchivo(id) {
        //debugger;
        let objeto = await CargarDatos(Index.urlBitacoraArchivos, id)
        let total = objeto.length;
        let texto = "";
        var i = 0;
        if (total == null)
            return;

        for (let item in objeto) {

            let btnVer = $('<a/>').attr({
                class: "btn btn-outline-primary btn-sm",
                href: Index.urlVer + "/" + objeto[item].id,
                target: "_blank",
                title: "Ver"
            }).append($("<i>").attr({ class: "far fa-eye" }));

            let btnDescargar = $('<a/>').attr({
                class: "btn btn-outline-primary btn-sm",
                href: Index.urlDescargar + "/" + objeto[item].id,
                target: "_blank",
                title: "Descargar"
            }).append($("<i>").attr({ class: "fa fa-download" }));


            let tr = $('<tr/>');
            let tdNombre = $('<td/>').text(objeto[item].nombre);
            let tdUsuario = $('<td/>').text(objeto[item].nombreUsuario);
            let tdFecha = $('<td/>').text(objeto[item].fechaCreacion);
            let tdAcciones = $('<td/>').append(btnVer, " ", btnDescargar);


            tr.append(tdNombre, tdUsuario, tdFecha, tdAcciones);



            $("#BitacoraArchivoCuerpo").append(tr);

            //texto += "<tr>  <td>" + objeto[item].nombre + " </td><td>" + objeto[item].usuarioId + "  </td><td>" + objeto[item].fechaCreacion + "  </td><td>  </td></tr>";
        }

        //$("#BitacoraArchivoCuerpo").html(texto);

    }

    async function guardaDatosPresupuesto(obs) {

        try {
            debugger;
            let data = armaJsonAjusteCDP();
            let montoOC = DesFormatearMonto(txtMontoOC.val());
            jsolicitud["detalle"] = data;
            jsolicitud["montoAprox"] = montoOC;

            //let data = ArmaObj()

            if (jsolicitud !== undefined) {
                jsolicitud["observacionGeneral"]= obs;
            }

            $("#loader").show();

            let response = await guardarTransaccion(Index.urlAjustarSolicitud, jsolicitud);

            //console.log("respuesta:");
            //console.log(response);

            $("#folioGuardadoMsje").show();
            //btnAutorizaCDP.prop('readonly', false);
        } catch (e) {
            alert(e);
        } finally {
            $("#loader").hide();
        }
    }

    btnConfirm.click(async function () {

        await GuardarArchivo();



    });

    function validaMontos() {

        //var valores = $('#divMultianual').children('input');
        let suma = 0.0;
        let cant = 0;
        $('.Ajuste').each(function () {
            cant += 1;
            valor = $(this).val();
            suma += parseFloat(DesFormatearMonto(valor));

        });

        let totalAprox = DesFormatearMonto(txtMontoAprox.val());
        let totalOC = DesFormatearMonto(txtMontoOC.val());
        if (parseFloat(totalOC) > parseFloat(totalAprox)) {
            Swal.fire('El monto total OC no puede exceder el monto presupuestado, solo se pueden hacer ajustes de reducción de montos', '', 'info');
            return true;
        }
        else if (totalOC != suma) {
            Swal.fire('El monto total OC debe coincidir con la suma de los valores anuales', '', 'info');
            return true;
        } else if (suma == 0) {
            Swal.fire('El monto de una compra no puede ser 0', '', 'info');
            return true;
        }
        
        return false;

    }

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



}();