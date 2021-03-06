﻿using System;
using System.Collections.Generic;
using System.Text;
using Ovning5.Vehicles;

namespace Ovning5
{
    public class GarageManager : IManager
    {
        IUI ui = new UI();
        private static int nrOfVehicles;
        GarageHandler garageHandler;


        public GarageManager()
        {
            nrOfVehicles = Util.AskForPositiveInt("This is a garage application. Enter how many vehicles the garage should have capacity for", ui);
            garageHandler = new GarageHandler(nrOfVehicles);
        }

        public void PrintMenu()
        {

            do
            {
                ui.Print("1. Populate the garage with som vehicles " +
                    "\n2. Print all vehicles in the garage " +
                    "\n3. Print vehicle types and how many of each there are in the garage" +
                    "\n4. Park a vehicle " +
                    "\n5. Pick up a vehicle" +
                    "\n6. Get information about the vehicle by giving the registration number" +
                    "\n7. Get a group of vehicles with certain attributes" +
                    "\nQ. Quit application");
                switch (ui.GetInput())
                {
                    case "1":
                        SeedData();
                        break;
                    case "2":
                        PrintAll();
                        break;
                    case "3":
                        PrintVehicleTypes();
                        break;
                    case "4":
                        Park();
                        break;
                    case "5":
                        PickUp();
                        break;
                    case "6":
                        FindVehicleWithRegNr();
                        break;
                    case "7":
                        FindVehicle();

                        break;
                    case "Q":
                        Environment.Exit(0);
                        break;
                    default:
                        ui.Print("Wrong choice, try again");
                        break;
                };

            } while (true);
        }

        private void FindVehicle()
        {
            ui.Print("In order to find the vehicle in the garage you must specify some characteristics of it.");
            int choice = 0;
            string vehicleType = "";
            do
            {
                choice = Util.AskForPositiveInt("Give the vehicle type: \n1. Car \n2." +
                    " Bus \n3. Boat \n4. Motorcycle \n5. Airplane \n6 don't know", ui);

                switch (choice)
                {
                    case 1:
                        vehicleType = "Car";
                        break;
                    case 2:
                        vehicleType = "Bus";
                        break;
                    case 3:
                        vehicleType = "Boat";
                        break;
                    case 4:
                        vehicleType = "Motorcycle";
                        break;
                    case 5:
                        vehicleType = "Airplane";
                        break;
                    case 6:
                        vehicleType = "none";
                        break;
                    default:
                        ui.Print("Wrong choice!");
                        break;
                }
            } while (choice < 1 || choice > 6);

            do
            {
                choice = Util.AskForPositiveInt("Do you know the color of your vehicle \n1. Yes \n2. No", ui);
            } while (choice < 1 || choice > 2);
            string color = "none";
            if (choice == 1)
                color = Util.AskForAlphabets("What color does the vehicle you want have?", ui).Trim().ToLower();

            do
            {
                choice = Util.AskForPositiveInt("Do you know how many wheels your vehicle have \n1. Yes \n2. No", ui);
            } while (choice < 1 || choice > 2);
            int nrOfWheels = -1;
            if (choice == 1)
                nrOfWheels = Util.AskForPositiveInt("How many wheels does the you are seeking have?: ", ui);

            var tupleList = new List<(string, string)>();
            if (color != "none")
                tupleList.Add(("Color", color));
            if (nrOfWheels != -1)
                tupleList.Add(("NrOfWheels", nrOfWheels.ToString()));

            ui.Print(garageHandler.FindVehiclesByPropertyValues(tupleList, vehicleType));


        }

        private void FindVehicleWithRegNr()
        {
            string regNr = Util.AskForString("Registration number: ", ui);

            IVehicle v = garageHandler.GetVehicleByRegNr(regNr);

            if (v != null)
                ui.Print($"Vehicle type: {v.GetType().Name} Regnr: {v.RegNr} Number of wheels {v.NrOfWheels} Color {v.Color}");
            else
                ui.Print("A vehicle with that registration number does not exist in the garage");
            ui.Print("");
        }

        private void PrintAll()
        {
            if (string.IsNullOrEmpty(garageHandler.PrintAll()))
                ui.Print("There are no vehicles in the garage");
            else
                ui.Print(garageHandler.PrintAll());

        }

        private void PickUp()
        {

            string regNr = Util.AskForString("Give the registration number of the vehicle you want to pick up", ui);
            if (garageHandler.Remove(regNr))
                ui.Print($"The vehicle with registration nr: {regNr} was picked up");
            else
                ui.Print($"A vehicle with registration nr: {regNr} is not parked in the garage");

            ui.Print("");
        }

        private void PrintVehicleTypes()
        {
            var result = garageHandler.GetVehicleTypes();
            ui.Print(result);
        }

        private void Park()
        {
            int vehicleType;
            do
            {
                vehicleType = Util.AskForPositiveInt("Enter which type of vehicle you want to park \n1. Car \n2. Bus \n3. Boat \n4. Motorcycle \n5. Airplane", ui);
            } while (vehicleType < 1 || vehicleType > 5);

            ui.Print("Enter the three required vehicle data");

            string regNr;
            regNr = Util.AskForString("Registration number: ", ui);
            while (!garageHandler.UniqueRegNr(regNr))
                regNr = Util.AskForString("A vehicle with that registration number is already parked in the garage. Give new registration number", ui);

            int nrOfWheels = Util.AskForPositiveInt("Number of wheels: ", ui);
            string color = Util.AskForAlphabets("Color: ", ui);
            string fuelType;
            int nrOfSeats;
            double length;
            double cylinderVolume;
            double wingSpan;

            IVehicle vehicle = null;
            switch (vehicleType)
            {
                case 1:
                    do
                    {
                        string input1 = Util.AskForString("Enter fueltype for the car \n1. Gasoline \n2. Diesel", ui);

                        if (input1 == "1")
                        {
                            fuelType = "Gasoline";
                            break;
                        }
                        else if (input1 == "2")
                        {
                            fuelType = "Diesel";
                            break;
                        }
                        else
                            ui.Print("Enter a valid fuel type");
                    } while (true);
                    vehicle = new Car(regNr, nrOfWheels, color, fuelType);
                    break;
                case 2:
                    nrOfSeats = Util.AskForPositiveInt("Enter how many seats the bus have", ui);
                    vehicle = new Bus(regNr, nrOfWheels, color, nrOfSeats);
                    break;
                case 3:
                    length = Util.AskForPositiveDouble("Enter the length of the boat", ui);
                    vehicle = new Boat(regNr, nrOfWheels, color, length);
                    break;
                case 4:
                    cylinderVolume = Util.AskForPositiveDouble("Enter the cylinder volume of the motorcycle", ui);
                    vehicle = new Motorcycle(regNr, nrOfWheels, color, cylinderVolume);
                    break;
                case 5:
                    wingSpan = Util.AskForPositiveDouble("Enter the wing span of the airplane", ui);
                    vehicle = new Airplane(regNr, nrOfWheels, color, wingSpan);
                    break;
                default:
                    ui.Print("Wrong choice, try again");
                    break;
            }

            // The veicle is added inside the if
            if (garageHandler.Add(vehicle))
                ui.Print($"The {vehicle.GetType().Name} was parked successfully");
            else
                ui.Print($"The {vehicle.GetType().Name} could not be parked. The garage is full");

            ui.Print("");

        }

        public void SeedData()
        {
            Car car = new Car("abc123", 4, "Red", "Diesel");
            garageHandler.Add(car);
            Car car2 = new Car("sdf334", 4, "Red", "Gasoline");
            garageHandler.Add(car2);
            Vehicle bus = new Bus("cbe321", 4, "Red", 12);
            garageHandler.Add(bus);
            Boat boat = new Boat("AB12", 0, "Brown", 7.5);
            garageHandler.Add(boat);
            Motorcycle motorcycle = new Motorcycle("123MBO", 2, "Red", 1.6);
            garageHandler.Add(motorcycle);
            Airplane airplane = new Airplane("DC34b", 3, "White", 33);
            garageHandler.Add(airplane);

            ui.Print("The garage was populated with some vehicles");
            ui.Print("");

        }

    }
}
