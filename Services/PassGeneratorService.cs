namespace PetProject.Services
{
    public class PassGeneratorService
    {
        public string GeneratePassword(int longPass, bool isCaps, bool isNumbers, bool isSymbols)
        {
            string result = null!;

            Random random = new Random();
            List<char> acceptableValues = new();

            int m = 97, n = 122;
            for (int x = 0; m <= n; x++, m++)
            {
                acceptableValues.Add((char)m);
            }


            if (isCaps)
            {
                m = 65;
                n = 90;
                for (int x = 0; m <= n; x++, m++)
                {
                    acceptableValues.Add((char)m);
                }
            }            
            if (isNumbers)
            {
                m = 48;
                n = 57;
                for (int x = 0; m <= n; x++, m++)
                {
                    acceptableValues.Add((char)m);
                }
            }
            if (isSymbols)
            {
                m = 58;
                n = 64;
                for (int x = 0; m <= n; x++, m++)
                {
                    acceptableValues.Add((char)m);
                }

                m = 91;
                n = 96;
                for (int x = 0; m <= n; x++, m++)
                {
                    acceptableValues.Add((char)m);
                }

                m = 123;
                n = 126;
                for (int x = 0; m <= n; x++, m++)
                {
                    acceptableValues.Add((char)m);
                }
            }


            for (int x = 0; x <= longPass; x++)
            {
                result += acceptableValues[random.Next(0, acceptableValues.Count)];
            }
            return result;
        }
    }
}
