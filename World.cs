using System.Text.Json;

namespace LifeSim;

// WORLD: deliberately separate from the main menu. Keep character state as individual
// variables and integer switches rather than putting it into a Character class.
public static class World
{
    public static void Run()
    {
        int world_started = 0;
        int character_loaded = 0;
        int save_written = 0;

        string era = "";
        string character_name = "";
        string gender = "";
        string character_class = "";
        int age = 17;

        while (world_started == 0)
        {
            Console.Clear();
            Console.WriteLine("Choose your era -");
            Console.WriteLine("1. 547BCE");
            Console.WriteLine();
            Console.Write(">> ");
            string era_input = (Console.ReadLine() ?? "").Trim();

            if (era_input == "1" || era_input.Equals("547BCE", StringComparison.OrdinalIgnoreCase) || era_input.Equals("547 BCE", StringComparison.OrdinalIgnoreCase))
            {
                era = "547BCE";
                world_started = 1;
            }
        }

        Console.Clear();
        Console.WriteLine("Generating world...");
        Console.WriteLine();
        Console.WriteLine("Class: European");
        Console.WriteLine("Age: 17");
        Console.WriteLine();

        character_class = "European";
        age = 17;

        string save_directory = Path.Combine(Directory.GetCurrentDirectory(), "saves");
        Directory.CreateDirectory(save_directory);

        while (character_name.Length == 0)
        {
            Console.Write("What is Your name:\n\n>> ");
            character_name = (Console.ReadLine() ?? "").Trim();
            if (character_name.Length == 0)
            {
                Console.WriteLine("Name cannot be blank.");
            }
        }

        string safe_name = MakeSafeFileName(character_name);
        string save_path = Directory.EnumerateFiles(save_directory, "*.json")
            .FirstOrDefault(path => Path.GetFileNameWithoutExtension(path).Equals(safe_name, StringComparison.OrdinalIgnoreCase))
            ?? Path.Combine(save_directory, safe_name + ".json");

        // A matching character-name file means continue that character instead of creating another.
        if (File.Exists(save_path))
        {
            try
            {
                using JsonDocument save_document = JsonDocument.Parse(File.ReadAllText(save_path));
                JsonElement root = save_document.RootElement;

                character_name = ReadString(root, "character_name", character_name);
                gender = ReadString(root, "gender", "");
                character_class = ReadString(root, "character_class", character_class);
                era = ReadString(root, "era", era);
                age = ReadInt(root, "age", age);
                character_loaded = 1;
            }
            catch (JsonException)
            {
                Console.WriteLine();
                Console.WriteLine("The existing save could not be read as valid JSON. It was not overwritten.");
                Console.WriteLine("Press Enter to return to the main menu.");
                Console.ReadLine();
                return;
            }
            catch (IOException)
            {
                Console.WriteLine();
                Console.WriteLine("The existing save could not be read. It was not overwritten.");
                Console.WriteLine("Press Enter to return to the main menu.");
                Console.ReadLine();
                return;
            }
        }
        else
        {
            Console.Write("\nWhat is Your gender?\n\n>> ");
            gender = (Console.ReadLine() ?? "").Trim();
        }

        Console.WriteLine();
        Console.WriteLine(character_loaded == 1 ? "Loading..." : "# Saving...");

        if (character_loaded == 0)
        {
            // Save individual variables directly. No Character class is used.
            string json = JsonSerializer.Serialize(new
            {
                character_name,
                gender,
                character_class,
                age,
                era,
                world_exists = 1,
                character_exists = 1,
                poster = 0
            }, new JsonSerializerOptions { WriteIndented = true });

            try
            {
                File.WriteAllText(save_path, json);
                save_written = 1;
            }
            catch (IOException)
            {
                Console.WriteLine("Could not write the JSON save file.");
                Console.WriteLine("Press Enter to return to the main menu.");
                Console.ReadLine();
                return;
            }
        }
        else
        {
            save_written = 1;
        }

        Console.Clear(); // OS.CLEAR
        if (save_written == 1)
        {
            Console.WriteLine(character_loaded == 1 ? "Save loaded." : "Save created.");
            Console.WriteLine($"Character: {character_name}");
            Console.WriteLine($"Era: {era}");
            Console.WriteLine();
            Console.WriteLine("World initialized. Gameplay will be added here next.");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to return to the main menu.");
        Console.ReadLine();
    }

    private static string MakeSafeFileName(string name)
    {
        char[] invalid_characters = Path.GetInvalidFileNameChars();
        string safe_name = new string(name.Select(character => invalid_characters.Contains(character) ? '_' : character).ToArray()).Trim().Trim('.');
        return safe_name.Length == 0 ? "player" : safe_name;
    }

    private static string ReadString(JsonElement root, string property_name, string fallback)
    {
        return root.TryGetProperty(property_name, out JsonElement value) && value.ValueKind == JsonValueKind.String
            ? value.GetString() ?? fallback
            : fallback;
    }

    private static int ReadInt(JsonElement root, string property_name, int fallback)
    {
        return root.TryGetProperty(property_name, out JsonElement value) && value.TryGetInt32(out int number)
            ? number
            : fallback;
    }
}
