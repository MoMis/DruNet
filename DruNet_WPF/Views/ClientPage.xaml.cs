﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DruNet_WPF.Core;

namespace DruNet_WPF.Views
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private int backCounter = 0;
        private string editigFileName = null;

        public ClientPage()
        {
            InitializeComponent();

            //ApplicationLogicInitializer.ClientRun();
            Client.Instance.PrintOutputOnTextBlock += PrintOutput;
            //Client.Instance.Run();
        }

        public void PrintOutput(string output)
        {
            Dispatcher.Invoke(() =>
            {
                OutputTb.Text += (output + "\n");
            });
        }

        /*
        private void CommandLine_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (CommandLineTb.Text)
                {
                    case "/login":
                        Login(CommandLineTb.Text);
                        break;
                    case "/ls":
                        Client.Instance.ViewTree();
                        break;
                    case "/logout":
                        Client.Instance.locker = 1;
                        Client.Instance.Send(0, " ");
                        break;
                    case "/cd":
                        Client.Instance.CreateDirectory(CommandLineTb.Text);
                        break;
                    case "/rf":
                        Client.Instance.ReadFile(CommandLineTb.Text);
                        break;
                    case "/cf":
                        Client.Instance.CreateFile(CommandLineTb.Text);
                        break;
                    default:
                        break;
                }

                CommandLineTb.Text = String.Empty;
            }
        }
        */

        private void LoginAction(object sender, RoutedEventArgs e)
        {
            Client.Instance.Send(1, LoginTb.Text);
            var serverResponse = Client.Instance.ReceiveFlag();

            if (serverResponse == 1)
            {
                Client.Instance.Send(1, PasswordTb.Password);
                serverResponse = Client.Instance.ReceiveFlag();
                if (serverResponse == 1)
                {
                    PrintOutput("Logged succesful!");
                    Client.Instance.locker = 0;
                    LoginPanel.Visibility = Visibility.Hidden;
                    ParametersPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    PrintOutput("Wrong Password!");
                }
            }
            else
            {
                PrintOutput("Wrong Login");
            }
        }

        private void CreateDir_OnClick(object sender, RoutedEventArgs e)
        {
            if (ParameterTb.Text != String.Empty)
            {
                Client.Instance.CreateDirectory(ParameterTb.Text);
                Client.Instance.Print("Successfullu created new directory: " + ParameterTb.Text);
                ParameterTb.Text = String.Empty;
                backCounter++;
                BackButton.IsEnabled = true;
            }
            else MessageBox.Show("Uzupełnij nazwę!", "Uwaga!");
        }

        private void CreateFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (ParameterTb.Text != String.Empty)
            {
                var fileWithContent = ParameterTb.Text.Split('<');
                fileWithContent[0].Trim();
                fileWithContent[1].Trim();

                Client.Instance.CreateFile(fileWithContent[0], fileWithContent[1]);
                ParameterTb.Text = String.Empty;
                Client.Instance.Print("Successfully added file: " + fileWithContent[0]);
            }
            else MessageBox.Show("Uzupełnij nazwę!", "Uwaga!");
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (ParameterTb.Text != String.Empty)
            {
                Client.Instance.ReadFile(ParameterTb.Text);
                ParameterTb.Text = String.Empty;
            }
            else MessageBox.Show("Uzupełnij nazwę!", "Uwaga!");
        }

        private void Logout_OnClick(object sender, RoutedEventArgs e)
        {
            Client.Instance.LogOut();
            LoginPanel.Visibility = Visibility.Visible;
            ParametersPanel.Visibility = Visibility.Hidden;
        }

        private void GetCurrentPath_OnClick(object sender, RoutedEventArgs e)
        {
            Client.Instance.GetCurrentPath();
        }

        private void GoToDir_OnClick(object sender, RoutedEventArgs e)
        {
            if (GoToDirectoryTb.Text != String.Empty)
            {
                Client.Instance.CreateDirectory(GoToDirectoryTb.Text);
                Client.Instance.Print(GoToDirectoryTb.Text);
                BackButton.IsEnabled = true;
                GoToDirectoryTb.Text = String.Empty;
                backCounter++;
                BackButton.IsEnabled = true;
            }
            else MessageBox.Show("Uzupełnij nazwę!", "Uwaga!");
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (backCounter == 0)
                BackButton.IsEnabled = false;
            else
            {
                backCounter--;
                Client.Instance.PrevDir();
            }
            
        }

        private void ListCatalog_OnClick(object sender, RoutedEventArgs e)
        {
            Client.Instance.ViewTree();
        }

        private void EditFile_OnClick(object sender, RoutedEventArgs e)
        {
            CreateFile_OnClick(sender, e);
        }
    }
}
