using System;

namespace VendingMachine_MW
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine vendingMachine = new VendingMachine();
        start:
            Console.WriteLine($"============================================ ");
            Console.WriteLine($"<<=========== Vending Machine ============>> ");
            Console.WriteLine($"============================================ ");
            if (vendingMachine.getDepositedMoney() == 0)
                Console.Write($"Masukkan uang deposit.. ");
            else
                Console.Write($"Masukkan uang deposit tambahan.. ");
            var input = Console.ReadLine();

            try
            {
                var convertedInput = int.Parse(input);
                if (!vendingMachine.checkIsValidMoney(convertedInput))
                {
                    Console.WriteLine("Silahkan masukkan uang pecahan 2000/5000/10000/20000/50000");
                    goto start;
                }

                vendingMachine.addDepositedMoney(convertedInput);
                Console.Write("Apakah anda ingin memasukan deposit lagi  [y][t] ? ");
                var answer = Console.ReadLine();

                if (answer.ToLower() == "y")
                    goto start;
                else
                {
                menu:
                    vendingMachine.showMenu();
                enterItemToBuy:
                    Console.Write("Silahkan masukan ID item yang akan anda beli ? ");
                    Console.WriteLine($"{Environment.NewLine}============================================ ");
                    var currentItemToBuy = Console.ReadLine();
                    try
                    {
                        int idCurrentItemToBuy = int.Parse(currentItemToBuy);
                        if (vendingMachine.checkAndSetIsItemExist(idCurrentItemToBuy))
                        {
                            var processResult = vendingMachine.buyItem();

                            Console.WriteLine($"============================================ ");
                            Console.WriteLine(processResult);
                            Console.WriteLine($"============================================ ");
                            if (processResult.IndexOf("Error") > -1)
                                goto enterItemToBuy;
                            else
                            {
                                Console.Write("Tekan enter untuk memulai kembali");
                                Console.ReadKey();
                                Console.Clear();
                                goto start;
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID yang anda masukkan tidak ditemukan!!!");
                            goto enterItemToBuy;
                        }
                    }
                    catch (Exception)
                    {
                        goto menu;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Silahkan masukkan uang pecahan 2000/5000/10000/20000/50000");
                goto start;
            }
        }
    }
}
