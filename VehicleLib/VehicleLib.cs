using System;

namespace VehicleLib
{
    
    public abstract class Vehicle
    {
        public string vehicleType { get; set; }

        public float speed { get; set; }

        public int fuelCapacity
        {
            get { return fuelCapacity; }
            set { FuelCut(value); }
        }
        public abstract float avgFuelConsump { get; set; }
        public float currentFuel
        {
            get { return currentFuel; }
            set { Math.Clamp(value, 0, fuelCapacity); }
        }
        
        public enum Time
        {
            Hours,
            Minutes,
            Seconds
        }

        public enum FuelType
        {
            Current,
            Max
        }

        public Vehicle(int fuelCapacity, float avgFuelConsump)
        {
            this.fuelCapacity = fuelCapacity;
            this.avgFuelConsump = avgFuelConsump;
        }

        // Если объем бака изменяется на значение, меньше текущего запаса топлива, то запас топлива обрезается до значения объема бака.
        private int FuelCut(int value)
        {
            if (value < currentFuel)
            {
                currentFuel = Math.Clamp(currentFuel, 0, value);
            }
            return value;
        }

        public float DistanceByFuel(FuelType type)
        {
            return ((type == FuelType.Current) ? currentFuel : fuelCapacity) / avgFuelConsump * 100;
        }

        public void Info()
        {
            Console.WriteLine($"Power reserve: {DistanceByFuel(FuelType.Current)} kilometers");
        }

        public float TimeByDistance(float distance, Time time)
        {
            switch (time)
            {
                case Time.Hours:
                    return distance / speed;
                case Time.Minutes:
                    return (distance / speed) * 60;
                case Time.Seconds:
                    return (distance / speed) * 3600;
                default:
                    throw new Exception("Invalid time argument");
            }
        }
    }

    public class PassangerCar : Vehicle
    {
        
        public override float avgFuelConsump
        {
            // Геттер возвращает уже подверженное "штрафу" от кол-ва пассажиров значение.
            get { return AffectedFuelConsump(); }
            set { avgFuelConsump = value; }
        }

        public int passangerCapacity
        {
            get { return passangerCapacity; }
            set { PassangersCut(value); }
        }
        public int currentPasangers
        {
            get { return currentPasangers; }
            set { Math.Clamp(value, 0, passangerCapacity); }
        }

        public PassangerCar(int fuelCapacity, float avgFuelConsump, int passangerCapacity) : base(fuelCapacity, avgFuelConsump)
        {
            vehicleType = "PassangerCar";
            this.passangerCapacity = passangerCapacity;
        }

        private int PassangersCut(int value)
        {
            if (value < currentPasangers)
            {
                currentPasangers = Math.Clamp(currentPasangers, 0, value);
            }
            return value;
        }

        // Расход топлива рассчитывается с учетом текущего кол-ва пассажиров.
        // Количество пассажиров влияет на расход топлива, что в свою очередь влияет на запас хода.
        private float AffectedFuelConsump()
        {
            float percentage = 1 + (currentPasangers * 0.06f);
            return avgFuelConsump * percentage;
        }
    }
}
