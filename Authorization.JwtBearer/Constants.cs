namespace Authorization.JwtBearer;

public static class Constants
{
    public const string Issuer = "https://localhost:7183";
    public const string Audience = Issuer;
    public const string SecretKey = "this_is_secret_key_for_jwt";
}
