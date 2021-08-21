var valueURl;

function EliminarPlanLealtad(partitionKey, rowKey) {
    swal({
        title: "¿Está seguro que desea eliminar el Plan de Lealtad?",
        icon: "warning",
        buttons: ["Cancelar", "Eliminar"],
        dangerMode: true
    }).then(function (willDelete) {
        if (willDelete) {
            EliminarPlanLealtadAJAX("post", "/PlanLealtad/Eliminar", partitionKey, rowKey);
        } else {
            swal("Cancelado", "La acción eliminar ha sido detenida.", "error");
        }
    });
}

function EliminarPlanLealtadAJAX(method, uri, partitionKey, rowKey) {

    $.ajax({
        method: method,
        url: uri,
        async: true,
        cache: false,
        data: { "pLlaveParticion": partitionKey, "pLlaveFila": rowKey },
        success: function (data) {
            if (data.isCompleted === true) {
                swal({
                    title: "Éxito",
                    text: "El registro ha sido eliminado exitosamente.",
                    icon: "success",
                    type: "success"
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
        url: "/PlanLealtad/Formulario",
        async: true,
        cache: false,
        data: { "pLlaveParticion": partitionKey, "pLlaveFila": rowKey },
        success: function (response) {

            var data = response;

            if (data !== null) {

                $("#modal-title").text("Actualizar Plan de Lealtad");
                $('#divImagen').show();
                //Limpiar las cajas de texto de los mensajes
                $('#msgTipoCliente').text('');
                $('#msgDescripcion').text('');
                $('#msgBeneficios').text('');
                $('#msgBono').text('');
                $('#msgUrlimage').text('');
                $('#RowKey').text('');
                $('#PartitionKey').text('');
                
                //asignar los valores a cada control 
                $("#txtTipoCliente").val(data.tipoCliente);
                $("#txtDescripcion").val(data.descripcion);
                $("#txtBeneficios").val(data.beneficios);
                $("#txtBono").val(data.bonoNivel);

                $('#imgSalida').attr("src", data.foto);
                var imagen = data.foto;
                $("#pAccion").val("Actualizar");
                $('#RowKey').val(rowKey);
                $('#PartitionKey').val(partitionKey);
                $('#target').show();
                //$('#divImg').hide();
                //$('#divFilter').show();
                $("#btn_Registrar").html("Guardar");
            }
            else {

                $("#modal-title").text("Registrar Plan de Lealtad");
                //Limpiar las cajas de texto de los mensajes
                $('#msgTipoCliente').text('');
                $('#msgDescripcion').text('');
                $('#msgBeneficios').text('');
                $('#msgUrlimage').text('');
                $('#msgBono').text('');
                $("#txtTipoCliente").val("");
                $("#txtDescripcion").val("");
                $("#txtBeneficios").val("");
                $("#txtBono").val("");
                $("#Urlimage").val("");

                $("#pAccion").val("Agregar");
                $('#RowKey').text('');
                $('#PartitionKey').text('');
                $('#divImagen').hide();
                $('#target').hide();
                $('#divImg').show();
                $('#divFilter').hide();
            }

        }
    });

}

$("#btn_Registrar").click(function () {
    //funcion que quita espacios en blanco en los controles
    QuitarEspaciosBlanco();

    var _existe = ExistePlanLealtad();

    if (_existe === true) {
        $('#msgTipoCliente').text('Ya existe un plan de lealtad con este tipo de cliente');
        $('#msgDescripcion').text('Ya existe un plan de lealtad con esta descripción');
        $('#msgBeneficios').text('Ya existe un plan de lealtad con este beneficio');
        $('#msgBono').text('Ya existe un plan de lealtad con este bono');
    }
    else {
        //event.preventDefault();
        $('#msgTipoCliente').text('');
        $('#msgDescripcion').text('');
        $('#msgBeneficios').text('');
        $('#msgBono').text('');
        $('#msgUrlimage').text('');
        Descripcion = $('#txtDescripcion').val();
        TipoCliente = $('#txtTipoCliente').val();
        Beneficios = $('#txtBeneficios').val();
        Bono = $('#txtBono').val();
        Urlimage = $('#Urlimage').val();

        var pAccion = $("[name='pAccion']").val();

        if (Descripcion === '') {
            $('#msgDescripcion').text('Dato Requerido');
        }
        if (TipoCliente === '') {
            $('#msgTipoCliente').text('Dato Requerido');
        }

        if (Beneficios === '') {
            $('#msgBeneficios').text('Dato Requerido');
        }
        if (Bono === '') {
            $('#msgBono').text('Dato Requerido');
        }

        if (Urlimage === '' && pAccion === 'Agregar') {
            //enviar la url 
            $('#msgUrlimage').text('Dato Requerido');
        }

        if (Descripcion !== '' && TipoCliente !== '' && Beneficios !== '' && Bono !== '') {

            var PartitionKey = '';
            var RowKey = '';

            if (pAccion === "Actualizar") {
                PartitionKey = $('#PartitionKey').val();
                RowKey = $('#RowKey').val();
            }

            var DatosPlanLealtad = new FormData();
            DatosPlanLealtad.append('tipoCliente', TipoCliente);
            DatosPlanLealtad.append('descripcion', Descripcion);
            DatosPlanLealtad.append('beneficios', Beneficios);
            DatosPlanLealtad.append('bono', Bono);

            if ($('#Urlimage')[0].files[0] !== '' && $('#Urlimage')[0].files[0] !== undefined && $('#Urlimage')[0].files[0] !== null) {
                DatosPlanLealtad.append('image', $('#Urlimage')[0].files[0]);
            }
            else {
                DatosPlanLealtad.append('imagenPath', $('#imgSalida').attr("src"));
            }

            DatosPlanLealtad.append('accion', pAccion);
            DatosPlanLealtad.append('PartitionKey', PartitionKey);
            DatosPlanLealtad.append('RowKey', RowKey);

            if (pAccion === "Agregar") {
                textoMensaje = "El registro ha sido agregado exitosamente.";
            }
            else {
                textoMensaje = "El registro ha sido modificado exitosamente.";
            }

            $.ajax({
                type: "POST",
                url: "/PlanLealtad/EjecutarAcciones",
                data: DatosPlanLealtad,
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
    strDescripcion = $('#txtDescripcion').val();
    strTipoCliente = $('#txtTipoCliente').val();
    strBeneficios = $('#txtBeneficios').val();
    strBono = $('#txtBono').val();

    var _Descripcion = "";
    var _TipoCliente = "";
    var _Beneficios = "";
    var _Bono = "";

    //quitar los espacios en blanco de mas en los controles
    _Descripcion = strDescripcion.replace(/\s+/g, " ").trim();
    _TipoCliente = strTipoCliente.replace(/\s+/g, " ").trim();
    _Beneficios = strBeneficios.replace(/\s+/g, " ").trim();
    _Bono = strBono.replace(/\s+/g, " ").trim();

    //se actualizan los campos
    $('#txtDescripcion').val(_Descripcion);
    $('#txtTipoCliente').val(_TipoCliente);
    $('#txtBeneficios').val(_Beneficios);
    $('#txtBono').val(_Bono);
}

function ExistePlanLealtad() {

    Descripcion = $('#txtDescripcion').val();
    TipoCliente = $('#txtTipoCliente').val();
    Beneficios = $('#txtBeneficios').val();
    Bono = $('#txtBono').val();

    var existe;

    $.ajax({
        method: "Post",
        cache: false,
        async: false,
        url: "/PlanLealtad/ConsultarDatos",
        data: { "tipoCliente": TipoCliente, "descripcion": Descripcion, "Beneficios": Beneficios, "Bono": Bono },
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

$(function () {
    $('input[type="text"]').change(function () {
        this.value = $.trim(this.value);
    });
});

$("#modal-register").on("hide.bs.modal", function () {
    //Limpiar las cajas de texto de los mensajes
    $('#msgTipoCliente').text('');
    $('#msgDescripcion').text('');
    $('#msgBeneficios').text('');
    $('#msgUrlimage').text('');
    
    $("#txtTipoCliente").val("");
    $("#txtDescripcion").val("");
    $("#txtBeneficios").val("");
    $("#Urlimage").val("");
    $("#pAccion").val("Agregar");
    $('#RowKey').text('');
    $('#PartitionKey').text('');
    $('#divImagen').hide();
    $('#target').hide();
    $('#divImg').show();
    $('#divFilter').hide();

});

$(document).on('change', 'input[type="file"]', function () {
    
    $('#msgUrlimage').text('');

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

