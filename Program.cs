using static System.Reflection.Metadata.BlobBuilder;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary = new List<SweEngGloss>();//Skapar och initialiserar lista
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }

            public static void Translation(string translateWord)
            {

                bool foundTranslation = false;

                foreach (SweEngGloss gloss in dictionary)
                {
                    if (gloss.word_swe == translateWord)
                    {
                        Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                        foundTranslation = true;
                    }
                    if (gloss.word_eng == translateWord)
                    {
                        Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                        foundTranslation = true;
                    }
                }
                if (!foundTranslation)
                {
                    throw new KeyNotFoundException("Translation not found for this word");
                }
            }         
        }
        static string LoadFile(string fileToLoad)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileToLoad))
                {
                    dictionary = new List<SweEngGloss>(); // Empty it!
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        SweEngGloss gloss = new SweEngGloss(line);
                        dictionary.Add(gloss);
                        line = sr.ReadLine();
                    }
                }
                Console.WriteLine($"{fileToLoad} has been loaded");
                return $"{fileToLoad} has been loaded";
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found. Please provide a valid file path");
                return "File not found. Please provide a valid file path";
            }
            catch (IOException)
            {
                Console.WriteLine("return \"An I/O error occured while loading the file");
                return "An I/O error occured while loading the file";
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
        private static bool IsWordAlreadyInList(string sweWord, string engWord)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if(gloss.word_swe == sweWord || gloss.word_eng == engWord)
                {
                    return true; //Returnering om ordet finns
                }
            }
            return false; //Returnering om ordet inte finns
        }
        public static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("------------------------------");
            Console.WriteLine("'help'       -  hjälp");
            Console.WriteLine("'load'       -  ladda");
            Console.WriteLine("'list'       -  lista");
            Console.WriteLine("'translate'  -  översätt");
            Console.WriteLine("'new'        -  lägg till ord");
            Console.WriteLine("'delete'     -  ta bort ord");
            Console.WriteLine("'quit'       -  avsluta");
            Console.WriteLine("------------------------------");
        }
        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            PrintHelp();
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else if (command == "load")
                {
                    if (argument.Length == 2)
                    {
                        string directory = "..\\..\\..\\dict\\";
                        string fileToLoad = ($"{directory}{argument[1]}");
                        LoadFile(fileToLoad);
                    }
                    else if (argument.Length == 1)
                    {
                        string fileToLoad = defaultFile;
                        LoadFile(fileToLoad);
                    }
                }
                else if (command == "list")
                {
                    if (dictionary.Count == 0)
                    {
                        Console.WriteLine("No list loaded. Please load list");
                    }
                    else
                    {
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                        }
                    }
                }
                else if (command == "new")
                {
                    //FIXME: Felhantering och kontroll eventuella dubletter vid addering av nya ord
                    if (argument.Length == 3)
                    {
                        string sweWordToAdd = argument[1];
                        string engWordToAdd = argument[2];

                        if (!IsWordAlreadyInList(sweWordToAdd, engWordToAdd))
                        {
                            dictionary.Add(new SweEngGloss(sweWordToAdd, engWordToAdd));
                            Console.WriteLine($"{sweWordToAdd}/{engWordToAdd} added to the list");
                        }
                        else
                        {
                            Console.WriteLine("Word already exists in the list");
                        }
                    }
                    else if (argument.Length == 1)
                    {
                        Console.Write("Write word in Swedish: ");
                        string sweWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string engWord = Console.ReadLine();

                        if (!IsWordAlreadyInList(sweWord, engWord))
                        {
                            dictionary.Add(new SweEngGloss(sweWord, engWord));
                            Console.WriteLine($"{sweWord}/{engWord} added to the list");
                        }
                        else
                        {
                            Console.WriteLine("Word already exists in the list");
                        }
                    }
                }
                else if (command == "delete") 
                {
                    //FIXME: Felhantering om man försöker ta bort ord som inte finns 
                    //TODO: En else if som tar två ord för att bara skriva 'delete {svenskt/engelskt ord}'
                    if (argument.Length == 3)
                    {
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++) {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == argument[1] && gloss.word_eng == argument[2])
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.Write("Write word in Swedish: ");
                        string sweWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string engWord = Console.ReadLine();
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == sweWord && gloss.word_eng == engWord)
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)
                    {
                        try
                        {
                            string userInputToTranslate = argument[1];
                            SweEngGloss.Translation(userInputToTranslate);
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                    else if (argument.Length == 1)
                    {
                        try
                        {
                            Console.WriteLine("Write word to be translated: ");
                            string userInputToTranslate = Console.ReadLine();
                            SweEngGloss.Translation(userInputToTranslate);
                        }
                        catch(KeyNotFoundException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                else if (command == "help")
                {
                    PrintHelp();
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
            //TODO: En ReadKey-funktion som väntar på input för att stänga för att säkerställa att användaren hinner se output
        }
    }
}