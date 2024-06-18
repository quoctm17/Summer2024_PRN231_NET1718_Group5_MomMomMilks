namespace MomMomMilks.Types
{
    public record Response(
        int error,
        String message,
        object? data
        );
}
