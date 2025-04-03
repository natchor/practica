DropZoneConfig = function () {

    let esVer = window.location.pathname.indexOf("/Ver/") > 0;
    let esAprobacion = window.location.pathname.indexOf("/Aprobacion/") > 0;
    let esIngresar = window.location.pathname.indexOf("/Index") > 0;
    let isPreload = false;

    let btnConfirm = $("#btnGuardarArchivo");


    function myParamName() {
        return "files";
    }

    //let hdnRutaArchivo = $("#hdnRutaArchivo");

    Dropzone.autoDiscover = false;

    //Dropzone.options.frmDropZone = {
    var frmDropZone = new Dropzone("#frmDropZone", {
        paramName: myParamName, // The name that will be used to transfer the file
        parallelUploads: 1,
        maxFiles: 30,
        maxFilesize: 100,
        uploadMultiple: true,
        //addRemoveLinks: true,
        //dictRemoveFileConfirmation: "¿Esta seguro de eliminar el archivo?",
        thumbnailWidth: 250,
        thumbnailHeight: 250,
        acceptedFiles: ".jpg,.jpeg,.png,.pdf,.txt,.doc,.docx,.xls,.xlsx,.msg,.zip",
        dictDefaultMessage: "Arrastra los archivos aquí para subirlos",
        dictFallbackMessage: "Su navegador no admite la carga de archivos mediante la función de arrastrar y soltar.",
        dictFallbackText: "Utilice el formulario de respaldo a continuación para cargar sus archivos como en los viejos tiempos.",
        dictFileTooBig: "El archivo es demasiado grande ({{filesize}}MiB). Tamaño maximo: {{maxFilesize}}MiB.",
        dictInvalidFileType: "No puede cargar archivos de este tipo.",
        dictResponseError: "El servidor respondió con el código {{statusCode}}.",
        dictCancelUpload: "Cancelar carga",
        dictCancelUploadConfirmation: "¿Estás seguro de que deseas cancelar esta carga?",
        //dictRemoveFile: "<span class='text-danger'><i class=\"fa fa-trash-alt fa-2x\"></i></span>",
        dictMaxFilesExceeded: "No puede cargar más archivos.",
        accept: function (file, done) {

            //let ext = file.name.split('.').pop();

            //ext = ext.includes("xls") ? "xls" : ext;
            //ext = ext.includes("doc") ? "doc" : ext;

            ////this.createThumbnailFromUrl(file, "../img/icons/icon-" + ext + ".png");f
            //this.emit("thumbnail", file, "../img/icons/icon-" + ext + ".png");

            done();
        },
        init: function () {
            let thisDropzone = this;

            if (esIngresar) {
                return;
            }

            let arrayUrl = window.location.pathname.split("/");
            let solicitudId = arrayUrl[arrayUrl.length - 1];


            //Call the action method to load the images from the server
            $.getJSON("/Solicitud/GetArchivos/" + solicitudId).done(function (data) {
                if (data.data != '') {

                    $.each(data.data, function (index, item) {
                        //// Create the mock file:
                        let mockFile = {
                            name: item.nombre,
                            size: item.size,
                            id: item.id,
                            objetoArchivo: item
                        };

                        isPreload = true;
                        // Call the default addedfile event handler
                        thisDropzone.emit("addedfile", mockFile);

                        // Quita la barra cargadora
                        thisDropzone.emit("complete", mockFile);


                        thisDropzone.files.push(mockFile);
                        isPreload = false;
                        // And optionally show the thumbnail of the file:
                        //thisDropzone.emit("thumbnail", mockFile, item.fullPath);

                        // If you use the maxFiles option, make sure you adjust it to the
                        // correct amount:
                        //var existingFileCount = 1; // The number of files already uploaded
                        //myDropzone.options.maxFiles = myDropzone.options.maxFiles - existingFileCount;
                    });
                }
            });
        }
    });

    //frmDropZone.on("removedfile", function (file) {
    //    $.post(Index.urlEliminarArchivo + "/" + file.id, function () {
    //        frmDropZone.emit("removedfile", file);
    //        //frmDropZone.removeFile(file);
    //        //file.previewElement.parentNode.removeChild(file.previewElement);
    //    });
    //});

    frmDropZone.on("sending", function (file, response, formData) {
        formData["__RequestAntiForgeryToken"] = document.getElementsByName("__RequestVerificationToken").value;
    });

    frmDropZone.on("sendingmultiple", function (file, response, formData) {
        formData["__RequestAntiForgeryToken"] = document.getElementsByName("__RequestVerificationToken").value;
    });

    frmDropZone.on("success", async function (file, response) {



        file.objetoArchivo = response;


        
        //btnConfirm.show();



    });



    frmDropZone.on("addedfile", function (file) {

        

        let btnDescargar = Dropzone.createElement("<a class='text-info' style='margin-left: 2rem;' href='" + Index.urlDescargar + "/" + file.id + "'><i class=\"fa fa-download fa-2x\"></i></a>");
        let btnVer = Dropzone.createElement("<a class='text-info' target='_blank' style='' href='" + Index.urlVer + "/" + file.id + "'><i class=\"fa fa-eye fa-2x\"></i></a>");


        file.previewElement.appendChild(btnDescargar);
        file.previewElement.appendChild(btnVer);

        //if (esVer || esAprobacion) {
        //    return;
        //}

        if (isPreload) {
            return;
        }


        let btnEliminar = Dropzone.createElement("<span class='text-danger'><i class=\"fa fa-trash-alt fa-2x\"></i></span>");
        file.previewElement.appendChild(btnEliminar);

        // Listen to the remove button click event
        btnEliminar.addEventListener("click", async function (e) {

            const result = await Swal.fire({
                title: '¿Seguro de querer eliminar el archivo?',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `Si eliminar`,
                denyButtonText: `No eliminar`,
            });

            //if (window.confirm('¿Seguro de querer eliminar el archivo?')) {
            if (result.isConfirmed) {
                $.post(Index.urlEliminarArchivo + "/" + file.id, { "fullpath": file.objetoArchivo.fullPath }).done(function () {
                    frmDropZone.removeFile(file);

                    Toast.fire({
                        icon: 'success',
                        title: 'Archivo eliminado'
                    });
                }).fail(function () {
                    //frmDropZone.removeFile(file);


                    Toast.fire({
                        icon: 'error',
                        title: 'Error al eliminar archivo'
                    });
                });
            }
        });



    });

    frmDropZone.on("complete", function (file) {
        if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
            //alert("nada");

        } else {
            //alert("tiene algo");
        }


        //let archivosStr = frmDropZone.files.map(f => f.serverPath).join("|");

        //hdnRutaArchivo.val(archivosStr);
    });



    frmDropZone.on("error", function (file, response) {
        var r = response;
        console.log("Drop Err:");
        console.log(r);
    });

    return {
        frmDropZone: frmDropZone
    }

}();
