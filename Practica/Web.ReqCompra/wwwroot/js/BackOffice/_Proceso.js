Proceso = function () {

    //let hdnId = $("#CargoId");
    let tablaSolicitud = $("#divSolicitud");
    let txtBuscar = $("#solicitudBuscarId");
    let botonBuscar = $("#btnAprobacionId");



    $(document).ready(async function () {
        txtBuscar.tooltip({ 'trigger': 'focus', 'title': 'Ingrese año y número de solicitud los 0 son opcionales' });

    });


    $(document).on('click', ".BtnAnular", async function () {
        //debugger;
        //let seleccionadosTodos = false;
        //let seleccionados = false;
        //let usuario = $(this).text();
        //let idsolici = $(this).closest('td').attr('solicitudId');
        let fila = $(this).closest('tr');

        let idsolici = fila.attr('IdSolicitud');

        const { value: obs } = await Swal.fire({

            input: 'textarea',
            //inputLabel: '¿Esta seguro de anular la Solicitud?',
            inputPlaceholder: 'Ingrese una Observación',
            inputAttributes: {
                'aria-label': 'Ingrese una Observación...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para anular la solicitud presione el botón "Guardar" `,
            title: 'Anular Solicitud',

        });

        if (obs || obs == "") {
            let id = {};
            id["solicitudId"] = idsolici;
            id["observacion"] = obs;
            let response = await CargarDatos(Index.urlPostAnulaSolicitud, id)

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
                title: 'Solicitud Anulada'
            })

        }
    });


    $(document).on('click', ".BtnDesFinalizar", async function () {

        let fila = $(this).closest('tr');

        let idsolici = fila.attr('IdSolicitud');

        const { value: obs } = await Swal.fire({

            input: 'textarea',
            //inputLabel: '¿Esta seguro de deshacer estado finalizada de la Solicitud?',
            inputPlaceholder: 'Ingrese una Observación',
            inputAttributes: {
                'aria-label': 'Ingrese una Observación...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para deshacer estado finalizada de la solicitud presione el botón "Guardar" `,
            title: 'Deshacer Estado',

        });

        if (obs || obs == "") {
            let id = {};
            id["solicitudId"] = idsolici;
            id["observacion"] = obs;
            let response = await CargarDatos(Index.urlPostDeshacerEstadoSolicitud, id)

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
                title: 'Se quito Estado Finalizada'
            })

        }
    });


    $(document).on('click', ".BtnRechazar", async function () {

        let fila = $(this).closest('tr');

        let idsolici = fila.attr('IdSolicitud');

        const { value: obs } = await Swal.fire({

            input: 'textarea',
            //inputLabel: '¿Esta seguro de deshacer estado finalizada de la Solicitud?',
            inputPlaceholder: 'Ingrese una Observación',
            inputAttributes: {
                'aria-label': 'Ingrese una Observación...'
            },
            showCancelButton: true,
            confirmButtonText: `Guardar`,
            cancelButtonText: `Cancelar`,
            text: `Para rechazar solicitud presione el botón "Guardar" `,
            title: 'Rechazar Solicitud',

        });

        if (obs || obs == "") {
            let id = {};
            id["solicitudId"] = idsolici;
            id["observacion"] = obs;
            let response = await CargarDatos(Index.urlPostRechazarSolicitud, id)

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
                title: 'Solicitud Rechazada'
            })

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

    botonBuscar.click(function () {
        //debugger;
        CargarTabla(tablaSolicitud, Index.urlGetBuscaSolicitud, txtBuscar.val());
    });

    async function CargarTabla(elemento, urlPath, id) {
       // debugger;
        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id: id }
            });
            
            if (response != null)
                Solicitud(response);
            else {
                elemento.html("");
                Swal.fire('Error', 'Solicitud no encontrada', 'error');
            }

            //elemento.html(response);
            //

            ////$(document).ready(function () {
            //if (!$.fn.DataTable.isDataTable(solicitudesId)) {
            //    $(solicitudesId).DataTable();
            //}



        } catch (e) {
            elemento.html("");
            Swal.fire('Error', 'Solicitud no encontrada', 'error');
        }


    }


    function Solicitud(solicitud) {
        //debugger;
        let texto = " <div class='tab-pane fade show active' id='solicitud' role='tabpanel' aria-labelledby='solicitud - tab'><div class='mt-3'><table id = 'tblsolicitud' class='table table-striped table-bordered'><tr><th style='width: 5%'>Id</th><th style='width: 40%'>Nombre</th><th style='width: 25%'>Estado</th><th style='width: 10%'>Anular</th><th style='width: 10%'>DesFinalizar</th><th style='width: 10%'>Rechazar</th></tr>";
            
        //let total = solicitud.length;

        var i = 0;
        if (solicitud == null)
            return;

        if( solicitud != null) {
            texto += "<tr IdSolicitud=" + solicitud.id + ">" +
                "<td> " + solicitud.id + "</td> " +
                "<td>  <a href='../Solicitud/Ver/" + solicitud.id + "'  target='_blank'  data-toggle='tooltip' title='ver'> " + solicitud.nombreCompra +" </a></td>  "+
                "<td>" + solicitud.estado.nombre + "</td>  ";
            if (solicitud.estadoId < 10)
                texto += "<td align='center'><button type='button' class='btn btn-outline-primary BtnAnular' ><i class='fas fa-eject'></i></i></button></td>";
            else
                texto += "<td align='center'></td>";
            if (solicitud.estadoId == 10)
                texto += "<td align='center'><button type='button' class='btn btn-outline-primary BtnDesFinalizar' ><i class='fas fa-reply'></i></i></button></td>";
            else
                texto += "<td align='center'></td>";
            if (solicitud.estadoId < 10)
                texto += "<td align='center'><button type='button' class='btn btn-outline-primary BtnRechazar' ><i class='fas fa-reply-all'></i></i></button></td>";
            else
                texto += "<td align='center'></td>";

            texto +="</tr > ";

        }
        texto +=  "</table ></div></div>";

        tablaSolicitud.html(texto);


        
               
            

        







    }

   



}();