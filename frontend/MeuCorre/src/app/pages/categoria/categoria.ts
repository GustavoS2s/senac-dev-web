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
}
