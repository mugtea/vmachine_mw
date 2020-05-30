using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine_MW
{
    public class VendingMachine
    {
        private int depositedMoney;
        private Item selectedItem;
        private List<Item> listItem;
        private List<int> listFractionOfMoney;

        public VendingMachine()
        {
            initializeDummyData();
        }

        private void initializeDummyData()
        {
            listFractionOfMoney = new List<int>() { 2000, 5000, 10000, 20000, 50000 };
            listItem = new List<Item>();
            listItem.Add(new Item()
            {
                id = 1,
                name = "Biskuit",
                price = 6000,
                stock = 2
            });
            listItem.Add(new Item()
            {
                id = 2,
                name = "Chips",
                price = 8000,
                stock = 2
            });
            listItem.Add(new Item()
            {
                id = 3,
                name = "Oreo",
                price = 10000,
                stock = 2
            });
            listItem.Add(new Item()
            {
                id = 4,
                name = "Tango",
                price = 12000,
                stock = 2
            });
            listItem.Add(new Item()
            {
                id = 5,
                name = "Cokelat",
                price = 15000,
                stock = 2
            });
        }
        private void setSelectedItem(Item selectedItem)
        {
            this.selectedItem = selectedItem;
        }
        private void decreaseStock()
        {
            var tempItem = listItem.Where(x => x.id == selectedItem.id).FirstOrDefault();
            if (tempItem != null) tempItem.stock -= 1;
        }
        private bool checkIsStockExist()
        {
            return selectedItem.stock > 0;
        }
        private bool checkIsValidWithExistingMultiple(int change, int item, List<int> temp)
        {
            foreach (var x in temp.Where(x => x <= item).OrderByDescending(x => x))
            {
                if (change % x == 0)
                    return true;
            }
            return false;
        }
        private bool checkIsDepositedMoneyEnough()
        {
            return depositedMoney >= selectedItem.price;
        }
        private string getChange(int change)
        {
            List<int> result = new List<int>();
            bool isLoop = true;
            if (listFractionOfMoney.IndexOf(change) > -1)
                result.Add(change);
            else
            {
                var currentIndex = 0;
                var isValidOfFractionMoney = false;
                while (isLoop)
                {
                    var listFractionOfMoneyFiltered = listFractionOfMoney.Where(x => x <= change).OrderByDescending(x => x).ToList();
                    var item = listFractionOfMoneyFiltered[currentIndex];
                    if (item <= change)
                    {
                        var tempSisaSetelahnya = change - item;
                        isValidOfFractionMoney = checkIsValidWithExistingMultiple(tempSisaSetelahnya, item, listFractionOfMoneyFiltered);

                        if (isValidOfFractionMoney)
                        {
                            currentIndex = 0;
                            result.Add(item);
                            change -= item;
                            if (change == 0)
                                isLoop = false;
                        }
                        else
                            currentIndex++;
                    }
                }
            }

            var tempResult = result.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            List<string> listResult = new List<string>();
            foreach (var item in tempResult)
                listResult.Add($"{item.Key} [{item.Value} Lembar]");

            return $"Uang kembalian dalam pecahan {string.Join(",", listResult)}";
        }
        public int getDepositedMoney()
        {
            return depositedMoney;
        }
        public void addDepositedMoney(int depositedMoney)
        {
            this.depositedMoney += depositedMoney;
        }
        public bool checkIsValidMoney(int money)
        {
            List<int> listAllowedCurreny = new List<int>() { 2000, 5000, 10000, 20000, 50000 };
            return listAllowedCurreny.IndexOf(money) > -1;
        }
        public bool checkAndSetIsItemExist(int id)
        {
            var resultItem = listItem.Where(x => x.id == id).FirstOrDefault();
            if (resultItem == null)
            {
                return false;
            }
            else
            {
                setSelectedItem(resultItem);
            }
            return true;
        }
        public string buyItem()
        {
            string result = string.Empty;
            if (checkIsDepositedMoneyEnough())
            {
                if (checkIsStockExist())
                {
                    int change = depositedMoney - selectedItem.price;
                    result = $"Pembelian {selectedItem.name} seharga {selectedItem.price} berhasil {Environment.NewLine}";
                    result += getChange(change);
                    decreaseStock();
                    depositedMoney = 0;
                }
                else
                {
                    result = "Error : Maaf stok item ini sedang kosong";
                }
            }
            else
                result = "Error : Uang deposit anda tidak cukup untuk membeli item ini";
            return result;
        }
        public void showMenu()
        {
            Console.WriteLine("============================================ ");
            Console.WriteLine($"Uang Deposit ==> {depositedMoney}");
            Console.WriteLine("================ Daftar Menu =============== ");
            Console.WriteLine($"ID\tNama Barang\tHarga\t\tStok");
            Console.WriteLine("============================================ ");
            foreach (var item in listItem)
                Console.WriteLine($"{item.id}\t{item.name}\t\t{item.price}\t\t{item.stock}");
            Console.WriteLine("============================================ ");
        }
    }
}
