using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private string secretNumber;
        private int attempts;

        public MainWindow()
        {
            InitializeComponent();
            secretNumber = GenerateSecretNumber();
            attempts = 0;
        }

        private string GenerateSecretNumber()
        {
            string digits = "1234567890";
            Random rnd = new Random();
            return new string(digits.OrderBy(c => rnd.Next()).Take(4).ToArray());
        }

        private (int bulls, int cows) CountBullsAndCows(string secret, string guess)
        {
            int bulls = 0;
            int cows = 0;
            bool[] checkedSecret = new bool[4];
            bool[] checkedGuess = new bool[4];


            for (int i = 0; i < 4; i++)
            {
                if (secret[i] == guess[i])
                {
                    bulls++;
                    checkedSecret[i] = true;
                    checkedGuess[i] = true;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (!checkedGuess[i])
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!checkedSecret[j] && secret[j] == guess[i])
                        {
                            cows++;
                            checkedSecret[j] = true;
                            break;
                        }
                    }
                }
            }

            return (bulls, cows);
        }

        private void CheckGuess(object sender, RoutedEventArgs e)
        {
            string guess = GuessTextBox.Text;
            if (guess.Length != 4 || !guess.All(char.IsDigit) || guess[0] == '0')
            {
                ResultTextBlock.Text = "Неверный формат числа.";
                return;
            }

            attempts++;
            var result = CountBullsAndCows(secretNumber, guess);
            ResultTextBlock.Text = $"Быки: {result.bulls}, Коровы: {result.cows}";

            if (result.bulls == 4)
            {
                MessageBox.Show($"Поздравляю! Вы угадали число за {attempts} попыток!");
                AttemptsTextBlock.Text = $"Количество попыток: {attempts}";
                Close();
            }
        }
    }
}