// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$.extend(true, $.fn.dataTable.defaults, {
    "order": [0, 'desc'],
    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
    //stateSave: true,
    language: {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        },
        searchPlaceholder: "Buscar",
        search: ""
        //"order": order
    },
    //"lengthChange": false,
    dom: "<'tableFiltro row'<'col-4'l><'col-3'B><'col-5'f>><t><'row'<'col-sm-12 col-md-3'l><'col-sm-12 col-md-5'i><'col-sm-12 col-md-4'p>>", // posicion de los elementos "https://datatables.net/reference/option/dom#:~:targetText=DataTables%20will%20add%20a%20number,CSS%20applied%20to%20the%20elements."
    buttons: [
        {
            //'copy',
            extend: 'csvHtml5',
            className: 'btn btn-outline-primary',
            titleAttr: 'Archivo CSV',
            text: '<i class= "fas fa-file-csv" ></i>', 
            init: function (api, node, config) {
                $(node).removeClass('btn-secondary')
            }
            
        },
        {
            extend: 'excelHtml5',
            text: '<i class= "fas fa-file-excel" ></i>',
            className: "btn btn-outline-primary",
            titleAttr: 'Archivo Excel',
            init: function (api, node, config) {
                $(node).removeClass('btn-secondary')
            }
                   
        },
        {
            text: "<i class='fas fa-print'></i>",
            extend: 'print',
            titleAttr: 'Imprimir Tabla',
            className: "btn btn-outline-primary",
            init: function (api, node, config) {
                $(node).removeClass('btn-secondary')
            }
        },
       
    ],
    "columnDefs": [{
        "width": "20rem", "targets": 2 
    }]
});

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


var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})

var idleTime = 0;
var conf = null
$(document).ready(async function () {
    $('[data-toggle="tooltip"]').tooltip()
    
    conf = await $.get({
        url: Layout.urlGetConf
    });

    
    var idleInterval = undefined; //setInterval(timerIncrement, 60000); // 1 minute

    // Incrementa el tiempo inactivo cada 1 minuto
    var startInt = function () {
        idleInterval = setInterval(timerIncrement, 60000);
    }

    // Zero the idle timer on mouse movement.
    $(this).mousemove(function (e) {
        clearInterval(idleInterval)
        startInt()
        idleTime = 0;
    });
    $(this).keypress(function (e) {
        clearInterval(idleInterval);
        startInt();
        idleTime = 0;
    });

});

function timerIncrement() {
    idleTime = idleTime + 1;
    if (idleTime >= conf.tiemposesion) {
        window.location.href = conf.sitio + "/Login";
    }
}


/**
 * 
 * @param {any} elementoSelect
 * @param {any} urlPath
 */
async function CargarSelectNoEditable(elementoSelect, urlPath) {
    //debugger;
    let response;
    //debugger;



    try {
        elementoSelect.attr("disabled", "disabled");
        //let tool = "#" + elementoSelect.attr('id') + "tool";
        let valor = elementoSelect.data("valor");

        //$("#UnidadMedidaInId option[value!='']").remove()
        elementoSelect.children("option[value != '']").remove();

        response = await $.get({
            url: urlPath
        });

        $.each(response, function (i, item) {
            elementoSelect.append(new Option(item.nombre, item.id));
            if (item.id == valor) {
                elementoSelect.prop("title", item.nombre);
            }
        });


        if (valor !== undefined) {
            elementoSelect.children("option[value='" + valor + "']").attr("selected", "selected");

        }


        elementoSelect.removeAttr("disabled");
        return response;

    } catch (e) {
        console.log(e);
    }

}

/**
 * Usado en el formulario de solicitud
 * @param {any} elementoSelect
 * @param {any} urlPath
 * @param {any} esAprobacion
 */
async function CargarSelect(elementoSelect, urlPath, esAprobacion = false) {
    //debugger;
    let response;
    //debugger;
    

    
    try {
        elementoSelect.attr("disabled", "disabled");
        //let tool = "#" + elementoSelect.attr('id') + "tool";
        let valor = elementoSelect.data("valor");

        //$("#UnidadMedidaInId option[value!='']").remove()
        elementoSelect.children("option[value != '']").remove();

        response = await $.get({
            url: urlPath
        });
        
        $.each(response, function (i, item) {
            elementoSelect.append(new Option(item.nombre, item.id));
            if (item.id == valor) {
            //    $(tool).html(item.nombre);
                elementoSelect.prop("title", item.nombre);
            }
        });

       
        if (valor !== undefined) {
            elementoSelect.children("option[value='" + valor + "']").attr("selected", "selected");
            //debugger;
            

           
            

        }

        if (!esAprobacion) {
            elementoSelect.removeAttr("disabled");
            elementoSelect.editableSelect();
        } else {
            
            // aqui estoy aprobando
            
        }
            
        
        return response;

    } catch (e) {
        console.log(e);
    }

}

/**
 * Usado en filtros de las tablas
 * @param {any} elementoSelect
 * @param {any} urlPath
 * @param {any} esEditable
 */
async function CargarSelectEditable(elementoSelect, urlPath, esEditable) {

    let response;

    try {
        elementoSelect.attr("disabled", "disabled");

        //$("#UnidadMedidaInId option[value!='']").remove()
        elementoSelect.children("option[value != '']").remove();

        response = await $.get({
            url: urlPath
        });

        elementoSelect.append(new Option("Seleccionar", 0));

        $.each(response, function (i, item) {
            elementoSelect.append(new Option(item.nombre, item.id));
        });

        let valor = elementoSelect.data("valor");

        if (valor !== undefined) {
            elementoSelect.children("option[value='" + valor + "']").attr("selected", "selected");
            

        }

        elementoSelect.removeAttr("disabled");

        if (esEditable) {
            elementoSelect.editableSelect();
        }

        return response;

    } catch (e) {
        console.log(e);
    }

}

/**
 * Usado en el formulario de solicitud
 * @param {any} elementoSelect
 * @param {any} urlPath
 * @param {any} esAprobacion
 */
async function CargarEditableSelect(elementoSelect, urlPath, esAprobacion = false) {
    //debugger;
    let response;

    try {
     /*   elementoSelect.attr("disabled", "disabled");*/

        //$("#UnidadMedidaInId option[value!='']").remove()
        $("#txtProgramaPresupuestario").editableSelect('clear');

        response = await $.get({
            url: urlPath
        });

        $.each(response, function (i, item) {
            // Agrega una opción al cuadro desplegable
            // Insertar contenido
            var text = item.nombre;
            // Insertar posición
            //var index = 10;
            //elementoSelect.editableSelect("add", text, i, [{ name: 'value', value: item.id }]);
            //debugger;
            $("#txtProgramaPresupuestario").editableSelect("add", text, i, [{ name: 'value', value: item.id  }]);

            //elementoSelect.append(new Option(item.nombre, item.id));
        });

        let valor = elementoSelect.data("valor");

        if (valor !== undefined) {
            elementoSelect.children("option[value='" + valor + "']").attr("selected", "selected");
        }

        if (!esAprobacion) {
          /*  elementoSelect.removeAttr("disabled");*/
            elementoSelect.editableSelect();
        } else {
            // aqui estoy aprobando

        }


        return response;

    } catch (e) {
        console.log(e);
    }

}

async function CargarDatosMantenedores(urlPath, tipo, id) {

    let response;

    try {

        response = await $.post({
            url: urlPath,
            data: { ids: id, tipo: tipo }
        });
        debugger;
        return response;


    } catch (e) {
        console.log(e);
    }


}

async function guardarTransaccion(urlPath, datosEntidad) {
    let response;
    try {

        response = await $.post({
            url: urlPath,
            data:  datosEntidad 
        });


        return response;

    } catch (e) {
        console.log(e);
        throw new Error("Error al guardar transaccion, mensaje: " + e.mensaje);
    }
}

/**
 * Recibe: 1.111.111 - 1.111.111,11 
 * Devuelve:  111111 - 1111111.11
 * @param {any} valorStr
 */
function DesFormatearMonto(valorStr) {


    let desformateado = valorStr.toString().replace(/\./g, "");
    desformateado = desformateado.replace(",", ".");

    if (!$.isNumeric(desformateado))
        return 0;

    return desformateado//;.replace(".", ",");
}

function FormateaMonto(nStr, decimales) {
    //debugger;
    if (nStr == undefined)
        return 0;

    nStr = nStr.toString().replace(",", ".");

    if (!$.isNumeric(nStr))
        return 0;

    nStr = parseFloat(nStr).toFixed(decimales);
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return (x1 + x2);
}

function ValidarForm(form){
    //let form = frmAprobar;
    let estaOk = false;

    if (form[0].checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        estaOk = true;
    }

    form[0].classList.add('was-validated');

    return estaOk;
}

function DatosForm(form) {

    let data = {};

    let camposDisabled = form.find('[disabled]');
    camposDisabled.prop('disabled', false)

    data = form.serializeArray().reduce(function (obj, item) {

        let elemento = $("input[name='" + item.name + "']");
        let valor = elemento.hasClass("es-input") ? elemento.parent().find("ul.es-list").find(".es-visible").val() : item.value;

        if (elemento.data("type") == "monto") {
            valor = DesFormatearMonto(valor).toString().replace(".", ",");
        }

        obj[item.name] = valor;

        return obj;
    }, {});

    camposDisabled.prop('disabled', true);

    return data;

}

; (function ($) {

    $.fn.cargando = function () {
        $(this).attr("disabled", "disabled");
        $(this).data("texto-original", $(this).html());
        $(this).html(
            `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> ${$(this).html()}`
        );
    };

    $.fn.reiniciarCarga = function () {
        $(this).removeAttr("disabled");
        $(this).html($(this).data("texto-original"));
    };
})(jQuery);