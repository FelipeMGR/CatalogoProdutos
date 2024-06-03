using APICatalogo.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace APICatalogo.Extensions
{
    public static class ApiExceptionsMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            //Configura o Middleware do ExceptionHandler para uso. As exceções tratadas aqui são nível global, mas a nível de pipeline.
            app.UseExceptionHandler(
                appError =>
                {
                    //Define um delegate que será acionado quando uma exceção não tratada for disparada.
                    appError.Run(async context =>
                    {
                        //Define o tipo de códifo HTTP que será retornado.
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        //Define o tipo de estrutura de dados da resposta.
                        context.Response.ContentType = "application/json";
                        //Nos permite acessar certas informações da exceção que foi disparada.
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        //Verifica se o feature é nulo.
                        if (contextFeature != null)
                        {
                            //O método Write é usado para escrever dados no corpo da requisição.
                            //A classe ErrorDetails se torna o "local" onde as informações serão escritas.
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                //Aqui, é feita a atribuição daquilo que foi definido anteriormente à todas as propriedades da classe ErrorDetails.
                                //O método "Write/WriteAsync" escreverá tudo que foi passado anteriormente nas propriedas da classe.
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message,
                                Trace = contextFeature.Error.StackTrace
                                //Essas propriedas acima definem quais informações serão mostradas na tela do usuario quando uma exceção não tratada for disparada.
                            }.ToString());
                        }
                    });
                }
                );
        }
    }
}
