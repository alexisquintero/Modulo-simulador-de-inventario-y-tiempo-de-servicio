Data we need:
   *Inventario
      -Por cada producto en la venta
         .Id producto
         .Cantidad vendida
	 .Fecha
   *Tiempo de servicio
      -Por cada venta
         .Id empleado
	 .Id centro de atención(ej: caja rápida)
	 .Fecha
	 .Hora inicio transacción
	 .Hora fin transacción
   *Reportes
      -Inventario (Por cada producto)
         .Id producto
	 .Unidad
	 .Fecha de vencimiento
	 .Stock
	 .Precio de compra unitario
	 .Activo que representa
      -Clientes (Por cada compra)
         .Id compra
	 .Fecha
	 .Monto compra
	 .Monto total cliente
      -Empleado (Por cada venta)
	 .Id venta
         .Id cliente
	 .Nombre cliente
	 .Apellido cliente
	 .Monto venta
	 .Monto ventas totales
	 .Cantidad ventas por Monto venta
	 .Monto ventas totales
	 .Cantidad ventas por día/mes/año
	 .Cantidad total de empleados
      -Ventas (Por cada día/mes/año)
         .Cantidad de ventas
	 .Suma de los montos
	 .Ganancia bruta
	 .Cantidad de ventas totales
	 .Suma de los montos totales
	 .Ganancia bruta total

