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

            ErrorCodes.NetworkError => "Ein Netzwerkfehler ist aufgetreten. Bitte überprüfe deine Internetverbindung.",

            ErrorCodes.FailedConnection => "Die Verbindung zum Server ist fehlgeschlagen. Bitte versuche es später erneut.",

            ErrorCodes.NotFound => "Der gesuchte Inhalt konnte nicht gefunden werden.",

            ErrorCodes.DataCreationFailed => "Das Element konnte nicht erstellt werden. Bitte versuche es noch einmal.",

            ErrorCodes.DataUpdateFailed => "Die Änderungen konnten nicht gespeichert werden. Bitte überprüfe deine Eingaben.",

            ErrorCodes.DataDeletionFailed => "Das Element konnte nicht gelöscht werden. Bitte versuche es später erneut.",

            ErrorCodes.IdenticalData => "Die eingegebenen Daten sind identisch mit den bereits vorhandenen. Es wurden keine Änderungen vorgenommen.",

            _ => "Ein unerwarteter Fehler ist aufgetreten. Bitte versuche es später noch einmal.",
        };
    }
}
