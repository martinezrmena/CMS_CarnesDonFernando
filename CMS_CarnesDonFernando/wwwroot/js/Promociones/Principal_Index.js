var valueURl;
var _dateToday = new Date(); 

function EliminarPromocion(partitionKey, rowKey) {
    swal({
        title: "¿Está seguro que desea eliminar la promoción?",
        icon: "warning",
        buttons: ["Cancelar", "Eliminar"],
        dangerMode: true
    }).then(function (willDelete) {
        if (willDelete) {
            EliminarPromocionAJAX("post", "/Promociones/Eliminar", partitionKey, rowKey);
        } else {
            swal("Cancelado", "La acción eliminar ha sido detenida.", "error");
        }
    });
    


   
}

function EliminarPromocionAJAX(method, uri, partitionKey, rowKey) {

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

function Eliminar(partitionKey, rowKey) {
    $.ajax({
        type: "POST",
        url: "/Promociones/EjecutarAcciones",
        contentType: false, // Not to set any content header
        processData: false,
        success: function (data) {
            if (data === true) {
        
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
        url: "/Promociones/Formulario",
        async: true,
        cache: false,
        data: { "pLlaveParticion": partitionKey, "pLlaveFila": rowKey },
        success: function (response) {

            var data = response;

            if (data !== null) {

                $("#modal-title").text("Actualizar Promoción");
                $('#divImagen').show();
                //Limpiar las cajas de texto de los mensajes
                $('#msgNombre').text('');
                $('#msgEnlace').text('');
                $('#msgFechaInicio').text('');
                $('#msgFechaFin').text('');
                $('#msgUrlimage').text('');
                $('#RowKey').text('');
                $('#PartitionKey').text('');

                //asignar los valores a cada control 
                $("#txtNombre").val(data.titulo);
                $("#txtEnlace").val(data.enlace);
                $("#txtFechaInicio").val(data.fecha_Publicacion);
                $("#txtFechaFin").val(data.fecha_Finalizacion);
                $('#imgSalida').attr("src", data.imagenUrl);
                var imagen = data.imagenUrl;
                $("#pAccion").val("Actualizar");
                $('#RowKey').val(rowKey);
                $('#PartitionKey').val(partitionKey);
                $('#target').show(); 
                //$('#divImg').hide();
               // $('#divFilter').show();      
                $("#btn_Registrar").html("Guardar");
            }
            else {

                $("#modal-title").text("Registrar Promoción");
                //Limpiar las cajas de texto de los mensajes
                $('#msgNombre').text(''); 
                $('#msgEnlace').text('');
                $('#msgFechaInicio').text('');
                $('#msgFechaFin').text('');
                $('#msgUrlimage').text('');
                $("#txtNombre").val("");
                $("#txtEnlace").val("");
                $("#txtFechaInicio").val("");
                $("#txtFechaFin").val("");
                $("#Urlimage").val("");
                $("#pAccion").val("Agregar");
                $('#RowKey').text('');
                $('#PartitionKey').text('');
                $('#divImagen').hide(); 
                $('#target').hide();
                //$('#divImg').show();
                $('#divFilter').hide(); 
            }
        }
    });

}

$("#btn_Registrar").click(function () {

    //funcion que quita espacios en blanco en los controles
    QuitarEspaciosBlanco();

    var _existe = ExistePromocion();

    if (_existe === true) {

        $('#msgNombre').text('Ya existe una promoción con este nombre');
        $('#msgEnlace').text('Ya existe una promoción con este enlace');
        $('#msgFechaInicio').text('Ya existe una promoción con esta fecha de inicio');
        $('#msgFechaFin').text('Ya existe una promoción con esta fecha de finalización');
    }
    else {

        //event.preventDefault();
        $('#msgNombre').text('');
        $('#msgEnlace').text('');
        $('#msgFechaInicio').text('');
        $('#msgFechaFin').text('');
        $('#msgUrlimage').text('');
        Enlace = $('#txtEnlace').val();
        Titulo = $('#txtNombre').val();
        FechaInicio = $('#txtFechaInicio').datepicker().val();
        FechaFinal = $('#txtFechaFin').datepicker().val();
        Urlimage = $('#Urlimage').val();
        EnlaceSinEspacios = $.trim(Enlace);
        TituloSinEspacios = $.trim(Titulo);
        FechaInicioSinEspacios = $.trim(FechaInicio);
        FechaFinalSinEspacios = $.trim(FechaFinal);
        UrlimageSinEspacios = $.trim(Urlimage);

        var pAccion = $("[name='pAccion']").val();

        if (TituloSinEspacios === '') {
            $('#msgNombre').text('Dato Requerido');

        }

        if (EnlaceSinEspacios === '') {
            $('#msgEnlace').text('Dato Requerido');
        }
        if (FechaInicioSinEspacios === '') {
            $('#msgFechaInicio').text('Dato Requerido');
        }
        if (FechaFinalSinEspacios === '') {
            $('#msgFechaFin').text('Dato Requerido');

        }
        var bol = validate_fecha(FechaInicio, FechaFinal);
        var res = isUrlValid();

        if (UrlimageSinEspacios === '' && pAccion === 'Agregar') {
            //enviar la url 
            $('#msgUrlimage').text('Dato Requerido');
        }

        if (EnlaceSinEspacios !== '' && FechaInicioSinEspacios !== '' && FechaFinalSinEspacios !== '' && bol !== 0 && ((pAccion === 'Actualizar') || (UrlimageSinEspacios !== '' && pAccion === 'Agregar')) && res !== 0) {

            var Enlace = $('#txtEnlace').val();
            var Titulo = $('#txtNombre').val();
            var FechaInicio = $('#txtFechaInicio').val();
            var FechaFinal = $('#txtFechaFin').val();

            var PartitionKey = '';
            var RowKey = '';

            if (pAccion === "Actualizar") {
                PartitionKey = $('#PartitionKey').val();
                RowKey = $('#RowKey').val();
            }

            var DatosCarne = new FormData();
            DatosCarne.append('enlace', Enlace);
            DatosCarne.append('titulo', Titulo);
            DatosCarne.append('fechaInicia', FechaInicio);
            DatosCarne.append('fechaFin', FechaFinal);
            if ($('#Urlimage')[0].files[0] !== '' && $('#Urlimage')[0].files[0] !== undefined && $('#Urlimage')[0].files[0] !== null) {
                DatosCarne.append('imagen', $('#Urlimage')[0].files[0]);
            }
            else {
                DatosCarne.append('imagenPath', $('#imgSalida').attr("src"));
            }

            DatosCarne.append('accion', pAccion);

            DatosCarne.append('PartitionKey', PartitionKey);
            DatosCarne.append('RowKey', RowKey);

            if (pAccion === "Agregar") {
                textoMensaje = "El registro ha sido agregado exitosamente.";
            }
            else {
                textoMensaje = "El registro ha sido modificado exitosamente.";
            }

            $.ajax({
                type: "POST",
                url: "/Promociones/EjecutarAcciones",
                data: DatosCarne,
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
                            text: textoMensaje,
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

    }

});

function QuitarEspaciosBlanco() {
    strEnlace = $('#txtEnlace').val();
    strTitulo = $('#txtNombre').val();

    var _Enlace = "";
    var _Titulo = "";

    //quitar los espacios en blanco de mas en los controles
    _Enlace = strEnlace.replace(/\s+/g, " ").trim();
    _Titulo = strTitulo.replace(/\s+/g, " ").trim();

    //se actualizan los campos
    $('#txtEnlace').val(_Enlace);
    $('#txtNombre').val(_Titulo);
}

function ExistePromocion() {

    enlace = $('#txtEnlace').val();
    titulo = $('#txtNombre').val();
    fechaIni = $('#txtFechaInicio').val();
    fechaFin = $('#txtFechaFin').val();

    var existe;

    $.ajax({
        method: "Post",
        cache: false,
        async: false,
        url: "/Promociones/ConsultarDatos",
        data: { "enlace": enlace, "titulo": titulo, "fechaInicio": fechaIni, "fechaFin": fechaFin },
        success: function (respuesta) {

            existe = respuesta;
        }
    });

    return existe;
}

function readURL(input) {
    valueURl = input.value;
}

$(document).on('keyup', "input[type='search']", function () {
    var oTable = $('.dataTable').dataTable();
    oTable.fnFilter($(this).val());
});

$("#filter").click(function () {
    $('#divImg').show();
    $('#divFilter').hide();
   
});

function mostrarImagen(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imgSalida').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$(document).ready(function () {

    $("#txtFechaInicio").datepicker({
        regional: 'es',
        minDate: _dateToday,
        numberOfMonths: 1,
        format: 'dd/mm/yyyy'
    });

    $("#txtFechaFin").datepicker({
        minDate: _dateToday,
        regional: 'es',
        numberOfMonths: 1,
        format: 'dd/mm/yyyy'
    });

});

$('#txtFechaFin').change(function () {
    $(this).datepicker('hide');
});

$('#txtFechaInicio').change(function () {
    $(this).datepicker('hide');
});

function justNumbers(e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum === 8) || (keynum === 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
}
    
function validate_fecha(fechaInicial, fechaFinal) {

    //obtenemos la fecha actual
    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();
    // se le da el formato dd/MM/yyyy
    var today = day + "/" + month + "/" + year; 
 
    valuesStart = fechaInicial.split("/");
    valuesEnd = fechaFinal.split("/");
    valuesToday = today.split("/");

    // Verificamos que la fecha no sea posterior a la actual
    var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
    var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);
    var dateToday = new Date(valuesToday[2], valuesToday[1] - 1, valuesToday[0]);

    if (dateEnd < dateToday) {
        $('#msgFechaFin').text('La fecha de finalización debe ser igual o mayor a la fecha actual');
        return 0;
    }
    else {
        if (dateStart > dateEnd) {
            $('#msgFechaInicio').text('La fecha de finalización debe ser igual o mayor a la fecha de inicio');
            return 0;
        }
        return 1; 
    }     
}

$(function () {
    $('input[type="text"]').change(function () {
        this.value = $.trim(this.value);
    });
});

$(document).ready(function () {
    $("#txtFechaInicio").keyup(function () {
        if ($(this).val().length === 2) {
            $(this).val($(this).val() + "/");
        } else if ($(this).val().length === 5) {
            $(this).val($(this).val() + "/");
        }
    });
});

$("#modal-register").on("hide.bs.modal", function () {
    //Limpiar las cajas de texto de los mensajes
    $('#msgNombre').text('');
    $('#msgDesDetalle').text('');
    $('#msgDesResumen').text('');
    $('#msgFechaInicio').text('');
    $('#msgFechaFin').text('');
    $('#msgUrlimage').text('');
    
    $("#txtNombre").val("");
    $("#txtFechaInicio").val("");
    $("#txtFechaFin").val("");
    $("#txtDesDetalle").val("");
    $("#txtDesResumen").val("");
    $("#Urlimage").val("");
    $("#pAccion").val("Agregar");
    $('#RowKey').text('');
    $('#PartitionKey').text('');
    $('#divImagen').hide();
    $('#target').hide();
    $('#divImg').show();
    $('#divFilter').hide();

});  

$(document).ready(function () {
    $("#txtFechaFin").keyup(function () {
        if ($(this).val().length === 2) {
            $(this).val($(this).val() + "/");
        } else if ($(this).val().length === 5) {
            $(this).val($(this).val() + "/");
        }
    });
});

function isUrlValid() {
    var url = $('#txtEnlace').val();

    //valida los espacios en blanco de la url
    var bool = validarEspaciosBlanco(url);

    if (bool === true) {

        url_validaFacebook = /(http|https):\/\/www\.facebook\.com\/(photo(\.php|s)|permalink\.php|media|questions|notes|[^\/]+\/(activity|posts|photos))[\/?].*/;


        //url_validate = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
        if (!url_validaFacebook.test(url)) {
            $('#msgEnlace').text('Url de Facebook no válida');
            return 0;
        }
        else {
            return 1;
        }
    }
    else {
        $('#msgEnlace').text('Url de Facebook no válida');
    }

  
}

$('#txtEnlace').keydown(function (e) {

    if (e.keyCode === 32) { return false; }

}); 

function validarEspaciosBlanco(texto) {
    if (texto.indexOf(" ") !== -1) {
        return false;
    }
    return true;
}

$(document).on('change', 'input[type="file"]', function () {
    // this.files[0].size recupera el tamaño del archivo
    // alert(this.files[0].size);
    $('#msgUrlimage').text('');
    $('#Urlimage').text('');

    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;

    if (fileSize > 1000000) {
        $('#msgUrlimage').text('El archivo no debe superar 1MB');
        this.value = '';
        this.files[0].name = '';
    } else {
        // recuperamos la extensión del archivo
        var ext = fileName.split('.').pop();

        // console.log(ext);
        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png': break;
            default:
                $('#msgUrlimage').text('El archivo no tiene la extensión adecuada');
                this.value = ''; 
                this.files[0].name = '';
        }
    }
});



           
           
           
           
           
           
           
              
           
              
           
           
           
           
           
