using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();
        
        var response = await next(cancellationToken);
        
        timer.Stop();
        var timeTaken = timer.Elapsed;
        if(timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} Seconds to complete",
                typeof(TRequest).Name, (double)timeTaken.Milliseconds/1000);
        
        logger.LogInformation("[END] Handled {Request} with {Response}, took {TimeTaken} Seconds",
            typeof(TRequest).Name, typeof(TResponse).Name, (double)timeTaken.Milliseconds/1000);

        return response;
    }
}