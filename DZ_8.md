# Домашнее задание №8 regular уровень
## Перове задание
### а)
Name - сериализуется, так как это публичное свойство
Score - тоже сериализуется по той же причине
secretCode - приватное, не сериализуется
IsAlive - поле, не сериализуется по умолчанию

### б)
Можно добавить публичный геттер-сеттер с приватным сеттером и добавить атрибут [JsonInclude]

### в)
IsAlive не попадает, так как это поле, решить проблему можно добавив атрибут [JsonInclude] или в настройках json-а поставить параметр IncludeFields = true.

## Второе задание 
### а)
`public class Game
{
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public string[] Genres { get; set; }
    public bool IsMultiplayer { get; set; }
}`

### б)
