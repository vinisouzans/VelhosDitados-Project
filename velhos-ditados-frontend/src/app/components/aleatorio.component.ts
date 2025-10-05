import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DitadosService, Ditado } from '../services/ditados.service';

@Component({
  selector: 'app-aleatorio',
  standalone: true,
  imports: [CommonModule], // ✅ resolve o erro do *ngIf
  templateUrl: './aleatorio.component.html',
  styleUrls: ['./aleatorio.component.css']
})
export class AleatorioComponent {
  ditadoAtual?: Ditado;

  constructor(private ditadosService: DitadosService) {}

  ngOnInit(): void {
    this.carregarDitado();
  }

  carregarDitado() {
    this.ditadosService.getAleatorio().subscribe(d => this.ditadoAtual = d);
  }
}