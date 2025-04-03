

Home = function () {

    let solicitudesId = "#tblSolicitudes";
    let tblSolicitudes = $(solicitudesId);
    let urlPath = "";//Home.urlTablaSolicitud ;
    let tablaSolicitudes = $("#divSolicitudes");
    //let tblGestionSolicitudes = $("#tblGestionSolicitudes");
    let btnGestion = $("#gestionId");
    let btnMiGestion = $("#divMiGestion");
    let btnMisAprobaciones = $("#aprovacionesId");
    let btnMisSolicitudes = $("#misSolicitudesId");
    let btnPendientes = $("#PendientesId");

    let btnLicitacion = $("#btnLicitacion");


    let mainTable = null;


    $(document).ready(async function () {
   

        await CargarTabla(tablaSolicitudes, Index.urlGetMisAprobaciones);
        mainTable.order([1, 'desc']).draw();

        marcarActiva(this);
        btnMisAprobaciones.toggleClass("active");
       
    });

    function marcarActiva(btn) {
        $(".bandejas").removeClass("active");
        //btn.addClass("active");
    }
    

    btnMisAprobaciones.click(async function () {
        
        marcarActiva(this);
        btnMisAprobaciones.toggleClass("active");
        try {
            $(this).cargando();
            await CargarTabla(tablaSolicitudes, Index.urlGetMisAprobaciones);

            //debugger;

            mainTable.order([1, 'desc']).draw();

        } catch (e) {

        } finally {

            $(this).reiniciarCarga();

        }
        return;
    });

    btnPendientes.click(function () {
        marcarActiva(this);
        btnPendientes.toggleClass("active");
        try {
            $(this).cargando();

            CargarTabla(tablaSolicitudes, Index.urlGetPorFinalizar);

        } catch (e) {

        } finally {
            $(this).reiniciarCarga();
        }
        return;
    });

    btnMisSolicitudes.click(function () {
        marcarActiva(this);
        btnMisSolicitudes.toggleClass("active");
        try {
            $(this).cargando();

            CargarTabla(tablaSolicitudes, Index.urlGetSolicitudes);

        } catch (e) {

        } finally {
            $(this).reiniciarCarga();
        }
        return;
    });

    btnMiGestion.click(async function () {
        marcarActiva(this);
        btnMiGestion.toggleClass("active");
        try {
            $(this).cargando();

            CargarTabla(tablaSolicitudes, Index.urlGetMisGestiones);

        } catch (e) {

        } finally {
            $(this).reiniciarCarga();
        }
        return;

    });

    $(document).on('click', "#btnLicitacion", async function () {

        try {

            
            let idSolic = $(this).closest('td').attr('tpCompId');
            let datos = await CargarDatosSelect(Index.urlGetLisEstadoLic, idSolic);

            const { value: estdLic } = await Swal.fire({
                title: 'Seleccione el estado e ingrese una observación',
                input: 'select',
                inputOptions: datos,
                html: '<textarea class="form-control" id="TextareaId1" rows="3" placeholder="ingrese una observación (opcional)"></textarea>',

                icon: 'warning',
                inputPlaceholder: 'Seleccione el estado',
                showCancelButton: true,
                inputValidator: (value) => {
                    return new Promise((resolve) => {
                        //if (value === 'revB') {
                        resolve()
                        //} else {
                        //    resolve('You need to select Revisión bases :)')
                        //}
                    })
                }
            });

            if (!estdLic) {
                return;
            }
            let obs = $("#TextareaId1").val();
            
            const response = await $.post({
                url: Index.urlSetEstadoLic,
                data: { estado: estdLic, solicitudId: $(this).data("solicitudid"), obs: obs }
            });


            //let row = $(solicitudesId).DataTable().row(`#row-${$(this).data("solicitudid")}`);
            let tbl = $(solicitudesId).DataTable();
            tbl.cell(`#row-${$(this).data("solicitudid")}`, 2).data(response.nombre);
            tbl.draw();
            //let row = table.row(`#row-${$(this).data("solicitudid")}`);

            //console.log(row.data());

            Toast.fire({
                icon: 'success',
                title: `Estado de licitación actualizado con exito.`
            });

        } catch (e) {
            Toast.fire({
                icon: 'error',
                title: `Error al actualizar estado de licitación`
            });
        }

        

    });

    $(document).on('click', "#btnContrato", async function () {

        try {

            const { value: estdLic } = await Swal.fire({
                title: 'Ingrese las fechas de inicio y fin del contrato',
                allowOutsideClick: false,
                html: ' <div class="form-group">' +
                    '       <label><b>Fecha inicio contrato</b> </label>' +
                    '           <input class="form-control" type="date" id="fechainicioID" placeholder="Fecha inicio contrato" required>' +
                    '   </div>' +

                   
                    '   <div class="form-group">' +
                    '      <label><b>Fecha fin contrato</b> </label>' +
                   
                    '       <input class="form-control" type="date" id="fechafinID"  placeholder="Fecha fin contrato" required>' +
                    '</div>'+
                '<div id="mensaje" style="display:none;"> <span style="color: red;"> Debe ingrsar ambas fechas para continuar. </span><div>',
               
               
                showCancelButton: true,
                inputValidator: (value) => {
                    return new Promise((resolve) => {
                        //if (value === 'revB') {
                        resolve()
                        //} else {
                        //    resolve('You need to select Revisión bases :)')
                        //}
                    })
                }
            });

            if (!estdLic) {
                
                return;
            }
            let fechaI = $("#fechainicioID").val();
            let fechaF = $("#fechafinID").val();
            if (fechaI == null || fechaF == null || fechaI == "" || fechaF == "") {
                let mensaje = $("#mensaje");
                mensaje.css("display", "block");
                Swal.fire('Debe ingresar ambas fechas para guardar intentelo nuevamente.')
                return;

            }

            const response = await $.post({
                url: Index.urlFechaContrato,
                data: { fechaI: fechaI, fechaF: fechaF, solicitudId: $(this).data("solicitudid") }
            });
                

            Toast.fire({
                icon: 'success',
                title: `Fechas actualizadas con exito.`
            });

        } catch (e) {
            Toast.fire({
                icon: 'error',
                title: `Error al actualizar fechas de contrato`
            });
        }



    });


    btnGestion.click(async function () {
        marcarActiva(this);
        btnGestion.toggleClass("active");
        try {
            $(this).cargando();

            await CargarTabla(tablaSolicitudes, Index.urlGetGestion);

            $("#tblSolicitudes").DataTable().column(6).visible(false)
            $("#tblSolicitudes").DataTable().column(8).visible(false);
            $("#tblSolicitudes").DataTable().column(9).visible(false);

            let selectEstados = $("#selectEstados");

            await CargarSelectEditable(selectEstados, Index.urlSelectEstados, false);

            SumoSelectLoad();


        } catch (e) {

        } finally {
            $(this).reiniciarCarga();
        }
        return;
    });


    SumoSelectLoad = () => {
        $('#selectColumnas').SumoSelect({
            placeholder: 'Seleccionar aqui',
            captionFormat: '{0} Seleccionados',
            search: true,
            searchText: 'Buscar...',
            noMatch: 'No existen coincidencias para "{0}"'
        });

        $('#selectColumnas').on('sumo:opened', function (sumo) {

            $("#tblSolicitudes").DataTable().columns().eq(0).each(function (index) {
                var column = $("#tblSolicitudes").DataTable().column(index);

                /*    console.log(column);*/

                if (column.visible())
                    $('#selectColumnas')[0].sumo.selectItem(index);
            });

        });
    }
   

    $(document).on('change', "select[name='GSfiltros']", async function () {

        let estaadoIdVal = $("#selectEstados").val();
        let tieneComSIGFEVal = $("#selectTieneComSIGFE").val();
        let tieneEnvOCVal = $("#selectTieneEnvOC").val();

        let data = { estadoId: estaadoIdVal, tieneComSIGFE: tieneComSIGFEVal, tieneFechaEnvOC: tieneEnvOCVal }


        await CargarTabla(tablaSolicitudes, Index.urlGetGestion, data);
        let selectEstados = $("#selectEstados");

        selectEstados.attr("data-valor", estaadoIdVal);
        $("#selectTieneComSIGFE").val(tieneComSIGFEVal);
        $("#selectTieneEnvOC").val(tieneEnvOCVal);

        await CargarSelectEditable(selectEstados, Index.urlSelectEstados, false);

        $("#tblSolicitudes").DataTable().column(6).visible(false);
        $("#tblSolicitudes").DataTable().column(8).visible(false);
        $("#tblSolicitudes").DataTable().column(9).visible(false);
        SumoSelectLoad();

    });


    async function CargarDatosSelect(urlPath, tipoCompra) {

        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { tipoCompra: tipoCompra }
            });

            let opciones = '{'

            for (let item in response) {
                opciones += ',"' + response[item].codigoStr + '" : "' + response[item].nombre + '"';
            }
            opciones += '}';

            opciones = opciones.replace(',', '');
            let lista = JSON.parse(opciones);

            return lista;


        } catch (e) {
            console.log(e);
        }


    }


    async function CargarTabla(elemento, urlPath, filtros = undefined) {

        let response;

        try {
            
            response = await $.get({
                url: urlPath,
                data: filtros
            });
            elemento.html(response);
            //alert("aqui");

            //$(document).ready(function () {
            if (!$.fn.DataTable.isDataTable(solicitudesId)) {

                $(`${solicitudesId} thead tr`).clone(true).appendTo(`${solicitudesId} thead`);
                $(`${solicitudesId} thead tr:eq(1) th`).each(function (i) {
                    let title = $(this).text();
                    $(this).html(`<input type="text" class="form-control form-control-sm" placeholder="Buscar ${title}" />`);

                    $('input', this).on('keyup change', function () {
                        //debugger;
                        if (mainTable.column(i).search() !== this.value) {
                            mainTable
                                .column(i)
                                .search(this.value)
                                .draw();
                        }
                    });
                });


                mainTable = $(solicitudesId).DataTable({
                    responsive: true,
                    orderCellsTop: true,
                    fixedHeader: true
                });

                $(document).on('click', ".SumoSelect .multiple ul li", async function () {
                    let idColumn = -1;
                    let sel = $(this).children("label").html();

                    $('select#selectColumnas option:contains(' + sel + ')').each(function () {
                        if ($(this).text() == sel) {
                            idColumn = $(this).val();
                            return false;
                        }
                        return true;
                    });

                    //alert(idColumn);

                    var column = mainTable.column(idColumn);

                    column.visible(!column.visible());

                });


            }


        } catch (e) {
            console.log(e);
        }

    }


}();