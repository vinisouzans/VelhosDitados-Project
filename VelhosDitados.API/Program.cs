using Microsoft.EntityFrameworkCore;
using VelhosDitados.API.Data;
using VelhosDitados.API.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

// CONFIGURAÇÃO DA CONEXÃO COM SQLITE
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Se estiver em container Docker, usa caminho absoluto
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    connectionString = "Data Source=/app/data/velhosditados.db";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowAngularDev");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

//INICIALIZAÇÃO DO BANCO DE DADOS COM TODOS OS DITADOS
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        Console.WriteLine("Inicializando SQLite...");
        // SQLite não precisa aguardar, é instantâneo
        // REMOVI: await Task.Delay(10000);

        // Cria o banco se não existir
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Banco SQLite verificado/criado!");

        // Adiciona dados iniciais se a tabela estiver vazia
        if (!context.Ditados.Any())
        {
            var todosDitados = new[]
            {
                "Em terra de Minhoca macarronada é suruba.",
                "Mais por Fora que cofrinho de mecânico.",
                "Mais por fora que cotovelo de taxista.",
                "Só pé que anda que leva a topada.",
                "Mais parado que Saci de patinete.",
                "Se fazendo de alho para levar uma socada.",
                "Se ferradura dessa sorte jumento não puxava carroça.",
                "Tô mais tranquilo que vaca na Índia.",
                "Cavalo Marinho se faz de peixe para não puxar carroça.",
                "Se finge de morto para comer o coveiro.",
                "É o olho do dono que engorda o Leitão.",
                "É como diz o meu tio que namora com uma anã, o buraco é mais embaixo.",
                "Cada cachorro que lamba sua caceta.",
                "Em terra de esqueleto toda fratura é exposta.",
                "Pra quem ta se afogando, jacaré vira tronco.",
                "Em terra de Saci uma calça da pra dois.",
                "Até explicar que fucinho de porco não é tomada.",
                "Até explicar que perna de barata não é serrote.",
                "Aranha vive é do que tece.",
                "Em terra de mudo, soco na boca é argumento.",
                "Mais cansado que o tatuador do MC Guimê.",
                "Se faz de picolé pra poder levar madeira.",
                "Mais pra frente que bigode de foca.",
                "Se finge de salada pra poder levar pepino.",
                "Mais folgado que volante de kombi.",
                "Em briga de elefante, quem sofre é a grama.",
                "Quem vive de talento é vendedor de chocolate.",
                "Rir com dente é fácil, quero ver banguela.",
                "Mais ultrapassado que carro de autoescola.",
                "Quem acha tudo gozado é camareira de motel.",
                "Onde tem abelha, formiga come sal.",
                "Mais saltitante que saci em tiroteio.",
                "Mais enrolado que namoro de cobra.",
                "É fazendo merda que aduba a vida.",
                "Quem ganha dinheiro com suor dos outros é dono de sauna.",
                "Quer moleza vai chupar nariz de velho.",
                "Quer moleza senta no pudim.",
                "Quem caminha porque quer não tem direito de cansar.",
                "Em terra de cacto todo abraço machuca.",
                "Estou mais preocupado do que peixe em semana santa.",
                "Mais inútil que cinzeiro em moto.",
                "Se faz de parede velha pra levar pintada nova.",
                "Mais cansado que o barbeiro do Gustavo Lima.",
                "Boca fechada não entra mosquito.",
                "Papagaio que fala demais é o primeiro a ser vendido.",
                "Se bater em madeira tirasse o azar, pica-pau não estava em extinção.",
                "Mais arriscado que peidar com diarreia.",
                "Mais pra frente que para-choque de fusca.",
                "Bode vestido de anjinho ainda tem cheiro de bode.",
                "Mais lento que caminhão em rotatória.",
                "Sapo cego não vê perereca.",
                "Mais por fora que casinha de cachorro.",
                "Se chiar resolve-se, Somrrisal não morria afogado.",
                "Tá igual papel higiênico, quando não tá no rolo tá na merda.",
                "Mais por fora que umbigo de piriguete.",
                "Mais liso que suvaco de estatua.",
                "Se corno voasse, a gente não enxergava o céu.",
                "Até explicar que caroço de manga não é sabonete.",
                "Mais cansado que o padre da Gretchen.",
                "Mais por fora que estepe de Jeep.",
                "Quer que o mundo acabe em barranco pra morrer escorado.",
                "Melhor andar atoa do que ficar parado atoa.",
                "Enquanto tem bambu tem flecha.",
                "Quem quer muito, traz de casa.",
                "Mais preguiçoso do que o cara que fez a bandeira do Japão.",
                "Mais sem cabeça que mula de folclore.",
                "De onde se tira o pão não se come a carne.",
                "Mais desconfiado que cego que tem amante.",
                "Mais perdido que mudo em karaokê.",
                "Mais fresco que cu de pinguim.",
                "Igual arame liso, cerca, cerca, mas não fura ninguém.",
                "Mais inútil que buzina em avião.",
                "Vendendo o almoço pra comprar a janta.",
                "Prestígio só dá dinheiro pra Nestle.",
                "Quem vive de amor é dono de motel.",
                "Se faz de leitão pra poder mamar deitado.",
                "Mais sumido que orelha de freira.",
                "Só se vive uma vez.",
                "Mais saidinho que preso no natal.",
                "Mais manso que pardal de igreja.",
                "Ralando mais que barriga de cobra.",
                "Mais parado que olho de boneca.",
                "Quando um não quer dois não briga.",
                "Mais sumido que ponta de durex.",
                "Mais perigoso que barbeiro com soluço.",
                "Mais folgado que mosquito de banheiro.",
                "Respeito é bom e conserva os dentes.",
                "Camarão que dorme a onda leva.",
                "Mais ligado que rádio de preso.",
                "Depois da meia-noite guardanapo vira bolo.",
                "Depois da meia-noite urubu vira frango.",
                "Quem não escuta cuidado, escuta coitado.",
                "Mais largo que cintura de sapo.",
                "Mais pra frente que pé de palhaço.",
                "Mais chato que chinelo de gordo.",
                "Finge de vesgo pra comer em dois pratos.",
                "Se eu gostasse de MIMIMI eu arrumava um GATO GAGO.",
                "Quem não chora não mama.",
                "Em tempos de guerra, qualquer buraco é trincheira.",
                "Não quero saber se o futebol é de Saci, eu quero é gol de letra.",
                "Em jogo de curupira todo gol é de calcanhar.",
                "O mau do urubu é achar que todo boi tá morto.",
                "Em terra de bêbado, álcool em gel é patê.",
                "Mais sério que defunto.",
                "Mais vale uma abelha voando, do que duas na mão",
                "Mais vale um peito na boca do que dois no sutiã.",
                "Mais parado que saci de skate.",
                "Não quero saber quem pintou a zebra, eu quero o restinho de tinta.",
                "O que é um peido para quem tá cagado.",
                "Pisou na merda, abre os dedos.",
                "Tá no inferno abraça o capeta.",
                "Mais por fora que a cueca do super-homem.",
                "Mais por fora que saco de índio.",
                "Mais enrolado que rabo de porco.",
                "Mais enrolado que briga de polvo.",
                "Mais vale um buraco na sua camisinha do que uma camisinha no seu buraco.",
                "Mais indeciso do que camaleão no arco-íris.",
                "Mais indeciso do que camaleão daltônico.",
                "Quem refresca bunda de pato é lagoa.",
                "Mais perdido que surdo em bingo.",
                "Mais perdido que cego em tiroteio.",
                "Mais perdido que piolho em cabeça de careca.",
                "Mais perdido que barata em forró.",
                "Mais perdido que isqueiro de fumante.",
                "Galinha que acompanha Pato morre afogada.",
                "Cachorro que é mordido por cobra, tem medo até de linguiça.",
                "Pardal que acompanha João de Barro vira ajudante de pedreiro.",
                "Em terra de saci todo chute é voadora.",
                "Em terra de medroso, todo lençol é fantasma.",
                "Finge de doido pra poder cagar dentro de casa.",
                "Jacaré parado vira bolsa de madame.",
                "Passarinho que come pedra sabe o cu que tem.",
                "Quem come quieto come sempre.",
                "Passarinho que acompanha morcego dorme de cabeça pra baixo.",
                "Mais perdido que azeitona em boca de banguela.",
                "Mais perdido que cebola em salada de fruta.",
                "Mais perdido que cachorro na mudança.",
                "Em terra de cego, quem tem olho é rei.",
                "Cavalo dado não se olha os dentes.",
                "Quem não tem orelha não usa óculos.",
                "Em rio que tem piranha, jacaré bebe água de canudinho.",
                "O boi baba porque não sabe cuspir."
            };

            foreach (var ditado in todosDitados)
            {
                context.Ditados.Add(new Ditado { Descricao = ditado });
            }

            await context.SaveChangesAsync();
            Console.WriteLine($" {todosDitados.Length} ditados adicionados ao banco!");
        }
        else
        {
            Console.WriteLine("Banco já contém dados. Nenhum ditado adicionado.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar banco: {ex.Message}");
    }
}

app.Run();