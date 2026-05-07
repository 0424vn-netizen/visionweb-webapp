using AS.Core.Common.Log;
using AS.VW.Scheduler.Cybersource.Auth.Client;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace AS.VW.Scheduler.Cybersource.Auth.Business
{
    public class KeyData
    {
        public string Password { get; set; }

        public byte[] Salt { get; set; }
    }

    public class FileEncryption
    {
        private readonly string _keyFilePath;
        private readonly bool _deleteOriginalFile;
        private readonly bool _deleteZipFile;
        private readonly KeyProtection _keyProtection;
        private readonly ILog LogManager = Logger.GetLogger(typeof(CybersourceClient));

        public FileEncryption(string keyFilePath = null, bool deleteOriginalFile = true, bool deleteZipFile = true)
        {
            _keyFilePath = keyFilePath ?? @"E:\Program Files\DDS Apps\FileTools\EncryptFile\Encrypt.xml";
            _deleteOriginalFile = deleteOriginalFile;
            _deleteZipFile = deleteZipFile;
            _keyProtection = new KeyProtection();
        }

        public string EncryptFile(string inputFilePath, string outputFilePath = null, bool needZip = true)
        {
            if (!File.Exists(inputFilePath))
            {
                LogManager.Error("File does not exist: " + inputFilePath);
                return null;
            }

            if (inputFilePath.Contains(".encrypted"))
            {
                LogManager.Warn("File already encrypted, skipping: " + inputFilePath);
                return inputFilePath;
            }

            try
            {
                string fileToEncrypt = inputFilePath;
                string encryptedFilePath;

                if (needZip)
                {
                    string zipFilePath = ZipFileProperly(inputFilePath);
                    if (zipFilePath == null)
                    {
                        LogManager.Error("Error zipping file, skipping: " + inputFilePath);
                        return null;
                    }
                    fileToEncrypt = zipFilePath;
                    encryptedFilePath = outputFilePath ?? (inputFilePath + ".zip.encrypted");
                }
                else
                {
                    encryptedFilePath = outputFilePath ?? (inputFilePath + ".encrypted");
                }

                EncryptFileCore(fileToEncrypt, encryptedFilePath);

                if (_deleteZipFile && needZip && File.Exists(fileToEncrypt) && fileToEncrypt != inputFilePath)
                {
                    File.Delete(fileToEncrypt);
                }

                if (_deleteOriginalFile && File.Exists(inputFilePath))
                {
                    File.Delete(inputFilePath);
                }

                LogManager.Info("Encrypted successfully: " + encryptedFilePath);
                return encryptedFilePath;
            }
            catch (Exception ex)
            {
                LogManager.Error("Error encrypting file: " + ex.Message);
                return null;
            }
        }

        private void EncryptFileCore(string fileIn, string fileOut)
        {
            using (FileStream inputStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read))
            using (SymmetricAlgorithm algorithm = GenerateEncryptionAlgorithm())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
            {
                using (FileStream outputStream = new FileStream(fileOut, FileMode.CreateNew, FileAccess.Write))
                {
                    byte[] header = GenerateCryptoClaimInfo(algorithm);
                    outputStream.Write(header, 0, header.Length);
                }

                try
                {
                    using (FileStream outputStream = new FileStream(fileOut, FileMode.Append, FileAccess.Write))
                    using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                    {
                        int bufferSize = 4096;
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead;

                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, bufferSize);
                            if (bytesRead > 0)
                            {
                                cryptoStream.Write(buffer, 0, bytesRead);
                            }
                        }
                        while (bytesRead != 0);

                        cryptoStream.FlushFinalBlock();
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(fileOut))
                    {
                        try { File.Delete(fileOut); } catch { }
                    }
                    LogManager.Error($"Error during encryption: {ex.Message}");
                    throw;
                }
            }
        }

        private SymmetricAlgorithm GenerateEncryptionAlgorithm()
        {
            if (!string.IsNullOrEmpty(_keyFilePath) && File.Exists(_keyFilePath))
            {
                var keys = ReadKeysFromXML(_keyFilePath);
                if (keys != null && keys.Count > 0)
                {
                    var firstKey = keys[0];
                    LogManager.Info($"Using key from XML");

                    using (PasswordDeriveBytes pdb = new PasswordDeriveBytes(firstKey.Password, firstKey.Salt))
                    {
                        Rijndael rijndael = Rijndael.Create();
                        rijndael.Key = pdb.GetBytes(32);
                        rijndael.IV = pdb.GetBytes(16);
                        rijndael.Padding = PaddingMode.PKCS7;
                        rijndael.Mode = CipherMode.CBC;
                        return rijndael;
                    }
                }
            }

            LogManager.Info("Using default key");
            return GenerateEncryptionAlgorithmDefault();
        }

        private SymmetricAlgorithm GenerateEncryptionAlgorithmDefault()
        {
            string passwordBase64 = "g/X/+kWM4fpGcDvfYFUuOw==";
            string saltBase64 = "XJlPnR9v7oeZnqZ1nHN1Pg==";

            byte[] passwordBytes = Convert.FromBase64String(passwordBase64);
            byte[] saltBytes = Convert.FromBase64String(saltBase64);
            string password = Encoding.ASCII.GetString(passwordBytes);

            using (PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, saltBytes))
            {
                Rijndael rijndael = Rijndael.Create();
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                rijndael.Padding = PaddingMode.PKCS7;
                return rijndael;
            }
        }

        private List<KeyData> ReadKeysFromXML(string xmlFilePath)
        {
            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    return null;
                }

                XDocument doc = XDocument.Load(xmlFilePath);
                var keys = new List<KeyData>();

                foreach (var keyElement in doc.Descendants("key"))
                {
                    string encryptedValue = keyElement.Attribute("value")?.Value;
                    string encryptedIv = keyElement.Attribute("iv")?.Value;

                    if (string.IsNullOrEmpty(encryptedValue) || string.IsNullOrEmpty(encryptedIv))
                    {
                        continue;
                    }

                    string password = _keyProtection.DecryptText(encryptedValue);
                    string saltString = _keyProtection.DecryptText(encryptedIv);

                    if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(saltString))
                    {
                        LogManager.Error("Failed to decrypt key/iv");
                        continue;
                    }

                    byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
                    byte[] saltBytes = Encoding.ASCII.GetBytes(saltString);

                    keys.Add(new KeyData
                    {
                        Password = password,
                        Salt = saltBytes
                    });
                }

                return keys;
            }
            catch (Exception ex)
            {
                LogManager.Error($"Error reading XML file: {ex.Message}");
                return null;
            }
        }

        private byte[] GenerateCryptoClaimInfo(SymmetricAlgorithm alg)
        {
            using (ICryptoTransform encryptor = alg.CreateEncryptor())
            {
                byte[] inputBlock = new byte[encryptor.InputBlockSize];

                if (alg.KeySize >= encryptor.InputBlockSize)
                {
                    for (int i = 0; i < encryptor.InputBlockSize; i++)
                    {
                        inputBlock[i] = alg.Key[i];
                    }
                }
                else
                {
                    for (int j = 0; j < alg.KeySize / 8; j++)
                    {
                        inputBlock[j] = alg.Key[j];
                    }
                    for (int k = alg.KeySize / 8; k < encryptor.InputBlockSize; k++)
                    {
                        inputBlock[k] = 60;
                    }
                }

                byte[] outputBlock = new byte[encryptor.OutputBlockSize];
                encryptor.TransformBlock(inputBlock, 0, inputBlock.Length, outputBlock, 0);

                return outputBlock;
            }
        }

        private string ZipFileProperly(string filePath)
        {
            try
            {
                string zipPath = filePath + ".zip";
                string fileName = Path.GetFileName(filePath);

                if (File.Exists(zipPath))
                {
                    try 
                    { 
                        File.Delete(zipPath);
                    } 
                    catch { }
                }

                using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                    using (Stream entryStream = entry.Open())
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        fileStream.CopyTo(entryStream);
                    }
                }

                LogManager.Info($"Zipped: {fileName}");
                return zipPath;
            }
            catch (Exception ex)
            {
                LogManager.Error("Error zipping file: " + ex.Message);
                LogManager.Error("Details: " + ex.ToString());
                return null;
            }
        }
    }
}
