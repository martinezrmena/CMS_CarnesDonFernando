var valueURl;


function EliminarProductoMes(partitionKey, rowKey) {
    swal({
        title: "¿Está seguro que desea eliminar el producto?",
        icon: "warning",
        buttons: ["Cancelar", "Eliminar"],
        dangerMode: true
    }).then(function (willDelete) {
        if (willDelete) {
            EliminarProductoAJAX("post", "/ProductosMes/Eliminar", partitionKey, rowKey);
        } else {
            swal("Cancelado", "La acción eliminar ha sido detenida.", "error");
        }
    });
    


   
}

function EliminarProductoAJAX(method, uri, partitionKey, rowKey) {

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
        url: "/ProductosMes/EjecutarAcciones",
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
        url: "/ProductosMes/Formulario",
        async: true,
        cache: false,
        data: { "pLlaveParticion": partitionKey, "pLlaveFila": rowKey },
        success: function (response) {

            var data = response;

            if (data !== null) {

                $("#modal-title").text("Actualizar Producto del Mes");
                $('#divImagen').show();
                //Limpiar las cajas de texto de los mensajes
                $('#msgNombre').text('');
                $('#msgDesDetalle').text('');
                $('#msgDesResumen').text('');
                $('#msgFechaInicio').text('');
                $('#msgFechaFin').text('');
                $('#msgUrlimage').text('');
                $('#RowKey').text('');
                $('#PartitionKey').text('');
                $('#msgPreparacion').text('');

                //asignar los valores a cada control 
                $("#txtNombre").val(data.titulo);
                $("#txtFechaInicio").val(data.fecha_Publicacion);
                $("#txtFechaFin").val(data.fecha_Finalizacion);
                $("#txtDesDetalle").val(data.descripcion_Detalle);
                $("#txtDesResumen").val(data.descripcion_Resumen);
                $('#imgSalida').attr("src", data.imagenUrl);
                var imagen = data.imagenUrl;
                $("#pAccion").val("Actualizar");
                $('#RowKey').val(rowKey);
                $('#PartitionKey').val(partitionKey);
                $('#target').show(); 
               // $('#divImg').hide();
                $('#divFilter').show();      
                $("#btn_Registrar").html("Guardar");


                //Checks
                var preparacion = data.preparacion;       

                if (preparacion.indexOf("Coccion") >= 0) {
                    $('#chkCoccion').attr('checked', true);
                }
                if (preparacion.indexOf("Frio") >= 0) {
                    $('#chkFrio').attr('checked', true);
                }
                if (preparacion.indexOf("Horno") >= 0) {
                    $('#chkHorno').attr('checked', true);
                }
                if (preparacion.indexOf("Parrilla") >= 0) {
                    $('#chkParrilla').attr('checked', true);
                }
                if (preparacion.indexOf("Sarten") >= 0) {
                    $('#chkSarten').attr('checked', true);
                }
            }
            else {

                $("#modal-title").text("Registrar Producto del Mes");
                //Limpiar las cajas de texto de los mensajes
                $('#msgNombre').text('');
                $('#msgDesDetalle').text('');
                $('#msgDesResumen').text('');
                $('#msgFechaInicio').text('');
                $('#msgFechaFin').text('');
                $('#msgUrlimage').text('');
                $('#msgPreparacion').text('');
                $("#txtNombre").val("");
                $("#Urlimage").val("");
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
                //$('#divImg').show();
                $('#divFilter').hide(); 

                $('#chkCoccion').attr('checked', false);
                $('#chkFrio').attr('checked', false);
                $('#chkHorno').attr('checked', false);
                $('#chkParrilla').attr('checked', false);
                $('#chkSarten').attr('checked', false);
            }
 
        }
    });

}

$("#btn_Registrar").click(function () {
    //funcion que quita los espacios en blanco de mas en los controles
    QuitarEspaciosBlanco();

    var _existe = ExisteProducto();

    if (_existe === true) {
        $('#msgNombre').text('Ya existe un producto con este nombre');
        $('#msgFechaInicio').text('Ya existe un producto con esta fecha de inicio');
        $('#msgFechaFin').text('Ya existe un producto con esta fecha de finalización');
        $('#msgDesResumen').text('Ya existe un producto con esta descrición resumen');
        $('#msgDesDetalle').text('Ya existe un producto con esta descrición detalle');
        $('#msgPreparacion').text('Ya existe un producto con esta preparación');
    }
    else {

        //event.preventDefault();
        $('#msgNombre').text('');
        $('#msgFechaInicio').text('');
        $('#msgFechaFin').text('');
        $('#msgDesResumen').text('');
        $('#msgDesDetalle').text('');
        $('#msgUrlimage').text('');
        $('#msgPreparacion').text('');
        DescripcionResumen = $('#txtDesResumen').val();
        DescripcionDetalle = $('#txtDesDetalle').val();
        Titulo = $('#txtNombre').val();
        FechaInicio = $('#txtFechaInicio').val();
        FechaFinal = $('#txtFechaFin').val();
        Urlimage = $('#Urlimage').val();

        var listaPreparacion = '';

        $("input[name=Preparacion]").each(function (index) {
            if ($(this).is(':checked')) {

                if (listaPreparacion === "") {
                    listaPreparacion = listaPreparacion + $(this).val();
                }
                else {
                    listaPreparacion = listaPreparacion + ", " + $(this).val();
                }
            }
        });

        var pAccion = $("[name='pAccion']").val();

        if (Titulo === '') {
            $('#msgNombre').text('Dato Requerido');
        }
        if (DescripcionDetalle === '') {
            $('#msgDesDetalle').text('Dato Requerido');
        }

        if (DescripcionResumen === '') {
            $('#msgDesResumen').text('Dato Requerido');
        }
        if (FechaInicio === '') {
            $('#msgFechaInicio').text('Dato Requerido');
        }
        if (FechaFinal === '') {
            $('#msgFechaFin').text('Dato Requerido');
        }

        var bol = validate_fechaMayorQue(FechaInicio, FechaFinal);

        if (Urlimage === '' && pAccion === 'Agregar') {
            //enviar la url 
            $('#msgUrlimage').text('Dato Requerido');
        }

        if (listaPreparacion === '') {
            $('#msgPreparacion').text('Dato Requerido');
        }

        if (Titulo !== '' && DescripcionResumen !== '' && FechaInicio !== '' && FechaFinal !== '' && DescripcionDetalle !== '' && listaPreparacion !== '' && bol !== 0) {
            var DescripcionResumen = $('#txtDesResumen').val();
            var DescripcionDetalle = $('#txtDesDetalle').val();
            var Titulo = $('#txtNombre').val();
            var FechaInicio = $('#txtFechaInicio').val();
            var FechaFinal = $('#txtFechaFin').val();
            var PartitionKey = '';
            var RowKey = '';

            if (pAccion === "Actualizar") {
                PartitionKey = $('#PartitionKey').val();
                RowKey = $('#RowKey').val();
            }

            var Preparacion = listaPreparacion;

            var DatosCarne = new FormData();
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
            DatosCarne.append('descripcion_Resumen', DescripcionResumen);
            DatosCarne.append('descripcion_Detalle', DescripcionDetalle);
            DatosCarne.append('preparacion', Preparacion);

            if (pAccion === "Agregar") {
                textoMensaje = "El registro ha sido agregado exitosamente.";
            }
            else {
                textoMensaje = "El registro ha sido modificado exitosamente.";
            }

            $.ajax({
                type: "POST",
                url: "/ProductosMes/EjecutarAcciones",
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
    strDescripcionResumen = $('#txtDesResumen').val();
    strDescripcionDetalle = $('#txtDesDetalle').val();
    strTitulo = $('#txtNombre').val();

    var _DescResumen = "";
    var _DescDetalle = "";
    var _Titulo = "";

    //quitar los espacios en blanco de mas en los controles
    _DescResumen = strDescripcionResumen.replace(/\s+/g, " ").trim();
    _DescDetalle = strDescripcionDetalle.replace(/\s+/g, " ").trim();
    _Titulo = strTitulo.replace(/\s+/g, " ").trim();

    //se actualizan los campos
    $('#txtDesDetalle').val(_DescDetalle);
    $('#txtDesResumen').val(_DescResumen);
    $('#txtNombre').val(_Titulo);
}

function ExisteProducto() {

    DescripcionResumen = $('#txtDesResumen').val();
    DescripcionDetalle = $('#txtDesDetalle').val();
    Titulo = $('#txtNombre').val();
    FechaInicio = $('#txtFechaInicio').val();
    FechaFinal = $('#txtFechaFin').val();

    var listaPreparacion = '';

    $("input[name=Preparacion]").each(function (index) {
        if ($(this).is(':checked')) {

            if (listaPreparacion === "") {
                listaPreparacion = listaPreparacion + $(this).val();
            }
            else {
                listaPreparacion = listaPreparacion + ", " + $(this).val();
            }
        }
    });

    var existe;

    $.ajax({
        method: "Post",
        cache: false,
        async: false,
        url: "/ProductosMes/ConsultarDatos",
        data: { "titulo": Titulo, "detalle": DescripcionDetalle, "resumen": DescripcionResumen, "preparacion": listaPreparacion, "fechaInicio": FechaInicio, "fechaFin": FechaFinal },
        success: function (respuesta) {

            existe = respuesta;
        }
    });

    return existe;
}

$("#Urlimage").change(function () {
    var a = document.getElementById("Urlimage").files[0].name;
    readURL(this);
});

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


$("#txtFechaInicio").datepicker({
    regional: 'es',
    minDate: 0,
    numberOfMonths: 1,
    format: 'dd/mm/yyyy'
});

$("#txtFechaFin").datepicker({
    minDate: 0,
    regional: 'es',
    numberOfMonths: 1,
    format: 'dd/mm/yyyy'
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

function validate_fechaMayorQue(fechaInicial, fechaFinal) {
    valuesStart = fechaInicial.split("/");
    valuesEnd = fechaFinal.split("/");

    // Verificamos que la fecha no sea posterior a la actual
    var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
    var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);
    if (dateStart > dateEnd) {
        $('#msgFechaInicio').text('La fecha de finalización debe ser igual o mayor a la fecha de inicio');
        return 0;
    }
    return 1;
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
    $('#msgPreparacion').text('');
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

    $('#chkCoccion').attr('checked', false);
    $('#chkFrio').attr('checked', false);
    $('#chkHorno').attr('checked', false);
    $('#chkParrilla').attr('checked', false);
    $('#chkSarten').attr('checked', false);
});

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

           
           
           
           
           
           
           
           
           
              
           
              
           
           
           
           
           
