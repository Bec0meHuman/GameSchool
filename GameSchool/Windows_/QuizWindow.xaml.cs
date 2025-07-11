using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GameSchool.Classes_;

namespace GameSchool.Windows_
{
    public partial class QuizWindow : Window
    {
        private List<QuizQuestion> questions;
        private int currentQuestionIndex = 0;
        private int totalQuestions = 0;
        private List<Button> answerButtons;
        private bool[] answeredQuestions;
        private int[] selectedAnswers;
        private int correctAnswersCount = 0;
        private bool currentQuestionAnswered = false;

        public QuizWindow()
        {
            InitializeComponent();
            LoadQuestions();
            InitializeAnswerButtons();
            answeredQuestions = new bool[totalQuestions];
            selectedAnswers = new int[totalQuestions];
            for (int i = 0; i < totalQuestions; i++)
            {
                selectedAnswers[i] = -1; // -1 означает, что ответ не выбран
            }
            DisplayCurrentQuestion();
            UpdateNextButtonState();
            
            // Скрываем предупреждение при старте
            WarningText.Opacity = 0;
        }

        private void LoadQuestions()
        {
            try
            {
                questions = SF.context.QuizQuestion.ToList();
                totalQuestions = questions.Count;
                if (totalQuestions == 0)
                {
                    MessageBox.Show("В базе данных нет вопросов.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    return;
                }
                UpdateQuestionCounter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке вопросов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeAnswerButtons()
        {
            AnswersPanel.Children.Clear();
            answerButtons = new List<Button>();

            for (int i = 0; i < 4; i++)
            {
                Button button = new Button
                {
                    Margin = new Thickness(0, 10, 0, 10)
                };
                button.Click += AnswerButton_Click;
                button.Tag = i;
                answerButtons.Add(button);
                AnswersPanel.Children.Add(button);
            }
        }

        private void DisplayCurrentQuestion()
        {
            if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                QuestionText.Text = question.QuestionText;
                
                answerButtons[0].Content = question.Option1;
                answerButtons[1].Content = question.Option2;
                answerButtons[2].Content = question.Option3;
                answerButtons[3].Content = question.Option4;

                UpdateQuestionCounter();
                UpdateNavigationButtons();
                currentQuestionAnswered = answeredQuestions[currentQuestionIndex];
                
                // Обновляем текст кнопки "Далее"
                var nextButton = this.FindName("NextButton") as Button;
                if (nextButton != null)
                {
                    nextButton.Content = currentQuestionIndex == totalQuestions - 1 ? "ЗАКОНЧИТЬ" : "ДАЛЕЕ";
                }
                
                // Если вопрос уже был отвечен, показываем результат
                if (currentQuestionAnswered)
                {
                    ShowAnswerResult(question);
                    ShowWarningMessage("Ответ уже дан и не может быть изменен");
                }
                else
                {
                    HideWarningMessage();
                }
            }
        }

        private void ShowAnswerResult(QuizQuestion question)
        {
            foreach (var button in answerButtons)
            {
                button.IsEnabled = false;
                int buttonIndex = (int)button.Tag;
                
                if (buttonIndex == question.CorrectAnswerIndex)
                {
                    button.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#4CAF50"));
                }
                
                // Показываем выбранный пользователем ответ
                if (buttonIndex == selectedAnswers[currentQuestionIndex])
                {
                    if (buttonIndex != question.CorrectAnswerIndex)
                    {
                        button.Background = new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#F44336"));
                    }
                    button.BorderThickness = new Thickness(3);
                    button.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFFFFF"));
                }
            }
        }

        private void ShowWarningMessage(string message)
        {
            WarningText.Text = message;
            // Анимация появления предупреждения
            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = WarningText.Opacity,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            WarningText.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void HideWarningMessage()
        {
            // Анимация скрытия предупреждения
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = WarningText.Opacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            WarningText.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void UpdateQuestionCounter()
        {
            QuestionNumberText.Text = $"Вопрос {currentQuestionIndex + 1} из {totalQuestions}";
        }

        private void UpdateNavigationButtons()
        {
            var prevButton = this.FindName("PreviousButton") as Button;
            var nextButton = this.FindName("NextButton") as Button;

            if (prevButton != null)
                prevButton.IsEnabled = currentQuestionIndex > 0;

            if (nextButton != null)
                nextButton.IsEnabled = currentQuestionAnswered || answeredQuestions[currentQuestionIndex];
        }

        private void UpdateNextButtonState()
        {
            var nextButton = this.FindName("NextButton") as Button;
            if (nextButton != null)
            {
                nextButton.IsEnabled = currentQuestionAnswered || answeredQuestions[currentQuestionIndex];
            }
        }

        private void ShowTestResults()
        {
            double percentage = (double)correctAnswersCount / totalQuestions * 100;
            string grade = percentage >= 90 ? "Отлично!" :
                         percentage >= 70 ? "Хорошо!" :
                         percentage >= 50 ? "Удовлетворительно" :
                         "Нужно подготовиться лучше";

            MessageBox.Show(
                $"Тест завершен!\n\n" +
                $"Правильных ответов: {correctAnswersCount} из {totalQuestions}\n" +
                $"Процент правильных ответов: {percentage:F1}%\n" +
                $"Оценка: {grade}",
                "Результаты теста",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && currentQuestionIndex < questions.Count && !currentQuestionAnswered)
            {
                int selectedAnswer = (int)button.Tag;
                var question = questions[currentQuestionIndex];

                foreach (var answerButton in answerButtons)
                {
                    answerButton.IsEnabled = false;
                }

                // Сохраняем выбранный ответ
                selectedAnswers[currentQuestionIndex] = selectedAnswer;

                if (selectedAnswer == question.CorrectAnswerIndex)
                {
                    button.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#4CAF50"));
                    correctAnswersCount++;
            }
            else
            {
                    button.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#F44336"));
                    answerButtons[question.CorrectAnswerIndex].Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#4CAF50"));
                }

                // Добавляем подсветку выбранного ответа
                button.BorderThickness = new Thickness(3);
                button.BorderBrush = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#FFFFFF"));

                currentQuestionAnswered = true;
                answeredQuestions[currentQuestionIndex] = true;
                UpdateNextButtonState();
                
                ShowWarningMessage("Ответ принят. Изменить его нельзя");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex == totalQuestions - 1)
            {
                ShowTestResults();
                return;
            }

            if (currentQuestionIndex < totalQuestions - 1 && answeredQuestions[currentQuestionIndex])
            {
                currentQuestionIndex++;
                DisplayCurrentQuestion();
                if (!answeredQuestions[currentQuestionIndex])
                {
                    ResetAnswerButtons();
                }
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                DisplayCurrentQuestion();
            }
        }

        private void ResetAnswerButtons()
        {
            foreach (var button in answerButtons)
            {
                button.IsEnabled = true;
                button.Background = null;
                button.BorderThickness = new Thickness(1.5);
                button.BorderBrush = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#00E5FF"));
            }
            HideWarningMessage();
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
    }
}