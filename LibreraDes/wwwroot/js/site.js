﻿document.addEventListener("DOMContentLoaded", function () {
    // Animación suave para los dropdowns
    var dropdowns = document.querySelectorAll('.nav-item.dropdown');
    dropdowns.forEach(function (dropdown) {
        dropdown.addEventListener('mouseenter', function () {
            let dropdownMenu = this.querySelector('.dropdown-menu');
            if (dropdownMenu) {
                dropdownMenu.style.display = 'block';
                dropdownMenu.classList.add('fade-in');
            }
        });

        dropdown.addEventListener('mouseleave', function () {
            let dropdownMenu = this.querySelector('.dropdown-menu');
            if (dropdownMenu) {
                dropdownMenu.classList.remove('fade-in');
                setTimeout(function () {
                    dropdownMenu.style.display = 'none';
                }, 300); // Espera a que termine la animación antes de ocultarlo
            }
        });
    });

    // Animación al hacer clic en el botón del navbar-toggler
    var navbarToggler = document.querySelector('.navbar-toggler');
    navbarToggler.addEventListener('click', function () {
        navbarToggler.classList.toggle('active');
        if (navbarToggler.classList.contains('active')) {
            navbarToggler.style.transform = 'rotate(90deg)';
        } else {
            navbarToggler.style.transform = 'rotate(0deg)';
        }
    });

    // Transiciones suaves para las imágenes de perfil
    var profileImages = document.querySelectorAll('.navbar .nav-link img');
    profileImages.forEach(function (image) {
        image.addEventListener('mouseenter', function () {
            image.style.transform = 'scale(1.1)';
            image.style.transition = 'transform 0.3s ease, border-color 0.3s ease';
        });

        image.addEventListener('mouseleave', function () {
            image.style.transform = 'scale(1)';
        });
    });

    // Animación en el botón de búsqueda
    var searchInput = document.querySelector('.form-control');
    searchInput.addEventListener('focus', function () {
        this.style.borderColor = '#FFD700';
        this.style.boxShadow = '0 0 10px rgba(255, 215, 0, 0.5)';
        this.style.transition = 'box-shadow 0.3s ease';
    });

    searchInput.addEventListener('blur', function () {
        this.style.borderColor = '';
        this.style.boxShadow = '';
    });

    // Efecto de hover en los botones de la barra de navegación
    var navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    navLinks.forEach(function (link) {
        link.addEventListener('mouseenter', function () {
            this.style.color = '#FFD700';
        });

        link.addEventListener('mouseleave', function () {
            this.style.color = '';
        });
    });

    // Activar el dropdown con clic en lugar de hover para pantallas táctiles
    var touchDropdowns = document.querySelectorAll('.navbar-nav .nav-item.dropdown');
    touchDropdowns.forEach(function (dropdown) {
        dropdown.addEventListener('click', function (e) {
            if (window.innerWidth < 992) {
                e.preventDefault();
                let dropdownMenu = this.querySelector('.dropdown-menu');
                if (dropdownMenu.style.display === 'block') {
                    dropdownMenu.style.display = 'none';
                } else {
                    dropdownMenu.style.display = 'block';
                }
            }
        });
    });



    // Suavizar el scroll de la página
    var links = document.querySelectorAll('a[href^="#"]');
    links.forEach(function (link) {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            var target = document.querySelector(this.getAttribute('href'));
            window.scrollTo({
                top: target.offsetTop,
                behavior: 'smooth'
            });
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    // Función para verificar el número de productos en el carrito
    function actualizarIndicadorCarrito() {
        fetch('/Carrito/ObtenerCantidadProductos')
            .then(response => response.json())
            .then(data => {
                var indicador = document.getElementById("carrito-indicador");
                if (data.cantidad > 0) {
                    indicador.innerText = data.cantidad;
                    indicador.style.display = "inline";
                } else {
                    indicador.style.display = "none";
                }
            });
    }

    // Llamar a la función al cargar la página
    actualizarIndicadorCarrito();

    // Actualizar el indicador cada 10 segundos (por ejemplo)
    setInterval(actualizarIndicadorCarrito, 10000);
});
