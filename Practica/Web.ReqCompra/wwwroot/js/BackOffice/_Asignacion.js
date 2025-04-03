Asignacion = function () {

    let tblUsuarios = $("#tblUsuarios");
    let tblUsuariosDos = $("#tblUsuariosDos");
    let tblAsignar = $("#tblAsignar");
    let divSolicitudesAsignacion = $("#divSolicitudesAsignacion");
    let IdUsuario = "";
    let divAnalistasAAsignar = $("#divAnalistasAAsignar");
    
    let classOrigen = $(".BtnUsuarioOrigen");
    let classDestino = $(".BtnUsuariodestino");


    $(document).ready(async function () {
        let usuario = await CargarDatos(Index.urlGetUsuariosCompra)
        Usuarios(usuario);

        //$('[data-toggle="tooltip"]').tooltip(); 
        //divSolicitudesAsignacion.css('visibility', 'hidden');

    });

    $(document).on('click', ".BtnUsuarioOrigen", async function () {
        //debugger;
        let seleccionadosTodos = false;
        let seleccionados = false;
        let usuario = $(this).text();
        let id = $(this).closest('td').attr('IdUsuario');
        
        let solicitudes = await CargarDatos(Index.urlGetSolicitudesCompra,id)

        pintarMatriz(solicitudes, usuario);

        $('#divSolicitudesAsignacion').show();
        divSolicitudesAsignacion.show();
        divAnalistasAAsignar.hide();


    });


    $(document).on('click', ".BtnUsuarioDestino", async function () {
        //debugger;
        let seleccionadosTodos = false;
        let seleccionados = false;
        let usuario = $(this).text();
        IdUsuario = $(this).closest('td').attr('IdUsuario');
        let error = 0;

        let confirmResult = await Swal.fire({

            title: 'Asignación de solicitudes',
            text: "Esta seguro de asignar estas solicitudes a: " + usuario,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Si, Asignar',
            cancelButtonText: 'No',
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
        });

        if (!confirmResult.value) {
            await Swal.fire('Ok! no se asigno la solicitud', '', 'info');
            return;
        }

        error = guardaValores();

        await divAnalistasAAsignar.hide();
        
        
        if (error > 0) {
            Swal.fire('Error', 'Las solicitudes no fueron asignadas', 'error');
        }
        else {
            await Swal.fire('Asignados', 'Las solicitudes fueron asignadas', 'success');
            //divSolicitudesAsignacion.hide();
            let solicitudes = await CargarDatos(Index.urlGetSolicitudesCompra, 0)

            pintarMatriz(solicitudes, 'Sin asignar');


        }

    });

 

     async function guardaValores() {
        debugger;
        let error = 0;
         await $(".BtnRol").each(async function (i) {
            if ($(this).hasClass('active')) {
                //alert(this);
                seleccionados = true;
                let sol = {}
                sol['id'] = $(this).closest('td').attr('IdSolicitud');
                sol['analistaProcesoId'] = IdUsuario;
                let respuesta = await guardarTransaccion(Index.urlPostAsignarSolicitudesCompra, sol);
                
                if (respuesta == -1)
                    error += 1;
            }
         });


         return error;
    }

    $(document).on('mouseover', ".BtnRol", function () {
        let fila = $(this).closest('td');
        let ojo = fila.find('.p2');
        $(".p2").css("display", "none");
        ojo.css('display', 'block');
            
    })

    $(document).on('mouseout', ".BtnRol", function () {
        let fila = $(this).closest('td');
        let ojo = fila.find('.p2');
        setTimeout(function () {
            ojo.css('display', 'none');
        }, 10000);
        
        
    })
       


    $(document).on('click', ".BtnRol", function () {
        var marca = true;
        //debugger;
        $(this).toggleClass("active");

        $(".BtnRol:not(.active)").each(function (i, item) {
            marca = false;
            return false;
        });


        let cont = 0;
        $(".BtnRol").each(function (i) {
            if ($(this).hasClass('active')) {
                cont += 1;
                return false;
            }
        });


        if (cont==0) { divAnalistasAAsignar.hide(); } else { divAnalistasAAsignar.show();}

        //divAnalistasAAsignar.show();
        $("#chkMarcarTodo").prop('checked', marca);
    });


    

    function Usuarios(usuario) {
        //debugger;
        let texto = "";
        let textoT1 = "<tr> <td align='center' IdUsuario='0'><button type='button' class='btn btn-primary BtnUsuario' style='width:90%; padding: 0px 0px 0px 0px; margin: 3px 3px 3px 3px; font-size: 15px;'> Sin Asignar </button></td></tr>";
        let total = usuario.length;

        var i = 0;
        if (total == null)
            return;
        
        for (let item in usuario) {
            texto += "<tr> <td align='center' IdUsuario='" + usuario[item].id + "'><button type='button' class='btn btn-outline-primary BtnUsuario' style='width:90%; padding: 0px 0px 0px 0px; margin: 3px 3px 3px 3px; font-size: 15px;'>" + usuario[item].userName + "</button></td></tr>"
                
        }
        textoT1 += texto;

        tblUsuarios.html(textoT1.replaceAll("BtnUsuario", "BtnUsuarioOrigen"));
        tblUsuariosDos.html(texto.replaceAll("BtnUsuario", "BtnUsuarioDestino"));

     
        
     

  
        
    }

    $("#BtnAceptarMensaje").click(function () {
        //debugger;
   
        //Llamada ajax 




    });

    function pintarMatriz(matriz, usuario) {
        debugger;
        let total = matriz.length;
        let h2 = $("#HId3");
        h2.html(usuario + ': ' + total + ' solicitudes');
        let iconNew = '<a class="p3" href="#" title="Nuevo"><span style="font-size: 2.1em; "><i class="fas fa-award"></i></span> </a>';
        var texto = "<tr>";
        var i = 0;

        while (i <= total - 1) {
            //debugger;
            for (var j = 0; j <= 7; j++) {
                debugger;
                let anyo = + matriz[i].nroSolicitud.substring(2, 5);
                let num = + matriz[i].nroSolicitud.substring(6);
                let numAn = anyo + "-" + num;

                texto += "<td align='center' IdSolicitud='" + matriz[i].id + "'><div><label class='btn btn-outline-success BtnRol' style='margin: 5px; width:65px; padding: 5px 5px;' data-toggle='tooltip' data-placement='top'  title='" + matriz[i].nombreCompra + " \n " + matriz[i].solicitante.fullName + "'>" + numAn + '</label></div><a class="p2" href="../Solicitud/Ver/' + matriz[i].id + '"  target="_blank"  data-toggle="tooltip" title="Ver"><span style="font-size: 1.5em; "><i class="fas fa-eye"></i></span> </a> ' + iconNew +' </td>';

                if (i == total - 1) {
                    $("#tblAsignar").html(texto);
                    return;
                }

                i++;
            }
            texto += "</tr><tr>";

        }

        tblAsignar.html(texto);
        
        //$(document).ready(function(){
                      
        //    });
    }

   



    async function CargarDatos(urlPath,id) {

        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id:id }
                
            });
            debugger;
            return response;


        } catch (e) {
            console.log(e);
        }


    }


    async function ObtenerDatos(elemento, urlPath, id) {

        let response;

        try {

            response = await $.get({
                url: urlPath,
                data: { id: id }
            });
            elemento.html(response);
            //

            ////$(document).ready(function () {
            //if (!$.fn.DataTable.isDataTable(solicitudesId)) {
            //    $(solicitudesId).DataTable();
            //}



        } catch (e) {
            console.log(e);
        }


    }





}();