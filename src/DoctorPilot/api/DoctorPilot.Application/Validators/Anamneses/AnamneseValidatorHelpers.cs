using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.Validators.Anamneses;

internal static class AnamneseValidatorHelpers
{
    public static bool TryParseTipo(string? value, out AnamneseSessaoTypeEnum tipo)
        => Enum.TryParse(value, true, out tipo) && Enum.IsDefined(tipo);
}
