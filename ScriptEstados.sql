-- Script para insertar estados básicos en la tabla Estado
-- Ejecutar este script en la base de datos EmplaniappBD

USE EmplaniappBD;
GO

-- Verificar si los estados ya existen antes de insertarlos
IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 1)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (1, 'Activo');
END

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 2)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (2, 'Inactivo');
END

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 3)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (3, 'En Licencia');
END

-- Verificar que los estados se insertaron correctamente
SELECT * FROM Estado ORDER BY idEstado;
GO 