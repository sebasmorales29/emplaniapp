-- Script para verificar y crear datos básicos necesarios para EmplaniApp
USE EmplaniappBD;

-- Verificar Estados existentes
SELECT 'Estados' as Tabla, * FROM Estado;

-- Verificar Cargos existentes  
SELECT 'Cargos' as Tabla, * FROM Cargos;

-- Verificar si existe dirección con ID 1
SELECT 'Direcciones' as Tabla, * FROM Direccion WHERE idDireccion = 1;

-- Crear dirección básica si no existe
IF NOT EXISTS (SELECT 1 FROM Direccion WHERE idDireccion = 1)
BEGIN
    -- Insertar provincia si no existe
    IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 1)
        INSERT INTO Provincia (idProvincia, nombreProvincia) VALUES (1, 'San José');
    
    -- Insertar cantón si no existe
    IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 1)
        INSERT INTO Canton (idCanton, nombreCanton, idProvincia) VALUES (1, 'San José', 1);
    
    -- Insertar distrito si no existe
    IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 1)
        INSERT INTO Distrito (idDistrito, nombreDistrito, idCanton) VALUES (1, 'Carmen', 1);
    
    -- Insertar calle si no existe
    IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 1)
        INSERT INTO Calle (idCalle, nombreCalle, idDistrito) VALUES (1, 'Avenida Central', 1);
    
    -- Insertar dirección
    INSERT INTO Direccion (idDireccion, idProvincia, idCanton, idDistrito, idCalle) 
    VALUES (1, 1, 1, 1, 1);
    
    PRINT 'Dirección básica creada';
END

-- Verificar empleados existentes para ver el patrón de IDs
SELECT 'Empleados Existentes' as Info, COUNT(*) as Total, MAX(idEmpleado) as MaxID FROM Empleado;

-- Mostrar estructura de tabla Empleado para verificar columnas
SELECT 'Columnas de Empleado' as Info, COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Empleado'
ORDER BY ORDINAL_POSITION; 