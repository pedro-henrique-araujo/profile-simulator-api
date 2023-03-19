using ProfileSimulator.Api.Data;
using System.Text.Json;

namespace ProfileSimulator.Api
{
    public class PersonDataGenerator
    {
        private Dictionary<string, string[]>? _names;
        private Random _random;
        private string[] _possibleMaleFirstNames;
        private string[] _possibleFemaleFirstNames;
        private string[] _possibleLastNames;
        private Profile _profile;
        private const string POSSIBLE_MALE_FIRST_NAMES_KEY = "male_names";
        private const string POSSIBLE_FEMALE_FIRTS_NAMES_KEY = "female_names";

        public PersonDataGenerator(Profile profile)
        {
            var namesAsJson = File.ReadAllText("names.json");
            _names = JsonSerializer.Deserialize<Dictionary<string, string[]>>(namesAsJson);
            _random = new Random();
            _possibleMaleFirstNames = _names[POSSIBLE_MALE_FIRST_NAMES_KEY];
            _possibleFemaleFirstNames = _names[POSSIBLE_FEMALE_FIRTS_NAMES_KEY];
            _possibleLastNames = _names["last_names"];
            _profile = profile;
        }

        public void PickAGender()
        {
            var possibleGenders = new[] { 0, 1 };
            _profile.Gender = possibleGenders.PickARandomItem();
        }

        public void PickAFirstName()
        {
            var possibleFirstNames = GetPossibleNamesForGender();
            _profile.FirstName = possibleFirstNames.PickARandomItem();
        }

        public void PickALastName()
        {
            _profile.LastName = _possibleLastNames.PickARandomItem();
        }

        public void PickADateOfBirth()
        {
            var today = DateTime.Today;

            var yearsToDescrease = _random.Next(70);
            var yearOfBirth = today.Year - yearsToDescrease;

            var randomMonth = _random.Next(12) + 1;
            var monthOfBirth = randomMonth;
            if (yearOfBirth == today.Year)
            {
                monthOfBirth = Math.Min(randomMonth, today.Month);
            }

            var randomDay = _random.Next(DateTime.DaysInMonth(yearOfBirth, monthOfBirth)) + 1;
            var dayOfBirth = randomDay;
            if (monthOfBirth == today.Month)
            {
                dayOfBirth = Math.Min(randomDay, today.Day);
            }

            _profile.DateOfBirth = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);
        }

        public void PickAnEmail()
        {
            _profile.Email = _profile.LastName.ToLower() + '.' + _profile.FirstName.ToLower() + "@fmail.com";
        }       

        private string[] GetPossibleNamesForGender()
        {
            if (_profile.Gender == 0)
            {
                return _possibleMaleFirstNames;
            }

            return _possibleFemaleFirstNames;
        }

        public int GetAge()
        {
            var today = DateTime.Today;

            var timeDifference = today - _profile.DateOfBirth;

            var age = Convert.ToInt32(Math.Floor(timeDifference.TotalDays / 365.25));

            return age;
        }
    }
}
