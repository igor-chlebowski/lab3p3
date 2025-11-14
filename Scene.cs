using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Duck;

public class Scene
{
    public Duck Player { get; set; } = new("") { Behaviour = new Duck.PlayerControlled() };
    public List<Duck> Ducks { get; set; } = [];
    public DateTime Time { get; set; } = DateTime.Now;

    public Scene()
    {
    }

    public Scene(string path)
    {
        string dir = @"C:\Prog3\Lab10";
        // TODO: Stage 1
        // Load the embedded file given as a path and parse it.
        // Add parsed ducks to the Ducks list.
        //using Stream duckStream = File.OpenRead("/Resources/ducks.csv");
        var sciezka = path.Split('.');
        int idx = path.LastIndexOf('.');
        StringBuilder sb = new StringBuilder();
      
        foreach (string s in sciezka)
        {
            sb.Append(s);
            sb.Append('\\');
        }
        sb[idx] = '.';
        sb.Remove(sb.Length-1, 1);
        var filePath = sb.ToString();
        Directory.SetCurrentDirectory(dir);
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var duckData = line.Split(';');
            var duckPosCSV = duckData[1].Split(',');
            Vector3 duckPos = new Vector3(Convert.ToSingle(duckPosCSV[0]), 0.0f, Convert.ToSingle(duckPosCSV[1]));
            string name = duckData[0];
            var duckRotation = Convert.ToSingle(duckData[2]);
            var duckScale = Convert.ToSingle(duckData[3]);
            Console.WriteLine(name + " " + duckPos + " " + duckRotation + " " + duckScale);
            Ducks.Add(new Duck(name, duckPos, duckRotation, duckScale));

        }
    }

    public IEnumerable<Duck> GetAllDucks()
    {
        yield return Player;
        foreach (var duck in Ducks)
        {
            yield return duck;
        }
    }

    public void Update(float dt, KeyboardState keyboard, MouseState mouse)
    {
        Time += TimeSpan.FromMinutes(dt);
        Player.Update(dt, keyboard, mouse);
        foreach (var duck in Ducks)
        {
            duck.Update(dt, keyboard, mouse);
        }
    }

    public void QuackSave()
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        // register converters for OpenTK vectors so the serializer doesn't walk computed properties
        //options.Converters.Add(new OpenTkVector3JsonConverter());
        //options.Converters.Add(new OpenTkVector2JsonConverter());

        string jsonString = JsonSerializer.Serialize(this, options);

        using FileStream fs = new("Scene_save.json", FileMode.Create);
        using GZipStream gzipStream = new(fs, CompressionMode.Compress);
        using var writer = new StreamWriter(gzipStream, Encoding.UTF8);
        writer.Write(jsonString);
    }

    public Scene? QuackLoad()
    {
        // TODO: Stage 3
        // Implement quack-load functionality
        return null;
    }
}