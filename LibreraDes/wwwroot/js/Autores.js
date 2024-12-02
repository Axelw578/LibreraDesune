// Función para abrir el modal de creación
function openCreateModal() {
    const createModal = new bootstrap.Modal(document.getElementById('createModal'));
    createModal.show();
}

// Función para abrir el modal de edición con datos prellenados
function openEditModal(id, nombre, fechaNacimiento) {
    $('#editId').val(id);
    $('#editName').val(nombre);
    $('#editDate').val(fechaNacimiento);

    const editModal = new bootstrap.Modal(document.getElementById('editModal'));
    editModal.show();
}

// Función para crear un autor
function createAutor() {
    const nombre = $('#createName').val().trim();
    const fechaNacimiento = $('#createDate').val().trim();

    if (!nombre || !fechaNacimiento) {
        $('#createAlert').removeClass('d-none').text('Todos los campos son obligatorios.');
        return;
    }

    $.ajax({
        url: '/Autores/Create',
        type: 'POST',
        data: {
            Nombre: nombre,
            FechaNacimiento: fechaNacimiento,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            alert('Autor creado con éxito');
            location.reload(); // Recargar la página para ver el nuevo autor
        },
        error: function (xhr) {
            $('#createAlert').removeClass('d-none').text(xhr.responseJSON?.message || 'Hubo un error al crear el autor.');
        }
    });
}

// Función para actualizar un autor
function updateAutor() {
    const id = $('#editId').val();
    const nombre = $('#editName').val().trim();
    const fechaNacimiento = $('#editDate').val().trim();

    if (!nombre || !fechaNacimiento) {
        $('#editAlert').removeClass('d-none').text('Todos los campos son obligatorios.');
        return;
    }

    $.ajax({
        url: `/Autores/Edit/${id}`,
        type: 'POST',
        data: {
            Id: id,
            Nombre: nombre,
            FechaNacimiento: fechaNacimiento,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            alert('Autor actualizado con éxito');
            location.reload(); // Recargar la página para ver los cambios
        },
        error: function (xhr) {
            $('#editAlert').removeClass('d-none').text(xhr.responseJSON?.message || 'Hubo un error al actualizar el autor.');
        }
    });
}

// Función para confirmar la eliminación de un autor
function confirmDelete(id) {
    if (confirm("¿Estás seguro de que deseas eliminar este autor? Esta acción no se puede deshacer.")) {
        $.ajax({
            url: `/Autores/DeleteConfirmed/${id}`,
            type: 'POST',
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                alert('Autor eliminado con éxito');
                location.reload(); // Recargar la página después de la eliminación
            },
            error: function (xhr) {
                alert(xhr.responseJSON?.message || "Hubo un error al intentar eliminar el autor.");
            }
        });
    }
}
