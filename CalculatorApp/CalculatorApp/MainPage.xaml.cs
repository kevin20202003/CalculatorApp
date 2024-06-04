using CalculatorApp.Data;
using CalculatorApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CalculatorApp
{
    public partial class MainPage : ContentPage
    {
        private readonly Database _database;
        private string dbPath;

        public MainPage()
        {
            InitializeComponent();

            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "operations.db3");
            _database = new Database(dbPath);

            LoadOperations();
        }

        private async void LoadOperations()
        {
            var operations = await _database.GetOperationsAsync();
            OperationsListView.ItemsSource = operations;
        }

        private async void OnSumButtonClicked(object sender, EventArgs e)
        {
            PerformOperation("Suma");
        }

        private async void OnSubtractButtonClicked(object sender, EventArgs e)
        {
            PerformOperation("Resta");
        }

        private async void OnMultiplyButtonClicked(object sender, EventArgs e)
        {
            PerformOperation("Multiplicacion");
        }

        private async void OnDivideButtonClicked(object sender, EventArgs e)
        {
            PerformOperation("Division");
        }

        private async void PerformOperation(string operationType)
        {
            if (double.TryParse(Number1Entry.Text, out double number1) && double.TryParse(Number2Entry.Text, out double number2))
            {
                if (operationType == "Division" && number2 == 0)
                {
                    ResultLabel.Text = "División por cero no permitida";
                    return;
                }

                double result = 0;

                switch (operationType)
                {
                    case "Suma":
                        result = number1 + number2;
                        break;
                    case "Resta":
                        result = number1 - number2;
                        break;
                    case "Multiplicacion":
                        result = number1 * number2;
                        break;
                    case "Division":
                        result = number1 / number2;
                        break;
                    default:
                        ResultLabel.Text = "Operación no válida";
                        return;
                }

                var operation = new Operation
                {
                    Date = DateTime.Now,
                    OperationType = operationType,
                    Number1 = number1,
                    Number2 = number2,
                    Result = result
                };

                await _database.SaveOperationAsync(operation);
                LoadOperations();
                ResultLabel.Text = $"Resultado: {result}";
            }
            else
            {
                ResultLabel.Text = "Ingrese números válidos";
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var operation = (sender as Button)?.BindingContext as Operation;

            if (operation != null)
            {
                bool answer = await DisplayAlert("Eliminar", "¿Estás seguro de que deseas eliminar esta operación?", "Sí", "Cancelar");

                if (answer)
                {
                    await _database.DeleteOperationAsync(operation);
                    LoadOperations();
                }
            }
        }
    }
}
