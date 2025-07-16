using System;
using Humanizer;

namespace MyExtensions
{
    public class TextHelper
    {
        public string NumeroATexto(int numero)
        {
            return numero.ToWords(new System.Globalization.CultureInfo("es-ES"));
        }
    }
}