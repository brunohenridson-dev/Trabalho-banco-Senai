using System;
using System.Collections.Generic;
using MiniBanco.ConsoleApp.Models;

namespace MiniBanco.ConsoleApp
{
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

                    Console.Write("\nDigite o nome do titular da conta: ");
                    string nomeTitular = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(nomeTitular))
                    {
                        nomeTitular = "Titular Padrão";
                    }

                    abrirConta = false;

                    Console.WriteLine($"\nSucesso! Conta selecionada: {tipoSelecionado}");
                    Console.WriteLine("O número da sua Conta é: " + numeroAleatorio);

                    Conta novaConta;
                    if (tipoSelecionado == TipoConta.ContaCorrente)
                    {
                        novaConta = new ContaCorrente(numeroAleatorio, saldoAtual, nomeTitular, 500m);
                    }
                    else if (tipoSelecionado == TipoConta.ContaPoupanca)
                    {
                        novaConta = new ContaPoupanca(numeroAleatorio, saldoAtual, nomeTitular, 10, 0.5);
                    }
                    else
                    {
                        novaConta = new ContaSalario(numeroAleatorio, saldoAtual, nomeTitular, "Empresa X", 2);
                    }

                    listaContas.Add(novaConta);

                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
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
                Conta contaEncontrada = null;
                foreach (Conta c in listaContas)
                {
                    if (c.numero == idconta)
                    {
                        contaEncontrada = c;
                        break;
                    }
                }

                if (contaEncontrada != null)
                {
                    while (continuar)
                    {
                        Console.Clear();
                        Console.WriteLine($"=== Conta Atual: {tipoSelecionado} | Titular: {contaEncontrada.titular} | Saldo: R$ {contaEncontrada.saldo} | Pontos: {contaEncontrada.pontos} ===");
                        
                        double indice = contaEncontrada.CalcularIndiceSaude();
                        Console.WriteLine($"Índice de Saúde: {indice:F1}%");

                        Console.WriteLine("O que você quer fazer?");
                        Console.WriteLine("1 - Depositar");
                        Console.WriteLine("2 - Sacar");
                        Console.WriteLine("3 - Sair");
                        Console.WriteLine("4 - Ver Transações");
                        Console.WriteLine("5 - Exibir Relatório Completo");
                        Console.Write("Escolha uma opção: ");

                        string escolha = Console.ReadLine();

                        switch (escolha)
                        {
                            case "1":
                                Console.Write("Qual o tipo da transação (ex: Depósito, Tarifa)? ");
                                string tipo = Console.ReadLine();

                                Console.Write("Qual o valor que você quer depositar/cobrar? ");
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

                            case "5":
                                Console.Clear();
                                contaEncontrada.GerarRelatorio();
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

    public enum TipoConta
    {
        ContaCorrente = 1,
        ContaPoupanca = 2,
        ContaSaldo = 3
    }
}
