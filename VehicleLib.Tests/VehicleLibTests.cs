using NUnit.Framework;
using System;
using VehicleLib;

namespace VehicleLib.Tests
{
    public class Tests
    {

        [Test]
        [TestCase(50, 10, 5, 0, ExpectedResult = 500)]
        [TestCase(50, 10, 5, 5, ExpectedResult = 385)]
        [TestCase(57, 12, 5, 0, ExpectedResult = 475)]
        [TestCase(52, 10, 5, -3, ExpectedResult = 520)]
        [TestCase(35, 10, 5, 1, ExpectedResult = 330)]
        public float DistanceByMaxFuelTest(int fuelCapacity, float avgFuelConsump, int passangerCapacity, int currentPassangers)
        {
            PassangerCar car = new PassangerCar(fuelCapacity, avgFuelConsump, passangerCapacity);
            car.currentPasangers = currentPassangers;
            return MathF.Round(car.DistanceByFuel(Vehicle.FuelType.Max));
        }

        [Test]
        [TestCase(50, 10, 5, 0, 40, ExpectedResult = 400)]
        [TestCase(50, 10, 5, 5, 50, ExpectedResult = 385)]
        [TestCase(57, 12, 5, 0, 999, ExpectedResult = 475)]
        [TestCase(52, 10, 5, 3, -10, ExpectedResult = 0)]
        [TestCase(35, 10, 5, 1, 30.5f, ExpectedResult = 288)]
        public float DistanceByCurrentFuelTest(int fuelCapacity, float avgFuelConsump, int passangerCapacity, int currentPassangers, float currentFuel)
        {
            PassangerCar car = new PassangerCar(fuelCapacity, avgFuelConsump, passangerCapacity);
            car.currentPasangers = currentPassangers;
            car.currentFuel = currentFuel;
            return MathF.Round(car.DistanceByFuel(Vehicle.FuelType.Current));
        }
    }
}