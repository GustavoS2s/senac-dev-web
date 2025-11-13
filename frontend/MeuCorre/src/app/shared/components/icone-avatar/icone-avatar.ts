import { Component, Input, input } from '@angular/core';

@Component({
  selector: 'app-icone-avatar',
  imports: [],
  templateUrl: './icone-avatar.html',
  styleUrl: './icone-avatar.css',
})
export class IconeAvatar {
  @Input() cor: string = '#00000';
  @Input() icone: string = 'ri-user-line';
}
