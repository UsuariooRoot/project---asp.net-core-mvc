﻿namespace ECommerce.Models
{
    public class Carrito
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Precio * Cantidad;
    }
}
