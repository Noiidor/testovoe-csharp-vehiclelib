using System;

namespace VehicleLib
{
    
    public class Vehicle
    {
        public string vehicleType;
        public float avgFuelConsump;
        public float fuelCapacity;
        public float currentFuel;
        public float speed;

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

        public float DistanceByFuel(FuelType type)
        {
            if (type == FuelType.Current)
            {
                return (currentFuel / avgFuelConsump) * 100;
            }
            else
            {
                return (fuelCapacity / avgFuelConsump) * 100;
            }
            
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
}
