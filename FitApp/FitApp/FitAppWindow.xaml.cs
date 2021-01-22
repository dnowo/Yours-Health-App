using System;
using System.Windows;
using System.Windows.Input;

namespace FitApp
{
    public partial class MainWindow
    {
        public Home homePage = new Home();
        public Menu menuPage;
        public Training trainingPage;
        public Info infoPage = new Info();

        public MainWindow()
        {
            InitializeComponent();
            menuPage = new Menu();
            trainingPage = new Training();
            updateTotalKcalEatenAndBurned(this, menuPage.KcalTotal);
            updateTotalKcalEatenAndBurned(this, -trainingPage.KcalTotal);
            mainFrame.Navigate(infoPage);
            menuPage.EatenKcalEvent += updateTotalKcalEatenAndBurned;
            trainingPage.BurnedKcalEvent += updateTotalKcalEatenAndBurned;
        }

        private void updateTotalKcalEatenAndBurned(object sender, int kcal)
        {
            homePage.TotalKcalsEatenAndBurned += kcal;
        }

        private void listViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            ttHome.Visibility = toggleBtn.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void windowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void homeClick(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(homePage);
            homePage.displayInfo();
        }

        private void menuClick(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(menuPage);
        }

        private void trainingClick(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(trainingPage);
        }

        private void infoClick(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Navigate(infoPage);
        }
    }
}
