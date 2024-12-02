// Mostrar mensaje de alerta dentro del modal correspondiente
function mostrarMensajeModal(modalId, mensaje, tipo = 'success') {
    const alertBox = document.querySelector(`${modalId} .modal-alert`);
    alertBox.classList.remove('d-none', 'alert-success', 'alert-danger');
    alertBox.classList.add(`alert-${tipo}`);
    alertBox.textContent = mensaje;

    // Animación de aparición y desaparición
    alertBox.style.opacity = 1;
    setTimeout(() => {
        alertBox.style.transition = 'opacity 0.5s';
        alertBox.style.opacity = 0;
        setTimeout(() => {
            alertBox.classList.add('d-none');
            alertBox.style.transition = '';
        }, 500);
    }, 3000);
}

// Abrir modal para crear una nueva categoría
function openCreateModal() {
    $('#createModal').modal('show');
    document.getElementById('createForm').reset();
    document.querySelector('#createModal .modal-alert').classList.add('d-none');
}

// Crear una nueva categoría
async function createCategoria() {
    const nombre = document.getElementById('createName').value.trim();
    const descripcion = document.getElementById('createDescription').value.trim();

    if (!nombre || !descripcion) {
        mostrarMensajeModal('#createModal', 'Todos los campos son obligatorios', 'danger');
        return;
    }

    try {
        const response = await fetch('/Categorias/Create', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nombre, descripcion })
        });

        if (response.ok) {
            mostrarMensajeModal('#createModal', 'Categoría creada con éxito');
            setTimeout(() => {
                $('#createModal').modal('hide');
                location.reload();
            }, 1500);
        } else {
            throw new Error('Error al crear la categoría');
        }
    } catch (error) {
        mostrarMensajeModal('#createModal', error.message, 'danger');
    }
}

// Abrir modal para editar una categoría existente
function openEditModal(id, nombre, descripcion) {
    $('#editModal').modal('show');
    document.getElementById('editId').value = id;
    document.getElementById('editName').value = nombre;
    document.getElementById('editDescription').value = descripcion;
    document.querySelector('#editModal .modal-alert').classList.add('d-none');
}

// Actualizar una categoría
async function updateCategoria() {
    const id = document.getElementById('editId').value;
    const nombre = document.getElementById('editName').value.trim();
    const descripcion = document.getElementById('editDescription').value.trim();

    if (!nombre || !descripcion) {
        mostrarMensajeModal('#editModal', 'Todos los campos son obligatorios', 'danger');
        return;
    }

    try {
        const response = await fetch(`/Categorias/Edit/${id}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id, nombre, descripcion })
        });

        if (response.ok) {
            mostrarMensajeModal('#editModal', 'Categoría actualizada con éxito');
            setTimeout(() => {
                $('#editModal').modal('hide');
                location.reload();
            }, 1500);
        } else {
            throw new Error('Error al actualizar la categoría');
        }
    } catch (error) {
        mostrarMensajeModal('#editModal', error.message, 'danger');
    }
}

// Confirmar eliminación de una categoría
function confirmDelete(id) {
    if (confirm('¿Está seguro de que desea eliminar esta categoría?')) {
        deleteCategoria(id);
    }
}

// Eliminar una categoría
async function deleteCategoria(id) {
    try {
        const response = await fetch(`/Categorias/Delete/${id}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        });

        if (response.ok) {
            mostrarMensajeModal('body', 'Categoría eliminada con éxito');
            location.reload();
        } else {
            throw new Error('Error al eliminar la categoría');
        }
    } catch (error) {
        mostrarMensajeModal('body', error.message, 'danger');
    }
}
