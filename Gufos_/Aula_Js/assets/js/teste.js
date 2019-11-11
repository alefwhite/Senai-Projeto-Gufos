var campo = document.getElementById('nome-tipo-evento');
var campo2 = document.getElementById('nome-tipo-codigo');

function inserir() {
  let txt = campo.value;

  if(txt.length > 3){
    campo.className = 'input2';

  } else {
    campo.className = 'input1';
  }

}

function enviar() {
  // previnir comportamento padrão da página
  event.preventDefault();

  // acessar através de variavel o 'tbody'
  var tabela = document.getElementById('tabela-lista-corpo');

  // crio a linha da tabela
  var linha = document.createElement('tr');

  // crio a coluna da tabela
  var colunaId = document.createElement('td');
  var colunaEvento = document.createElement('td');

  // Colocando o valor do input dentro
  colunaId.textContent = campo2.value;
  colunaEvento.textContent = campo.value;

  // Adicionando as colunas na linha, conteudo filho no pai
  linha.appendChild(colunaId);
  linha.appendChild(colunaEvento);

  // Icluindo as linha no corpo
  tabela.appendChild(linha);

  // Limpar os campos
  campo.value = "";
  campo2.value = "";
  campo.className = "";
  
  // Outra fora de limpar os campos
  // var form = document.getElementsByTagName('form')[0];
  // form.reset();

  campo2.focus();  

}





// // Cria um elemento 'p' dentro da div 'teste'
// function enviar() {
//   event.preventDefault(); 
//   var divTeste = document.getElementById('teste');
//   var p = document.createElement('p');
//   divTeste.appendChild(p);
//   p.textContent = campo.value;      
// }


// document.getElementsByTagName('p')[0].innerHTML = "Olá Mundo";

//document.getElementsByClassName('pp')[0].innerHTML = "Olá Mundo";

//document.getElementById('rodape').innerHTML = "Olá";

//document.querySelector('#rodape').innerHTML = "Olá";

