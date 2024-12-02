// Home.js

// Esperar a que el DOM esté completamente cargado
document.addEventListener("DOMContentLoaded", function () {
    // Seleccionar todos los botones de "Ver Detalles"
    const detailButtons = document.querySelectorAll(".animated-button");

    detailButtons.forEach((button) => {
        button.addEventListener("click", function (event) {
            // Evitar el comportamiento predeterminado del enlace
            event.preventDefault();

            // Obtener la URL del enlace
            const url = this.getAttribute("href");

            // Animar el botón al hacer clic
            this.classList.add("btn-clicked");

            // Redirigir después de un breve retraso para la animación
            setTimeout(() => {
                window.location.href = url; // Redirigir a la URL del libro
            }, 300); // Retraso en milisegundos
        });
    });

    // Efecto de ocultar/mostrar detalles del libro al hacer clic en el recuadro de detalles
    const detalleLibro = document.getElementById("detalleLibro");
    if (detalleLibro) {
        detalleLibro.addEventListener("click", function () {
            // Alternar la visibilidad con un efecto de deslizamiento
            this.classList.toggle("oculto");
        });
    }

    // Efecto de desplazamiento suave para enlaces (opcional)
    const smoothScrollLinks = document.querySelectorAll('a[href^="#"]');

    smoothScrollLinks.forEach((link) => {
        link.addEventListener("click", function (event) {
            event.preventDefault();

            const targetId = this.getAttribute("href");
            const targetElement = document.querySelector(targetId);

            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: "smooth"
                });
            }
        });
    });

    // Manejo del formulario "Agregar al carrito"
    const addToCartForms = document.querySelectorAll(".add-to-cart-form");

    addToCartForms.forEach((form) => {
        form.addEventListener("submit", function (event) {
            event.preventDefault(); // Prevenir el comportamiento predeterminado

            const libroId = this.querySelector("input[name='libroId']").value;
            const cantidad = this.querySelector("input[name='cantidad']").value;

            // Enviar los datos mediante AJAX (usando jQuery en este ejemplo)
            $.ajax({
                url: this.action, // La acción del formulario apunta al controlador
                method: "POST",
                data: {
                    libroId: libroId,
                    cantidad: cantidad
                },
                success: function (response) {
                    alert("Libro añadido al carrito exitosamente.");
                },
                error: function () {
                    alert("Error al añadir el libro al carrito.");
                }
            });
        });
    });
});
