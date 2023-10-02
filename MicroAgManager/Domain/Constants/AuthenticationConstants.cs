namespace Domain.Constants
{
    public static class AuthenticationConstants
    {
        public static string LoginLocked { get; private set; } = "User is locked out";
        public static string LoginUnauthorized { get; private set; } = "User is unauthorized";
        public static string LoginInvalid { get; private set; } = "Login is Invalid";
        public static string RegistrationUserExists { get; private set; } = "User already Exists";
        public static string RegistrationFailed { get; private set; } = "User creation failed! Please check user details and try again.";
    }
    public static class TennantAccessLevelConstants
    { 
        public static string Admin { get; private set; } = "Admin";
        public static string SingleUser { get; private set; } = "Single User";
    }
}
