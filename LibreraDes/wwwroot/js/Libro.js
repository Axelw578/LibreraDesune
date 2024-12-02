$(document).ready(function () {
    console.log("jQuery cargado");

    // Obtener categorías y autores al cargar el documento
    function cargarCategoriasAutores() {
        $.get('/Libros/ObtenerCategorias', function (categorias) {
            const selectCategorias = $('#selectCategoria');
            selectCategorias.empty(); // Limpiar opciones existentes
            $.each(categorias, function (index, categoria) {
                selectCategorias.append($('<option></option>').attr('value', categoria.Id).text(categoria.Nombre));
            });
        }).fail(function () {
            showMessage('error', 'Error al cargar las categorías.');
        });

        $.get('/Libros/ObtenerAutores', function (autores) {
            const selectAutores = $('#selectAutor');
            selectAutores.empty(); // Limpiar opciones existentes
            $.each(autores, function (index, autor) {
                selectAutores.append($('<option></option>').attr('value', autor.Id).text(autor.Nombre));
            });
        }).fail(function () {
            showMessage('error', 'Error al cargar los autores.');
        });
    }

    cargarCategoriasAutores(); // Llamar a la función al cargar el documento

    // Función para eliminar libro
    $(document).on('click', '.eliminar-producto', function () {
        console.log("Botón eliminar presionado");
        let libroId = $(this).data('libroid');

        $.post('/Carrito/EliminarDelCarrito', { libroId: libroId }, function (response) {
            if (response.success) {
                $('button[data-libroid="' + libroId + '"]').closest('tr').remove();
                actualizarTotal();
                actualizarCarritoCount();
            } else {
                showMessage('error', 'Hubo un error al eliminar el producto.');
            }
        }).fail(function () {
            showMessage('error', 'Error al eliminar el producto.');
        });
    });

    // Función para actualizar cantidad usando delegación de eventos
    $(document).on('change', '.cantidad-input', function () {
        console.log("Cantidad actualizada");
        let libroId = $(this).data('libroid');
        let cantidad = $(this).val();

        if (cantidad < 1) {
            cantidad = 1;
            $(this).val(cantidad);
        }

        $.post('/Carrito/ActualizarCantidad', { libroId: libroId, cantidad: cantidad }, function (response) {
            if (response.success) {
                let fila = $('input[data-libroid="' + libroId + '"]').closest('tr');
                let precioUnitario = parseFloat(fila.find('td:eq(3)').text().replace('$', '').replace(',', ''));
                let nuevoSubtotal = precioUnitario * cantidad;
                fila.find('.subtotal').text('$' + nuevoSubtotal.toFixed(2));
                actualizarTotal();
                actualizarCarritoCount();
            } else {
                showMessage('error', 'Hubo un error al actualizar la cantidad.');
            }
        }).fail(function () {
            showMessage('error', 'Error al actualizar la cantidad.');
        });
    });

    // Evento para procesar la compra usando delegación
    $('#realizar-pedido').on('click', function (e) {
        e.preventDefault();
        console.log("Realizar pedido presionado");

        let seleccionados = $('.producto-seleccionado:checked');
        if (seleccionados.length === 0) {
            showMessage('error', 'Selecciona al menos un producto para realizar la compra.');
        } else {
            let productosSeleccionados = seleccionados.map(function () {
                return $(this).val();
            }).get();

            $.ajax({
                type: 'POST',
                url: '/Carrito/ProcesarCompra',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(productosSeleccionados),
                success: function (response) {
                    if (response.success) {
                        showMessage('success', response.message || 'Compra realizada con éxito.');
                        seleccionados.each(function () {
                            $(this).closest('tr').remove();
                        });
                        actualizarTotal();
                        actualizarCarritoCount();
                    } else {
                        showMessage('error', response.message || 'Hubo un error al procesar la compra.');
                    }
                },
                error: function (xhr, status, error) {
                    showMessage('error', 'Error de conexión: ' + error);
                }
            });
        }
    });
});
