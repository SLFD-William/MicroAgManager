namespace Domain.Constants
{
    public static class AuthenticationConstants
    {
        public const string LoginLocked  = "User is locked out";
        public const string LoginUnauthorized  = "User is unauthorized";
        public const string LoginInvalid  = "Login is Invalid";
        public const string RegistrationUserExists  = "User already Exists";
        public const string RegistrationFailed  = "User creation failed! Please check user details and try again.";
    }
    public static class TennantAccessLevelConstants
    { 
        public const string Admin  = "Admin";
        public const string SingleUser  = "Single User";
    }
}
