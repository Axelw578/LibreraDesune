$(document).ready(function () {
    // Actualiza el subtotal y el total automáticamente
    function actualizarTotal() {
        let total = 0;
        $('.producto-seleccionado:checked').each(function () {
            let fila = $(this).closest('tr');
            let subtotal = parseFloat(fila.find('.subtotal').text().replace('$', '').replace(',', ''));
            total += subtotal;
        });
        $('#total').text('$' + total.toFixed(2));
    }

    // Evento cuando se selecciona o deselecciona un producto
    $('.producto-seleccionado').on('change', function () {
        actualizarTotal();
    });

    // Evento cuando se cambia la cantidad
    $('.cantidad-input').on('change', function () {
        let libroId = $(this).data('libroid');
        let cantidad = $(this).val();

        // Llamada AJAX para actualizar la cantidad sin refrescar la página
        $.post('/Carrito/ActualizarCantidad', { libroId: libroId, cantidad: cantidad }, function (response) {
            // Actualizar el subtotal de esa fila
            let fila = $('input[data-libroid="' + libroId + '"]').closest('tr');
            let precioUnitario = parseFloat(fila.find('td:eq(3)').text().replace('$', '').replace(',', ''));
            let nuevoSubtotal = precioUnitario * cantidad;
            fila.find('.subtotal').text('$' + nuevoSubtotal.toFixed(2));

            // Actualizar el total
            actualizarTotal();
        });
    });

    // Evento para eliminar un producto del carrito
    $('.eliminar-producto').on('click', function () {
        let libroId = $(this).data('libroid');

        // Llamada AJAX para eliminar el producto
        $.post('/Carrito/EliminarDelCarrito', { libroId: libroId }, function () {
            // Eliminar la fila del producto en el DOM
            $('button[data-libroid="' + libroId + '"]').closest('tr').remove();

            // Actualizar el total
            actualizarTotal();
        });
    });

    // Actualizar el total al cargar la página
    actualizarTotal();

    // Función para actualizar el carrito
    function actualizarCarrito() {
        $.ajax({
            url: '/Carrito/ObtenerCarrito', // Cambia a la ruta correcta
            method: 'GET',
            success: function (data) {
                $('#items-carrito').empty();
                let total = 0;

                data.forEach(item => {
                    total += item.Subtotal; // Suma el subtotal de cada item
                    $('#items-carrito').append(`
                        <div class="libro">
                            <img src="${item.URLImagenLibro}" alt="${item.TituloLibro}">
                            <div class="titulo-libro">${item.TituloLibro}</div>
                            <input type="number" value="${item.Cantidad}" class="cantidad-input" data-libroid="${item.LibroId}" min="1">
                            <button class="btn-aumentar" data-libro-id="${item.LibroId}">+</button>
                            <button class="btn-disminuir" data-libro-id="${item.LibroId}">-</button>
                            <button class="btn-eliminar" data-libro-id="${item.LibroId}">Eliminar</button>
                        </div>
                    `);
                });

                $('#total-carrito').text(`Total: $${total.toFixed(2)}`); // Actualiza el total
                actualizarTotal(); // Asegurarse de que el total se actualice
            }
        });
    }

    // Carga inicial del carrito
    actualizarCarrito();

    // Aumentar cantidad
    $(document).on('click', '.btn-aumentar', function () {
        const libroId = $(this).data('libro-id');
        $.ajax({
            url: '/Carrito/AumentarCantidad', // Cambia a la ruta correcta
            method: 'POST',
            data: { libroId: libroId },
            success: function () {
                actualizarCarrito(); // Actualiza el carrito
            }
        });
    });

    // Disminuir cantidad
    $(document).on('click', '.btn-disminuir', function () {
        const libroId = $(this).data('libro-id');
        $.ajax({
            url: '/Carrito/DisminuirCantidad', // Cambia a la ruta correcta
            method: 'POST',
            data: { libroId: libroId },
            success: function () {
                actualizarCarrito(); // Actualiza el carrito
            }
        });
    });

    // Eliminar libro
    $(document).on('click', '.btn-eliminar', function () {
        const libroId = $(this).data('libro-id');
        $.ajax({
            url: '/Carrito/Eliminar', // Cambia a la ruta correcta
            method: 'POST',
            data: { libroId: libroId },
            success: function () {
                actualizarCarrito(); // Actualiza el carrito
            }
        });
    });
});
    <script>
        $(document).ready(function () {
            // Eliminar producto
            $('.eliminar-producto').on('click', function () {
                var libroId = $(this).data('libroid');
                // Aquí puedes hacer la lógica para eliminar el producto, por ejemplo, enviar una solicitud AJAX
                console.log("Eliminar libro con ID: " + libroId);
            });

            // Actualizar cantidad y recalcular subtotal
            $('.cantidad-input').on('change', function () {
                var cantidad = $(this).val();
                var libroId = $(this).data('libroid');
                // Aquí puedes hacer la lógica para actualizar la cantidad en el carrito
                console.log("Actualizar cantidad de libro con ID: " + libroId + " a " + cantidad);
            });

            // Seleccionar productos para comprar
            $('#form-carrito').on('submit', function (e) {
                var seleccionados = $('.producto-seleccionado:checked').length;
                if (seleccionados === 0) {
                    e.preventDefault();
                    alert('Por favor selecciona al menos un producto.');
                }
            });
        });

        <script>
            $(document).ready(function () {
                // Eliminar producto
                $('.eliminar-producto').on('click', function () {
                    var libroId = $(this).data('libroid');
                    // Aquí puedes hacer la lógica para eliminar el producto, por ejemplo, enviar una solicitud AJAX
                    console.log("Eliminar libro con ID: " + libroId);
                });

            // Actualizar cantidad y recalcular subtotal
            $('.cantidad-input').on('change', function () {
                var cantidad = $(this).val();
            var libroId = $(this).data('libroid');
            // Aquí puedes hacer la lógica para actualizar la cantidad en el carrito
            console.log("Actualizar cantidad de libro con ID: " + libroId + " a " + cantidad);
            });

            // Seleccionar productos para comprar
            $('#form-carrito').on('submit', function (e) {
                var seleccionados = $('.producto-seleccionado:checked').length;
            if (seleccionados === 0) {
                e.preventDefault();
            alert('Por favor selecciona al menos un producto.');
                }
            });
        });