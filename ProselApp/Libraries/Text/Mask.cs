using System.Linq;

namespace ProselApp.Libraries.Text
{
    public class Mask
    {
        public static string FormatarReal(decimal valor)
        {
            return $"R$ {valor}";
        }
        public static float ConverterIntToDecimal(int valor)
        {
            //10000 -> "10000" -> "100.00" -> 100.00
            string valorPagarMeString = valor.ToString();
            string valorDecimalString = valorPagarMeString.Substring(0, valorPagarMeString.Length - 2) + "," + valorPagarMeString.Substring(valorPagarMeString.Length - 2);

            var dec = float.Parse(valorDecimalString);

            return dec;
        }
        public static string PrimeiroNome(string nomeCompleto)
        {
            return nomeCompleto.Split(' ')[0];
        }
        public static string PrimeirasDuasLetrasNome(string nomeCompleto)
        {
            if (!string.IsNullOrEmpty(nomeCompleto))
            {
                string[] ArrayNomes = nomeCompleto.Split(" ");
                string resultado = ArrayNomes[0].Substring(0, 1);
                if (ArrayNomes.Count() > 1) resultado += ArrayNomes[^1].Substring(0, 1);
                return resultado;
            }
            return "";
        }
    }
}