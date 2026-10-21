namespace DoctorPilot.Api.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string TenantRequired = nameof(TenantRequired);
        public const string TenantAdministration = nameof(TenantAdministration);
        public const string ProfessionalDirectoryAccess = nameof(ProfessionalDirectoryAccess);
        public const string ProfessionalRequired = nameof(ProfessionalRequired);
        public const string OperationalAccess = nameof(OperationalAccess);
        public const string AdministrativeAssistantAccess = nameof(AdministrativeAssistantAccess);
        public const string TemplateAccess = nameof(TemplateAccess);
    }
}
