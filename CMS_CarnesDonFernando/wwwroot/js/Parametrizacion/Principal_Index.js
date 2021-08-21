var valueURl;

$("#btn_Registrar").click(function () {
    //event.preventDefault();
    $('#msgUserMail').text('');
    $('#msgUserPass').text('');
    $('#msgHost').text('');
    $('#msgPort').text('');
   
    UserMail = $('#txtUserMail').val();
    UserPass = $('#txtUserPass').val();
    Host = $('#txtHost').val();
    Port = $('#txtPort').val();

    CorreoSinEspacios = $.trim(UserMail);
    PassSinEspacios = $.trim(UserPass);
    HostSinEspacios = $.trim(Host);
    PortSinEspacios = $.trim(Port);

    var pAccion = $("[name='pAccion']").val();

    if (CorreoSinEspacios === '') {
        $('#msgUserMail').text('Dato Requerido');
    }

    if (PassSinEspacios === '') {
        $('#msgUserPass').text('Dato Requerido');
    }

    if (HostSinEspacios === '') {
        $('#msgHost').text('Dato Requerido');
    }

    if (PortSinEspacios === '') {
        $('#msgPort').text('Dato Requerido');
    }
    //if ((CorreoSinEspacios !== '' && validarCorreo(UserMail) !== false) && PassSinEspacios !== '' && (HostSinEspacios !== '' && validarSMTP(Host) !== false) && PortSinEspacios !== '') {

    if ((CorreoSinEspacios !== '' && validarCorreo(UserMail) !== false) && PassSinEspacios !== '' && HostSinEspacios !== '' && PortSinEspacios !== '') {

        var PartitionKey = '';
        var RowKey = '';

        if (pAccion === "Actualizar") {
            PartitionKey = $('#PartitionKey').val();
            RowKey = $('#RowKey').val();
        }

        var Datos = new FormData();
        Datos.append('user', UserMail);
        Datos.append('pass', UserPass);
        Datos.append('client', Host);
        Datos.append('port', Port);
        Datos.append('accion', pAccion);
        Datos.append('PartitionKey', PartitionKey);
        Datos.append('RowKey', RowKey);

        if (pAccion === "Agregar") {
            textoMensaje = "El registro ha sido agregado exitosamente.";
        }
        else {
            textoMensaje = "El registro ha sido modificado exitosamente.";
        }

        $.ajax({
            type: "POST",
            url: "/Parametrizacion/EjecutarAcciones",
            data: Datos,
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

function validarCorreo(correo) {
    
    filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (filter.test(correo)) {
       
        return true;
    }
    else {
        return false;
    }
}

function validarSMTP(smtp) {

    filter = /^(Smtp|Mail|smtp|mail)+\.(live|gmail|gmx|carnesdonfernando)+\.(com|net)+$/;

    if (filter.test(smtp)) {

        return true;
    }
    else {
        return false;
    }
}







