namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class ResponseStorageConfig
    {
        public bool EnableStorage { get; set; } = false;

        public string StorageDirectory { get; set; }

        public string SaltKeyFilePath { get; set; }

        public string EncryptionPassword { get; set; } = "password1";

        public bool EnableEncryption { get; set; } = true;
    }
}
