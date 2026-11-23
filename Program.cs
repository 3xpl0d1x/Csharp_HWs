using System.Net.Http.Headers;

public interface ICombatant
{
    public string Name { get; }
    public int HP { get; set; }
    public bool IsDead { get; }
    public int Damage { get; }
    public void Attack(ICombatant target);
    public void TakeDamage(int amount);
}
public class Knight : ICombatant
{
    public string Name { get; }
    public int HP { get; set; }
    public bool IsDead => HP <= 0;
    public int Damage { get; }
    public Knight (string name)
    {
        Name = name;
        HP = 150;
        Damage = 25;
    }

    public void Attack(ICombatant target)
    {
        if (IsDead)
        {
            Console.WriteLine("Knight is dead, dead cannot attack!");
            return;
        }
        if (target.IsDead)
        {
            Console.WriteLine("Knight's target is dead, cannot attack corpses!");
            return;
        }
        Console.WriteLine($"{Name}: No mercy for my enemies!");
        target.TakeDamage(Damage);
    }
    public void TakeDamage(int amount)
    {
        HP = Math.Max(0, HP - amount);
        Console.WriteLine($"Knight {Name} recieved {amount} damage. {HP} HP left");
    }

}
public class Mage : ICombatant
{
    public string Name { get; }
    public int HP { get; set; }
    public bool IsDead => HP <= 0;
    public int Damage { get; }
    public Mage(string name)
    {
        Name = name;
        HP = 80;
        Damage = 40;
    }

    public void Attack(ICombatant target)
    {
        if (IsDead)
        {
            Console.WriteLine("Mage is dead, dead cannot attack!");
            return;
        }
        if (target.IsDead)
        {
            Console.WriteLine("Target is dead, no need to waste mana!");
            return;
        }
        Console.WriteLine($"{Name}: Feel my power!!");
        target.TakeDamage(Damage);
        if (!target.IsDead)
        {
            target.TakeDamage(Damage);
        }
        
    }
    public void TakeDamage(int amount)
    {
        HP = Math.Max(0, HP - amount);
        Console.WriteLine($"Mage {Name} recieved {amount} damage. {HP} HP left");
    }
}
public enum Rarity
{
    Common,
    Rare,
    Legendary
}
public class Player
{
    public string Name { get; }
    public int HP { get; private set; }
    public int Damage { get; private set; }

    public Player(string name, int hp = 10, int damage = 10)
    {
        Name = name;
        HP = hp;
        Damage = damage;
    }

    public void Heal(int amount)
    {
        HP += amount;
        Console.WriteLine($"{Name} restored {amount} HP. Now: {HP} HP");
    }
    public void IncreaseDamage(int amount)
    {
        Damage += amount;
        Console.WriteLine($"{Name} became stronger for {amount}. Now: {Damage} damage");
    }
    public void TakeDamage(int amount)
    {
        HP = Math.Min(0, HP - amount);
        Console.WriteLine($"{Name} recieved {amount} damage. Now: {HP} HP");
    }
}

public interface IUsableItem
{
    string Name { get; }
    string Description { get; }
    Rarity Rarity { get; }
    void Use(Player player);
}
public class HealthPotion : IUsableItem
{
    public string Name => "Health potion";
    public string Description => "Restores 50 HP";
    public Rarity Rarity => Rarity.Common;
    public void Use(Player player)
    {
        player.Heal(50);
        Console.WriteLine($"{player.Name} uses {Name} and restored 50 HP!");
    }
}
public class Sword : IUsableItem
{
    public string Name => "Cool sword";
    public string Description => "Increses damage by 10";
    public Rarity Rarity => Rarity.Rare;
    public void Use(Player player)
    {
        player.IncreaseDamage(10);
        Console.WriteLine($"{player.Name} took a {Name} and became stronger by 10");
    }
}
public class AncientScroll : IUsableItem
{
    public string Name => "Ancient scroll";
    public string Description => $"Cursed! -20 HP, but +30 damage. Deal?";
    public Rarity Rarity => Rarity.Legendary;
    public void Use(Player player)
    {
        player.TakeDamage(20);
        player.IncreaseDamage(30);
        Console.WriteLine($"This scroll is cursed! {player.Name} loses 20 HP, but now is stronger and deals +10 damage");
    }
}
public interface IDamageCalculator
{
    int CalculateDamage(int baseDamage, int targetArmor);
}
public class PhysicalDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        return Math.Max(1,baseDamage - targetArmor/2);
    }
}
public class MagicDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor/4);
        if (targetArmor == 0)
        {
            damage *= 2;
        }
        return damage;
    }
}
public class FireDamageCalculator : IDamageCalculator
{
    public int CalculateDamage(int baseDamage, int targetArmor)
    {
        int damage = Math.Max(1, baseDamage - targetArmor / 10);
        if (baseDamage > 50)
        {
            damage += 10;
        }
        return damage;
    }
}
public class Unit
{
    public string Name { get; }
    public int HP { get; set; }
    public int Armor { get; }
    public Unit(string name, int hp, int armor)
    {
        Name = name;
        HP = hp;
        Armor = armor;
    }
}
public class DamageSystem
{
    private IDamageCalculator _calculator;
    public DamageSystem(IDamageCalculator calculator)
    {
        _calculator = calculator;
    }

    public void ApplyDamage(Unit target, int baseDamage)
    {
        int damage = _calculator.CalculateDamage(baseDamage, target.Armor);
        target.HP = Math.Max(0, target.HP - damage);
        Console.WriteLine($"Unit {target.Name} recieved {damage} damage. Now: {target.HP} HP");
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== 2.1 ===");
        Knight knight = new Knight("Bobby");
        Mage mage = new Mage("Magnum");

        while (!knight.IsDead && !mage.IsDead)
        {
            knight.Attack(mage);
            if (mage.IsDead) break;

            mage.Attack(knight);
        }
        if (knight.IsDead) Console.WriteLine($"{knight.Name} had fallen under enormous power of {mage.Name}.");
        else Console.WriteLine($"{mage.Name} is so weak he died under {knight.Name}'s pressure");


        Console.WriteLine("=== 2.2 ===");
        Player player = new Player("Hero");

        List<IUsableItem> items = new List<IUsableItem>
        {
            new HealthPotion(),
            new Sword(),
            new AncientScroll(),
        };

        Console.WriteLine("Uses all items");
        foreach (var item in items)
        {
            Console.WriteLine($"[{item.Rarity}] {item.Name}: {item.Description}");
            item.Use(player);
        }
        Console.WriteLine("Final state:");
        Console.WriteLine($"Player: {player.Name}");
        Console.WriteLine($"HP: {player.HP}");
        Console.WriteLine($"Damage: {player.Damage}");
        Console.WriteLine(player.HP > 0 ? "Player is alive!" : "Player is dead");

        Console.WriteLine("=== 2.3 ===");
        Unit unit = new Unit("Zbadriever", 500, 40);
        DamageSystem phys = new DamageSystem(new PhysicalDamageCalculator());
        DamageSystem mag = new DamageSystem(new MagicDamageCalculator());
        DamageSystem fire = new DamageSystem(new FireDamageCalculator());

        phys.ApplyDamage(unit, 60);
        mag.ApplyDamage(unit, 60);
        fire.ApplyDamage(unit, 60);
    }
}
/*
 * Вопрос для размышления:
 * 
 * Мы создаем новый класс реализующий IDamageCalculator, в котором прописываем свою логику получения урона
 * DamageSystem не трогаем, так как там работаем с интерфейсом и его логикой
 */