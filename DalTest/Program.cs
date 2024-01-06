using DalApi;
using DO;
using DalTest;
using Dal;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static IDependeency? s_dalDependeency = new DependeencyImplementation(); //stage 1

        private static int mainMenue() //Main Menu
        {
            Console.WriteLine(@"please enter a number
                                    0 to exit
                                    1 to task
                                    2 to engineer
                                    3 to dependencies");

            string? change = Console.ReadLine();
            return int.Parse(change!);   
        }

        private static void subMenue() //Sub menu
        {
            Console.WriteLine(@"please enter a number
                                    0 to exit
                                    1 to create
                                    2 to read
                                    3 to read all
                                    4 to update
                                    5 to delete");
        }
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependeency);
                int x = mainMenue();
                

                while (x!=0)
                {

                    if (x == 1) //task
                    {
                        subMenue(); 
                        string? y = Console.ReadLine();
                        int act = int.Parse(y!);

                        switch (act)
                        {
                            case 0:
                            {
                                    x = 0;
                                    break;
                            }
                            case 1:
                            {
                                    break;
                            }

                            case 2:
                            {
                                    break;
                            }

                            case 3:
                            {
                                    break;
                            }

                            case 4:
                            {
                                    break;
                            }

                            case 5:
                            {
                                    break;
                            }

                            default:
                            {
                                    Console.WriteLine("Eror");
                                    break;
                            }
                        }

                    }

                    if (x == 2) //engineer
                    {
                        subMenue();
                        string? y = Console.ReadLine();
                        int act = int.Parse(y!);

                        switch (act)
                        {
                            case 0:
                                {
                                    x = 0;
                                    break;
                                }
                            case 1:
                                {
                                    break;
                                }

                            case 2:
                                {
                                    break;
                                }

                            case 3:
                                {
                                    break;
                                }

                            case 4:
                                {
                                    break;
                                }

                            case 5:
                                {
                                    break;
                                }

                            default:
                            {
                                    Console.WriteLine("Eror");
                                    break;
                            }
                        }

                    }

                    if (x == 3) //dependeency
                    {
                        subMenue();
                        string? y = Console.ReadLine();
                        int act = int.Parse(y!);

                        switch (act)
                        {
                            case 0:
                                {
                                    x = 0;
                                    break;
                                }
                            case 1:
                                {
                                    break;
                                }

                            case 2:
                                {
                                    break;
                                }

                            case 3:
                                {
                                    break;
                                }

                            case 4:
                                {
                                    break;
                                }

                            case 5:
                                {
                                    break;
                                }

                            default:
                                {
                                    Console.WriteLine("Eror");
                                    break;
                                }
                        }

                    }

                    if(x!=0)
                    {
                        x = mainMenue();
                    }

                }
            }

            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
