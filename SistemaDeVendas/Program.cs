using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Text;
using SistemaDeVendas.Classes;
using System.Linq.Expressions;

internal class Program
{
    private static void Main(string[] args)
    {
        Login();
    }

    #region Cadastros
    public static void MenuCadastros(string usuario)
    {
        Console.Clear();
        Console.WriteLine("1 - Usuário");
        Console.WriteLine("2 - Produto");
        Console.WriteLine("3 - Voltar");

        int opcaoCadastro = int.Parse(Console.ReadLine());

        switch (opcaoCadastro)
        {
            case 1:
                if(usuario == "admin")
                {
                    CadastroUsuario(usuario);
                }
                else
                {
                    Console.WriteLine("Somente o administrador do sistema pode cadastrar um usuário novo!");
                    Console.ReadKey();
                    MenuPrincipal(usuario);
                }
                
                break;

            case 2:
                CadastroProduto(usuario);
                break;
            case 3:
                MenuPrincipal(usuario);
                break;
        }
    }
    public static void CadastroProduto(string usuario)
    {
        Console.Clear();
        Produto produto = new Produto();

        Console.WriteLine("Insira o código do produto:");
        produto.Id = int.Parse(Console.ReadLine());

        Console.WriteLine("Insira o nome do produto:");
        produto.Nome = Console.ReadLine();

        Console.WriteLine("Insira o preço do produto:");
        produto.Preco = float.Parse(Console.ReadLine());

        Console.WriteLine("Insira a descrição do produto:");
        produto.Descricao = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Produtos";
        string arquivo = "Produtos.txt";

        string caminho = Path.Combine(diretorio, arquivo);

        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }

            string dados = $"{produto.Id}\n{produto.Nome}\n{produto.Preco}\n{produto.Descricao}\n";
            File.AppendAllText(caminho, dados);

            Console.WriteLine("Seu produto foi cadastrado com sucesso! Verifique!\n");
            Console.WriteLine("Você será encaminhado de volta para o menu principal");
            Console.ReadKey();
            MenuPrincipal(usuario);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuario);
        }
    }
    public static void CadastroUsuario(string usuarioLogado)
    {
        Console.Clear();
        Usuario usuario = new Usuario();

        Console.WriteLine("Insira o login do usuário:");
        usuario.Login = Console.ReadLine();

        Console.WriteLine("Insira a senha do usuário:");
        usuario.Senha = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";

        string caminho = Path.Combine(diretorio, arquivo);

        try
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
                Console.WriteLine("Diretório criado com sucesso!");
            }

            if (!File.Exists(caminho))
            {
                using (File.Create(caminho)) { }

                Console.WriteLine("Arquivo criado com sucesso!");
            }

            string dados = $"{usuario.Login}\n{usuario.Senha}\n";
            File.AppendAllText(caminho, dados);

            Console.WriteLine("Novo usuário cadastrado com sucesso! Verifique!");
            Console.WriteLine("Você será encaminhado para o menu principal, pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
            Console.ReadKey();
            MenuPrincipal(usuarioLogado);
        }
    }
    #endregion

    #region Vendas
    public static void MenuVendas(string usuario)
    {
        Console.Clear();
        Console.WriteLine("1 - Vender");
        Console.WriteLine("2 - Relatório de vendas");
        Console.WriteLine("3 - Voltar");

        int opcaoVendas = int.Parse(Console.ReadLine());

        switch (opcaoVendas)
        {
            case 1:
                Vender(usuario);
                break;

            case 2:
                if(usuario == "admin")
                {
                    RelatorioVendas(usuario);
                }
                else
                {
                    Console.WriteLine("Somente o administrador do sistema pode ter acesso a aba de relatórios!");
                    Console.ReadKey();
                    MenuPrincipal(usuario);
                }
                
                break;
            case 3:
                MenuPrincipal(usuario);
                break;
        }
    }
    public static void Vender(string usuario)
    {
        string diretorio = @"C:\SistemaDeVendas\Produtos";
        string arquivo = "Produtos.txt";

        string caminho = Path.Combine(diretorio, arquivo);

        string[] produtos = File.ReadAllLines(caminho);

        var checaArquivo = new FileInfo(caminho);

        if (checaArquivo.Length == 0)
        {
            Console.WriteLine("Não existem produtos cadastrados para venda! Você será encaminhado ao menu principal!");
            Console.ReadKey();
            Login();
        }
        else
        {
            int contador = 1, contCod = 1, contNome = 2,contPre= 3, contDesc = 4;
            string codigo = "", nome = "", preco = "", descricao = "", contCodAux = "";
            string[] produtosLidos = new string[99];
            Console.Clear();
            Console.WriteLine("Selecione o código do produto:\n");

            foreach (string produto in produtos)
            {
                if(contador == 1)
                {
                    codigo = produto;
                }
                else
                {
                    if(contador == 2)
                    {
                        nome = produto;
                    }
                    else
                    {
                        if(contador == 3)
                        {
                            preco = produto;
                        }
                        else
                        {
                            descricao = produto;

                            Console.WriteLine($"Código: {codigo} - {nome}\nPreço = R${preco}\nDescrição: {descricao}\n");
                            
                            produtosLidos[contCod] = codigo;
                            produtosLidos[contNome] = nome;
                            produtosLidos[contPre] = preco;
                            produtosLidos[contDesc] = descricao;
                            if(contCodAux == codigo)
                            {
                                break;
                                Console.ReadKey();
                            }
                            contCod += 4;
                            contNome += 4;
                            contPre += 4;
                            contDesc += 4;

                            contador = 1;
                        }
                    }
                }
                contador++;
            }          
            string opcaoProduto = (Console.ReadLine());
            int posicaoVetorCod = 1;
            for(int i = 0; i <2; i++)
            {
                if(opcaoProduto == produtosLidos[posicaoVetorCod])
                {
                    EfetuarVenda(produtosLidos[posicaoVetorCod], produtosLidos[posicaoVetorCod + 1], produtosLidos[posicaoVetorCod + 2], produtosLidos[posicaoVetorCod + 3], usuario);                   
                }
                else
                {
                    posicaoVetorCod += 4;
                }
                i++;
            }

        }
    }
    public static void RelatorioVendas(string usuario)
    {
        DateOnly dataHoje = DateOnly.FromDateTime(DateTime.Now);
        Console.Clear();
        Console.WriteLine("Escolha o tipo de relatório a ser gerado:");
        Console.WriteLine("1 - Relatório geral de vendas");
        Console.WriteLine($"2 - Relatório de vendas do dia de hoje: {dataHoje}");
        Console.WriteLine("3 - Voltar");

        int opcaoRelatorio = int.Parse(Console.ReadLine());

        switch (opcaoRelatorio)
        {
            case 1:
                RelatorioGeral(usuario);
                break;
            case 2:
                RelatorioDiario(usuario);
                break;  
            case 3:
                MenuPrincipal(usuario);
                break;
        }

    }
    public static void RelatorioGeral(string usuario)
    {

    }
    public static void RelatorioDiario(string usuario)
    {

    }
    public static void EfetuarVenda(string codigo, string nome, string preco, string descricao, string usuario)
    {
        Console.Clear();
        Venda venda = new Venda();

        Console.WriteLine("Informe o nome do cliente:");
        venda.Cliente = Console.ReadLine();

        Console.WriteLine("Escolha uma opção de pagamento:");

        foreach (Venda.Pagamento opcao in Enum.GetValues(typeof(Venda.Pagamento)))
        {
            Console.WriteLine($"{(int)opcao}. {opcao}");
        }

        Console.Write("Digite o número da opção desejada: ");

        string pagamento = Console.ReadLine();

        if (Enum.TryParse(pagamento, out Venda.Pagamento escolha))
        {
            Console.WriteLine($"Você escolheu: {escolha}");
        }
        else
        {
            Console.WriteLine("Opção inválida! Tente novamente!");
            SelecionaPagamento();
        }

        venda.Data = DateOnly.FromDateTime(DateTime.Now);

        Console.WriteLine("Deseja realizar alguma nota sobre a compra?");
        Console.WriteLine("1 - Sim");
        Console.WriteLine("2 - Não");
        int anotar = int.Parse(Console.ReadLine());

        if(anotar == 1)
        {
            Console.WriteLine("Digite sua anotação:");
            venda.Anotacao = Console.ReadLine();
        }
        else
        {
            venda.Anotacao = "Sem anotações";
        }

        Console.WriteLine("Deseja confirmar a venda?");
        Console.WriteLine("1 - Confirmar");
        Console.WriteLine("2 - Cancelar");

        int confCompra = int.Parse(Console.ReadLine());

        if(confCompra == 1)
        {
            string diretorio = @"C:\SistemaDeVendas\Vendas";
            string arquivo = "Vendas.txt";

            string caminho = Path.Combine(diretorio, arquivo);

            try
            {
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                    Console.WriteLine("Diretório criado com sucesso!");
                }

                if (!File.Exists(caminho))
                {
                    using (File.Create(caminho)) { }

                    Console.WriteLine("Arquivo criado com sucesso!");
                }

                string dados = $"Cliente: {venda.Cliente}\nPagamento:{escolha}\nData: {venda.Data}\nAnotações: {venda.Anotacao}\n" +
                    $"Produto: {codigo} - {nome}\nPreço: {preco}\nDescrição:{descricao}\nVenda feita por {usuario}";

                File.AppendAllText(caminho, dados);

                Console.WriteLine("Compra registrada! Verifique!");
                Console.WriteLine("Você será encaminhado para o menu principal, pressione qualquer tecla para continuar");
                Console.ReadKey();
                MenuPrincipal(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                Console.WriteLine("Apesar do erro, você será encaminhado para o menu principal, verifique o possível erro! Pressione qualquer tecla para continuar");
                Console.ReadKey();
                MenuPrincipal(usuario);
            }
        }
        else
        {
            Console.WriteLine("Compra cancelada! Você será encaminhado de volta ao menu principal");
            Console.ReadKey();
            MenuPrincipal(usuario);
        }

    }
    public static void SelecionaPagamento()
    {
        Console.WriteLine("Escolha uma opção de pagamento:");

        foreach (Venda.Pagamento opcao in Enum.GetValues(typeof(Venda.Pagamento)))
        {
            Console.WriteLine($"{(int)opcao}. {opcao}");
        }

        Console.Write("Digite o número da opção desejada: ");

        string pagamento = Console.ReadLine();

        if (Enum.TryParse(pagamento, out Venda.Pagamento escolha))
        {
            Console.WriteLine($"Você escolheu: {escolha}");
        }
        else
        {
            Console.WriteLine("Opção inválida! Tente novamente!");
            SelecionaPagamento();
        }
    }
    #endregion

    #region Login
    public static void Login()
    {
        Console.Clear();
        Console.WriteLine("Vendas de Cantina UnoChapeco");
        Console.WriteLine("Selecione a opção de login:");
        Console.WriteLine("1 - Usuário administrador");
        Console.WriteLine("2 - Usuário");

        int opcaoLoginUsu =  int.Parse(Console.ReadLine());

        switch (opcaoLoginUsu)
        {
            case 1:
                LoginAdmin();
                break;

            case 2:
                LoginUsuario();
                break;
        }
    }

    public static void MenuPrincipal(string usuario) 
    {
        Console.Clear();
        Console.WriteLine($"Bem vindo ao sistema de vendas {usuario}!");
        Console.WriteLine("1 - Cadastros");
        Console.WriteLine("2 - Vendas");

        int opcaoMenu = int.Parse(Console.ReadLine());

        switch (opcaoMenu)
        {
            case 1:
                Console.WriteLine();
                MenuCadastros(usuario);
                break;

            case 2:
                MenuVendas(usuario);
                break;
        }
    }

    public static void LoginAdmin()
    {
        Console.Clear();
        Console.WriteLine("Login: ");
        string loginUsuAdm = Console.ReadLine();
        Console.WriteLine("Senha: ");
        string senhaUsuAdm = Console.ReadLine();

        if (loginUsuAdm != "admin" && senhaUsuAdm != "adm123")
        {
            do
            {
                Console.WriteLine("Senha ou login incorretos! Tente novamente!");
                Console.WriteLine("Vendas de Cantina UnoChapeco");
                Console.WriteLine("Login: ");
                loginUsuAdm = Console.ReadLine();
                Console.WriteLine("Senha: ");
                senhaUsuAdm = Console.ReadLine();

                if (loginUsuAdm == "admin" && senhaUsuAdm == "adm123")
                {
                    Console.WriteLine("Acesso liberado por login e senha");
                    Console.ReadKey();
                    break;
                }

            } while (loginUsuAdm != "admin" && senhaUsuAdm != "adm123");
        }
        else
        {
            Console.WriteLine("Acesso (Administrador) liberado por login e senha");
            Console.ReadKey();
            MenuPrincipal(loginUsuAdm);
        }
        Console.Clear();
    }

    public static void LoginUsuario()
    {
        Console.Clear();
        Console.WriteLine("Login: ");
        string loginUsu = Console.ReadLine();
        Console.WriteLine("Senha: ");
        string senhaUsu = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";
        string caminho = Path.Combine(diretorio, arquivo);

        string[] usuarios = File.ReadAllLines(caminho);

        var checaArquivo = new FileInfo(caminho);

        if(checaArquivo.Length == 0)
        {
            Console.WriteLine("Não existem usuários cadastrados!");
            Console.ReadKey();
            Login();
        }
        else
        {
            int contador = 1;
            string login = "", senha = "";

            foreach (string usuario in usuarios)
            {
                if (contador % 2 == 0)
                {
                    senha = usuario.Trim();

                    if (login == loginUsu && senha == senhaUsu)
                    {
                        Console.WriteLine($"Sistema acessado pelo usuário {login}");
                        Console.ReadKey();
                        MenuPrincipal(login);
                    }
                }
                else
                {
                    login = usuario.Trim();
                }
                contador++;
            }
        }

        Console.WriteLine("Senha ou login inválidos! Deseja voltar a tela de login?");
        Console.WriteLine("1 - Sim");
        Console.WriteLine("2 - Tentar novamente");
        int opcaoLogin = int.Parse(Console.ReadLine());

        if(opcaoLogin == 1)
        {
            Login();
        }
        else
        {
            NovaTentativa();
        }
    }

    public static void NovaTentativa()
    {
        Console.Clear();
        Console.WriteLine("Senha ou login incorretos, tente novamente!");
        Console.WriteLine("Login: ");
        string loginUsu = Console.ReadLine();
        Console.WriteLine("Senha: ");
        string senhaUsu = Console.ReadLine();

        string diretorio = @"C:\SistemaDeVendas\Usuarios";
        string arquivo = "Usuarios.txt";
        string caminho = Path.Combine(diretorio, arquivo);

        string[] usuarios = File.ReadAllLines(caminho);

        var checaArquivo = new FileInfo(caminho);

        if (checaArquivo.Length == 0)
        {
            Console.WriteLine("Não existem usuários cadastrados!");
            Console.ReadKey();
            Login();
        }
        else
        {
            int contador = 1;
            string login = "", senha = "";

            foreach (string usuario in usuarios)
            {
                if (contador % 2 == 0)
                {
                    senha = usuario.Trim();

                    if (login == loginUsu && senha == senhaUsu)
                    {
                        Console.WriteLine($"Sistema acessado pelo usuário {login}");
                        Console.ReadKey();
                        MenuPrincipal(login);
                    }
                }
                else
                {
                    login = usuario.Trim();
                }
                contador++;
            }
        }
        Console.WriteLine("Senha ou login inválidos! Deseja voltar a tela de login?");
        Console.WriteLine("1 - Sim");
        Console.WriteLine("2 - Tentar novamente");
        int opcaoLogin = int.Parse(Console.ReadLine());

        if (opcaoLogin == 1)
        {
            Login();
        }
        else
        {
            NovaTentativa();
        }
    }
    #endregion
}