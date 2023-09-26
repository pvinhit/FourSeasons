using System.Text.RegularExpressions;

namespace be.Helpers
{
    public class ValidateHelper
    {
        private static ValidateHelper instance;
        public static ValidateHelper Instance
        {
            get { if (instance == null) instance = new ValidateHelper(); return ValidateHelper.instance; }
            private set { ValidateHelper.instance = value; }
        }
        public bool IsMail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
        public const string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public static bool IsPhone(string number)
        {
            string regex = @"^(\+[0-9]{9})$";
            return Regex.IsMatch(number, regex, RegexOptions.IgnoreCase);
        }

    }
}
