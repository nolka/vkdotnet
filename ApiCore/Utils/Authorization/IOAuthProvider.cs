namespace ApiCore.Utils.Authorization
{
    public interface IOAuthProvider
    {
        SessionInfo Authorize(int appId, string scope, string display);
    }
}