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
        [TestCase(52, 10, 5, 3, ExpectedResult = 441)]
        [TestCase(35, 10, 5, 1, ExpectedResult = 330)]
        public float DistanceByMaxFuelTest(int fuelCapacity, float avgFuelConsump, int passangerCapacity, int currentPassangers)
        {
            PassangerCar car = new PassangerCar(fuelCapacity, avgFuelConsump, passangerCapacity);
            car.currentPasangers = currentPassangers;
            return MathF.Round(car.DistanceByFuel(Vehicle.FuelType.Max));
        }
    }
}