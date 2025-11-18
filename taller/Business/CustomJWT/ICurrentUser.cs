namespace Business.CustomJWT
{
    /// <summary>
    /// Define un contrato para obtener información del usuario actualmente autenticado
    /// dentro del contexto de ejecución.
    ///
    /// Esta interfaz abstrae el acceso a los datos del usuario (por ejemplo,
    /// obtenidos desde un token JWT) y facilita la validación de roles o permisos
    /// específicos dentro de la capa de negocio.
    ///
    /// Es comúnmente utilizada en servicios o controladores que requieren conocer
    /// la identidad y privilegios del usuario que realiza la operación.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Identificador de la persona asociada al usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Puede ser <c>null</c> si no existe un usuario autenticado en el contexto actual.
        /// </remarks>
        int? PersonId { get; }


        int? UserId { get; }
    }
}