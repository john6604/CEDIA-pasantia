using System;
using Humanizer;

namespace Cedia.Common
{
    public class TextHelper
    {
        public string NumberToText(int numero)
        {
            return numero.ToWords(new System.Globalization.CultureInfo("es-ES"));
        }
    }
}