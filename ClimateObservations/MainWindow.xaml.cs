using ClimateObservations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClimateObservations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Observer observer;
        Observation observation;
        Geolocation geolocation;
        Measurement measurement;
        Category category;

        ClimateObservationRepository database = new ClimateObservationRepository();
        public MainWindow()
        {
            InitializeComponent();
            lstbxAllObservers.ItemsSource = database.GetAllObservers();
            lstbxGeolocation.ItemsSource = database.GetAllGeolocations();
            lstbxCategory.ItemsSource = database.GetCategories(null);
        }
        private void UpdateObservers()
        {
            lstbxAllObservers.ItemsSource = null;
            lstbxAllObservers.ItemsSource = database.GetAllObservers();
        }
        private void UpdateCurrentObservations(int observerID)
        {
            lstbxCurrentObservation.ItemsSource = null;
            lstbxCurrentObservation.ItemsSource = database.GetAllObservations(observerID);
        }
        private void UpdateMeasurements()
        {
            lstbxMeasurement.ItemsSource = null;
            lstbxMeasurement.ItemsSource = database.GetAllMeasurements(observation.ID);
        }
        private void NewObserver(object sender, RoutedEventArgs e)
        {
            observer = database.RegisterNewObserver(txtFirstname.Text,txtLastname.Text);

            UpdateObservers();
            lblCurrObserver.Content = observer.ToString();
            MessageBox.Show($"{observer.FirstName} {observer.LastName} är nu registrerad som klimatobservatör.");
            
            txtFirstname.Clear();
            txtLastname.Clear();
            lstbxCurrentObservation.ItemsSource = null;
            lstbxMeasurement.ItemsSource = null;
        }
        private void DeleteObserver(object sender, RoutedEventArgs e)
        {
            observer = (Observer)lstbxAllObservers.SelectedItem;

            if (database.DeleteObserver(observer.ID) == true)
            {
                MessageBox.Show($"{observer} har raderats.");
            }
            else
            {
                MessageBox.Show($"{observer} har gjort klimatobservationer och kan inte tas bort.");
            }
            UpdateObservers();
        }
        private void SelectedObserver(object sender, RoutedEventArgs e)
        {
            observer = (Observer) lstbxAllObservers.SelectedItem;
            if (observer != null)
            {
                lblCurrObserver.Content = $"{observer}";
                UpdateCurrentObservations(observer.ID);
            }
            else { }
        }
        private void CreateObservation(object sender, RoutedEventArgs e)
        {
           observer = (Observer)lstbxAllObservers.SelectedItem;
           geolocation = (Geolocation)lstbxGeolocation.SelectedItem;

           observation = database.RegisterNewObservation(observer.ID, geolocation.ID);
           UpdateCurrentObservations(observer.ID);
           MessageBox.Show("Du har sparat din observation");
           
        }
        private void AddMeasurement (object sender, RoutedEventArgs e)
        {
            category = (Category)lstbxCategory.SelectedItem;
            int value = int.Parse(txtValue.Text);
            database.AddMeasurement(value, observation.ID, category.ID);
            UpdateMeasurements();
            lblUnit.Content = null;
            txtValue.Text = null;

            MessageBox.Show("Du har lagt till ett mätvärde.");
        }
        private void ChooseCategory(object sender, RoutedEventArgs e)
        {
            var category = (Category)lstbxCategory.SelectedItem;
            lblUnit.Content = $"{category.Unit}";
            var categories = database.GetCategories(category.ID);

            lstbxCategory.ItemsSource = null;
            lstbxCategory.ItemsSource = categories;
        }
        private void ResetCategory(object sender, RoutedEventArgs e)
        {
            lstbxCategory.ItemsSource = null;
            lstbxCategory.ItemsSource = database.GetCategories(null);
        }
        private void SelectedObservation(object sender, RoutedEventArgs e)
        {
            observation = (Observation)lstbxCurrentObservation.SelectedItem;
            observer = database.GetSelectedObserver(observation.ObserverID);
            UpdateMeasurements();
        }
        private void SelectedMeasurement (object sender, RoutedEventArgs e)
        {
            measurement = (Measurement)lstbxMeasurement.SelectedItem;
            txtValue.Text = measurement.Value.ToString();
            lblUnit.Content = measurement.Unit;
        }
        private void EditMeasurement (object sender, RoutedEventArgs e)
        {
            measurement = (Measurement)lstbxMeasurement.SelectedItem;
            database.AlterMeasurement(double.Parse(txtValue.Text), measurement.ID);
            UpdateMeasurements();
            lblUnit.Content = null;
            txtValue.Text = null;
            MessageBox.Show("Du har redigerat ett mätvärde.");
        }
    }
}
