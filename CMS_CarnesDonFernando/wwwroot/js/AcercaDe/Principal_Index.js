var valueURl;

$("#btn_Registrar").click(function () {
    //event.preventDefault();
    $('#msgTitulo').text('');
    $('#msgDescripcion').text('');
   
    Titulo = $('#txtTitulo').val();
    Descripcion = $('#txtDescripcion').val();
    TituloSinEspacios = $.trim(Titulo);
    DescripcionSinEspacios = $.trim(Descripcion);

    var pAccion = $("[name='pAccion']").val();

    if (TituloSinEspacios === '') {
        $('#msgTitulo').text('Dato Requerido');
    }

    if (DescripcionSinEspacios === '') {
        $('#msgDescripcion').text('Dato Requerido');
    }

    if (TituloSinEspacios !== '' && DescripcionSinEspacios !== '') {

        var PartitionKey = '';
        var RowKey = '';

        if (pAccion === "Actualizar") {
            PartitionKey = $('#PartitionKey').val();
            RowKey = $('#RowKey').val();
        }

        var DatosCarne = new FormData();
        DatosCarne.append('titulo', Titulo);
        DatosCarne.append('descripcion', Descripcion);
        DatosCarne.append('accion', pAccion);
        DatosCarne.append('PartitionKey', PartitionKey);
        DatosCarne.append('RowKey', RowKey);

        var textoMensaje = "";

        if (pAccion === "Agregar") {
            textoMensaje = "El registro ha sido agregado exitosamente.";
        }
        else {
            textoMensaje = "El registro ha sido modificado exitosamente.";
        }

        $.ajax({
            type: "POST",
            url: "/AcercaDe/EjecutarAcciones",
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
});







