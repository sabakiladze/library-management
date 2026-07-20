using Application.Interfaces.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

public class FileRepository<T> : IFileRepository<T>
{
    private readonly string _filePath;

    public FileRepository(string fileName)
    {
        string directory = @"C:\Users\kilad\Downloads\library-management-fixed-v2\library-management\Infrastructure\Files";

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _filePath = Path.Combine(directory, fileName);
    }


    public List<T> GetAllLine()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        try
        {
            string text = File.ReadAllText(_filePath);

            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };


            return JsonSerializer.Deserialize<List<T>>(text, options)
                   ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<T>();
        }
    }


    public void SaveAll(List<T> data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };


        string json = JsonSerializer.Serialize(data, options);

        File.WriteAllText(_filePath, json);
    }
}