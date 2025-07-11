using System;

namespace GameSchool.Classes_
{
    public class MathProblem
    {
        public string Expression { get; private set; }
        public double CorrectAnswer { get; private set; }
        public DifficultyLevel Difficulty { get; private set; }
        public OperationType Operation { get; private set; }

        public enum DifficultyLevel
        {
            Easy,
            Medium,
            Hard
        }

        public enum OperationType
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Mixed
        }

        private static readonly Random random = new Random();

        public MathProblem(DifficultyLevel difficulty, OperationType operation)
        {
            Difficulty = difficulty;
            Operation = operation;
            GenerateProblem();
        }

        private void GenerateProblem()
        {
            int num1, num2;
            switch (Difficulty)
            {
                case DifficultyLevel.Easy:
                    num1 = random.Next(1, 21); // 1-20
                    num2 = random.Next(1, 21);
                    break;
                case DifficultyLevel.Medium:
                    num1 = random.Next(10, 101); // 10-100
                    num2 = random.Next(10, 101);
                    break;
                case DifficultyLevel.Hard:
                    num1 = random.Next(50, 501); // 50-500
                    num2 = random.Next(50, 501);
                    break;
                default:
                    num1 = num2 = 0;
                    break;
            }

            if (Operation == OperationType.Mixed)
            {
                Operation = (OperationType)random.Next(0, 4);
            }

            switch (Operation)
            {
                case OperationType.Addition:
                    Expression = $"{num1} + {num2} = ?";
                    CorrectAnswer = num1 + num2;
                    break;
                case OperationType.Subtraction:
                    Expression = $"{num1} - {num2} = ?";
                    CorrectAnswer = num1 - num2;
                    break;
                case OperationType.Multiplication:
                    // Уменьшаем числа для умножения
                    num1 = num1 / (Difficulty == DifficultyLevel.Hard ? 10 : 5);
                    num2 = num2 / (Difficulty == DifficultyLevel.Hard ? 10 : 5);
                    Expression = $"{num1} × {num2} = ?";
                    CorrectAnswer = num1 * num2;
                    break;
                case OperationType.Division:
                    // Генерируем числа, которые делятся нацело
                    CorrectAnswer = random.Next(1, Difficulty == DifficultyLevel.Hard ? 21 : 11);
                    num2 = random.Next(1, Difficulty == DifficultyLevel.Hard ? 11 : 6);
                    num1 = (int)(CorrectAnswer * num2);
                    Expression = $"{num1} ÷ {num2} = ?";
                    break;
            }
        }

        public bool CheckAnswer(double userAnswer)
        {
            return Math.Abs(userAnswer - CorrectAnswer) < 0.0001; // Учитываем погрешность для дробных чисел
        }
    }
} 