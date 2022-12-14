using System;

namespace VehicleLib
{
    // Задание реалозовано при помощи паттерна "Стратегия"
    public abstract class Vehicle
    {
        public string vehicleType;

        public float speed;

        private float _fuelCapacity;
        public float fuelCapacity
        {
            get { return _fuelCapacity; }
            // Если объем бака изменяется на значение, меньше текущего запаса топлива, то запас топлива обрезается до значения объема бака.
            set { _fuelCapacity =  value < 0 ? 0 : ValueCut(value, ref _currentFuel); }
        }

        private protected float _avgFuelConsump;
        public float avgFuelConsump
        {
            get { return AffectedFuelConsump(); } // Возвращается значение, подверженное внешним факторам, которые могут влиять на запас хода.
            set { _avgFuelConsump = value; }
        }

        private float _currentFuel;
        public float currentFuel
        {
            get { return _currentFuel; }
            set { _currentFuel = Math.Clamp(value, 0, fuelCapacity); } // Текущее топливо не может быть меньше нуля или больше объема бака.
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
        
        /// <param name="fuelCapacity"><see cref="Vehicle"/> fuel capacity in liters.</param>
        /// <param name="avgFuelConsump"><see cref="Vehicle"/> fuel consumption in liters per 100 km.</param>
        public Vehicle(int fuelCapacity, float avgFuelConsump) // Родительский конструктор, который наследуют дочерние классы.
        {
            this.fuelCapacity = fuelCapacity;
            this.avgFuelConsump = avgFuelConsump;
        }

        // Если запас хода должен изменяться в зависимости от каких-либо внешних факторов, то нужно переопределить этот метод.
        private protected virtual float AffectedFuelConsump()
        {
            return _avgFuelConsump;
        }

        // Метод, используемый для "обрезания" значения текущего "чего-либо", если Capacity изменяется на меньшее значение.
        private protected float ValueCut(float value, ref float cut)
        {
            if (value < cut)
            {
                cut = Math.Clamp(cut, 0, value);
            }
            return value;
        }
        private protected int ValueCut(int value, ref int cut)
        {
            if (value < cut)
            {
                cut = Math.Clamp(cut, 0, value);
            }
            return value;
        }

        // Метод для вычисления расстояния, которое ТС сможет проехать на полном/оставшемся запасе топлива.
        // Выбор между максимальным и текущим топливом можно было сделать через простой bool, но по-моему визуально лучше воспринимается enum

        /// <summary>
        /// Calculates the distance that <see cref="Vehicle"/> will travel on current/maximum fuel.
        /// </summary>
        /// <param name="type">Use maximum or current fuel for calculation</param>
        /// <returns></returns>
        public float DistanceByFuel(FuelType type)
        {
            return ((type == FuelType.Current) ? currentFuel : fuelCapacity) / avgFuelConsump * 100;
        }

        // Метод для отображения информации о запасе хода и типе ТС.
        /// <summary>
        /// Displays information about <see cref="Vehicle"/> power reserve and <see cref="vehicleType"/>.
        /// </summary>
        public void Info()
        {
            Console.WriteLine($"This is {vehicleType}");
            Console.WriteLine($"Power reserve: {DistanceByFuel(FuelType.Current)} kilometers");
        }

        // Метод для вычисления количества времени, требуемого что бы проехать заданное расстояние.
        // Если автомобилю не хватит топлива, что бы проехать заданное расстояние(или скорость нулевая), то возвращается 0.

        /// <summary>
        /// Calculates how much time need to travel to given distance.
        /// </summary>
        /// <param name="distance">Distance in kilometers.</param>
        /// <param name="time">Calculate time in seconds, minutes or hours.</param>
        /// <returns></returns>
        public float TimeByDistance(float distance, Time time)
        {
            float maxDistance = DistanceByFuel(FuelType.Current);
            if (distance > maxDistance)
            {
                return 0;
            }
            float travelTime = speed > 0 ? distance / speed : 0;
            switch (time)
            {
                case Time.Hours:
                    return travelTime;
                case Time.Minutes:
                    return travelTime * 60;
                case Time.Seconds:
                    return travelTime * 3600;
                default:
                    throw new Exception("Invalid time argument");
            }
        }
    }

    public class PassangerCar : Vehicle
    {
        private int _passangerCapacity;
        public int passangerCapacity
        {
            get { return _passangerCapacity; }
            set { _passangerCapacity = value < 0 ? 0 : ValueCut(value, ref _currentPasangers); }
        }

        private int _currentPasangers;
        public int currentPasangers
        {
            get { return _currentPasangers; }
            set { _currentPasangers = Math.Clamp(value, 0, passangerCapacity); }
        }

        
        /// <inheritdoc cref="Vehicle.Vehicle(int, float)"/>
        public PassangerCar(int fuelCapacity, float avgFuelConsump, int passangerCapacity) : base(fuelCapacity, avgFuelConsump)
        {
            vehicleType = "PassangerCar";
            this.passangerCapacity = passangerCapacity;
        }

        // Каждый пассажир уменьшает запас хода на 6%.
        // Количество пассажиров влияет на расход топлива, что в свою очередь влияет на запас хода.
        private protected override float AffectedFuelConsump()
        {
            float percentage = 1 + (currentPasangers * 0.06f);
            return _avgFuelConsump * percentage;
        }
    }

    public class CargoCar : Vehicle
    {
        private int _loadCapacity;
        public int loadCapacity
        {
            get { return _loadCapacity; }
            set { _loadCapacity = value < 0 ? 0 : ValueCut(value, ref _currentLoad); }
        }

        private int _currentLoad;
        public int currentLoad
        {
            get { return _currentLoad; }
            set { FullLoad(value); }
        }

        /// <inheritdoc cref="Vehicle.Vehicle(int, float)"/>
        /// <param name="loadCapacity">How much cargo in kilograms <see cref="Vehicle"/> can carry.</param>
        public CargoCar(int fuelCapacity, float avgFuelConsump, int loadCapacity) : base(fuelCapacity, avgFuelConsump)
        {
            vehicleType = "CargoCar";
            this.loadCapacity = loadCapacity;
        }

        // Не совсем понятно что означает "полный груз" в просьбе "Дополните класс проверкой может ли автомобиль принять полный груз на борт",
        // Поэтому добавил простую проверку на превышение грузоподъемности.
        private void FullLoad(int value)
        {
            if (value > loadCapacity)
            {
                throw new Exception("Cannot load so much cargo");
            }
            else
            {
                _currentLoad = value;
            }
            
        }

        // Каждые 200 кг груза уменьшают запас хода на 4%.
        private protected override float AffectedFuelConsump()
        {
            float percentage = 1 + ((currentLoad/200) * 0.04f);
            return _avgFuelConsump * percentage;
        }
    }
    // В задании был упомянут класс спортивного автомобиля, но никаких спецификаций предоставлено не было,
    // Видимо задание недоработано.

}
