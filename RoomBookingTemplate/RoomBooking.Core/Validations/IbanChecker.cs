using System.Numerics;

namespace RoomBooking.Core.Validations
{
  public static class IbanChecker
    {
        const int IbanNumberMinSize = 15;
        const int IbanNumberMaxSize = 34;

        /// <summary>
        /// Statische Methode zur Prüfung eines als String übergebenen Iban´s
        /// </summary>
        /// <param name="iban">zu prüfende Iban</param>
        /// <returns>true wenn Iban korrekt, sonst false</returns>
        public static bool CheckIban(string iban)
        {
            if (iban == null)
            {
                return false;
            }
            iban = iban.Replace(" ", ""); // Leerzeichen entfernen
            if (string.IsNullOrEmpty(iban) || iban.Length < IbanNumberMinSize
                || iban.Length > IbanNumberMaxSize)
            {
                return false;
            }
            string lettersSubstitutedIban = "";
            //Ersten 4 Stellen nach hinten verschieben
            string reorderedIban = (iban.Substring(4) + iban.Substring(0, 4)).ToUpper();
            //Nun Buchstaben durch Zahlen ersetzen:
            for (int i = 0; i < reorderedIban.Length; i++)
            {
                char actChar = reorderedIban[i];
                if (!char.IsDigit(actChar))
                {
                    if ((actChar >= 'A') && (actChar <= 'Z'))
                    {
                        lettersSubstitutedIban += (actChar - 'A' + 10);
                    }
                    else
                    {
                        return false; //Weder Buchstabe noch Ziffer -> ungültig!
                    }
                }
                else
                {
                    lettersSubstitutedIban += actChar;
                }
            }
            BigInteger bigInteger = BigInteger.Parse(lettersSubstitutedIban);
            int rest = (int)(bigInteger % 97);
            return rest == 1;
        }
    }
}
