
using GastappApi.DTOs;

namespace GastappApi
{
    public static class ResponseMessages
    {
        // Errores
        public static ResponseMessage EmailAlreadyRegistered => new()
        {
            Code = 0,
            Message = "Este correo ya está registrado por otro usuario"
        };

        public static ResponseMessage UsernameAlreadyRegistered => new()
        {
            Code = 1,
            Message = "Este usuario ya está registrado por otro usuario"
        };

        public static ResponseMessage PasswordNotSafe => new()
        {
            Code = 2,
            Message = "Ingrese una contraseña más segura"
        };

        public static ResponseMessage UserNotFound => new()
        {
            Code = 3,
            Message = "Usuario no encontrado"
        };

        public static ResponseMessage IncorrectPassword => new()
        {
            Code = 4,
            Message = "Error autenticacion"
        };
        public static ResponseMessage ErrorInvalidToken => new()
        {
            Code = 5,
            Message = "Token Invalido"
        };

        public static ResponseMessage TokenRefreshed => new()
        {
            Code = 6,
            Message = "Token refescado"
        };

        // Mensajes de éxito
        public static ResponseMessage UserRegisteredSuccessfully => new()
        {
            Code = 100,
            Message = "Usuario registrado con éxito"
        };
        public static ResponseMessage LoginSuccesfully => new()
        {
            Code = 101,
            Message = "Usuario logueado con éxito",
        };
    }


    public class ResponseMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Object? Data { get; set; }
    }
}
