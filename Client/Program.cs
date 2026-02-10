using SnmpSharpNet;
using Spectre.Console;
using Spectre.Console.Json;
using Spectre.Console.Rendering;
using System.Management;
using System.Net;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace Client
{

    public class UserModel
    {
        public string user { get; set; }
        public string model { get; set; }

        public UserModel(string user = "-", string model = "-", string name = "-")
        {
            this.user = user;
            this.model = model;
        }
    }

    public class ipResponse
    {
        public string address { get; set; }
        public string hostname { get; set; }
        public string lastLoggedUser { get; set; }

        public string operatingSystem { get; set; }
        public string model { get; set; }
        public string serialNumber { get; set; }
        public int procGen { get; set; }

        public DateTime lastCheckedDate { get; set; }
        public DateTime lastFoundDate { get; set; }

        public bool successFinding { get; set; }

        public ipResponse(string address, DateTime lastCheckedDate)
        {
            this.address = address;
            this.lastCheckedDate = lastCheckedDate;
            this.lastLoggedUser = "";
            this.hostname = "";
            this.operatingSystem = "";
            this.lastFoundDate = new DateTime();
            this.procGen = 0;
        }
    }
    public class IP
    {
        public string address { get; set; }
        public string hostname { get; set; }
        public string lastCheckedDate { get; set; }

        public bool isOperated { get; set; }

        public IP(string address = "127.0.01", string hostname = "", string lastCheckedDate = "00.00.0001")
        {
            this.address = address;
            this.hostname = hostname;
            this.lastCheckedDate = lastCheckedDate;
        }
    }

    class Server
    {
        [JsonInclude]
        private string address { get; set; }
        [JsonInclude]
        private string port { get; set; }

        public Server(string address, string port)
        {
            this.address = address;
            this.port = port;
        }

        public string getAddress() => address;
        public string getPort() => port;

        public void setAddress(string address)
        {
            this.address = address;
        }

        public void setPort(string port)
        {
            this.port = port;
        }
    }

    public class TerminalTheme
    {
        // Normal ANSI colors
        [YamlMember(Alias = "color_01")]
        public string BlackHost { get; set; }

        [YamlMember(Alias = "color_02")]
        public string RedSyntaxString { get; set; }

        [YamlMember(Alias = "color_03")]
        public string GreenCommand { get; set; }

        [YamlMember(Alias = "color_04")]
        public string YellowCommandSecond { get; set; }

        [YamlMember(Alias = "color_05")]
        public string BluePath { get; set; }

        [YamlMember(Alias = "color_06")]
        public string MagentaSyntaxVar { get; set; }

        [YamlMember(Alias = "color_07")]
        public string CyanPrompt { get; set; }

        [YamlMember(Alias = "color_08")]
        public string White { get; set; }

        // Bright ANSI colors
        [YamlMember(Alias = "color_09")]
        public string BrightBlack { get; set; }

        [YamlMember(Alias = "color_10")]
        public string BrightRedCommandError { get; set; }

        [YamlMember(Alias = "color_11")]
        public string BrightGreenExec { get; set; }

        [YamlMember(Alias = "color_12")]
        public string BrightYellow { get; set; }

        [YamlMember(Alias = "color_13")]
        public string BrightBlueFolder { get; set; }

        [YamlMember(Alias = "color_14")]
        public string BrightMagenta { get; set; }

        [YamlMember(Alias = "color_15")]
        public string BrightCyan { get; set; }

        [YamlMember(Alias = "color_16")]
        public string BrightWhite { get; set; }

        // Core colors
        [YamlMember(Alias = "background")]
        public string Background { get; set; }

        [YamlMember(Alias = "foreground")]
        public string ForegroundText { get; set; }

        [YamlMember(Alias = "cursor")]
        public string Cursor { get; set; }

        public TerminalTheme()
        {
            // Standard ANSI (approx Windows / VT100 defaults)
            BlackHost = "#000000";
            RedSyntaxString = "#800000";
            GreenCommand = "#008000";
            YellowCommandSecond = "#808000";
            BluePath = "#000080";
            MagentaSyntaxVar = "#800080";
            CyanPrompt = "#008080";
            White = "#C0C0C0";

            BrightBlack = "#808080";
            BrightRedCommandError = "#FF0000";
            BrightGreenExec = "#00FF00";
            BrightYellow = "#FFFF00";
            BrightBlueFolder = "#0000FF";
            BrightMagenta = "#FF00FF";
            BrightCyan = "#00FFFF";
            BrightWhite = "#FFFFFF";

            Background = "#000000";
            ForegroundText = "#C0C0C0";
            Cursor = "#C0C0C0";
        }

    }

    public sealed class TerminalThemeRGB
    {
        // Normal ANSI colors
        public (byte R, byte G, byte B) BlackHost { get; }
        public (byte R, byte G, byte B) RedSyntaxString { get; }
        public (byte R, byte G, byte B) GreenCommand { get; }
        public (byte R, byte G, byte B) YellowCommandSecond { get; }
        public (byte R, byte G, byte B) BluePath { get; }
        public (byte R, byte G, byte B) MagentaSyntaxVar { get; }
        public (byte R, byte G, byte B) CyanPrompt { get; }
        public (byte R, byte G, byte B) White { get; }

        // Bright ANSI colors
        public (byte R, byte G, byte B) BrightBlack { get; }
        public (byte R, byte G, byte B) BrightRedCommandError { get; }
        public (byte R, byte G, byte B) BrightGreenExec { get; }
        public (byte R, byte G, byte B) BrightYellow { get; }
        public (byte R, byte G, byte B) BrightBlueFolder { get; }
        public (byte R, byte G, byte B) BrightMagenta { get; }
        public (byte R, byte G, byte B) BrightCyan { get; }
        public (byte R, byte G, byte B) BrightWhite { get; }

        // Core colors
        public (byte R, byte G, byte B) Background { get; }
        public (byte R, byte G, byte B) ForegroundText { get; }
        public (byte R, byte G, byte B) Cursor { get; }

        public TerminalThemeRGB(TerminalTheme theme)
        {
            if (theme is null) throw new ArgumentNullException(nameof(theme));

            BlackHost = HexToRgbBytes(theme.BlackHost);
            RedSyntaxString = HexToRgbBytes(theme.RedSyntaxString);
            GreenCommand = HexToRgbBytes(theme.GreenCommand);
            YellowCommandSecond = HexToRgbBytes(theme.YellowCommandSecond);
            BluePath = HexToRgbBytes(theme.BluePath);
            MagentaSyntaxVar = HexToRgbBytes(theme.MagentaSyntaxVar);
            CyanPrompt = HexToRgbBytes(theme.CyanPrompt);
            White = HexToRgbBytes(theme.White);

            BrightBlack = HexToRgbBytes(theme.BrightBlack);
            BrightRedCommandError = HexToRgbBytes(theme.BrightRedCommandError);
            BrightGreenExec = HexToRgbBytes(theme.BrightGreenExec);
            BrightYellow = HexToRgbBytes(theme.BrightYellow);
            BrightBlueFolder = HexToRgbBytes(theme.BrightBlueFolder);
            BrightMagenta = HexToRgbBytes(theme.BrightMagenta);
            BrightCyan = HexToRgbBytes(theme.BrightCyan);
            BrightWhite = HexToRgbBytes(theme.BrightWhite);

            Background = HexToRgbBytes(theme.Background);
            ForegroundText = HexToRgbBytes(theme.ForegroundText);
            Cursor = HexToRgbBytes(theme.Cursor);
        }

        private static (byte R, byte G, byte B) HexToRgbBytes(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Hex color is null/empty.", nameof(hex));

            hex = hex.Trim();

            if (hex.StartsWith("#", StringComparison.Ordinal))
                hex = hex[1..];

            // Support #RGB shorthand if you ever decide to use it
            if (hex.Length == 3)
                hex = string.Concat(hex[0], hex[0], hex[1], hex[1], hex[2], hex[2]);

            if (hex.Length != 6)
                throw new FormatException($"Invalid hex color '{hex}'. Expected 6 hex digits (RRGGBB).");

            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);

            return (r, g, b);
        }
    }






    internal class Program
    {
        const int MENU_EXIT = -1;
        const int MENU_SERVER_CLIENT = 0;
        const int MENU_DATABYSE_CONNECTION_TYPE = 1;
        const int MENU_DATABASE_JSON = 2;
        const int MENU_DATABASE_INPUT = 3;
        const int MENU_PROCESS_SERVER_SIDE = 4;

        const int MENU_CONNECT_TO_SERVER_TYPE = 11;
        const int MENU_CLIENT_JSON = 12;
        const int MENU_CLIENT_INPUT = 13;
        const int MENU_INPUT_CREDENTIALS = 14;
        const int MENU_PROCESS_CLIENT = 15;
        const int MENU_SELECT_THEME = 16;

        const int MAX_PING_COUNTER = 3;

        const string SELECTION_BACK = "back";
        const string NO_LOGGED_USER = "no_user";

        const int PING_TIMEOUT = 1000;

        const int MAX_HTTP_REQUEST_RETRY = 2;

        const int FRAMES = 4;

        private static readonly object _scanCtsLock = new();
        private static CancellationTokenSource? _currentScanCts;

        static TerminalTheme myTheme = new TerminalTheme();
        static TerminalThemeRGB myThemeRgb = new TerminalThemeRGB(myTheme);
        static Color highlightColor;
        static Color searchHighlightColor;
        static Color promptChoicesColor;
        static Color promptDefaultChoicesColor;
        static String headerColor;
        static void SetCurrentScanCts(CancellationTokenSource? cts)
        {
            lock (_scanCtsLock)
                _currentScanCts = cts;
        }

        static void CancelCurrentScan()
        {
            lock (_scanCtsLock)
                _currentScanCts?.Cancel();
        }

        public static string url = "";
        static async Task Main(string[] args)
        {
            string currentDir = Directory.GetCurrentDirectory();

            int menu = MENU_CONNECT_TO_SERVER_TYPE;
            int height = AnsiConsole.Console.Profile.Height;
            Server serverData = null;

            // Used to check if already started displaying data - to not start multiple workers
            // bool startedDisplaying = false;

            // Used for providing custom credentals to WMI
            bool usingCustomCredentials = false;
            bool askedForCredentials = false;
            string credentialsUsername = "";
            SecureString credentialsPassword = ToSecureString();

            int processedAddresses = 0;
            int httpRequestCounter = 0;


            using var appCts = new CancellationTokenSource();
            var shortcutTask = ListenForShortcutAsync(appCts);

            myThemeRgb = new TerminalThemeRGB(myTheme);
            highlightColor = new Color(myThemeRgb.CyanPrompt.R,
                        myThemeRgb.CyanPrompt.G,
                        myThemeRgb.CyanPrompt.B);

            searchHighlightColor = new Color(myThemeRgb.BrightCyan.R,
                        myThemeRgb.BrightCyan.G,
                        myThemeRgb.BrightCyan.B);

            promptChoicesColor = new Color(myThemeRgb.BrightMagenta.R,
                        myThemeRgb.BrightMagenta.G,
                        myThemeRgb.BrightMagenta.B);


            promptDefaultChoicesColor = new Color(myThemeRgb.BrightGreenExec.R,
                        myThemeRgb.BrightGreenExec.G,
                        myThemeRgb.BrightGreenExec.B);

            headerColor = myTheme.BluePath;/*new Color(myThemeRgb.BluePath.R,
                        myThemeRgb.BluePath.G,
                        myThemeRgb.BluePath.B);*/

            while (true)
            {



                if (menu == MENU_EXIT)
                {
                    break;
                }
                if (menu == MENU_CONNECT_TO_SERVER_TYPE)
                {
                    //getting server connection data
                    Console.Clear();

                    var clientConnectionPrompt = new SelectionPrompt<string>()
                    .Title("Choose client connection configuration method:")
                    .PageSize(5)
                    .AddChoices(new[] { "Load from .JSON", "Input manually", "Change Theme", "Exit" });



                    clientConnectionPrompt.HighlightStyle = new Style(highlightColor);
                    var clientConnectionChoice = AnsiConsole.Prompt(clientConnectionPrompt);



                    //resseting state of displaying live data of scanner
                    //startedDisplaying = false;

                    if (clientConnectionChoice == "Load from .JSON")
                    {
                        menu = MENU_CLIENT_JSON;
                    }
                    else if (clientConnectionChoice == "Input manually")
                    {
                        menu = MENU_CLIENT_INPUT;
                    }
                    else if (clientConnectionChoice == "Change Theme")
                    {
                        menu = MENU_SELECT_THEME;
                    }
                    else if (clientConnectionChoice == "Exit")
                    {
                        menu = MENU_SERVER_CLIENT;

                    }
                }
                else if (menu == MENU_CLIENT_JSON)
                {
                    Console.Clear();
                    string fileSelection = Program.fileSelection(currentDir, height);

                    if (fileSelection == "..")
                    {
                        if (Path.GetDirectoryName(currentDir) != null)
                        {
                            currentDir = Path.GetDirectoryName(currentDir);
                        }


                    }
                    else if (fileSelection.ToLower().EndsWith(".json"))
                    {

                        string absolutePath = $"{currentDir}{Path.DirectorySeparatorChar}{fileSelection}"
                        ;
                        //Unix puts only file name (without path) in fileSelection
                        var json = File.ReadAllText(absolutePath);

                        try
                        {
                            serverData = JsonSerializer.Deserialize<Server>(json);
                        }
                        catch (JsonException ex)
                        {
                            menu = MENU_CLIENT_JSON;
                            AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Failed to read:[/]  [{myTheme.YellowCommandSecond}]{absolutePath}[/]");
                            AnsiConsole.MarkupLine($"[{myTheme.BrightBlack}]{ex.Message}[/]");
                        }

                        if (serverData is not null)
                        {
                            var stringColor = new Color(myThemeRgb.BrightRedCommandError.R,
                                myThemeRgb.BrightRedCommandError.B,
                                myThemeRgb.BrightRedCommandError.G);

                            var memberColor = new Color(myThemeRgb.BrightBlueFolder.R,
                                myThemeRgb.BrightBlueFolder.B,
                                myThemeRgb.BrightBlueFolder.G);

                            var jsonPrint = new JsonText(
                           $$"""
                            { 
                                "serverAddress": "{{serverData.getAddress()}}", 
                                "portNumber": "{{serverData.getPort()}}"
                            }
                        """)
                                .StringColor(stringColor)
                                .MemberColor(memberColor);

                            var borderColor = new Color(myThemeRgb.BrightYellow.R,
                                myThemeRgb.BrightYellow.B,
                                myThemeRgb.BrightYellow.G);

                            AnsiConsole.Write(
                                new Panel(jsonPrint)
                                    .Header("Data read from JSON")
                                    .Collapse()
                                    .RoundedBorder()
                                    .BorderColor(borderColor));

                            if (serverData.getAddress() is null || serverData.getPort() is null)
                            {
                                AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Failed to read data[/]");

                                AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");
                                Console.ReadLine();
                                menu = MENU_CLIENT_JSON;
                            }
                            else
                            {
                                menu = MENU_PROCESS_CLIENT;
                                AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");
                                Console.ReadLine();
                            }



                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[{myTheme.BrightYellow}]File loaded but empty/null object:[/] {fileSelection}");
                        }



                    }
                    else if (fileSelection == SELECTION_BACK)
                    {
                        menu = MENU_CONNECT_TO_SERVER_TYPE;
                    }
                    else
                    {
                        currentDir += $"{Path.DirectorySeparatorChar}{fileSelection}";
                    }



                }
                else if (menu == MENU_CLIENT_INPUT)
                {
                    var serverNameInput = AnsiConsole.Prompt(

                    new TextPrompt<string>($"[[[{myTheme.BrightYellow}]localhost[/]]] Enter server hostname/ipAddress?:").AllowEmpty());
                    var serverName = serverNameInput != "" ? serverNameInput : "localhost";

                    var portInput = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[[[{myTheme.BrightYellow}]60719[/]]] Enter port number:").AllowEmpty());
                    string portNumber = portInput != "" ? portInput : "60719";

                    var textPrompt = new TextPrompt<bool>("Save to .json?")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(true)
                        .WithConverter(choice => choice ? "y" : "n");


                    textPrompt.ChoicesStyle(new Style(promptChoicesColor));
                    textPrompt.DefaultValueStyle(new Style(promptDefaultChoicesColor));


                    var ifSave = AnsiConsole.Prompt(textPrompt);

                    var stringColor = new Color(myThemeRgb.BrightRedCommandError.R,
                                myThemeRgb.BrightRedCommandError.B,
                                myThemeRgb.BrightRedCommandError.G);

                    var memberColor = new Color(myThemeRgb.BrightBlueFolder.R,
                        myThemeRgb.BrightBlueFolder.B,
                        myThemeRgb.BrightBlueFolder.G);



                    var borderColor = new Color(myThemeRgb.BrightYellow.R,
                        myThemeRgb.BrightYellow.B,
                        myThemeRgb.BrightYellow.G);

                    var jsonPrint = new Spectre.Console.Json.JsonText(
                           $$"""
                            { 
                                "serverName": "{{serverName}}", 
                                "portNumber": "{{portNumber}}"
                            }
                        """).StringColor(stringColor)
                        .MemberColor(memberColor); ;

                    AnsiConsole.Write(
                        new Panel(jsonPrint)
                            .Header("Data saved to JSON")
                            .Collapse()
                            .RoundedBorder()
                            .BorderColor(borderColor));



                    serverData = new Server(serverName, portNumber);

                    if (ifSave == true)
                    {
                        Server toSave = new Server(serverName, portNumber);

                        string serializedToSave = JsonSerializer.Serialize(toSave);

                        string fileName = currentDir + Path.DirectorySeparatorChar + "clientConnection_" + toSave.getAddress() + ".json";
                        bool isSavingSucces = true;
                        try
                        {
                            File.WriteAllText(fileName, serializedToSave);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            isSavingSucces = false;
                            AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Acces denied[/] to: {fileName}");
                        }

                        if (isSavingSucces)
                        {
                            currentDir += Path.DirectorySeparatorChar + fileName;
                            var path = new TextPath(currentDir).RootColor(Color.Red)
            .SeparatorColor(new Color(myThemeRgb.BrightBlack.R, myThemeRgb.BrightBlack.G, myThemeRgb.BrightBlack.B))
            .StemColor(new Color(myThemeRgb.BluePath.R, myThemeRgb.BluePath.G, myThemeRgb.BluePath.B))
            .LeafColor(new Color(myThemeRgb.YellowCommandSecond.R, myThemeRgb.YellowCommandSecond.G, myThemeRgb.YellowCommandSecond.B));

                            AnsiConsole.MarkupLine($"[{myTheme.GreenCommand}]Saved to:[/]");
                            AnsiConsole.Write(path);
                            AnsiConsole.WriteLine();
                        }


                        AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");
                        Console.ReadLine();

                    }
                    menu = MENU_PROCESS_CLIENT;


                }
                else if (menu == MENU_INPUT_CREDENTIALS)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("Enter custom credentials:");
                    usingCustomCredentials = true;
                    var usernameInput = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[[{Environment.UserName.ToString()}]] Enter username:").AllowEmpty());
                    credentialsUsername = usernameInput != "" ? usernameInput : Environment.UserName.ToString();




                    var passwordInput = AnsiConsole.Prompt(
                    new TextPrompt<string>("[[password]] Enter password:").AllowEmpty().Secret());
                    credentialsPassword = passwordInput != "" ? ToSecureString(passwordInput) : ToSecureString("password");

                    menu = MENU_PROCESS_CLIENT;
                }
                else if (menu == MENU_SELECT_THEME)
                {
                    Console.Clear();
                    if (!currentDir.EndsWith("Themes"))
                    {
                        currentDir += $"{Path.DirectorySeparatorChar}Themes";
                    }


                    var files = Directory.GetFiles(currentDir);
                    var path = new TextPath(currentDir).RootColor(Color.Red)
            .SeparatorColor(new Color(myThemeRgb.BrightBlack.R, myThemeRgb.BrightBlack.G, myThemeRgb.BrightBlack.B))
            .StemColor(new Color(myThemeRgb.BluePath.R, myThemeRgb.BluePath.G, myThemeRgb.BluePath.B))
            .LeafColor(new Color(myThemeRgb.YellowCommandSecond.R, myThemeRgb.YellowCommandSecond.G, myThemeRgb.YellowCommandSecond.B));
                    var panel = new Panel(path);
                    panel.Border = BoxBorder.Square;

                    AnsiConsole.Write(panel);


                    List<string> fileNames = new List<string>();


                    fileNames.Add("..");

                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file).ToLower() == ".yml")
                        {
                            fileNames.Add(Path.GetFileName(file));

                        }
                    }
                    fileNames.Add(SELECTION_BACK);


                    var themePrompt = new SelectionPrompt<string>()
                        .Title("Choose Theme:")
                        .WrapAround(true)
                        .EnableSearch()
                        .PageSize(height)
                        .AddChoices(fileNames);


                    themePrompt.HighlightStyle = new Style(highlightColor);

                    var themeSelection = AnsiConsole.Prompt(themePrompt);


                    if (themeSelection == SELECTION_BACK)
                    {
                        menu = MENU_CONNECT_TO_SERVER_TYPE;
                    }
                    else
                    {


                        string absThemeSelection = currentDir + Path.DirectorySeparatorChar + themeSelection;
                        try
                        {
                            var yaml = File.ReadAllText(absThemeSelection);

                            var deserializer = new DeserializerBuilder()
                                .WithNamingConvention(NullNamingConvention.Instance)
                                .IgnoreUnmatchedProperties()
                                .Build();

                            myTheme = deserializer.Deserialize<TerminalTheme>(yaml);
                            myThemeRgb = new TerminalThemeRGB(myTheme);
                            highlightColor = new Color(myThemeRgb.CyanPrompt.R,
                                        myThemeRgb.CyanPrompt.G,
                                        myThemeRgb.CyanPrompt.B);

                            searchHighlightColor = new Color(myThemeRgb.BrightCyan.R,
                                        myThemeRgb.BrightCyan.G,
                                        myThemeRgb.BrightCyan.B);

                            promptChoicesColor = new Color(myThemeRgb.BrightMagenta.R,
                            myThemeRgb.BrightMagenta.G,
                            myThemeRgb.BrightMagenta.B);

                            promptDefaultChoicesColor = new Color(myThemeRgb.BrightGreenExec.R,
                            myThemeRgb.BrightGreenExec.G,
                            myThemeRgb.BrightGreenExec.B);

                            headerColor = myTheme.BluePath;/* new Color(myThemeRgb.BluePath.R,
                        myThemeRgb.BluePath.G,
                        myThemeRgb.BluePath.B);*/


                            var (r, g, b) = HexToRgb(myTheme.Background);
                            AnsiConsole.Background = new Color((byte)r, (byte)g, (byte)b);
                            // AnsiConsole.Background = Color.White;
                            AnsiConsole.Clear(); // IMPORTANT
                            AnsiConsole.MarkupLine($"[{myTheme.BrightYellow}]Selected {themeSelection}[/]");
                            AnsiConsole.Markup($"[{myTheme.BrightGreenExec}]Press [bold]<Enter>[/] to continue...[/]");

                            Console.ReadLine();
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            menu = MENU_CONNECT_TO_SERVER_TYPE;
                            AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Error[/] {ex.Message}");
                            AnsiConsole.Markup($"[{myTheme.BrightRedCommandError}]Press [bold]<Enter>[/] to continue...[/]");

                            Console.ReadLine();
                        }

                    }

                }
                else if (menu == MENU_PROCESS_CLIENT)
                {

                    using var cts = new CancellationTokenSource();
                    SetCurrentScanCts(cts);

                    if (!askedForCredentials)
                    {
                        bool isRunningOnLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                        askedForCredentials = true;

                        bool reqCustomCredentials = false;





                        if ((isRunningOnLinux && !usingCustomCredentials) || reqCustomCredentials)
                        {
                            menu = MENU_INPUT_CREDENTIALS;

                            continue;
                        }
                    }





                    AnsiConsole.Clear();


                    url = $"http://{serverData.getAddress()}:{serverData.getPort()}/api/pcs/oldest";


                    try
                    {
                        AnsiConsole.MarkupLine("Processing client side:");
                        //AnsiConsole.MarkupLine("[yellow]Waiting for server response[/]");


                        using var client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Client-Hostname", Dns.GetHostName().ToString());



                        List<IP> addresses = client.GetFromJsonAsync<List<IP>>(url).GetAwaiter().GetResult();


                        if (addresses is null)
                        {
                            AnsiConsole.MarkupLine($"No data acquired from server");
                        }
                        else
                        {
                            addresses.Sort((a, b) => a.address.CompareTo(b.address));

                            foreach (var a in addresses)
                            {
                                a.isOperated = false;
                            }


                            // List into which put results of scanning
                            List<ipResponse> ipResponses = new List<ipResponse>();





                            var table = new Spectre.Console.Table();

                            Task displayTask = null;
                            Task responseTask = null;
                            Task scanTask = null;
                            //Printing progress
                            try
                            {
                                displayTask = AnsiConsole.Live(table)
                                   .AutoClear(false)
                                   .StartAsync(async ctx =>
                                   {

                                       //startedDisplaying = true;
                                       var refresh = TimeSpan.FromMilliseconds(100);
                                       int frame = 0;

                                       try
                                       {


                                           while (!cts.IsCancellationRequested)
                                           //while (!cts.IsCancellationRequested && ipResponses.Count() != addresses.Count())
                                           {

                                               cts.Token.ThrowIfCancellationRequested();

                                               if (responseTask != null && (responseTask.IsCompleted || responseTask.IsCanceled || responseTask.Status == TaskStatus.RanToCompletion))
                                               {
                                                   break;
                                               }
                                               //AnsiConsole.Clear();
                                               //ctx.UpdateTarget(BuildTable(ipResponses, addresses, frame, processedAddresses));

                                               List<ipResponse> responsesSnapshot;
                                               List<IP> addressesSnapshot;

                                               lock (_lock)
                                               {
                                                   responsesSnapshot = ipResponses.ToList();
                                                   addressesSnapshot = addresses.ToList();
                                               }

                                               var currentTable = BuildTable(responsesSnapshot, addressesSnapshot, frame, processedAddresses);

                                               // Update BOTH table and progress bar together
                                               ctx.UpdateTarget(
                                                   BuildView(
                                                       currentTable,
                                                       responsesSnapshot.Count,
                                                       addressesSnapshot.Count
                                                   // $"Scanning... frame={ipResponses.Count() / addresses.Count()}"
                                                   )
                                               );


                                               frame++;
                                               if (frame >= FRAMES)
                                               {
                                                   frame = 0;
                                               }



                                               await Task.Delay(refresh, cts.Token);//.ContinueWith(_ => { return; });
                                           }
                                       }
                                       catch (OperationCanceledException)
                                       {
                                           AnsiConsole.Clear();
                                           menu = MENU_CONNECT_TO_SERVER_TYPE;
                                           AnsiConsole.MarkupLine("[yellow]Scan stopped[/]");
                                           AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");
                                       }
                                       catch (Exception ex)
                                       {
                                           // This is the exception that causes "Faulted"
                                           AnsiConsole.WriteException(ex);
                                           throw;
                                       }
                                       //throw new OperationCanceledException("Cancel");
                                   });

                            }
                            catch (OperationCanceledException)
                            {
                                AnsiConsole.Clear();
                                menu = MENU_CONNECT_TO_SERVER_TYPE;
                                AnsiConsole.MarkupLine("[yellow]Scan stopped[/]");
                                AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");

                                try
                                {
                                    //shortcutTask.Dispose();
                                }
                                catch (System.InvalidOperationException)
                                {

                                }
                                continue;
                            }

                            ParallelOptions options = new ParallelOptions
                            {
                                CancellationToken = cts.Token//, MaxDegreeOfParallelism = 1
                            };

                            //foreach (var ip in addresses)
                            try
                            {

                                scanTask = Parallel.ForEachAsync(

                                    addresses, options,
                                    async (ip, ct) =>
                                    {

                                        //Throwin excepttion if Cancelation Requested
                                        cts.Token.ThrowIfCancellationRequested();




                                        ip.isOperated = true;



                                        Ping pingSender = new Ping();

                                        int pingCounter = 0;
                                        string data = "aa";
                                        byte[] buffer = Encoding.ASCII.GetBytes(data);

                                        ipResponse response = new ipResponse(ip.address.ToString(), DateTime.Now);
                                        bool pingSuccess = false;

                                        IPAddress pingAddress = IPAddress.Parse(ip.address.ToString());

                                        while (pingCounter < MAX_PING_COUNTER)
                                        {

                                            //Throwin excepttion if Cancelation Requested (should be in multiple places)
                                            cts.Token.ThrowIfCancellationRequested();

                                            try
                                            {
                                                PingReply reply = pingSender.Send(pingAddress, PING_TIMEOUT);
                                                if (reply.Status == IPStatus.Success)
                                                {
                                                    response.successFinding = true;
                                                    response.lastFoundDate = DateTime.Now;
                                                    IPHostEntry hostname = null;
                                                    try
                                                    {
                                                        //string hostname=Dns.GetHostEntry(pingAddress)?.ToString() ?? "";
                                                        hostname = Dns.GetHostEntry(pingAddress);
                                                        IPHostEntry host = Dns.GetHostEntry(ip.address.ToString());

                                                        response.hostname = host.HostName;

                                                    }
                                                    catch (SocketException ex)
                                                    {
                                                        response.hostname = "";

                                                    }
                                                    catch (OperationCanceledException)
                                                    {
                                                        AnsiConsole.Clear();
                                                        menu = MENU_CONNECT_TO_SERVER_TYPE;
                                                        AnsiConsole.MarkupLine("[yellow]Scan stopped[/]");
                                                        AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");
                                                        //Console.ReadLine();
                                                        continue;
                                                    }


                                                    if (response.hostname != "")
                                                    {

                                                        var aux = GetUserAndModel(ref menu, usingCustomCredentials, credentialsUsername, credentialsPassword.ToString(), response.hostname);

                                                        response.lastLoggedUser = aux.user;
                                                        response.model = aux.model;


                                                        response.operatingSystem = getOSVersion(response.hostname, usingCustomCredentials, credentialsUsername, credentialsPassword);

                                                        response.serialNumber = getSN(response.hostname, usingCustomCredentials, credentialsUsername, credentialsPassword);

                                                        response.procGen = getProcGen(response.hostname);

                                                        if (response.lastLoggedUser == "-" && response.model == "-" && response.operatingSystem == "-" && response.serialNumber == "-")
                                                        {
                                                            response.model = GetPrinterModel(ip.address.ToString());
                                                            response.serialNumber = GetPrinterSN(ip.address.ToString());

                                                        }
                                                    }
                                                    else
                                                    {
                                                        response.lastLoggedUser = "-";

                                                        response.model = GetPrinterModel(ip.address.ToString());
                                                        response.serialNumber = GetPrinterSN(ip.address.ToString());

                                                        response.operatingSystem = "-";
                                                        response.serialNumber = "-";

                                                    }


                                                    pingSuccess = true;
                                                    response.successFinding = true;
                                                    break;
                                                }
                                            }
                                            catch (PingException ex)
                                            {
                                                response.operatingSystem = "-";

                                                response.serialNumber = "-";
                                            }
                                            catch (OperationCanceledException)
                                            {
                                                AnsiConsole.Clear();
                                                menu = MENU_CONNECT_TO_SERVER_TYPE;
                                                AnsiConsole.MarkupLine("[yellow]Scan stopped[/]");
                                                AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");

                                                continue;
                                            }

                                            pingCounter++;




                                        }

                                        if (pingSuccess == false)
                                        {
                                            response.successFinding = false;
                                        }


                                        //ipResponses.Add(response);

                                        lock (_lock)
                                        {
                                            ipResponses.Add(response);
                                        }


                                        ip.isOperated = false;


                                        //processedAddresses++;
                                        Interlocked.Increment(ref processedAddresses);


                                    }
                                );//for parallel


                                //scanTask.Wait();

                                await scanTask;

                                //sending response to server
                                responseTask = sendResponseToServer(serverData.getAddress(), serverData.getPort(), client, ipResponses);
                                await responseTask;
                                await displayTask;
                                AnsiConsole.Clear();




                                //Console.Clear();
                                //AnsiConsole.Write(BuildTable(ipResponses, addresses, 0, processedAddresses));

                                if (cts.IsCancellationRequested)
                                {
                                    AnsiConsole.Clear();
                                    menu = MENU_CONNECT_TO_SERVER_TYPE;
                                    AnsiConsole.MarkupLine("[yellow]Scan stopped[/]");
                                    AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");

                                    break;
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                AnsiConsole.Clear();
                                menu = MENU_CONNECT_TO_SERVER_TYPE;
                                AnsiConsole.MarkupLine("[yellow]Scan stopped[/]");
                                AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");

                                continue;
                            }


                        }


                    }
                    catch (UriFormatException ex)
                    {
                        AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Unable to connect to server with url: [/] {url}");
                        AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/][/]");
                        Console.ReadLine();
                        menu = MENU_CONNECT_TO_SERVER_TYPE;
                    }
                    catch (HttpRequestException ex)
                    {
                        AnsiConsole.Clear();
                        Thread.Sleep(2000);

                        if (httpRequestCounter > MAX_HTTP_REQUEST_RETRY)
                        {
                            httpRequestCounter = 0;
                            AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Error[/] {ex.GetType()} {ex.Message}");
                            Thread.Sleep(1000);
                            AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/][/]");
                            Console.ReadLine();


                            menu = MENU_CONNECT_TO_SERVER_TYPE;
                        }
                        httpRequestCounter++;

                    }
                    catch (System.AggregateException)
                    {

                        menu = MENU_CONNECT_TO_SERVER_TYPE;
                    }
                    catch (Exception ex)
                    {
                        cts.Cancel();

                    }
                    finally
                    {

                        SetCurrentScanCts(null);
                    }


                }
            }
        }

        private static readonly object _lock = new();


        private static UserModel GetUserAndModel(ref int menu, bool usingCustomCredentials, string credentialsUsername, string credentialsPassword, string hostname)
        {
            UserModel mySystem = new UserModel();
            try// getting current logged user on remote host
            {
                // Works only for client running windows

                var options = usingCustomCredentials ? new ConnectionOptions
                {
                    Username = credentialsUsername,
                    Password = credentialsPassword,
                    Timeout = new System.TimeSpan(0, 0, 30)
                } : new ConnectionOptions { Timeout = new System.TimeSpan(0, 0, 30) };


                var scope = new ManagementScope($@"\\{hostname}\root\cimv2", options);
                scope.Connect();

                var query = new ObjectQuery("SELECT UserName,Model FROM Win32_ComputerSystem");
                using var searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject mo in collection)
                {

                    var aux = mo["UserName"];
                    mySystem.user = aux?.ToString() ?? "-";
                    aux = mo["Model"];
                    mySystem.model = aux?.ToString() ?? "-";
                }




            }
            catch (UnauthorizedAccessException ex)
            {
                // Wrong credentials or insufficient permissions.
                //AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Insufficient[/] permissions to get current logged user on [yellow]{hostname}[/]");
                //AnsiConsole.Markup($"[{myTheme.BrightBlack}]Press [bold]<Enter>[/] to continue...[/]");
                //Console.ReadLine();
                //AnsiConsole.Clear();
                //menu = MENU_CONNECT_TO_SERVER_TYPE;

            }
            catch (Exception ex)
            {
                return mySystem;
            }
            return mySystem;
        }

        private static int getProcGen(string hostname = "", bool usingCustomCredentials = false, string credentialsUsername = "", SecureString credentialsPassword = null)
        {

            string foundProcGen = "", foundProcName = "";
            try//getting windows version of remote machine
            {

                // Works only for client running windows
                var options = usingCustomCredentials ? new ConnectionOptions
                {
                    Username = credentialsUsername,
                    Password = credentialsPassword.ToString(),
                    Timeout = new TimeSpan(0, 0, 30)
                } : new ConnectionOptions { Timeout = new System.TimeSpan(0, 0, 30) };//waiting 30 seconds

                var scope = new ManagementScope($@"\\{hostname}\root\cimv2", options);


                scope.Connect();




                var query = new ObjectQuery("SELECT Name FROM Win32_Processor");
                using var searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection collection = searcher.Get();


                foreach (ManagementObject m in collection)
                {
                    foundProcName = m["Name"].ToString();
                    for (int i = 0; i < foundProcName.Length; i++)
                    {
                        if (foundProcName[i] == '-' && i < foundProcName.Length - 1)
                        {
                            foundProcGen = foundProcName[i + 1].ToString();
                            if (foundProcName[i + 1] == '1' && i < foundProcName.Length - 2) { foundProcGen += foundProcName[i + 2].ToString(); }
                            break;
                        }
                    }
                }


            }
            catch (Exception)
            {
                return 0;
            }
            try
            {
                return Int32.Parse(foundProcGen);
            }
            catch
            {
                return 0;
            }

        }

        private static string getSN(string hostname = "", bool usingCustomCredentials = false, string credentialsUsername = "", SecureString credentialsPassword = null)
        {
            try//getting windows version of remote machine
            {

                // Works only for client running windows
                var options = usingCustomCredentials ? new ConnectionOptions
                {
                    Username = credentialsUsername,
                    Password = credentialsPassword.ToString(),
                    Timeout = new TimeSpan(0, 0, 30)
                } : new ConnectionOptions { Timeout = new System.TimeSpan(0, 0, 30) };//waiting 30 seconds

                var scope = new ManagementScope($@"\\{hostname}\root\cimv2", options);


                scope.Connect();


                var query = new ObjectQuery(
                    "SELECT  SerialNumber FROM Win32_BIOS");

                using var searcher = new ManagementObjectSearcher(scope, query);
                searcher.Options.Timeout = new TimeSpan(0, 0, 30);//10 sec
                using var results = searcher.Get();

                foreach (ManagementObject os in results)
                {
                    var sn = (string?)os["SerialNumber"];

                    return sn;
                }


            }
            catch (Exception)
            {
                return "-";
            }

            return "-";
        }

        private static string getOSVersion(string hostname, bool usingCustomCredentials = false, string credentialsUsername = "", SecureString credentialsPassword = null)
        {

            try//getting windows version of remote machine
            {


                // Works only for client running windows
                var options = usingCustomCredentials ? new ConnectionOptions
                {
                    Username = credentialsUsername,
                    Password = credentialsPassword.ToString(),
                    Timeout = new TimeSpan(0, 0, 30)
                } : new ConnectionOptions { Timeout = new System.TimeSpan(0, 0, 30) };//waiting 30 seconds

                var scope = new ManagementScope($@"\\{hostname}\root\cimv2", options);
                scope.Connect();

                // Win32_OperatingSystem instead of Win32_ComputerSystem
                var query = new ObjectQuery(
                    "SELECT Caption, Version, BuildNumber FROM Win32_OperatingSystem");

                using var searcher = new ManagementObjectSearcher(scope, query);
                using var results = searcher.Get();

                foreach (ManagementObject os in results)
                {
                    var caption = (string?)os["Caption"];
                    var version = (string?)os["Version"];
                    var buildStr = (string?)os["BuildNumber"];

                    int.TryParse(buildStr, out var build);

                    // Windows 11 starts at build 22000
                    var isWindows11 = build >= 22000;

                    var operatingSystem = isWindows11 ? "Windows 11" : "Windows 10";
                    return operatingSystem;
                }



            }
            catch (Exception)
            {
                return "-";
            }

            return "-";
        }

        private static string GetPrinterSN(string ip = "127.0.0.1", SnmpVersion version = SnmpVersion.Ver1)
        {
            // SNMP community string (default = "public")
            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = version;

            // The printer IP
            IPAddress address = IPAddress.Parse(ip);

            UdpTarget target = new UdpTarget(address, 161, 2000, 1);

            // Standard sysDescr OID
            //Oid oid = new Oid("1.3.6.1.2.1.1.1.0");
            SnmpSharpNet.Oid oidSerialNumber = new SnmpSharpNet.Oid("1.3.6.1.2.1.43.5.1.1.17.1");

            Pdu pdu = new Pdu(PduType.Get);
            pdu.VbList.Add(oidSerialNumber);
            try
            {
                //SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                SnmpPacket result = (SnmpPacket)target.Request(pdu, param);

                if (result != null && result.Pdu.ErrorStatus == 0)
                {
                    return result.Pdu.VbList[0].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                return "-";
            }

            return "-";

        }

        private static string GetPrinterModel(string ip = "127.0.0.1", SnmpVersion version = SnmpVersion.Ver1)
        {

            // SNMP community string (default = "public")
            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = version;

            // The printer IP
            IPAddress address = IPAddress.Parse(ip);

            UdpTarget target = new UdpTarget(address, 161, 2000, 1);

            // Standard sysDescr OID
            //Oid oid = new Oid("1.3.6.1.2.1.1.1.0");
            SnmpSharpNet.Oid oidModel = new SnmpSharpNet.Oid("1.3.6.1.2.1.43.5.1.1.16.1");


            Pdu pdu = new Pdu(PduType.Get);
            pdu.VbList.Add(oidModel);
            try
            {
                //SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                SnmpPacket result = (SnmpPacket)target.Request(pdu, param);

                if (result != null && result.Pdu.ErrorStatus == 0)
                {
                    return result.Pdu.VbList[0].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                return "-";
            }


            return "-";
        }

        static int GetRenderableWidth(IRenderable renderable)
        {
            var profile = AnsiConsole.Console.Profile;

            // Use current console width if available, otherwise fall back
            var maxWidth = profile.Width > 0 ? profile.Width : 80;

            // RenderOptions needs capabilities + console size
            var options = new RenderOptions(
                profile.Capabilities,
                new Size(maxWidth, profile.Height > 0 ? profile.Height : 25)
            );

            // Measurement.Max is the preferred/maximum width for the renderable
            var measurement = renderable.Measure(options, maxWidth);
            return measurement.Max;
        }


        static IRenderable BuildView(
            Table table,
            int processed,
            int total
            //string status
            )
        {

            int tableWidth = GetRenderableWidth(table);


            var pct = total <= 0 ? 0 : (double)processed / total * 100.0;

            var bar = new BarChart()
                .Width(tableWidth)
                .WithMaxValue(100)
                .AddItem("Progress", pct, color: null)   // color is optional (null)
                .ShowValues()
                .UseValueFormatter(v => $"{v:0}% ({processed}/{total})");

            return new Rows(
                table,
                bar
            //new Rule(),
            //new Markup($"[{myTheme.BrightBlack}]{status}[/]")
            );
        }


        static Spectre.Console.Table BuildTable(List<ipResponse> ipResponses = null, List<IP> addresses = null, int counter = 0, int processedAddresses = 0)
        {
            addresses = addresses ?? new List<IP>();
            ipResponses = ipResponses ?? new List<ipResponse>();

            var tab = new Spectre.Console.Table();
            tab.Title($"[bold]Live Ping[/]  (Ctrl + 'Q' to stop) - Processed {processedAddresses} addresses");
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] IpAddress [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Hostname [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Last logged user [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Last checked date [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Operating system [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Proc. Gen. [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Last found Date [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] Model [/]")));
            tab.AddColumn(new TableColumn(new Markup($"[{headerColor}] SerialNumber [/]")));
            int responsesAmount = ipResponses.Count();
            if (responsesAmount > 0)
            {
                foreach (var re in ipResponses)
                {
                    var addr = new Markup($"{re.address.ToString()}");
                    var hostname = new Markup($"{re.hostname.ToString()}");
                    var lastLoggedUser = new Markup($"{re.lastLoggedUser.ToString()}");
                    if (!re.successFinding)
                    {

                        tab.AddRow($"[{myTheme.BrightRedCommandError}]{re.address.ToString()}[/]", $"[{myTheme.BrightRedCommandError}] - not found -[/]", $"[{myTheme.BrightRedCommandError}] - not found -[/]", re.lastCheckedDate.ToString("dd/MM/yyyy HH:mm:ss"), "-", re.procGen.ToString(), "-", "-", "-");

                    }
                    else
                    {
                        try
                        {

                            string tabAddress = re.address?.ToString() ?? "-";
                            string tabHostname = re.hostname?.ToString() ?? "-";
                            string tabLastLoggedUser = re.lastLoggedUser?.ToString() ?? "-";
                            //string tabLastCheckedDate = re.lastCheckedDate?.ToString() ?? "-";
                            string tabOperatingSystem = re.operatingSystem?.ToString() ?? "-";
                            //string tabLastFoundDate = re.lastFoundDate?.ToString() ?? "-";
                            string tabModel = re.model?.ToString() ?? "-";
                            string tabSN = re.serialNumber?.ToString() ?? "-";
                            string tabProcGen = re.procGen.ToString();
                            tab.AddRow($"[{myTheme.GreenCommand}]{tabAddress}[/]", tabHostname, tabLastLoggedUser, $"[{myTheme.GreenCommand}]{re.lastCheckedDate.ToString("dd/MM/yyyy HH:mm:ss")}[/]",
                            $"[{myTheme.GreenCommand}]{tabOperatingSystem}[/]", $"[{myTheme.GreenCommand}]{tabProcGen}[/]", $"[{myTheme.GreenCommand}]{re.lastFoundDate.ToString()}[/]", $"[{myTheme.GreenCommand}]{tabModel}[/]", $"[{myTheme.GreenCommand}]{tabSN}[/]");


                        }
                        catch (Exception ex)
                        {
                            tab.AddEmptyRow();
                        }

                    }
                }

                if (addresses.Count() > 0)
                {
                    foreach (var re in addresses)
                    {
                        if (!(ipResponses.Any(x => x.address == re.address)))
                        {
                            if (re.isOperated)
                            {
                                List<string> myFrames = new List<string> { "-", "/", "|", "\\" };
                                var auxAddress = re.address?.ToString() ?? "-";
                                var auxCheckedDate = re.lastCheckedDate?.ToString() ?? "-";
                                tab.AddRow(auxAddress, myFrames[counter], "---", auxCheckedDate.Replace('/', '.'), "---", "---", "---", "---");
                            }
                            else
                            {
                                tab.AddRow(re.address.ToString(), "---", "---", re.lastCheckedDate.ToString().Replace('/', '.') ?? "-", "---", "---", "---", "---");
                            }

                        }

                    }
                }

                tab.Collapse();

            }
            return tab;
        }

        private static SecureString ToSecureString(string s = "")
        {
            var ss = new SecureString();
            foreach (var c in s) ss.AppendChar(c);
            ss.MakeReadOnly();
            return ss;
        }


        private static async Task sendResponseToServer(string serverIp, string serverPort, HttpClient client, List<ipResponse> ipResponses)
        {
            //Console.Clear();
            //Console.WriteLine();
            //Console.WriteLine("Sending response to server");

            string url = $"http://{serverIp}:{serverPort}/api/pcs/response";
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync(url, ipResponses);

            if (httpResponse.IsSuccessStatusCode)
            {
                //Console.WriteLine("✅ Update successful!");
                string result = await httpResponse.Content.ReadAsStringAsync();
                //Console.WriteLine($"Response: {result}");
            }
            else
            {
                //Console.WriteLine($"❌ Error: {httpResponse.StatusCode}");
            }
            await Task.Delay(2000);
            //Console.WriteLine("Finished sending");
        }

        private static string fileSelection(string currentDir, int height)
        {
            var files = Directory.GetFiles(currentDir);
            var directories = Directory.GetDirectories(currentDir);


            var rootColor = new Color(myThemeRgb.BrightRedCommandError.R,
                myThemeRgb.BrightRedCommandError.G,
                myThemeRgb.BrightRedCommandError.B);


            var separatorColor = new Color(myThemeRgb.BrightRedCommandError.R,
                myThemeRgb.BrightRedCommandError.G,
                myThemeRgb.BrightRedCommandError.B);
            ;


            var stemColor = new Color(myThemeRgb.MagentaSyntaxVar.R,
                myThemeRgb.MagentaSyntaxVar.G,
                myThemeRgb.MagentaSyntaxVar.B);

            var leafColor = new Color(myThemeRgb.BrightGreenExec.R,
                myThemeRgb.BrightGreenExec.G,
                myThemeRgb.BrightGreenExec.B);

            var path = new TextPath(currentDir).RootColor(rootColor)
            .SeparatorColor(separatorColor)
            .StemColor(stemColor)
            .LeafColor(leafColor);

            var panel = new Panel(path);
            panel.Border = BoxBorder.Square;

            AnsiConsole.Write(panel);


            List<string> fileNames = new List<string>();

            fileNames.Add("..");

            foreach (var file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".json")
                {
                    fileNames.Add(Path.GetFileName(file));

                }
            }

            foreach (var directory in directories)
            {
                fileNames.Add(Path.GetFileName(directory));
            }


            fileNames.Add(SELECTION_BACK);





            var selectionPrompt = new SelectionPrompt<string>()
            .Title("Choose file:")
            .PageSize(height)
            .WrapAround(true)
            .AddChoices(fileNames);
            selectionPrompt.HighlightStyle = new Style(highlightColor);
            selectionPrompt.SearchHighlightStyle = new Style(searchHighlightColor);

            var fileSelection = AnsiConsole.Prompt(selectionPrompt);

            return fileSelection;
        }


        static async Task ListenForShortcutAsync(CancellationTokenSource _cts)
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (!Console.KeyAvailable)
                {
                    await Task.Delay(50, _cts.Token);
                    continue;
                }

                var key = Console.ReadKey(intercept: true);

                if (key.Modifiers.HasFlag(ConsoleModifiers.Control) &&
                    key.Key == ConsoleKey.Q)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine($"[{myTheme.BrightRedCommandError}]Ctrl+Q[/] detected...");
                    CancelCurrentScan();
                }
            }

            // cooperative cancellation
            //_cts.Token.ThrowIfCancellationRequested();
        }

        static (int R, int G, int B) HexToRgb(string hex)
        {
            hex = hex.TrimStart('#');

            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }




    }
}