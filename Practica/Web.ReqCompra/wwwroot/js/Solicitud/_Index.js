Solicitud = function () {
    let divisa = '';
    let simboloDivisa = "";
    let boolActualiza = false;
    //let simboloDivisaElement = $("span[name='simboloDivisa']");
    let rutExtranjero = $("#rutExtranjero");
    let selectModalidadCompra = $("#txtModalidadCompra");
    let txtNombreCompra = $("#txtNombreCompra");
    let selectContraparteTecnica = $("#txtContraparteTecnica");
    let selectTipoMoneda = $("#txtTipoMoneda");
    let selectTipoCompra = $("#txtTipoCompra");
    let selectUnidadDemandante = $("#txtUnidadDemandante");
    let selectConceptoPresupuestario = $("#txtConceptoPresupuestario");
    let selectProgramaPresupuestario = $("#txtProgramaPresupuestario");
    let tbMontoAprox = $("#txtMontoAprox");
    let tbMontoUTM = $("#txtMontoUTM");
    let txtMontoMultiAnual = $("#txtMontoMultiAnual");
    let txtObservacionGeneral = $("#txtObservacionGeneral");
    let txtMontoAnhoActual = $("#txtMontoAnhoActual");
    let frmSolicitud = $("#frmSolicitud");
    let txtUtm = $("#txtUtm");
    let txtUF = $("#txtUF");
    let txtEuro = $("#txtEuro");
    let txtDolar = $("#txtDolar");
    let hdnId = $("#hdnId");
    let hdnMontoCDPId = $("#hdnMontoCDPId");
    let tblBitacora = $("#BitacoraCuerpo");
    let tblBitacoraEstados = $("#EstadoTramitacionCuerpo");
    let tblAprobaciones = $("#AprobacionCuerpo");
    let div1 = $("#div1");
    let tramiId = $("#tramiId");
    
    let IDIniciativaVigente = $("#IDIniciativaVigente");
    let IDNombreIniciativaVigente = $("#IDNombreIniciativaVigente");

    let btnMasAnios = $("#btnMasAnios");

    let cardOrdenCompra = $("#cardOrdenCompra");
    let cardContrato = $("#cardContrato");

    let ordenCompraId = $("#ordenCompraId");
    let fechaContratoId = $("#fechaContratoId");
    let ajusteID = $("#ajusteID").val();
    let btnDescargarCDP = $("#btnDescargarCDP");
    let btnDescargarCS = $("#btnDescargarCS");
    let hdnConvenioId = $("#hdnConvenioId");
    let hdnCertificadoSaldo = $("#hdnCertificadoSaldo");

    




    let esAprobacion = window.location.pathname.indexOf("/Aprobacion/") > 0;
    let esVer = window.location.pathname.indexOf("/Ver/") > 0;
    let esEditar = window.location.pathname.indexOf("/Editar/") > 0;
    let esIngreso = window.location.pathname.indexOf("/Index") > 0;
    let valSelConTec = false;
    let valSelUniDem = true;
    let valSelProPre = false;
    let valSelConPre = false;
    //let valSelTipMon = false;

    if (esEditar) {
        debugger;
        $("#div4").hide();
        if ($("#div4").html() != "4") {
            window.history.back();
        }

    }

    let tratoDirecto = 4;
    let montoMaxCompraAgil = 1000000; // TODO: en el document ready se puede generar una llamada a back para obtener este valor de forma dinamica


    let hdnRolId = $("#hdnRolId");
    let hdnCargoId = $("#hdnCargoId");
    let divPresupuesto = $("#DivSoloPresupuestoId");
    let txtRequerimeintoSIGFE = $("#txtFolioRequerimientoSIGFE");
    let txtCompromisoSIGFE = $("#txtFolioCompromisoSIGFE");
    let btnAutorizaCDP = $("#btnAutorizaCDP");
    let bitaId = $("#bitaId");
    let classAnuales = $(".Anuales");
    let btnModificarSolicitud = $("#btnModificarSolicitud");
    let btnHabilitarCampos = $("#btnHabilitarCampos");
    let btnsUpdt = $("#btnsUpdt");
    let hdnPuedeActualizar = $("#hdnPuedeActualizar");
    let txtAntecedente = $("#txtAntecedente");
    let txtIDCuentaCorriente = $("#txtIDCuentaCorriente");
    let txtIDSaldoCuenta = $("#txtIDSaldoCuenta");
    let txtIDBanco = $("#txtIDBanco");
    let btnAutorizaCS = $("#btnAutorizaCS");
    let CdpId = $("#btnCDP");
    let txtFolioRequerimientoSIGFE = $("#txtFolioRequerimientoSIGFE");
    //let btnAutorizaCDP = $("#btnAutorizaCDP");
    
    
    
   
    

    $(document).ready(async function () {
        
        if (hdnPuedeActualizar.val()==1 && esVer) {
            btnsUpdt.show("fast");
        } else {
            btnsUpdt.hide("fast");
            btnsUpdt.remove();
        }

        selectTipoMoneda.change();
        ordenCompraId.toggleClass("active");
        
        if (!esIngreso) {
            valSelConTec = true;
            valSelUniDem = true;
            valSelProPre = true;
            valSelConPre = true;
            //valSelTipMon = true;


            bitacora(hdnId.val());
            bitacoraArchivo(hdnId.val());
            mostrarBitacoraEstados(hdnId.val());
            aprobaciones(hdnId.val());
        } else {
            
            bitaId.slideUp();
            bitaId.hide();
        }

       
        btnAutorizaCS.hide();

        if (hdnRolId.val() == 8) {
            txtAntecedente.prop("disabled", false);
            txtAntecedente.focus();
            txtFolioRequerimientoSIGFE.prop("disabled", false);
        }


        if (hdnRolId.val() == 9) {
            txtIDCuentaCorriente.prop("disabled", false);
            txtIDSaldoCuenta.prop("disabled", false);
            txtIDBanco.prop("disabled", false);
            btnAutorizaCS.prop("disabled", false);
            txtIDCuentaCorriente.focus();
            btnAutorizaCS.show();
        }

        if (btnAutorizaCDP.data("access")) {

            btnAutorizaCDP.show();
            //debugger;
            if (DesFormatearMonto(tbMontoUTM.val()) > parseFloat(hdnMontoCDPId.val()) && hdnRolId.val() != 3) {
                hdnMontoCDPId

                btnAutorizaCDP.slideUp();
                btnAutorizaCDP.hide();
            }
        } else {
            btnAutorizaCDP.slideUp();
            btnAutorizaCDP.hide();
        }

        let soloVisualizacion = !esIngreso && !esEditar;
        CargarSelect(selectContraparteTecnica, Index.urlGetContraparteTecnica, soloVisualizacion);
        CargarSelect(selectUnidadDemandante, Index.urlGetSectorConPresupuesto, soloVisualizacion);
        CargarSelect(selectConceptoPresupuestario, Index.urlGetConceptoPresupuestario, soloVisualizacion);
        await CargarSelect(selectProgramaPresupuestario, Index.urlGetProgramaPresupuestario, soloVisualizacion);
        debugger;
        let conv = await CargarDatos(Index.urlGetExtraPresupuestario, hdnId.val());  
        
        if (conv != null) {
            debugger;
            txtAntecedente.val(conv.antecedente) 
            txtIDCuentaCorriente.val(conv.cuentaCorriente);
            txtIDSaldoCuenta.val(conv.saldoCuenta);
            txtIDBanco.val(conv.banco);
            hdnConvenioId.val(conv.id);
            CdpId.hide();
            btnDescargarCDP.hide();
            

        }

        debugger;
        if (hdnCertificadoSaldo.val() && hdnRolId.val() == 3) {
            CdpId.css("display", "none");
            btnAutorizaCDP.css("display", "none");
            btnAutorizaCS.prop("disabled", false);
            btnAutorizaCS.show();
        }

        if (esIngreso) {

            if (selectProgramaPresupuestario.children("option").length == 2) {
                let valor = selectProgramaPresupuestario.children("option").last().val();

                selectProgramaPresupuestario.val(valor);
                //selectProgramaPresupuestario.attr("disabled", "disabled");
            }

        } else {

            let dec = 2;
            divisa = selectTipoMoneda.find('option:selected').text();

            if (divisa == "Peso Chileno") {
                dec = 0;
            }

            tbMontoAprox.val(FormateaMonto(tbMontoAprox.val(), dec));
            tbMontoUTM.val(FormateaMonto(tbMontoUTM.val(), 2));
            txtMontoMultiAnual.val(FormateaMonto(txtMontoMultiAnual.val(), dec));

            await CargarMontoMultiAnual();

            formatearMultianual(dec);
        }

        selectModalidadCompra.change();


        selectContraparteTecnica.on('select.editable-select', async function (e) {
            valSelConTec = true;
        });
        selectUnidadDemandante.on('select.editable-select', async function (e) {
            valSelUniDem = true;
        });
        selectProgramaPresupuestario.on('select.editable-select', async function (e) {
            valSelProPre = true;
        });
        selectConceptoPresupuestario.on('select.editable-select', async function (e) {
            valSelConPre = true;
        });


        agregarTooltipSiempre();
        if (esVer) {
            
            agregarTooltipVer();
        }

    });

    selectTipoMoneda.change(function () {
        divisa = $(this).find('option:selected').text();

        let dec = 2;

        if (divisa == "Peso Chileno") {
            dec = 0;
        }

        convertirAPesosToolTipTipoMoneda(divisa);
        let montoAproxDes = DesFormatearMonto(tbMontoAprox.val());

        let montoAprox = DesFormatearMonto(tbMontoAprox.val());
        let montoAproxFor = FormateaMonto(montoAprox, dec);
        tbMontoAprox.val(montoAproxFor);

        formatearMultianual(dec);


        let divisaConvert = convertirMoneda(montoAproxDes, divisa);
        let valorUTM = divisaConvert.valorMonto;
        valorUTM = FormateaMonto(valorUTM, 2);
        tbMontoUTM.val(valorUTM);

        simboloDivisa = divisaConvert.simboloDivisa;
        $("span[name='simboloDivisa']").html(simboloDivisa);

    });

    function formatearMultianual(dec) {
        $("input[name='SolicitudDetalle']").each(function (index) {
            const montoAprox = DesFormatearMonto($(this).val());
            const montoAproxFor = FormateaMonto(montoAprox, dec);
            $(this).val(montoAproxFor);
        });
    }

    function press(e) {
        let code = e.key;
        let es = false;
        if (code === "Enter" || code === "Tab" || code === "Escape") {
            //e.preventDefault();
            return true;
        }
        return false;
    }

    $(document).on("keydown", "#txtTipoMoneda", function (e) {
        let es = press(e);
        if (es) return;

        //valSelTipMon = false;
    });

    $(document).on("focusout", "#txtContraparteTecnica", function (e) {
        valSelConTec = validarEditableSelect($(this));
    });
    $(document).on("focusout", "#txtUnidadDemandante", function (e) {
        valSelUniDem = validarEditableSelect($(this));
    });
    $(document).on("focusout", "#txtProgramaPresupuestario", function (e) {
        valSelProPre = validarEditableSelect($(this));
    });
    $(document).on("focusout", "#txtConceptoPresupuestario", function (e) {
        valSelConPre = validarEditableSelect($(this));
    });


    function validarEditableSelect(selectElement) {
        let ret = true;
        let valSel = selectElement.siblings('.es-list').find('li.es-visible').text();
        let valEscr = selectElement.val();

        if (valSel != valEscr) {
            ret = false;
        }

        return ret;
    }


    $(document).on("blur", ".Anuales", function () {
        convertirAPesosToolTip($(this),divisa);
        
    });

    $(document).on("blur", "#txtMontoAprox", function () {
        convertirAPesosToolTip($(this), divisa);

    });
    function agregarTooltipVer() {
        tooltip($("#IDIniciativaVigente"), $("#IDIniciativaVigente").val());//IDiniciativa
        tooltip($("#IDNombreIniciativaVigente"), $("#IDNombreIniciativaVigente").val());//NombreIniciativa

        $('#txtUnidadDemandante').prop("disabled", false);
        $('#txtProgramaPresupuestario').prop("disabled", false);
        $('#txtConceptoPresupuestario').prop("disabled", false);
        $("#txtUnidadDemandante").attr("disabled", "disabled");
        $("#txtProgramaPresupuestario").attr("disabled", "disabled");
        $("#txtConceptoPresupuestario").attr("disabled", "disabled");
        tooltip($("#txtUnidadDemandante"), $("#txtUnidadDemandante").prop("title"));//unidad demandante
        tooltip($("#txtProgramaPresupuestario"), $("#txtProgramaPresupuestario").prop("title"));//programaPresupuestario
        tooltip($("#txtConceptoPresupuestario"), $("#txtConceptoPresupuestario").prop("title"));//ConceptoPresupuestario

        $('.Anuales').each(function () {
            convertirAPesosToolTip($(this), divisa);
        });
        convertirAPesosToolTip(tbMontoAprox, divisa);

    }
    function agregarTooltipSiempre() {
        tooltip($("#IDNombreSolicitante"), $("#IDNombreSolicitante").val());//nombre
        tooltip($("#IDFechaSolicitud"), $("#IDFechaSolicitud").val());//fecha
        tooltip($("#IDDepartamentoSolicitante"), $("#IDDepartamentoSolicitante").val());//departamento

 }

    function tooltip(elemento, valor) {
        new jBox('Tooltip', {
            //attach: $(this),
            attach: elemento,
            theme: 'TooltipDark',
            animation: 'zoomOut',
            content: valor
        });
    }



    function convertirAPesosToolTipTipoMoneda(divisa) {
        $('.Anuales').each(function () {
            convertirAPesosToolTip($(this), divisa);   
        });
        convertirAPesosToolTip(tbMontoAprox,divisa);
    }

    function convertirAPesosToolTip(elemento ,divisa) {
        let val = convertirMonedaPesosChilenos(elemento.val(), divisa);
        let valSet = FormateaMonto(val.valorMonto, 0);
        //classAnuales.tooltip({ 'trigger': 'focus', 'title': Math.random() });
        new jBox('Tooltip', {
            //attach: $(this),
            attach: elemento,
            theme: 'TooltipDark',
            animation: 'zoomOut',
            content: "Monto en pesos: $" + valSet
        });

    }

    async function CargarMontoMultiAnual() {

        let response = await $.get({
            url: Index.urlDetalleSolicitud + "/" + hdnId.val()
        });

        console.log(response);

        //$("input[name='SolicitudDetalle']").parent().remove();
        $("#divMultianual").empty();

        let soloVisualizacion = !esIngreso && !esEditar;
        //debugger;
        $.each(response, function (index, item) {

            campoMontosMultiAnual(item, soloVisualizacion, index)

        });

        let divisaSeleccionda = selectTipoMoneda.find('option:selected').text();
        let valSimbolo = convertirMoneda(0, divisaSeleccionda)
        $("span[name='simboloDivisa']").html(valSimbolo.simboloDivisa);
    }

    function campoMontosMultiAnual(item, esVisualizacion, index) {
        debugger;
        let anio = $("input[name='SolicitudDetalle']").last().data("anio") + 1;
        let valor = "";



        if (item != undefined) {
            anio = item.anio;
            valor = FormateaMonto(item.montoMonedaSel, 2);
            if (ajusteID === "1") 
                valor = FormateaMonto(item.montoMonedaSelFinal, 2);
            
        }

        let divAnio = $("<div>").attr({ id: "divAnio" + anio });

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
            required: "required",
            placeholder: "Monto año " + anio
        });

        divTb.append(span).append(tb);


        if (!esVisualizacion) {
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
            else {
                btn = $("<button>").attr({
                    type: "button",
                    //id: "btnMenosAnios",
                    name: "eliminarMontoAnio",
                    class: "btn btn-primary",
                    "data-anio": anio
                }).html($("<i>").attr({ class: "fas fa-minus" }));
            }




            divTb.append(btn)
        }

        divAnio.append(lbl).append(divTb)

        $("#divMultianual").append(divAnio);

        if (esVisualizacion) {
            $("#btnMasAnios").remove();
            //$("input[name='SolicitudDetalle']").attr("disabled", "disabled");
            $("input[name='SolicitudDetalle']").attr('disabled', "disabled");
            
        }


    }

    $(document).on('click', "#printFrm", function () {
        //btnMasAnios.click(function () {

        //$("#frmContainer").printElement();
        //PrintElem("frmContainer");
        let paraImprimir = $("main[role='main']");

        paraImprimir.printThis({
            importCSS: true,
            importStyle: true
            //header: "<h1>Look at all of my kitties!</h1>"
        });

    });

    btnDescargarCDP.click(async function () {
        let id = hdnId.val();
        window.location.href = `${Index.urlGetDescargarCDP}?id=${id}`;
    });

    btnDescargarCS.click(async function () {
        let id = hdnId.val();
        window.location.href = `${Index.urlGetDescargarCS}?id=${id}`;
    });

    /*btnCDP.click(async function () {

        let response;
        let id = hdnId.val();

        try {

            response = await $.post({
                url: Index.urlGetDescargarCDP,
                data: { id: id }
            });
            debugger;
            return response;

        } catch (e) {
            console.log(e);
        }
    });
    */
    div1.click(async function () {
        let id = hdnId.val();
        let objeto = await CargarDatos(Index.urlBitacoraEstado, id);
        debugger;
        let total = objeto.length;
        let html = '<div class="b" ><table class="table table-striped" style = "text-align: left;" > <tbody> <tr><th width="20%" >Usuario</th><th width="20%" >Fecha</th><th width="60%">Observación</th> </tr>';
        var i = 0;     

        for (let item in objeto) {

            let obs = (objeto[item].observacion == null) ? "" : objeto[item].observacion;
            html += '<tr> <td> ' + objeto[item].user.userName + " </td><td> " + objeto[item].fecha.substring(0, 19).replace('T', ' ') + ' </td><td> ' + obs + ' </td></tr>';
        }

        html += '</tbody></table></div>';

        if (total == null || total == 0) {

            html ="Esta solicitud no registra cambios de estados."
            
        }

        mostrarBitacoraEstados(html);

    });

    async function mostrarBitacoraEstados(htmltext) {
        Swal.fire({
            title: 'Bitacora de Estados',
            //icon: 'info',
            html:htmltext,   
            showCloseButton: true,
            showCancelButton: false,
            
            
        })

    }

    async function mostrarBitacoraEstados(id) {

        //let id = hdnId.val();
        let objeto = await CargarDatos(Index.urlBitacoraEstado, id);
        debugger;
        let total = objeto.length;
        let html = '';
        var i = 0;

        for (let item in objeto) {

            let obs = (objeto[item].observacion == null) ? "" : objeto[item].observacion;
            html += '<tr> <td> ' + objeto[item].user.userName + " </td><td> " + objeto[item].fecha.substring(0, 19).replace('T', ' ') + ' </td><td> ' + obs + ' </td></tr>';
        }

        tblBitacoraEstados.html(html);

        if (total == null || total == 0) {
            $('#salto').css('display', 'none');
            tramiId.css('display', 'none');

        }


    }



    $(document).on('click', "#btnMasAnios", function () {
        //btnMasAnios.click(function () {
        if (selectModalidadCompra.val() == 1) {
            $("#btnMasAnios").attr("disabled", "disabled");
            return;
        }
        campoMontosMultiAnual();

        let divisaSeleccionda = selectTipoMoneda.find('option:selected').text();
        let valSimbolo = convertirMoneda(0, divisaSeleccionda)
        $("span[name='simboloDivisa']").html(valSimbolo.simboloDivisa);

    });



    $(document).on("click", "button[name='eliminarMontoAnio']", function () {
        let anio = $(this).data("anio");
        $(this).closest("div[id='divAnio" + anio + "']").remove();
    });

    

    btnHabilitarCampos.click(function () {
        let hdnRolId = $("#hdnRolId");
        btnModificarSolicitud.show();
        btnHabilitarCampos.hide();
        hdnId.prop("disabled", false);
        let rol = hdnRolId.val();
        boolActualiza = true;
        switch (rol) {
            case "3":
            case "2":
                desbloqueaPresup();
                break;
            case "4":
            case "5":
                desbloqueaCompra();
                break;
            case "1":
                desbloqueaPresup();
                desbloqueaCompra();
                break;
            default:

                break;
        }

    });

    

    btnModificarSolicitud.click(async function () {


        const { value: obs } = await  Swal.fire({

            input: 'textarea',
            inputLabel: 'Observación para actualizar Solicitud',
            inputPlaceholder: 'escriba una observación ...',
            inputAttributes: {
                'aria-label': 'escriba una observación ...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para hacer cambios en la solicitud presione el boton "Guardar" `,
            title: 'Actualizar Solicitud',

        });

        if (obs || obs == "") {
            await modifyDatosPresupuestoCompras(obs);
            Toast.fire({
                icon: 'success',
                title: 'Solicitud Actualizada'
            });

        }
        bloquearCampos();
       
        btnModificarSolicitud.hide();
        btnHabilitarCampos.show();

        


    });


    tbMontoAprox.change(function () {

        let montoAproxDes = DesFormatearMonto($(this).val());
        //let montoAproxFor = FormateaMonto(montoAproxDes, dec);
        let montoAproxFor = FormatearCampos($(this), $(this).val());
        divisa = selectTipoMoneda.find('option:selected').text();

        let valueUTM = convertirMoneda(montoAproxDes, divisa).valorMonto;
        //value = DesFormatearMonto(value);
        valueUTM = FormateaMonto(valueUTM, 2);


        /*$(this).val(montoAproxFor);*/
        tbMontoUTM.val(valueUTM);

        if (selectModalidadCompra.val() == 1) {
            txtMontoAnhoActual.val(montoAproxFor)
        }

    });

    function sumarMontosAnuales() {

        let suma = 0.0;
        let cant = 0;
        $('.Anuales').each(function () {
            cant += 1;
            valor = $(this).val();
            suma += parseFloat(DesFormatearMonto(valor));

        });

        suma = FormateaMonto(suma, 2);

        return suma;
    }

    $(document).on("blur", "input[name='SolicitudDetalle']", function () {

        
        FormatearCampos($(this), $(this).val());
        debugger;
       

        let suma = sumarMontosAnuales();
        

        tbMontoAprox.val(suma);
        tbMontoAprox.change();

    });


    function FormatearCampos(campo, valor) {
        let dec = 2;

        if (divisa == "Peso Chileno") {
            dec = 0;
        }

        let montoDesformateado = DesFormatearMonto(valor);
        let montoFormateado = FormateaMonto(montoDesformateado, dec);


        campo.val(montoFormateado);

        return montoFormateado;
    }

    selectModalidadCompra.change(function () {
        

        if (selectModalidadCompra.val() == 2) { // multianual
            //txtMontoMultiAnual.prop('readonly', false);
            txtMontoMultiAnual.removeAttr('disabled');
            $("#btnMasAnios").removeAttr("disabled");
            
        }
        else { // anual
            txtMontoMultiAnual.attr("disabled", "disabled");
            txtMontoMultiAnual.val("");

            $("#btnMasAnios").attr("disabled", "disabled");
            let o = 0;
            $('.Anuales').each(function () {
                if (o > 0) {
                    let anio = $(this).data("anio");
                    $(this).closest("div[id='divAnio" + anio + "']").remove();
                }
                o += 1;
            });

        }

        let suma = sumarMontosAnuales();


        tbMontoAprox.val(suma);
        tbMontoAprox.change();

    });

    /**
     * Convierte monto a UTM
     * @param {any} montoAprox
     * @param {any} tpMoneda
     */
    function convertirMoneda(montoAprox, tpMoneda) {
        //debugger;
        let valor = 0;
        let utm = parseFloat(txtUtm.val().replace(",", "."));
        let uf = parseFloat(txtUF.val().replace(",", "."));
        let euro = parseFloat(txtEuro.val().replace(",", "."));
        let dolar = parseFloat(txtDolar.val().replace(",", "."));

        let simboloDivisa = "";


        switch (tpMoneda) {
            case '1':
            case 'UTM':
                valor = montoAprox;
                simboloDivisa = "UTM";
                break;
            case '4':
            case 'Euro':
                valor = ((montoAprox * euro) / utm);
                simboloDivisa = "€";
                break;
            case '2':
            case 'UF':
                valor = ((montoAprox * uf) / utm);
                simboloDivisa = "UF";
                break;
            case '3':
            case 'Dolar':
                valor = ((montoAprox * dolar) / utm);
                simboloDivisa = "USD";
                break;
            case '5':
            case 'Peso Chileno':
                valor = (montoAprox / utm);
                simboloDivisa = "$";
                break;

            default:
                valor = 0;
        }



        let ret = { valorMonto: valor, simboloDivisa: simboloDivisa };

        return ret;
        //return valor;

    }

    /**
     * Convierte monto a PesosChilenos
     * @param {any} montoAprox
     * @param {any} tpMoneda
     */
    function convertirMonedaPesosChilenos(montoAprox, tpMoneda) {
        //debugger;
        let valor = 0;
        let utm = parseFloat(txtUtm.val().replace(",", "."));
        let uf = parseFloat(txtUF.val().replace(",", "."));
        let euro = parseFloat(txtEuro.val().replace(",", "."));
        let dolar = parseFloat(txtDolar.val().replace(",", "."));

        let simboloDivisa = "";
        let montoDivisa = -1;

        montoAprox = DesFormatearMonto(montoAprox);


        switch (tpMoneda) {
            case '1':
            case 'UTM':
                valor = montoAprox * utm;
                simboloDivisa = "UTM";
                montoDivisa = utm;
                break;
            case '4':
            case 'Euro':
                valor = (montoAprox * euro);
                simboloDivisa = "€";
                break;
            case '2':
            case 'UF':
                valor = (montoAprox * uf);
                simboloDivisa = "UF";
                montoDivisa = uf;
                break;
            case '3':
            case 'Dolar':
                valor = (montoAprox * dolar);
                simboloDivisa = "$";
                montoDivisa = dolar;
                break;
            case '5':
            case 'Peso Chileno':
                valor = montoAprox;
                simboloDivisa = "$";
                montoDivisa = 1;
                break;

            default:
                valor = -1;
        }



        let ret = { valorMonto: valor, simboloDivisa: simboloDivisa, montoDivisa: montoDivisa };

        return ret;
        //return valor;

    }


    txtMontoMultiAnual.keyup(function () {

        let montoAproxDes = DesFormatearMonto($(this).val());

        $(this).val(FormateaMonto(montoAproxDes, 0));
    });


    function ArmaObjCS() {
        var jobj = {};
        
        jobj['id'] = 0;
        jobj['solicitudId'] = hdnId.val(); 
       
        //jobj['autorizadorFinId'] = hdnId.val(); 
        //jobj['autorizadorPresId'] = hdnId.val(); 

        jobj['antecedente'] = txtAntecedente.val()
        //jobj['cuentaCorriente'] = txtIDCuentaCorriente.val();;
        //jobj['saldoCuenta'] = txtIDSaldoCuenta.val();
        //jobj['banco'] = txtIDBanco.val();
        //jobj['certificadoSaldo'] = hdnId.val(); 
        //jobj['fechaAutorizacionFin'] = hdnId.val(); 
        //jobj['fechaAutorizacionPres'] = hdnId.val(); 
        
        
         



        return jobj;
    }

    function ArmaObj() {
        var jobj = {};
        jobj['id'] = hdnId.val();
        jobj['folioRequerimientoSIGFE'] = txtRequerimeintoSIGFE.val();
        jobj['foliocompromisoSIGFE'] = txtCompromisoSIGFE.val();
        


        return jobj;
    }

    function ArmaObjM() {
        var jobj = {};

        jobj['id'] = hdnId.val();

        jobj['contraparteTecnicaId'] = selectContraparteTecnica.val();
        jobj['observacionGeneral'] = txtObservacionGeneral.val();
        jobj['tipoCompraId'] = selectTipoCompra.val();

        jobj['unidadDemandanteId'] = selectUnidadDemandante.val();
        jobj['programaPresupuestarioId'] = selectProgramaPresupuestario.val();
        jobj['iniciativaVigenteId'] = IDIniciativaVigente.val();
        jobj['iniciativaVigente'] = IDNombreIniciativaVigente.val();
        jobj['conceptoPresupuestarioId'] = selectConceptoPresupuestario.val();
        jobj['folioRequerimientoSIGFE'] = txtRequerimeintoSIGFE.val();
        jobj['foliocompromisoSIGFE'] = txtCompromisoSIGFE.val();




        return jobj;
    }

    txtRequerimeintoSIGFE.blur(async function () {
        if (boolActualiza)
            return;

        if ($(this).val().length > 0) {
            guardaDatosPresupuesto();
        }

    });

    txtAntecedente.blur(async function () {
        if (boolActualiza)
            return;
        if ($(this).val().length > 0) {
            debugger;



            await guardaGlosaCS("");


            Toast.fire({
                icon: 'success',
                title: 'Glosa CS registrada'
            });



        }

    });

    txtCompromisoSIGFE.blur(async function () {
        if (boolActualiza)
            return;
        if ($(this).val().length > 0) {
            debugger;

            const { value: obs } = await Swal.fire({

                input: 'textarea',
                inputLabel: 'Observación Folio Compromiso SIGFE',
                inputPlaceholder: 'Si tienes una observación, escribir aqui...',
                inputAttributes: {
                    'aria-label': 'Si tienes una observación, escribir aqui...'
                },
                showCancelButton: true,
                confirmButtonText: `Guardar`,
                cancelButtonText: `Cancelar`,
                text: `Para guardar folio compromiso SIGFE "${txtCompromisoSIGFE.val()}" con o sin observaciones presione el boton "Guardar" `,
                title: 'Registro Folio compromiso SIGFE',

            });

            if (obs || obs == "") {
                await guardaDatosPresupuesto(obs);

               
                Toast.fire({
                    icon: 'success',
                    title: 'Folio de Compromiso SIGFE registrado'
                });

            }


            //guardaDatosPresupuesto();

            
        }

    });


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
            let tdAcciones = $('<td/>').append(btnVer," ", btnDescargar);


            tr.append(tdNombre, tdUsuario, tdFecha, tdAcciones);



            $("#BitacoraArchivoCuerpo").append(tr);

            //texto += "<tr>  <td>" + objeto[item].nombre + " </td><td>" + objeto[item].usuarioId + "  </td><td>" + objeto[item].fechaCreacion + "  </td><td>  </td></tr>";
        }
        
        //$("#BitacoraArchivoCuerpo").html(texto);

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

    async function guardaGlosaCS(obs) {

        try {


            let data = ArmaObjCS()

            if (data !== undefined) {
                data['observacion'] = obs;
            }

            $("#loader").show();

            let response = await guardarTransaccion(Index.urlGuardarGlosaCS, data);


        } catch (e) {
            alert(e);
        } finally {
            $("#loader").hide();
        }
    }


    async function guardaDatosPresupuesto(obs) {

        try {


            let data = ArmaObj()

            if (data !== undefined) {
                data['observacion'] = obs;
            }

            $("#loader").show();

            let response = await guardarTransaccion(Index.urlGuardarCampoPresupuesto, data);

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

    async function modifyDatosPresupuestoCompras(obs) {
        debugger;
        try {


            let data = ArmaObjM();
            data["archivos"] = DropZoneConfig.frmDropZone.files.map(f => f.objetoArchivo);;
            data["objetivoJustificacion"]=obs;
            //if (data !== undefined) {
            //data['observacion'] = obs;
            //}

            //$("#loader").show();

            let response = await guardarTransaccion(Index.urlModify, data);

            //console.log("respuesta:");
            //console.log(response);
            if (response != null) {
                Swal.fire('Los datos han sido Actualizados')
            } else {
                Swal.fire('Error a intentar guardar los datos')
            }

            //btnAutorizaCDP.prop('readonly', false);
        } catch (e) {
            alert(e);
        } finally {
            //$("#loader").hide();
        }
    }


    function validaMontos() {

        //var valores = $('#divMultianual').children('input');
        let suma = 0.0;
        let cant = 0;
        $('.Anuales').each(function () {
            cant += 1;
            valor = $(this).val();
            suma += parseFloat(DesFormatearMonto(valor));

        });

        suma = parseFloat(suma).toFixed(2);

        let totalOC = parseFloat(DesFormatearMonto(tbMontoAprox.val()));// DesFormatearMonto(tbMontoAprox.val());
        if (totalOC != suma) {
            Swal.fire('El monto total OC debe coincidir con la suma de los valores anuales', '', 'info');
            return true;
        } else if (suma == 0) {
            Swal.fire('El monto de una compra no puede ser 0', '', 'info');
            return true;
        }
        else if (selectModalidadCompra.val() == 2 && cant < 2) {
            Swal.fire('Si seleccionó compra multianual debe completar al menos 2 años', '', 'info');
            return true;
        } else if (DesFormatearMonto(tbMontoUTM.val()) > 30.0 && selectTipoCompra.val() == 3) {
            Swal.fire('Compra Agíl no puede superar las 30 UTM ', '', 'info');
            return true;
        //} else if (!valSelTipMon) {
        //    selectTipoMoneda.focus();
        //    Swal.fire('Seleccione una moneda valida', '', 'info');
        //    return true;
        } else if (!valSelUniDem) {
            $("#txtUnidadDemandante").focus();
            Swal.fire('Seleccione una unidad demandante valida', '', 'info');
            return true;
        } else if (!valSelProPre) {
            $("#txtProgramaPresupuestario").focus();
            Swal.fire('Seleccione un programa presupuestario valido', '', 'info');
            return true;
        } else if (!valSelConPre) {
            $("#txtConceptoPresupuestario").focus();
            Swal.fire('Seleccione un concepto presupuestario valido', '', 'info');
            return true;
        } else if (!valSelConTec) {
            $("#txtContraparteTecnica").focus();
            Swal.fire('Seleccione un responsable de recepción valido', '', 'info');
            return true;
        }
        return false;

    }

    selectTipoCompra.change(function () {

        let msje = $("#msjeTipoCompra");
        debugger;
      
        if ($(this).val() == tratoDirecto && !msje.is(":visible")) {
            msje.show();
        } else {
            msje.hide();
        }

    })

    function validaArchivos() {

        let files = DropZoneConfig.frmDropZone.files.map(f => f.objetoArchivo);

        if (files.length == 0) {
            Swal.fire('Debe ingresar al menos un archivo', '', 'info');
            /* btnSubmit.reiniciarCarga();*/
            return true;
        }

        return false;
    }




    function marcarActiva(btn) {
        $(".bandejas").removeClass("active");
        //btn.addClass("active");
    }

    function bloquearCampos() {
        selectUnidadDemandante.prop("disabled", true);
        selectConceptoPresupuestario.prop("disabled", true);
        selectProgramaPresupuestario.prop("disabled", true);
        txtRequerimeintoSIGFE.prop("disabled", true);
        txtCompromisoSIGFE.prop("disabled", true);
        IDIniciativaVigente.prop("disabled", true);
        IDNombreIniciativaVigente.prop("disabled", true);
        selectContraparteTecnica.prop("disabled", true);
        selectTipoCompra.prop("disabled", true);
        txtObservacionGeneral.prop("disabled", true);
    }

    function desbloqueaPresup() {
        selectUnidadDemandante.prop("disabled", false);
        selectConceptoPresupuestario.prop("disabled", false);
        selectProgramaPresupuestario.prop("disabled", false);
        txtRequerimeintoSIGFE.prop("disabled", false);
        txtCompromisoSIGFE.prop("disabled", false);
        IDIniciativaVigente.prop("disabled", false);
        IDNombreIniciativaVigente.prop("disabled", false);
        IDNombreIniciativaVigente.removeAttr("readonly");
        IDIniciativaVigente.removeAttr("readonly");


    }

    function desbloqueaCompra() {
        selectContraparteTecnica.prop("disabled", false);
        selectTipoCompra.prop("disabled", false);
        txtObservacionGeneral.prop("disabled", false);
    }

    ordenCompraId.click(async function () {

        marcarActiva(this);
        ordenCompraId.toggleClass("active");
        try {
            $(this).cargando();
            cardOrdenCompra.css("display", "block");
            cardContrato.css("display", "none");

        } catch (e) {

        } finally {

            $(this).reiniciarCarga();

        }
        return;
    });

    fechaContratoId.click(async function () {

        marcarActiva(this);
        fechaContratoId.toggleClass("active");
        try {
            $(this).cargando();
            cardOrdenCompra.css("display", "none");
            cardContrato.css("display", "block");





        } catch (e) {

        } finally {

            $(this).reiniciarCarga();

        }
        return;
    });


    frmSolicitud.submit(async function (e) {
        e.preventDefault();
        debugger;
        //alert();

        let validaMon = await validaMontos();
        let validaArch = await validaArchivos();

        if (validaMon || validaArch)
            return;


        let btnSubmit = $("button[type='submit']");
        btnSubmit.cargando();

        let confirmResult = await Swal.fire({
            title: '¿Esta seguro de guardar nueva solicitud?',
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: `Si, ingresar`,
            denyButtonText: `No ingresar`,
            showLoaderOnConfirm: true
        });


        if (!confirmResult.value) {
            await Swal.fire('Ok! no se guardo la solicitud', '', 'info');
            btnSubmit.reiniciarCarga();
            return;
        }

        try {

            let estaOk = ValidarForm($(this));

            if (!estaOk) {
                return;
            }

            let data = DatosForm($(this));


            data["archivos"] = DropZoneConfig.frmDropZone.files.map(f => f.objetoArchivo);;

            let obj = [];

            $("input[name='SolicitudDetalle']").each(function (index) {

                let monto = convertirMonedaPesosChilenos($(this).val(), divisa).valorMonto;

                let detalle = {
                    //montoPresupuestado: $(this).val(),
                    montoPresupuestado: monto.toString().replace(".", ","),
                    montoMonedaSel: $(this).val(),
                    anio: $(this).data("anio"),
                };

                obj.push(detalle);

            });

            data["detalle"] = obj;

            //console.log(data);
            let response = await guardarTransaccion(Index.urlGuardar, data);


            if (response) {
                let msjeTitulo = data.Id > 0 ? 'Solicitud actualizada' : 'Nueva solicitud';

                await Swal.fire(msjeTitulo, 'La solicitud fue guardada', 'success');
                window.location.href = "/";
            }


        } catch (e) {
            console.log(e);
            await Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Algo salio mal',
                footer: e.mensajeError
            });
        } finally {
            btnSubmit.reiniciarCarga();
            //BtnReset(btnSubmit);

        }
    });


    return {
        convertirMoneda: convertirMoneda,
        convertirMonedaPesosChilenos: convertirMonedaPesosChilenos
    }

}();