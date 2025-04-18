using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System;
using System.Collections.Generic;
using System.IO;

namespace Observer.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void NewsPublisher_RegisterObserver_ObserverAddedToList()
        {
            // Arrange
            var publisher = new NewsPublisher();
            var observer = new NewsSubscriber("Test Observer");

            // Act
            publisher.RegisterObserver(observer);

            // Assert
            // Для проверки нам нужно получить доступ к приватному полю _observers
            // В реальном проекте можно использовать рефлексию или изменить модификатор доступа для тестов
            var observersField = typeof(NewsPublisher).GetField("_observers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var observers = (List<IObserver>)observersField.GetValue(publisher);

            Assert.AreEqual(1, observers.Count);
            Assert.AreSame(observer, observers[0]);
        }

        [TestMethod]
        public void NewsPublisher_RemoveObserver_ObserverRemovedFromList()
        {
            // Arrange
            var publisher = new NewsPublisher();
            var observer1 = new NewsSubscriber("Observer 1");
            var observer2 = new NewsSubscriber("Observer 2");

            publisher.RegisterObserver(observer1);
            publisher.RegisterObserver(observer2);

            // Act
            publisher.RemoveObserver(observer1);

            // Assert
            var observersField = typeof(NewsPublisher).GetField("_observers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var observers = (List<IObserver>)observersField.GetValue(publisher);

            Assert.AreEqual(1, observers.Count);
            Assert.AreSame(observer2, observers[0]);
        }

        [TestMethod]
        public void NewsPublisher_NotifyObservers_AllObserversNotified()
        {
            // Arrange
            var publisher = new NewsPublisher();
            var observer1 = new TestObserver("Observer 1");
            var observer2 = new TestObserver("Observer 2");

            publisher.RegisterObserver(observer1);
            publisher.RegisterObserver(observer2);
            string testMessage = "Test message";

            // Act
            publisher.PublishNews(testMessage);

            // Assert
            Assert.AreEqual(testMessage, observer1.LastReceivedMessage);
            Assert.AreEqual(testMessage, observer2.LastReceivedMessage);
        }

        [TestMethod]
        public void NewsSubscriber_Update_WritesToConsole()
        {
            // Arrange
            var subscriber = new NewsSubscriber("Test Subscriber");
            string testMessage = "Test message";
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            subscriber.Update(testMessage);

            // Assert
            string expectedOutput = $"{subscriber.GetType().GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(subscriber)} получил новость: {testMessage}{Environment.NewLine}";
            Assert.AreEqual(expectedOutput, consoleOutput.ToString());
        }

        // Вспомогательный класс для тестирования уведомлений
        private class TestObserver : IObserver
        {
            public string LastReceivedMessage { get; private set; }
            private string _name;

            public TestObserver(string name)
            {
                _name = name;
            }

            public void Update(string message)
            {
                LastReceivedMessage = message;
            }
        }
    }
}
