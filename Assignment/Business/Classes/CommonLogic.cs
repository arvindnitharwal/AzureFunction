using System.Linq;
using System.Text.RegularExpressions;
namespace Assignment.Business.Common {
    public static class CommonLogic {
        private static int[] firstDigit = { 6, 7, 8, 9 };
        public static bool ValidMobileNumber (double mobileNumber) {
            //only indian number allowed
            //first numbar must be 6,7,8,9 and length =10
            return firstDigit.Contains ((int) mobileNumber % 1000000000);
        }
        //only first name allowed
        public static bool ValidName (string name) {
            var regex = new Regex(@"^[a-zA-Z]\w*$");
            return !(string.IsNullOrEmpty (name) || regex.IsMatch(name.Trim()));
        }
        public static bool ValidLoanAmount (string loanAmount) {
            var regex = new Regex ("^[0-9]+$");
            return (!string.IsNullOrEmpty (loanAmount) && regex.IsMatch (loanAmount));
        }
    }
}