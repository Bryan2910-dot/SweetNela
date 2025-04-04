using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweetNela.Models
{
    public class PedidoMejora
    {
         public String? Sabor { get; set; }
        public String? Tamaño { get; set; }
        public String? Relleno { get; set; }
        public String? Detalles { get; set; }
        public String? Fecha { get; set; }
        public String? Hora { get; set; }
        public String? Lugar { get; set; }

        public double Calcular1()
        {
            double resultado = 0;
            switch (Tamaño)
            {
                case "Pequeño":
                    resultado += 10;
                    break;
                case "Mediano":
                    resultado += 20;
                    break;
                case "Grande":
                    resultado += 30;
                    break;
            }
            return resultado;
        }
        
        public double Calcular2()
        {
            double resultado = 0;
            switch (Sabor)
            {
                case "Chocolate":
                    resultado += 0;
                    break;
                case "Vainilla":
                    resultado += 0;
                    break;
                case "Keke ingles":
                    resultado += 10;
                    break;
                case "Red Velvet":
                    resultado += 10;
                    break;
            }
            return resultado;
        }
        public double Calcular3()
        {
            double resultado = 0;
            switch (Relleno)
            {
                case "Manjar blanco":
                    resultado += 0;
                    break;
                case "Fudge":
                    resultado += 0;
                    break;
                case "Oreo trozada":
                    resultado += 3;
                    break;
                case "Chin chin":
                    resultado += 3;
                    break;
            }
            return resultado;
        }

    }
}