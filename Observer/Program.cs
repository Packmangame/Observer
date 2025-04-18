using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{

// Интерфейс наблюдателя (подписчика)
public interface IObserver
    {
        void Update(string message);
    }

    // Интерфейс субъекта (издателя)
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers();
    }

    // Конкретный субъект (издатель)
    public class NewsPublisher : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _latestNews;

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_latestNews);
            }
        }

        // Метод для изменения состояния и уведомления подписчиков
        public void PublishNews(string news)
        {
            _latestNews = news;
            NotifyObservers();
        }
    }

    // Конкретный наблюдатель (подписчик)
    public class NewsSubscriber : IObserver
    {
        private string _name;

        public NewsSubscriber(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{_name} получил новость: {message}");
        }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем издателя
            var publisher = new NewsPublisher();

            // Создаем подписчиков
            var subscriber1 = new NewsSubscriber("Подписчик 1");
            var subscriber2 = new NewsSubscriber("Подписчик 2");
            var subscriber3 = new NewsSubscriber("Подписчик 3");

            // Подписываем наблюдателей на издателя
            publisher.RegisterObserver(subscriber1);
            publisher.RegisterObserver(subscriber2);
            publisher.RegisterObserver(subscriber3);

            Console.WriteLine("Напиши новость и нажми Enter для публикации \nНовости:");
            publisher.PublishNews($"{Console.ReadLine()}");

            // Отписываем одного подписчика
            publisher.RemoveObserver(subscriber2);

            Console.WriteLine("Напиши новость и нажми Enter для публикации \nНовости:");
            publisher.PublishNews($"{Console.ReadLine()}");

            Console.ReadKey();
        }
}
       
    }
