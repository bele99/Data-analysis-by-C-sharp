using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaVersion02
{
    public class MenuOption
    {
        private int Option = 0;

        public int CheckOption()
        {
            do
            {
                try
                {
                    Option = Convert.ToInt32(Console.ReadLine());
                    if (Option > 5 || Option < 1)
                    {
                        Console.WriteLine("Please select a menu number between 1 and 5.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("You have encountered a technical problem, please contact the administrator.");
                    Option = -1;
                }
            } while (Option < 1 || Option > 5);
            return Option;
        }
    }

    public class ReadUserOption : MenuOption
    {
        public void MenuPrint()
        {
            Console.WriteLine("****************************************");
            Console.WriteLine("*            ** Data Menu **           *");
            Console.WriteLine("* 1. LoadData                          *");
            Console.WriteLine("* 2. ProcessData                       *");
            Console.WriteLine("* 3. DisplayDataOnChart                *");
            Console.WriteLine("* 4. TrainModel                        *");
            Console.WriteLine("* 5. Quit                              *");
            Console.WriteLine("****************************************");
            Console.Write("Choose an option [1-5]: ");
        }
    }

    public class ReadProcessOption : MenuOption
    {
        public void MenuPrint()
        {
            Console.WriteLine("****************************************");
            Console.WriteLine("*        ** ProcessData Menu **        *");
            Console.WriteLine("* 1. SetLabel                          *");
            Console.WriteLine("* 2. FillNulls                         *");
            Console.WriteLine("* 3. OrderData                         *");
            Console.WriteLine("* 4. FilterData                        *");
            Console.WriteLine("* 5. ReturnToUpperMenu                 *");
            Console.WriteLine("****************************************");
            Console.Write("Choose an option [1-5]: ");
        }
    }

    public class ReadDataOption : MenuOption
    {
        public void MenuPrint()
        {
            Console.WriteLine("****************************************");
            Console.WriteLine("*         ** Data Info Menu **          *");
            Console.WriteLine("* 1. Data Info                          *");
            Console.WriteLine("* 2. Data description                   *");
            Console.WriteLine("* 3. Data display (head 5)              *");
            Console.WriteLine("* 4. Count Row & Column                 *");
            Console.WriteLine("* 5. ReturnToUpperMenu                  *");
            Console.WriteLine("****************************************");
            Console.Write("Choose an option [1-5]: ");
        }
    }
}