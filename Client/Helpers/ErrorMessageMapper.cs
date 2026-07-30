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

            ErrorCodes.CategoryInUse => "Diese Kategorie kann man nicht löschen, " +
            "da sie noch von Produkten verwendet wird. " +
            "Bitte weisen Sie den Produkten eine andere Kategorie zu um diese Kategorie zu löschen.",

            ErrorCodes.DataDeletionAndCreationOfProductFailded => "Das Produkt konnte nicht gespeichert werden " +
            "und beim automatischen Löschen der verwaisten Beschreibung trat ein Fehler auf. " +
            "Bitte informiere den Support.",

            ErrorCodes.CategoryNotFound => "Die ausgewählte Kategorie konnte in der Datenbank nicht gefunden werden.",

            ErrorCodes.ProductAlreadyExists => "Ein Produkt mit genau diesem Namen und Preis existiert bereits.",

            ErrorCodes.DescriptionAlreadyExists => "Eine exakt gleiche Beschreibung existiert bereits in der Datenbank.",

            ErrorCodes.CategoryAlreadyExists => "Eine Kategorie mit diesem Namen existiert bereits. Bitte wähle einen anderen Namen.",

            ErrorCodes.DescriptionCreationFailed => "Die Produktbeschreibung konnte nicht erfolgreich gespeichert werden.",

            ErrorCodes.ProductCreationFailed => "Das Produkt konnte nicht angelegt werden. Bitte versuche es erneut.",

            ErrorCodes.CategoryCreationFailed => "Die neue Kategorie konnte nicht erstellt werden.",

            ErrorCodes.ProductUpdateFailed => "Die Änderungen am Produkt konnten nicht gespeichert werden.",

            ErrorCodes.DescriptionUpdateFailed => "Die Änderungen an der Beschreibung konnten nicht gespeichert werden.",

            ErrorCodes.CategoryUpdateFailed => "Die Änderungen an der Kategorie konnten nicht gespeichert werden.",

            ErrorCodes.ProductDeletionFailed => "Das Produkt konnte leider nicht gelöscht werden.",

            ErrorCodes.DescriptionDeletionFailed => "Die zum Produkt gehörende Beschreibung konnte nicht gelöscht werden.",

            ErrorCodes.CategoryDeletionFailed => "Die Kategorie konnte leider nicht gelöscht werden.",

            _ => "Ein unerwarteter Fehler ist aufgetreten. Bitte versuche es später noch einmal.",
        };
    }
}