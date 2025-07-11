using System;
using System.Collections.Generic;

namespace GameSchool.Classes_
{
    public class GameEngine
    {
        private Random _random = new Random();
        private int _num1, _num2;
        private string _operation;
        private int _currentScore;
        private int _remainingTime;

        public int Num1 => _num1;
        public int Num2 => _num2;
        public string Operation => _operation;
        public string ProblemText => $"{_num1} {Operation} {_num2} = ?";
        public int CurrentScore => _currentScore;
        public int RemainingTime => _remainingTime;

        public void StartNewGame(int difficultyLevel, int timeInSeconds)
        {
            if (difficultyLevel <= 0 || timeInSeconds <= 0)
                throw new ArgumentException("Difficulty and time must be positive.");

            _currentScore = 0;
            _remainingTime = timeInSeconds;
            GenerateProblem();
        }

        public void Tick()
        {
            _remainingTime--;
            if (_remainingTime < 0)
                _remainingTime = 0;
        }

        public bool IsTimeUp() => _remainingTime <= 0;

        public void SubmitAnswer(string answerText, out string feedback, out int correctAnswer)
        {
            if (!int.TryParse(answerText, out int userAnswer))
            {
                feedback = "Введите число.";
                correctAnswer = CalculateCorrectAnswer();
                return;
            }

            correctAnswer = CalculateCorrectAnswer();
            if (userAnswer == correctAnswer)
            {
                feedback = "Правильно!";
                _currentScore += 10;
            }
            else
            {
                feedback = $"Неверно! Правильный ответ: {correctAnswer}.";
            }

            GenerateProblem();
        }

        private void GenerateProblem()
        {
            int difficulty = GetDifficultyRange();

            _num1 = _random.Next(1, difficulty + 1);
            _num2 = _random.Next(1, difficulty + 1);

            int opIndex = _random.Next(0, 4); // 0: +, 1: -, 2: *, 3: /
            switch (opIndex)
            {
                case 0:
                    _operation = "+";
                    break;
                case 1:
                    _operation = "-";
                    if (_num1 < _num2) Swap(ref _num1, ref _num2);
                    break;
                case 2:
                    _operation = "*";
                    break;
                case 3:
                    _operation = "/";
                    _num2 = Math.Max(1, _num2); // avoid division by zero
                    _num1 = _num2 * _random.Next(1, difficulty / _num2 + 1);
                    break;
            }
        }

        private int GetDifficultyRange()
        {
            // Легкий = 10, Средний = 50, Сложный = 100
            return 10; // можно сделать параметром или расширить
        }

        private int CalculateCorrectAnswer()
        {
            switch (_operation)
            {
                case "+": return _num1 + _num2;
                case "-": return _num1 - _num2;
                case "*": return _num1 * _num2;
                case "/": return _num1 / _num2;
                default: return 0;
            }
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}