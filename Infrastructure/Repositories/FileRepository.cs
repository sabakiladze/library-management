using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class FileRepository<T> : IFileRepository<T>
    {
        private readonly string _filePath;
        /// ამას კარგად გადავავლო მერე თვალი
        public FileRepository(string fileName)
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _filePath = Path.Combine(directory, fileName);
        }

        public List<T> GetAllLine()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                string text = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(text) ?? new List<T>();
            }
            catch (JsonException)
            {
                return new List<T>();
            }
        }

        public void SaveAll(List<T> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);

            File.WriteAllText(_filePath, json);
        }////ეს თავიდან უნდა გავიარო
    }
}

