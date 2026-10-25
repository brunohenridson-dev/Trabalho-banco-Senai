using System;
using System.Collections.Generic;

namespace MiniBanco.ConsoleApp.Models
{
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
            return percentual;
        }

        public virtual void GerarRelatorio()
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("           MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS             ");
            Console.WriteLine("==================================================================");
            Console.WriteLine("\n> DADOS DA CONTA:");
            Console.WriteLine($"  - Titular: {titular} | Conta: {numero}");
            Console.WriteLine($"  - Saldo Atual: R$ {saldo:N2}");

            Console.WriteLine($"\n> MOVIMENTAÇÕES REGISTRADAS ({minhasTransacoes.Count} TRANSAÇÕES):");
            foreach (var t in minhasTransacoes)
            {
                int ptsT = t.tipo.ToLower().Contains("depósito") ? 10 : (t.tipo.ToLower().Contains("saque") ? 5 : 0);
                string icone = ptsT == 10 ? "[OK]" : (ptsT == 5 ? "[ ! ]" : "[ X ]");
                Console.WriteLine($"  {icone} {t.descricao} ------------------- {t.tipo}: R$ {t.valor:N2} ({ptsT} pts)");
            }

            double indice = CalcularIndiceSaude();
            int maxPts = minhasTransacoes.Count * 10;

            Console.WriteLine("\n> RESUMO DA PONTUAÇÃO:");
            Console.WriteLine($"  - Pontuação Atingida: {pontos} de {maxPts} pontos possíveis");
            Console.WriteLine($"  - Índice de Saúde: {indice:F1}%");

            string classificacaoFinal = "[ CONTA EM RISCO ]";
            if (indice >= 90.0)
            {
                classificacaoFinal = "[ CONTA SAUDÁVEL ]";
            }
            else if (indice >= 60.0)
            {
                classificacaoFinal = "[ CONTA EM ATENÇÃO ]";
            }

            Console.WriteLine($"  - Classificação Final: {classificacaoFinal}");
            
            Console.WriteLine("\n> RELATÓRIO DE PENDÊNCIAS E RECOMENDAÇÕES DA GERÊNCIA:");
            
            bool temTarifas = false;
            bool temSaques = false;

            foreach (var t in minhasTransacoes)
            {
                if (t.tipo.Equals("Tarifa", StringComparison.OrdinalIgnoreCase))
                {
                    if (!temTarifas)
                    {
                        Console.WriteLine("  🔴 Itens Críticos (Tipo \"Tarifa\"): cobranças que exigem revisão ou negociação com o cliente.");
                        temTarifas = true;
                    }
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Revisar pacote de tarifas com o cliente.");
                }
            }

            foreach (var t in minhasTransacoes)
            {
                if (t.tipo.Equals("Saque", StringComparison.OrdinalIgnoreCase))
                {
                    if (!temSaques)
                    {
                        Console.WriteLine("  🟡 Itens de Atenção (Tipo \"Saque\"): retiradas que exigem acompanhamento.");
                        temSaques = true;
                    }
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Acompanhar volume de retiradas.");
                }
            }

            if (!temTarifas && !temSaques)
            {
                Console.WriteLine("  🟢 Nenhuma pendência identificada. Oferecer produtos de investimento ao cliente.");
            }
        }
    }
}
