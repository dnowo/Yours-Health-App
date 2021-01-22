using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FitApp
{
    public partial class Training
    {
        public event EventHandler<int> BurnedKcalEvent;
        public int KcalTotal { get; set; }

        public Training()
        {
            InitializeComponent();
            initializeSports();
            initializeTrainingHistory();
            setComboBox();
            setTrainingHistory();
            calculateTotalBurnedCalories();
        }

        private void initializeSports()
        {
            using (var db = new FitAppDB())
            {
                var sportsList = db.Sports.ToList();

                if (sportsList.Count > 0)
                    return;

                var sport = new Sport {SportName = "Bieganie"};
                db.Sports.Add(sport);

                sport = new Sport {SportName = "Jazda na rowerze"};
                db.Sports.Add(sport);

                db.SaveChanges();
            }
        }

        private void initializeTrainingHistory()
        {
            using (var db = new FitAppDB())
            {
                var trainingList = db.Trainings.ToList();

                if (trainingList.Count > 0)
                    return;

                var sportsList = db.Sports.ToList();

                var historyItem = new TrainingModel
                {
                    TrainingDate = DateTime.Now.Date,
                    TrainingTime = DateTime.Parse("07:00").TimeOfDay,
                    TrainingType = sportsList[0],
                    KcalBurned = 300
                };

                db.Sports.Attach(sportsList[0]);
                db.Trainings.Add(historyItem);

                historyItem = new TrainingModel
                {
                    TrainingDate = DateTime.Now.Date,
                    TrainingTime = DateTime.Parse("03:00").TimeOfDay,
                    TrainingType = sportsList[1],
                    KcalBurned = 400
                };

                db.Sports.Attach(sportsList[1]);
                db.Trainings.Add(historyItem);

                db.SaveChanges();
            }
        }

        private void setComboBox()
        {
            using (var db = new FitAppDB())
            {
                var sportsList = db.Sports.ToList();
                comboBoxTrainingType.ItemsSource = sportsList;
            }
        }

        private void setTrainingHistory()
        {
            using (var db = new FitAppDB())
            {
                var trainingList = db.Trainings.Include("TrainingType").ToList();
                trainingList.Reverse();
                trainingHistory.ItemsSource = trainingList;
            }
        }

        private void addTraining_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new FitAppDB())
            {
                var sport = comboBoxTrainingType.SelectedItem as Sport;

                if (sport == null)
                    return;

                var trainingTime = calculateTrainingTime(BeginTime.Value, EndTime.Value);

                if (trainingTime == TimeSpan.Zero)
                    return;

                int caloriesBurned = calculateBurnedCalories(trainingTime, sport);

                var training = new TrainingModel
                {
                    KcalBurned = caloriesBurned,
                    TrainingDate = DateTime.Now,
                    TrainingTime = trainingTime,
                    TrainingType = sport
                };
                db.Sports.Attach(sport);
                db.Trainings.Add(training);

                db.SaveChanges();

                BurnedKcalEvent?.Invoke(this, -caloriesBurned);
            }

            comboBoxTrainingType.SelectedItem = 0;

            calculateTotalBurnedCalories();
            setTrainingHistory();
        }

        private void calculateTotalBurnedCalories()
        {
            using (var db = new FitAppDB())
            {
                KcalTotal = 0;
                var trainingList = db.Trainings.ToList();
                foreach (var training in trainingList)
                {
                    if (training.TrainingDate.Date == DateTime.Today)
                        KcalTotal += training.KcalBurned;
                }

                dayKcalBurned.Text = KcalTotal.ToString();
            }
        }

        private int calculateBurnedCalories(TimeSpan trainingTime, Sport sport)
        {
            using (var db = new FitAppDB())
            {
                User user = db.Users.ToList()[0];

                if (sport.SportName.Equals("Bieganie"))
                {
                    // Man Calories Burned = [(Age x 0.2017) — (Weight x 0.09036) +(150 x 0.6309) — 55.0969] x Time / 4.184.
                    // Woman Calories Burned = [(Age x 0.074) — (Weight x 0.05741) + (150 x 0.4472) — 20.4022] x Time / 4.184.
                    switch (user.Sex)
                    {
                        case true:
                            double first = (user.Age * 0.2017) - (user.Weight * 0.09036) + (150 * 0.6309) - 55.0969;
                            double second = first * (trainingTime.TotalMinutes / 4.184);
                            return (int) second;

                        case false:
                            double third = (user.Age * 0.074) - (user.Weight * 0.05741) + (150 * 0.4472) - 20.4022;
                            double fourth = third * (trainingTime.TotalMinutes / 4.184);
                            return (int) fourth;
                    }
                }

                if (sport.SportName.Equals("Jazda na rowerze")) //cycling
                {
                    // Bicycling, leisure, 9.4 mph Calories burned per minute = (5.8 x body weight in Kg x 3.5) ÷ 200
                    double first = (5.8 * user.Weight * 3.5) / 200;
                    double second = first * trainingTime.TotalMinutes;
                    return (int) second;
                }
            }

            return 0;
        }

        private TimeSpan calculateTrainingTime(DateTime? begin, DateTime? end)
        {
            if (begin == null || end == null)
            {
                MessageBox.Show("Podaj prawidłowe daty!");
                return TimeSpan.Zero;
            }

            return ((DateTime)end).Subtract((DateTime)begin);
        }

        private void onSelect(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;

            var result = MessageBox.Show("Czy na pewno chcesz usunąć trening?", "Usuń trening", MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var selectedTraining = e.AddedItems[0] as TrainingModel;
                using (var db = new FitAppDB())
                {
                    var trainingToDelete = db.Trainings.FirstOrDefault(x => x.Id == selectedTraining.Id);

                    if (trainingToDelete == null)
                        return;

                    if (trainingToDelete.TrainingDate.Date == DateTime.Today)
                        BurnedKcalEvent?.Invoke(this, trainingToDelete.KcalBurned);

                    db.Trainings.Remove(trainingToDelete);
                    db.SaveChanges();
                }

                calculateTotalBurnedCalories();
                setTrainingHistory();
            }
        }
    }
}