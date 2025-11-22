// ✨ JavaScript para CrearEmpleado.cshtml - Movido externamente para evitar conflictos con Razor

// Función de validación de email
function isValidEmailFormat(email) {
    return email.indexOf('@') !== -1 && email.indexOf('.') !== -1;
}

$(document).ready(function () {
    console.log('🔍 DIAGNÓSTICO: jQuery cargado correctamente. Inicializando funcionalidad del formulario...');
    
    // Lógica para mostrar/ocultar contraseña
    $('.toggle-password').click(function () {
        $(this).toggleClass('fa-eye fa-eye-slash');
        var input = $(this).closest('.password-wrapper').find('input');
        input.attr('type', input.attr('type') === 'password' ? 'text' : 'password');
    });
    
    // ✨ MEJORA: Verificar si el usuario es administrador
    var isAdmin = window.userIsAdmin || false;
    console.log('🔍 DIAGNÓSTICO: Usuario es admin:', isAdmin);
    
    // Manejar envío del formulario
    $('#createEmployeeForm').on('submit', function (e) {
        console.log('🔍 DIAGNÓSTICO: Formulario se está enviando...');
        console.log('🔍 DIAGNÓSTICO: isAdmin:', isAdmin);
        
        // ✨ CORRECCIÓN: Validar formulario antes de mostrar modal de admin
        var esFormularioValido = true;

        // Verificar que la cédula no sean 0
        var identificacion = $('[name="cedula"]').val();
        if (identificacion === 0) {
            console.log('🔍 DIAGNÓSTICO: Identificación vacía');
            esFormularioValido = false;
            $('[name="cedula"]').addClass('is-invalid');
        }

        // Verificar campos requeridos
        var camposRequeridos = ['nombre', 'primerApellido', 'cedula', 'numeroTelefonico', 'correoInstitucional', 'UserName', 'Password', 'ConfirmPassword', 'idCargo', 'fechaContratacion', 'salarioAprobado', 'periocidadPago', 'idMoneda', 'cuentaIBAN', 'idBanco'];
        camposRequeridos.forEach(function(campo) {
            var input = $('[name="' + campo + '"]');
            if (input.length > 0 && (!input.val() || input.val().trim() === '')) {
                console.log('🔍 DIAGNÓSTICO: Campo requerido vacío:', campo);
                esFormularioValido = false;
                input.addClass('is-invalid');
            }
        });
        
        // Verificar dropdowns requeridos
        var provincia = $('[name="idProvincia"]').val();
        if (!provincia || provincia === '') {
            console.log('🔍 DIAGNÓSTICO: Provincia no seleccionada');
            esFormularioValido = false;
            $('[name="idProvincia"]').addClass('is-invalid');
        }
        
        if (!esFormularioValido) {
            console.log('🔍 DIAGNÓSTICO: Formulario no es válido, no se puede enviar');
            e.preventDefault();
            alert('Por favor, complete todos los campos requeridos antes de continuar.');
            return false;
        }
        
        console.log('🔍 DIAGNÓSTICO: Formulario es válido, procediendo...');
        
        if (isAdmin) {
            console.log('🔍 DIAGNÓSTICO: Usuario es admin y formulario válido, mostrando modal...');
            e.preventDefault();
            if (window.AdminPasswordModal && window.AdminPasswordModal.show) {
                window.AdminPasswordModal.show();
            } else {
                $('#adminPasswordInput').val('');
                $('#adminPasswordError').hide();
                $('#adminPasswordModal').modal('show');
            }
        } else {
            console.log('🔍 DIAGNÓSTICO: Usuario no es admin, enviando formulario directamente...');
        }
    });

    $('#confirmAdminPasswordBtn').click(function () {
        console.log('🔍 DIAGNÓSTICO: Botón de confirmación de admin presionado');
        var password = $('#adminPasswordInput').val();
        console.log('🔍 DIAGNÓSTICO: Contraseña ingresada:', password ? '***' : 'VACÍA');
        
        if (!password) {
            console.log('🔍 DIAGNÓSTICO: Contraseña vacía, mostrando error');
            $('#adminPasswordError').text('La contraseña no puede estar vacía.').show();
            return;
        }
        var token = $('input[name="__RequestVerificationToken"]').val();

        console.log('🔍 DIAGNÓSTICO: Enviando AJAX para validar contraseña de admin...');
        $.ajax({
            url: window.validateAdminPasswordUrl,
            type: 'POST',
            dataType: 'json',
            data: {
                password: password,
                __RequestVerificationToken: token
            },
            success: function (response) {
                console.log('🔍 DIAGNÓSTICO: Respuesta AJAX recibida:', response);
                if (response.success) {
                    console.log('🔍 DIAGNÓSTICO: Contraseña válida, enviando formulario...');
                    $('#adminPasswordModal').modal('hide');
                    var form = $('#createEmployeeForm');
                    if (form.length > 0) {
                        form.off('submit').submit();
                    } else {
                        console.log('🔍 DIAGNÓSTICO: Contraseña inválida, mostrando error');
                        $('#adminPasswordError').text('Contraseña incorrecta.').show();
                    }
                }
            },
            error: function () {
                alert('Ocurrió un error al validar la contraseña.');
                $('#adminPasswordModal').modal('hide');
            }
        });
    });

    $('#cancelAdminPasswordBtn').click(function () {
        $('#adminPasswordModal').modal('hide');
    });

    // ✨ NUEVO: Validación en tiempo real del nombre de usuario
    var timeoutId;
    $('#UserName').on('input', function() {
        var username = $(this).val();
        var input = $(this);
        
        // Limpiar timeout anterior
        clearTimeout(timeoutId);
        
        // Si está vacío, no validar
        if (!username || username.trim() === '') {
            input.removeClass('is-valid is-invalid');
            $('#username-feedback').remove();
            return;
        }
        
        // Esperar 500ms antes de validar (para evitar muchas consultas)
        timeoutId = setTimeout(function() {
            $.ajax({
                url: window.verifyUsernameUrl,
                type: 'POST',
                data: { username: username },
                success: function(response) {
                    $('#username-feedback').remove();
                    if (response.existe) {
                        input.addClass('is-invalid');
                        input.after('<div id="username-feedback" class="invalid-feedback">Este nombre de usuario ya está siendo utilizado.</div>');
                    } else {
                        input.addClass('is-valid');
                        input.after('<div id="username-feedback" class="valid-feedback">Nombre de usuario disponible.</div>');
                    }
                },
                error: function() {
                    $('#username-feedback').remove();
                    input.removeClass('is-valid is-invalid');
                }
            });
        }, 500);
    });
    
    // ✨ NUEVO: Validación en tiempo real del correo electrónico
    var emailTimeoutId;
    $('#correoInstitucional').on('input', function() {
        var email = $(this).val();
        var input = $(this);
        
        // Limpiar timeout anterior
        clearTimeout(emailTimeoutId);
        
        // Si está vacío, no validar
        if (!email || email.trim() === '') {
            input.removeClass('is-valid is-invalid');
            $('#email-feedback').remove();
            return;
        }
        
        // Validar formato de email básico
        if (!isValidEmailFormat(email)) {
            input.removeClass('is-valid is-invalid');
            $('#email-feedback').remove();
            return;
        }
        
        // Esperar 500ms antes de validar
        emailTimeoutId = setTimeout(function() {
            $.ajax({
                url: window.verifyEmailUrl,
                type: 'POST',
                data: { email: email },
                success: function(response) {
                    $('#email-feedback').remove();
                    if (response.existe) {
                        input.addClass('is-invalid');
                        input.after('<div id="email-feedback" class="invalid-feedback">Este correo electrónico ya está siendo utilizado.</div>');
                    } else {
                        input.addClass('is-valid');
                        input.after('<div id="email-feedback" class="valid-feedback">Correo electrónico disponible.</div>');
                    }
                },
                error: function() {
                    $('#email-feedback').remove();
                    input.removeClass('is-valid is-invalid');
                }
            });
        }, 500);
    });
});
