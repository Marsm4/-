using System;
using System.Data.Common;

class Programs
{
    static char[,] playerGrid = new char[10, 10];
    static char[,] computerGrid = new char[10, 10];

    static void Main(string[] args)
    {
        InitializeGrids();

        // Расставляем корабли компьютера
        RandomlyPlaceShips(computerGrid);

        // Расставляем корабли игрока
        Console.WriteLine("Расставьте свои корабли.");
        ManuallyPlaceShips(playerGrid);

        // Начинаем игру
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== Игра Морской бой ====");
            Console.WriteLine("Компьютер:");
            DrawGrid(computerGrid, true);
            Console.WriteLine("Вы:");
            DrawGrid(playerGrid, false);


            // Ход игрока

            Console.WriteLine("Ваш ход. Введите координаты (например, A1):");
            string target = Console.ReadLine();
            bool playerHit = ProcessTurn(target, computerGrid);

            if (CheckGameOver(computerGrid))
            {
                Console.WriteLine("Вы победили!");
                break;
            }

            // Ход компьютера
            Console.WriteLine("Ход компьютера...");
            string computerTarget = GenerateRandomTarget();
            bool computerHit = ProcessTurn(computerTarget, playerGrid);

            if (CheckGameOver(playerGrid))
            {
                Console.WriteLine("Компьютер победил.");
                break;
            }
        }

        Console.ReadLine();
    }



    static void InitializeGrids()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                playerGrid[i, j] = '~';
                computerGrid[i, j] = '~';
            }
        }
    }

    static void DrawGrid(char[,] grid, bool hideShips)
    {
        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((char)(65 + i) + " ");
            for (int j = 0; j < 10; j++)
            {
                if (hideShips && grid[i, j] == 'O')
                {
                    Console.Write("~ ");
                }
                else
                {
                    Console.Write(grid[i, j] + " ");
                }
            }
            Console.WriteLine();
        }
    }

    static void ManuallyPlaceShips(char[,] grid)
    {
        PlaceShip(grid, 4);  // четырехпалубный
        PlaceShip(grid, 3);  // трехпалубные
        PlaceShip(grid, 3);
        PlaceShip(grid, 2);  // двухпалубные
        PlaceShip(grid, 2);
        PlaceShip(grid, 2);
        PlaceShip(grid, 1);  // однопалубные
        PlaceShip(grid, 1);
        PlaceShip(grid, 1);
        PlaceShip(grid, 1);
    }

    //static void Maina(string[] args)
    //{
    //    InitializeGrids();

    //    // Расположение кораблей
    //    Console.WriteLine("Выберите способ расстановки кораблей:");
    //    Console.WriteLine("1. Рандомная расстановка");
    //    Console.WriteLine("2. Ручная расстановка");
    //    Console.Write("Введите номер способа: ");
    //    string choice = Console.ReadLine();

    //    if (choice == "1")
    //    {
    //        RandomlyPlaceShips(playerGrid);
    //    }
    //    else if (choice == "2")
    //    {
    //        ManuallyPlaceShips(playerGrid);
    //    }
    //    else
    //    {
    //        Console.WriteLine("Некорректный выбор. Используется рандомная расстановка.");
    //        RandomlyPlaceShips(playerGrid);
    //    }

    //    // Начало игры...
    //}


    // Начало игры...
    static void PlaceShip(char[,] grid, int size)
    {
        Console.WriteLine("ВВедите координаты. P.S Поле состоит из английских букв A, B, C, D, E, F, G, H, I, J и цифр от нуля 0 до 9");
        Console.WriteLine($"Расставьте корабль размером {size} клеток (например, A1 A2 A3):");
  
        string input = Console.ReadLine();
        string[] coordinates = input.Split(' ');

        bool isValid = true;

        foreach (string coordinate in coordinates)
        {
            if (!IsValidCoordinate(coordinate) || IsOccupied(grid, coordinate) || HasNeighboringShip(grid, coordinate))
            {
                isValid = false;
                break;
            }
        }

        if (isValid)
        {
            foreach (string coordinate in coordinates)
            {
                int row = coordinate[0] - 65;
                int column = int.Parse(coordinate.Substring(1));
                grid[row, column] = 'O';
            }
        }
        else
        {
            Console.WriteLine("Неверные координаты");
            PlaceShip(grid, size);
        }
    }

    static bool IsValidCoordinate(string coordinate)
    {
        if (coordinate.Length < 2 || coordinate.Length > 3)
            return false;

        char row = coordinate[0];
        string column = coordinate.Substring(1);

        if (row < 'A' || row > 'J')
            return false;

        int columnNumber;

        if (!int.TryParse(column, out columnNumber))
            return false;

        if (columnNumber < 0 || columnNumber > 9)
            return false;

        return true;
    }

    static bool IsOccupied(char[,] grid, string coordinate)
    {
        int row = coordinate[0] - 65;
        int column = int.Parse(coordinate.Substring(1));

        return grid[row, column] == 'O';
    }

    static void RandomlyPlaceShips(char[,] grid)
    {
        int fourDeckCount = 1;
        int threeDeckCount = 2;
        int twoDeckCount = 3;
        int oneDeckCount = 4;

        Random random = new Random();

        while (fourDeckCount > 0 || threeDeckCount > 0 || twoDeckCount > 0 || oneDeckCount > 0)
        {
            //Генерирация случайных координат для корабля
            int x = random.Next(0, 10);
            int y = random.Next(0, 10);

            // Случайным образом определяю ориентацию корабля (горизонтальную или вертикальную)
            bool isHorizontal = random.Next(2) == 0;

            // Рассчитайте длину судна в зависимости от его типа
            int length = 0;

            if (fourDeckCount > 0)
            {
                length = 4;
                fourDeckCount--;
            }
            else if (threeDeckCount > 0)
            {
                length = 3;
                threeDeckCount--;
            }
            else if (twoDeckCount > 0)
            {
                length = 2;
                twoDeckCount--;
            }
            else if (oneDeckCount > 0)
            {
                length = 1;
                oneDeckCount--;
            }

            // Проверка, можно ли разместить корабль в произвольных координатах и ориентации
            if (CanPlaceShip(grid, x, y, length, isHorizontal))
            {
                // Поместщение корабля на сетку
                PlaceShip(grid, x, y, length, isHorizontal);
            }
        }
    }
    static bool CanPlaceShip(char[,] grid, int x, int y, int length, bool isHorizontal)
    {
        // Проверка, не выходит ли корабль за пределы зоны
        if (isHorizontal && (x + length) > 10)
        {
            return false;
        }
        else if (!isHorizontal && (y + length) > 10)
        {
            return false;
        }

        // Проверка, есть ли уже судно по указанным координатам
        for (int i = 0; i < length; i++)
        {
            if (isHorizontal && grid[x + i, y] == 'O')
            {
                return false;
            }
            else if (!isHorizontal && grid[x, y + i] == 'O')
            {
                return false;
            }
        }

        return true;
    }

    static void PlaceShip(char[,] grid, int x, int y, int length, bool isHorizontal)
    {
        // Помещаю корабль на сетку
        for (int i = 0; i < length; i++)
        {
            if (isHorizontal)
            {
                grid[x + i, y] = 'O';
            }
            else
            {
                grid[x, y + i] = 'O';
            }
        }
    }


    static string GenerateRandomTarget()
    {
        Random rand = new Random();
        int row = rand.Next(0, 10);
        int column = rand.Next(0, 10);

        return ((char)(65 + row)).ToString() + column.ToString();
    }

    static bool ProcessTurn(string target, char[,] grid)
    {
        int row = target[0] - 65;
        int column = int.Parse(target.Substring(1));

        if (grid[row, column] == 'O')
        {
            grid[row, column] = 'X';
            Console.WriteLine("Попадание!");

            // Проверка наличия соседних палуб и продолжает поражать их, пока не промахнетесь
            while (true)
            {
                bool hasAdjacentTarget = false;
                if (column > 0 && grid[row, column - 1] == 'O') // Left
                {
                    grid[row, column - 1] = 'X';
                    Console.WriteLine("Компьютер добил попадание в соседнюю клетку слева!");
                    hasAdjacentTarget = true;
                }
                if (column < grid.GetLength(1) - 1 && grid[row, column + 1] == 'O') // Right
                {
                    grid[row, column + 1] = 'X';
                    Console.WriteLine("Компьютер добил попадание в соседнюю клетку справа!");
                    hasAdjacentTarget = true;
                }
                if (row > 0 && grid[row - 1, column] == 'O') // Up
                {
                    grid[row - 1, column] = 'X';
                    Console.WriteLine("Компьютер добил / попал в соседнюю клетку сверху!");
                    hasAdjacentTarget = true;
                }
                if (row < grid.GetLength(0) - 1 && grid[row + 1, column] == 'O') // Down
                {
                    grid[row + 1, column] = 'X';
                    Console.WriteLine("Компьютер добил / попал в соседнюю клетку снизу!");
                    hasAdjacentTarget = true;
                }

                if (!hasAdjacentTarget)
                {
                    break;
                }
            }
            // Проверка, не затонул ли весь корабль целиком
            bool isShipSunk = true;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 'O')
                    {
                        isShipSunk = false;
                        break;
                    }
                }
                if (!isShipSunk)
                {
                    break;
                }
            }

            if (isShipSunk)
            {
                for (int i = Math.Max(0, row - 1); i <= Math.Min(grid.GetLength(0) - 1, row + 1); i++)
                {
                    for (int j = Math.Max(0, column - 1); j <= Math.Min(grid.GetLength(1) - 1, column + 1); j++)
                    {
                        if (grid[i, j] != 'X')
                        {
                            grid[i, j] = '@';
                        }
                    }
                }
        

    

// Если корабль потоплен, компьютер продолжает игру и начинает стрелять рандомно пока не ранит корабль, тогда цикл должен повториться
Console.WriteLine("Корабль потоплен!");
                ResetGrid(grid);
            }

            return true;
        }
        else if (grid[row, column] == '~')
        {
            grid[row, column] = '*';
            Console.WriteLine("Промазал!");
            return false;
        }
        else
        {
            Console.WriteLine("Вы уже стреляли по этой клетке. Попробуйте еще раз.");
            return false;
        }
    }

    static void ResetGrid(char[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == 'X' || grid[i, j] == '*')
                {
                    grid[i, j] = '~';
                }
            }
        }
    }




    static bool CheckGameOver(char[,] grid)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (grid[i, j] == 'O')
                {
                    return false;
                }
            }
        }

        return true;
    }
    //Проверка Соприкосновения кораблей
    static bool HasNeighboringShip(char[,] grid, string coordinate)
    {
        int row = coordinate[0] - 65;
        int column = int.Parse(coordinate.Substring(1));


        // Проверяем соседние клетки по вертикали
        if (row - 1 >= 0 && grid[row - 1, column] == 'O')
        {
            return true;
        }
        if (row + 1 < 10 && grid[row + 1, column] == 'O')
        {
            return true;
        }

        // Проверяем соседние клетки по горизонтали
        if (column - 1 >= 0 && grid[row, column - 1] == 'O')
        {
            return true;
        }
        if (column + 1 < 10 && grid[row, column + 1] == 'O')
        {
            return true;
        }

        return false;
    }
}
