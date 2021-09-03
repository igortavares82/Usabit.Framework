using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Usabit.Framework.Message.Abstraction
{
    public interface IApplicationResult<TResult> : IActionResult
    {
        TResult Result { get; set; }
        string Message { get; set; }
        List<string> Validations { get; set; }
        string Protocol { get; }
        HttpStatusCode HttpStatusCode { get; set; }
        bool AutoAssignHttpStatusCode { get; set; }

        /// <summary>
        /// 422 Unprocessable Entity - Utilizado para validações de negócio ou alguma informação necessária para uma ação.        
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetToUnprocessableEntity(string msg);

        /// <summary>
        /// 200 OK - Utilizado para todas as situações de sucesso.
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToOk(string msg = "Sucesso");

        /// <summary>
        /// 201 Created - Utilizado para criação de registro no banco
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToCreated(string msg = "Criado com sucesso.");

        /// <summary>
        /// 202 Accepted - Utilizado para chamadas async ou adicionar algum item em fila de processamento.
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToAccepted(string msg = "Em fila de processamento.");

        /// <summary>
        /// 400 Bad Request - Utilizado para a maioria dos erros de request.
        /// <c>Campos obrigatórios no request ou erro na camada de serviço.</c>
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToBadRequest(string msg = "Request inválido");

        /// <summary>
        /// 500 Internal Server Error - Utilizado para erros internos no servidor ou exceptions.
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToInternalServerError(string msg = "Erro de comunicação.");

        /// <summary>
        /// 403 Forbidden - Utilizado para requisições rejeitadas pelo servidor.        
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToForbidden(string msg = "O servidor recusou a requisição.");

        /// <summary>
        /// 404 Not Found - Utilizado para informar rota inválida.
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToNotFound(string msg = "Recurso não encontrado no servidor.");

        /// <summary>
        /// 503 Service Unavailable - Utilizado para informar serviço indisponível.        
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToServiceUnavailable(string msg = "Serviço indisponível.");

        /// <summary>
        /// 401 Unauthorized - Utilizado para token inválido ou restrição de acesso.
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToUnauthorized(string msg = "Não autorizado o acesso ao recurso.");

        /// <summary>
        /// 406 NotAcceptable - Utilizado para informar que existe informações que devem ser passadas no header.         
        /// </summary>    
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToNotAcceptable(string msg = "Informação Authorization no header é obrigatório.");

        /// <summary>
        ///  408 RequestTimeout
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToRequestTimeout(string msg = "Tempo limite atingido no request.");

        /// <summary>
        ///  504 GatewayTimeout
        /// </summary>
        /// <param name="msg"></param>
        IApplicationResult<TResult> SetHttpStatusToGatewayTimeout(string msg = "Tempo limite atingido.");
    }
}
