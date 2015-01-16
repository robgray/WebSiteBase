namespace Domain.Interfaces
{
    public interface ICryptographyProvider
    {
        string GetRandomString(int length);
        string GetHash(string salt, string plainText);
        byte[] GetRandomBytes(int size);
    }
}
