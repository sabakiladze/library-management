using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FileRepository<T> : IFileRepository<T>
    {
        private readonly string _filePath;

        public FileRepository(string fileName)
        {
            string directory = @"C:\Users\kilad\Downloads\library-management-fixed-v2\library-management\Infrastructure\Files";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _filePath = Path.Combine(directory, fileName);
        }

        public async Task<List<T>> GetAllLineAsync()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            try
            {
                string text = await File.ReadAllTextAsync(_filePath);

                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };

                return JsonSerializer.Deserialize<List<T>>(text, options)
                       ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[warning] Could not read {Path.GetFileName(_filePath)}: {ex.Message}");
                return new List<T>();
            }
        }

        public async Task SaveAllAsync(List<T> data)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                string json = JsonSerializer.Serialize(data, options);

                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save {Path.GetFileName(_filePath)}: {ex.Message}", ex);
            }
        }
    }
}
