using System.Globalization;

namespace PayrollHrms.Services
{
    public class EmployeeNumberService
    {
        public string Generate(string lastName, DateTime dateOfBirth)
        {
            string cleanLastName = new string(
                lastName
                    .Where(char.IsLetter)
                    .ToArray()
            );

            string namePrefix = cleanLastName.Length >= 3
                ? cleanLastName[..3]
                : cleanLastName.PadRight(3, '*');

            int randomNumber = Random.Shared.Next(0, 100000);

            string randomPart = randomNumber.ToString("D5");

            string birthDatePart = dateOfBirth
                .ToString("ddMMMyyyy", CultureInfo.InvariantCulture)
                .ToUpperInvariant();

            return $"{namePrefix.ToUpperInvariant()}-{randomPart}-{birthDatePart}";
        }
    }
}