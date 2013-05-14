namespace ApiCore.Utils.Authorization
{
    public interface IVKAuthProvider
    {
        SessionInfo Authorize();
    }
}