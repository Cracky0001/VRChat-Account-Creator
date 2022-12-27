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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Diagnostics;

namespace VRChatAccountCreator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // Prüfen, ob das Passwort-Textfeld leer ist
            if (string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                // Generieren eines zufälligen Passworts
                string password = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 32);
                PasswordTextBox.Text = password;
            }
            // Create random date of birth
            Random random = new Random();
            int day = random.Next(1, 29);
            int month = random.Next(1, 13);
            int year = random.Next(1980, 2005);

            // Initialize Chromedriver
            ChromeDriver driver = new ChromeDriver();

            // Open VRCHat website
            driver.Url = "https://vrchat.com/home/register";

            // Speichern der Anmeldeinformationen im Ordner "Accounts"
            string accountsDirectory = "Accounts";
            if (!Directory.Exists(accountsDirectory))
            {
                Directory.CreateDirectory(accountsDirectory);
            }
            string filePath = $"{accountsDirectory}/{UsernameTextBox.Text}.txt";
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine("###################################################");
                sw.WriteLine("Username: " + UsernameTextBox.Text);
                sw.WriteLine("Password: " + PasswordTextBox.Text);
                sw.WriteLine("Email: " + EmailTextBox.Text);
                sw.WriteLine("###################################################");
                sw.WriteLine($"{UsernameTextBox.Text}:{PasswordTextBox.Text}");
                sw.WriteLine("###################################################");
                sw.WriteLine("");
                sw.WriteLine("made by https://github.com/Cracky0001");
            }

            // Fill out
            driver.FindElementById("username").SendKeys(UsernameTextBox.Text);
            driver.FindElementById("password").SendKeys(PasswordTextBox.Text);
            driver.FindElementById("email").SendKeys(EmailTextBox.Text);
            driver.FindElementById("password-again").SendKeys(PasswordTextBox.Text);
            driver.FindElementById("email-again").SendKeys(EmailTextBox.Text);
            driver.FindElementById("day").SendKeys(day.ToString());
            driver.FindElementById("year").SendKeys(year.ToString());
            System.Threading.Thread.Sleep(100);
            IWebElement checkboxElement = driver.FindElement(By.Id("tos"));
            checkboxElement.Click();

            
        }

        private void OpenAccountDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string accountsDirectory = "Accounts";
            if (Directory.Exists(accountsDirectory))
            {
                Process process = new Process();
                process.StartInfo.FileName = "explorer.exe";
                process.StartInfo.Arguments = accountsDirectory;
                process.Start();
            }
            else
            {
                MessageBox.Show("Verzeichnis wurde nicht gefunden!");
            }
        }

    }
}