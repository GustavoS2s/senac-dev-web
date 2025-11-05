import { Routes } from '@angular/router';
import { Dashboard } from './pages/dashboard/dashboard';
import { Categoria } from './pages/categoria/categoria';

export const routes: Routes = [
  {
    path: '',
    component: Dashboard,
  },

  {
    path: 'config/categorias',
    component: Categoria
  }

];
