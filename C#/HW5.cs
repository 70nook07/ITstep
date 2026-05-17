namespace CSharp;

public enum EnergyEfficiency
{
    A,
    B,
    C,
    D
}

public class Freezer
{
    private string _brand;
    private double _capacityLiters;
    private int _tempCelsius;
    private bool _isFastFreeze;
    private EnergyEfficiency _energyClass;

    // props
    public string Brand { get; set; }
    public double CapacityLiters { get; set; }
    public int TempCelsius { get; set; }
    public bool IsFastFreeze { get; set; }
    public EnergyEfficiency EnergyClass { get; set; }

    // default constructor
    public Freezer() : this("Unknown", 0, -18, false, EnergyEfficiency.A)
    {
    }

    // brand and capacity
    public Freezer(string brand, double capacityLiters) : this(brand, capacityLiters, -18, false, EnergyEfficiency.A)
    {
    }

    // brand, capacity and temperature
    public Freezer(string brand, double capacityLiters, int temperatureCelsius) : this(brand, capacityLiters,
        temperatureCelsius, false, EnergyEfficiency.A)
    {
    }

    // all params
    public Freezer(string brand, double capacityLiters, int temperatureCelsius, bool isFastFreezeActive,
        EnergyEfficiency energyClass)
    {
        _brand = brand;
        _capacityLiters = capacityLiters;
        _tempCelsius = temperatureCelsius;
        _isFastFreeze = isFastFreezeActive;
        _energyClass = energyClass;
    }

    public override string ToString()
    {
        return $"Freezer [Brand: {_brand}, Capacity: {_capacityLiters} L, " +
               $"Temperature: {_tempCelsius}°C, FastFreeze: {_isFastFreeze}, " + $"Energy Class: {_energyClass}]";
    }
}

class HW5
{
    static void Main()
    {
        Freezer[] freezers = new Freezer[]
        {
            new Freezer(), // default
            new Freezer("ArctikCool", 250.5), // brand + capacity
            new Freezer("Frostie", 300.0, -22), // brand + capacity + temp
            new Freezer("Icicle", 200.0, -20, true, EnergyEfficiency.B), // all params
            new Freezer("PolarStar", 180.0, -18, false, EnergyEfficiency.A)
        };
        Console.WriteLine("Freezer collection:");
        foreach (Freezer freezer in freezers)
        {
            Console.WriteLine(freezer.ToString());
        }
    }
}
