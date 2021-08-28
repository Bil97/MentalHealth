using System.Globalization;

namespace MentalHealth.Client.Services
{
    public class CurrencyConverter
    {
        private decimal Amount { get; set; }
        private decimal ConvertedAmount { get; set; }
        private string Culture { get; set; }

        public CurrencyConverter(decimal amount = 0, string culture = "sw-KE")
        {
            Amount = amount;
            Culture = culture;
        }

        public string Currency
        {
            get
            {
                Convert();
                return ConvertedAmount.ToString("C2", CultureInfo.CreateSpecificCulture(Culture));
            }
        }

        private void Convert()
        {
            // Do the conversion here and asign the value to ConvertedAmount
            ConvertedAmount = Amount;
        }
    }

}
