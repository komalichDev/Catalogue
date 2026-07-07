using Common.Exception;

namespace Client.Helpers;

public static class ErrorMessageMapper
{
    public static string ToUserMessage(this ErrorCodes errorCode)
    {
        return errorCode switch
        {
            ErrorCodes.None => "Alles hat reibungslos funktioniert.",

            ErrorCodes.NoDataFound => "Es konnten leider keine Daten gefunden werden. Bitte überprüfe deine Suchkriterien.",

            ErrorCodes.EntityNotFound => "Das gesuchte Element existiert nicht mehr oder wurde entfernt.",

            _ => "Ein unerwarteter Fehler ist aufgetreten. Bitte versuche es später noch einmal.",
        };
    }
}
