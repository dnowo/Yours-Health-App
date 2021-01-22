using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FitApp
{
    public partial class InputData
    {
        public event EventHandler RefreshHomePage;
       
        private bool edit;

        public InputData(bool edit = false)
        {
            InitializeComponent();
            SaveButton.Content = "Edytuj";
            this.edit = edit;
            fillWithData();
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            WeightInput.Text = HeightInput.Text = AgeInput.Text;
            WomanRadio.IsChecked = ManRadio.IsChecked = false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                if (validateInput(WeightInput.Text, AgeInput.Text, HeightInput.Text))
                {
                    using (var db = new FitAppDB())
                    {
                        var user = new User()
                        {
                            Weight = float.Parse(WeightInput.Text),
                            Age = Int32.Parse(AgeInput.Text),
                            Height = float.Parse(HeightInput.Text),
                            Sex = ManRadio.IsChecked == true
                        };

                        db.Users.Add(user);
                        db.SaveChanges();
                    }

                    RefreshHomePage?.Invoke(this, EventArgs.Empty);
                    Close();
                }
            }
            else
            {
                if (validateInput(WeightInput.Text, AgeInput.Text, HeightInput.Text))
                {
                    using (var db = new FitAppDB())
                    {
                        var users = db.Users.ToList();
                        var user = users[0];

                        if (user != null)
                        {
                            user.Weight = float.Parse(WeightInput.Text);
                            user.Age = int.Parse(AgeInput.Text);
                            user.Height = float.Parse(HeightInput.Text);
                            user.Sex = ManRadio.IsChecked == true;
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    RefreshHomePage?.Invoke(this, EventArgs.Empty);
                    Close();
                }
            }
        }

        private bool validateInput(string weight, string age, string height)
        {
            if (weight.Length <= 1 || age.Length <= 1 || height.Length <= 1)
            {
                MessageBox.Show("Wszystkie pola muszą być uzupełnione!");
                return false;
            }

            if (double.Parse(height) > 230.0 || double.Parse(height) < 100.0)
            {
                MessageBox.Show("Podano zły wzrost!");
                return false;
            }

            if (double.Parse(weight) > 250.0 || double.Parse(weight) < 20.0)
            {
                MessageBox.Show("Podano złą wagę!");
                return false;
            }

            if (int.Parse(age) > 100 || int.Parse(age) < 7)
            {
                MessageBox.Show("Podano zły wiek!");
                return false;
            }

            return true;
        }

        private void windowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void close_btn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void fillWithData()
        {
            using (var db = new FitAppDB())
            {
                var users = db.Users.ToList();
                User user;
                if (users.Count > 0)
                    user = users[0];
                else
                    return;

                WeightInput.Text = user.Weight.ToString(CultureInfo.CurrentCulture);
                AgeInput.Text = user.Age.ToString();
                HeightInput.Text = user.Height.ToString(CultureInfo.CurrentCulture);
                WomanRadio.IsChecked = !user.Sex;
                ManRadio.IsChecked = user.Sex;
            }
        }
    }
}