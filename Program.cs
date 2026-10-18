using LifeSim;

// MAIN: menu and start routing only.
int game_started = 0;
string menu_input = "";

while (game_started == 0)
{
    Console.Clear();
    Console.WriteLine("Welcome to life sim");
    Console.WriteLine("1. Start");
    Console.WriteLine("2. Options");
    Console.WriteLine("3. Credits");
    Console.WriteLine();
    Console.Write(">> ");

    menu_input = (Console.ReadLine() ?? "").Trim();

    // Accept either the full word or its menu number.
    if (menu_input.Equals("start", StringComparison.OrdinalIgnoreCase) || menu_input == "1")
    {
        game_started = 1;
    }
    else if (menu_input.Equals("options", StringComparison.OrdinalIgnoreCase) || menu_input == "2")
    {
        // Placeholder by design: no options page yet. Loop back to the menu.
        continue;
    }
    else if (menu_input.Equals("credits", StringComparison.OrdinalIgnoreCase) || menu_input == "3")
    {
        // Placeholder by design: no credits page yet. Loop back to the menu.
        continue;
    }
}

if (game_started == 1)
{
    World.Run();
}
