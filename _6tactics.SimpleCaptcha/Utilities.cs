using System.Collections.Generic;

namespace _6tactics.SimpleCaptcha
{
    public class Utilities
    {
        protected static List<string> Operations => new List<string>
        {
            "multiply", "times", "add", "plus", "subtract", "minus"
        };

        protected static List<int> Numbers => new List<int>
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        private static int Calculation(int a, int b, string operation)
        {
            if (operation.Equals("multiply") || operation.Equals("times"))
                return a * b;
            if (operation.Equals("add") || operation.Equals("plus"))
                return a + b;
            if (operation.Equals("subtract") || operation.Equals("minus"))
                return a - b;

            return -100;
        }

        private static bool CaptchaValidatorHandler(int result, string captchaTask)
        {
            string[] captchaParts = captchaTask.Split(' ');
            int a = int.Parse(captchaParts[0]);
            string operation = captchaParts[1];
            int b = int.Parse(captchaParts[2]);

            return result == Calculation(a, b, operation);
        }

        public static bool IsCaptchaValid(string result, string captchaTask)
        {
            int captchaResult;
            if (!int.TryParse(result, out captchaResult))
                captchaResult = -100;
            return CaptchaValidatorHandler(captchaResult, captchaTask);
        }

        public static bool IsCaptchaValid(int result, string captchaTask)
        {
            return CaptchaValidatorHandler(result, captchaTask);
        }
    }
}
