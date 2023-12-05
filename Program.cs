using System;
using System.Collections.Generic;

public static class EventSystem
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    public static void CreateEvent(string eventName)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = null;
        }
        else
        {
            Console.WriteLine($"Событие с именем {eventName} уже зарегистрировано");
        }
    }

    public static void RemoveEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary.Remove(eventName);
        }
        else
        {
            Console.WriteLine($"Событие с именем {eventName} не зарегистрировано");
        }
    }

    public static void RaiseEvent(string eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out Action callback))
        {
            if (callback != null)
            {
                callback.Invoke();
            }
            else
            {
                Console.WriteLine($"Событие с именем {eventName} не имеет подписчиков");
            }
        }
        else
        {
            Console.WriteLine($"Событие с именем {eventName} не зарегистрировано");
        }
    }

    public static void Clear()
    {
        eventDictionary.Clear();
    }

    public static void Subscribe(string eventName, Action callBack)
    {
        if (eventDictionary.TryGetValue(eventName, out Action callback))
        {
            callback += callBack;
            eventDictionary[eventName] = callback;
        }
        else
        {
            Console.WriteLine($"Событие с именем {eventName} не зарегистрировано");
        }
    }

    public static void UnSubscribe(string eventName, Action callBack)
    {
        if (eventDictionary.TryGetValue(eventName, out Action callback))
        {
            callback -= callBack;
            eventDictionary[eventName] = callback;
        }
        else
        {
            Console.WriteLine($"Событие с именем {eventName} не зарегистрировано");
        }
    }
}

public class Player
{
    public Player()
    {
        EventSystem.Subscribe("AttackWaveStart", OnAttackWaveStart);
        EventSystem.Subscribe("AttackWaveEnd", OnAttackWaveEnd);
    }

    public void PrepareToAttack()
    {
        // Подписываемся на события
        EventSystem.Subscribe("AttackWaveStart", OnAttackWaveStart);
        EventSystem.Subscribe("AttackWaveEnd", OnAttackWaveEnd);
    }

    private void OnAttackWaveStart()
    {
        Console.WriteLine($"Сейчас {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}, я готов к отражению атаки монстров!");
    }

    private void OnAttackWaveEnd()
    {
        Console.WriteLine("Я успешно отбился от врагов и теперь могу вернуться домой");
        // Отписываемся от события AttackWaveEnd
        EventSystem.UnSubscribe("AttackWaveEnd", OnAttackWaveEnd);
    }
}

public class OtherClass
{
    public OtherClass()
    {
        EventSystem.Subscribe("AttackWaveStart", OnAttackWaveStart);
        EventSystem.Subscribe("AttackWaveEnd", OnAttackWaveEnd);
    }

    private void OnAttackWaveStart()
    {
        Console.WriteLine("Другой класс: Атака началась!");
    }

    private void OnAttackWaveEnd()
    {
        Console.WriteLine("Другой класс: Атака окончена!");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Создаем события
        EventSystem.CreateEvent("AttackWaveStart");
        EventSystem.CreateEvent("AttackWaveEnd");

        // Создаем объекты классов
        Player player = new Player();
        OtherClass otherClass = new OtherClass();

        // Вызываем события два раза подряд
        EventSystem.RaiseEvent("AttackWaveStart");
        EventSystem.RaiseEvent("AttackWaveStart");
        EventSystem.RaiseEvent("AttackWaveEnd");
        EventSystem.RaiseEvent("AttackWaveEnd");

        // Очищаем систему событий
        EventSystem.Clear();

        // Повторное вызов событий
        EventSystem.RaiseEvent("AttackWaveStart");
        EventSystem.RaiseEvent("AttackWaveEnd");
    }
}