namespace AMT.Data.Entities
{
    public class UserAuthorizationDim
    {
        public string UserKey { get; set; }
        public string UserScope { get; set; }
        public string StudentPermission { get; set; }
        public string SectionPermission { get; set; }
        public string SchoolPermission { get; set; }
        public int? DistrictId { get; set; }
    }
}
