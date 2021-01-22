using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FitApp
{
    public partial class Menu
    {
        public event EventHandler<int> EatenKcalEvent;
        public int KcalTotal { get; set; }

        public Menu()
        {
            InitializeComponent();
            fillWithData_foodProducts();
            fillWithData_Meals();
            setComboBox(this, EventArgs.Empty);
            setDietHistory();
            refreshSummary();
        }

        private void fillWithData_foodProducts()
        {
            using (var db = new FitAppDB())
            {
                var products = from d in db.FoodProducts select d;
                var productsList = products.ToList();

                if (productsList.Count > 0)
                    return;

                var foodProduct = new FoodProduct()
                {
                    Name = "Ziemniak pieczony bez tłuszczu",
                    Weight = 100,
                    Kcal = 93
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Ziemniaki gotowane na parze",
                    Weight = 250,
                    Kcal = 192
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Ziemniaki gotowane z solą",
                    Weight = 200,
                    Kcal = 180
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Puree ziemniaczane",
                    Weight = 150,
                    Kcal = 135
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Frytki karbowane z piekarnika",
                    Weight = 100,
                    Kcal = 186
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Frytki McDonald's",
                    Weight = 100,
                    Kcal = 299
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Kotlet schabowy",
                    Weight = 130,
                    Kcal = 413
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Kopytka",
                    Weight = 200,
                    Kcal = 294
                };
                db.FoodProducts.Add(foodProduct);
                foodProduct = new FoodProduct()
                {
                    Name = "Cielęcina duszona",
                    Weight = 100,
                    Kcal = 188
                };
                db.FoodProducts.Add(foodProduct);
                db.SaveChanges();
            }
        }

        private void fillWithData_Meals()
        {
            using (var db = new FitAppDB())
            {
                var meals = from d in db.Meals select d;
                var mealsList = meals.ToList();

                if (mealsList.Count > 0)
                    return;

                var products = from d in db.FoodProducts select d;

                var meal = new Meal()
                {
                    Name = "Schabowy z ziemniakami",
                    Type = "Obiad"
                };

                var productsList = products.Where(p => p.Name == "Kotlet schabowy" || p.Name == "Ziemniaki gotowane z solą").ToList();

                foreach (var product in productsList)
                {
                    if (product != null)
                    {
                        db.FoodProducts.Attach(product);
                        meal.Ingredients.Add(product);
                    }
                }

                if (productsList.Count > 0)
                    db.Meals.Add(meal);

                meal = new Meal()
                {
                    Name = "Kopytka z cielęciną",
                    Type = "Kolacja"
                };

                productsList = products.Where(p => p.Name == "Kopytka" || p.Name == "Cielęcina duszona").ToList();

                foreach (var product in productsList)
                {
                    if (product != null)
                    {
                        db.FoodProducts.Attach(product);
                        meal.Ingredients.Add(product);
                    }
                }

                if (productsList.Count > 0)
                    db.Meals.Add(meal);

                db.SaveChanges();
            }
        }

        private void setComboBox(object sender, EventArgs e)
        {
            using (var db = new FitAppDB())
            {
                var meals = db.Meals.Include("Ingredients").ToList();
                comboBoxMeal.ItemsSource = meals;
            }
        }

        private void setDietHistory()
        {
            using (var db = new FitAppDB())
            {
                var dataSource = new List<DietHistoryItem>();
                var dietList = db.Diet.Include("Meal.Ingredients").ToList();

                foreach (var diet in dietList)
                {
                    var kcalSum = diet.Meal.Ingredients.Sum(i => i.Kcal);
                    dataSource.Add(new DietHistoryItem {Diet = diet, Kcal = kcalSum});
                }

                dataSource.Reverse();
                dietHistory.ItemsSource = dataSource;
            }
        }

        private void refreshSummary()
        {
            summaryDay.Text = DateTime.Now.ToString("dddd, dd MMM yyyy");

            KcalTotal = 0;
            var historyItems = dietHistory.Items;
            foreach (var item in historyItems)
            {
                if (((DietHistoryItem)item).Diet.Date.Date == DateTime.Today)
                    KcalTotal += ((DietHistoryItem)item).Kcal;
            }

            dayKcalEaten.Text = KcalTotal.ToString();
        }

        private void newMeal_OnClick(object sender, RoutedEventArgs e)
        {
            AddMealForm addMealWindow = new AddMealForm();
            addMealWindow.RefreshComboBox += setComboBox;
            addMealWindow.Show();
        }

        private void addMealToDiet_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new FitAppDB())
            {
                var meal = comboBoxMeal.SelectedItem as Meal;

                if (meal == null)
                    return;

                var eatenMeal = new Diet
                {
                    Meal = meal,
                    Date = DateTime.Now
                };
                db.Meals.Attach(meal);
                db.Diet.Add(eatenMeal);

                db.SaveChanges();

                setDietHistory();
                refreshSummary();
                EatenKcalEvent?.Invoke(this, meal.Ingredients.Sum(i => i.Kcal));
            }
        }

        private void onSelect(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;

            var result = MessageBox.Show("Czy na pewno chcesz usunąć posiłek z historii?", "Usuń posiłek", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedHistoryItem = e.AddedItems[0] as DietHistoryItem;
                using (var db = new FitAppDB())
                {
                    var mealToDelete = db.Diet.Include("Meal.Ingredients").FirstOrDefault(x => x.Id == selectedHistoryItem.Diet.Id);

                    if (mealToDelete == null)
                        return;

                    if (mealToDelete.Date.Date == DateTime.Today)
                        EatenKcalEvent?.Invoke(this, -mealToDelete.Meal.Ingredients.Sum(i => i.Kcal));

                    db.Diet.Remove(mealToDelete);
                    db.SaveChanges();
                }

                setDietHistory();
                refreshSummary();
            }
        }

    }

    class DietHistoryItem
    {
        public Diet Diet { get; set; }
        public int Kcal { get; set; }
    }
}
