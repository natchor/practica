Reporte = function () {

    //let hdnId = $("#CargoId");

    let btnReporteTR = $("#rptTiempoRespuesta");
    let rptTiempoRespuestaVer = $("#rptTiempoRespuestaVer");
    //let tabla = $("#tblSolicitudes");



    $(document).ready(async function () {
        
        btnReporteTR.attr('href', Index.urlDescargar);

    });







    rptTiempoRespuestaVer.click(async function() {
        debugger;
        //alert("estoy aqui");
        let filtro = filtros();
       
        let respuesta = await CargarDatos(Index.urlVer, JSON.stringify(filtro));
        var resultado = JSON.parse(respuesta);
        ArmaTabla(resultado.lista1);

    });

    function filtros() {
        let fil = {};
        fil['id'] = 53;
        fil['nombre'] = "Cristian";
        fil['estado'] = "muy bien ";
        return fil;
    }

    async function CargarDatos(urlPath, id) {

        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id: id }

            });
            debugger;
            return response;


        } catch (e) {
            console.log(e);
        }


    }


    async function ArmaTabla(reporte) {
        //alert("estoy aqui");
        debugger;
        let texto = ' <table id="tblSolicitudes" class="table table-striped table-bordered">';
        let encabezado = "<thead><tr><th>Fecha </th><th>FechaAnterior </th><th>Observacion </th><th>dias </th><th>OrigenUserName </th><th>OrigenNombreCargo </th><th>OrigenNombreSector </th><th>OrigenUserRole </th><th>DestinoUserName </th><th>DestinoNombreCargo </th><th>DestinoNombreSector </th><th>DestinoUserRole </th><th>NroSolicitud </th><th>IniciativaVigenteId </th><th>IniciativaVigente </th><th>NombreCompra </th><th>MontoUTM </th><th>CDPNum </th><th>ContraparteTecnicaId </th><th>FechaCreacion </th><th>OrdenCompra </th><td>FaseCDP </th><td>MontoCLP </th> </tr></thead><tbody>";
        texto = texto + encabezado;
        let total = reporte.length;

        var i = 0;
        if (total == null)
            return;

        for (let item in reporte) {
            texto += "<tr><td>Fecha </td><td>FechaAnterior </td><td>Observacion </td><td>dias </td><td>OrigenUserName </td><td>OrigenNombreCargo </td><td>OrigenNombreSector </td><td>OrigenUserRole </td><td>DestinoUserName </td><td>DestinoNombreCargo </td><td>DestinoNombreSector </td><td>DestinoUserRole </td><td>NroSolicitud </td><td>IniciativaVigenteId </td><td>IniciativaVigente </td><td>NombreCompra </td><td>MontoUTM </td><td>CDPNum </td><td>ContraparteTecnicaId </td><td>FechaCreacion </td><td>OrdenCompra </td><td>FaseCDP </td><td>MontoCLP </td> </tr>";
        }
        
        //tabla.html(textoT1.replaceAll("BtnUsuario", "BtnUsuarioOrigen"));
        await $("#divtabla").html(texto +"<tbody></table>");
        $("#tblSolicitudes").DataTable();

    }


}();