using System;
using System.Linq;
using System.Windows;

namespace FitApp
{
    public partial class Home
    {
        public int TotalKcalsEatenAndBurned { get; set; }

        public Home()
        {
            InitializeComponent();
            displayInfo();
        }

        public void displayInfo()
        {
            using (var db = new FitAppDB())
            {
                var users = db.Users.ToList();

                if (users.Count >= 1)
                {
                    EditData.IsEnabled = true;
                    var sex = users[0].Sex ? "Mężczyzna" : "Kobieta";
                    InsertData.IsEnabled = false;
                    PersonAge.Text = "Wiek: " + users[0].Age;
                    PersonWeight.Text = "Waga (kg): " + users[0].Weight;
                    PersonHeight.Text = "Wzrost (cm): " + users[0].Height;
                    PersonSex.Text = "Płeć: " + sex;
                    kcalInfo.Text = TotalKcalsEatenAndBurned + "/" + calculateKcalBasedOnActivity();
                    bmrKcalInfo.Text = calculateBMR() + "";
                }
                else
                {
                    InsertData.IsEnabled = true;
                    EditData.IsEnabled = false;
                }
            }

            kcalAlert.Visibility = TotalKcalsEatenAndBurned >= calculateKcalBasedOnActivity() - 500 ? Visibility.Visible : Visibility.Hidden;
        }

        private void refreshPage(object sender, EventArgs e)
        {
            displayInfo();
        }

        private void insertData_OnClick(object sender, RoutedEventArgs e)
        {
            var windowInput = new InputData();
            windowInput.RefreshHomePage += refreshPage;
            windowInput.Show();
        }

        private int calculateBMR()
        {
            // The Revised Harris - Benedict Equation
            // Men BMR = 88.362 + (13.397 x weight in kg) +(4.799 x height in cm) -(5.677 x age in years)
            // Women BMR = 447.593 + (9.247 x weight in kg) +(3.098 x height in cm) -(4.330 x age in years)

            FitAppDB db = new FitAppDB();
            var pps = from d in db.Users
                select d;
            var persons = pps.ToList();
            User person;
            if (persons.Count >= 1)
            {
                person = persons[0];
            }
            else
            {
                return 0;
            }

            // Men
            if (person.Sex)
            {
                var menBmr = 88.362 + (13.397 * person.Weight) + (4.799 * person.Height) - (5.677 * person.Age);
                return (int)menBmr;
            }

            //Women
            var womanBmr = 447.593 + (9.247 * person.Weight) + (3.098 * person.Height) - (4.330 * person.Age);
            return (int)womanBmr;
        }

        // Little to no exercise   Daily kilocalories needed = BMR x 1.2
        // Light exercise(1–3 days per week)	Daily kilocalories needed = BMR x 1.375
        // Moderate exercise(3–5 days per week)	Daily kilocalories needed = BMR x 1.55
        // Heavy exercise(6–7 days per week)	Daily kilocalories needed = BMR x 1.725
        // Very heavy exercise(twice per day, extra heavy workouts)   Daily kilocalories needed = BMR x 1.9
        private int calculateKcalBasedOnActivity()
        {
            var bmr = calculateBMR();
            if (LittleActivityRadio.IsChecked == true)
            {
                return (int)(bmr * 1.2);
            }

            if (LightActivityRadio.IsChecked == true)
            {
                return (int)(bmr * 1.375);
            }

            if (ModerateActivityRadio.IsChecked == true)
            {
                return (int)(bmr * 1.55);
            }

            if (HeavyActivityRadio.IsChecked == true)
            {
                return (int)(bmr * 1.725);
            }

            if (VeryHeavyActivityRadio.IsChecked == true)
            {
                return (int)(bmr * 1.9);
            }

            return bmr;
        }

        private void littleActivityRadio_OnClick(object sender, RoutedEventArgs e)
        {
            displayInfo();
        }

        private void editData_OnClickData_OnClick(object sender, RoutedEventArgs e)
        {
            var editData = new InputData(true);
            editData.RefreshHomePage += refreshPage;
            editData.Show();
        }
    }
}