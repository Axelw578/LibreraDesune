$(document).ready(function () {
    // Al hacer submit en el formulario de login
    $("#loginForm").submit(function (e) {
        e.preventDefault(); // Evitar el envío tradicional del formulario

        // Obtener los valores de los campos
        var correo = $("#correo").val().trim();
        var clave = $("#clave").val().trim();

        // Limpiar mensajes de error previos
        $("#loginError").hide();
        $(".invalid-feedback").remove();
        $(".is-invalid").removeClass("is-invalid");

        // Validaciones simples de los campos
        if (correo === "") {
            mostrarError("#correo", "El campo de correo es obligatorio.");
            return;
        }

        if (!validarCorreo(correo)) {
            mostrarError("#correo", "Por favor, ingrese un correo electrónico válido.");
            return;
        }

        if (clave === "") {
            mostrarError("#clave", "El campo de contraseña es obligatorio.");
            return;
        }

        // Mostrar animación de carga
        $("button[type=submit]").html('Iniciando sesión...').attr('disabled', true);

        // Realizar la solicitud Ajax para el login
        $.ajax({
            type: "POST",
            url: "/Cuenta/Login",
            data: {
                correo: correo,
                clave: clave
            },
            success: function (response) {
                // Si el servidor indica éxito en el inicio de sesión
                if (response.success) {
                    window.location.href = "/Home/Index";
                } else {
                    // Mostrar el mensaje de error recibido del servidor
                    $("#loginError").text(response.message).fadeIn();

                    // Restaurar el botón
                    $("button[type=submit]").html('Iniciar Sesión').attr('disabled', false);
                }
            },
            error: function (xhr) {
                // Mostrar un mensaje de error según el estado de la respuesta
                var mensajeError = "Ocurrió un error inesperado.";
                if (xhr.status === 401) {
                    mensajeError = "Correo o contraseña incorrectos.";
                } else if (xhr.status === 404) {
                    mensajeError = "Usuario no encontrado.";
                }

                $("#loginError").text(mensajeError).fadeIn();

                // Restaurar el botón
                $("button[type=submit]").html('Iniciar Sesión').attr('disabled', false);
            }
        });
    });

    // Función para mostrar errores debajo de los campos
    function mostrarError(selector, mensaje) {
        $(selector).addClass("is-invalid");
        $(selector).after('<div class="invalid-feedback">' + mensaje + '</div>');
    }

    // Validación simple de formato de correo
    function validarCorreo(correo) {
        var regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(correo);
    }
});
