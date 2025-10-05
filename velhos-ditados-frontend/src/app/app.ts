import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AleatorioComponent } from './components/aleatorio.component';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [AleatorioComponent, RouterOutlet, HttpClientModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css'] // corrigido: styleUrls no plural
})
export class App {
  protected readonly title = signal('velhos-ditados-frontend');
}
