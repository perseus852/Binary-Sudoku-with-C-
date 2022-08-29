using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binary_Sudoku
{
    class Program
    {
        static int userScore = 0;
        static int cursorX = 0, cursorY = 0;
        static Random rnd = new Random();

        static char[,] table =
        {
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' },
            { '■', '■', '■', '■', '■', '■', '■', '■', '■' }
        };

        static string[] elements =
        {
            "X",
            "XX",
            "XXX",
            "XX/XX",
            "X/X",
            "X/X/X",
            "XX/X■",
            "XX/■X",
            "X■/XX",
            "■X/XX",
        };

        static string element = GenerateElement();
        static bool isElementInserted = false;

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            while (true)
            {
                string rowIndexes = RowCalculator();

                DeleteRows(rowIndexes);

                Console.Clear();

                if (isElementInserted)
                {
                    element = GenerateElement();
                    isElementInserted = false;
                }
                Console.WriteLine("Binary Sudoku");
                WriteTable();
                Console.WriteLine("");
                Console.WriteLine("Element : ");
                WriteElementReadable(element);
                Console.WriteLine("");
                Console.WriteLine("User Score : " + userScore);

                ConsoleKey ch = Console.ReadKey().Key;

                switch (ch)
                {
                    case ConsoleKey.UpArrow:

                        cursorY--;

                        if (cursorY < 0)
                            cursorY = 0;

                        break;
                    case ConsoleKey.LeftArrow:

                        cursorX--;

                        if (cursorX < 0)
                            cursorX = 0;

                        break;
                    case ConsoleKey.DownArrow:

                        cursorY++;

                        int maxY = table.GetLength(0) - GetElementMaxY(element);

                        if (cursorY > maxY)
                            cursorY = maxY;

                        break;
                    case ConsoleKey.RightArrow:

                        cursorX++;

                        int maxX = table.GetLength(1) - GetElementMaxX(element);

                        if (cursorX > maxX)
                            cursorX = maxX;

                        break;
                    case ConsoleKey.Enter:

                        isElementInserted = InsertElement(element, cursorX, cursorY);

                        break;
                }
            }
        }

        static string GenerateElement()
        {
            string newElement = "", selectedElement = elements[rnd.Next(0, elements.Length)];

            for (int i = 0; i < selectedElement.Length; i++)
            {
                if (selectedElement[i] == 'X')
                    newElement += rnd.Next(0, 2).ToString();
                else
                    newElement += selectedElement[i];
            }

            return newElement;
        }

        //kullanıcıya üretilen elementi consol'da okunaklı hale getirir.
        static void WriteElementReadable(string element)
        {
            for (int i = 0; i < element.Length; i++)
            {
                if (element[i] == '/')
                    Console.WriteLine("");
                else if (element[i] == '■')
                    Console.Write(" ");
                else
                    Console.Write(element[i]);
            }

            Console.WriteLine("");
        }

        //eger uygun şartlar karşılanırsa elementin tabloya eklenmesi
        static bool InsertElement(string element, int x, int y)
        {
            if (!CheckArea(element, x, y))
            {
                return false;
            }

            if (!CheckTable(element, x, y))
            {
                return false;
            }

            int _x = x, _y = y;

            for (int i = 0; i < element.Length; i++)
            {
                if (element[i] == '/')
                {
                    _y++;
                    _x = x;
                    continue;
                }

                if (element[i] != '■')
                    table[_y, _x] = element[i];

                _x++;
            }

            return true;
        }

        static bool CheckArea(string element, int x, int y)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;

            int max_X = GetElementMaxX(element);
            int max_Y = GetElementMaxY(element);

            if (max_X + x > 9 || max_Y + y > 9)
                return false;

            return true;
        }


        static int GetElementMaxX(string element)
        {
            int max_X = 0; // anlık satırda bulunan uzunluk bilgisi
            int max_XX = 0; // bütün satırlar arasındaki en uzun olarak tutulacak deger

            for (int i = 0; i < element.Length; i++)
            {
                if (element[i] == '/')
                {
                    if (max_XX < max_X)
                        max_XX = max_X;

                    max_X = 0;
                }
                else
                    max_X++;
            }

            if (max_XX < max_X)
                max_XX = max_X;

            return max_XX;
        }

        static int GetElementMaxY(string element)
        {
            int max_Y = 1;

            for (int i = 0; i < element.Length; i++)
            {
                if (element[i] == '/')
                {
                    max_Y++;
                }
            }

            return max_Y;
        }

        // tablo üzerindeki 0 veya 1'lerin çakışmasını kontrol etmek için
        static bool CheckTable(string element, int x, int y)
        {
            int _x = x, _y = y; // alt satıra geçtiginde daktilo mantıgıyla satır bilgisini ilk baştaki degere geri çekmek için tanımlandı.

            for (int i = 0; i < element.Length; i++)
            {
                if (element[i] == '/')
                {
                    _y++;
                    _x = x;
                    continue;
                }

                if (element[i] == '■')
                {
                    _x++;
                    continue;
                }

                if (element[i] == '0' || element[i] == '1')
                {
                    if (table[_y, _x] != '■')
                        return false;
                }

                _x++;
            }

            return true;
        }

        //tabloyu ekrana yazdırma fonksiyonu
        static void WriteTable()
        {
            ConsoleColor lastColor = ConsoleColor.Red;

            int counter = 0;
            string[] coordinates = GetElementCoordinates(element);

            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0)
                {
                    if (lastColor == ConsoleColor.Red)
                        lastColor = ConsoleColor.White;
                    else
                        lastColor = ConsoleColor.Red;
                }

                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j > 0)
                    {
                        if (lastColor == ConsoleColor.Red)
                            lastColor = ConsoleColor.White;
                        else
                            lastColor = ConsoleColor.Red;

                        //koordinatlar dizisinin dışına taşmamak için bir counter degişkeni oluşturuldu burada i veya j yi kullanamama sebebimiz elementin koordinatları içeren dizinin
                        //tablo ile alakası olmamasıdır. Counter degişkenini ilk kontrol etme sebebimiz diger koşulları kontrol etmeden if' i kırmasıdır.
                        //Eger en son yapsaydık bu kontrolü index out of range hatası verecekti.
                    }
                    if (counter < coordinates.Length && int.Parse(coordinates[counter][0].ToString()) == j && int.Parse(coordinates[counter][2].ToString()) == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(table[i, j] + " ");
                        counter++;
                    }
                    else
                    {
                        Console.ForegroundColor = lastColor;
                        Console.Write(table[i, j] + " ");
                    }
                }

                Console.WriteLine("");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        static string[] GetElementCoordinates(string element)
        {
            string tempCoordinate = "", coordinate = "";
            int x = cursorX, y = cursorY;

            for (int i = 0; i < element.Length; i++)
            {
                if (element[i] == '/')
                {
                    y++;
                    x = cursorX;
                    continue;
                }

                if (element[i] == '■')
                {
                    x++;
                    continue;
                }

                if (element[i] == '1' || element[i] == '0')
                {
                    tempCoordinate += x + "," + y + "|";
                    x++;
                }
            }

            // son uzun çizgi karakterini eliyoruz ki hataya sebep olmasın
            for (int i = 0; i < tempCoordinate.Length - 1; i++)
                coordinate += tempCoordinate[i];

            return coordinate.Split('|');
        }

        static string RowCalculator()
        {
            string tempRowIndexes = "", rowIndexes = "";
            int mainSum = 0;

            for (int i = 0; i < table.GetLength(0); i++)
            {
                int rowSum = 0;

                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] == '■')
                    {
                        rowSum = 0;
                        break;
                    }

                    int radix = 2;
                    int power = table.GetLength(0) - j - 1;
                    int powResult = 1, result = 0;

                    for (int p = 0; p < power; p++)
                    {
                        powResult *= radix;
                    }

                    result = powResult * int.Parse(table[i, j].ToString());
                    rowSum += result;
                }

                if (rowSum != 0)
                {
                    tempRowIndexes += i + ",";
                    mainSum += rowSum;
                }
            }

            for (int i = 0; i < tempRowIndexes.Length - 1; i++)
                rowIndexes += tempRowIndexes[i];

            userScore += mainSum;

            return rowIndexes;
        }

        static void DeleteRows(string rowIndexes)
        {
            if (rowIndexes.Length == 0)
                return;

            string[] rowIndexesArray = rowIndexes.Split(',');

            for (int i = 0; i < rowIndexesArray.Length; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    table[int.Parse(rowIndexesArray[i]), j] = '■';
                }
            }
        }


    }
}
