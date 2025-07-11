using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GameSchool.Classes_;
using System.Linq;
using System.Collections.ObjectModel;

namespace GameSchool.Windows_
{
    public partial class MathQuizWindow : Window
    {
        private List<MathProblem> problems;
        private int currentProblemIndex;
        private int correctAnswers;
        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        private bool isTestActive;
        private ObservableCollection<GameScores> quizHistory;
        private readonly Entities dbContext;
        private readonly Users currentUser;

        public MathQuizWindow(Users user)
        {
            InitializeComponent();
            currentUser = user;
            dbContext = new Entities();
            LoadQuizHistory();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void LoadQuizHistory()
        {
            // Загружаем историю тестов текущего пользователя
            var history = dbContext.GameScores
                .Where(gs => gs.UserID == currentUser.UserID && gs.GameName == "MathQuiz")
                .OrderByDescending(gs => gs.DatePlayed)
                .ToList();

            quizHistory = new ObservableCollection<GameScores>(history);
            HistoryListBox.ItemsSource = quizHistory;
        }

        private void SaveQuizResult()
        {
            var result = new GameScores
            {
                UserID = currentUser.UserID,
                GameName = "MathQuiz",
                Score = correctAnswers,
                DatePlayed = DateTime.Now,
                Difficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem).Content.ToString()
            };

            dbContext.GameScores.Add(result);
            dbContext.SaveChanges();

            // Обновляем список в UI
            quizHistory.Insert(0, result);
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TimeTextBox.Text, out int minutes) || minutes <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное время (больше 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(ProblemsCountTextBox.Text, out int problemCount) || problemCount <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество примеров (больше 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получаем выбранные настройки
            var difficulty = (MathProblem.DifficultyLevel)DifficultyComboBox.SelectedIndex;
            var operation = (MathProblem.OperationType)OperationComboBox.SelectedIndex;

            // Генерируем примеры
            problems = new List<MathProblem>();
            for (int i = 0; i < problemCount; i++)
            {
                problems.Add(new MathProblem(difficulty, operation));
            }

            // Инициализируем переменные
            currentProblemIndex = 0;
            correctAnswers = 0;
            remainingTime = TimeSpan.FromMinutes(minutes);
            isTestActive = true;

            // Настраиваем таймер
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Показываем интерфейс теста
            SettingsGrid.Visibility = Visibility.Collapsed;
            QuizGrid.Visibility = Visibility.Visible;

            // Отображаем первый пример
            DisplayCurrentProblem();
            AnswerTextBox.Focus();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTime = remainingTime.Add(TimeSpan.FromSeconds(-1));
            TimerText.Text = $"Осталось времени: {remainingTime:mm\\:ss}";

            if (remainingTime <= TimeSpan.Zero)
            {
                EndTest("Время истекло!");
            }
        }

        private void DisplayCurrentProblem()
        {
            if (currentProblemIndex < problems.Count)
            {
                ProblemText.Text = problems[currentProblemIndex].Expression;
                ProgressText.Text = $"Пример {currentProblemIndex + 1} из {problems.Count}";
                ScoreText.Text = $"Правильных ответов: {correctAnswers}";
                AnswerTextBox.Text = "";
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            SubmitAnswer();
        }

        private void AnswerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitAnswer();
            }
        }

        private void SubmitAnswer()
        {
            if (!isTestActive) return;

            if (double.TryParse(AnswerTextBox.Text, out double userAnswer))
            {
                var currentProblem = problems[currentProblemIndex];
                bool isCorrect = currentProblem.CheckAnswer(userAnswer);

                if (isCorrect)
                {
                    correctAnswers++;
                }

                currentProblemIndex++;

                if (currentProblemIndex >= problems.Count)
                {
                    EndTest("Тест завершен!");
                }
                else
                {
                    DisplayCurrentProblem();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите числовой ответ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EndTest(string message)
        {
            timer.Stop();
            isTestActive = false;

            double percentage = (double)correctAnswers / problems.Count * 100;
            string grade = percentage >= 90 ? "Отлично!" :
                         percentage >= 70 ? "Хорошо!" :
                         percentage >= 50 ? "Удовлетворительно" :
                         "Нужно подготовиться лучше";

            MessageBox.Show(
                $"{message}\n\n" +
                $"Правильных ответов: {correctAnswers} из {problems.Count}\n" +
                $"Процент правильных ответов: {percentage:F1}%\n" +
                $"Оценка: {grade}",
                "Результаты теста",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // Возвращаемся к настройкам теста
            QuizGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Visible;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и десятичную точку/запятую
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"^[0-9]*(\.|\,)?[0-9]*$");
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void EndQuiz()
        {
            timer.Stop();
            SaveQuizResult();
            MessageBox.Show($"Тест завершен!\nПравильных ответов: {correctAnswers} из {ProblemsCountTextBox.Text}", 
                          "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            
            QuizGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Visible;
            HistoryGrid.Visibility = Visibility.Visible;
        }

        protected override void OnClosed(EventArgs e)
        {
            dbContext.Dispose();
            base.OnClosed(e);
        }
    }
} 