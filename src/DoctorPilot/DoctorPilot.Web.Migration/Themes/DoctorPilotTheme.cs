using MudBlazor;

namespace DoctorPilot.Web.Migration.Themes;

public static class DoctorPilotTheme
{
    private static readonly string[] FontFamily =
    [
        "Segoe UI",
        "Helvetica Neue",
        "Arial",
        "sans-serif"
    ];

    public static MudTheme Default { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#3157D5",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#0F9F9A",
            SecondaryContrastText = "#FFFFFF",
            Tertiary = "#22B8CF",
            TertiaryContrastText = "#07253A",
            Background = "#E3EAF2",
            BackgroundGray = "#D8E1EB",
            Surface = "#E4EAF1",
            AppbarBackground = "#10233F",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#10233F",
            DrawerText = "#E7EEF8",
            DrawerIcon = "#AFC3DD",
            TextPrimary = "#17243A",
            TextSecondary = "#52627A",
            TextDisabled = "#8B98AA",
            ActionDefault = "#52627A",
            Divider = "#CBD6E2",
            DividerLight = "#D7E0EA",
            LinesDefault = "#CBD6E2",
            LinesInputs = "#BCC9D9",
            TableLines = "#E2E9F2",
            TableHover = "#EDF3FA",
            Error = "#C83E4D",
            ErrorContrastText = "#FFFFFF",
            Warning = "#B86B00",
            WarningContrastText = "#FFFFFF",
            Success = "#27875A",
            SuccessContrastText = "#FFFFFF",
            Info = "#2673C9",
            InfoContrastText = "#FFFFFF"
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = FontFamily
            },
            H1 = new H1Typography
            {
                FontFamily = FontFamily,
                FontWeight = "700"
            },
            H2 = new H2Typography
            {
                FontFamily = FontFamily,
                FontWeight = "700"
            },
            H3 = new H3Typography
            {
                FontFamily = FontFamily,
                FontWeight = "700"
            },
            H4 = new H4Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600"
            },
            H5 = new H5Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600"
            },
            H6 = new H6Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600"
            },
            Subtitle1 = new Subtitle1Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600"
            },
            Subtitle2 = new Subtitle2Typography
            {
                FontFamily = FontFamily,
                FontWeight = "600"
            },
            Body1 = new Body1Typography
            {
                FontFamily = FontFamily
            },
            Body2 = new Body2Typography
            {
                FontFamily = FontFamily
            },
            Button = new ButtonTypography
            {
                FontFamily = FontFamily,
                FontWeight = "600",
                TextTransform = "none"
            },
            Caption = new CaptionTypography
            {
                FontFamily = FontFamily
            },
            Overline = new OverlineTypography
            {
                FontFamily = FontFamily
            }
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "12px",
            DrawerWidthLeft = "250px",
            AppbarHeight = "64px"
        }
    };
}
