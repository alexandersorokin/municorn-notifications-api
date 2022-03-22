namespace Municorn.Notifications.Api
{
    public static class ClientErrorCodes
    {
        public const string Error = "client:error";

        public const string ContentNotExists = "client:contentNotExists";

        public const string SerializationFailed = "client:serializationFailed";

        public const string DeserializationFailed = "client:deserializationFailed";

        public const string TimeoutError = "client:requestTimeout";

        public const string ThrottledError = "client:requestThrottled";

        public const string UnavailableError = "client:serviceUnavailable";
    }
}
