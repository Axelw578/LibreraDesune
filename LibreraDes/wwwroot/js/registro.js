document.addEventListener("DOMContentLoaded", function () {
    const registroForm = document.getElementById("registroForm");
    const registroError = document.getElementById("registroError");

    // Manejo del evento de envío del formulario
    registroForm.addEventListener("submit", async function (event) {
        event.preventDefault(); // Evita el envío normal del formulario

        // Captura los valores del formulario
        const nombreUsuario = document.getElementById("nombreUsuario").value.trim();
        const correo = document.getElementById("correo").value.trim();
        const clave = document.getElementById("clave").value;
        const confirmarClave = document.getElementById("confirmarClave").value;
        const urlFotoPerfil = document.getElementById("urlFotoPerfil").value.trim();

        // Validaciones simples
        if (!nombreUsuario) {
            mostrarError("El nombre de usuario es obligatorio.");
            return;
        }
        if (!correo || !esCorreoValido(correo)) {
            mostrarError("Por favor, introduce un correo electrónico válido.");
            return;
        }
        if (!clave) {
            mostrarError("La contraseña es obligatoria.");
            return;
        }
        if (clave !== confirmarClave) {
            mostrarError("Las contraseñas no coinciden.");
            return;
        }

        // Preparar datos para enviar
        const datos = {
            nombreUsuario,
            correo,
            clave,
            urlFotoPerfil
        };

        try {
            const response = await fetch("/Cuenta/Registrar", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": document.getElementsByName('__RequestVerificationToken')[0].value // Token CSRF
                },
                body: JSON.stringify(datos)
            });

            // Verificar la respuesta del servidor
            if (!response.ok) {
                const errorMensaje = await response.text(); // Leer el mensaje del servidor
                mostrarError(errorMensaje);
                return;
            }

            // Si el registro fue exitoso
            const resultado = await response.text();

            if (resultado.includes("registrado con éxito")) {
                mostrarMensajeExito("Registro exitoso. Redirigiendo...");
                setTimeout(() => {
                    window.location.href = "/Cuenta/Login"; // Redirige a la página de login
                }, 2000); // Espera 2 segundos antes de redirigir
            } else {
                mostrarError(resultado);
            }
        } catch (error) {
            mostrarError("Ocurrió un error al registrar. Intente nuevamente más tarde.");
        }
    });

    // Validar el formato del correo electrónico
    function esCorreoValido(correo) {
        const regexCorreo = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        return regexCorreo.test(correo);
    }

    // Función para mostrar mensajes de error
    function mostrarError(mensaje) {
        registroError.textContent = mensaje;
        registroError.style.display = "block";
        registroError.style.whiteSpace = "normal"; // Ajustar texto en múltiples líneas
        registroError.style.visibility = "visible";
        registroError.style.overflow = "auto";
        registroError.style.height = "auto";
        registroError.style.maxHeight = "300px"; // Límite de altura máximo

        setTimeout(() => {
            registroError.style.display = "none"; // Ocultar mensaje después de 10 segundos
        }, 10000); // Tiempo para mensajes largos
    }

    // Función para mostrar mensajes de éxito
    function mostrarMensajeExito(mensaje) {
        const mensajeExito = document.createElement("div");
        mensajeExito.className = "alert alert-success";
        mensajeExito.textContent = mensaje;
        registroForm.prepend(mensajeExito); // Inserta el mensaje al inicio del formulario

        // Eliminar el mensaje después de 5 segundos
        setTimeout(() => {
            mensajeExito.remove();
        }, 5000);
    }
});
