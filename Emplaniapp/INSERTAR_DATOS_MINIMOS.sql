-- ✅ SCRIPT: Insertar datos mínimos para funcionamiento de EmplaniApp
-- Descripción: Datos básicos requeridos para que la aplicación funcione correctamente
-- Fecha: 2025

USE EmplaniappBD
GO

PRINT '🚀 ==============================================';
PRINT '🚀 INICIANDO INSERCIÓN DE DATOS MÍNIMOS';
PRINT '🚀 ==============================================';

-- ==============================================
-- PROVINCIAS (Costa Rica)
-- ==============================================
PRINT '📍 Verificando Provincias...';

IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 1)
BEGIN
    INSERT INTO Provincia (idProvincia, nombreProvincia) VALUES (1, 'San José');
    PRINT '✅ Provincia San José insertada con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 2)
BEGIN
    INSERT INTO Provincia (idProvincia, nombreProvincia) VALUES (2, 'Alajuela');
    PRINT '✅ Provincia Alajuela insertada con ID = 2';
END

IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 3)
BEGIN
    INSERT INTO Provincia (idProvincia, nombreProvincia) VALUES (3, 'Cartago');
    PRINT '✅ Provincia Cartago insertada con ID = 3';
END

-- ==============================================
-- CANTONES (Algunos de ejemplo)
-- ==============================================
PRINT '🏘️ Verificando Cantones...';

IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 1)
BEGIN
    INSERT INTO Canton (idCanton, nombreCanton, idProvincia) VALUES (1, 'San José', 1);
    PRINT '✅ Cantón San José insertado con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 2)
BEGIN
    INSERT INTO Canton (idCanton, nombreCanton, idProvincia) VALUES (2, 'Escazú', 1);
    PRINT '✅ Cantón Escazú insertado con ID = 2';
END

IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 3)
BEGIN
    INSERT INTO Canton (idCanton, nombreCanton, idProvincia) VALUES (3, 'Desamparados', 1);
    PRINT '✅ Cantón Desamparados insertado con ID = 3';
END

-- ==============================================
-- DISTRITOS (Algunos de ejemplo)
-- ==============================================
PRINT '🏡 Verificando Distritos...';

IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 1)
BEGIN
    INSERT INTO Distrito (idDistrito, nombreDistrito, idCanton) VALUES (1, 'Carmen', 1);
    PRINT '✅ Distrito Carmen insertado con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 2)
BEGIN
    INSERT INTO Distrito (idDistrito, nombreDistrito, idCanton) VALUES (2, 'Catedral', 1);
    PRINT '✅ Distrito Catedral insertado con ID = 2';
END

IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 3)
BEGIN
    INSERT INTO Distrito (idDistrito, nombreDistrito, idCanton) VALUES (3, 'San Miguel', 3);
    PRINT '✅ Distrito San Miguel insertado con ID = 3';
END

-- ==============================================
-- CALLES (Algunas de ejemplo)
-- ==============================================
PRINT '🛣️ Verificando Calles...';

IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 1)
BEGIN
    INSERT INTO Calle (idCalle, nombreCalle, idDistrito) VALUES (1, 'Avenida Central', 1);
    PRINT '✅ Calle Avenida Central insertada con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 2)
BEGIN
    INSERT INTO Calle (idCalle, nombreCalle, idDistrito) VALUES (2, 'Calle 2', 1);
    PRINT '✅ Calle 2 insertada con ID = 2';
END

IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 3)
BEGIN
    INSERT INTO Calle (idCalle, nombreCalle, idDistrito) VALUES (3, 'Avenida Segunda', 2);
    PRINT '✅ Calle Avenida Segunda insertada con ID = 3';
END

-- ==============================================
-- ESTADO: Estados básicos para empleados
-- ==============================================
PRINT '📊 Verificando Estados...';

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 1)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (1, 'Activo');
    PRINT '✅ Estado Activo insertado con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 2)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (2, 'Inactivo');
    PRINT '✅ Estado Inactivo insertado con ID = 2';
END

-- ==============================================
-- CARGOS: Cargos básicos para empleados
-- ==============================================
PRINT '💼 Verificando Cargos...';

IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 1)
BEGIN
    INSERT INTO NumeroOcupacion (idNumeroOcupacion, numeroOcupacion) VALUES (1, 9832);
    PRINT '✅ Número ocupación 9832 insertado con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 1)
BEGIN
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (1, 'Desarrollador de Software', 1);
    PRINT '✅ Cargo Desarrollador de Software insertado con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 2)
BEGIN
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (2, 'Analista de Sistemas', 1);
    PRINT '✅ Cargo Analista de Sistemas insertado con ID = 2';
END

-- ==============================================
-- TIPO MONEDA: Monedas básicas
-- ==============================================
PRINT '💰 Verificando Monedas...';

IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE idTipoMoneda = 1)
BEGIN
    INSERT INTO TipoMoneda (idTipoMoneda, nombreMoneda) VALUES (1, 'Colones (CRC)');
    PRINT '✅ Moneda Colones insertada con ID = 1';
END

IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE idTipoMoneda = 2)
BEGIN
    INSERT INTO TipoMoneda (idTipoMoneda, nombreMoneda) VALUES (2, 'Dólares (USD)');
    PRINT '✅ Moneda Dólares insertada con ID = 2';
END

-- ==============================================
-- BANCOS: Bancos básicos de Costa Rica
-- ==============================================
PRINT '🏦 Verificando Bancos...';

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 1)
BEGIN
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco Nacional de Costa Rica');
    PRINT '✅ Banco Nacional insertado';
END

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 2)
BEGIN
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco de Costa Rica');
    PRINT '✅ Banco de Costa Rica insertado';
END

IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 3)
BEGIN
    INSERT INTO Bancos (nombreBanco) VALUES ('BAC San José');
    PRINT '✅ BAC San José insertado';
END

-- ==============================================
-- DIRECCIÓN: Dirección por defecto (REQUERIDO para empleados)
-- ==============================================
PRINT '🏠 Verificando Direcciones...';

IF NOT EXISTS (SELECT 1 FROM Direccion WHERE idDireccion = 1)
BEGIN
    INSERT INTO Direccion (idDireccion, idProvincia, idCanton, idDistrito, idCalle) 
    VALUES (1, 1, 1, 1, 1);
    PRINT '✅ Dirección por defecto insertada con ID = 1';
END

-- ==============================================
-- VERIFICACIÓN FINAL
-- ==============================================
PRINT '🔍 ==============================================';
PRINT '🔍 VERIFICACIÓN FINAL DE DATOS';
PRINT '🔍 ==============================================';

-- Contar registros en cada tabla
DECLARE @countProvincias INT = (SELECT COUNT(*) FROM Provincia);
DECLARE @countCantones INT = (SELECT COUNT(*) FROM Canton);
DECLARE @countDistritos INT = (SELECT COUNT(*) FROM Distrito);
DECLARE @countCalles INT = (SELECT COUNT(*) FROM Calle);
DECLARE @countEstados INT = (SELECT COUNT(*) FROM Estado);
DECLARE @countCargos INT = (SELECT COUNT(*) FROM Cargos);
DECLARE @countMonedas INT = (SELECT COUNT(*) FROM TipoMoneda);
DECLARE @countBancos INT = (SELECT COUNT(*) FROM Bancos);
DECLARE @countDirecciones INT = (SELECT COUNT(*) FROM Direccion);

PRINT '📊 Resumen de datos insertados:';
PRINT '   • Provincias: ' + CAST(@countProvincias AS NVARCHAR(10));
PRINT '   • Cantones: ' + CAST(@countCantones AS NVARCHAR(10));
PRINT '   • Distritos: ' + CAST(@countDistritos AS NVARCHAR(10));
PRINT '   • Calles: ' + CAST(@countCalles AS NVARCHAR(10));
PRINT '   • Estados: ' + CAST(@countEstados AS NVARCHAR(10));
PRINT '   • Cargos: ' + CAST(@countCargos AS NVARCHAR(10));
PRINT '   • Monedas: ' + CAST(@countMonedas AS NVARCHAR(10));
PRINT '   • Bancos: ' + CAST(@countBancos AS NVARCHAR(10));
PRINT '   • Direcciones: ' + CAST(@countDirecciones AS NVARCHAR(10));

PRINT '✅ ==============================================';
PRINT '✅ INSERCIÓN COMPLETADA EXITOSAMENTE';
PRINT '✅ La aplicación EmplaniApp está lista para usar';
PRINT '✅ ==============================================';

GO 