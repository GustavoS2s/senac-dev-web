import { Component } from '@angular/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { CategoriaModel } from './models/categoria.model';

@Component({
  selector: 'app-categorias',
  imports: [NgbNavModule],
  templateUrl: './categoria.html',
  styleUrl: './categoria.css',
})
export class Categorias {
  active = 1;

  categorias_receitas: CategoriaModel[] = [
    {
      id: '1', 
      nome: 'Salário', 
      descricao: 'Recebimento mensal', 
      cor: '#28a745', 
      icone: '', 
      ativo: true
    },
    {
      id: '2',
      nome: 'Freelance',
      descricao: 'Trabalhos avulsos',
      cor: '#17a2b8',
      icone: '',
      ativo: true
    },
    {
      id: '3',
      nome: 'Investimentos',
      descricao: 'Rendimentos de investimentos',
      cor: '#ffc107',
      icone: '',
      ativo: true
    },
  ];

  categorias_despesas: CategoriaModel[] = [
    {
      id: '1',
      nome: 'Alimentação',
      descricao: 'Alimentação',
      cor: '#dc3545',
      icone: 'fas fa-utensils',
      ativo: true
    },
    {
      id: '2',
      nome: 'Transporte',
      descricao: 'Despesas com transporte',
      cor: '#fd7e14',
      icone: 'fas fa-bus',
      ativo: true
    },
    {
      id: '3',
      nome: 'Lazer',
      descricao: 'Despesas com lazer',
      cor: '#ffc107',
      icone: 'fas fa-film' ,
      ativo: true
    },
  ];
}