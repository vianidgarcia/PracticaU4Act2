namespace PracticaU4Act2;

internal static class ErrorMessages
{
    public const string EmptyTree = "El árbol está vacío.";
    public const string EmptyKey = "Escribe una clave entera.";
    public const string EmptySequence = "Escribe al menos una clave en la secuencia.";

    public static string DuplicateKey(int key) =>
        $"La clave {key} ya existe; un árbol binario de búsqueda no admite duplicados.";

    public static string KeyNotFound(int key) =>
        $"La clave {key} no existe en el árbol.";

    public static string InvalidKey(string text) =>
        $"'{text}' no es un número entero válido.";
}
