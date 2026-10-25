using System;

namespace MiniBanco.ConsoleApp.Models
{
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

        public override void GerarRelatorio()
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine("           MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS             ");
            Console.WriteLine("==================================================================");
            Console.WriteLine("\n[1/3] PROCESSANDO CONTA SALÁRIO");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("> DADOS DA CONTA:");
            Console.WriteLine("  - Tipo: Conta Salário");
            Console.WriteLine($"  - Titular: {titular} | Conta: {numero}");
            Console.WriteLine($"  - Atributo Específico: Empregador: {Empregador} | Limite de Saques: {LimiteSaquesMensais}");
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
                        Console.WriteLine("  🔴 Itens Críticos (Tipo \"Tarifa\"): cobranças que exigem revisão ou negociação.");
                        temTarifas = true;
                    }
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Negociar tarifas cobradas.");
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
                    Console.WriteLine($"    - [{t.descricao}] R$ {t.valor:N2}: Monitorar limite de saques mensais gratuitos.");
                }
            }

            if (!temTarifas && !temSaques)
            {
                Console.WriteLine("  🟢 Conta Salário regular. Monitorar limites mensais de saque.");
            }
        }
    }
}
