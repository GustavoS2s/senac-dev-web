

import { Component, signal, computed } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css' 
})
export class App {
  protected readonly title = signal('MeuCorre - Finanças Pessoais');
  nome = signal<string>('Gustavo Alves');
  alternarNome = signal<number>(0);


  quantidade300 = signal<number>(1);

  quantidade500 = signal<number>(1);

  readonly preco300 = 20;
  readonly preco500 = 30;

  total = computed(() => {
    const subtotal300 = this.quantidade300() * this.preco300;
    const subtotal500 = this.quantidade500() * this.preco500;
    return subtotal300 + subtotal500;
  });

  alternar(opcao: number) {
    this.alternarNome.set(opcao);
    if (opcao === 1) {
      this.nome.set('Gustavo');
    } else {
      this.nome.set('Gustavo Alves');
    }
  }

  atualizarQuantidade300(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const valor = parseInt(inputElement.value, 10) || 0;
    this.quantidade300.set(Math.max(0, valor));
  }


  atualizarQuantidade500(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const valor = parseInt(inputElement.value, 10) || 0;
    this.quantidade500.set(Math.max(0, valor));
  }
}