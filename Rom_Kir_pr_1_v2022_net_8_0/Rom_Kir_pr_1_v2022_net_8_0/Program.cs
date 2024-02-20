using System;

class Program
{
    static void Main()
    {
        // Флаг для управления повторением выбора операций
        bool repeat = true;
        string rookPos = string.Empty;
        string piecePos = string.Empty;

        // Основной цикл программы, продолжается до тех пор, пока пользователь не выберет выход
        while (repeat)
        {
            // Вывод меню выбора действий
            Console.WriteLine("Выберите одно из действий:");
            Console.WriteLine("1. Разместить фигуры на шахматной доске");
            Console.WriteLine("2. Определить, бьет ли ладья фигуру");
            Console.WriteLine("3. Выйти из программы");
            Console.Write("Ваш выбор: ");

            // Считывание выбора действия от пользователя и преобразование его в целое число
            int choice = Convert.ToInt32(Console.ReadLine());

            // Выполнение выбранного действия
            switch (choice)
            {
                case 1:
                    // Размещение фигур на шахматной доске
                    SetupBoard(out rookPos, out piecePos);
                    break;
                case 2:
                    // Проверка возможности побить фигуру ладьёй
                    if (rookPos != string.Empty && piecePos != string.Empty)
                    {
                        CheckCapture(rookPos, piecePos);
                    }
                    else
                    {
                        Console.WriteLine("Фигуры на шахматной доске не размещены");
                    }
                    break;
                case 3:
                    // Завершение программы
                    repeat = false;
                    break;
                default:
                    // Обработка неверного выбора
                    Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                    break;
            }

            // Переход на новую строку для удобства чтения
            Console.WriteLine();
        }
    }

    // Метод для размещения фигур на доске
    static void SetupBoard(out string rookPos, out string piecePos)
    {
        // Инициализация доски
        char[,] board = new char[8, 8];
        InitializeBoard(board);

        // Запрос координат для размещения ладьи и фигуры
        Console.WriteLine("Введите координаты ладьи и фигуры (в формате x1y1 x2y2):");
        string input = Console.ReadLine();

        // Разделение введённых координат
        string[] coordinates = input.Split(' ');

        // Проверка корректности введённых координат
        if (coordinates.Length != 2 || coordinates[0] == coordinates[1] || !ValidateCoordinates(coordinates[0]) || !ValidateCoordinates(coordinates[1]))
        {
            Console.WriteLine("Введены некорректные координаты");
            rookPos = string.Empty;
            piecePos = string.Empty;
            return;
        }

        // Размещение фигур на доске
        rookPos = coordinates[0];
        piecePos = coordinates[1];
        PlacePieces(board, rookPos, piecePos);

        // Вывод доски
        DrawBoard(board);
    }

    // Метод для проверки, может ли ладья побить фигуру
    static void CheckCapture(string rookPos, string piecePos)
    {
        // Вывод информации о проверке
        Console.WriteLine("Операция  2: Определение, бьет ли ладья фигуру");
        Console.WriteLine();

        // Вычисление координат ладьи и фигуры
        int rookX = rookPos[0] - 'a';
        int rookY = rookPos[1] - '1';

        int pieceX = piecePos[0] - 'a';
        int pieceY = piecePos[1] - '1';

        // Проверка, может ли ладья побить фигуру
        if (rookX == pieceX || rookY == pieceY)
        {
            Console.WriteLine("Ладья сможет побить фигуру за  1 ход");
        }
        else
        {
            int moveCount = 0;
            List<string> moves = new List<string>();

            // Генерация последовательности ходов
            if (rookX != pieceX)
            {
                moveCount++;
                moves.Add($"R{piecePos[0]}{rookPos[1]}");
            }

            if (rookY != pieceY)
            {
                moveCount++;
                moves.Add($"Rx{piecePos[0]}{piecePos[1]}#"); // x - означает срубку фигуры, # - означает мат, но вданном случае конец партии  
            }

            // Вывод результатов
            Console.WriteLine($"Ладья сможет побить фигуру за {moveCount} ход(а)");
            Console.WriteLine("Последовательность ходов:");
            Console.WriteLine(string.Join(" -> ", moves));
        }
    }

    // Метод для инициализации доски
    static void InitializeBoard(char[,] board)
    {
        // Заполнение доски пустыми клетками
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                board[row, col] = '-';
            }
        }
    }

    // Метод для размещения фигур на доске
    static void PlacePieces(char[,] board, string rookPos, string piecePos)
    {
        // Вычисление координат ладьи и фигуры
        int rookX = rookPos[0] - 'a';
        int rookY = rookPos[1] - '1';

        int pieceX = piecePos[0] - 'a';
        int pieceY = piecePos[1] - '1';

        // Размещение ладьи и фигуры на доске
        MoveRook(board, rookX, rookY);
        PlacePiece(board, pieceX, pieceY, 'F');
    }

    // Метод для размещения ладьи на доске
    static void MoveRook(char[,] board, int x, int y)
    {
        // Проверка корректности координат
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            board[y, x] = 'R'; // Размещаем ладью на выбранных координатах
        }
    }

    // Метод для размещения фигуры на доске
    static void PlacePiece(char[,] board, int x, int y, char piece)
    {
        // Проверка корректности координат
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            board[y, x] = piece; // Размещаем фигуру на выбранных координатах
        }
    }

    // Метод для вывода доски
    static void DrawBoard(char[,] board)
    {
        // Вывод заголовка доски
        Console.WriteLine("   a b c d e f g h");

        // Вывод доски в обратном порядке (с верхней стороны)
        for (int row = 7; row >= 0; row--)
        {
            Console.Write($"{row + 1} ");

            for (int col = 0; col < 8; col++)
            {
                Console.Write(board[row, col] + " ");
            }

            Console.WriteLine();
        }
    }

    // Метод для проверки корректности введённых координат
    static bool ValidateCoordinates(string coordinate)
    {
        // Проверка длины строки координат
        if (coordinate.Length != 2)
        {
            return false;
        }

        // Проверка диапазона символов координат
        char file = coordinate[0];
        char rank = coordinate[1];

        if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
        {
            return false;
        }

        return true;
    }

    // Метод для определения возможности побить фигуру ладьёй
    static bool CanRookCapture(int rookX, int rookY, int pieceX, int pieceY)
    {
        return rookX == pieceX || rookY == pieceY;
    }
}
