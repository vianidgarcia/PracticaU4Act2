namespace PracticaU4Act2;

internal static class MensajesError
{
    public const string ArbolVacio = "El árbol está vacío.";
    public const string ClaveVacia = "Escribe una clave entera.";
    public const string SecuenciaVacia = "Escribe al menos una clave en la secuencia.";

    public static string ClaveDuplicada(int clave) =>
        $"La clave {clave} ya existe; un árbol binario de búsqueda no admite duplicados.";

    public static string ClaveInexistente(int clave) =>
        $"La clave {clave} no existe en el árbol.";

    public static string ClaveInvalida(string texto) =>
        $"'{texto}' no es un número entero válido.";
}
