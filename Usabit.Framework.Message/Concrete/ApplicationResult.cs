using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Usabit.Framework.Message.Abstraction;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Usabit.Framework.Result.Concrete
{
    [DataContract(Name = "application-result", Namespace = "http://usabit.com.br/framework/reuslt/type")]
    public class ApplicationResult<TResult> : IApplicationResult<TResult>
    {
        public ApplicationResult() { }

        [DataMember(Name = "result")]
        public TResult Result { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "validations")]
        public List<string> Validations { get; set; } = new List<string>();

        [DataMember(Name = "protocol")]
        public string Protocol { get; private set; }
        
        [IgnoreDataMember]
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        [IgnoreDataMember]
        public bool AutoAssignHttpStatusCode { get; set; } = true;

        public IApplicationResult<TResult> Set(HttpStatusCode httpStatusCode, string msg)
        {
            Message = msg;
            HttpStatusCode = httpStatusCode;

            return this as IApplicationResult<TResult>;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(this));
            context.HttpContext.Response.Headers.Add("content-type", "application/json");

            if (this.AutoAssignHttpStatusCode && this.Validations?.Count > 0)
            {
                this.SetToUnprocessableEntity("Unprocessable entity");
                context.HttpContext.Response.StatusCode = (int)this.HttpStatusCode;
            }
            else
                context.HttpContext.Response.StatusCode = (int)this.HttpStatusCode;

            await content.CopyToAsync(context.HttpContext.Response.Body);
        }

        public IApplicationResult<TResult> SetToUnprocessableEntity(string msg) => Set((HttpStatusCode)422, msg);

        public IApplicationResult<TResult> SetHttpStatusToOk(string msg = "Sucesso") => Set(HttpStatusCode.OK, msg);

        public IApplicationResult<TResult> SetHttpStatusToCreated(string msg = "Criado com sucesso.") => Set(HttpStatusCode.Created, msg);

        public IApplicationResult<TResult> SetHttpStatusToAccepted(string msg = "Em fila de processamento.") => Set(HttpStatusCode.Accepted, msg);

        public IApplicationResult<TResult> SetHttpStatusToBadRequest(string msg = "Request inválido") => Set(HttpStatusCode.BadRequest, msg);

        public IApplicationResult<TResult> SetHttpStatusToInternalServerError(string msg = "Erro de comunicação.") => Set(HttpStatusCode.InternalServerError, msg);

        public IApplicationResult<TResult> SetHttpStatusToForbidden(string msg = "O servidor recusou a requisição.") => Set(HttpStatusCode.Forbidden, msg);

        public IApplicationResult<TResult> SetHttpStatusToNotFound(string msg = "Recurso não encontrado no servidor.") => Set(HttpStatusCode.NotFound, msg);

        public IApplicationResult<TResult> SetHttpStatusToServiceUnavailable(string msg = "Serviço indisponível.") => Set(HttpStatusCode.ServiceUnavailable, msg);
        
        public IApplicationResult<TResult> SetHttpStatusToUnauthorized(string msg = "Não autorizado o acesso ao recurso.") => Set(HttpStatusCode.Unauthorized, msg);

        public IApplicationResult<TResult> SetHttpStatusToNotAcceptable(string msg = "Informação Authorization no header é obrigatório.") => Set(HttpStatusCode.NotAcceptable, msg);

        public IApplicationResult<TResult> SetHttpStatusToRequestTimeout(string msg = "Tempo limite atingido no request.") => Set(HttpStatusCode.RequestTimeout, msg);

        public IApplicationResult<TResult> SetHttpStatusToGatewayTimeout(string msg = "Tempo limite atingido.") => Set(HttpStatusCode.GatewayTimeout, msg);
    }
}
