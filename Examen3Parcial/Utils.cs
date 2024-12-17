using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen3Parcial
{
    class Utils
    {
        public async Task<string> SaveAudioAsync(Byte[] audio)
        {
            try
            {
                string appDataDirectory = FileSystem.AppDataDirectory;
                string filePath = Path.Combine(appDataDirectory, "audio.mp3");
                await File.WriteAllBytesAsync(filePath, audio);
                Console.WriteLine($"Audio guardado en: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar audio {ex.Message}");
                return null;
            }
        }
    }
}
