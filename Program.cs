namespace Session07_Assignment03_OOP_Part01
{
        // =========================================================================
        //  Q9. (Pre-interface problem) Create a class Car with methods
        //      MoveForward() and MoveBackward() that just print messages. Now imagine
        //      you also need MoveUp()/MoveDown(). What is the problem with forcing
        //      every vehicle class to implement all four methods?
        //      (This is what interfaces solve.)
        // =========================================================================
        class PreInterfaceCar
        {
            public void MoveForward() { Console.WriteLine("Car moves forward."); }
            public void MoveBackward() { Console.WriteLine("Car moves backward."); }
        }

        // The "old way": ONE base class that forces ALL vehicles to have all 4 methods.
        abstract class OldVehicle
        {
            public abstract void MoveForward();
            public abstract void MoveBackward();
            public abstract void MoveUp();
            public abstract void MoveDown();
        }

        class OldCar : OldVehicle
        {
            public override void MoveForward() { Console.WriteLine("OldCar moves forward."); }
            public override void MoveBackward() { Console.WriteLine("OldCar moves backward."); }

            // A car cannot fly, but it is FORCED to implement these two methods.
            // Faking them with an exception is the only honest option, and the
            // mistake is only discovered at RUNTIME.
            public override void MoveUp() { throw new NotSupportedException("A car cannot move up."); }
            public override void MoveDown() { throw new NotSupportedException("A car cannot move down."); }
        }

        // =========================================================================
        //  Q10. Create two interfaces:
        //       IMoveable -> MoveForward(), MoveBackward()
        //       IFlyable  -> MoveUp(), MoveDown()
        // =========================================================================
        interface IMoveable
        {
            void MoveForward();
            void MoveBackward();
        }

        interface IFlyable
        {
            void MoveUp();
            void MoveDown();
        }

        // =========================================================================
        //  Q11. Implement the interfaces in three classes:
        //       Car : IMoveable                — messages about moving on the ground.
        //       Ship : IMoveable               — messages about moving on the sea.
        //       Airplane : IMoveable, IFlyable — messages about moving in the air.
        // =========================================================================
        class Car : IMoveable
        {
            public void MoveForward() { Console.WriteLine("Car drives forward on the road."); }
            public void MoveBackward() { Console.WriteLine("Car reverses on the road."); }
        }

        class Ship : IMoveable
        {
            public void MoveForward() { Console.WriteLine("Ship sails forward on the sea."); }
            public void MoveBackward() { Console.WriteLine("Ship sails backward on the sea."); }
        }

        class Airplane : IMoveable, IFlyable
        {
            public void MoveForward() { Console.WriteLine("Airplane flies forward in the air."); }
            public void MoveBackward() { Console.WriteLine("Airplane moves backward in the air."); }
            public void MoveUp() { Console.WriteLine("Airplane climbs up."); }
            public void MoveDown() { Console.WriteLine("Airplane descends."); }
        }

        // =========================================================================
        //  Q13. Create an interface IVehicle that inherits from both IMoveable and
        //       IFlyable without adding new members. Then create a class
        //       Vehicle : IVehicle that implements all four methods as virtual.
        //       What is the benefit of an interface inheriting other interfaces?
        // =========================================================================
        interface IVehicle : IMoveable, IFlyable
        {
            // no new members
        }

        class Vehicle : IVehicle
        {
            public virtual void MoveForward() { Console.WriteLine("Vehicle moves forward."); }
            public virtual void MoveBackward() { Console.WriteLine("Vehicle moves backward."); }
            public virtual void MoveUp() { Console.WriteLine("Vehicle moves up."); }
            public virtual void MoveDown() { Console.WriteLine("Vehicle moves down."); }
        }

        // A derived class showing why 'virtual' matters here.
        class FlyingCar : Vehicle
        {
            public override void MoveUp() { Console.WriteLine("FlyingCar lifts off the road!"); }
        }

        // =========================================================================
        //  Q14. (Challenge) Can a class implement IMoveable explicitly? Rewrite
        //       Ship.MoveForward() using explicit interface implementation
        //       (void IMoveable.MoveForward()), then try calling it on a Ship object
        //       directly. What happens, and how must you call it instead?
        // =========================================================================
        class ShipExplicit : IMoveable
        {
            // EXPLICIT implementation: no access modifier, and the name is prefixed
            // with the interface name.
            void IMoveable.MoveForward()
            {
                Console.WriteLine("ShipExplicit sails forward (explicit implementation).");
            }

            // MoveBackward stays a normal (implicit) implementation.
            public void MoveBackward()
            {
                Console.WriteLine("ShipExplicit sails backward (implicit implementation).");
            }
        }

        internal class Program
        {
            // =====================================================================
            //  MAIN: menu
            // =====================================================================
            static void Main()
            {
                while (true)
                {
                    Console.WriteLine("\n===== OOP Assignment 3 - Part 1 (Interfaces) =====");
                    Console.WriteLine("1 - Q9 : the problem before interfaces");
                    Console.WriteLine("2 - Q12: concrete types and interface references (Q10-Q12)");
                    Console.WriteLine("3 - Q13: IVehicle and Vehicle");
                    Console.WriteLine("4 - Q14: explicit interface implementation");
                    Console.WriteLine("0 - Exit");
                    Console.Write("Your choice: ");

                    string input = Console.ReadLine() ?? "";
                    if (input == "0") break;
                    Console.WriteLine();

                    switch (input)
                    {
                        case "1": Q9(); break;
                        case "2": Q12(); break;
                        case "3": Q13(); break;
                        case "4": Q14(); break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
            }

            // Q9 demo
            static void Q9()
            {
                PreInterfaceCar simpleCar = new PreInterfaceCar();
                simpleCar.MoveForward();
                simpleCar.MoveBackward();

                Console.WriteLine("\nNow every vehicle must have 4 methods (OldVehicle):");
                OldVehicle oldCar = new OldCar();
                oldCar.MoveForward();

                try
                {
                    oldCar.MoveUp(); // compiles fine, but makes no sense for a car
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine("Runtime error: " + ex.Message);
                }

                // THE PROBLEM:
                // 1. A Car/Ship is forced to implement MoveUp/MoveDown even though it can't fly.
                // 2. The only options are empty methods or exceptions -> fake code.
                // 3. The compiler cannot stop someone from calling MoveUp() on a car; the
                //    mistake is discovered only at runtime.
                // 4. Every new ability (e.g. Dive) would force a change in ALL classes.
                // Interfaces fix this: each class implements ONLY the abilities it really has.
            }

            // Q12 demo
            // - Create each class with its concrete type and call its methods.
            // - Then declare IMoveable carRef = new Car(); and IMoveable planeRef = new Airplane();
            //   and call MoveForward()/MoveBackward() through the interface references.
            // - Question: Can you call MoveUp() on planeRef? Why or why not? What reference
            //   type would you need?
            static void Q12()
            {
                Console.WriteLine("--- Concrete types ---");
                Car car = new Car();
                car.MoveForward();
                car.MoveBackward();

                Ship ship = new Ship();
                ship.MoveForward();
                ship.MoveBackward();

                Airplane plane = new Airplane();
                plane.MoveForward();
                plane.MoveBackward();
                plane.MoveUp();
                plane.MoveDown();

                Console.WriteLine("\n--- Interface references ---");
                IMoveable carRef = new Car();
                IMoveable planeRef = new Airplane();

                carRef.MoveForward();
                carRef.MoveBackward();
                planeRef.MoveForward();
                planeRef.MoveBackward();

                // planeRef.MoveUp();  // COMPILE ERROR CS1061:
                //   'IMoveable' does not contain a definition for 'MoveUp'.
                // WHY: the compiler only allows the members of the REFERENCE type (IMoveable),
                // even though the real object (Airplane) has MoveUp.
                // FIX: use a reference whose type has MoveUp:
                Console.WriteLine("\n--- Reaching MoveUp through IFlyable ---");
                if (planeRef is IFlyable flyable)   // safe check + cast
                {
                    flyable.MoveUp();
                    flyable.MoveDown();
                }

                IFlyable flyRef = (IFlyable)planeRef;  // explicit cast (throws InvalidCastException if the object isn't IFlyable)
                flyRef.MoveUp();

                // The same check on a Car correctly fails:
                Console.WriteLine($"\nIs carRef an IFlyable? {carRef is IFlyable}");
            }

            // Q13 demo
            static void Q13()
            {
                IVehicle vehicle = new Vehicle();
                vehicle.MoveForward();
                vehicle.MoveBackward();
                vehicle.MoveUp();      // all four are available through ONE reference
                vehicle.MoveDown();

                Console.WriteLine("\nVirtual methods allow overriding:");
                IVehicle flyingCar = new FlyingCar();
                flyingCar.MoveForward(); // inherited from Vehicle
                flyingCar.MoveUp();      // overridden in FlyingCar

                // The same object can also be used where only a smaller contract is needed:
                IMoveable asMoveable = flyingCar;
                asMoveable.MoveForward();

                // BENEFIT of an interface inheriting other interfaces:
                // 1. It combines several small contracts into ONE bigger contract
                //    (IVehicle = IMoveable + IFlyable) without repeating the members.
                // 2. A class implements one interface (IVehicle) and gets all the rules.
                // 3. Code can accept "something that can move AND fly" with one parameter type.
                // 4. An IVehicle can still be passed anywhere an IMoveable or IFlyable is expected.
            }

            // Q14 demo
            static void Q14()
            {
                ShipExplicit ship = new ShipExplicit();

                // ship.MoveForward();  // COMPILE ERROR CS1061:
                //   'ShipExplicit' does not contain a definition for 'MoveForward'.
                // An explicit implementation is PRIVATE to the class: it is visible only
                // through the interface.

                ship.MoveBackward(); // works: implicit implementation is public

                // How to call it: through an IMoveable reference
                IMoveable moveable = ship;
                moveable.MoveForward();

                // or with a cast in one line
                ((IMoveable)ship).MoveForward();

                // WHY use explicit implementation?
                // - To hide interface members from the class's public API.
                // - To resolve a name conflict when two interfaces have a member with the same name.
            }
        }
    }
