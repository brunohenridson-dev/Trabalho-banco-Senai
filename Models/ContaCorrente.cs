using System;

namespace MiniBanco.ConsoleApp.Models
{
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

        public override void GerarRelatorio()
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("           MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS             ");
            Console.WriteLine("==================================================================");
            Console.WriteLine("\n[1/2] PROCESSANDO CONTA CORRENTE");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("> DADOS DA CONTA:");
            Console.WriteLine("  - Tipo: Conta Corrente");
            Console.WriteLine($"  - Titular: {titular} | Conta: {numero}");
            Console.WriteLine($"  - Atributo Específico: Limite Cheque Especial: R$ {LimiteChequeEspecial:N2}");
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
            if (maxPts == 0) maxPts = 50;

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
                        Console.WriteLine("  🔴 Itens Críticos (Tipo \"Tarifa\"): cobranças que exigem revisão ou negociação.");
                        temTarifas = true;
                    }
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Avaliar isenção ou migração para pacote de serviços.");
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
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Conta utilizando limite ou alto volume de saques. Contatar cliente.");
                }
            }

            if (!temTarifas && !temSaques)
            {
                Console.WriteLine("  🟢 Nenhuma pendência crítica identificada.");
            }
        }
    }
}
