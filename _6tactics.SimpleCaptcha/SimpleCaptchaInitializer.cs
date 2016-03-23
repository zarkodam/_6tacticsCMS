using System;
using System.Collections.Generic;

namespace _6tactics.SimpleCaptcha
{
    public class SimpleCaptchaInitializer : Utilities
    {
        public static Dictionary<string, string> CurrentCaptcha { get; set; }

        public string GenerateCaptchaString(string captchaFor)
        {
            var random = new Random();

            int a = Numbers[random.Next(0, 9)];
            int b = Numbers[random.Next(0, 9)];
            string operation = Operations[random.Next(0, 5)];

            CurrentCaptcha = new Dictionary<string, string> {[captchaFor] = string.Concat(a, " ", operation, " ", b) };

            return string.Concat(a, " ", operation, " ", b);
        }
    }
}
