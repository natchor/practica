Aprobacion = function () {

    let form = $("#frmAprobar");
    let btnRechazar = $("#btnRechazar");
    let btnAprobar = $("#btnAprobar");
    let btnAprobarCDP = $("#btnAutorizaCDP");
    let hdnId = $("#hdnId");
    let CdpId = $("#cdpVerId");
    let btnAutorizaCS = $("#btnAutorizaCS");

    let hdnMontoCDPId = $("#hdnMontoCDPId");
    var CDPAutorizado = false;
    let hdnRolId = $("#hdnRolId");
    let ValidacionCDP = $("#ValidacionCDP");
    let txtRequerimientoSIGFE = $("#txtFolioRequerimientoSIGFE");
    let tbMontoUTM = $("#txtMontoUTM");
    let txtAntecedente = $("#txtAntecedente");
    let txtIDCuentaCorriente = $("#txtIDCuentaCorriente");
    let txtIDSaldoCuenta = $("#txtIDSaldoCuenta");
    let txtIDBanco = $("#txtIDBanco");
    let hdnConvenioId = $("#hdnConvenioId");
    let hdnCertificadoSaldo = $("#hdnCertificadoSaldo");
    let CSAprobado = false;

    $(document).ready(async function () {
        CDPAutorizado = ValidacionCDP.val();

    });

    btnAprobar.click(async function () {
        debugger;
        try {
            $(this).cargando();
            

            

            if (txtRequerimientoSIGFE.val() == ""  && hdnRolId.val() == 2) {
                $("#txtRequerimientoSIGFE").focus();

                Swal.fire('CDP', 'Antes de Aprobar debe ingresar folio requerimiento SIGFE', 'warning');
                return;
            }

            if (txtRequerimientoSIGFE.val() == "" && hdnRolId.val() == 8) {
                $("#txtRequerimientoSIGFE").focus();

                Swal.fire('CS', 'Antes de Aprobar debe ingresar folio requerimiento SIGFE', 'warning');
                return;
            }
            if (txtAntecedente.val() == "" && hdnRolId.val() == 8) {
                $("#txtAntecedente").focus();

                Swal.fire('CS', 'Antes de Aprobar debe ingresar glosa antecedente de convenio', 'warning');
                return;
            }
   

            if ((txtIDCuentaCorriente.val() == "" || txtIDSaldoCuenta.val() == "" || txtIDBanco.val() == "" || !CSAprobado) && hdnRolId.val() == 9 && hdnConvenioId.val() != 0 ) {
                txtIDCuentaCorriente.focus();
                Swal.fire('CS', 'Antes de Aprobar debe completar todos los datos de convenio y aprobar el certificado de saldo', 'warning');
                return;
            }

            if (hdnRolId.val() == 3 && hdnConvenioId.val() != 0 && CSAprobado==false) {
                Swal.fire('CS', 'Antes de Aprobar debe completar todos los datos de convenio y aprobar el certificado de saldo', 'warning');
                return;
            } else {
                await submitFrm(Index.urlAprobarSolicitud, "Aprobada", "Solicitud aprobada con exito");
                return;
            }



            if (hdnRolId.val() == 2 || hdnRolId.val() == 3) {
                if (CDPAutorizado == true || CDPAutorizado == "True")
                    await submitFrm(Index.urlAprobarSolicitud, "Aprobada", "Solicitud aprobada con exito");
                else {
                    if ((hdnRolId.val() == 2 && DesFormatearMonto(tbMontoUTM.val()) > parseFloat(hdnMontoCDPId.val()))) {  //if 
                        await submitFrm(Index.urlAprobarSolicitud, "Aprobada", "Solicitud aprobada con exito");
                    }

                    else { 

                    Swal.fire('CDP', 'Antes de Aprobar debe generar el CDP', 'warning');
                }
                }
            } else {
                //console.log("2");
                await submitFrm(Index.urlAprobarSolicitud, "Aprobada", "Solicitud aprobada con exito");
            }

        } catch (e) {

        } finally {
            $(this).reiniciarCarga();
        }

        

    });

    btnAutorizaCS.click(async function () {
        debugger;
        let response;
        let obj = armaObjCS();
        let validar = validaCamposCS();
        let url = Index.urlAprobarCS1;

        if (hdnRolId.val() == 3)
            url = Index.urlAprobarCS2

        if (!validar) {
            Swal.fire('CS', 'Antes de Autorizar CS debe agregar los datos relacionados al convenio', 'warning');
            return;
        }



        try {
            $(this).cargando();

            response = await $.post({
                url: url,
                data: {obj:obj}
            });


            if ($.isNumeric(response)) {
                btnAutorizaCS.hide();
                CSAprobado = true;
                Swal.fire('CS', 'CS generado', 'success');
                


            }
            else
                Swal.fire('CDP', 'Error al generar CDP', 'success');

            //else
            //Swal.fire('CDP', 'Error al generar CS', 'success');


        } catch (e) {
            console.log(e);
        } finally {
            $(this).reiniciarCarga();
        }


    });

    btnAprobarCDP.click(async function () {
        
        let response;
        let id = hdnId.val();
        let folio = $("#txtFolioRequerimientoSIGFE").val();
        if (folio.length < 1) {
            Swal.fire('CDP', 'Antes de Aprobar CDP debe agregar Folio Requerimiento SIGFE', 'warning');
            return;
        }

        try {
            $(this).cargando();

            response = await $.post({
                url: Index.urlAprobarCDP,
                data: { id: id }
            });



            if ($.isNumeric(response)) {
                btnAprobarCDP.hide();
                let folio = $("#txtFolioRequerimientoSIGFE");
                folio.attr("readonly", true); 
                Swal.fire('CDP', 'CDP generado', 'success');
                CdpId.html("Ver CDP");
                CDPAutorizado = true;
                
                
            }
            else
                Swal.fire('CDP', 'Error al generar CDP', 'success');


        } catch (e) {
            console.log(e);
        } finally {
            $(this).reiniciarCarga();
        }

     
    });

    btnRechazar.click(async function () {

        try {
            $(this).cargando();

            let estaOk = ValidarForm(form);

            if (!estaOk) {
                return;
            }

        
            const result = await Swal.fire({
                title: 'Rechazar solicitud',
                text: "Esta acción no se podrá revertir.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Ok, rechazar',
                cancelButtonText: 'Cancelar',
                reverseButtons: false,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33'
            });

            //debugger;
            //console.log(result)

            if (result.isConfirmed) {
                await submitFrm(Index.urlRechazar, "Rechazada", "Solicitud rechazada con exito");
            }

      

        } catch (e) {

        } finally {
            $(this).reiniciarCarga();
        }

    });

    function validaCamposCS() {

        if (txtAntecedente.val().length < 1) {
            txtAntecedente.focus();
            return false;
        }
        if (txtIDCuentaCorriente.val().length < 1) {
            txtIDCuentaCorriente.focus();
            return false;
        }

        if (txtIDSaldoCuenta.val().length < 1) {
            txtIDSaldoCuenta.focus();
            return false;
        }

        if (txtIDBanco.val().length < 1) {
            txtIDBanco.focus();
            return false;
        }

        return true;


    }

    function armaObjCS() {
       

        var jobj = {};

        jobj['id'] = 0;
        jobj['solicitudId'] = hdnId.val();
        //jobj['antecedente'] = txtAntecedente.val()
        jobj['cuentaCorriente'] = txtIDCuentaCorriente.val();;
        jobj['saldoCuenta'] = txtIDSaldoCuenta.val();
        jobj['banco'] = txtIDBanco.val();
        //jobj['certificadoSaldo'] = hdnId.val(); 
        //jobj['fechaAutorizacionFin'] = hdnId.val(); 
        //jobj['fechaAutorizacionPres'] = hdnId.val(); 

        return jobj;


    }

    async function submitFrm(url, titleMsje, cuerpoMsje) {

        try {


            let estaOk = ValidarForm(form);

            if (!estaOk) {
                return;
            }

            let data = DatosForm(form);

            data["archivos"] = DropZoneConfig.frmDropZone.files.map(f => f.objetoArchivo);;


            //if (hdnRolId.val() == 2) {
            //    data["analistaPresupuestoId"] = 
            //}


            let response = await guardarTransaccion(url, data);


            if (response) {
                await Swal.fire(titleMsje, cuerpoMsje, 'success');
                window.location.href = "/"
            }

        } catch (e) {
            await Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Algo salio mal',
                footer: e.mensajeError
            });
        }
    }

   


}();