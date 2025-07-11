using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSchool.Classes_;
using System.Linq;

namespace GameSchool.Tests
{
    [TestClass]
    public class MainWindowTests
    {
        private GameEngine _gameEngine;

        [TestInitialize]
        public void TestInitialize()
        {
            _gameEngine = new GameEngine();
            _gameEngine.StartNewGame(10, 30); // Легкий уровень, 30 секунд
        }

        [TestMethod]
        public void StartNewGame_ShouldSetInitialValuesCorrectly()
        {
            Assert.AreEqual(0, _gameEngine.CurrentScore);
            Assert.AreEqual(30, _gameEngine.RemainingTime);
            Assert.IsFalse(string.IsNullOrEmpty(_gameEngine.ProblemText));
        }

        [TestMethod]
        public void SubmitCorrectAnswer_ShouldIncreaseScore()
        {
            int correctAnswer = _gameEngine.Num1 + _gameEngine.Num2;

            _gameEngine.SubmitAnswer(correctAnswer.ToString(), out string feedback, out _);

            Assert.AreEqual(10, _gameEngine.CurrentScore);
            Assert.IsTrue(feedback.Contains("Правильно"));
        }

        [TestMethod]
        public void SubmitIncorrectAnswer_ShouldNotIncreaseScore()
        {
            _gameEngine.SubmitAnswer("999", out string feedback, out _);

            Assert.AreEqual(0, _gameEngine.CurrentScore);
            Assert.IsTrue(feedback.Contains("Неверно"));
        }

        [TestMethod]
        public void Tick_DecreasesRemainingTime()
        {
            _gameEngine.Tick();

            Assert.AreEqual(29, _gameEngine.RemainingTime);
        }

        [TestMethod]
        public void IsTimeUp_ReturnsFalse_WhenTimeLeft()
        {
            var result = _gameEngine.IsTimeUp();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTimeUp_ReturnsTrue_WhenTimeIsZero()
        {
            for (int i = 0; i < 31; i++)
            {
                _gameEngine.Tick();
            }

            var result = _gameEngine.IsTimeUp();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SubtractionProblem_ShouldHavePositiveResult()
        {
            _gameEngine.SubmitAnswer("999", out _, out _);

            if (_gameEngine.Operation == "-")
            {
                Assert.IsTrue(_gameEngine.Num1 >= _gameEngine.Num2);
            }
        }

        
    }
}