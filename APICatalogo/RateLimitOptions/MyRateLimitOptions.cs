namespace APICatalogo.MyRateLimitOptions;

public class MyRateLimitOptions
{
    public const string MyRateLimit = "MyRateLimit";
    public int PermitLimit { get; set; } = 5; //define o limite de requisições dentro de uma janela de tempo determinada
    public int Window { get; set; } = 10; // define a janela de tempo
    public int ReplenishmentPeriod { get; set; } = 2; // define a cada quanto tempo um novo token será gerado
    public int QueueLimit { get; set; } = 2; // define o limite de requisições que podem ser colocadas em fila de espera
    public int SegmentsPerWindow { get; set; } = 8;
    public int TokenLimit { get; set; } = 10; // define o limite de tokens, para caso a configuração do limiter seja uma token bucket
    public int TokenLimit2 { get; set; } = 20;
    public int TokensPerPeriod { get; set; } = 4; // define quantos tokens serão gerados (o tempo de geração é definido no ReplenishmentPeriod)
    public bool AutoReplenishment { get; set; } = false; // define se a geração dos tokens será automática
}
