var valueURl;

function EliminarSucursal(partitionKey, rowKey) {
    swal({
        title: "¿Está seguro que desea eliminar la sucursal?",
        icon: "warning",
        buttons: ["Cancelar", "Eliminar"],
        dangerMode: true
    }).then(function (willDelete) {
        if (willDelete) {
            EliminarSucursalAJAX("post", "/Sucursales/Eliminar", partitionKey, rowKey);
        } else {
            swal("Cancelado", "La acción eliminar ha sido detenida.", "error");
        }
    });




}

function EliminarSucursalAJAX(method, uri, partitionKey, rowKey) {

    $.ajax({
        method: method,
        url: uri,
        async: true,
        cache: false,
        data: { "pLlaveParticion": partitionKey, "pLlaveFila": rowKey },
        success: function (data) {
            if (data.isCompleted === true) {
                swal({
                    title: '\u00C9xito',
                    text: "El registro ha sido eliminado exitosamente.",
                    icon: "success",
                    type: "success",
                    html: true
                }).then(function () {
                    window.location.reload();
                });
            }
            else {
                swal({
                    title: "Error",
                    text: "A ocurrido un error.",
                    icon: "error",
                    type: "error"
                }).then(function () {
                    window.location.reload();
                });
            }
        }
    });




}

function CargarFormulario(partitionKey, rowKey) {

    $.ajax({
        url: "/Sucursales/Formulario",
        async: true,
        cache: false,
        data: { "pLlaveParticion": partitionKey, "pLlaveFila": rowKey },
        success: function (response) {

            var data = response;

            if (data !== null) {

                $("#modal-title").text("Actualizar Sucursales");
                $('#divImgFoto').show();
                $('#divImgMaps').show();
                $('#divImgWaze').show();

                //Limpiar las cajas de texto de los mensajes
                $('#msgNombre').text('');
                $('#msgImagenSucursal').text('');
                $('#msgTxtMaps').text('');
                $('#msgImgMaps').text('');
                $('#msgImgWaze').text('');
                $('#msgtxtWaze').text('');
                $('#msgTelTienda').text('');
                $('#msgTelRestaurante').text('');
                $('#msgHorarioTienda').text('');
                $('#msgHorarioRestaurante').text('');
                $('#RowKey').text('');
                $('#PartitionKey').text('');

                //asignar los valores a cada control 
                $("#txtNombre").val(data.nombre);
            
                $("#txtMaps").val(data.enlaceGoogleMaps);
                $("#txtWaze").val(data.enlaceWaze);
                $('#txtTelTienda').val(data.telefonoTienda);
                $('#txtTelRestaurante').val(data.telefonoRestaurante);
                $('#txtHorarioTienda').val(data.horarioTienda);
                $('#txtHorarioTienda').removeAttr("disabled");
                $('#txtHorarioRestaurante').val(data.horarioRestaurante);
                $('#txtHorarioRestaurante').removeAttr("disabled");

                $('#imgSalidaFoto').attr("src", data.fotoUrl);
                $('#imgSalidaGoogle').attr("src", data.iconoGoogleMaps);
                $('#imgSalidaWaze').attr("src", data.iconoWaze);

                $("#pAccion").val("Actualizar");
                $('#RowKey').val(rowKey);
                $('#PartitionKey').val(partitionKey);
                $('#target').show();
                //$('#divImg').hide();
                //$('#divFilter').show();
                $("#btn_Registrar").html("Guardar");
            }
            else {

                $("#modal-title").text("Registrar Sucursal");
                //Limpiar las cajas de texto de los mensajes
                $('#msgNombre').text('');
                $('#msgImagenSucursal').text('');
                $('#msgTxtMaps').text('');
                $('#msgImgMaps').text('');
                $('#msgImgWaze').text('');
                $('#msgtxtWaze').text('');
                $('#msgTelTienda').text('');
                $('#msgTelRestaurante').text('');
                $('#msgHorarioTienda').text('');
                $('#msgHorarioRestaurante').text('');
              
                $("#txtNombre").val("");      
                $("#urlWaze").val("");
                $("#imgMaps").val("");
                $("#imgMaps").val("");
                $("#txtWaze").val("");
                $('#txtTelTienda').val("");
                $('#txtTelRestaurante').val("");
                $('#txtHorarioTienda').val("");
                $('#txtHorarioRestaurante').val("");
       
                $("#pAccion").val("Agregar");
                $('#RowKey').text('');
                $('#PartitionKey').text('');
                //$('#divImagen').hide();
                $('#imgSalidaFoto').attr("src", "");
                $('#imgSalidaGoogle').attr("src", "");
                $('#imgSalidaWaze').attr("src", "");
                $('#target').hide();
                $('#divImgFoto').show();
                $('#divImgMaps').show();
                $('#divImgWaze').show();

                $('#divFilter').hide();
            }

        }
    });

}

$("#btn_Registrar").click(function () {

    //funcion que quita espacios en blanco en los controles
    QuitarEspaciosBlanco();

    var pAccionPrev = $("[name='pAccion']").val();

    var _existe = ExisteSucursal();

    if (pAccionPrev === "Actualizar") {
        _existe = false;
    }

    if (_existe === true) {

        $('#msgNombre').text('Ya existe una sucursal con este nombre');
        $('#msgTxtMaps').text('Ya existe una sucursal con esta URL de Google Maps');
        $('#msgtxtWaze').text('Ya existe una sucursal con esta URL de Waze');
        $('#msgTelTienda').text('Ya existe una sucursal con este teléfono de tienda');
        $('#msgTelRestaurante').text('Ya existe una sucursal con este teléfono de restaurante');
        $('#msgHorarioTienda').text('Ya existe una sucursal con este horario de tienda');
        $('#msgHorarioRestaurante').text('Ya existe una sucursal con este horario de restaurante');
    }
    else {
        //Limpia los mensajes
        $('#msgNombre').text('');
        $('#msgImagenSucursal').text('');
        $('#msgTxtMaps').text('');
        $('#msgImgMaps').text('');
        $('#msgImgWaze').text('');
        $('#msgtxtWaze').text('');
        $('#msgTelTienda').text('');
        $('#msgTelRestaurante').text('');
        $('#msgHorarioTienda').text('');
        $('#msgHorarioRestaurante').text('');

        //valores de los controles
        Nombre = $('#txtNombre').val();
        UrlImagenSucursal = $('#imagenSucursal').val();
        direccionMaps = $('#txtMaps').val();
        ImageMaps = $('#imgMaps').val();
        direccionWaze = $('#txtWaze').val();
        ImageWaze = $('#imgWaze').val();
        TelTienda = $('#txtTelTienda').val();
        TelRestaurante = $('#txtTelRestaurante').val();
        HorarioTienda = $('#txtHorarioTienda').val();
        HorarioRestaurante = $('#txtHorarioRestaurante').val();

        NombreL = $.trim(Nombre);
        UrlImagenSucursalL = $.trim(UrlImagenSucursal);
        direccionMapsL = $.trim(direccionMaps);
        ImageMapsL = $.trim(ImageMaps);
        direccionWazeL = $.trim(direccionWaze);
        ImageWazeL = $.trim(ImageWaze);

        var pAccion = $("[name='pAccion']").val();

        //Validaciones campos
        if (Nombre === '') {
            $('#msgNombre').text('Dato Requerido');
        }
        if (direccionMapsL === '') {
            $('#msgTxtMaps').text('Dato Requerido');
        }
        if (direccionWazeL === '') {
            $('#msgtxtWaze').text('Dato Requerido');
        }
        //if (TelTienda === '') {
        //    $('#msgTelTienda').text('Dato Requerido');
        //}
        //if (TelRestaurante === '') {
        //    $('#msgTelRestaurante').text('Dato Requerido');
        //}
        if (UrlImagenSucursalL === '' && pAccion === 'Agregar') {
            //enviar la url 
            $('#msgImagenSucursal').text('Dato Requerido');
        }
        if (ImageWazeL === '' && pAccion === 'Agregar') {
            //enviar la url 
            $('#msgImgWaze').text('Dato Requerido');
        }
        if (ImageMapsL === '' && pAccion === 'Agregar') {
            //enviar la url 
            $('#msgImgMaps').text('Dato Requerido');
        }
        //if (HorarioTienda === '') {
        //    $('#msgHorarioTienda').text('Dato Requerido');
        //}
        //if (HorarioRestaurante === '') {
        //    $('#msgHorarioRestaurante').text('Dato Requerido');
        //}

        //if (TelTienda === '') {
        //    $('#msgTelTienda').text('Dato Requerido');
        //} else {
        //    if (TelTienda.length < 8) {
        //        $('#msgTelTienda').text('El teléfono de la tienda no puede tener menos de 8 dígitos');
        //    }
        //}

        //if (TelRestaurante === '') {
        //    $('#msgTelRestaurante').text('Dato Requerido');
        //} else {
        //    if (TelRestaurante.length < 8) {
        //        $('#msgTelRestaurante').text('El teléfono del restaurante no puede tener menos de 8 dígitos');
        //    }
        //}

        var urlMaps = isUrlValidMaps();
        var urlWaze = isUrlValidWaze();
        var PartitionKey = '';
        var RowKey = '';
        var DatosSucursal = new FormData();

        if (pAccion === "Agregar") {
            //if (NombreL !== '' && UrlImagenSucursalL !== '' && direccionMapsL !== '' && ImageMapsL !== '' && direccionWazeL !== '' && ImageWazeL !== '' && urlMaps !== 0 && urlWaze !== 0 && TelTienda !== '' && TelRestaurante !== '' && HorarioTienda !== '' && HorarioRestaurante !== '' && TelTienda.length === 8 && TelRestaurante.length === 8) {

            if (NombreL !== '' && UrlImagenSucursalL !== '' && direccionMapsL !== '' && ImageMapsL !== '' && direccionWazeL !== '' && ImageWazeL !== '' && urlMaps !== 0 && urlWaze !== 0 ) {

                if (pAccion === "Actualizar") {
                    PartitionKey = $('#PartitionKey').val();
                    RowKey = $('#RowKey').val();
                }


                //DatosSucursal.append('centro', Centro);
                DatosSucursal.append('nombre', Nombre);
                if ($('#imagenSucursal')[0].files[0] !== '' && $('#imagenSucursal')[0].files[0] !== undefined && $('#imagenSucursal')[0].files[0] !== null) {
                    DatosSucursal.append('foto', $('#imagenSucursal')[0].files[0]);
                }
                else {
                    DatosSucursal.append('FotoUrl', $('#imagenSucursal').attr("src"));
                }
                DatosSucursal.append('direccionGoogleMaps', direccionMaps);
                if ($('#imgMaps')[0].files[0] !== '' && $('#imgMaps')[0].files[0] !== undefined && $('#imgMaps')[0].files[0] !== null) {
                    DatosSucursal.append('iconoGoogleMaps', $('#imgMaps')[0].files[0]);
                }
                DatosSucursal.append('direccionWaze', direccionWaze);
                if ($('#imgWaze')[0].files[0] !== '' && $('#imgWaze')[0].files[0] !== undefined && $('#imgWaze')[0].files[0] !== null) {
                    DatosSucursal.append('iconoWaze', $('#imgWaze')[0].files[0]);
                }
                DatosSucursal.append('direccionWaze', direccionWaze);

                DatosSucursal.append('accion', pAccion);
                DatosSucursal.append('PartitionKey', PartitionKey);
                DatosSucursal.append('RowKey', RowKey);

                DatosSucursal.append('horarioTienda', HorarioTienda);
                DatosSucursal.append('horarioRestaurante', HorarioRestaurante);
                DatosSucursal.append('telefonoTienda', TelTienda);
                DatosSucursal.append('telefonoRestaurante', TelRestaurante);

                $.ajax({
                    type: "POST",
                    url: "/Sucursales/EjecutarAcciones",
                    data: DatosSucursal,
                    //async: false,
                    contentType: false, // Not to set any content header
                    processData: false,
                    beforeSend: function () {
                        $('#btn_Registrar').attr('disabled', true);

                        // add spinner to button
                        $('#btn_Registrar').html(
                            '<span class="spinner-border spinner-border-sm" ></span> Procesando...'
                        );
                    },
                    complete: function () {
                        $('#btn_Registrar').attr('disabled', false);
                    },
                    success: function (data) {
                        if (data === true) {
                            $("#modal-register").modal('hide');

                            swal({
                                title: '\u00C9xito',
                                text: "El registro ha sido agregado exitosamente.",
                                icon: "success",
                                type: "success",
                                html: true
                            }).then(function () {
                                window.location.reload();
                            });
                        }

                    },
                    error: function (result) {
                        swal("Error", "Ocurrió un error al registrar la sucursal.", "error");
                    }
                });
            }
        }
        else {
            if (NombreL !== '' && direccionMapsL !== '' && direccionWazeL !== '' && urlMaps !== 0 && urlWaze !== 0 ) {

                if (pAccion === "Actualizar") {
                    PartitionKey = $('#PartitionKey').val();
                    RowKey = $('#RowKey').val();
                }

                DatosSucursal.append('nombre', Nombre);

                if ($('#imagenSucursal')[0].files[0] !== '' && $('#imagenSucursal')[0].files[0] !== undefined && $('#imagenSucursal')[0].files[0] !== null) {
                    DatosSucursal.append('foto', $('#imagenSucursal')[0].files[0]);
                }
                else {
                    DatosSucursal.append('imagenPath', $('#imgSalidaFoto').attr("src"));
                }

                DatosSucursal.append('direccionGoogleMaps', direccionMaps);

                if ($('#imgMaps')[0].files[0] !== '' && $('#imgMaps')[0].files[0] !== undefined && $('#imgMaps')[0].files[0] !== null) {
                    DatosSucursal.append('iconoGoogleMaps', $('#imgMaps')[0].files[0]);
                }
                else {
                    DatosSucursal.append('imagenPathMaps', $('#imgSalidaGoogle').attr("src"));
                }

                DatosSucursal.append('direccionWaze', direccionWaze);

                if ($('#imgWaze')[0].files[0] !== '' && $('#imgWaze')[0].files[0] !== undefined && $('#imgWaze')[0].files[0] !== null) {
                    DatosSucursal.append('iconoWaze', $('#imgWaze')[0].files[0]);
                }
                else {
                    DatosSucursal.append('imagenPathWaze', $('#imgSalidaWaze').attr("src"));
                }


                DatosSucursal.append('direccionWaze', direccionWaze);

                DatosSucursal.append('accion', pAccion);
                DatosSucursal.append('PartitionKey', PartitionKey);
                DatosSucursal.append('RowKey', RowKey);

                DatosSucursal.append('horarioTienda', HorarioTienda);
                DatosSucursal.append('horarioRestaurante', HorarioRestaurante);
                DatosSucursal.append('telefonoTienda', TelTienda);
                DatosSucursal.append('telefonoRestaurante', TelRestaurante);

                $.ajax({
                    type: "POST",
                    url: "/Sucursales/EjecutarAcciones",
                    data: DatosSucursal,
                    //async: false,
                    contentType: false, // Not to set any content header
                    processData: false,
                    beforeSend: function () {
                        $('#btn_Registrar').attr('disabled', true);

                        // add spinner to button
                        $('#btn_Registrar').html(
                            '<span class="spinner-border spinner-border-sm" ></span> Procesando...'
                        );
                    },
                    complete: function () {
                        $('#btn_Registrar').attr('disabled', false);
                    },
                    success: function (data) {
                        if (data === true) {
                            $("#modal-register").modal('hide');

                            swal({
                                title: '\u00C9xito',
                                text: "El registro ha sido actualizado exitosamente.",
                                icon: "success",
                                type: "success",
                                html: true
                            }).then(function () {
                                window.location.reload();
                            });
                        }

                    },
                    error: function (result) {
                        swal("Error", "Ocurrió un error al registrar la sucursal.", "error");
                    }
                });
            }

        }
    }

});

function QuitarEspaciosBlanco() {
    strNombre = $('#txtNombre').val();
    strDireccionMaps = $('#txtMaps').val();
    strDireccionWaze = $('#txtWaze').val();
    strTelTienda = $('#txtTelTienda').val();
    strTelRestaurante = $('#txtTelRestaurante').val();
    strHorarioTienda = $('#txtHorarioTienda').val();
    strHorarioRestaurante = $('#txtHorarioRestaurante').val();

    var _Nombre = "";
    var _DirGM = "";
    var _DirWZ = "";
    var _TelT = "";
    var _TelR = "";
    var _HoraT = "";
    var _HoraR = "";

    //quitar los espacios en blanco de mas en los controles
    _Nombre = strNombre.replace(/\s+/g, " ").trim();
    _DirGM = strDireccionMaps.replace(/\s+/g, " ").trim();
    _DirWZ = strDireccionWaze.replace(/\s+/g, " ").trim();
    _TelT = strTelTienda.replace(/\s+/g, " ").trim();
    _TelR = strTelRestaurante.replace(/\s+/g, " ").trim();
    _HoraT = strHorarioTienda.replace(/\s+/g, " ").trim();
    _HoraR = strHorarioRestaurante.replace(/\s+/g, " ").trim();

    //se actualizan los campos
    $('#txtNombre').val(_Nombre);
    $('#txtMaps').val(_DirGM);
    $('#txtWaze').val(_DirWZ);
    $('#txtTelTienda').val(_TelT);
    $('#txtTelRestaurante').val(_TelR);
    $('#txtHorarioTienda').val(_HoraT);
    $('#txtHorarioRestaurante').val(_HoraR);
}

function ExisteSucursal() {

    Nombre = $('#txtNombre').val();
    direccionMaps = $('#txtMaps').val();
    direccionWaze = $('#txtWaze').val();
    TelTienda = $('#txtTelTienda').val();
    TelRestaurante = $('#txtTelRestaurante').val();
    HorarioTienda = $('#txtHorarioTienda').val();
    HorarioRestaurante = $('#txtHorarioRestaurante').val();

    var existe;

    $.ajax({
        method: "Post",
        cache: false,
        async: false,
        url: "/Sucursales/ConsultarDatos",
        data: { "nombre": Nombre, "enlaceGM": direccionMaps, "enlaceWZ": direccionWaze, "telTienda": TelTienda, "telRestaurante": TelRestaurante, "horarioTienda": HorarioTienda, "horarioRestaurante": HorarioRestaurante},
        success: function (respuesta) {

            existe = respuesta;
        }
    });

    return existe;
}

$("#btnHorarioTienda").click(function (){
    //valores de los controles
    var HorarioT = $('#txtHorarioTienda').val();
    var pAccion = $("[name='pAccion']").val();

    //Obtener los dias
    var diaT = $('#ddlDiaT option:selected').text();
    var diaHastaT = $('#ddlDiaHastaT option:selected').text();
   
    // Obtener las horas
    var horarioIniT = $('#dateIniT').find('option:selected').text(); 
    var horarioFinT = $('#dateFinT').find('option:selected').text(); 
    var horarioTienda = "";

    //Se arma el formato para el horario
    var horaT = " de: " + horarioIniT;
    var horaHastaT = " a: " + horarioFinT;

    //Asignacion del horario al 
    if (diaHastaT === "Cerrado") {
        horarioTienda = diaT + " " + diaHastaT;

        if (HorarioT === "") {
            $('#txtHorarioTienda').val(horarioTienda);
        }
        else {

            if (HorarioT.includes(horarioTienda) !== true) {

                HorarioT = HorarioT + "," + horarioTienda;

                $('#txtHorarioTienda').val(HorarioT);
            }
        }
    }
    else {
        if (diaHastaT !== "") {
            horarioTienda = diaT + " a " + diaHastaT + horaT + horaHastaT;
        }
        else {
            horarioTienda = diaT + " " + horaT + horaHastaT;

        }

        if (HorarioT === "") {
            $('#txtHorarioTienda').val(horarioTienda);
        }
        else {

            if (HorarioT.includes(horarioTienda) !== true) {
                HorarioT = HorarioT + "," + horarioTienda;

                $('#txtHorarioTienda').val(HorarioT);
            }
        }
    }

    //Default combos
    $('#ddlDiaT').val('Lunes');
    $('#ddlDiaHastaT').val('');
    $('#dateIniT').val(0);
    $('#dateFinT').val(0);
 
});

$("#btnHorarioRestaurante").click(function () {
    //valores de los controles
    var HorarioR = $('#txtHorarioRestaurante').val();
    var pAccion = $("[name='pAccion']").val();

    //Obtener los dias
    var diaR = $('#ddlDiaR option:selected').text();
    var diaHastaR = $('#ddlDiaHastaR option:selected').text();

    // Obtener las horas
    var horarioIniR = $('#dateIniR').find('option:selected').text();
    var horarioFinR = $('#dateFinR').find('option:selected').text();
    var horarioRestaurante = "";

    //Se arma el formato para el horario
    var horaR = " de: " + horarioIniR;
    var horaHastaR = " a: " + horarioFinR;

    //Asignacion del horario al 
    if (diaHastaR === "Cerrado") {
        horarioRestaurante = diaR + " " + diaHastaR;

        if (HorarioR === "") {
            $('#txtHorarioRestaurante').val(horarioRestaurante);
        }
        else {

            if (HorarioR.includes(horarioRestaurante) !== true) {

                HorarioR = HorarioR + "," + horarioRestaurante;

                $('#txtHorarioRestaurante').val(HorarioR);
            }
        }
    }
    else {
        if (diaHastaR !== "") {
            horarioRestaurante = diaR + " a " + diaHastaR + horaR + horaHastaR;
        }
        else {
            horarioRestaurante = diaR + " " + horaR + horaHastaR;

        }

        if (HorarioR === "") {
            $('#txtHorarioRestaurante').val(horarioRestaurante);
        }
        else {

            if (HorarioR.includes(horarioRestaurante) !== true) {
                HorarioR = HorarioR + "," + horarioRestaurante;

                $('#txtHorarioRestaurante').val(HorarioR);
            }
        }
    }

    //Default combos
    $('#ddlDiaR').val('Lunes');
    $('#ddlDiaHastaR').val('');
    $('#dateIniR').val(0);
    $('#dateFinR').val(0);

});

$("#Urlimage").change(function () {
    var a = document.getElementById("Urlimage").files[0].name;
    readURL(this);
});

function readURL(input) {
    valueURl = input.value;
}

$("#imagenSucursal").change(function (e) {

    $('#imagenSucursal').text('');
    $('#msgImagenSucursal').text('');
    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;

    if (fileSize > 1000000) {
        $('#msgImagenSucursal').text('El archivo no debe superar 1MB');
        this.value = '';
      //  this.files[0].name = '';
    } else {
        // recuperamos la extensión del archivo
        var ext = fileName.split('.').pop();

        // console.log(ext);
        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png': break;
            default:
                $('#msgImagenSucursal').text('El archivo no tiene la extensión adecuada');
                this.value = '';
              //  this.files[0].name = '';
        }
    }
});

$("#imgMaps").change(function (e) {
    $('#imgMaps').text('');
    $('#msgImgMaps').text('');
    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;

    if (fileSize > 1000000) {
        $('#msgImgMaps').text('El archivo no debe superar 1MB');
        this.value = '';
       // this.files[0].name = '';
    } else {
        // recuperamos la extensión del archivo
        var ext = fileName.split('.').pop();

        // console.log(ext);
        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png': break;
            default:
                $('#msgImgMaps').text('El archivo no tiene la extensión adecuada');
                this.value = '';
             //   this.files[0].name = '';
        }
    }
});

$("#imgWaze").change(function (e) {
    $('#imgWaze').text('');
    $('#msgImgWaze').text('');
    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;

    if (fileSize > 1000000) {
        $('#msgImgWaze').text('El archivo no debe superar 1MB');
        this.value = '';
        //this.files[0].name = '';
    } else {
        // recuperamos la extensión del archivo
        var ext = fileName.split('.').pop();

        // console.log(ext);
        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png': break;
            default:
                $('#msgImgWaze').text('El archivo no tiene la extensión adecuada');
                this.value = '';
                //this.files[0].name = '';
        }
    }
});

$(document).on('keyup', "input[type='search']", function () {
    var oTable = $('.dataTable').dataTable();
    oTable.fnFilter($(this).val());
});

$("#filter").click(function () {
    $('#divImg').show();
    $('#divFilter').hide();

});

$("#modal-register").on("hide.bs.modal", function () {
    //Limpiar las cajas de texto de los mensajes
    $('#msgNombre').text('');
    $('#msgImagenSucursal').text('');
    $('#msgTxtMaps').text('');
    $('#msgImgMaps').text('');
    $('#msgImgWaze').text('');
    $('#msgtxtWaze').text('');
    $('#msgTelTienda').text('');
    $('#msgTelRestaurante').text('');
    $('#msgHorarioTienda').text('');
    $('#msgHorarioRestaurante').text('');

    $("#txtNombre").val("");
    $("#imagenSucursal").val("");
    $("#imgWaze").val("");
    $("#imgMaps").val("");
    $("#txtMaps").val("");
    $("#txtWaze").val("");
    $('#txtTelTienda').val("");
    $('#txtTelRestaurante').val("");
    $('#txtHorarioTienda').val("");
    $('#txtHorarioRestaurante').val("");

    $("#pAccion").val("Agregar");
    $('#RowKey').text('');
    $('#PartitionKey').text('');
    //$('#divImagen').hide();
    $('#imgSalidaFoto').attr("src", "");
    $('#imgSalidaGoogle').attr("src", "");
    $('#imgSalidaWaze').attr("src", "");
    $('#target').hide();
    $('#divImgFoto').show();
    $('#divImgMaps').show();
    $('#divImgWaze').show();
    $('#divFilter').hide();

});

function mostrarImagen(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imgSalidaFoto').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$(function () {
    $('input[type="text"]').change(function () {
        this.value = $.trim(this.value);
    });
});

function isUrlValidMaps() {
    var url = $('#txtMaps').val();

    var bool = validarEspaciosBlanco(url);

    if (bool === true) {
        url_validate = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
        if (!url_validate.test(url)) {
            $('#msgTxtMaps').text('Url no válida');
            return 0;
        }
        else {
            return 1;
        }
    }
    else {
        $('#msgTxtMaps').text('Url no válida');
    }


}

function isUrlValidWaze() {
    var url = $('#txtWaze').val();

    var bool = validarEspaciosBlanco(url);

    if (bool === true) {
        url_validate = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
        if (!url_validate.test(url)) {
            $('#msgtxtWaze').text('Url no válida');
            return 0;
        }
        else {
            return 1;
        }
    }
    else {
        $('#msgtxtWaze').text('Url no válida');
    }

}

$('#txtWaze').keydown(function (e) {

    if (e.keyCode === 32) { return false; }

}); 

$('#txtMaps').keydown(function (e) {

    if (e.keyCode === 32) { return false; }

}); 

function validarEspaciosBlanco(texto) {
    if (texto.indexOf(" ") !== -1) {
        return false;
    }
    return true;
}



























