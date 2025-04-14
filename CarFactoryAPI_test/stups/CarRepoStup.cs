using CarAPI.Entities;
using CarAPI.Repositories_DAL;
using System;
using System.Collections.Generic;

namespace CarFactoryAPI_test.stups
{
    internal class CarRepoStup : ICarsRepository
    {
        private List<Car> cars = new List<Car>(); // Simulated collection of cars

        public bool AddCar(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            // Simulate adding car to the collection
            cars.Add(car);
            return true;
        }

        public bool AssignToOwner(int carId, int ownerId)
        {
            // For the purpose of this stub, assume that assigning to an owner always succeeds
            return true;
        }

        public List<Car> GetAllCars()
        {
            // Return all cars in the collection
            return cars;
        }

        public Car GetCarById(int id)
        {
            // Find and return the car with the specified id, or null if not found
            return cars.Find(car => car.Id == id);
        }

        public bool Remove(int carId)
        {
            // Simulate removing car from the collection
            return cars.RemoveAll(car => car.Id == carId) > 0;
        }
    }
}
