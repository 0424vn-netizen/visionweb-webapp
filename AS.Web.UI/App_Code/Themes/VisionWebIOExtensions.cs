using System.IO;
using System.Text;

/// <summary>
/// Summary description for IOExtensions
/// </summary>
public class VisionWebIOExtensions
{
    public static void CopyDirectory(string sourcePath, string destinationPath)
    {
        if (!destinationPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            destinationPath += Path.DirectorySeparatorChar;
        }
        if (!Directory.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationPath);
        }
        string[] filesAndSubDirs = Directory.GetFileSystemEntries(sourcePath);
        foreach (string item in filesAndSubDirs)
        {
            // Sub directories
            if (Directory.Exists(item))
            {
                CopyDirectory(item, destinationPath + Path.GetFileName(item));
            }
            // Files in directory
            else
            {
                string destinationFilePath = destinationPath + Path.GetFileName(item);
                //Remove readonly before save 
                if (File.Exists(destinationFilePath))
                {
                    File.SetAttributes(destinationFilePath, FileAttributes.Normal);
                }
                File.Copy(item, destinationFilePath, true);
                File.SetAttributes(destinationFilePath, FileAttributes.Normal);
            }
        }
    }
    public static StringBuilder ReadTextFile(string fullPath)
    {
        StringBuilder builder = new StringBuilder(File.ReadAllText(fullPath));
        return builder;
    }
    public static void WriteTextFile(string destFilePath, StringBuilder text)
    {
        if (File.Exists(destFilePath))
        {
            File.SetAttributes(destFilePath, FileAttributes.Normal);
        }
        File.WriteAllText(destFilePath, text.ToString());
    }
}
