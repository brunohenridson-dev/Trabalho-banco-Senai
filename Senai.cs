using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    { 
        int numeroAleatorio = Random.Shared.Next(1, 1001);
        decimal saldoAtual = 100;
        bool continuar = true;

        List<Conta> listaContas = new List<Conta>();
        bool abrirConta = true;

        TipoConta tipoSelecionado = default;

        while (abrirConta)
        {
            Console.Clear();
            Console.WriteLine("Escolha o tipo de conta ===");
            Console.WriteLine("1 - Conta Corrente");
            Console.WriteLine("2 - Conta Poupança");
            Console.WriteLine("3 - Conta Saldo");
            Console.Write("Digite o número da opção desejada: ");

            string opcaoDe = Console.ReadLine();

            if (int.TryParse(opcaoDe, out int opcaoSinha) && Enum.IsDefined(typeof(TipoConta), opcaoSinha))
            {
                tipoSelecionado = (TipoConta)opcaoSinha;
                abrirConta = false;

                Console.WriteLine($"\nSucesso! Conta selecionada: {tipoSelecionado}");
                Console.WriteLine("O número da sua Conta é: " + numeroAleatorio);

                Conta novaConta;
                if (tipoSelecionado == TipoConta.ContaCorrente)
                {
                    novaConta = new ContaCorrente(numeroAleatorio, saldoAtual, "Titular", 200m);
                }
                else if (tipoSelecionado == TipoConta.ContaPoupanca)
                {
                    novaConta = new ContaPoupanca(numeroAleatorio, saldoAtual, "Titular", 10, 1.0);
                }
                else
                {
                    novaConta = new ContaSalario(numeroAleatorio, saldoAtual, "Titular", "Empresa X", 2);
                }

                listaContas.Add(novaConta);

                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nOpção inválida! Pressione qualquer tecla para tentar novamente.");
                Console.ReadKey();
            }
        }

        Console.Clear();
        Console.WriteLine("Qual o número da sua conta?");
        
        if (int.TryParse(Console.ReadLine(), out int idconta))
        {
            Conta contaEncontrada = listaContas.Find(c => c.numero == idconta);

            if (contaEncontrada != null)
            {
                while (continuar)
                {
                    Console.Clear();
                    Console.WriteLine($"=== Conta Atual: {tipoSelecionado} | Saldo: R$ {contaEncontrada.saldo} | Pontos: {contaEncontrada.pontos} ===");
                    
                    double indice = contaEncontrada.CalcularIndiceSaude();
                    Console.WriteLine($"Índice de Saúde: {indice:F2}%");

                    Console.WriteLine("O que você quer fazer?");
                    Console.WriteLine("1 - Depositar");
                    Console.WriteLine("2 - Sacar");
                    Console.WriteLine("3 - Sair");
                    Console.WriteLine("4 - Ver Transações");
                    Console.Write("Escolha uma opção: ");

                    string escolha = Console.ReadLine();

                    switch (escolha)
                    {
                        case "1":
                            Console.Write("Qual o tipo da transação (ex: Depósito, Tarifa)? ");
                            string tipo = Console.ReadLine();

                            Console.Write("Qual o valor que você quer depositar? ");
                            string leia = Console.ReadLine();

                            bool value = decimal.TryParse(leia, out decimal valor);

                            if (!value || valor <= 0)
                            {
                                Console.WriteLine("Erro: digite um valor válido maior que zero. Pressione qualquer tecla.");
                                Console.ReadKey();
                                break;
                            }

                            Console.Write("Qual a descrição do que você quer fazer? ");
                            string descricao = Console.ReadLine();

                            Transacao minhaTransacao = new Transacao(descricao, tipo, valor);

                            contaEncontrada.minhasTransacoes.Add(minhaTransacao);
                            contaEncontrada.Depositar(valor, descricao);

                            Console.WriteLine($"\nTransação registrada! Tipo: {minhaTransacao.tipo}, Valor: R$ {minhaTransacao.valor}, Descrição: {minhaTransacao.descricao}");
                            Console.WriteLine($"Novo saldo atual: R$ {contaEncontrada.saldo}");
                            Console.WriteLine("\nPressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;

                        case "2":
                            Console.Write("Qual o tipo da transação (ex: Saque)? ");
                            string tipoSaque = Console.ReadLine();

                            Console.Write("Qual o valor que você quer sacar? ");
                            string leiaSaque = Console.ReadLine();

                            bool conversaoSucesso = decimal.TryParse(leiaSaque, out decimal saque);

                            if (!conversaoSucesso)
                            {
                                Console.WriteLine("Erro: você digitou um valor inválido. Use somente números.");
                                Console.ReadKey();
                                break;
                            }

                            Console.Write("Qual a descrição do que você quer fazer? ");
                            string descricaoSaque = Console.ReadLine();

                            try
                            {
                                contaEncontrada.Sacar(saque);

                                Transacao transacaoSaque = new Transacao(descricaoSaque, tipoSaque, saque);
                                contaEncontrada.minhasTransacoes.Add(transacaoSaque);

                                Console.WriteLine("\nSaque registrado com sucesso!");
                                Console.WriteLine($"Novo saldo atual: R$ {contaEncontrada.saldo}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nErro ao sacar: {ex.Message}");
                            }
                            
                            Console.WriteLine("\nPressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;

                        case "3":
                            Console.WriteLine("Tenha uma boa semana!");
                            continuar = false;
                            break;

                        case "4":
                            Console.Clear();
                            if (contaEncontrada.minhasTransacoes.Count == 0)
                            {
                                Console.WriteLine("Nenhuma transação registrada.");
                            }
                            else
                            {
                                Console.WriteLine("=== Lista de Transações ===");

                                foreach (Transacao transacao in contaEncontrada.minhasTransacoes)
                                {
                                    Console.WriteLine($"Tipo: {transacao.tipo} | Valor: R$ {transacao.valor} | Descrição: {transacao.descricao}");
                                }
                            }

                            Console.WriteLine($"\nSaldo atual: R$ {contaEncontrada.saldo}");
                            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                            Console.ReadKey();
                            break;

                        default:
                            Console.WriteLine("Opção desconhecida. Pressione qualquer tecla.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Você não tem uma conta com esse número.");
            }
        }
        else
        {
            Console.WriteLine("Número de conta inválido.");
        }
    }
}

public abstract class Conta
{
    public int numero { get; set; }
    public decimal saldo { get; protected set; }
    public string titular { get; set; }
    public int pontos { get; set; }
    public List<Transacao> minhasTransacoes { get; set; } = new List<Transacao>();

    public Conta(string titular, decimal saldo, int numero, int pontos)
    {
        this.numero = numero;
        this.titular = titular;
        this.saldo = saldo;
        this.pontos = pontos;
    }

    public virtual void Sacar(decimal valor)
    {
        if (valor <= 0 || valor > saldo)
        {
            throw new Exception("Saldo insuficiente.");
        }
        saldo -= valor;
        pontos += 5;
    }
    
    public virtual void Depositar(decimal valor, string descricao)
    {
        saldo += valor;
        pontos += 10;
    }

    public virtual void AplicarFechamentoMensal()
    {
        saldo -= 10;
    }

    public virtual double CalcularIndiceSaude()
    {
        if (minhasTransacoes.Count == 0)
        {
            return 0.0;
        }

        double pontuacaoMaximaPossivel = minhasTransacoes.Count * 10.0;
        double percentual = ((double)pontos / pontuacaoMaximaPossivel) * 100.0;

        if (percentual > 90.0 && percentual <= 100.0)
        {
            Console.WriteLine("sua conta está saudável");
        }
        else if (percentual > 60.0 && percentual <= 90.0)
        {
            Console.WriteLine("conta em atenção");
        }
        else
        {
            Console.WriteLine("conta em risco");
        }

        return percentual;
    }
}

public class ContaCorrente : Conta
{
    public decimal LimiteChequeEspecial { get; set; }

    public ContaCorrente(int numero, decimal saldo, string titular, decimal limiteChequeEspecial)
        : base(titular, saldo, numero, 0)
    {
        this.LimiteChequeEspecial = limiteChequeEspecial;
    }

    public override void Sacar(decimal valor)
    {
        decimal saldoTotal = saldo + LimiteChequeEspecial;

        if (valor <= 0 || valor > saldoTotal)
        {
            throw new Exception("Saldo insuficiente (considerando o limite).");
        }

        saldo -= valor;
        pontos += 5;
    }

    public override void AplicarFechamentoMensal()
    {
        saldo -= 10.00m; 
    }
}

public class ContaPoupanca : Conta
{
    public int DiaAniversario { get; set; }
    public double TaxaRendimentoMensal { get; set; }

    public ContaPoupanca(int numero, decimal saldo, string titular, int diaAniversario, double taxaRendimentoMensal)
        : base(titular, saldo, numero, 0)
    {
        this.DiaAniversario = diaAniversario;
        this.TaxaRendimentoMensal = taxaRendimentoMensal;
    }

    public override void AplicarFechamentoMensal()
    {
        decimal rendimento = saldo * (decimal)TaxaRendimentoMensal / 100m;
        saldo += rendimento; 
    }
}

public class ContaSalario : Conta
{
    public string Empregador { get; set; }
    public int LimiteSaquesMensais { get; set; }
    private int saquesRealizadosNoMes = 0;

    public ContaSalario(int numero, decimal saldo, string titular, string empregador, int limiteSaquesMensais)
        : base(titular, saldo, numero, 0)
    {
        this.Empregador = empregador;
        this.LimiteSaquesMensais = limiteSaquesMensais;
    }

    public override void Sacar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new Exception("O valor do saque deve ser maior que zero.");
        }

        decimal tarifa = 0m;
        saquesRealizadosNoMes++;

        if (saquesRealizadosNoMes > LimiteSaquesMensais)
        {
            tarifa = 5.00m; 
        }

        if (valor + tarifa > saldo)
        {
            saquesRealizadosNoMes--; 
            throw new Exception("Saldo insuficiente para realizar o saque e cobrir a tarifa.");
        }

        saldo -= (valor + tarifa); 
        pontos += 5;
    }

    public override void AplicarFechamentoMensal()
    {
        saquesRealizadosNoMes = 0; 
    }
}

public class Transacao
{
    public string descricao { get; set; }
    public string tipo { get; set; }
    public decimal valor { get; set; }

    public Transacao(string descricao, string tipo, decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor da transação deve ser maior que zero", nameof(valor));
        }

        this.descricao = descricao;
        this.tipo = tipo;
        this.valor = valor;
    }
}

public enum TipoConta
{
    ContaCorrente = 1,
    ContaPoupanca = 2,
    ContaSaldo = 3
}
