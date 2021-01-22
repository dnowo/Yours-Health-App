using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FitApp
{
    public partial class AddMealForm
    {
        public event EventHandler RefreshComboBox;

        public AddMealForm()
        {
            InitializeComponent();
            setComboBoxes();
        }

        private void setComboBoxes(bool isTea = false)
        {
            using (var db = new FitAppDB())
            {
                List<FoodProduct> productList;
                if (isTea) 
                    productList = db.FoodProducts.Where(i => i.Name.StartsWith("Ziemniak")).ToList();
                else
                    productList = db.FoodProducts.ToList();

                comboBoxIngredient1.ItemsSource = productList;
                comboBoxIngredient2.ItemsSource = productList;
                comboBoxIngredient3.ItemsSource = productList;
                comboBoxIngredient4.ItemsSource = productList;
                comboBoxIngredient5.ItemsSource = productList;
            }
        }

        private RadioButton whichRadioButton()
        {
            if (BreakfastRadio.IsChecked == true)
                return BreakfastRadio;
            else if (DinnerRadio.IsChecked == true)
                return DinnerRadio;
            else if (TeaRadio.IsChecked == true)
                return TeaRadio;
            else
                return SupperRadio;
        }

        private void windowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_btn_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxMealName.Text.Trim() == "")
                return;

            using (var db = new FitAppDB())
            {
                var meal = new Meal()
                {
                    Name = textBoxMealName.Text,
                    Type = whichRadioButton().Content.ToString()
                };

                var product1 = (FoodProduct)comboBoxIngredient1.SelectedValue;
                if (product1 != null)
                {
                    db.FoodProducts.Attach(product1);
                    meal.Ingredients.Add(product1);
                }

                var product2 = (FoodProduct)comboBoxIngredient2.SelectedValue;
                if (product2 != null)
                {
                    db.FoodProducts.Attach(product2);
                    meal.Ingredients.Add(product2);
                }

                var product3 = (FoodProduct)comboBoxIngredient3.SelectedValue;
                if (product3 != null)
                {
                    db.FoodProducts.Attach(product3);
                    meal.Ingredients.Add(product3);
                }

                var product4 = (FoodProduct)comboBoxIngredient4.SelectedValue;
                if (product4 != null)
                {
                    db.FoodProducts.Attach(product4);
                    meal.Ingredients.Add(product4);
                }

                var product5 = (FoodProduct)comboBoxIngredient5.SelectedValue;
                if (product5 != null)
                {
                    db.FoodProducts.Attach(product5);
                    meal.Ingredients.Add(product5);
                }

                if (meal.Ingredients.Count == 0)
                    return;

                db.Meals.Add(meal);

                db.SaveChanges();
            }

            RefreshComboBox?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void teaRadio_OnClick(object sender, RoutedEventArgs e)
        {
            setComboBoxes(TeaRadio.IsChecked ?? false);
        }
    }
}
