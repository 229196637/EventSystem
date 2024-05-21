namespace DefaultNamespace.EventSystem.Events
{
    public class NetworkConnetError : NetworkEvent
    {
        public string error = "";

        public NetworkConnetError(string err)
        {
            error = err;
        }
    }
}