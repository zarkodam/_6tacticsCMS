using System;
using System.Collections.Generic;

namespace _6tactics.Cms.Services.Common
{
    public class SimpleCaptchaService : ISimpleCaptchaService
    {
        private List<string> Operations => new List<string>
        {
            "multiply", "times", "add", "plus", "subtract", "minus"
        };

        private List<int> Numbers => new List<int>
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

        public string GenerateCaptcha()
        {
            var random = new Random();

            int a = Numbers[random.Next(0, 9)];
            int b = Numbers[random.Next(0, 9)];
            string operation = Operations[random.Next(0, 5)];

            return string.Concat(a, " ", operation, " ", b);
        }

        public bool Check(int result, int a, int b, string operation)
        {
            return result == Calculation(a, b, operation);
        }
    }
}
