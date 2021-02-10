namespace AMT.Data.Descriptors.Enumerations
{
    public class UserAuthorizationScope : Enumeration<UserAuthorizationScope, string>
    {
        public static readonly UserAuthorizationScope Section = new UserAuthorizationScope("AuthorizationScope.Section", "Teacher");
        public static readonly UserAuthorizationScope School = new UserAuthorizationScope("AuthorizationScope.School", "School administrator");
        public static readonly UserAuthorizationScope District = new UserAuthorizationScope("AuthorizationScope.District", "District administrator");
        public UserAuthorizationScope(string value, string displayName) : base(value, displayName) { }
    }
}
