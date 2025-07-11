using System;
using System.Windows;
using GameSchool.Classes_;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameSchool.Windows_
{
    public partial class AddQuizWindow : Window
    {
        private int currentQuizId;
        private int questionsAdded = 0;

        public AddQuizWindow(int quizId)
        {
            InitializeComponent();
            currentQuizId = quizId;
            
            // Если quizId = 0, создаем новую викторину
            if (quizId == 0)
            {
                try
                {
                    var newQuiz = new Quiz
                    {
                        Name = txtQuestionText.Text
                    };

                    SF.context.Quiz.Add(newQuiz);
                    SF.context.SaveChanges();
                    currentQuizId = newQuiz.ID;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании викторины: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }
            }
            
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            this.Title = $"Добавить вопрос (добавлено: {questionsAdded})";
        }

        private void ClearForm()
        {
            txtQuestionText.Text = "";
            txtOption1.Text = "";
            txtOption2.Text = "";
            txtOption3.Text = "";
            txtOption4.Text = "";
            cmbCorrectAnswer.SelectedIndex = -1;
            txtQuestionText.Focus();
        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            string question = txtQuestionText.Text.Trim();
            string option1 = txtOption1.Text.Trim();
            string option2 = txtOption2.Text.Trim();
            string option3 = txtOption3.Text.Trim();
            string option4 = txtOption4.Text.Trim();
            var selectedItem = cmbCorrectAnswer.SelectedItem as ComboBoxItem;

            if (string.IsNullOrWhiteSpace(question) ||
                string.IsNullOrWhiteSpace(option1) ||
                string.IsNullOrWhiteSpace(option2) ||
                string.IsNullOrWhiteSpace(option3) ||
                string.IsNullOrWhiteSpace(option4) ||
                selectedItem == null)
            {
                MessageBox.Show("Заполните все поля и выберите правильный ответ.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int correctIndex = int.Parse(selectedItem.Tag.ToString());

            try
            {
                // Проверяем существование викторины
                var quiz = SF.context.Quiz.FirstOrDefault(q => q.ID == currentQuizId);
                if (quiz == null)
                {
                    MessageBox.Show("Ошибка: Викторина не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newQuestion = new QuizQuestion
                {
                    QuestionText = question,
                    Option1 = option1,
                    Option2 = option2,
                    Option3 = option3,
                    Option4 = option4,
                    CorrectAnswerIndex = correctIndex,
                    QuizID = currentQuizId
                };

                SF.context.QuizQuestion.Add(newQuestion);
                SF.context.SaveChanges();

                questionsAdded++;
                UpdateTitle();

                var result = MessageBox.Show("Вопрос успешно сохранен! Хотите добавить еще один вопрос?",
                    "Успех", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ClearForm();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (questionsAdded == 0)
            {
                try
                {
                    // Если вопросов не добавлено, удаляем пустую викторину
                    var quiz = SF.context.Quiz.FirstOrDefault(q => q.ID == currentQuizId);
                    if (quiz != null)
                    {
                        SF.context.Quiz.Remove(quiz);
                        SF.context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пустой викторины: {ex.Message}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
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
            this.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}