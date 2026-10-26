ele começa num while onde ele da o tipo de conta a escolher e da um número aleatório para sua conta,ela pergunta o nome do titular da conta,ele usa o int try parse
pra não dar erro quando  o usuário digita algo diferente do número,o código declara um número aleatório para ser usado como número da conta depois,ele declara o saldo
padrão da conta e declara um bool continuar que será usado no próximo looping. quando o úsuario solicita uma conta do tipo de ENUM, o código cria um objeto com as
informações da conta e adiciona a lista,depois ele pergunta o número da conta e pesquisa sobre ela,depois ele usa Case e faz as escolha para depósito,saque,sair
ver transações e exibir relatório completo. o case da escolha ele pergunta e coloca na variável tipo,ele coloca na variável leia o valor que você quer depositar e usa
um decimal try parse e força que ele seja maior que zero ele adiciona a conta encontrada as minhas transações e adiciona o depósito e diz as coisas feitas ao usuário.
o case 2 pergunta o tipo de transação e o valor do saque e usa o decimal try parse para ter um valor correto,depois pergunta a descrição e usa um try-catch para que o 
saque tenha um valor válido. o caso três é para quando a pessoa sair ela manda ter uma boa semana. o caso 4 da uma lista de transações e o 5 gera um relatório,caso o 
usuário faça uma ação inválida ele vai para o default. temos os métodos sacar e depositar valores cada um com os seus pontos. o código tem conta corrente herdando de
conta,conta poupança herdando de conta e conta salário herdando de conta com o saque realizado no mês de modo privado,a conta salário modifica o sacar da classe conta
comum,adicionando tarifa e contando os saques realizados no mês,se os saques realizados no mês forem maiores que o limite de saques mensais ele adiciona uma multa.
depois o código vai e faz classes contas com número,saldo,titular e pontos e não deixa sacar valores com saldo insuficiente
nem deixa o saldo ser controlado por outras classe porque é protected set,ele tambem faz uma lista de transação e métodos de sacar de depositar de aplicarfechamento
de calcularindice e etc,todos podendo serem modificados por outras classes porque é virtual,ele tambem gera um relatório com diversas outras informações. ele tambem tem 
indices com a classificação de contas. tem um if para dizer se a conta esta saudável,em atenção ou em risco. ele persegue em busca de transação ou saques com o foreach
ele tambem usa foreach para descobrir transações,quando o código não acha tarifas e saques pendentes ele joga que não tem pendencias identificadas. ele tambem tem uma 
classe transação que força o valor ser maior que zero e tem a descrição,tipos e valores. o relatório conta com a classificação da conta,com a pontuação atingida comparada
ao máximo,com a quantidade de movimentações registradas e tambem mostra um relatório de pendencias quando gerado,ele usa um foreach para mostrar as minhas transações e 
sua descrição,pontuação além do seu índice de saúde. a estrutura do foreach tem StringComparison.OrdinalIgnoreCase para comparar ignorando qualquer diferença de String que 
pode ir ao código,minha transação checa para se tem saques caso tenha ela joga os saques para o usuário. o código usa construtores explícitos nas classes como foi pedido
pelo trabalho,além de ter uma lista de contas para mostrar ao usuário,tambem  usa $ como método de contatenação ao invez de " " + " " o enum do código é usado para
dar o tipo da conta criada
