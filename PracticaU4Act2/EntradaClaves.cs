namespace PracticaU4Act2;

// Convierte el texto que escribe el usuario en claves enteras, con mensajes claros si algo no es válido.
public static class EntradaClaves
{
    private static readonly char[] Separadores = { ',', ';', ' ', '\t', '\r', '\n' };

    public static int LeerClave(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException(MensajesError.ClaveVacia);

        texto = texto.Trim();
        if (!int.TryParse(texto, out int clave))
            throw new ArgumentException(MensajesError.ClaveInvalida(texto));
        return clave;
    }

    public static List<int> LeerSecuencia(string? texto)
    {
        string[] partes = (texto ?? "").Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0)
            throw new ArgumentException(MensajesError.SecuenciaVacia);

        var claves = new List<int>(partes.Length);
        foreach (string parte in partes)
            claves.Add(LeerClave(parte));
        return claves;
    }
}
