import { Component } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-categoria',
  imports: [NgbNavModule],
  templateUrl: './categoria.html',
  styleUrl: './categoria.css',
})
export class Categoria {
  active = 1;

  categoria_receitas = [
    { nome: 'Café da Manhã', imagem: 'assets/categorias/cafe-manha.jpg' },
    { nome: 'Almoço', imagem: 'assets/categorias/almoco.jpg' },
    { nome: 'Lanches', imagem: 'assets/categorias/lanches.jpg' },
    { nome: 'Jantar', imagem: 'assets/categorias/jantar.jpg' },
    { nome: 'Sobremesas', imagem: 'assets/categorias/sobremesas.jpg' },
    { nome: 'Bebidas', imagem: 'assets/categorias/bebidas.jpg' },
  ];

  categoria_despesas = [
    { nome: 'Moradia', icone: 'assets/categorias/moradia.jpg' },
    { nome: 'Alimentação', imagem: 'assets/categorias/alimentacao.jpg' },
    { nome: 'Transporte', imagem: 'assets/categorias/transporte.jpg' },
    { nome: 'Saúde', imagem: 'assets/categorias/saude.jpg' },
    { nome: 'Lazer', imagem: 'assets/categorias/lazer.jpg' },
    { nome: 'Educação', imagem: 'assets/categorias/educacao.jpg' },
  ];
}
