using System.Collections;
using Utilities.Exceptions;

public static class GenericValidationHelper
{
    /// <summary>
    /// Valida que ninguno de los valores enviados sea inválido:
    /// - null
    /// - string vacío o espacios
    /// - números igual a 0
    /// - decimales igual a 0
    /// - bool = false
    /// - colecciones vacías
    /// </summary>
    public static void ValidateObject(params object?[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];

            if (value == null)
                throw new BusinessException($"El valor en la posición {i + 1} no puede ser nulo.");

            switch (value)
            {
                case string str when string.IsNullOrWhiteSpace(str):
                    throw new BusinessException($"El valor en la posición {i + 1} no puede estar vacío.");
                case int n when n == 0:
                    throw new BusinessException($"El valor en la posición {i + 1} no puede ser 0.");
                case long n when n == 0:
                    throw new BusinessException($"El valor en la posición {i + 1} no puede ser 0.");
                case decimal n when n == 0:
                    throw new BusinessException($"El valor en la posición {i + 1} no puede ser 0.");
                case double n when n == 0:
                    throw new BusinessException($"El valor en la posición {i + 1} no puede ser 0.");
                case float n when n == 0:
                    throw new BusinessException($"El valor en la posición {i + 1} no puede ser 0.");
                case bool b when b == false:
                    throw new BusinessException($"El valor en la posición {i + 1} no puede ser false.");
                case IEnumerable e when value is not string && !e.Cast<object>().Any():
                    throw new BusinessException($"La colección en la posición {i + 1} no puede estar vacía.");
            }
        }
    }
}
