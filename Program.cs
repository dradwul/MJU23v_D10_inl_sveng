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
                foreach (SweEngGloss gloss in dictionary)
                {
                    if (gloss.word_swe == translateWord)
                        Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                    if (gloss.word_eng == translateWord)
                        Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                }
            }         
        }
        static string LoadFile(string fileToLoad)
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
            return "File loaded";
        }
        public static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("------------------------------");
            Console.WriteLine("'help'       -  hjälp");
            Console.WriteLine("'load'       -  ladda");
            Console.WriteLine("'list'       -  lista");
            Console.WriteLine("'translate'  -  översätt");
            Console.WriteLine("'add'        -  lägg till ord");
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
                //FIXME: Felhantering vid tom/ogiltig input
                //TODO: Lägg in en verifiering för användaren att listan laddats
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
                else if (command == "list") //TODO: Kolla så att denna fungerar efter initialisering av listan
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
                else if (command == "new")
                {
                    //FIXME: Felhantering och kontroll eventuella dubletter vid addering av nya ord
                    if (argument.Length == 3)
                    {
                        dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                    }
                    else if (argument.Length == 1)
                    {
                        Console.Write("Write word in Swedish: ");
                        string sweWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string engWord = Console.ReadLine();
                        dictionary.Add(new SweEngGloss(sweWord, engWord));
                    }
                }
                else if (command == "delete") 
                {
                    //FIXME: Felhantering om man försöker ta bort ord som inte finns 
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
                    //TODO: Kontrollera så funktion fungerar när listan är initialiserad
                    //FIXME: Felhantering vid ogiltig input. T.ex. ord som inte finns
                {
                    if (argument.Length == 2)
                    {
                        string userInputToTranslate = argument[1];
                        SweEngGloss.Translation(userInputToTranslate);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string userInputToTranslate = Console.ReadLine();
                        SweEngGloss.Translation(userInputToTranslate);
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