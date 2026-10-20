namespace DoctorPilot.Api.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string TenantRequired = nameof(TenantRequired);
        public const string ProfessionalRequired = nameof(ProfessionalRequired);
        public const string OperationalAccess = nameof(OperationalAccess);
        public const string AdministrativeAssistantAccess = nameof(AdministrativeAssistantAccess);
    }
}
