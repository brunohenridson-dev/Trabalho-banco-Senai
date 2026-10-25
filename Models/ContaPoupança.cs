using System;

namespace MiniBanco.ConsoleApp.Models
{
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

        public override void GerarRelatorio()
        {
            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine("[2/2] PROCESSANDO CONTA POUPANÇA");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("> DADOS DA CONTA:");
            Console.WriteLine("  - Tipo: Conta Poupança");
            Console.WriteLine($"  - Titular: {titular} | Conta: {numero}");
            Console.WriteLine($"  - Atributo Específico: Rendimento: {TaxaRendimentoMensal}% ao mês | Aniversário: dia {DiaAniversario}");
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
            if (maxPts == 0) maxPts = 30;

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
                        Console.WriteLine("  🔴 Itens Críticos (Tipo \"Tarifa\"): cobranças que exigem revisão.");
                        temTarifas = true;
                    }
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Revisar cobranças.");
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
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Acompanhar saques na poupança.");
                }
            }

            if (!temTarifas && !temSaques)
            {
                Console.WriteLine("  🟢 Nenhuma pendência identificada. Oferecer produtos de investimento ao cliente.");
            }
        }
    }
}
