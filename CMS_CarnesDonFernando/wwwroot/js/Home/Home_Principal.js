
    $('#btnIngresar').click(function () {
        $('#msgLogin').text('');
        var DataLogin = new FormData();

        DataLogin.append('UserMail', $('#txtCorreo').val());
        DataLogin.append('UserPassword', $('#txtContrasenna').val());

        $.ajax({
            async: false,
            type: 'POST',
            url: '/Home/IniciarSeccion',
            data: DataLogin,
            contentType: false, // Not to set any content header
            processData: false,
            success: function (data) {
                console.log(data);
                if (data > 0) {
                    window.location.href = '/Home/Dashboard';
                } else {
                    $('#msgLogin').text('Usuario o contraseña incorrecto');
                }
            }
        });
    });

//Evitar los espacios en blanco al utilizar la tecla escape
$('#txtCorreo').keydown(function (e) {

    if (e.keyCode === 32) { return false; }

}); 

$('#txtContrasenna').keydown(function (e) {

    if (e.keyCode === 32) { return false; }

}); 


//Eventos
$('#txtCorreo').keypress(function (event) {

    var contrasenna = $('#txtContrasenna').val();
    var correo = $('#txtCorreo').val();

    var keycode = event.keyCode ? event.keyCode : event.which;

    if (keycode === 13) {

        if (correo !== "") {
            if (contrasenna === "") {
                $('#txtContrasenna').focus();
            }
            else {

                $('#btnIngresar').trigger('click');
            }
        }
    }
});

$('#txtContrasenna').keypress(function (event) {

    var keycode = event.keyCode ? event.keyCode : event.which;

    if (keycode === 13) {

        $('#btnIngresar').trigger('click');
    }
});
