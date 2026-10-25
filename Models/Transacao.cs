using System;

namespace MiniBanco.ConsoleApp.Models
{
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
}
