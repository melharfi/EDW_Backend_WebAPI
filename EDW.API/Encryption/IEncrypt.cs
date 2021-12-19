namespace EDW.API.Encryption
{
    public interface IEncrypt
    {
        public string HashPassword(string password);
    }
}
