-- =====================================================
-- SCRIPT: VERIFICAR Y CREAR DATOS BÁSICOS NECESARIOS
-- Propósito: Asegurar que existan los registros mínimos 
--           para que funcione la creación de empleados
-- =====================================================

USE EmplaniappBD;
GO

PRINT '🔍 VERIFICANDO DATOS BÁSICOS NECESARIOS...';
PRINT '';

-- =====================================================
-- 1. VERIFICAR Y CREAR ESTADOS
-- =====================================================
PRINT '📊 Verificando Estados...';
IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 1)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (1, 'Activo');
    PRINT '✅ Estado "Activo" creado con ID = 1';
END
ELSE
    PRINT '✅ Estado con ID = 1 ya existe';

IF NOT EXISTS (SELECT 1 FROM Estado WHERE idEstado = 2)
BEGIN
    INSERT INTO Estado (idEstado, nombreEstado) VALUES (2, 'Inactivo');
    PRINT '✅ Estado "Inactivo" creado con ID = 2';
END
ELSE
    PRINT '✅ Estado con ID = 2 ya existe';

-- =====================================================
-- 2. VERIFICAR Y CREAR UBICACIÓN GEOGRÁFICA
-- =====================================================
PRINT '';
PRINT '🗺️ Verificando ubicación geográfica...';

-- Provincia
IF NOT EXISTS (SELECT 1 FROM Provincia WHERE idProvincia = 1)
BEGIN
    INSERT INTO Provincia (idProvincia, nombreProvincia) VALUES (1, 'San José');
    PRINT '✅ Provincia "San José" creada con ID = 1';
END
ELSE
    PRINT '✅ Provincia con ID = 1 ya existe';

-- Canton
IF NOT EXISTS (SELECT 1 FROM Canton WHERE idCanton = 1)
BEGIN
    INSERT INTO Canton (idCanton, nombreCanton, idProvincia) VALUES (1, 'San José', 1);
    PRINT '✅ Canton "San José" creado con ID = 1';
END
ELSE
    PRINT '✅ Canton con ID = 1 ya existe';

-- Distrito
IF NOT EXISTS (SELECT 1 FROM Distrito WHERE idDistrito = 1)
BEGIN
    INSERT INTO Distrito (idDistrito, nombreDistrito, idCanton) VALUES (1, 'Carmen', 1);
    PRINT '✅ Distrito "Carmen" creado con ID = 1';
END
ELSE
    PRINT '✅ Distrito con ID = 1 ya existe';

-- Calle
IF NOT EXISTS (SELECT 1 FROM Calle WHERE idCalle = 1)
BEGIN
    INSERT INTO Calle (idCalle, nombreCalle, idDistrito) VALUES (1, 'Calle Central', 1);
    PRINT '✅ Calle "Calle Central" creada con ID = 1';
END
ELSE
    PRINT '✅ Calle con ID = 1 ya existe';

-- NOTA: La tabla Direccion existe en BD pero no se usa en el modelo Empleados
-- El modelo usa campos separados: idProvincia, idCanton, idDistrito, idCalle
-- No es necesario crear registros en Direccion para el funcionamiento del sistema

-- =====================================================
-- 3. VERIFICAR Y CREAR DATOS LABORALES
-- =====================================================
PRINT '';
PRINT '💼 Verificando datos laborales...';

-- Número de Ocupación
IF NOT EXISTS (SELECT 1 FROM NumeroOcupacion WHERE idNumeroOcupacion = 1)
BEGIN
    INSERT INTO NumeroOcupacion (idNumeroOcupacion, numeroOcupacion) VALUES (1, 1001);
    PRINT '✅ Número de ocupación creado con ID = 1';
END
ELSE
    PRINT '✅ Número de ocupación con ID = 1 ya existe';

-- Cargo
IF NOT EXISTS (SELECT 1 FROM Cargos WHERE idCargo = 1)
BEGIN
    INSERT INTO Cargos (idCargo, nombreCargo, idNumeroOcupacion) VALUES (1, 'Empleado General', 1);
    PRINT '✅ Cargo "Empleado General" creado con ID = 1';
END
ELSE
    PRINT '✅ Cargo con ID = 1 ya existe';

-- =====================================================
-- 4. VERIFICAR Y CREAR DATOS FINANCIEROS
-- =====================================================
PRINT '';
PRINT '💰 Verificando datos financieros...';

-- Tipo de Moneda
IF NOT EXISTS (SELECT 1 FROM TipoMoneda WHERE idTipoMoneda = 1)
BEGIN
    INSERT INTO TipoMoneda (nombreMoneda) VALUES ('Colón Costarricense');
    PRINT '✅ Moneda "Colón Costarricense" creada';
END
ELSE
    PRINT '✅ Tipo de moneda con ID = 1 ya existe';

-- Banco
IF NOT EXISTS (SELECT 1 FROM Bancos WHERE idBanco = 1)
BEGIN
    INSERT INTO Bancos (nombreBanco) VALUES ('Banco Nacional de Costa Rica');
    PRINT '✅ Banco "Banco Nacional de Costa Rica" creado';
END
ELSE
    PRINT '✅ Banco con ID = 1 ya existe';

-- =====================================================
-- 5. VERIFICAR Y CREAR TIPOS DE REMUNERACIÓN/RETENCIÓN
-- =====================================================
PRINT '';
PRINT '📋 Verificando tipos de remuneración y retención...';

-- Tipos de Remuneración
IF NOT EXISTS (SELECT 1 FROM TipoRemuneracion WHERE nombreTipoRemuneracion = 'Horas Extra')
BEGIN
    INSERT INTO TipoRemuneracion (nombreTipoRemuneracion, porcentajeRemuneracion, idEstado) 
    VALUES ('Horas Extra', 1.5, 1);
    PRINT '✅ Tipo remuneración "Horas Extra" creado';
END
ELSE
    PRINT '✅ Tipo remuneración "Horas Extra" ya existe';

IF NOT EXISTS (SELECT 1 FROM TipoRemuneracion WHERE nombreTipoRemuneracion = 'Pago Quincenal')
BEGIN
    INSERT INTO TipoRemuneracion (nombreTipoRemuneracion, porcentajeRemuneracion, idEstado) 
    VALUES ('Pago Quincenal', 1.0, 1);
    PRINT '✅ Tipo remuneración "Pago Quincenal" creado';
END
ELSE
    PRINT '✅ Tipo remuneración "Pago Quincenal" ya existe';

-- Tipos de Retención básicos
IF NOT EXISTS (SELECT 1 FROM TipoRetenciones WHERE nombreTipoRetencio = 'CCSS Empleado')
BEGIN
    INSERT INTO TipoRetenciones (nombreTipoRetencio, porcentajeRetencion, idEstado) 
    VALUES ('CCSS Empleado', 10.5, 1);
    PRINT '✅ Tipo retención "CCSS Empleado" creado';
END
ELSE
    PRINT '✅ Tipo retención "CCSS Empleado" ya existe';

IF NOT EXISTS (SELECT 1 FROM TipoRetenciones WHERE nombreTipoRetencio = 'Impuesto Renta')
BEGIN
    INSERT INTO TipoRetenciones (nombreTipoRetencio, porcentajeRetencion, idEstado) 
    VALUES ('Impuesto Renta', 10.0, 1);
    PRINT '✅ Tipo retención "Impuesto Renta" creado';
END
ELSE
    PRINT '✅ Tipo retención "Impuesto Renta" ya existe';

-- =====================================================
-- 6. VERIFICAR INTEGRIDAD DE DATOS
-- =====================================================
PRINT '';
PRINT '🔍 VERIFICANDO INTEGRIDAD DE DATOS...';
PRINT '';

-- Contar registros en tablas críticas
DECLARE @EstadosCount INT, @ProvinciasCount INT, @CantonesCount INT, @DistritosCount INT;
DECLARE @CallesCount INT, @CargosCount INT, @BancosCount INT, @TipoMonedasCount INT;

SELECT @EstadosCount = COUNT(*) FROM Estado;
SELECT @ProvinciasCount = COUNT(*) FROM Provincia;
SELECT @CantonesCount = COUNT(*) FROM Canton;
SELECT @DistritosCount = COUNT(*) FROM Distrito;
SELECT @CallesCount = COUNT(*) FROM Calle;
SELECT @CargosCount = COUNT(*) FROM Cargos;
SELECT @BancosCount = COUNT(*) FROM Bancos;
SELECT @TipoMonedasCount = COUNT(*) FROM TipoMoneda;

PRINT '📊 RESUMEN DE DATOS BÁSICOS:';
PRINT '   • Estados: ' + CAST(@EstadosCount AS VARCHAR(10));
PRINT '   • Provincias: ' + CAST(@ProvinciasCount AS VARCHAR(10));
PRINT '   • Cantones: ' + CAST(@CantonesCount AS VARCHAR(10));
PRINT '   • Distritos: ' + CAST(@DistritosCount AS VARCHAR(10));
PRINT '   • Calles: ' + CAST(@CallesCount AS VARCHAR(10));
PRINT '   • Cargos: ' + CAST(@CargosCount AS VARCHAR(10));
PRINT '   • Bancos: ' + CAST(@BancosCount AS VARCHAR(10));
PRINT '   • Tipos de Moneda: ' + CAST(@TipoMonedasCount AS VARCHAR(10));

-- Verificar que existan los registros críticos con ID = 1
IF EXISTS (
    SELECT 1 FROM Estado WHERE idEstado = 1
) AND EXISTS (
    SELECT 1 FROM Provincia WHERE idProvincia = 1  
) AND EXISTS (
    SELECT 1 FROM Canton WHERE idCanton = 1
) AND EXISTS (
    SELECT 1 FROM Distrito WHERE idDistrito = 1
) AND EXISTS (
    SELECT 1 FROM Calle WHERE idCalle = 1
) AND EXISTS (
    SELECT 1 FROM Cargos WHERE idCargo = 1
) AND EXISTS (
    SELECT 1 FROM Bancos WHERE idBanco = 1
) AND EXISTS (
    SELECT 1 FROM TipoMoneda WHERE idTipoMoneda = 1
)
BEGIN
    PRINT '';
    PRINT '✅ ¡ÉXITO! Todos los datos básicos necesarios están presentes.';
    PRINT '✅ El sistema debería poder crear empleados sin problemas.';
END
ELSE
BEGIN
    PRINT '';
    PRINT '❌ ADVERTENCIA: Faltan algunos datos básicos críticos.';
    PRINT '❌ Revisa los mensajes anteriores para identificar qué falta.';
END

PRINT '';
PRINT '🎯 SCRIPT COMPLETADO.';
PRINT '📋 Si el sistema sigue fallando, revisa los logs de debugging en Visual Studio.';
GO 