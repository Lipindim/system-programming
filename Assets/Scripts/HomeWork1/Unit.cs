using System;


public class Unit
{
    public int Health { get; private set; }

    public Unit(int initialHealth)
    {
        Health = initialHealth;
    }

    public void Heal(int healValue)
    {
        if (healValue < 0)
            throw new ArgumentException("Heal value must be not negative.");

        Health += healValue;
    }
}

