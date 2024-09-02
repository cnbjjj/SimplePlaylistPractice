class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Stack<string> history = new Stack<string>();
        Queue<string> playlist = new Queue<string>();
        int option = -1;
        while (option != (int)Options.EXIT)
        {
            option = ReadInputAsInt(MENU, null, WARNING, 1, 5);
            switch ((Options)option)
            {
                case Options.ADD:
                    string song = ReadInputAsString("Enter song name: ", null, WARNING, str => str.Trim().Length > 0);
                    Add(song, ref playlist);
                    Console.WriteLine($"[ {song} ] added to your playlist [ {string.Join(", ", playlist)} ]");
                    break;
                case Options.PLAY:
                    Play(ref playlist, ref history);
                    break;
                case Options.SKIP:
                    Skip(ref playlist, ref history);
                    break;
                case Options.REWIND:
                    Rewind(ref playlist, ref history);
                    break;
                case Options.EXIT:
                    Exit();
                    break;
                default:
                    Console.WriteLine(WARNING);
                    break;
            }
            Console.WriteLine("\n---------------------------------------------------------------\n");
        }
    }

    enum Options
    {
        ADD = 1,
        PLAY,
        SKIP,
        REWIND,
        EXIT
    }
    static string currentSong = "";
    const string WARNING = "Invalid Input";
    const string MENU = "1. Add a song to the playlist\n2. Play the next song in the playlist\n3. Skip the next song in the playlist\n4. Rewind to the previous song\n5. Exit\nChoose an option: ";
    static bool IsPlaylistEmpty(Queue<string> playlist)
    {
        if (playlist.Count == 0)
        {
            Console.WriteLine("Your playlist is empty.");
            return true;
        }
        return false;
    }
    static string GetNextSong(Queue<string> playlist)
    {
        return playlist.Count == 0 ? "No song in the playlist" : playlist.Peek();
    }
    static string GetPreviousSong(Stack<string> history)
    {
        return history.Count == 0 ? "" : history.Peek();
    }
    static void Add(string song, ref Queue<string> playlist)
    {
        playlist.Enqueue(song);
    }
    static void Play(ref Queue<string> playlist, ref Stack<string> history)
    {
        if (IsPlaylistEmpty(playlist)) return;
        if(currentSong != "") history.Push(currentSong);
        currentSong = playlist.Dequeue();
        Console.WriteLine($"Now playing: [ {currentSong} ]");
        Console.WriteLine($"Next song: [ {GetNextSong(playlist)} ]");
    }
    static void Skip(ref Queue<string> playlist, ref Stack<string> history)
    {
        if (IsPlaylistEmpty(playlist)) return;
        Console.WriteLine($"Skipping the next song [ {playlist.Dequeue()} ].");
        Console.WriteLine($"Now playing: [ {currentSong} ]");
        Console.WriteLine($"Next song: [ {GetNextSong(playlist)} ]");
    }
    static void Rewind(ref Queue<string> playlist, ref Stack<string> history)
    {
        if (GetPreviousSong(history) == "")
        {
            Console.WriteLine("No played song in the history.");
            return;
        }
        string previousSong = history.Pop();
        Queue<string> newPlaylist = new Queue<string>();
        newPlaylist.Enqueue(previousSong);
        while (playlist.Count > 0)
        {
            newPlaylist.Enqueue(playlist.Dequeue());
        }
        playlist = newPlaylist;
        Play(ref playlist, ref history);
    }
    static void Exit()
    {
        Console.WriteLine("Goodbye!");
    }

    // Utility Functions
    static int ReadInputAsInt(string question, string quit = null, string waring = WARNING, int min = int.MinValue, int max = int.MaxValue)
    {
        string res = ReadInputAsString(question, "", waring, str => int.TryParse(str, out int num) && num >= min && num <= max);
        if (res == quit) return int.MinValue;
        return int.Parse(res);
    }
    static string ReadInputAsString(string question, string quit = null, string warning = WARNING, Func<string, bool> validator = null, Func<string, string> Prompt = null)
    {
        if (validator == null) validator = (str) => !string.IsNullOrEmpty(str);
        if (Prompt == null) Prompt = ReadRawInput;
        string str = Prompt(question).ToLower();
        while (str != quit && !validator(str))
        {
            Console.WriteLine(warning);
            str = Prompt(question).ToLower();
        }
        return str;
    }
    static string ReadRawInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }
}