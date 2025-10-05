import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Ditado {
  id: string;
  descricao: string;
}

@Injectable({
  providedIn: 'root'
})
export class DitadosService {

  private apiUrl = 'http://localhost:7001/api/ditados'; // sua URL da API

  constructor(private http: HttpClient) { }

  getAleatorio(): Observable<Ditado> {
    return this.http.get<Ditado>(`${this.apiUrl}/aleatorio`);
  }
}