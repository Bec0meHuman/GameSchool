using System;
using System.Windows;
using System.Windows.Input;
using GameSchool.Windows_;
using GameSchool.Classes_;

namespace GameSchool.Windows_
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddQuizButton_Click(object sender, RoutedEventArgs e)
        {
            AddQuizWindow addQuizWindow = new AddQuizWindow(0); // 0 для создания новой викторины
            addQuizWindow.Show();
            Close();
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            QuizWindow quizWindow = new QuizWindow();
            quizWindow.Show();
            Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void MathQuizButton_Click(object sender, RoutedEventArgs e)
        {
            MathQuizWindow mathQuizWindow = new MathQuizWindow(SF.us);
            mathQuizWindow.Show();
            Close();
        }
    }
}