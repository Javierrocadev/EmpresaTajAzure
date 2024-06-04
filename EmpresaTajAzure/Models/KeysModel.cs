namespace EmpresaTajAzure.Models
{
    public class KeysModel
    {
        public string MySql { get; set; }
        public string UrlApi { get; set; }
        public string ApplicationIDSecretName { get; set; }
        public string SecretKeySecretName { get; set; }
        public string UrlLogicAppSecretName { get; set; }
        public string StorageAccountSecretName { get; set; }
    }
}
