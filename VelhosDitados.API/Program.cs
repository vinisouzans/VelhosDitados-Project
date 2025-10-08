using Microsoft.EntityFrameworkCore;
using VelhosDitados.API.Data;
using VelhosDitados.API.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

// CONFIGURA��O DA CONEX�O COM SQLITE
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

//INICIALIZA��O DO BANCO DE DADOS COM TODOS OS DITADOS
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        Console.WriteLine("Inicializando SQLite...");
        // SQLite n�o precisa aguardar, � instant�neo
        // REMOVI: await Task.Delay(10000);

        // Cria o banco se n�o existir
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Banco SQLite verificado/criado!");

        // Adiciona dados iniciais se a tabela estiver vazia
        if (!context.Ditados.Any())
        {
            var todosDitados = new[]
            {
                "Em terra de Minhoca macarronada � suruba.",
                "Mais por Fora que cofrinho de mec�nico.",
                "Mais por fora que cotovelo de taxista.",
                "S� p� que anda que leva a topada.",
                "Mais parado que Saci de patinete.",
                "Se fazendo de alho para levar uma socada.",
                "Se ferradura dessa sorte jumento n�o puxava carro�a.",
                "T� mais tranquilo que vaca na �ndia.",
                "Cavalo Marinho se faz de peixe para n�o puxar carro�a.",
                "Se finge de morto para comer o coveiro.",
                "� o olho do dono que engorda o Leit�o.",
                "� como diz o meu tio que namora com uma an�, o buraco � mais embaixo.",
                "Cada cachorro que lamba sua caceta.",
                "Em terra de esqueleto toda fratura � exposta.",
                "Pra quem ta se afogando, jacar� vira tronco.",
                "Em terra de Saci uma cal�a da pra dois.",
                "At� explicar que fucinho de porco n�o � tomada.",
                "At� explicar que perna de barata n�o � serrote.",
                "Aranha vive � do que tece.",
                "Em terra de mudo, soco na boca � argumento.",
                "Mais cansado que o tatuador do MC Guim�.",
                "Se faz de picol� pra poder levar madeira.",
                "Mais pra frente que bigode de foca.",
                "Se finge de salada pra poder levar pepino.",
                "Mais folgado que volante de kombi.",
                "Em briga de elefante, quem sofre � a grama.",
                "Quem vive de talento � vendedor de chocolate.",
                "Rir com dente � f�cil, quero ver banguela.",
                "Mais ultrapassado que carro de autoescola.",
                "Quem acha tudo gozado � camareira de motel.",
                "Onde tem abelha, formiga come sal.",
                "Mais saltitante que saci em tiroteio.",
                "Mais enrolado que namoro de cobra.",
                "� fazendo merda que aduba a vida.",
                "Quem ganha dinheiro com suor dos outros � dono de sauna.",
                "Quer moleza vai chupar nariz de velho.",
                "Quer moleza senta no pudim.",
                "Quem caminha porque quer n�o tem direito de cansar.",
                "Em terra de cacto todo abra�o machuca.",
                "Estou mais preocupado do que peixe em semana santa.",
                "Mais in�til que cinzeiro em moto.",
                "Se faz de parede velha pra levar pintada nova.",
                "Mais cansado que o barbeiro do Gustavo Lima.",
                "Boca fechada n�o entra mosquito.",
                "Papagaio que fala demais � o primeiro a ser vendido.",
                "Se bater em madeira tirasse o azar, pica-pau n�o estava em extin��o.",
                "Mais arriscado que peidar com diarreia.",
                "Mais pra frente que para-choque de fusca.",
                "Bode vestido de anjinho ainda tem cheiro de bode.",
                "Mais lento que caminh�o em rotat�ria.",
                "Sapo cego n�o v� perereca.",
                "Mais por fora que casinha de cachorro.",
                "Se chiar resolve-se, Somrrisal n�o morria afogado.",
                "T� igual papel higi�nico, quando n�o t� no rolo t� na merda.",
                "Mais por fora que umbigo de piriguete.",
                "Mais liso que suvaco de estatua.",
                "Se corno voasse, a gente n�o enxergava o c�u.",
                "At� explicar que caro�o de manga n�o � sabonete.",
                "Mais cansado que o padre da Gretchen.",
                "Mais por fora que estepe de Jeep.",
                "Quer que o mundo acabe em barranco pra morrer escorado.",
                "Melhor andar atoa do que ficar parado atoa.",
                "Enquanto tem bambu tem flecha.",
                "Quem quer muito, traz de casa.",
                "Mais pregui�oso do que o cara que fez a bandeira do Jap�o.",
                "Mais sem cabe�a que mula de folclore.",
                "De onde se tira o p�o n�o se come a carne.",
                "Mais desconfiado que cego que tem amante.",
                "Mais perdido que mudo em karaok�.",
                "Mais fresco que cu de pinguim.",
                "Igual arame liso, cerca, cerca, mas n�o fura ningu�m.",
                "Mais in�til que buzina em avi�o.",
                "Vendendo o almo�o pra comprar a janta.",
                "Prest�gio s� d� dinheiro pra Nestle.",
                "Quem vive de amor � dono de motel.",
                "Se faz de leit�o pra poder mamar deitado.",
                "Mais sumido que orelha de freira.",
                "S� se vive uma vez.",
                "Mais saidinho que preso no natal.",
                "Mais manso que pardal de igreja.",
                "Ralando mais que barriga de cobra.",
                "Mais parado que olho de boneca.",
                "Quando um n�o quer dois n�o briga.",
                "Mais sumido que ponta de durex.",
                "Mais perigoso que barbeiro com solu�o.",
                "Mais folgado que mosquito de banheiro.",
                "Respeito � bom e conserva os dentes.",
                "Camar�o que dorme a onda leva.",
                "Mais ligado que r�dio de preso.",
                "Depois da meia-noite guardanapo vira bolo.",
                "Depois da meia-noite urubu vira frango.",
                "Quem n�o escuta cuidado, escuta coitado.",
                "Mais largo que cintura de sapo.",
                "Mais pra frente que p� de palha�o.",
                "Mais chato que chinelo de gordo.",
                "Finge de vesgo pra comer em dois pratos.",
                "Se eu gostasse de MIMIMI eu arrumava um GATO GAGO.",
                "Quem n�o chora n�o mama.",
                "Em tempos de guerra, qualquer buraco � trincheira.",
                "N�o quero saber se o futebol � de Saci, eu quero � gol de letra.",
                "Em jogo de curupira todo gol � de calcanhar.",
                "O mau do urubu � achar que todo boi t� morto.",
                "Em terra de b�bado, �lcool em gel � pat�.",
                "Mais s�rio que defunto.",
                "Mais vale uma abelha voando, do que duas na m�o",
                "Mais vale um peito na boca do que dois no suti�.",
                "Mais parado que saci de skate.",
                "N�o quero saber quem pintou a zebra, eu quero o restinho de tinta.",
                "O que � um peido para quem t� cagado.",
                "Pisou na merda, abre os dedos.",
                "T� no inferno abra�a o capeta.",
                "Mais por fora que a cueca do super-homem.",
                "Mais por fora que saco de �ndio.",
                "Mais enrolado que rabo de porco.",
                "Mais enrolado que briga de polvo.",
                "Mais vale um buraco na sua camisinha do que uma camisinha no seu buraco.",
                "Mais indeciso do que camale�o no arco-�ris.",
                "Mais indeciso do que camale�o dalt�nico.",
                "Quem refresca bunda de pato � lagoa.",
                "Mais perdido que surdo em bingo.",
                "Mais perdido que cego em tiroteio.",
                "Mais perdido que piolho em cabe�a de careca.",
                "Mais perdido que barata em forr�.",
                "Mais perdido que isqueiro de fumante.",
                "Galinha que acompanha Pato morre afogada.",
                "Cachorro que � mordido por cobra, tem medo at� de lingui�a.",
                "Pardal que acompanha Jo�o de Barro vira ajudante de pedreiro.",
                "Em terra de saci todo chute � voadora.",
                "Em terra de medroso, todo len�ol � fantasma.",
                "Finge de doido pra poder cagar dentro de casa.",
                "Jacar� parado vira bolsa de madame.",
                "Passarinho que come pedra sabe o cu que tem.",
                "Quem come quieto come sempre.",
                "Passarinho que acompanha morcego dorme de cabe�a pra baixo.",
                "Mais perdido que azeitona em boca de banguela.",
                "Mais perdido que cebola em salada de fruta.",
                "Mais perdido que cachorro na mudan�a.",
                "Em terra de cego, quem tem olho � rei.",
                "Cavalo dado n�o se olha os dentes.",
                "Quem n�o tem orelha n�o usa �culos.",
                "Em rio que tem piranha, jacar� bebe �gua de canudinho.",
                "O boi baba porque n�o sabe cuspir."
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
            Console.WriteLine("Banco j� cont�m dados. Nenhum ditado adicionado.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar banco: {ex.Message}");
    }
}

app.Run();